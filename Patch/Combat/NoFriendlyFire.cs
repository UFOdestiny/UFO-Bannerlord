using HarmonyLib;
using JetBrains.Annotations;
using SandBox.GameComponents;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using UFO;
using UFO.Extension;
using UFO.Setting;
namespace UFO.Patch.Combat;

public static class NoFriendlyFire
{
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.AttackerAgentOrigin.TryGetParty(out var party) && party.IsPlayerParty() && attackInformation.IsFriendlyFire && SettingsManager.NoFriendlyFire.IsChanged)
            {
                __result = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoFriendlyFire));
        }
    }
}

[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class NoFriendlyFire_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        NoFriendlyFire.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}
