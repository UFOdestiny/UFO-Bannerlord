using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using UFO.Setting;

namespace UFO.Patch.Combat;

/// <summary>Prevents enemy formations from selecting retreat-only AI behaviours during siege missions.</summary>
internal static class EnemyRetreatBehavior
{
    internal static bool IsEnemyTeam(object behavior)
    {
        if (!SettingsManager.EnemiesNoRunningAway.IsChanged || Mission.Current?.PlayerTeam == null)
            return false;

        try
        {
            var team = behavior.GetType().GetProperty("Team", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(behavior) as Team;
            return team != null && team.Side != Mission.Current.PlayerTeam.Side;
        }
        catch (Exception exception)
        {
            SubModule.LogError(exception, typeof(EnemyRetreatBehavior));
            return false;
        }
    }
}

[HarmonyPatch(typeof(BehaviorRetreat), "GetAiWeight")]
public static class EnemyRetreatBehaviorPatch
{
    [HarmonyPrefix]
    public static bool Prefix(object __instance, ref float __result)
    {
        if (!EnemyRetreatBehavior.IsEnemyTeam(__instance)) return true;
        __result = 0f;
        return false;
    }
}

[HarmonyPatch(typeof(BehaviorRetreatToCastle), "GetAiWeight")]
public static class EnemyRetreatToCastleBehaviorPatch
{
    [HarmonyPrefix]
    public static bool Prefix(object __instance, ref float __result)
    {
        if (!EnemyRetreatBehavior.IsEnemyTeam(__instance)) return true;
        __result = 0f;
        return false;
    }
}

[HarmonyPatch(typeof(BehaviorRetreatToKeep), "GetAiWeight")]
public static class EnemyRetreatToKeepBehaviorPatch
{
    [HarmonyPrefix]
    public static bool Prefix(object __instance, ref float __result)
    {
        if (!EnemyRetreatBehavior.IsEnemyTeam(__instance)) return true;
        __result = 0f;
        return false;
    }
}
