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
var withoutNaval = args.Contains("--without-naval");
var dependenciesOption = args.FirstOrDefault(arg => arg.StartsWith("--dependencies="));
var dependencyPaths = dependenciesOption is null ? [harmonyPath] : File.ReadAllLines(dependenciesOption["--dependencies=".Length..]);
var assemblyPaths = trustedPlatformAssemblies
    .Concat(Directory.EnumerateFiles(gameBinPath, "*.dll"))
    .Concat(Directory.Exists(Path.Combine(gameRootPath, "Modules"))
        ? Directory.EnumerateFiles(Path.Combine(gameRootPath, "Modules"), "*.dll", SearchOption.AllDirectories)
        : [])
    .Concat(dependencyPaths)
    .Append(modAssemblyPath)
    .Where(File.Exists)
    .Where(path => !withoutNaval || !Path.GetFileName(path).StartsWith("NavalDLC", StringComparison.OrdinalIgnoreCase))
    .Distinct(StringComparer.OrdinalIgnoreCase);

using var context = new MetadataLoadContext(new PathAssemblyResolver(assemblyPaths));
var modAssembly = context.LoadFromAssemblyPath(modAssemblyPath);

if (args.Length >= 3 && args[2].StartsWith("method:", StringComparison.Ordinal))
{
    var methodName = args[2]["method:".Length..];
    var matches = new List<(Type Type, MethodInfo Method, string Path)>();
    foreach (var path in assemblyPaths.Where(path => !path.Equals(modAssemblyPath, StringComparison.OrdinalIgnoreCase)))
    {
        try
        {
            var assembly = context.LoadFromAssemblyPath(path);
            foreach (var type in assembly.GetTypes())
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                         .Where(method => method.Name == methodName))
                matches.Add((type, method, path));
        }
        catch (BadImageFormatException) { }
        catch (FileLoadException) { }
        catch (FileNotFoundException) { }
        catch (TypeLoadException) { }
    }

    if (matches.Count == 0)
    {
        Console.WriteLine($"METHOD NOT FOUND: {methodName}");
        return 1;
    }

    foreach (var match in matches.OrderBy(match => match.Type.Assembly.GetName().Name).ThenBy(match => match.Type.FullName))
    {
        Console.WriteLine($"ASSEMBLY: {match.Type.Assembly.GetName().Name} ({match.Path})");
        Console.WriteLine($"TYPE: {match.Type.FullName}");
        Console.WriteLine($"MEMBER: {FormatMember(match.Method)}");
    }
    return 0;
}

if (args.Length >= 3 && !args[2].StartsWith("--", StringComparison.Ordinal))
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
var report = new List<string>();

foreach (var type in modAssembly.GetTypes())
{
    foreach (var attribute in type.GetCustomAttributesData()) attributeTypes.Add(attribute.AttributeType.FullName ?? attribute.AttributeType.Name);
    foreach (var callback in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
    foreach (var attribute in callback.GetCustomAttributesData().Where(a => a.AttributeType.FullName == "UFO.Patching.OptionalPatchAttribute"))
    {
        var typeName = (string)attribute.ConstructorArguments[0].Value!;
        var methodName = (string)attribute.ConstructorArguments[1].Value!;
        var firstArgumentType = attribute.ConstructorArguments.Count > 2 ? (Type?)attribute.ConstructorArguments[2].Value : null;
        var targetType = assemblyPaths.Select(path => TryLoadType(context, path, typeName)).FirstOrDefault(match => match.HasValue)?.Type;
        if (targetType is null && !assemblyPaths.Any(path => Path.GetFileName(path) == "NavalDLC.dll")) continue;
        checkedPatches++;
        var candidates = targetType?.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(method => method.Name == methodName && (firstArgumentType is null || method.GetParameters().FirstOrDefault()?.ParameterType == firstArgumentType)).ToArray() ?? [];
        var label = $"{type.FullName}.{callback.Name} -> {typeName}.{methodName} (optional DLC)";
        report.Add(candidates.Length == 1 ? $"{type.FullName}.{callback.Name} -> {FormatTarget(candidates[0])} (optional DLC)" : label);
        if (candidates.Length != 1) findings.Add($"MISSING/AMBIGUOUS: {label} ({candidates.Length} matches)");
        else ValidatePatchParameters(type, callback, candidates[0], findings);
    }
    if (!type.GetCustomAttributesData().Any(IsHarmonyPatch)) continue;
    var target = new PatchTarget();
    foreach (var attribute in type.GetCustomAttributesData().Where(IsHarmonyPatch))
        target.Apply(attribute);

    foreach (var patchMethod in type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 .Where(m => m.Name is "Prefix" or "Postfix" or "Transpiler" or "Finalizer" ||
                             m.GetCustomAttributesData().Any(a => IsHarmonyRole(a, "HarmonyPrefix") || IsHarmonyRole(a, "HarmonyPostfix") || IsHarmonyRole(a, "HarmonyTranspiler") || IsHarmonyRole(a, "HarmonyFinalizer"))))
    {
        var methodTarget = target.Copy();
        foreach (var attribute in patchMethod.GetCustomAttributesData().Where(IsHarmonyPatch)) methodTarget.Apply(attribute);
        if (methodTarget.DeclaringType is null || string.IsNullOrEmpty(methodTarget.MethodName) && methodTarget.MethodKind == PatchMethodKind.Normal) continue;
        checkedPatches++;
        var method = methodTarget.Resolve(out var resolutionIssue);
        var label = $"{type.FullName}.{patchMethod.Name} -> {methodTarget.Describe()}";
        report.Add(method is null ? label : $"{type.FullName}.{patchMethod.Name} -> {FormatTarget(method)}");
        if (method is null) { findings.Add($"MISSING: {label}{resolutionIssue}"); continue; }
        if (resolutionIssue is not null) findings.Add($"AMBIGUOUS: {label}{resolutionIssue}");
        if (method.IsAbstract || method.GetMethodBody() is null) findings.Add($"NO BODY: {label}");
        ValidatePatchParameters(type, patchMethod, method, findings);
    }
}

Console.WriteLine($"Checked Harmony targets: {checkedPatches}");
Console.WriteLine($"Checked reflection contracts: {ReflectionContracts.Validate(context, assemblyPaths, findings)}");
if (checkedPatches == 0) Console.WriteLine($"Discovered class attributes: {string.Join(", ", attributeTypes.Order())}");
foreach (var finding in findings) Console.WriteLine(finding);
Console.WriteLine($"Incompatible targets or bindings: {findings.Count}");
var reportOption = args.FirstOrDefault(arg => arg.StartsWith("--report="));
if (reportOption is not null) File.WriteAllLines(reportOption["--report=".Length..], report.Concat(findings));
return findings.Count == 0 ? 0 : 1;

static bool IsHarmonyPatch(CustomAttributeData attribute) => attribute.AttributeType.FullName == "HarmonyLib.HarmonyPatch";
static string FormatTarget(MethodBase method) => $"{method.DeclaringType!.FullName}.{method.Name}({string.Join(", ", method.GetParameters().Select(parameter =>
    (parameter.IsOut ? "out " : parameter.ParameterType.IsByRef ? parameter.IsIn ? "in " : "ref " : "") +
    FormatType(parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType) + " " + parameter.Name))})" +
    (method is MethodInfo info ? $" : {FormatType(info.ReturnType)}" : "");
