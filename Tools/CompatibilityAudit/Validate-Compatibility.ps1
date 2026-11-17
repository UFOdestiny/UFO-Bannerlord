param(
    [string]$GameRoot = 'E:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord',
    [string]$WorkshopRoot,
    [string]$HarmonyAssemblyPath,
    [string]$ButterLibAssemblyPath,
    [string]$UIExtenderExAssemblyPath,
    [string]$McmAssemblyPath,
    [switch]$Package,
    [switch]$UpdateReadme
)

$ErrorActionPreference = 'Stop'
$repoRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$gameRoot = [IO.Path]::GetFullPath($GameRoot)
if (-not $WorkshopRoot) { $WorkshopRoot = [IO.Path]::GetFullPath((Join-Path $gameRoot '..\..\workshop\content\261550')) }
$gameBin = Join-Path $gameRoot 'bin\Win64_Shipping_Client'
$output = Join-Path $repoRoot 'obj\compatibility'
[void][IO.Directory]::CreateDirectory($output)
$defaults = @{
    HarmonyAssemblyPath = '2859188632\bin\Win64_Shipping_Client\0Harmony.dll'
    ButterLibAssemblyPath = '2859232415\bin\Win64_Shipping_Client\Bannerlord.ButterLib.dll'
    UIExtenderExAssemblyPath = '2859222409\bin\Win64_Shipping_Client\Bannerlord.UIExtenderEx.dll'
    McmAssemblyPath = '2859238197\bin\Win64_Shipping_Client\MCMv5.dll'
}
$properties = @("-p:GameFolder=$gameRoot", "-p:WorkshopRoot=$WorkshopRoot", '-p:DeployToGameFolder=false')
$dependencies = foreach ($name in $defaults.Keys) {
    $path = Get-Variable -Name $name -ValueOnly
    if (-not $path) { $path = Join-Path $WorkshopRoot $defaults[$name] }
    if (-not (Test-Path -LiteralPath $path)) { throw "Missing dependency: $path. Supply -$name." }
    $properties += "-p:$name=$path"
    [IO.Path]::GetFullPath($path)
}
$harmonyPath = $dependencies | Where-Object { [IO.Path]::GetFileName($_) -eq '0Harmony.dll' }
$dependencyList = Join-Path $output 'dependency-paths.txt'
$dependencies | ForEach-Object { Get-ChildItem -LiteralPath (Split-Path $_) -Filter *.dll | Select-Object -ExpandProperty FullName } |
    Sort-Object -Unique | Set-Content -LiteralPath $dependencyList

function Invoke-Checked([string]$program, [string[]]$arguments, [string]$logName, [bool]$quiet = $false) {
    if ($quiet) { & $program @arguments 2>&1 | Set-Content -LiteralPath (Join-Path $output $logName) }
    else { & $program @arguments 2>&1 | Tee-Object -FilePath (Join-Path $output $logName) }
    if ($LASTEXITCODE -ne 0) { throw "$program failed ($LASTEXITCODE). See $logName." }
}

