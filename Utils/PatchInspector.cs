using HarmonyLib;
using SandBox.GameComponents;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

public static class PatchInspector
{
    public static void PatchInformation()
    {
        MethodInfo target = AccessTools.Method(typeof(Mission), "UpdateMomentumRemaining");
        if (target == null)
        {
            InformationManager.DisplayMessage(new InformationMessage("Method Mission.UpdateMomentumRemaining not found.", Colors.Red));
            return;
        }

        var patchInfo = Harmony.GetPatchInfo(target);
        if (patchInfo == null)
        {
            InformationManager.DisplayMessage(new InformationMessage("No patches found on Mission.UpdateMomentumRemaining", Colors.Yellow));
            return;
        }

        InformationManager.DisplayMessage(new InformationMessage($"Patches on {target.DeclaringType.FullName}.{target.Name}:", Colors.Green));

        foreach (var patch in patchInfo.Prefixes)
        {
            string msg = $"[Prefix]   owner={patch.owner}, priority={patch.priority}, method={patch.PatchMethod.DeclaringType.FullName}.{patch.PatchMethod.Name}";
            InformationManager.DisplayMessage(new InformationMessage(msg, Colors.Green));
        }

        foreach (var patch in patchInfo.Postfixes)
        {
            string msg = $"[Postfix]  owner={patch.owner}, priority={patch.priority}, method={patch.PatchMethod.DeclaringType.FullName}.{patch.PatchMethod.Name}";
            InformationManager.DisplayMessage(new InformationMessage(msg, Colors.Green));
        }

        foreach (var patch in patchInfo.Transpilers)
        {
            string msg = $"[Transpiler] owner={patch.owner}, method={patch.PatchMethod.DeclaringType.FullName}.{patch.PatchMethod.Name}";
            InformationManager.DisplayMessage(new InformationMessage(msg, Colors.Green));
        }

        foreach (var patch in patchInfo.Finalizers)
        {
            string msg = $"[Finalizer] owner={patch.owner}, method={patch.PatchMethod.DeclaringType.FullName}.{patch.PatchMethod.Name}";
            InformationManager.DisplayMessage(new InformationMessage(msg, Colors.Green));
        }
    }
}
