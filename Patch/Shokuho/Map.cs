using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Shokuho;


[HarmonyPatch]
public static class MapSpeedMultiplier
{
    private static bool Prepare()
    {
        return AccessTools.TypeByName("Shokuho.CustomCampaign.Models.ShokuhoPartySpeedCalculatingModel") != null;
    }
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("Shokuho.CustomCampaign.Models.ShokuhoPartySpeedCalculatingModel");
        return AccessTools.Method(type, "CalculateFinalSpeed");
    }

    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateFinalSpeed(ref MobileParty mobileParty, ref ExplainedNumber finalSpeed, ref ExplainedNumber __result)
    {
        try
        {
            if (mobileParty.IsPlayerParty() && SettingsManager.MapSpeedMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.MapSpeedMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(MapSpeedMultiplier));
        }
    }
}


[HarmonyPatch]
public static class NpcMapSpeedPercentage
{

    private static bool Prepare()
    {
        return AccessTools.TypeByName("Shokuho.CustomCampaign.Models.ShokuhoPartySpeedCalculatingModel") != null;
    }
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("Shokuho.CustomCampaign.Models.ShokuhoPartySpeedCalculatingModel");
        return AccessTools.Method(type, "CalculateFinalSpeed");
    }



    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateFinalSpeed(ref MobileParty mobileParty, ref ExplainedNumber finalSpeed, ref ExplainedNumber __result)
    {
        try
        {
            if (!mobileParty.IsPlayerParty() && SettingsManager.NpcMapSpeedPercentage.IsChanged)
            {
                __result.AddPercentage(SettingsManager.NpcMapSpeedPercentage.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NpcMapSpeedPercentage));
        }
    }
}



[HarmonyPatch]
public static class MapVisibilityMultiplier
{
    private static bool Prepare()
    {
        return AccessTools.TypeByName("TaleWorlds.CampaignSystem.GameComponents.ShokuhoMapVisibilityModel") != null;
    }
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("TaleWorlds.CampaignSystem.GameComponents.ShokuhoMapVisibilityModel");
        return AccessTools.Method(type, "GetPartySpottingRange");
    }


    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetPartySpottingRange(ref MobileParty party, ref bool includeDescriptions, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.MapVisibilityMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.MapVisibilityMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(MapVisibilityMultiplier));
        }
    }
}

