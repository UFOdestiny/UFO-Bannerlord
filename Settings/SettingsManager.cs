using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Common;
using System;
using System.Collections.Generic;
using UFO.Extension;
using UFO.Localization;

namespace UFO.Setting;

public static class SettingsManager
{
    public struct CheatValue<T>
    {
        public bool IsChanged { get; }
        public T Value { get; }

        public CheatValue(bool isChanged, T value)
        {
            IsChanged = isChanged;
            Value = value;
        }
    }

    public static class Default
    {

        // UFO's
        public const bool PlayerAlwaysCrush = false;
        public const bool PlayerPartyCrush = false;
        public const bool EnemyCrush = false;


        public const bool KeepDaughter = false;

        public const Setting_Language LanguageSetting = Setting_Language.English;

        public const int AddMoneyThreshhold =0;
        public const int AddMoney_count = 0;

        public const int Village_Init_Gold_Extra = 0;
        public const int Town_Init_Gold_Extra = 0;

        public const int MaxAttr = 10;

        public const int xGang = 0;
        public const int xArt = 0;
        public const int xMerch = 0;
        public const int xVill = 0;
        public const int xRural = 0;


        public const bool UnblockableThrust_player = false;
        public const bool UnblockableThrust_ally = false;
        public const bool UnblockableThrust_enemy = false;


        // Cheat
        public const bool EnableHotkeys = false;

        public const bool EnableHotkeyTips = false;

        public const float MapSpeedMultiplier = 1f;

        public const float MapVisibilityMultiplier = 1f;

        public const float NpcMapSpeedPercentage = 100f;

        public const bool PartyInvisibleOnMap = false;

        public const bool CaravansInvisibleOnMap = false;

        public const float DamageTakenPercentage = 100f;

        public const bool Invincible = false;

        public const bool PlayerHorseInvincible = false;

        public const bool OneHitKill = false;


        public const bool SliceThroughEveryone = false;
        public const float PlayerMomentumDecayMultiplier = 1f;
        public const bool PlayerPartySliceThroughEveryone = false;
        public const bool SliceThroughEveryone_enemy = false;

        public const float HealthRegeneration = 0f;

        public const bool InfiniteAmmo = false;

        public const float DamageMultiplier = 1f;

        public const bool AlwaysKnockDown = false;

        public const bool NeverKnockedBackByAttacks = false;

        public const bool NoStuckArrows = false;

        public const bool InstantCrossbowReload = false;

        public const KnockoutOrKilled PartyKnockoutOrKilled = KnockoutOrKilled.Default;

        public const KnockoutOrKilled CompanionsKnockoutOrKilled = KnockoutOrKilled.Default;

        public const bool PartyInvincible = false;

        public const bool PartyHeroesInvincible = false;

        public const bool PartyOneHitKill = false;


        public const bool NoRunningAway = false;

        public const float PartyHealthRegeneration = 0f;

        public const bool PartyInfiniteAmmo = false;

        public const float PartyDamageMultiplier = 1f;

        public const bool NoFriendlyFire = false;


        public const KnockoutOrKilled FriendlyLordsKnockoutOrKilled = KnockoutOrKilled.Default;


        public const KnockoutOrKilled EnemyLordsKnockoutOrKilled = KnockoutOrKilled.Default;

        public const KnockoutOrKilled EnemyTroopsKnockoutOrKilled = KnockoutOrKilled.Default;


        public const bool EnemiesNoRunningAway = false;

        public const float EnemyDamagePercentage = 100f;



        public const float RenownRewardMultiplier = 1f;

        public const float InfluenceRewardMultiplier = 1f;

        public const bool AlwaysWinBattleSimulation = false;

        public const bool NoTroopSacrifice = false;

        public const int BanditHideoutTroopLimit = 0;

        public const float CombatZoomMultiplier = 1f;

        public const int ExtraInventoryCapacity = 0;

        public const bool NativeItemSpawning = false;

        public const int ExtraPartyMemberSize = 0;

        public const int ExtraPartyPrisonerSize = 0;

        public const int ExtraPartyMorale = 0;

        public const bool InstantEscape = false;

        public const float FoodConsumptionPercentage = 100f;

        public const float TroopWagesPercentage = 100f;

        public const bool FreeTroopUpgrades = false;

        public const bool FreeCompanionHiring = false;

        public const bool InstantPrisonerRecruitment = false;

        public const bool NoPrisonerEscape = false;

        public const float PartyHealingMultiplier = 1f;

        public const int ExtraCompanionLimit = 0;

        public const int ExtraClanPartyLimit = 0;

        public const int ExtraClanPartySize = 0;

        public const float RelationGainAfterBattleMultiplier = 1f;

        public const bool PerfectRelationships = false;

        public const bool NeverDieOfOldAge = false;

        public const bool BarterOfferAlwaysAccepted = false;

        public const bool NoBarterCooldown = false;

        public const bool ConversationAlwaysSuccessful = false;

        public const bool PerfectAttraction = false;

        public const bool AllowSameSexMarriage = false;

        public const float PregnancyChanceMultiplier = 1f;

        public const int AdjustPregnancyDuration = 36;

        public const float KingdomDecisionWeightMultiplier = 1f;

        public const bool NoRelationshipLossOnDecision = false;

        public const bool NoCrimeRatingForCrimes = false;

        public const bool RecruitExileClans = true;

        public const float DecisionOverrideInfluenceCostPercentage = 100f;

        public const float ExperienceMultiplier = 1f;

        public const float CompanionExperienceMultiplier = 1f;

        public const float ClanExperienceMultiplier = 1f;

        public const float LearningRateMultiplier = 1f;

        public const float CompanionLearningRateMultiplier = 1f;

        public const float LearningLimitMultiplier = 1f;

        public const float TroopExperienceMultiplier = 1f;

        public const bool FreeFocusPointAssignment = false;

        public const float SiegeBuildingSpeedMultiplier = 1f;

        public const float EnemySiegeBuildingSpeedPercentage = 100f;

        public const float FactionArmyCohesionLossPercentage = 100f;

        public const float ArmyCohesionLossPercentage = 100f;

        public const float ArmyFoodConsumptionPercentage = 100f;

        public const bool VillagesNeverRaided = false;

        public const bool DisguiseAlwaysWorks = false;

        public const bool FreeTroopRecruitment = false;

        public const float ItemTradingCostPercentage = 100f;

        public const float SellingPriceMultiplier = 1f;

        public const float TournamentMaximumBetMultiplier = 1f;

        public const int DailyFoodBonus = 0;

        public const int DailyGarrisonBonus = 0;

        public const int DailyMilitiaBonus = 0;

        public const int DailyProsperityBonus = 0;

