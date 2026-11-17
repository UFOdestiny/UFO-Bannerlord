using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.ExceptionServices;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace UFO.Diagnostics;

internal static class ModDiagnostics
{
    private static string RuntimeLogPath;
    private static bool IsLoadingGame;
    private static int LoadExceptionCount;
    [ThreadStatic] private static bool IsWritingTrace;

    internal static void InstallRuntimeTrace()
    {
        if (RuntimeLogPath != null) return;
        RuntimeLogPath = Path.Combine(Path.GetDirectoryName(typeof(ModDiagnostics).Assembly.Location),
            $"UFO-runtime-{DateTime.Now:yyyyMMdd-HHmmss}.log");
        AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TraceRuntime("Loaded " + typeof(ModDiagnostics).Assembly.Location +
            "; MVID=" + typeof(ModDiagnostics).Assembly.ManifestModule.ModuleVersionId);
    }

    internal static void BeginGameLoad()
    {
        IsLoadingGame = true;
        LoadExceptionCount = 0;
        TraceRuntime("Game load started; gameplay patches are inactive.");
    }

    internal static void CompleteGameLoad()
    {
        IsLoadingGame = false;
        TraceRuntime("Game initialization completed or game ended.");
    }

    private static void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs args)
    {
        if (IsWritingTrace) return;
        var fromUfo = args.Exception.StackTrace?.Contains("UFO.") == true;
        if (!fromUfo && (!IsLoadingGame || LoadExceptionCount >= 32)) return;
        LoadExceptionCount++;
        TraceRuntime("First-chance exception (may be handled by the game): " + args.Exception);
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args) =>
        TraceRuntime("Unhandled exception: " + args.ExceptionObject);

    internal static void TraceRuntime(string message)
    {
        if (RuntimeLogPath == null || IsWritingTrace) return;
        IsWritingTrace = true;
        try { File.AppendAllText(RuntimeLogPath, $"{DateTime.Now:O} {message}{Environment.NewLine}", Encoding.UTF8); }
        catch { }
        finally { IsWritingTrace = false; }
    }

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

        foreach (var patch in patchType is null ? new HarmonyPatch[0] : patchType.GetCustomAttributes<HarmonyPatch>())
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