static string FormatType(Type type) => type.IsGenericType
    ? type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(FormatType)) + ">"
    : type.Name;
static bool IsHarmonyRole(CustomAttributeData attribute, string name) => attribute.AttributeType.FullName == $"HarmonyLib.{name}";

static void ValidatePatchParameters(Type patchType, MethodInfo patchMethod, MethodBase target, List<string> findings)
{
    var targetParameters = target.GetParameters().ToDictionary(p => p.Name!, StringComparer.Ordinal);
    foreach (var parameter in patchMethod.GetParameters())
    {
        var name = parameter.Name!;
        if (name is "__state" or "__args" or "__originalMethod" or "__runOriginal" or "instructions" or "generator") continue;
        if (name == "__instance")
        {
            if (target.IsStatic) findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} uses __instance for a static target");
            else ValidateType(parameter, target.DeclaringType!, patchType, patchMethod, findings);
            continue;
        }
        if (name == "__result")
        {
            if (target is MethodInfo targetMethod && targetMethod.ReturnType.FullName != "System.Void")
            {
                ValidateType(parameter, targetMethod.ReturnType, patchType, patchMethod, findings);
                continue;
            }
            findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} uses __result for void target {target.DeclaringType!.FullName}.{target.Name}");
            continue;
        }
        if (name.StartsWith("___", StringComparison.Ordinal))
        {
            var fieldName = name[3..];
            var field = FindField(target.DeclaringType!, fieldName);
            if (field is null)
                findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} references missing field {target.DeclaringType!.FullName}.{fieldName}");
            else ValidateType(parameter, field.FieldType, patchType, patchMethod, findings);
            continue;
        }
        ParameterInfo? original;
        if (name.Length > 2 && name.StartsWith("__", StringComparison.Ordinal) && int.TryParse(name[2..], out var index))
            original = target.GetParameters().ElementAtOrDefault(index);
        else targetParameters.TryGetValue(name, out original);
        if (original is null)
            findings.Add($"BINDING: {patchType.FullName}.{patchMethod.Name} parameter '{name}' is absent from {target.DeclaringType!.FullName}.{target.Name}({string.Join(", ", targetParameters.Keys)})");
        else ValidateType(parameter, original.ParameterType, patchType, patchMethod, findings);
    }
}

static void ValidateType(ParameterInfo parameter, Type original, Type patchType, MethodInfo patchMethod, List<string> findings)
{
    var supplied = original.IsByRef ? original.GetElementType()! : original;
    var requested = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType;
    if (requested == supplied || requested.FullName == "System.Object" || !parameter.ParameterType.IsByRef && requested.IsAssignableFrom(supplied)) return;
    findings.Add($"TYPE: {patchType.FullName}.{patchMethod.Name} parameter '{parameter.Name}' requests {parameter.ParameterType}, target supplies {original}");
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
    public PatchTarget Copy() => (PatchTarget)MemberwiseClone();
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