        public const int DailyLoyaltyBonus = 0;

        public const int DailySecurityBonus = 0;

        public const int DailyHearthsBonus = 0;

        public const float GarrisonWagesPercentage = 100f;

        public const bool NeverRequireCivilianEquipment = false;

        public const float ConstructionPowerMultiplier = 1f;

        public const bool NoBribeToEnterKeep = false;

        public const bool SettlementsNeverRebel = false;

        public const float SmithingEnergyCostPercentage = 100f;

        public const bool UnlockAllParts = false;

        public const float SmithingDifficultyPercentage = 100f;

        public const float SmithingCostPercentage = 100f;

        public const int CraftedWeaponHandlingBonus = 0;

        public const int CraftedWeaponSwingDamageBonus = 0;

        public const int CraftedWeaponSwingSpeedBonus = 0;

        public const int CraftedWeaponThrustDamageBonus = 0;

        public const int CraftedWeaponThrustSpeedBonus = 0;

        public const float WorkshopBuyingCostPercentage = 100f;

        public const float WorkshopDailyExpensePercentage = 100f;

        public const float WorkshopSellingCostMultiplier = 1f;


        // Hero Enhance Settings

        public const bool EnableEverYoung = false;

        public const int EverYoungSkillNeed = 400;

        public const AutoChoosePerk_Type AutoChoosePerk = AutoChoosePerk_Type.No;

        public const float VigorDmgPercent = 0.02f;

        public const float VigorArmorAdd = 1f;

        public const float VigorShieldEndurancePercent = 1f;

        public const float VigorFinalDmgAdd = 0.334f;

        public const float VigorDmgTakenReduce = 0.334f;

        public const int VigorCrushThroughPositive = 5;

        public const int VigorCrushThroughNegative = 10;

        public const float IntelligenceAmmoAddPercent = 0.1f;

        public const int ControlAmmoNoConsumeRate = 5;

        public const float ControlDropDmgReducePercent = 0.05f;

        public const float ControlAimStabilityPercent = 0.1f;

        public const float ControlMountManeuverPercent = 0.05f;

        public const int ControlCritRate = 2;

        public const int ControlExemptionRate = 2;

        public const int ControlPenetrateRate = 3;

        public const float EnduranceHpAddPercent = 0.05f;

        public const float EnduranceHealRate = 0.05f;

        public const float EnduranceStaggerPercent = 0.2f;

        public const float EnduranceWalkSpeedPercent = 0.01f;

        public const float EnduranceMountSpeedPercent = 0.025f;

        public const float CunningPrisonerRecruitSpeedPercent = 0.1f;

        public const float CunningPrisonerCapacityPercent = 0.1f;

        public const float CunningRaidSpeedPercent = 0.1f;

        public const float CunningPartySpeedAdd = 0.1f;

        public const float CunningCompanionCapacityAdd = 0.2f;

        public const float SocialBoundary = 3.5f;

        public const float SocialHearthAdd = 0.25f;

        public const float SocialSettlementLoyaltyAdd = 0.25f;

        public const float SocialMilitiaAdd = 0.5f;

        public const float SocialRecruitSpeedPercent = 0.05f;

        public const float SocialTaxPercent = 0.05f;

        public const float SocialWorkshopProductionPercent = 0.1f;

        public const float SocialCompanionCapacityAdd = 0.2f;

        public const float IntelligenceBoundary = 3.5f;

        public const float IntelligenceExpRate = 0.05f;

        public const float IntelligenceSiegeEndurancePercent = 0.1f;

        public const float IntelligenceWallEndurancePercent = 0.1f;

        public const float IntelligenceBallistaAdd = 0.334f;

        public const float IntelligenceLeaderSettlementFoodPercent = 0.5f;

        public const float IntelligenceGovernorSettlementFoodPercent = 1f;

        public const float IntelligenceProsperityFoodCostReducePercent = 0.075f;

        public const float IntelligenceGarrisonWageReducePercent = 0.05f;

        public const float IntelligenceWorkshopProductionPercent = 0.25f;

        public const float CombatAttributeRatePlayer = 0f;

        public const float CombatAttributeRateClanMember = 0f;

        public const float CombatAttributeRateOther = 0f;

        public const float StrategyAttributeRatePlayer = 0f;

        public const float StrategyAttributeRateClanMember = 0f;

        public const float StrategyAttributeRateOther = 0f;

        public const bool TestMode = false;

    }

    private static bool IsPerCampaignInstanceLoaded => PerCampaignSettings<BannerlordCheatsPerCampaignSettings>.Instance != null;

    private static BannerlordCheatsGlobalSettings GlobalInstance => GlobalSettings<BannerlordCheatsGlobalSettings>.Instance ?? throw new InvalidOperationException("Should have checked if global instance is loaded!");

    private static BannerlordCheatsPerCampaignSettings PerCampaignInstance => PerCampaignSettings<BannerlordCheatsPerCampaignSettings>.Instance ?? throw new InvalidOperationException("Should have checked if per-campaign instance is loaded!");

    private static CheatValue<T> GetValue<T>(Func<BannerlordCheatsPerCampaignSettings, T> perCampaignGetter,
                                              Func<BannerlordCheatsGlobalSettings, T> globalGetter,
                                              T defaultValue)
    {
        if (IsPerCampaignInstanceLoaded)
        {
            var campaignValue = perCampaignGetter(PerCampaignInstance);
            if (!EqualityComparer<T>.Default.Equals(campaignValue, defaultValue))
                return new CheatValue<T>(true, campaignValue);
        }

        var globalValue = globalGetter(GlobalInstance);
        return !EqualityComparer<T>.Default.Equals(globalValue, defaultValue)
            ? new CheatValue<T>(true, globalValue)
            : new CheatValue<T>(false, defaultValue);
    }

    private static CheatValue<bool> GetValue(Func<BannerlordCheatsPerCampaignSettings, bool> perCampaignGetter,
                                              Func<BannerlordCheatsGlobalSettings, bool> globalGetter) =>
        GetValue(perCampaignGetter, globalGetter, false);

    private static CheatValue<T> GetDropdownValue<T>(Func<BannerlordCheatsPerCampaignSettings, Dropdown<LocalizedDropdownValue<T>>> perCampaignGetter,
                                                      Func<BannerlordCheatsGlobalSettings, Dropdown<LocalizedDropdownValue<T>>> globalGetter,
                                                      T defaultValue) where T : struct, Enum
    {
        if (IsPerCampaignInstanceLoaded && perCampaignGetter(PerCampaignInstance).GetValue().CompareTo(defaultValue) != 0)
            return new CheatValue<T>(true, perCampaignGetter(PerCampaignInstance).GetValue());
        
        if (globalGetter(GlobalInstance).GetValue().CompareTo(defaultValue) != 0)
            return new CheatValue<T>(true, globalGetter(GlobalInstance).GetValue());
        
        return new CheatValue<T>(false, defaultValue);
    }

