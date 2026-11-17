using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

if (args.Length < 1) return 64;
using var input = JsonDocument.Parse(File.ReadAllText(args[0]));
var items = input.RootElement.GetProperty("Items");
var trees = items.GetProperty("Compile").EnumerateArray()
    .Select(item => item.GetProperty("FullPath").GetString()!)
    .Where(path => !path.Contains("\\obj\\", StringComparison.OrdinalIgnoreCase))
    .Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), new CSharpParseOptions(LanguageVersion.CSharp12), path)).ToArray();
var references = items.GetProperty("ReferencePath").EnumerateArray()
    .Select(item => MetadataReference.CreateFromFile(item.GetProperty("FullPath").GetString()!));
var compilation = CSharpCompilation.Create("UFO", trees, references,
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
if (errors.Length > 0)
{
    foreach (var error in errors) Console.Error.WriteLine(error);
    return 1;
}
var used = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
var declarations = new List<(ISymbol Symbol, SyntaxNode Node)>();
var hooks = new List<object>();
var functions = new List<object>();
foreach (var tree in trees)
{
    var model = compilation.GetSemanticModel(tree);
    foreach (var node in tree.GetRoot().DescendantNodes())
    {
        if (node is IdentifierNameSyntax or GenericNameSyntax or ObjectCreationExpressionSyntax or AttributeSyntax or BinaryExpressionSyntax)
        {
            var symbol = model.GetSymbolInfo(node).Symbol;
            if (symbol is not null) used.Add(Canonical(symbol));
        }
        if (node is BaseMethodDeclarationSyntax or PropertyDeclarationSyntax or VariableDeclaratorSyntax or BaseTypeDeclarationSyntax)
        {
            var symbol = model.GetDeclaredSymbol(node);
            var declaration = node is VariableDeclaratorSyntax { Parent.Parent: FieldDeclarationSyntax field } && field.Declaration.Variables.Count == 1 ? field : node;
            if (symbol is IMethodSymbol or IPropertySymbol or IFieldSymbol or INamedTypeSymbol) declarations.Add((symbol, declaration));
        }
        if (node is MethodDeclarationSyntax method && model.GetDeclaredSymbol(method) is { } methodSymbol &&
            (methodSymbol.Name is "Prefix" or "Postfix" or "Transpiler" or "Finalizer" || methodSymbol.GetAttributes().Any(a => a.AttributeClass?.Name.StartsWith("Harmony") == true || a.AttributeClass?.Name == "OptionalPatchAttribute")))
        {
            var settings = method.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
                .Where(access => access.Expression.ToString() == "SettingsManager")
                .Select(access => access.Name.Identifier.ValueText).Distinct().Order().ToArray();
            var calls = method.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Select(call => model.GetSymbolInfo(call).Symbol).OfType<IMethodSymbol>()
                .Where(symbol => symbol.ContainingAssembly.Name == "UFO")
                .Select(symbol => Canonical(symbol).ToDisplayString()).Distinct().ToArray();
            hooks.Add(new { Symbol = methodSymbol.ToDisplayString(), File = tree.FilePath, Line = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                ClassAttributes = method.Parent is ClassDeclarationSyntax parent ? parent.AttributeLists.ToString() : "",
                MethodAttributes = method.AttributeLists.ToString(), Settings = settings, Calls = calls });
        }
        if (node is MethodDeclarationSyntax or PropertyDeclarationSyntax && model.GetDeclaredSymbol(node) is { } function)
        {
            functions.Add(new { Symbol = Canonical(function).ToDisplayString(), File = tree.FilePath, Line = node.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                Settings = node.DescendantNodes().OfType<MemberAccessExpressionSyntax>().Where(access => access.Expression.ToString() == "SettingsManager").Select(access => access.Name.Identifier.ValueText).Distinct().ToArray(),
                Calls = node.DescendantNodes().OfType<InvocationExpressionSyntax>().Select(call => model.GetSymbolInfo(call).Symbol).OfType<IMethodSymbol>()
                    .Where(symbol => symbol.ContainingAssembly.Name == "UFO").Select(symbol => Canonical(symbol).ToDisplayString()).Distinct().ToArray() });
        }
    }
}
foreach (var declaration in declarations.Where(d => IsFrameworkEntry(d.Symbol))) used.Add(Canonical(declaration.Symbol));
foreach (var symbol in used.ToArray())
    for (var type = symbol.ContainingType; type is not null; type = type.ContainingType) used.Add(Canonical(type));
var reflectionNames = trees.SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
    .Where(call => call.Expression.ToString().EndsWith(".Method") || call.Expression.ToString().EndsWith(".GetMethod") || call.Expression.ToString() == "RequireMethod")
    .SelectMany(call => call.ArgumentList.Arguments.Select(a => a.Expression).OfType<LiteralExpressionSyntax>())
    .Select(literal => literal.Token.ValueText).ToHashSet(StringComparer.Ordinal);
var candidates = declarations.Where(d => !used.Contains(Canonical(d.Symbol)) && !IsFrameworkEntry(d.Symbol) && !(d.Symbol is IMethodSymbol && reflectionNames.Contains(d.Symbol.Name)))
    .Select(d => new { Symbol = d.Symbol.ToDisplayString(), Kind = d.Symbol.Kind.ToString(), File = d.Node.SyntaxTree.FilePath,
        Line = d.Node.GetLocation().GetLineSpan().StartLinePosition.Line + 1, Start = d.Node.SpanStart, Length = d.Node.Span.Length }).ToArray();
var unusedUsings = compilation.GetDiagnostics().Where(d => d.Id == "CS8019")
    .Select(d => new { File = d.Location.SourceTree!.FilePath, Start = d.Location.SourceSpan.Start, Length = d.Location.SourceSpan.Length }).ToArray();
Console.WriteLine(JsonSerializer.Serialize(new { Candidates = candidates, UnusedUsings = unusedUsings, Hooks = hooks, Functions = functions }, new JsonSerializerOptions { WriteIndented = true }));
return args.Contains("--check") && (candidates.Length != 0 || unusedUsings.Length != 0) ? 1 : 0;

static ISymbol Canonical(ISymbol symbol) => symbol is IMethodSymbol method ? (method.ReducedFrom ?? method).OriginalDefinition : symbol.OriginalDefinition;

static bool IsFrameworkEntry(ISymbol symbol)
{
    if (symbol.IsOverride || symbol.IsAbstract || symbol.IsImplicitlyDeclared) return true;
    if (symbol is INamedTypeSymbol { BaseType.Name: "SaveableTypeDefiner" }) return true;
    if (symbol is IMethodSymbol { Name: "Prefix" or "Postfix" or "Transpiler" or "Finalizer" or "TargetMethod" or "TargetMethods" or "Prepare" or "Cleanup" }) return true;
    if (symbol.GetAttributes().Any(a => a.AttributeClass?.Name.StartsWith("Harmony") == true || a.AttributeClass?.Name.StartsWith("LocalizedSetting") == true || a.AttributeClass?.Name.StartsWith("Saveable") == true || a.AttributeClass?.Name == "OptionalPatchAttribute")) return true;
    if (symbol.ContainingType is { } type)
    {
        if (symbol is IMethodSymbol { MethodKind: MethodKind.Constructor } && type.BaseType?.Name is "SaveableTypeDefiner" or "Attribute") return true;
        foreach (var contract in type.AllInterfaces)
        foreach (var member in contract.GetMembers())
            if (SymbolEqualityComparer.Default.Equals(type.FindImplementationForInterfaceMember(member), symbol)) return true;
    }
    return false;
}
