using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using UFO.Model;

namespace UFO.Patching;

internal static class PatchBootstrapper
{
    internal static IReadOnlyList<string> Apply(Harmony harmony, Assembly assembly, bool earlyOnly)
    {
        var failures = new List<string>();
        foreach (var type in AccessTools.GetTypesFromAssembly(assembly))
        {
            if (!type.GetCustomAttributes(typeof(HarmonyPatch), false).Any())
                continue;
            // Only the scalar model limit is needed while creating/loading a game.
            // Hero/party patches must wait until deserialization has completed.
            if ((type == typeof(MaxAttributePatch)) != earlyOnly)
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
