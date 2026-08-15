using System.Reflection;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: HarmonyApiAudit <UFO.dll> <Bannerlord bin directory> [TypeName [MemberName]]");
    return 64;
}

var modAssemblyPath = Path.GetFullPath(args[0]);
var gameBinPath = Path.GetFullPath(args[1]);
if (!File.Exists(modAssemblyPath) || !Directory.Exists(gameBinPath))
{
    Console.Error.WriteLine("The mod assembly or game binary directory does not exist.");
    return 64;
}

var trustedPlatformAssemblies = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty)
    .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
var harmonyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", "lib.harmony", "2.4.2", "lib", "net10.0", "0Harmony.dll");
var gameRootPath = Directory.GetParent(gameBinPath)?.Parent?.FullName ?? gameBinPath;
var assemblyPaths = trustedPlatformAssemblies
    .Concat(Directory.EnumerateFiles(gameBinPath, "*.dll"))
    .Concat(Directory.Exists(Path.Combine(gameRootPath, "Modules"))
        ? Directory.EnumerateFiles(Path.Combine(gameRootPath, "Modules"), "*.dll", SearchOption.AllDirectories)
        : [])
    .Append(harmonyPath)
    .Append(modAssemblyPath)
    .Where(File.Exists)
    .Distinct(StringComparer.OrdinalIgnoreCase);

using var context = new MetadataLoadContext(new PathAssemblyResolver(assemblyPaths));
var modAssembly = context.LoadFromAssemblyPath(modAssemblyPath);

if (args.Length >= 3)
{
    var typeName = args[2];
    var memberName = args.Length >= 4 ? args[3] : null;
    var matchedTypes = assemblyPaths
        .Where(path => !path.Equals(modAssemblyPath, StringComparison.OrdinalIgnoreCase))
        .Select(path => TryLoadType(context, path, typeName))
        .Where(match => match.HasValue)
        .Select(match => match!.Value)
        .OrderBy(match => match.Type.Assembly.GetName().Name)
        .ThenBy(match => match.Type.FullName)
        .ToArray();

    if (matchedTypes.Length == 0)
    {
        Console.WriteLine($"TYPE NOT FOUND: {typeName}");
        return 1;
    }

    foreach (var match in matchedTypes)
    {
        Console.WriteLine($"ASSEMBLY: {match.Type.Assembly.GetName().Name} ({match.Path})");
        Console.WriteLine($"TYPE: {match.Type.FullName}");
        const BindingFlags memberFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        if (memberName is null)
        {
            foreach (var member in match.Type.GetMembers(memberFlags).Where(member => member is FieldInfo or PropertyInfo or MethodInfo).OrderBy(member => member.MemberType).ThenBy(member => member.Name))
                Console.WriteLine($"MEMBER: {FormatMember(member)}");
            continue;
        }

        var members = match.Type.GetMembers(memberFlags).Where(member => member.Name == memberName).ToArray();
        if (members.Length == 0) Console.WriteLine($"MEMBER NOT FOUND: {memberName}");
        foreach (var member in members) Console.WriteLine($"MEMBER: {FormatMember(member)}");
    }
    return 0;
}
var findings = new List<string>();
var checkedPatches = 0;
var attributeTypes = new HashSet<string>(StringComparer.Ordinal);

foreach (var type in modAssembly.GetTypes())
{
    foreach (var attribute in type.GetCustomAttributesData()) attributeTypes.Add(attribute.AttributeType.FullName ?? attribute.AttributeType.Name);
    var target = new PatchTarget();
    foreach (var attribute in type.GetCustomAttributesData().Where(IsHarmonyPatch))
        target.Apply(attribute);

    if (target.DeclaringType is null || string.IsNullOrEmpty(target.MethodName) && target.MethodKind == PatchMethodKind.Normal)
        continue;

    checkedPatches++;
    var method = target.Resolve(out var resolutionIssue);
    var label = $"{type.FullName} -> {target.Describe()}";
    if (method is null)
    {
        findings.Add($"MISSING: {label}{resolutionIssue}");
        continue;
    }
    if (resolutionIssue is not null) findings.Add($"AMBIGUOUS: {label}{resolutionIssue}");

    foreach (var patchMethod in type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 .Where(m => m.Name is "Prefix" or "Postfix" or "Transpiler" or "Finalizer" ||
                             m.GetCustomAttributesData().Any(a => IsHarmonyRole(a, "HarmonyPrefix") || IsHarmonyRole(a, "HarmonyPostfix") || IsHarmonyRole(a, "HarmonyTranspiler") || IsHarmonyRole(a, "HarmonyFinalizer"))))
    {
        ValidatePatchParameters(type, patchMethod, method, findings);
    }
}

Console.WriteLine($"Checked Harmony targets: {checkedPatches}");
if (checkedPatches == 0) Console.WriteLine($"Discovered class attributes: {string.Join(", ", attributeTypes.Order())}");
foreach (var finding in findings) Console.WriteLine(finding);
Console.WriteLine($"Incompatible targets or bindings: {findings.Count}");
return findings.Count == 0 ? 0 : 1;

static bool IsHarmonyPatch(CustomAttributeData attribute) => attribute.AttributeType.FullName == "HarmonyLib.HarmonyPatch";
static bool IsHarmonyRole(CustomAttributeData attribute, string name) => attribute.AttributeType.FullName == $"HarmonyLib.{name}";