    // Public properties using the helper methods
    public static CheatValue<bool> EnableHotkeys => 
        GetValue(s => s.EnableHotkeys, s => s.EnableHotkeys);

    public static CheatValue<bool> EnableHotkeyTips => 
        GetValue(s => s.EnableHotkeyTips, s => s.EnableHotkeyTips);

    public static CheatValue<float> MapSpeedMultiplier => 
        GetValue(s => s.MapSpeedMultiplier, s => s.MapSpeedMultiplier, 1f);

    public static CheatValue<float> MapVisibilityMultiplier => 
        GetValue(s => s.MapVisibilityMultiplier, s => s.MapVisibilityMultiplier, 1f);

    public static CheatValue<float> NpcMapSpeedPercentage => 
        GetValue(s => s.NpcMapSpeedPercentage, s => s.NpcMapSpeedPercentage, 100f);

    public static CheatValue<bool> PartyInvisibleOnMap => 
        GetValue(s => s.PartyInvisibleOnMap, s => s.PartyInvisibleOnMap);

    public static CheatValue<bool> CaravansInvisibleOnMap => 
        GetValue(s => s.CaravansInvisibleOnMap, s => s.CaravansInvisibleOnMap);

    public static CheatValue<float> DamageTakenPercentage => 
        GetValue(s => s.DamageTakenPercentage, s => s.DamageTakenPercentage, 100f);

    public static CheatValue<bool> Invincible => 
        GetValue(s => s.Invincible, s => s.Invincible);

    public static CheatValue<bool> PlayerHorseInvincible => 
        GetValue(s => s.PlayerHorseInvincible, s => s.PlayerHorseInvincible);

    public static CheatValue<bool> OneHitKill => 
        GetValue(s => s.OneHitKill, s => s.OneHitKill);

    public static CheatValue<bool> SliceThroughEveryone => 
        GetValue(s => s.SliceThroughEveryone, s => s.SliceThroughEveryone);

    public static CheatValue<float> PlayerMomentumDecayMultiplier =>
        GetValue(s => s.PlayerMomentumDecayMultiplier, s => s.PlayerMomentumDecayMultiplier, 1f);

    public static CheatValue<bool> PlayerPartySliceThroughEveryone =>
        GetValue(s => s.PlayerPartySliceThroughEveryone, s => s.PlayerPartySliceThroughEveryone);

    public static CheatValue<bool> SliceThroughEveryone_enemy => 
        GetValue(s => s.SliceThroughEveryone_enemy, s => s.SliceThroughEveryone_enemy);

    public static CheatValue<float> HealthRegeneration => 
        GetValue(s => s.HealthRegeneration, s => s.HealthRegeneration, 0f);

    public static CheatValue<bool> InfiniteAmmo => 
        GetValue(s => s.InfiniteAmmo, s => s.InfiniteAmmo);

    public static CheatValue<float> DamageMultiplier => 
        GetValue(s => s.DamageMultiplier, s => s.DamageMultiplier, 1f);

    public static CheatValue<bool> AlwaysKnockDown => 
        GetValue(s => s.AlwaysKnockDown, s => s.AlwaysKnockDown);

    public static CheatValue<bool> NeverKnockedBackByAttacks => 
        GetValue(s => s.NeverKnockedBackByAttacks, s => s.NeverKnockedBackByAttacks);

    public static CheatValue<bool> NoStuckArrows => 
        GetValue(s => s.NoStuckArrows, s => s.NoStuckArrows);

    public static CheatValue<bool> InstantCrossbowReload => 
        GetValue(s => s.InstantCrossbowReload, s => s.InstantCrossbowReload);

