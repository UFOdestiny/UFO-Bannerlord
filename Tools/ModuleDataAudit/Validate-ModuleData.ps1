param(
    [Parameter(Mandatory = $true)]
    [string]$GameRoot,
    [string]$ModuleRoot = (Join-Path $PSScriptRoot '..\..\Module')
)

$ErrorActionPreference = 'Stop'
$moduleRoot = [IO.Path]::GetFullPath($ModuleRoot)
$gameRoot = [IO.Path]::GetFullPath($GameRoot)
$moduleDataRoot = Join-Path $moduleRoot 'ModuleData'
$issues = [Collections.Generic.List[string]]::new()

function Read-Xml([string]$path) {
    try {
        $document = [xml]::new()
        $document.Load($path)
        return $document
    }
    catch { $issues.Add("Invalid XML: $path ($($_.Exception.Message))"); return $null }
}

function Get-IdSet([string]$root, [string]$elementName) {
    $ids = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    Get-ChildItem -LiteralPath $root -Recurse -Filter *.xml | Where-Object { $_.FullName -notlike "$root\Languages\*" } | ForEach-Object {
        $xml = Read-Xml $_.FullName
        if ($null -ne $xml) {
            $selector = if ($elementName -eq 'WeaponDescription') { '/*/WeaponDescription' } else { "//$elementName" }
            @($xml.SelectNodes($selector)) | ForEach-Object {
                if ($_.id) { [void]$ids.Add($_.id) }
            }
        }
    }
    Write-Output -NoEnumerate $ids
}

$moduleFiles = Get-ChildItem -LiteralPath $moduleDataRoot -Recurse -Filter *.xml
$moduleXml = @{}
foreach ($file in $moduleFiles) { $moduleXml[$file.FullName] = Read-Xml $file.FullName }

foreach ($elementName in 'Item', 'CraftedItem', 'CraftingPiece', 'CraftingTemplate', 'WeaponDescription') {
    $selector = if ($elementName -eq 'WeaponDescription') { '/*/WeaponDescription[@id]' } else { "//$elementName[@id]" }
    $seen = @{}
    foreach ($pair in $moduleXml.GetEnumerator()) {
        if ($null -eq $pair.Value) { continue }
        foreach ($node in @($pair.Value.SelectNodes($selector))) {
            if ($seen.ContainsKey($node.id)) { $issues.Add("Duplicate $elementName id '$($node.id)': $($seen[$node.id]) and $($pair.Key)") }
            else { $seen[$node.id] = $pair.Key }
        }
    }
}

$subModule = Read-Xml (Join-Path $moduleRoot 'SubModule.xml')
foreach ($node in @($subModule.Module.Xmls.XmlNode)) {
    $path = Join-Path $moduleDataRoot ($node.XmlName.path + '.xml')
    if (-not (Test-Path -LiteralPath $path)) { $issues.Add("SubModule XML path is missing: $($node.XmlName.path)") }
}

# A syntactically valid module_strings.xml is ignored unless GameText is registered.
if (-not @($subModule.Module.Xmls.XmlNode | Where-Object { $_.XmlName.id -eq 'GameText' -and $_.XmlName.path -eq 'module_strings' }).Count) {
    $issues.Add('module_strings.xml must be registered as GameText in SubModule.xml')
}
$gameTexts = Read-Xml (Join-Path $moduleDataRoot 'module_strings.xml')
$textIds = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
foreach ($entry in @($gameTexts.strings.string)) {
    if (-not $textIds.Add($entry.id)) { $issues.Add("Duplicate GameText id '$($entry.id)'") }
}
foreach ($pair in $moduleXml.GetEnumerator()) {
    if ($null -eq $pair.Value) { continue }
    foreach ($template in @($pair.Value.SelectNodes('//CraftingTemplate[@id]'))) {
        if (-not $textIds.Contains("str_crafting_template.$($template.id)")) { $issues.Add("Missing crafting template GameText: $($template.id)") }
    }
    foreach ($description in @($pair.Value.SelectNodes('/*/WeaponDescription[@id]'))) {
        if (-not $textIds.Contains("str_weapon_usage.$($description.id)")) { $issues.Add("Missing weapon usage GameText: $($description.id)") }
    }
    $seenStrings = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($entry in @($pair.Value.SelectNodes('/base/strings/string'))) {
        if (-not $seenStrings.Add($entry.id)) { $issues.Add("Duplicate localized string '$($entry.id)' in $($pair.Key)") }
    }
}

$nativeData = Join-Path $gameRoot 'Modules\Native\ModuleData'
$allPieceIds = Get-IdSet $nativeData 'CraftingPiece'
$allTemplateIds = Get-IdSet $nativeData 'CraftingTemplate'
$allWeaponDescriptionIds = Get-IdSet $nativeData 'WeaponDescription'
foreach ($pair in $moduleXml.GetEnumerator()) {
    if ($null -eq $pair.Value) { continue }
    foreach ($node in @($pair.Value.SelectNodes('//CraftingPiece[@id]'))) { [void]$allPieceIds.Add($node.id) }
    foreach ($node in @($pair.Value.SelectNodes('//CraftingTemplate[@id]'))) { [void]$allTemplateIds.Add($node.id) }
    foreach ($node in @($pair.Value.SelectNodes('/*/WeaponDescription[@id]'))) { [void]$allWeaponDescriptionIds.Add($node.id) }
}

foreach ($pair in $moduleXml.GetEnumerator()) {
    if ($null -eq $pair.Value) { continue }
    foreach ($node in @($pair.Value.SelectNodes('//CraftedItem[@crafting_template]'))) {
        if (-not $allTemplateIds.Contains($node.crafting_template)) { $issues.Add("Unknown crafting template '$($node.crafting_template)' in $($pair.Key)") }
    }
    foreach ($node in @($pair.Value.SelectNodes('//Piece[@id] | //UsablePiece[@piece_id] | //AvailablePiece[@id]'))) {
        $id = if ($node.piece_id) { $node.piece_id } else { $node.id }
        if (-not $allPieceIds.Contains($id)) { $issues.Add("Unknown crafting piece '$id' in $($pair.Key)") }
    }
    foreach ($node in @($pair.Value.SelectNodes('//CraftingTemplate/WeaponDescriptions/WeaponDescription[@id]'))) {
        if (-not $allWeaponDescriptionIds.Contains($node.id)) { $issues.Add("Unknown weapon description '$($node.id)' in $($pair.Key)") }
    }
}

Get-ChildItem -LiteralPath (Join-Path $moduleDataRoot 'Languages') -Recurse -Filter language_data.xml -ErrorAction SilentlyContinue | ForEach-Object {
    $languageData = Read-Xml $_.FullName
    $directory = Split-Path (Split-Path $_.FullName)
    foreach ($languageFile in @($languageData.LanguageData.LanguageFile)) {
        if (-not (Test-Path -LiteralPath (Join-Path $directory $languageFile.xml_path))) { $issues.Add("Missing language file '$($languageFile.xml_path)' referenced by $($_.FullName)") }
    }
}

if ($issues.Count -gt 0) {
    $issues | ForEach-Object { Write-Error $_ }
    exit 1
}

Write-Host "ModuleData audit passed: $($moduleFiles.Count) XML files, all registrations and cross-references resolved."