Push-Location $repoRoot
try {
    # No NuGet packages are required: all runtime references come from the game.
    # Scope the otherwise unrelated per-user NuGet configuration to this run.
    $savedAppData = $env:APPDATA
    try {
        $env:APPDATA = Join-Path $output 'appdata'
        foreach ($project in 'UFO.csproj', 'Tools\HarmonyApiAudit\HarmonyApiAudit.csproj', 'Tools\PatchSmoke\PatchSmoke.csproj', 'Tools\SourceAudit\SourceAudit.csproj') {
            $restoreArgs = @('restore', $project, '--configfile', (Join-Path $repoRoot 'Tools\HarmonyApiAudit\NuGet.Config'), '-p:NuGetAudit=false') + $properties
            Invoke-Checked dotnet $restoreArgs (([IO.Path]::GetFileNameWithoutExtension($project)) + '-restore.log')
        }
    }
    finally { $env:APPDATA = $savedAppData }

    Invoke-Checked dotnet (@('build', 'UFO.csproj', '-c', 'Release', '-p:Platform=x64', '--no-restore') + $properties) 'build.log'
    Invoke-Checked dotnet @('build', 'Tools\HarmonyApiAudit', '--no-restore') 'audit-build.log'
    Invoke-Checked dotnet @('build', 'Tools\PatchSmoke', '--no-restore', "-p:HarmonyAssemblyPath=$harmonyPath") 'smoke-build.log'
    Invoke-Checked dotnet @('build', 'Tools\SourceAudit', '--no-restore') 'source-build.log'
    Invoke-Checked dotnet (@('msbuild', 'UFO.csproj', '-t:ResolveReferences', '-p:Configuration=Release', '-p:Platform=x64', '-getItem:Compile,ReferencePath') + $properties) 'source-input.json' $true
    Invoke-Checked dotnet @('Tools\SourceAudit\bin\Debug\net10.0\SourceAudit.dll', (Join-Path $output 'source-input.json'), '--check') 'source-audit.json' $true
    $mod = Join-Path $repoRoot 'Module\bin\Win64_Shipping_Client\UFO.dll'
    $audit = Join-Path $repoRoot 'Tools\HarmonyApiAudit\bin\Debug\net10.0\HarmonyApiAudit.dll'
    $smoke = Join-Path $repoRoot 'Tools\PatchSmoke\bin\Debug\net48\PatchSmoke.exe'
    foreach ($withoutNaval in $false, $true) {
        $name = if ($withoutNaval) { 'without-naval' } else { 'with-naval' }
        $auditArgs = @($audit, $mod, $gameBin, "--dependencies=$dependencyList", "--report=$(Join-Path $output "targets-$name.txt")")
        $smokeArgs = @($mod, $gameRoot, $dependencyList)
        if ($withoutNaval) { $auditArgs += '--without-naval'; $smokeArgs += '--without-naval' }
        Invoke-Checked dotnet $auditArgs "audit-$name.log"
        Invoke-Checked $smoke $smokeArgs "smoke-$name.log"
    }
    Invoke-Checked pwsh @('-NoProfile', '-File', 'Tools\ModuleDataAudit\Validate-ModuleData.ps1', '-GameRoot', $gameRoot) 'moduledata.log'
    $featureArgs = @('Tools\CompatibilityAudit\validate_features.py', '--source-audit', (Join-Path $output 'source-audit.json'), '--targets', (Join-Path $output 'targets-with-naval.txt'))
    if ($UpdateReadme) { $featureArgs += '--update-readme' }
    Invoke-Checked python $featureArgs 'features.log'
    @($mod, (Join-Path $gameBin 'TaleWorlds.CampaignSystem.dll'), (Join-Path $gameBin 'TaleWorlds.Core.dll'), (Join-Path $gameBin 'TaleWorlds.MountAndBlade.dll')) + $dependencies |
        Get-FileHash -Algorithm SHA256 | Select-Object Path, Hash | ConvertTo-Json | Set-Content (Join-Path $output 'fingerprints.json')

    if ($Package) {
        $moduleRoot = Join-Path $repoRoot 'Module'
        $moduleInfo = [xml]::new()
        $moduleInfo.Load((Join-Path $moduleRoot 'SubModule.xml'))
        $version = $moduleInfo.Module.Version.value
        $dist = Join-Path $repoRoot 'dist'
        [void][IO.Directory]::CreateDirectory($dist)
        $zipPath = Join-Path $dist "UFO-$version.zip"
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        $archive = [IO.Compression.ZipArchive]::new([IO.File]::Open($zipPath, [IO.FileMode]::Create, [IO.FileAccess]::Write), [IO.Compression.ZipArchiveMode]::Create)
        try {
            foreach ($file in Get-ChildItem -LiteralPath $moduleRoot -Recurse -File) {
                $relative = [IO.Path]::GetRelativePath($moduleRoot, $file.FullName).Replace('\', '/')
                [void][IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $file.FullName, "UFO/$relative", [IO.Compression.CompressionLevel]::Optimal)
            }
        }
        finally { $archive.Dispose() }
        $archive = [IO.Compression.ZipFile]::OpenRead($zipPath)
        $hashAlgorithm = [Security.Cryptography.SHA256]::Create()
        try {
            $files = @(Get-ChildItem -LiteralPath $moduleRoot -Recurse -File)
            if ($archive.Entries.Count -ne $files.Count) { throw 'Package file count differs from Module.' }
            foreach ($file in $files) {
                $relative = [IO.Path]::GetRelativePath($moduleRoot, $file.FullName).Replace('\', '/')
                $entry = $archive.GetEntry("UFO/$relative")
                if ($null -eq $entry) { throw "Package entry missing: $relative" }
                $stream = $entry.Open()
                try { $actual = [Convert]::ToHexString($hashAlgorithm.ComputeHash($stream)) }
                finally { $stream.Dispose() }
                if ($actual -ne (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash) { throw "Package hash mismatch: $relative" }
            }
            Write-Host "Package verified: $($files.Count) entries match Module SHA-256 hashes."
        }
        finally { $hashAlgorithm.Dispose(); $archive.Dispose() }
        Write-Host "Package: $zipPath"
    }
    Write-Host "Compatibility validation passed. Logs: $output"
}
finally { Pop-Location }
