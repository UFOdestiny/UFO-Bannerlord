using HarmonyLib;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using UFO.Setting;

namespace UFO.Shokuho.Combat;


// CRUSH THROUGH EVERYONE

[HarmonyPatch]
internal class DecideCrushedThroughPrefixPatch_c
{
    private static bool Prepare()
    {
        return AccessTools.TypeByName("Shokuho.ShokuhoCustomCampaign.Models.ShokuhoCustomAgentApplyDamageModel") != null;
    }
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("Shokuho.ShokuhoCustomCampaign.Models.ShokuhoCustomAgentApplyDamageModel");
        return AccessTools.Method(type, "DecideCrushedThrough");
    }

    private static bool Prefix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
    {
        if (SettingsManager.TestMode.Value)
        {
            //return;
        }
        if (SettingsManager.PlayerAlwaysCrush.Value && attackerAgent.IsPlayerControlled)
        {
            __result = true;
            return false;
        }
        return true;
    }
}

[HarmonyPatch]
internal class DecideCrushedThroughPrefixPatch_s
{
    private static bool Prepare()
    {
        return AccessTools.TypeByName("Shokuho.CustomCampaign.CustomLocations.models.ShokuhoSandboxAgentApplyDamageModel") != null;
    }
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("Shokuho.CustomCampaign.CustomLocations.models.ShokuhoSandboxAgentApplyDamageModel");
        return AccessTools.Method(type, "DecideCrushedThrough");
    }
    private static bool Prefix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
    {
        if (SettingsManager.TestMode.Value)
        {
            //return;
        }
        if (SettingsManager.PlayerAlwaysCrush.Value && attackerAgent.IsPlayerControlled)
        {
            __result = true;
            return false;
        }
        return true;
    }

}

