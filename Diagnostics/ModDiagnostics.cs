using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace UFO.Diagnostics;

internal static class ModDiagnostics
{
    internal static string WriteError(Exception exception, Type patchType)
    {
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine(directory, $"Error-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");
        var report = new StringBuilder()
            .AppendLine("Thanks a lot for helping to improve this mod!")
            .AppendLine("Please share this file with the mod author.")
            .AppendLine()
            .AppendLine("Modules:");

        foreach (var module in ModuleHelper.GetModules())
            report.AppendLine($"{module.Name} {module.Version}");

        var patch = patchType?.GetCustomAttribute<HarmonyPatch>();
        if (patch is not null)
        {
            report.AppendLine().AppendLine("Harmony Patch:")
                .AppendLine("Type: " + patchType.FullName)
                .AppendLine("Declaring Type: " + patch.info.declaringType?.FullName)
                .AppendLine("Method: " + patch.info.methodName);
        }

        report.AppendLine().AppendLine("Exception:").AppendLine(exception.ToString());
        File.WriteAllText(path, report.ToString());
        InformationManager.DisplayMessage(new InformationMessage(path, Colors.Red));
        return path;
    }
}
