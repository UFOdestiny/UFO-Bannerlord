using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;

namespace UFO.Patching;

internal static class PatchBootstrapper
{
    internal static IReadOnlyList<string> Apply(Harmony harmony, Assembly assembly)
    {
        var failures = new List<string>();
        foreach (var type in AccessTools.GetTypesFromAssembly(assembly))
        {
            if (!type.GetCustomAttributes(typeof(HarmonyPatch), false).Any())
                continue;

            try { new PatchClassProcessor(harmony, type).Patch(); }
            catch (Exception exception)
            {
                failures.Add(type.FullName ?? type.Name);
                InformationManager.DisplayMessage(new InformationMessage($"UFO patch skipped: {type.Name} ({exception.Message})", Colors.Red));
            }
        }
        return failures;
    }
}
