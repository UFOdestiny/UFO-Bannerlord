using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch;



[HarmonyPatch(typeof(DefaultPartySpeedCalculatingModel), "CalculateFinalSpeed")]
public static class MapSpeedMultiplier
{
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


[HarmonyPatch(typeof(DefaultMapVisibilityModel), "GetPartySpottingRange")]
public static class MapVisibilityMultiplier
{
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


[HarmonyPatch(typeof(DefaultPartySpeedCalculatingModel), "CalculateFinalSpeed")]
public static class NpcMapSpeedPercentage
{
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



[HarmonyPatch(typeof(MobileParty), "ShouldBeIgnored", MethodType.Getter)]
public static class PartyInvisibleOnMap
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void ShouldBeIgnored(ref MobileParty __instance, ref bool __result)
    {
        try
        {
            if (__instance.IsPlayerParty() && SettingsManager.PartyInvisibleOnMap.Value)
            {
                __result = true;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyInvisibleOnMap));
        }
    }
}

[HarmonyPatch(typeof(MobileParty), "ShouldBeIgnored", MethodType.Getter)]
public static class CaravansInvisibleOnMap
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void ShouldBeIgnored(ref MobileParty __instance, ref bool __result)
    {
        try
        {
            if (__instance.IsCaravan && __instance.Owner.IsPlayer() && SettingsManager.CaravansInvisibleOnMap.Value)
            {
                __result = true;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CaravansInvisibleOnMap));
        }
    }
}

