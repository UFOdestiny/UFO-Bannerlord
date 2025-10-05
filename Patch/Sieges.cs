using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using UFO;
using UFO.Extension;
using UFO.Setting;
namespace UFO.Patch;


[HarmonyPatch(typeof(DefaultSiegeEventModel), "GetConstructionProgressPerHour")]
public static class EnemySiegeBuildingSpeedPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetConstructionProgressPerHour(ref SiegeEngineType type, ref SiegeEvent siegeEvent, ref ISiegeEventSide side, ref float __result)
    {
        try
        {
            BattleSideEnum side2;
            switch (side.BattleSide)
            {
                default:
                    return;
                case BattleSideEnum.Attacker:
                    side2 = BattleSideEnum.Defender;
                    break;
                case BattleSideEnum.Defender:
                    side2 = BattleSideEnum.Attacker;
                    break;
            }
            ISiegeEventSide siegeEventSide = siegeEvent.GetSiegeEventSide(side2);
            if (siegeEventSide != null && siegeEventSide.GetInvolvedPartiesForEventType().Any((PartyBase x) => x.IsPlayerParty()) && SettingsManager.EnemySiegeBuildingSpeedPercentage.IsChanged)
            {
                float num = SettingsManager.EnemySiegeBuildingSpeedPercentage.Value / 100f;
                float num2 = num * __result;
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnemySiegeBuildingSpeedPercentage));
        }
    }
}



[HarmonyPatch(typeof(DefaultSiegeEventModel), "GetConstructionProgressPerHour")]
public static class SiegeBuildingSpeedMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side, ref float __result)
    {
        try
        {
            if (side.IsPlayerSide() && SettingsManager.SiegeBuildingSpeedMultiplier.IsChanged)
            {
                __result *= SettingsManager.SiegeBuildingSpeedMultiplier.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SiegeBuildingSpeedMultiplier));
        }
    }
}