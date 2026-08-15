using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patching;

/// <summary>
/// Optional Naval DLC integration. This file deliberately contains no NavalDLC type reference:
/// UFO therefore remains loadable when the DLC is not installed.
/// </summary>
internal static class NavalDlcCompatibility
{
    private const string NavalAssemblyName = "NavalDLC";

    internal static void Apply(Harmony harmony)
    {
        if (!IsAvailable())
            return;

        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetCampaignSpeedBonusFactor", nameof(CampaignSpeedBonus));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetMaxOarForceFactor", nameof(OarForce));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetSailForceFactor", nameof(SailForce));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetCrewCapacityBonusFactor", nameof(CrewCapacity));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetDefaultCombatFactor", nameof(ShipCombatFactor));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetAdditionalAmmoBonus", nameof(AdditionalAmmo));

        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipDamageModel", "GetHourlyShipDamage", nameof(SeaAttrition));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCCampaignShipDamageModel", "GetShipDamage", nameof(BattleShipDamage));

        Patch(harmony, "NavalDLC.GameComponents.NavalDLCStormModel", "GetPositionDamageForStorm", nameof(StormDamage));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCStormModel", "GetHourlyStormSpawnChanceForPosition", nameof(StormFrequency));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCStormModel", "GetMaximumWeatherStrengthAtEye", nameof(StormStrength));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCStormModel", "GetHourlyIntensityChangeForStorm", nameof(StormStrength));

        Patch(harmony, "NavalDLC.GameComponents.NavalDLCShipCostModel", "GetShipTradeValue", nameof(ShipPurchaseCost));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCShipCostModel", "GetShipRepairCost", nameof(ShipRepairCost));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCShipCostModel", "GetShipUpgradePieceCost", nameof(ShipUpgradeCost));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCShipDeploymentModel", "GetShipDeploymentLimit", nameof(DeploymentLimit));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCFleetManagementModel", "get_MinimumTroopCountRequiredToSendShips", nameof(FleetMinimumTroops));

        Patch(harmony, "NavalDLC.GameComponents.NavalDLCBattleRewardModel", "CalculateRenownGain", nameof(NavalRenown));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCBattleRewardModel", "CalculateInfluenceGain", nameof(NavalInfluence));

        // Existing generic map options target Native models only; apply them to DLC replacements too.
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCPartySpeedCalculationModel", "CalculateFinalSpeed", nameof(ExistingMapSpeed));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCMapVisibilityModel", "GetPartySpottingRange", nameof(ExistingMapVisibility));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCMobilePartyFoodConsumptionModel", "CalculateDailyFoodConsumptionf", nameof(ExistingFoodConsumption));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCPartyWageModel", "GetTotalWage", nameof(ExistingWages));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCPartyHealingModel", "GetDailyHealingHpForHeroes", nameof(ExistingHeroHealing));
        Patch(harmony, "NavalDLC.GameComponents.NavalDLCPartyHealingModel", "GetDailyHealingForRegulars", nameof(ExistingTroopHealing));
    }

    internal static void GrantShip(string shipId) => InvokeCheat("AddShipToPlayer", shipId);

    internal static void UnlockFigurehead(string figureheadId) => InvokeCheat("UnlockFigurehead", figureheadId);

    private static void InvokeCheat(string methodName, string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            InformationManager.DisplayMessage(new InformationMessage(L10N.GetText("NavalDLCIdentifierRequired"), Colors.Red));
            return;
        }

        try
        {
            var type = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("NavalDLC.NavalDLCCheats", false)).FirstOrDefault(t => t != null);
            var method = type?.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                InformationManager.DisplayMessage(new InformationMessage(L10N.GetText("NavalDLCNotInstalled"), Colors.Red));
                return;
            }
            method.Invoke(null, new object[] { id.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() });
        }
        catch (Exception exception)
        {
            SubModule.LogError(exception, typeof(NavalDlcCompatibility));
        }
    }

    private static bool IsAvailable() => AppDomain.CurrentDomain.GetAssemblies().Any(a => string.Equals(a.GetName().Name, NavalAssemblyName, StringComparison.OrdinalIgnoreCase));

    private static void Patch(Harmony harmony, string typeName, string methodName, string callbackName)
    {
        try
        {
            var type = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(typeName, false)).FirstOrDefault(t => t != null);
            var original = type?.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m => m.Name == methodName);
            var callback = typeof(NavalDlcCompatibility).GetMethod(callbackName, BindingFlags.Static | BindingFlags.NonPublic);
            if (original != null && callback != null)
                harmony.Patch(original, postfix: new HarmonyMethod(callback));
        }
        catch (Exception exception)
        {
            SubModule.LogError(exception, typeof(NavalDlcCompatibility));
        }
    }

    private static bool IsPlayerOwned(object value, int depth = 0)
    {
        if (value == null || depth > 3) return false;
        if (value is MobileParty mobileParty) return mobileParty == MobileParty.MainParty || mobileParty.IsPlayerParty();
        if (value is PartyBase partyBase) return partyBase.IsPlayerParty();

        var type = value.GetType();
        foreach (var name in new[] { "Owner", "Party", "MobileParty", "PartyBase", "ShipOrigin" })
        {
            var property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null && IsPlayerOwned(property.GetValue(value), depth + 1)) return true;
        }
        return false;
    }

    private static void CampaignSpeedBonus(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalCampaignSpeedMultiplier.Value; }
    private static void OarForce(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalOarForceMultiplier.Value; }
    private static void SailForce(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalSailForceMultiplier.Value; }
    private static void CrewCapacity(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalCrewCapacityMultiplier.Value; }
    private static void ShipCombatFactor(object shipHull, ref float __result) { __result *= SettingsManager.NavalShipCombatFactorMultiplier.Value; }
    private static void AdditionalAmmo(object ship, ref int __result) { if (IsPlayerOwned(ship)) __result += SettingsManager.NavalAdditionalAmmo.Value; }
    private static void SeaAttrition(object owner, ref int __result) { if (IsPlayerOwned(owner)) __result = (int)Math.Round(__result * SettingsManager.NavalSeaAttritionPercentage.Value / 100f); }
    private static void BattleShipDamage(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalBattleShipDamagePercentage.Value / 100f; }
    private static void StormDamage(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalStormDamagePercentage.Value / 100f; }
    private static void StormFrequency(ref float __result) { __result *= SettingsManager.NavalStormFrequencyMultiplier.Value; }
    private static void StormStrength(ref float __result) { __result *= SettingsManager.NavalStormStrengthMultiplier.Value; }
    private static void ShipPurchaseCost(object buyer, ref float __result) { if (IsPlayerOwned(buyer)) __result *= SettingsManager.NavalShipPurchaseCostMultiplier.Value; }
    private static void ShipRepairCost(object owner, ref float __result) { if (IsPlayerOwned(owner)) __result *= SettingsManager.NavalShipRepairCostMultiplier.Value; }
    private static void ShipUpgradeCost(object owner, ref int __result) { if (IsPlayerOwned(owner)) __result = (int)Math.Round(__result * SettingsManager.NavalShipUpgradeCostMultiplier.Value); }
    private static void DeploymentLimit(object party, ref int __result) { if (IsPlayerOwned(party)) __result = Math.Max(1, (int)Math.Round(__result * SettingsManager.NavalDeploymentLimitMultiplier.Value)); }
    private static void FleetMinimumTroops(ref int __result) { __result = Math.Max(0, (int)Math.Round(__result * SettingsManager.NavalFleetMinimumTroopPercentage.Value / 100f)); }
    private static void NavalRenown(PartyBase winnerParty, ref ExplainedNumber __result) { if (winnerParty.IsPlayerParty()) { __result.AddMultiplier(SettingsManager.NavalBattleRewardMultiplier.Value); if (SettingsManager.RenownRewardMultiplier.IsChanged) __result.AddMultiplier(SettingsManager.RenownRewardMultiplier.Value); } }
    private static void NavalInfluence(PartyBase winnerParty, ref ExplainedNumber __result) { if (winnerParty.IsPlayerParty()) { __result.AddMultiplier(SettingsManager.NavalBattleRewardMultiplier.Value); if (SettingsManager.InfluenceRewardMultiplier.IsChanged) __result.AddMultiplier(SettingsManager.InfluenceRewardMultiplier.Value); } }
    private static void ExistingMapSpeed(MobileParty mobileParty, ref ExplainedNumber __result) { if (mobileParty.IsPlayerParty() && SettingsManager.MapSpeedMultiplier.IsChanged) __result.AddMultiplier(SettingsManager.MapSpeedMultiplier.Value); }
    private static void ExistingMapVisibility(MobileParty party, ref ExplainedNumber __result) { if (party.IsPlayerParty() && SettingsManager.MapVisibilityMultiplier.IsChanged) __result.AddMultiplier(SettingsManager.MapVisibilityMultiplier.Value); }
    private static void ExistingFoodConsumption(MobileParty party, ref ExplainedNumber __result) { if (party.IsPlayerParty() && SettingsManager.FoodConsumptionPercentage.IsChanged) __result.AddPercentage(SettingsManager.FoodConsumptionPercentage.Value); }
    private static void ExistingWages(MobileParty mobileParty, ref ExplainedNumber __result) { if (mobileParty != null && mobileParty.IsPlayerParty() && SettingsManager.TroopWagesPercentage.IsChanged) __result.AddPercentage(SettingsManager.TroopWagesPercentage.Value); }
    private static void ExistingHeroHealing(PartyBase partyBase, ref ExplainedNumber __result) { if (partyBase.IsPlayerParty() && SettingsManager.PartyHealingMultiplier.IsChanged) __result.AddMultiplier(SettingsManager.PartyHealingMultiplier.Value); }
    private static void ExistingTroopHealing(PartyBase partyBase, ref ExplainedNumber __result) { if (partyBase.IsPlayerParty() && SettingsManager.PartyHealingMultiplier.IsChanged) __result.AddMultiplier(SettingsManager.PartyHealingMultiplier.Value); }
}