    public static CheatValue<KnockoutOrKilled> PartyKnockoutOrKilled => 
        GetDropdownValue(s => s.PartyKnockoutOrKilled, s => s.PartyKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<KnockoutOrKilled> CompanionsKnockoutOrKilled => 
        GetDropdownValue(s => s.CompanionsKnockoutOrKilled, s => s.CompanionsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<bool> PartyInvincible => 
        GetValue(s => s.PartyInvincible, s => s.PartyInvincible);

    public static CheatValue<bool> PartyHeroesInvincible => 
        GetValue(s => s.PartyHeroesInvincible, s => s.PartyHeroesInvincible);

    public static CheatValue<bool> PartyOneHitKill => 
        GetValue(s => s.PartyOneHitKill, s => s.PartyOneHitKill);

    public static CheatValue<bool> NoRunningAway => 
        GetValue(s => s.NoRunningAway, s => s.NoRunningAway);

    public static CheatValue<float> PartyHealthRegeneration => 
        GetValue(s => s.PartyHealthRegeneration, s => s.PartyHealthRegeneration, 0f);

    public static CheatValue<bool> PartyInfiniteAmmo => 
        GetValue(s => s.PartyInfiniteAmmo, s => s.PartyInfiniteAmmo);

    public static CheatValue<float> PartyDamageMultiplier => 
        GetValue(s => s.PartyDamageMultiplier, s => s.PartyDamageMultiplier, 1f);

    public static CheatValue<bool> NoFriendlyFire => 
        GetValue(s => s.NoFriendlyFire, s => s.NoFriendlyFire);

    public static CheatValue<KnockoutOrKilled> FriendlyLordsKnockoutOrKilled => 
        GetDropdownValue(s => s.FriendlyLordsKnockoutOrKilled, s => s.FriendlyLordsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<KnockoutOrKilled> EnemyLordsKnockoutOrKilled => 
        GetDropdownValue(s => s.EnemyLordsKnockoutOrKilled, s => s.EnemyLordsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<KnockoutOrKilled> EnemyTroopsKnockoutOrKilled => 
        GetDropdownValue(s => s.EnemyTroopsKnockoutOrKilled, s => s.EnemyTroopsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<bool> EnemiesNoRunningAway => 
        GetValue(s => s.EnemiesNoRunningAway, s => s.EnemiesNoRunningAway);

    public static CheatValue<float> EnemyDamagePercentage => 
        GetValue(s => s.EnemyDamagePercentage, s => s.EnemyDamagePercentage, 100f);

    public static CheatValue<float> RenownRewardMultiplier => 
        GetValue(s => s.RenownRewardMultiplier, s => s.RenownRewardMultiplier, 1f);

    public static CheatValue<float> InfluenceRewardMultiplier => 
        GetValue(s => s.InfluenceRewardMultiplier, s => s.InfluenceRewardMultiplier, 1f);

    public static CheatValue<bool> AlwaysWinBattleSimulation => 
        GetValue(s => s.AlwaysWinBattleSimulation, s => s.AlwaysWinBattleSimulation);

    public static CheatValue<bool> NoTroopSacrifice => 
        GetValue(s => s.NoTroopSacrifice, s => s.NoTroopSacrifice);

    public static CheatValue<int> BanditHideoutTroopLimit => 
        GetValue(s => s.BanditHideoutTroopLimit, s => s.BanditHideoutTroopLimit, 0);

    public static CheatValue<float> CombatZoomMultiplier => 
        GetValue(s => s.CombatZoomMultiplier, s => s.CombatZoomMultiplier, 1f);

    public static CheatValue<int> ExtraInventoryCapacity => 
        GetValue(s => s.ExtraInventoryCapacity, s => s.ExtraInventoryCapacity, 0);

    public static CheatValue<bool> NativeItemSpawning => 
        GetValue(s => s.NativeItemSpawning, s => s.NativeItemSpawning);

    public static CheatValue<int> ExtraPartyMemberSize => 
        GetValue(s => s.ExtraPartyMemberSize, s => s.ExtraPartyMemberSize, 0);

    public static CheatValue<int> ExtraPartyPrisonerSize => 
        GetValue(s => s.ExtraPartyPrisonerSize, s => s.ExtraPartyPrisonerSize, 0);

    public static CheatValue<int> ExtraPartyMorale => 
        GetValue(s => s.ExtraPartyMorale, s => s.ExtraPartyMorale, 0);

    public static CheatValue<bool> InstantEscape => 
        GetValue(s => s.InstantEscape, s => s.InstantEscape);

    public static CheatValue<float> FoodConsumptionPercentage => 
        GetValue(s => s.FoodConsumptionPercentage, s => s.FoodConsumptionPercentage, 100f);

    public static CheatValue<float> TroopWagesPercentage => 
        GetValue(s => s.TroopWagesPercentage, s => s.TroopWagesPercentage, 100f);

    public static CheatValue<bool> FreeTroopUpgrades => 
        GetValue(s => s.FreeTroopUpgrades, s => s.FreeTroopUpgrades);

    public static CheatValue<bool> FreeCompanionHiring => 
        GetValue(s => s.FreeCompanionHiring, s => s.FreeCompanionHiring);

    public static CheatValue<bool> InstantPrisonerRecruitment => 
        GetValue(s => s.InstantPrisonerRecruitment, s => s.InstantPrisonerRecruitment);

    public static CheatValue<bool> NoPrisonerEscape => 
        GetValue(s => s.NoPrisonerEscape, s => s.NoPrisonerEscape);

    public static CheatValue<float> PartyHealingMultiplier => 
        GetValue(s => s.PartyHealingMultiplier, s => s.PartyHealingMultiplier, 1f);

    public static CheatValue<int> ExtraCompanionLimit => 
        GetValue(s => s.ExtraCompanionLimit, s => s.ExtraCompanionLimit, 0);

    public static CheatValue<int> ExtraClanPartyLimit => 
        GetValue(s => s.ExtraClanPartyLimit, s => s.ExtraClanPartyLimit, 0);

    public static CheatValue<int> ExtraClanPartySize => 
        GetValue(s => s.ExtraClanPartySize, s => s.ExtraClanPartySize, 0);

    public static CheatValue<float> RelationGainAfterBattleMultiplier => 
        GetValue(s => s.RelationGainAfterBattleMultiplier, s => s.RelationGainAfterBattleMultiplier, 1f);

    public static CheatValue<bool> PerfectRelationships => 
        GetValue(s => s.PerfectRelationships, s => s.PerfectRelationships);

    public static CheatValue<bool> NeverDieOfOldAge => 
        GetValue(s => s.NeverDieOfOldAge, s => s.NeverDieOfOldAge);

    public static CheatValue<bool> BarterOfferAlwaysAccepted => 
        GetValue(s => s.BarterOfferAlwaysAccepted, s => s.BarterOfferAlwaysAccepted);

    public static CheatValue<bool> NoBarterCooldown => 
        GetValue(s => s.NoBarterCooldown, s => s.NoBarterCooldown);

    public static CheatValue<bool> ConversationAlwaysSuccessful => 
        GetValue(s => s.ConversationAlwaysSuccessful, s => s.ConversationAlwaysSuccessful);

    public static CheatValue<bool> PerfectAttraction => 
        GetValue(s => s.PerfectAttraction, s => s.PerfectAttraction);

    public static CheatValue<bool> AllowSameSexMarriage => 
        GetValue(s => s.AllowSameSexMarriage, s => s.AllowSameSexMarriage);

    public static CheatValue<float> PregnancyChanceMultiplier => 
        GetValue(s => s.PregnancyChanceMultiplier, s => s.PregnancyChanceMultiplier, 1f);

    public static CheatValue<int> AdjustPregnancyDuration => 
        GetValue(s => s.AdjustPregnancyDuration, s => s.AdjustPregnancyDuration, 36);

    public static CheatValue<float> KingdomDecisionWeightMultiplier => 
        GetValue(s => s.KingdomDecisionWeightMultiplier, s => s.KingdomDecisionWeightMultiplier, 1f);

    public static CheatValue<bool> NoRelationshipLossOnDecision => 
        GetValue(s => s.NoRelationshipLossOnDecision, s => s.NoRelationshipLossOnDecision);

    public static CheatValue<bool> NoCrimeRatingForCrimes => 
        GetValue(s => s.NoCrimeRatingForCrimes, s => s.NoCrimeRatingForCrimes);

    public static CheatValue<bool> RecruitExileClans =>
        GetValue(s => s.RecruitExileClans, s => s.RecruitExileClans, true);

    public static CheatValue<float> DecisionOverrideInfluenceCostPercentage => 
        GetValue(s => s.DecisionOverrideInfluenceCostPercentage, s => s.DecisionOverrideInfluenceCostPercentage, 100f);

    public static CheatValue<float> ExperienceMultiplier => 
        GetValue(s => s.ExperienceMultiplier, s => s.ExperienceMultiplier, 1f);

    public static CheatValue<float> CompanionExperienceMultiplier => 
        GetValue(s => s.CompanionExperienceMultiplier, s => s.CompanionExperienceMultiplier, 1f);

    public static CheatValue<float> ClanExperienceMultiplier => 
        GetValue(s => s.ClanExperienceMultiplier, s => s.ClanExperienceMultiplier, 1f);

    public static CheatValue<float> LearningRateMultiplier => 
        GetValue(s => s.LearningRateMultiplier, s => s.LearningRateMultiplier, 1f);

    public static CheatValue<float> CompanionLearningRateMultiplier => 
        GetValue(s => s.CompanionLearningRateMultiplier, s => s.CompanionLearningRateMultiplier, 1f);

    public static CheatValue<float> LearningLimitMultiplier => 
        GetValue(s => s.LearningLimitMultiplier, s => s.LearningLimitMultiplier, 1f);

    public static CheatValue<float> TroopExperienceMultiplier => 
        GetValue(s => s.TroopExperienceMultiplier, s => s.TroopExperienceMultiplier, 1f);

    public static CheatValue<bool> FreeFocusPointAssignment => 
        GetValue(s => s.FreeFocusPointAssignment, s => s.FreeFocusPointAssignment);

    public static CheatValue<float> SiegeBuildingSpeedMultiplier => 
        GetValue(s => s.SiegeBuildingSpeedMultiplier, s => s.SiegeBuildingSpeedMultiplier, 1f);

    public static CheatValue<float> EnemySiegeBuildingSpeedPercentage => 
        GetValue(s => s.EnemySiegeBuildingSpeedPercentage, s => s.EnemySiegeBuildingSpeedPercentage, 100f);

    public static CheatValue<float> FactionArmyCohesionLossPercentage => 
        GetValue(s => s.FactionArmyCohesionLossPercentage, s => s.FactionArmyCohesionLossPercentage, 100f);

    public static CheatValue<float> ArmyCohesionLossPercentage => 
        GetValue(s => s.ArmyCohesionLossPercentage, s => s.ArmyCohesionLossPercentage, 100f);

    public static CheatValue<float> ArmyFoodConsumptionPercentage => 
        GetValue(s => s.ArmyFoodConsumptionPercentage, s => s.ArmyFoodConsumptionPercentage, 100f);

    public static CheatValue<bool> VillagesNeverRaided => 
        GetValue(s => s.VillagesNeverRaided, s => s.VillagesNeverRaided);

    public static CheatValue<bool> DisguiseAlwaysWorks => 
        GetValue(s => s.DisguiseAlwaysWorks, s => s.DisguiseAlwaysWorks);

    public static CheatValue<bool> FreeTroopRecruitment => 
        GetValue(s => s.FreeTroopRecruitment, s => s.FreeTroopRecruitment);

    public static CheatValue<float> ItemTradingCostPercentage => 
        GetValue(s => s.ItemTradingCostPercentage, s => s.ItemTradingCostPercentage, 100f);

    public static CheatValue<float> SellingPriceMultiplier => 
        GetValue(s => s.SellingPriceMultiplier, s => s.SellingPriceMultiplier, 1f);

    public static CheatValue<float> TournamentMaximumBetMultiplier => 
        GetValue(s => s.TournamentMaximumBetMultiplier, s => s.TournamentMaximumBetMultiplier, 1f);

    public static CheatValue<int> DailyFoodBonus => 
        GetValue(s => s.DailyFoodBonus, s => s.DailyFoodBonus, 0);

    public static CheatValue<int> DailyGarrisonBonus => 
        GetValue(s => s.DailyGarrisonBonus, s => s.DailyGarrisonBonus, 0);

    public static CheatValue<int> DailyMilitiaBonus => 
        GetValue(s => s.DailyMilitiaBonus, s => s.DailyMilitiaBonus, 0);

    public static CheatValue<int> DailyProsperityBonus => 
        GetValue(s => s.DailyProsperityBonus, s => s.DailyProsperityBonus, 0);

    public static CheatValue<int> DailyLoyaltyBonus => 
        GetValue(s => s.DailyLoyaltyBonus, s => s.DailyLoyaltyBonus, 0);

    public static CheatValue<int> DailySecurityBonus => 
        GetValue(s => s.DailySecurityBonus, s => s.DailySecurityBonus, 0);

    public static CheatValue<int> DailyHearthsBonus => 
        GetValue(s => s.DailyHearthsBonus, s => s.DailyHearthsBonus, 0);

    public static CheatValue<float> GarrisonWagesPercentage => 
        GetValue(s => s.GarrisonWagesPercentage, s => s.GarrisonWagesPercentage, 100f);

    public static CheatValue<bool> NeverRequireCivilianEquipment => 
        GetValue(s => s.NeverRequireCivilianEquipment, s => s.NeverRequireCivilianEquipment);

    public static CheatValue<float> ConstructionPowerMultiplier => 
        GetValue(s => s.ConstructionPowerMultiplier, s => s.ConstructionPowerMultiplier, 1f);

    public static CheatValue<bool> NoBribeToEnterKeep => 
        GetValue(s => s.NoBribeToEnterKeep, s => s.NoBribeToEnterKeep);

    public static CheatValue<bool> SettlementsNeverRebel => 
        GetValue(s => s.SettlementsNeverRebel, s => s.SettlementsNeverRebel);

    public static CheatValue<float> SmithingEnergyCostPercentage => 
        GetValue(s => s.SmithingEnergyCostPercentage, s => s.SmithingEnergyCostPercentage, 100f);

    public static CheatValue<bool> UnlockAllParts => 
        GetValue(s => s.UnlockAllParts, s => s.UnlockAllParts);

    public static CheatValue<float> SmithingDifficultyPercentage => 
        GetValue(s => s.SmithingDifficultyPercentage, s => s.SmithingDifficultyPercentage, 100f);

    public static CheatValue<float> SmithingCostPercentage => 
        GetValue(s => s.SmithingCostPercentage, s => s.SmithingCostPercentage, 100f);

    public static CheatValue<int> CraftedWeaponHandlingBonus => 
        GetValue(s => s.CraftedWeaponHandlingBonus, s => s.CraftedWeaponHandlingBonus, 0);

    public static CheatValue<int> CraftedWeaponSwingDamageBonus => 
        GetValue(s => s.CraftedWeaponSwingDamageBonus, s => s.CraftedWeaponSwingDamageBonus, 0);

    public static CheatValue<int> CraftedWeaponSwingSpeedBonus => 
        GetValue(s => s.CraftedWeaponSwingSpeedBonus, s => s.CraftedWeaponSwingSpeedBonus, 0);

    public static CheatValue<int> CraftedWeaponThrustDamageBonus => 
        GetValue(s => s.CraftedWeaponThrustDamageBonus, s => s.CraftedWeaponThrustDamageBonus, 0);

    public static CheatValue<int> CraftedWeaponThrustSpeedBonus => 
        GetValue(s => s.CraftedWeaponThrustSpeedBonus, s => s.CraftedWeaponThrustSpeedBonus, 0);

    public static CheatValue<float> WorkshopBuyingCostPercentage => 
        GetValue(s => s.WorkshopBuyingCostPercentage, s => s.WorkshopBuyingCostPercentage, 100f);

    public static CheatValue<float> WorkshopDailyExpensePercentage => 
        GetValue(s => s.WorkshopDailyExpensePercentage, s => s.WorkshopDailyExpensePercentage, 100f);

    public static CheatValue<float> WorkshopSellingCostMultiplier => 
        GetValue(s => s.WorkshopSellingCostMultiplier, s => s.WorkshopSellingCostMultiplier, 1f);

    public static CheatValue<AutoChoosePerk_Type> AutoChoosePerk => 
        GetDropdownValue(s => s.AutoChoosePerk, s => s.AutoChoosePerk, AutoChoosePerk_Type.No);

    public static CheatValue<Setting_Language> LanguageSetting => 
        GetDropdownValue(s => s.LanguageSetting, s => s.LanguageSetting, Setting_Language.English);

    public static CheatValue<int> Village_Init_Gold_Extra => 
        GetValue(s => s.Village_Init_Gold_Extra, s => s.Village_Init_Gold_Extra, 0);

    public static CheatValue<int> Town_Init_Gold_Extra => 
        GetValue(s => s.Town_Init_Gold_Extra, s => s.Town_Init_Gold_Extra, 0);


    public static CheatValue<bool> UnblockableThrust_player => 
        GetValue(s => s.UnblockableThrust_player, s => s.UnblockableThrust_player);

    public static CheatValue<bool> UnblockableThrust_ally => 
        GetValue(s => s.UnblockableThrust_ally, s => s.UnblockableThrust_ally);

    public static CheatValue<bool> UnblockableThrust_enemy => 
        GetValue(s => s.UnblockableThrust_enemy, s => s.UnblockableThrust_enemy);

    public static CheatValue<int> xGang => 
        GetValue(s => s.xGang, s => s.xGang, 0);

    public static CheatValue<int> xArt => 
        GetValue(s => s.xArt, s => s.xArt, 0);

    public static CheatValue<int> xMerch => 
        GetValue(s => s.xMerch, s => s.xMerch, 0);

    public static CheatValue<int> xVill => 
        GetValue(s => s.xVill, s => s.xVill, 0);

    public static CheatValue<int> xRural => 
        GetValue(s => s.xRural, s => s.xRural, 0);


    public static CheatValue<int> AddMoneyThreshhold => 
        GetValue(s => s.AddMoneyThreshhold, s => s.AddMoneyThreshhold, 0);

    public static CheatValue<int> MaxAttr => 
        GetValue(s => s.MaxAttr, s => s.MaxAttr, 10);

    public static CheatValue<int> AddMoney_count => 
        GetValue(s => s.AddMoney_count, s => s.AddMoney_count, 0);

    public static CheatValue<bool> KeepDaughter => 
        GetValue(s => s.KeepDaughter, s => s.KeepDaughter);


    public static CheatValue<bool> PlayerAlwaysCrush => 
        GetValue(s => s.PlayerAlwaysCrush, s => s.PlayerAlwaysCrush);


    public static CheatValue<bool> PlayerPartyCrush =>
        GetValue(s => s.PlayerPartyCrush, s => s.PlayerPartyCrush);

    public static CheatValue<bool> EnemyCrush => 
        GetValue(s => s.EnemyCrush, s => s.EnemyCrush);

    public static CheatValue<bool> EnableEverYoung => 
        GetValue(s => s.EnableEverYoung, s => s.EnableEverYoung);

    public static CheatValue<int> EverYoungSkillNeed => 
        GetValue(s => s.EverYoungSkillNeed, s => s.EverYoungSkillNeed, 400);

    public static CheatValue<float> VigorDmgPercent => 
        GetValue(s => s.VigorDmgPercent, s => s.VigorDmgPercent, 0.02f);

    public static CheatValue<float> VigorArmorAdd => 
        GetValue(s => s.VigorArmorAdd, s => s.VigorArmorAdd, 1f);

    public static CheatValue<float> VigorShieldEndurancePercent => 
        GetValue(s => s.VigorShieldEndurancePercent, s => s.VigorShieldEndurancePercent, 1f);

    public static CheatValue<float> VigorFinalDmgAdd => 
        GetValue(s => s.VigorFinalDmgAdd, s => s.VigorFinalDmgAdd, 0.334f);

    public static CheatValue<float> VigorDmgTakenReduce => 
        GetValue(s => s.VigorDmgTakenReduce, s => s.VigorDmgTakenReduce, 0.334f);

    public static CheatValue<int> VigorCrushThroughPositive => 
        GetValue(s => s.VigorCrushThroughPositive, s => s.VigorCrushThroughPositive, 5);

    public static CheatValue<int> VigorCrushThroughNegative => 
        GetValue(s => s.VigorCrushThroughNegative, s => s.VigorCrushThroughNegative, 10);

    public static CheatValue<float> IntelligenceAmmoAddPercent => 
        GetValue(s => s.IntelligenceAmmoAddPercent, s => s.IntelligenceAmmoAddPercent, 0.1f);

    public static CheatValue<int> ControlAmmoNoConsumeRate => 
        GetValue(s => s.ControlAmmoNoConsumeRate, s => s.ControlAmmoNoConsumeRate, 5);

    public static CheatValue<float> ControlDropDmgReducePercent => 
        GetValue(s => s.ControlDropDmgReducePercent, s => s.ControlDropDmgReducePercent, 0.05f);

    public static CheatValue<float> ControlAimStabilityPercent => 
        GetValue(s => s.ControlAimStabilityPercent, s => s.ControlAimStabilityPercent, 0.1f);

    public static CheatValue<float> ControlMountManeuverPercent => 
        GetValue(s => s.ControlMountManeuverPercent, s => s.ControlMountManeuverPercent, 0.05f);

    public static CheatValue<int> ControlCritRate => 
        GetValue(s => s.ControlCritRate, s => s.ControlCritRate, 2);

    public static CheatValue<int> ControlExemptionRate => 
        GetValue(s => s.ControlExemptionRate, s => s.ControlExemptionRate, 2);

    public static CheatValue<int> ControlPenetrateRate => 
        GetValue(s => s.ControlPenetrateRate, s => s.ControlPenetrateRate, 3);

    public static CheatValue<float> EnduranceHpAddPercent => 
        GetValue(s => s.EnduranceHpAddPercent, s => s.EnduranceHpAddPercent, 0.05f);

    public static CheatValue<float> EnduranceHealRate => 
        GetValue(s => s.EnduranceHealRate, s => s.EnduranceHealRate, 0.05f);

    public static CheatValue<float> EnduranceStaggerPercent => 
        GetValue(s => s.EnduranceStaggerPercent, s => s.EnduranceStaggerPercent, 0.2f);

    public static CheatValue<float> EnduranceWalkSpeedPercent => 
        GetValue(s => s.EnduranceWalkSpeedPercent, s => s.EnduranceWalkSpeedPercent, 0.01f);

    public static CheatValue<float> EnduranceMountSpeedPercent => 
        GetValue(s => s.EnduranceMountSpeedPercent, s => s.EnduranceMountSpeedPercent, 0.025f);

    public static CheatValue<float> CunningPrisonerRecruitSpeedPercent => 
        GetValue(s => s.CunningPrisonerRecruitSpeedPercent, s => s.CunningPrisonerRecruitSpeedPercent, 0.1f);

    public static CheatValue<float> CunningPrisonerCapacityPercent => 
        GetValue(s => s.CunningPrisonerCapacityPercent, s => s.CunningPrisonerCapacityPercent, 0.1f);

    public static CheatValue<float> CunningRaidSpeedPercent => 
        GetValue(s => s.CunningRaidSpeedPercent, s => s.CunningRaidSpeedPercent, 0.1f);

    public static CheatValue<float> CunningPartySpeedAdd => 
        GetValue(s => s.CunningPartySpeedAdd, s => s.CunningPartySpeedAdd, 0.1f);

    public static CheatValue<float> CunningCompanionCapacityAdd => 
        GetValue(s => s.CunningCompanionCapacityAdd, s => s.CunningCompanionCapacityAdd, 0.2f);


    public static CheatValue<float> SocialBoundary => 
        GetValue(s => s.SocialBoundary, s => s.SocialBoundary, 3.5f);

    public static CheatValue<float> SocialHearthAdd => 
        GetValue(s => s.SocialHearthAdd, s => s.SocialHearthAdd, 0.25f);

    public static CheatValue<float> SocialSettlementLoyaltyAdd => 
        GetValue(s => s.SocialSettlementLoyaltyAdd, s => s.SocialSettlementLoyaltyAdd, 0.25f);

    public static CheatValue<float> SocialMilitiaAdd => 
        GetValue(s => s.SocialMilitiaAdd, s => s.SocialMilitiaAdd, 0.5f);

    public static CheatValue<float> SocialRecruitSpeedPercent => 
        GetValue(s => s.SocialRecruitSpeedPercent, s => s.SocialRecruitSpeedPercent, 0.05f);

    public static CheatValue<float> SocialTaxPercent => 
        GetValue(s => s.SocialTaxPercent, s => s.SocialTaxPercent, 0.05f);

    public static CheatValue<float> SocialWorkshopProductionPercent => 
        GetValue(s => s.SocialWorkshopProductionPercent, s => s.SocialWorkshopProductionPercent, 0.1f);

    public static CheatValue<float> SocialCompanionCapacityAdd => 
        GetValue(s => s.SocialCompanionCapacityAdd, s => s.SocialCompanionCapacityAdd, 0.2f);

    public static CheatValue<float> IntelligenceBoundary => 
        GetValue(s => s.IntelligenceBoundary, s => s.IntelligenceBoundary, 3.5f);

    public static CheatValue<float> IntelligenceExpRate => 
        GetValue(s => s.IntelligenceExpRate, s => s.IntelligenceExpRate, 0.05f);

    public static CheatValue<float> IntelligenceSiegeEndurancePercent => 
        GetValue(s => s.IntelligenceSiegeEndurancePercent, s => s.IntelligenceSiegeEndurancePercent, 0.1f);

    public static CheatValue<float> IntelligenceWallEndurancePercent => 
        GetValue(s => s.IntelligenceWallEndurancePercent, s => s.IntelligenceWallEndurancePercent, 0.1f);

    public static CheatValue<float> IntelligenceBallistaAdd => 
        GetValue(s => s.IntelligenceBallistaAdd, s => s.IntelligenceBallistaAdd, 0.334f);

    public static CheatValue<float> IntelligenceLeaderSettlementFoodPercent => 
        GetValue(s => s.IntelligenceLeaderSettlementFoodPercent, s => s.IntelligenceLeaderSettlementFoodPercent, 0.5f);

    public static CheatValue<float> IntelligenceGovernorSettlementFoodPercent => 
        GetValue(s => s.IntelligenceGovernorSettlementFoodPercent, s => s.IntelligenceGovernorSettlementFoodPercent, 1f);

    public static CheatValue<float> IntelligenceProsperityFoodCostReducePercent => 
        GetValue(s => s.IntelligenceProsperityFoodCostReducePercent, s => s.IntelligenceProsperityFoodCostReducePercent, 0.075f);

    public static CheatValue<float> IntelligenceGarrisonWageReducePercent => 
        GetValue(s => s.IntelligenceGarrisonWageReducePercent, s => s.IntelligenceGarrisonWageReducePercent, 0.05f);

    public static CheatValue<float> IntelligenceWorkshopProductionPercent => 
        GetValue(s => s.IntelligenceWorkshopProductionPercent, s => s.IntelligenceWorkshopProductionPercent, 0.25f);

    public static CheatValue<float> CombatAttributeRatePlayer => 
        GetValue(s => s.CombatAttributeRatePlayer, s => s.CombatAttributeRatePlayer, 0f);

    public static CheatValue<float> CombatAttributeRateClanMember => 
        GetValue(s => s.CombatAttributeRateClanMember, s => s.CombatAttributeRateClanMember, 0f);

    public static CheatValue<float> CombatAttributeRateOther => 
        GetValue(s => s.CombatAttributeRateOther, s => s.CombatAttributeRateOther, 0f);

    public static CheatValue<float> StrategyAttributeRatePlayer => 
        GetValue(s => s.StrategyAttributeRatePlayer, s => s.StrategyAttributeRatePlayer, 0f);

    public static CheatValue<float> StrategyAttributeRateClanMember => 
        GetValue(s => s.StrategyAttributeRateClanMember, s => s.StrategyAttributeRateClanMember, 0f);

    public static CheatValue<float> StrategyAttributeRateOther => 
        GetValue(s => s.StrategyAttributeRateOther, s => s.StrategyAttributeRateOther, 0f);

    public static CheatValue<float> PlayerPartyRangedMovementPenaltyPercentage => GetValue(s => s.PlayerPartyRangedMovementPenaltyPercentage, s => s.PlayerPartyRangedMovementPenaltyPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedUnsteadyPenaltyPercentage => GetValue(s => s.PlayerPartyRangedUnsteadyPenaltyPercentage, s => s.PlayerPartyRangedUnsteadyPenaltyPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedBestAccuracyWaitPercentage => GetValue(s => s.PlayerPartyRangedBestAccuracyWaitPercentage, s => s.PlayerPartyRangedBestAccuracyWaitPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedRotationPenaltyPercentage => GetValue(s => s.PlayerPartyRangedRotationPenaltyPercentage, s => s.PlayerPartyRangedRotationPenaltyPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedAccelerationPenaltyPercentage => GetValue(s => s.PlayerPartyRangedAccelerationPenaltyPercentage, s => s.PlayerPartyRangedAccelerationPenaltyPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedAiShooterErrorPercentage => GetValue(s => s.PlayerPartyRangedAiShooterErrorPercentage, s => s.PlayerPartyRangedAiShooterErrorPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedAiLeadErrorPercentage => GetValue(s => s.PlayerPartyRangedAiLeadErrorPercentage, s => s.PlayerPartyRangedAiLeadErrorPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedAiHorizontalErrorPercentage => GetValue(s => s.PlayerPartyRangedAiHorizontalErrorPercentage, s => s.PlayerPartyRangedAiHorizontalErrorPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedAiVerticalErrorPercentage => GetValue(s => s.PlayerPartyRangedAiVerticalErrorPercentage, s => s.PlayerPartyRangedAiVerticalErrorPercentage, 1f);
    public static CheatValue<float> PlayerPartyRangedAiShootIntervalPercentage => GetValue(s => s.PlayerPartyRangedAiShootIntervalPercentage, s => s.PlayerPartyRangedAiShootIntervalPercentage, 1f);

    public static CheatValue<float> PlayerPartyMeleeSwingSpeedMultiplier => GetValue(s => s.PlayerPartyMeleeSwingSpeedMultiplier, s => s.PlayerPartyMeleeSwingSpeedMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeThrustReadySpeedMultiplier => GetValue(s => s.PlayerPartyMeleeThrustReadySpeedMultiplier, s => s.PlayerPartyMeleeThrustReadySpeedMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeHandlingMultiplier => GetValue(s => s.PlayerPartyMeleeHandlingMultiplier, s => s.PlayerPartyMeleeHandlingMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeOffhandDefenseSpeedMultiplier => GetValue(s => s.PlayerPartyMeleeOffhandDefenseSpeedMultiplier, s => s.PlayerPartyMeleeOffhandDefenseSpeedMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeShieldBashStunMultiplier => GetValue(s => s.PlayerPartyMeleeShieldBashStunMultiplier, s => s.PlayerPartyMeleeShieldBashStunMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeKickStunMultiplier => GetValue(s => s.PlayerPartyMeleeKickStunMultiplier, s => s.PlayerPartyMeleeKickStunMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeAiBlockDecisionMultiplier => GetValue(s => s.PlayerPartyMeleeAiBlockDecisionMultiplier, s => s.PlayerPartyMeleeAiBlockDecisionMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeAiParryDecisionMultiplier => GetValue(s => s.PlayerPartyMeleeAiParryDecisionMultiplier, s => s.PlayerPartyMeleeAiParryDecisionMultiplier, 1f);
    public static CheatValue<float> PlayerPartyMeleeAiShieldDefenseMultiplier => GetValue(s => s.PlayerPartyMeleeAiShieldDefenseMultiplier, s => s.PlayerPartyMeleeAiShieldDefenseMultiplier, 1f);

    public static CheatValue<float> NavalCampaignSpeedMultiplier => GetValue(s => s.NavalCampaignSpeedMultiplier, s => s.NavalCampaignSpeedMultiplier, 1f);
    public static CheatValue<float> NavalOarForceMultiplier => GetValue(s => s.NavalOarForceMultiplier, s => s.NavalOarForceMultiplier, 1f);
    public static CheatValue<float> NavalSailForceMultiplier => GetValue(s => s.NavalSailForceMultiplier, s => s.NavalSailForceMultiplier, 1f);
    public static CheatValue<float> NavalCrewCapacityMultiplier => GetValue(s => s.NavalCrewCapacityMultiplier, s => s.NavalCrewCapacityMultiplier, 1f);
    public static CheatValue<float> NavalShipCombatFactorMultiplier => GetValue(s => s.NavalShipCombatFactorMultiplier, s => s.NavalShipCombatFactorMultiplier, 1f);
    public static CheatValue<int> NavalAdditionalAmmo => GetValue(s => s.NavalAdditionalAmmo, s => s.NavalAdditionalAmmo, 0);
    public static CheatValue<float> NavalSeaAttritionPercentage => GetValue(s => s.NavalSeaAttritionPercentage, s => s.NavalSeaAttritionPercentage, 100f);
    public static CheatValue<float> NavalBattleShipDamagePercentage => GetValue(s => s.NavalBattleShipDamagePercentage, s => s.NavalBattleShipDamagePercentage, 100f);
    public static CheatValue<float> NavalStormDamagePercentage => GetValue(s => s.NavalStormDamagePercentage, s => s.NavalStormDamagePercentage, 100f);
    public static CheatValue<float> NavalStormFrequencyMultiplier => GetValue(s => s.NavalStormFrequencyMultiplier, s => s.NavalStormFrequencyMultiplier, 1f);
    public static CheatValue<float> NavalStormStrengthMultiplier => GetValue(s => s.NavalStormStrengthMultiplier, s => s.NavalStormStrengthMultiplier, 1f);
    public static CheatValue<float> NavalShipPurchaseCostMultiplier => GetValue(s => s.NavalShipPurchaseCostMultiplier, s => s.NavalShipPurchaseCostMultiplier, 1f);
    public static CheatValue<float> NavalShipRepairCostMultiplier => GetValue(s => s.NavalShipRepairCostMultiplier, s => s.NavalShipRepairCostMultiplier, 1f);
    public static CheatValue<float> NavalShipUpgradeCostMultiplier => GetValue(s => s.NavalShipUpgradeCostMultiplier, s => s.NavalShipUpgradeCostMultiplier, 1f);
    public static CheatValue<float> NavalDeploymentLimitMultiplier => GetValue(s => s.NavalDeploymentLimitMultiplier, s => s.NavalDeploymentLimitMultiplier, 1f);
    public static CheatValue<float> NavalBattleRewardMultiplier => GetValue(s => s.NavalBattleRewardMultiplier, s => s.NavalBattleRewardMultiplier, 1f);
    public static CheatValue<float> NavalFleetMinimumTroopPercentage => GetValue(s => s.NavalFleetMinimumTroopPercentage, s => s.NavalFleetMinimumTroopPercentage, 100f);

    public static CheatValue<bool> TestMode => 
        GetValue(s => s.TestMode, s => s.TestMode);
}
