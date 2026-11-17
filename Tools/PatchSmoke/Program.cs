using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;

// This isolated process installs the real Harmony wrappers without launching a
// campaign or invoking cheat callbacks. It does not establish in-game behaviour.
internal static class Program
{
    private static int Main(string[] args)
    {
        if (args.Length < 3 || args.Length > 4)
        {
            Console.Error.WriteLine("Usage: PatchSmoke <UFO.dll> <GameRoot> <dependency-paths.txt>");
            return 64;
        }
        var paths = File.ReadAllLines(args[2])
            .Concat(Directory.GetFiles(Path.Combine(args[1], "bin", "Win64_Shipping_Client"), "*.dll"))
            .Concat(Directory.GetFiles(Path.Combine(args[1], "Modules"), "*.dll", SearchOption.AllDirectories))
            .Where(File.Exists)
            .Where(path => args.Length < 4 || args[3] != "--without-naval" || !Path.GetFileName(path).StartsWith("NavalDLC", StringComparison.OrdinalIgnoreCase))
            .GroupBy(Path.GetFileNameWithoutExtension, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
            paths.TryGetValue(new AssemblyName(eventArgs.Name).Name, out var path) ? Assembly.LoadFrom(path) : null;
        return Run(args[0], paths);
    }

    private static int Run(string modPath, Dictionary<string, string> paths)
    {
        var assembly = Assembly.LoadFrom(Path.GetFullPath(modPath));
        ManagedRegressionChecks.Run(assembly);
        var harmony = new Harmony("UFO.offline-smoke");
        var failures = new List<string>();
        var count = 0;
        foreach (var type in assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(HarmonyPatch), false).Any()))
        {
            try
            {
                new PatchClassProcessor(harmony, type).Patch();
                Console.WriteLine("PASS " + type.FullName);
                count++;
            }
            catch (Exception exception)
            {
                failures.Add(type.FullName);
                Console.WriteLine("FAIL " + type.FullName + ": " + exception);
            }
        }
        if (paths.TryGetValue("NavalDLC", out var navalPath)) Assembly.LoadFrom(navalPath);
        foreach (var type in assembly.GetTypes())
        foreach (var callback in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
        foreach (var attribute in callback.GetCustomAttributesData().Where(attribute => attribute.AttributeType.FullName == "UFO.Patching.OptionalPatchAttribute"))
        {
            if (!paths.ContainsKey("NavalDLC")) continue;
            var targetTypeName = (string)attribute.ConstructorArguments[0].Value;
            var methodName = (string)attribute.ConstructorArguments[1].Value;
            var firstArgumentType = attribute.ConstructorArguments.Count > 2 ? (Type)attribute.ConstructorArguments[2].Value : null;
            try
            {
                var targetType = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(targetTypeName, false)).FirstOrDefault(t => t != null);
                var original = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Single(method => method.Name == methodName && (firstArgumentType == null || method.GetParameters().FirstOrDefault()?.ParameterType == firstArgumentType));
                harmony.Patch(original, postfix: new HarmonyMethod(callback));
                Console.WriteLine("PASS optional " + methodName);
                count++;
            }
            catch (Exception exception)
            {
                failures.Add(targetTypeName + "." + methodName);
                Console.WriteLine("FAIL optional " + methodName + ": " + exception);
            }
        }
        Console.WriteLine($"Patch installation: {count} passed, {failures.Count} failed. No game behaviours executed.");
        return failures.Count == 0 ? 0 : 1;
    }
}
