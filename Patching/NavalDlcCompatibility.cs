using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;
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

    internal static IReadOnlyList<string> Apply(Harmony harmony)
    {
        var failures = new List<string>();
        if (!IsAvailable()) return failures;
        foreach (var patchType in typeof(NavalDlcCompatibility).Assembly.GetTypes())
        foreach (var callback in patchType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
        foreach (OptionalPatchAttribute target in callback.GetCustomAttributes(typeof(OptionalPatchAttribute), false))
        {
            try
            {
                var type = FindLoadedType(target.TypeName);
                var candidates = type?.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(method => method.Name == target.MethodName &&
                        (target.FirstArgumentType == null || method.GetParameters().FirstOrDefault()?.ParameterType == target.FirstArgumentType)).ToArray();
                if (candidates == null || candidates.Length != 1)
                    throw new MissingMethodException(target.TypeName, target.MethodName);
                harmony.Patch(candidates[0], postfix: new HarmonyMethod(callback));
            }
            catch (Exception exception)
            {
                failures.Add(target.TypeName + "." + target.MethodName);
                SubModule.LogError(exception, typeof(NavalDlcCompatibility));
            }
        }
        return failures;
    }

    internal static void GrantShip(string shipId) => InvokeCheat("AddShipToPlayer", shipId);

    internal static void UnlockFigurehead(string figureheadId) => InvokeCheat("UnlockFigurehead", figureheadId);

    internal static void GrantAllShips()
    {
        var shipIds = GetCampaignObjects<ShipHull>()
            .Select(hull => hull.StringId)
            .Where(IsPlayableShipId)
            .ToList();
        InvokeCheat("AddShipToPlayer", shipIds);
    }

    internal static void UnlockAllFigureheads()
    {
        var type = typeof(DefaultFigureheads);
        var instance = DefaultFigureheads.Instance;
        var figureheadIds = type?.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.PropertyType == typeof(Figurehead))
            .Select(property => (instance == null ? null : property.GetValue(instance) as Figurehead)?.StringId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToList() ?? new List<string>();
        InvokeCheat("UnlockFigurehead", figureheadIds);
    }

    internal static void AddShipUpgradePieces(string upgradePieceIds) => AddShipUpgradePieces(ParseIdentifiers(upgradePieceIds));

    internal static void AddAllShipUpgradePieces()
    {
        var piecesBySlot = GetCampaignObjects<ShipUpgradePiece>()
            .Select(piece => new { Piece = piece, Id = piece.StringId, Value = GetUpgradePieceValue(piece) })
            .SelectMany(entry => GetTargetSlotIds(entry.Piece).Select(slotId => new { slotId, entry.Id, entry.Value }))
            .Where(entry => !string.IsNullOrWhiteSpace(entry.slotId) && !string.IsNullOrWhiteSpace(entry.Id))
            .GroupBy(entry => entry.slotId)
            .ToDictionary(group => group.Key, group => group.OrderByDescending(entry => entry.Value).First().Id);
        AddUpgradePiecesToPlayerShips(piecesBySlot);
    }

    private static void AddShipUpgradePieces(IEnumerable<string> requestedIds)
    {
        var ids = new HashSet<string>(requestedIds, StringComparer.OrdinalIgnoreCase);
        if (ids.Count == 0)
        {
            DisplayIdentifierRequired();
            return;
        }

        var piecesBySlot = GetCampaignObjects<ShipUpgradePiece>()
            .Where(piece => ids.Contains(piece.StringId))
            .SelectMany(piece => GetTargetSlotIds(piece).Select(slotId => new { slotId, id = piece.StringId }))
            .Where(entry => !string.IsNullOrWhiteSpace(entry.slotId) && !string.IsNullOrWhiteSpace(entry.id))
            .GroupBy(entry => entry.slotId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Last().id, StringComparer.OrdinalIgnoreCase);
        AddUpgradePiecesToPlayerShips(piecesBySlot);
    }

    private static void AddUpgradePiecesToPlayerShips(Dictionary<string, string> piecesBySlot)
    {
        if (piecesBySlot.Count == 0)
        {
            DisplayIdentifierRequired();
            return;
        }

        try
        {
            var helpers = FindLoadedType("NavalDLC.NavalDLCHelpers");
            var method = helpers?.GetMethod("AddUpgradePiecesToPartyShips", BindingFlags.Public | BindingFlags.Static);
            if (method == null || MobileParty.MainParty == null)
            {
                DisplayDlcUnavailable();
                return;
            }
            method.Invoke(null, new object[] { MobileParty.MainParty, piecesBySlot, null });
        }
        catch (Exception exception)
        {
            SubModule.LogError(exception, typeof(NavalDlcCompatibility));
        }
    }

    private static void InvokeCheat(string methodName, string id) => InvokeCheat(methodName, ParseIdentifiers(id));

    private static void InvokeCheat(string methodName, IEnumerable<string> ids)
    {
        var identifiers = ids.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        if (identifiers.Count == 0)
        {
            DisplayIdentifierRequired();
            return;
        }

        try
        {
            var type = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("NavalDLC.NavalDLCCheats", false)).FirstOrDefault(t => t != null);
            var method = type?.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                DisplayDlcUnavailable();
                return;
            }
            method.Invoke(null, new object[] { identifiers });
        }
        catch (Exception exception)
        {
            SubModule.LogError(exception, typeof(NavalDlcCompatibility));
        }
    }

    private static IEnumerable<T> GetCampaignObjects<T>() where T : MBObjectBase
    {
        return (IEnumerable<T>)MBObjectManager.Instance?.GetObjectTypeList<T>() ?? Enumerable.Empty<T>();
    }

    private static IEnumerable<string> GetTargetSlotIds(ShipUpgradePiece piece) => piece.TargetSlots.Select(slot => slot.StringId);

    private static int GetUpgradePieceValue(ShipUpgradePiece piece) => piece.LightValue + piece.MediumValue + piece.HeavyValue;

    private static Type FindLoadedType(string typeName) => AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(typeName, false)).FirstOrDefault(type => type != null);

    private static bool IsPlayableShipId(string id) => !string.IsNullOrWhiteSpace(id) &&
        id.IndexOf("storyline", StringComparison.OrdinalIgnoreCase) < 0 &&
        id.IndexOf("quest", StringComparison.OrdinalIgnoreCase) < 0 &&
        id.IndexOf("burning", StringComparison.OrdinalIgnoreCase) < 0 &&
        id.IndexOf("nested", StringComparison.OrdinalIgnoreCase) < 0;

    private static IEnumerable<string> ParseIdentifiers(string identifiers) => (identifiers ?? string.Empty).Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

    private static void DisplayIdentifierRequired() => InformationManager.DisplayMessage(new InformationMessage(L10N.GetText("NavalDLCIdentifierRequired"), Colors.Red));

    private static void DisplayDlcUnavailable() => InformationManager.DisplayMessage(new InformationMessage(L10N.GetText("NavalDLCNotInstalled"), Colors.Red));

    private static bool IsAvailable() => AppDomain.CurrentDomain.GetAssemblies().Any(a => string.Equals(a.GetName().Name, NavalAssemblyName, StringComparison.OrdinalIgnoreCase));

    private static bool IsPlayerOwned(object value)
    {
        if (value is MobileParty mobileParty) return mobileParty == MobileParty.MainParty || mobileParty.IsPlayerParty();
        if (value is PartyBase partyBase) return partyBase.IsPlayerParty();
        return value is Ship ship && ship.Owner.IsPlayerParty();
    }

    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetCampaignSpeedBonusFactor")]
    private static void CampaignSpeedBonus(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalCampaignSpeedMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetMaxOarForceFactor")]
    private static void OarForce(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalOarForceMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetSailForceFactor")]
    private static void SailForce(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalSailForceMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetCrewCapacityBonusFactor")]
    private static void CrewCapacity(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalCrewCapacityMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetDefaultCombatFactor")]
    private static void ShipCombatFactor(object shipHull, ref float __result) { __result *= SettingsManager.NavalShipCombatFactorMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel", "GetAdditionalAmmoBonus")]
    private static void AdditionalAmmo(object ship, ref int __result) { if (IsPlayerOwned(ship)) __result += SettingsManager.NavalAdditionalAmmo.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipDamageModel", "GetHourlyShipDamage")]
    private static void SeaAttrition(object owner, ref int __result) { if (IsPlayerOwned(owner)) __result = (int)Math.Round(__result * SettingsManager.NavalSeaAttritionPercentage.Value / 100f); }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCampaignShipDamageModel", "GetShipDamage")]
    private static void BattleShipDamage(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalBattleShipDamagePercentage.Value / 100f; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCStormModel", "GetPositionDamageForStorm")]
    private static void StormDamage(object ship, ref float __result) { if (IsPlayerOwned(ship)) __result *= SettingsManager.NavalStormDamagePercentage.Value / 100f; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCStormModel", "GetHourlyStormSpawnChanceForPosition")]
    private static void StormFrequency(ref float __result) { __result *= SettingsManager.NavalStormFrequencyMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCStormModel", "GetMaximumWeatherStrengthAtEye")]
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCStormModel", "GetHourlyIntensityChangeForStorm")]
    private static void StormStrength(ref float __result) { __result *= SettingsManager.NavalStormStrengthMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCShipCostModel", "GetShipTradeValue")]
    private static void ShipPurchaseCost(object buyer, ref float __result) { if (IsPlayerOwned(buyer)) __result *= SettingsManager.NavalShipPurchaseCostMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCShipCostModel", "GetShipRepairCost")]
    private static void ShipRepairCost(object owner, ref float __result) { if (IsPlayerOwned(owner)) __result *= SettingsManager.NavalShipRepairCostMultiplier.Value; }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCShipCostModel", "GetShipUpgradePieceCost")]
    private static void ShipUpgradeCost(object owner, ref int __result) { if (IsPlayerOwned(owner)) __result = (int)Math.Round(__result * SettingsManager.NavalShipUpgradeCostMultiplier.Value); }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCShipDeploymentModel", "GetShipDeploymentLimit")]
    private static void DeploymentLimit(object party, ref int __result) { if (IsPlayerOwned(party)) __result = Math.Max(1, (int)Math.Round(__result * SettingsManager.NavalDeploymentLimitMultiplier.Value)); }
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCFleetManagementModel", "get_MinimumTroopCountRequiredToSendShips")]
    private static void FleetMinimumTroops(ref int __result) { __result = Math.Max(0, (int)Math.Round(__result * SettingsManager.NavalFleetMinimumTroopPercentage.Value / 100f)); }
    // DLC model wrappers call the patched BaseModel. Apply only the extra naval
    // reward here; global renown/influence and generic map/economy bonuses run once.
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCBattleRewardModel", "CalculateRenownGain")]
    private static void NavalRenown(PartyBase winnerParty, ref ExplainedNumber __result) => ScaleNavalReward(winnerParty, ref __result);
    [OptionalPatch("NavalDLC.GameComponents.NavalDLCBattleRewardModel", "CalculateInfluenceGain")]
    private static void NavalInfluence(PartyBase winnerParty, ref ExplainedNumber __result) => ScaleNavalReward(winnerParty, ref __result);

    private static void ScaleNavalReward(PartyBase winnerParty, ref ExplainedNumber reward)
    {
        if (!winnerParty.IsPlayerParty() || winnerParty.MapEvent?.IsNavalMapEvent != true || !SettingsManager.NavalBattleRewardMultiplier.IsChanged) return;
        reward.AddFactor((SettingsManager.NavalBattleRewardMultiplier.Value - 1f) * (1f + reward.SumOfFactors));
    }

    [OptionalPatch("NavalDLC.GameComponents.NavalDLCCombatSimulationModel", "SimulateHit", typeof(Ship))]
    private static void NavalSimulationProtection(PartyBase struckParty, ref int troopCasualties, ref ExplainedNumber __result)
    {
        if (!struckParty.IsPlayerParty() || !SettingsManager.AlwaysWinBattleSimulation.IsChanged) return;
        __result = new ExplainedNumber(0f);
        troopCasualties = 0;
    }
}