static void ValidatePatchParameters(Type patchType, MethodInfo patchMethod, MethodBase target, List<string> findings)
{
    var targetParameters = target.GetParameters().ToDictionary(p => p.Name!, StringComparer.Ordinal);
    foreach (var parameter in patchMethod.GetParameters())
    {
        var name = parameter.Name!;
        if (name is "__instance" or "__state" or "__args" or "__originalMethod" or "__runOriginal" or "instructions" or "generator" || name.StartsWith("__", StringComparison.Ordinal) && char.IsDigit(name[2])) continue;
        if (name == "__result")
        {
            if (target is MethodInfo targetMethod && targetMethod.ReturnType != typeof(void)) continue;
            findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} uses __result for void target {target.DeclaringType!.FullName}.{target.Name}");
            continue;
        }
        if (name.StartsWith("___", StringComparison.Ordinal))
        {
            var fieldName = name[3..];
            if (FindField(target.DeclaringType!, fieldName) is null)
                findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} references missing field {target.DeclaringType!.FullName}.{fieldName}");
            continue;
        }
        if (!targetParameters.ContainsKey(name))
            findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} parameter '{name}' is absent from {target.DeclaringType!.FullName}.{target.Name}({string.Join(", ", targetParameters.Keys)})");
    }
}

static FieldInfo? FindField(Type type, string name)
{
    for (var current = type; current is not null; current = current.BaseType)
    {
        var field = current.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        if (field is not null) return field;
    }
    return null;
}

static (Type Type, string Path)? TryLoadType(MetadataLoadContext context, string path, string typeName)
{
    try
    {
        var assembly = context.LoadFromAssemblyPath(path);
        var type = assembly.GetType(typeName, throwOnError: false)
                   ?? assembly.GetTypes().FirstOrDefault(candidate => candidate.Name == typeName || candidate.FullName == typeName);
        return type is null ? null : (type, path);
    }
    catch (BadImageFormatException) { return null; }
    catch (FileLoadException) { return null; }
    catch (FileNotFoundException) { return null; }
    catch (TypeLoadException) { return null; }
}

static string FormatMember(MemberInfo member) => member switch
{
    FieldInfo field => $"field {field.FieldType.FullName} {field.Name}",
    PropertyInfo property => $"property {property.PropertyType.FullName} {property.Name} {{ {(property.GetMethod is null ? string.Empty : "get; ")}{(property.SetMethod is null ? string.Empty : "set;")} }}",
    MethodInfo method => $"method {method.ReturnType.FullName} {method.Name}({string.Join(", ", method.GetParameters().Select(parameter => $"{parameter.ParameterType.FullName} {parameter.Name}"))})",
    _ => $"{member.MemberType} {member.Name}"
};

enum PatchMethodKind { Normal, Getter, Setter, Constructor }

sealed class PatchTarget
{
    public Type? DeclaringType { get; private set; }
    public string? MethodName { get; private set; }
    public PatchMethodKind MethodKind { get; private set; }
    public Type[]? ArgumentTypes { get; private set; }

    public void Apply(CustomAttributeData attribute)
    {
        var parameters = attribute.Constructor.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            var parameterType = parameters[i].ParameterType;
            var value = attribute.ConstructorArguments[i].Value;
            if (parameterType.FullName == "System.Type") DeclaringType = value as Type;
            else if (parameterType.FullName == "System.String") MethodName = value as string;
            else if (parameterType.FullName == "HarmonyLib.MethodType" && value is not null)
            {
                var kind = Convert.ToInt32(value);
                MethodKind = kind switch { 1 => PatchMethodKind.Getter, 2 => PatchMethodKind.Setter, 3 => PatchMethodKind.Constructor, _ => PatchMethodKind.Normal };
            }
            else if (parameterType.IsArray && parameterType.GetElementType()?.FullName == "System.Type" && value is IList<CustomAttributeTypedArgument> values) ArgumentTypes = values.Select(v => (Type)v.Value!).ToArray();
        }
    }

    public MethodBase? Resolve(out string? issue)
    {
        issue = null;
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        if (DeclaringType is null) return null;
        if (MethodKind == PatchMethodKind.Getter) return DeclaringType.GetProperty(MethodName!, flags)?.GetMethod;
        if (MethodKind == PatchMethodKind.Setter) return DeclaringType.GetProperty(MethodName!, flags)?.SetMethod;
        if (MethodKind == PatchMethodKind.Constructor) return DeclaringType.GetConstructor(flags, binder: null, ArgumentTypes ?? Type.EmptyTypes, modifiers: null);
        if (ArgumentTypes is not null) return DeclaringType.GetMethod(MethodName!, flags, binder: null, ArgumentTypes, modifiers: null);
        var candidates = DeclaringType.GetMethods(flags).Where(m => m.Name == MethodName).OrderBy(m => m.MetadataToken).ToArray();
        if (candidates.Length > 1)
            issue = $" has {candidates.Length} overloads and the patch does not declare argument types: {string.Join(" | ", candidates.Select(FormatSignature))}";
        return candidates.FirstOrDefault();
    }

    public string Describe() => $"{DeclaringType?.FullName}.{MethodName}({string.Join(", ", ArgumentTypes?.Select(t => t.Name) ?? [])})";

    private static string FormatSignature(MethodInfo method) => $"({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})";
}
