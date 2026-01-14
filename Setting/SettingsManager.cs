using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Common;
using System;
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
        public const bool AICrush = false;
        public const bool AllyCrush = false;
        public const bool EnemyCrush = false;

        public const bool InfiniteMomentum = false;

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
        public const bool UnblockableThrust_AI = false;
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

        public const bool AlwaysCrushThroughShields = false;

        public const bool SliceThroughEveryone = false;
        public const bool SliceThroughEveryone_AI = false;
        public const bool SliceThroughEveryone_ally = false;
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

        public const float PartyDamageTakenPercentage = 100f;

        public const bool PartyOneHitKill = false;

        public const bool PartyOnlyKnockout = false;

        public const bool NoRunningAway = false;

        public const float PartyHealthRegeneration = 0f;

        public const bool PartyInfiniteAmmo = false;

        public const float PartyDamageMultiplier = 1f;

        public const bool NoFriendlyFire = false;

        public const float CompanionDeathPercentage = 100f;

        public const KnockoutOrKilled FriendlyLordsKnockoutOrKilled = KnockoutOrKilled.Default;

        public const float FriendlyLordCombatDeathPercentage = 100f;

        public const KnockoutOrKilled EnemyLordsKnockoutOrKilled = KnockoutOrKilled.Default;

        public const KnockoutOrKilled EnemyTroopsKnockoutOrKilled = KnockoutOrKilled.Default;

        public const bool EnemyOnlyKnockout = false;

        public const bool EnemiesNoRunningAway = false;

        public const float EnemyDamagePercentage = 100f;

        public const float EnemyLordCombatDeathPercentage = 100f;

        public const float EnemyLordCombatDeathChanceMultiplier = 1f;

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

        public const bool EveryoneBuysWorkshops = false;

        // Hero Enhance Settings

        public const bool EnableEverYoung = false;

        public const int EverYoungSkillNeed = 400;

        public const AutoChoosePerk_Type AutoChoosePerk = AutoChoosePerk_Type.No;

        public const float VigorDmgPercent = 0.02f;

        public const float VigorArmorAdd = 1f;

        public const float VigorMountArmorAdd = 1f;

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

        public const bool EnableDailyGainXp = false;

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

    // Generic helper methods to reduce code duplication
    private static CheatValue<bool> GetBoolValue(Func<BannerlordCheatsPerCampaignSettings, bool> perCampaignGetter,
                                                  Func<BannerlordCheatsGlobalSettings, bool> globalGetter)
    {
        if (IsPerCampaignInstanceLoaded && perCampaignGetter(PerCampaignInstance))
            return new CheatValue<bool>(true, perCampaignGetter(PerCampaignInstance));
        
        if (globalGetter(GlobalInstance))
            return new CheatValue<bool>(true, globalGetter(GlobalInstance));
        
        return new CheatValue<bool>(false, false);
    }

    private static CheatValue<int> GetIntValue(Func<BannerlordCheatsPerCampaignSettings, int> perCampaignGetter,
                                                Func<BannerlordCheatsGlobalSettings, int> globalGetter,
                                                int defaultValue)
    {
        if (IsPerCampaignInstanceLoaded && perCampaignGetter(PerCampaignInstance) != defaultValue)
            return new CheatValue<int>(true, perCampaignGetter(PerCampaignInstance));
        
        if (globalGetter(GlobalInstance) != defaultValue)
            return new CheatValue<int>(true, globalGetter(GlobalInstance));
        
        return new CheatValue<int>(false, defaultValue);
    }

    private static CheatValue<float> GetFloatValue(Func<BannerlordCheatsPerCampaignSettings, float> perCampaignGetter,
                                                    Func<BannerlordCheatsGlobalSettings, float> globalGetter,
                                                    float defaultValue)
    {
        if (IsPerCampaignInstanceLoaded && perCampaignGetter(PerCampaignInstance) != defaultValue)
            return new CheatValue<float>(true, perCampaignGetter(PerCampaignInstance));
        
        if (globalGetter(GlobalInstance) != defaultValue)
            return new CheatValue<float>(true, globalGetter(GlobalInstance));
        
        return new CheatValue<float>(false, defaultValue);
    }

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
        GetBoolValue(s => s.EnableHotkeys, s => s.EnableHotkeys);

    public static CheatValue<bool> EnableHotkeyTips => 
        GetBoolValue(s => s.EnableHotkeyTips, s => s.EnableHotkeyTips);

    public static CheatValue<float> MapSpeedMultiplier => 
        GetFloatValue(s => s.MapSpeedMultiplier, s => s.MapSpeedMultiplier, 1f);

    public static CheatValue<float> MapVisibilityMultiplier => 
        GetFloatValue(s => s.MapVisibilityMultiplier, s => s.MapVisibilityMultiplier, 1f);

    public static CheatValue<float> NpcMapSpeedPercentage => 
        GetFloatValue(s => s.NpcMapSpeedPercentage, s => s.NpcMapSpeedPercentage, 100f);

    public static CheatValue<bool> PartyInvisibleOnMap => 
        GetBoolValue(s => s.PartyInvisibleOnMap, s => s.PartyInvisibleOnMap);

    public static CheatValue<bool> CaravansInvisibleOnMap => 
        GetBoolValue(s => s.CaravansInvisibleOnMap, s => s.CaravansInvisibleOnMap);

    public static CheatValue<float> DamageTakenPercentage => 
        GetFloatValue(s => s.DamageTakenPercentage, s => s.DamageTakenPercentage, 100f);

    public static CheatValue<bool> Invincible => 
        GetBoolValue(s => s.Invincible, s => s.Invincible);

    public static CheatValue<bool> PlayerHorseInvincible => 
        GetBoolValue(s => s.PlayerHorseInvincible, s => s.PlayerHorseInvincible);

    public static CheatValue<bool> OneHitKill => 
        GetBoolValue(s => s.OneHitKill, s => s.OneHitKill);

    public static CheatValue<bool> SliceThroughEveryone => 
        GetBoolValue(s => s.SliceThroughEveryone, s => s.SliceThroughEveryone);

    public static CheatValue<bool> SliceThroughEveryone_ally => 
        GetBoolValue(s => s.SliceThroughEveryone_ally, s => s.SliceThroughEveryone_ally);

    public static CheatValue<bool> SliceThroughEveryone_enemy => 
        GetBoolValue(s => s.SliceThroughEveryone_enemy, s => s.SliceThroughEveryone_enemy);

    public static CheatValue<float> HealthRegeneration => 
        GetFloatValue(s => s.HealthRegeneration, s => s.HealthRegeneration, 0f);

    public static CheatValue<bool> InfiniteAmmo => 
        GetBoolValue(s => s.InfiniteAmmo, s => s.InfiniteAmmo);

    public static CheatValue<float> DamageMultiplier => 
        GetFloatValue(s => s.DamageMultiplier, s => s.DamageMultiplier, 1f);

    public static CheatValue<bool> AlwaysKnockDown => 
        GetBoolValue(s => s.AlwaysKnockDown, s => s.AlwaysKnockDown);

    public static CheatValue<bool> NeverKnockedBackByAttacks => 
        GetBoolValue(s => s.NeverKnockedBackByAttacks, s => s.NeverKnockedBackByAttacks);

    public static CheatValue<bool> NoStuckArrows => 
        GetBoolValue(s => s.NoStuckArrows, s => s.NoStuckArrows);

    public static CheatValue<bool> InstantCrossbowReload => 
        GetBoolValue(s => s.InstantCrossbowReload, s => s.InstantCrossbowReload);

    public static CheatValue<KnockoutOrKilled> PartyKnockoutOrKilled => 
        GetDropdownValue(s => s.PartyKnockoutOrKilled, s => s.PartyKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<KnockoutOrKilled> CompanionsKnockoutOrKilled => 
        GetDropdownValue(s => s.CompanionsKnockoutOrKilled, s => s.CompanionsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<bool> PartyInvincible => 
        GetBoolValue(s => s.PartyInvincible, s => s.PartyInvincible);

    public static CheatValue<bool> PartyHeroesInvincible => 
        GetBoolValue(s => s.PartyHeroesInvincible, s => s.PartyHeroesInvincible);

    public static CheatValue<float> PartyDamageTakenPercentage => 
        GetFloatValue(s => s.PartyDamageTakenPercentage, s => s.PartyDamageTakenPercentage, 100f);

    public static CheatValue<bool> PartyOneHitKill => 
        GetBoolValue(s => s.PartyOneHitKill, s => s.PartyOneHitKill);

    public static CheatValue<bool> NoRunningAway => 
        GetBoolValue(s => s.NoRunningAway, s => s.NoRunningAway);

    public static CheatValue<float> PartyHealthRegeneration => 
        GetFloatValue(s => s.PartyHealthRegeneration, s => s.PartyHealthRegeneration, 0f);

    public static CheatValue<bool> PartyInfiniteAmmo => 
        GetBoolValue(s => s.PartyInfiniteAmmo, s => s.PartyInfiniteAmmo);

    public static CheatValue<float> PartyDamageMultiplier => 
        GetFloatValue(s => s.PartyDamageMultiplier, s => s.PartyDamageMultiplier, 1f);

    public static CheatValue<bool> NoFriendlyFire => 
        GetBoolValue(s => s.NoFriendlyFire, s => s.NoFriendlyFire);

    public static CheatValue<KnockoutOrKilled> FriendlyLordsKnockoutOrKilled => 
        GetDropdownValue(s => s.FriendlyLordsKnockoutOrKilled, s => s.FriendlyLordsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<KnockoutOrKilled> EnemyLordsKnockoutOrKilled => 
        GetDropdownValue(s => s.EnemyLordsKnockoutOrKilled, s => s.EnemyLordsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<KnockoutOrKilled> EnemyTroopsKnockoutOrKilled => 
        GetDropdownValue(s => s.EnemyTroopsKnockoutOrKilled, s => s.EnemyTroopsKnockoutOrKilled, KnockoutOrKilled.Default);

    public static CheatValue<bool> EnemiesNoRunningAway => 
        GetBoolValue(s => s.EnemiesNoRunningAway, s => s.EnemiesNoRunningAway);

    public static CheatValue<float> EnemyDamagePercentage => 
        GetFloatValue(s => s.EnemyDamagePercentage, s => s.EnemyDamagePercentage, 100f);

    public static CheatValue<float> RenownRewardMultiplier => 
        GetFloatValue(s => s.RenownRewardMultiplier, s => s.RenownRewardMultiplier, 1f);

    public static CheatValue<float> InfluenceRewardMultiplier => 
        GetFloatValue(s => s.InfluenceRewardMultiplier, s => s.InfluenceRewardMultiplier, 1f);

    public static CheatValue<bool> AlwaysWinBattleSimulation => 
        GetBoolValue(s => s.AlwaysWinBattleSimulation, s => s.AlwaysWinBattleSimulation);

    public static CheatValue<bool> NoTroopSacrifice => 
        GetBoolValue(s => s.NoTroopSacrifice, s => s.NoTroopSacrifice);

    public static CheatValue<int> BanditHideoutTroopLimit => 
        GetIntValue(s => s.BanditHideoutTroopLimit, s => s.BanditHideoutTroopLimit, 0);

    public static CheatValue<float> CombatZoomMultiplier => 
        GetFloatValue(s => s.CombatZoomMultiplier, s => s.CombatZoomMultiplier, 1f);

    public static CheatValue<int> ExtraInventoryCapacity => 
        GetIntValue(s => s.ExtraInventoryCapacity, s => s.ExtraInventoryCapacity, 0);

    public static CheatValue<bool> NativeItemSpawning => 
        GetBoolValue(s => s.NativeItemSpawning, s => s.NativeItemSpawning);

    public static CheatValue<int> ExtraPartyMemberSize => 
        GetIntValue(s => s.ExtraPartyMemberSize, s => s.ExtraPartyMemberSize, 0);

    public static CheatValue<int> ExtraPartyPrisonerSize => 
        GetIntValue(s => s.ExtraPartyPrisonerSize, s => s.ExtraPartyPrisonerSize, 0);

    public static CheatValue<int> ExtraPartyMorale => 
        GetIntValue(s => s.ExtraPartyMorale, s => s.ExtraPartyMorale, 0);

    public static CheatValue<bool> InstantEscape => 
        GetBoolValue(s => s.InstantEscape, s => s.InstantEscape);

    public static CheatValue<float> FoodConsumptionPercentage => 
        GetFloatValue(s => s.FoodConsumptionPercentage, s => s.FoodConsumptionPercentage, 100f);

    public static CheatValue<float> TroopWagesPercentage => 
        GetFloatValue(s => s.TroopWagesPercentage, s => s.TroopWagesPercentage, 100f);

    public static CheatValue<bool> FreeTroopUpgrades => 
        GetBoolValue(s => s.FreeTroopUpgrades, s => s.FreeTroopUpgrades);

    public static CheatValue<bool> FreeCompanionHiring => 
        GetBoolValue(s => s.FreeCompanionHiring, s => s.FreeCompanionHiring);

    public static CheatValue<bool> InstantPrisonerRecruitment => 
        GetBoolValue(s => s.InstantPrisonerRecruitment, s => s.InstantPrisonerRecruitment);

    public static CheatValue<bool> NoPrisonerEscape => 
        GetBoolValue(s => s.NoPrisonerEscape, s => s.NoPrisonerEscape);

    public static CheatValue<float> PartyHealingMultiplier => 
        GetFloatValue(s => s.PartyHealingMultiplier, s => s.PartyHealingMultiplier, 1f);

    public static CheatValue<int> ExtraCompanionLimit => 
        GetIntValue(s => s.ExtraCompanionLimit, s => s.ExtraCompanionLimit, 0);

    public static CheatValue<int> ExtraClanPartyLimit => 
        GetIntValue(s => s.ExtraClanPartyLimit, s => s.ExtraClanPartyLimit, 0);

    public static CheatValue<int> ExtraClanPartySize => 
        GetIntValue(s => s.ExtraClanPartySize, s => s.ExtraClanPartySize, 0);

    public static CheatValue<float> RelationGainAfterBattleMultiplier => 
        GetFloatValue(s => s.RelationGainAfterBattleMultiplier, s => s.RelationGainAfterBattleMultiplier, 1f);

    public static CheatValue<bool> PerfectRelationships => 
        GetBoolValue(s => s.PerfectRelationships, s => s.PerfectRelationships);

    public static CheatValue<bool> NeverDieOfOldAge => 
        GetBoolValue(s => s.NeverDieOfOldAge, s => s.NeverDieOfOldAge);

    public static CheatValue<bool> BarterOfferAlwaysAccepted => 
        GetBoolValue(s => s.BarterOfferAlwaysAccepted, s => s.BarterOfferAlwaysAccepted);

    public static CheatValue<bool> NoBarterCooldown => 
        GetBoolValue(s => s.NoBarterCooldown, s => s.NoBarterCooldown);

    public static CheatValue<bool> ConversationAlwaysSuccessful => 
        GetBoolValue(s => s.ConversationAlwaysSuccessful, s => s.ConversationAlwaysSuccessful);

    public static CheatValue<bool> PerfectAttraction => 
        GetBoolValue(s => s.PerfectAttraction, s => s.PerfectAttraction);

    public static CheatValue<bool> AllowSameSexMarriage => 
        GetBoolValue(s => s.AllowSameSexMarriage, s => s.AllowSameSexMarriage);

    public static CheatValue<float> PregnancyChanceMultiplier => 
        GetFloatValue(s => s.PregnancyChanceMultiplier, s => s.PregnancyChanceMultiplier, 1f);

    public static CheatValue<int> AdjustPregnancyDuration => 
        GetIntValue(s => s.AdjustPregnancyDuration, s => s.AdjustPregnancyDuration, 36);

    public static CheatValue<float> KingdomDecisionWeightMultiplier => 
        GetFloatValue(s => s.KingdomDecisionWeightMultiplier, s => s.KingdomDecisionWeightMultiplier, 1f);

    public static CheatValue<bool> NoRelationshipLossOnDecision => 
        GetBoolValue(s => s.NoRelationshipLossOnDecision, s => s.NoRelationshipLossOnDecision);

    public static CheatValue<bool> NoCrimeRatingForCrimes => 
        GetBoolValue(s => s.NoCrimeRatingForCrimes, s => s.NoCrimeRatingForCrimes);

    public static CheatValue<float> DecisionOverrideInfluenceCostPercentage => 
        GetFloatValue(s => s.DecisionOverrideInfluenceCostPercentage, s => s.DecisionOverrideInfluenceCostPercentage, 100f);

    public static CheatValue<float> ExperienceMultiplier => 
        GetFloatValue(s => s.ExperienceMultiplier, s => s.ExperienceMultiplier, 1f);

    public static CheatValue<float> CompanionExperienceMultiplier => 
        GetFloatValue(s => s.CompanionExperienceMultiplier, s => s.CompanionExperienceMultiplier, 1f);

    public static CheatValue<float> ClanExperienceMultiplier => 
        GetFloatValue(s => s.ClanExperienceMultiplier, s => s.ClanExperienceMultiplier, 1f);

    public static CheatValue<float> LearningRateMultiplier => 
        GetFloatValue(s => s.LearningRateMultiplier, s => s.LearningRateMultiplier, 1f);

    public static CheatValue<float> CompanionLearningRateMultiplier => 
        GetFloatValue(s => s.CompanionLearningRateMultiplier, s => s.CompanionLearningRateMultiplier, 1f);

    public static CheatValue<float> LearningLimitMultiplier => 
        GetFloatValue(s => s.LearningLimitMultiplier, s => s.LearningLimitMultiplier, 1f);

    public static CheatValue<float> TroopExperienceMultiplier => 
        GetFloatValue(s => s.TroopExperienceMultiplier, s => s.TroopExperienceMultiplier, 1f);

    public static CheatValue<bool> FreeFocusPointAssignment => 
        GetBoolValue(s => s.FreeFocusPointAssignment, s => s.FreeFocusPointAssignment);

    public static CheatValue<float> SiegeBuildingSpeedMultiplier => 
        GetFloatValue(s => s.SiegeBuildingSpeedMultiplier, s => s.SiegeBuildingSpeedMultiplier, 1f);

    public static CheatValue<float> EnemySiegeBuildingSpeedPercentage => 
        GetFloatValue(s => s.EnemySiegeBuildingSpeedPercentage, s => s.EnemySiegeBuildingSpeedPercentage, 100f);

    public static CheatValue<float> FactionArmyCohesionLossPercentage => 
        GetFloatValue(s => s.FactionArmyCohesionLossPercentage, s => s.FactionArmyCohesionLossPercentage, 100f);

    public static CheatValue<float> ArmyCohesionLossPercentage => 
        GetFloatValue(s => s.ArmyCohesionLossPercentage, s => s.ArmyCohesionLossPercentage, 100f);

    public static CheatValue<float> ArmyFoodConsumptionPercentage => 
        GetFloatValue(s => s.ArmyFoodConsumptionPercentage, s => s.ArmyFoodConsumptionPercentage, 100f);

    public static CheatValue<bool> VillagesNeverRaided => 
        GetBoolValue(s => s.VillagesNeverRaided, s => s.VillagesNeverRaided);

    public static CheatValue<bool> DisguiseAlwaysWorks => 
        GetBoolValue(s => s.DisguiseAlwaysWorks, s => s.DisguiseAlwaysWorks);

    public static CheatValue<bool> FreeTroopRecruitment => 
        GetBoolValue(s => s.FreeTroopRecruitment, s => s.FreeTroopRecruitment);

    public static CheatValue<float> ItemTradingCostPercentage => 
        GetFloatValue(s => s.ItemTradingCostPercentage, s => s.ItemTradingCostPercentage, 100f);

    public static CheatValue<float> SellingPriceMultiplier => 
        GetFloatValue(s => s.SellingPriceMultiplier, s => s.SellingPriceMultiplier, 1f);

    public static CheatValue<float> TournamentMaximumBetMultiplier => 
        GetFloatValue(s => s.TournamentMaximumBetMultiplier, s => s.TournamentMaximumBetMultiplier, 1f);

    public static CheatValue<int> DailyFoodBonus => 
        GetIntValue(s => s.DailyFoodBonus, s => s.DailyFoodBonus, 0);

    public static CheatValue<int> DailyGarrisonBonus => 
        GetIntValue(s => s.DailyGarrisonBonus, s => s.DailyGarrisonBonus, 0);

    public static CheatValue<int> DailyMilitiaBonus => 
        GetIntValue(s => s.DailyMilitiaBonus, s => s.DailyMilitiaBonus, 0);

    public static CheatValue<int> DailyProsperityBonus => 
        GetIntValue(s => s.DailyProsperityBonus, s => s.DailyProsperityBonus, 0);

    public static CheatValue<int> DailyLoyaltyBonus => 
        GetIntValue(s => s.DailyLoyaltyBonus, s => s.DailyLoyaltyBonus, 0);

    public static CheatValue<int> DailySecurityBonus => 
        GetIntValue(s => s.DailySecurityBonus, s => s.DailySecurityBonus, 0);

    public static CheatValue<int> DailyHearthsBonus => 
        GetIntValue(s => s.DailyHearthsBonus, s => s.DailyHearthsBonus, 0);

    public static CheatValue<float> GarrisonWagesPercentage => 
        GetFloatValue(s => s.GarrisonWagesPercentage, s => s.GarrisonWagesPercentage, 100f);

    public static CheatValue<bool> NeverRequireCivilianEquipment => 
        GetBoolValue(s => s.NeverRequireCivilianEquipment, s => s.NeverRequireCivilianEquipment);

    public static CheatValue<float> ConstructionPowerMultiplier => 
        GetFloatValue(s => s.ConstructionPowerMultiplier, s => s.ConstructionPowerMultiplier, 1f);

    public static CheatValue<bool> NoBribeToEnterKeep => 
        GetBoolValue(s => s.NoBribeToEnterKeep, s => s.NoBribeToEnterKeep);

    public static CheatValue<bool> SettlementsNeverRebel => 
        GetBoolValue(s => s.SettlementsNeverRebel, s => s.SettlementsNeverRebel);

    public static CheatValue<float> SmithingEnergyCostPercentage => 
        GetFloatValue(s => s.SmithingEnergyCostPercentage, s => s.SmithingEnergyCostPercentage, 100f);

    public static CheatValue<bool> UnlockAllParts => 
        GetBoolValue(s => s.UnlockAllParts, s => s.UnlockAllParts);

    public static CheatValue<float> SmithingDifficultyPercentage => 
        GetFloatValue(s => s.SmithingDifficultyPercentage, s => s.SmithingDifficultyPercentage, 100f);

    public static CheatValue<float> SmithingCostPercentage => 
        GetFloatValue(s => s.SmithingCostPercentage, s => s.SmithingCostPercentage, 100f);

    public static CheatValue<int> CraftedWeaponHandlingBonus => 
        GetIntValue(s => s.CraftedWeaponHandlingBonus, s => s.CraftedWeaponHandlingBonus, 0);

    public static CheatValue<int> CraftedWeaponSwingDamageBonus => 
        GetIntValue(s => s.CraftedWeaponSwingDamageBonus, s => s.CraftedWeaponSwingDamageBonus, 0);

    public static CheatValue<int> CraftedWeaponSwingSpeedBonus => 
        GetIntValue(s => s.CraftedWeaponSwingSpeedBonus, s => s.CraftedWeaponSwingSpeedBonus, 0);

    public static CheatValue<int> CraftedWeaponThrustDamageBonus => 
        GetIntValue(s => s.CraftedWeaponThrustDamageBonus, s => s.CraftedWeaponThrustDamageBonus, 0);

    public static CheatValue<int> CraftedWeaponThrustSpeedBonus => 
        GetIntValue(s => s.CraftedWeaponThrustSpeedBonus, s => s.CraftedWeaponThrustSpeedBonus, 0);

    public static CheatValue<float> WorkshopBuyingCostPercentage => 
        GetFloatValue(s => s.WorkshopBuyingCostPercentage, s => s.WorkshopBuyingCostPercentage, 100f);

    public static CheatValue<float> WorkshopDailyExpensePercentage => 
        GetFloatValue(s => s.WorkshopDailyExpensePercentage, s => s.WorkshopDailyExpensePercentage, 100f);

    public static CheatValue<float> WorkshopSellingCostMultiplier => 
        GetFloatValue(s => s.WorkshopSellingCostMultiplier, s => s.WorkshopSellingCostMultiplier, 1f);

    public static CheatValue<AutoChoosePerk_Type> AutoChoosePerk => 
        GetDropdownValue(s => s.AutoChoosePerk, s => s.AutoChoosePerk, AutoChoosePerk_Type.No);

    public static CheatValue<Setting_Language> LanguageSetting => 
        GetDropdownValue(s => s.LanguageSetting, s => s.LanguageSetting, Setting_Language.English);

    public static CheatValue<int> Village_Init_Gold_Extra => 
        GetIntValue(s => s.Village_Init_Gold_Extra, s => s.Village_Init_Gold_Extra, 0);

    public static CheatValue<int> Town_Init_Gold_Extra => 
        GetIntValue(s => s.Town_Init_Gold_Extra, s => s.Town_Init_Gold_Extra, 0);


    public static CheatValue<bool> UnblockableThrust_player => 
        GetBoolValue(s => s.UnblockableThrust_player, s => s.UnblockableThrust_player);

    public static CheatValue<bool> UnblockableThrust_ally => 
        GetBoolValue(s => s.UnblockableThrust_ally, s => s.UnblockableThrust_ally);

    public static CheatValue<bool> UnblockableThrust_enemy => 
        GetBoolValue(s => s.UnblockableThrust_enemy, s => s.UnblockableThrust_enemy);

    public static CheatValue<int> xGang => 
        GetIntValue(s => s.xGang, s => s.xGang, 0);

    public static CheatValue<int> xArt => 
        GetIntValue(s => s.xArt, s => s.xArt, 0);

    public static CheatValue<int> xMerch => 
        GetIntValue(s => s.xMerch, s => s.xMerch, 0);

    public static CheatValue<int> xVill => 
        GetIntValue(s => s.xVill, s => s.xVill, 0);

    public static CheatValue<int> xRural => 
        GetIntValue(s => s.xRural, s => s.xRural, 0);


    public static CheatValue<int> AddMoneyThreshhold => 
        GetIntValue(s => s.AddMoneyThreshhold, s => s.AddMoneyThreshhold, 0);

    public static CheatValue<int> MaxAttr => 
        GetIntValue(s => s.MaxAttr, s => s.MaxAttr, 10);

    public static CheatValue<int> AddMoney_count => 
        GetIntValue(s => s.AddMoney_count, s => s.AddMoney_count, 0);

    public static CheatValue<bool> KeepDaughter => 
        GetBoolValue(s => s.KeepDaughter, s => s.KeepDaughter);


    public static CheatValue<bool> PlayerAlwaysCrush => 
        GetBoolValue(s => s.PlayerAlwaysCrush, s => s.PlayerAlwaysCrush);


    public static CheatValue<bool> AllyCrush => 
        GetBoolValue(s => s.AllyCrush, s => s.AllyCrush);

    public static CheatValue<bool> EnemyCrush => 
        GetBoolValue(s => s.EnemyCrush, s => s.EnemyCrush);

    public static CheatValue<bool> EnableEverYoung => 
        GetBoolValue(s => s.EnableEverYoung, s => s.EnableEverYoung);

    public static CheatValue<int> EverYoungSkillNeed => 
        GetIntValue(s => s.EverYoungSkillNeed, s => s.EverYoungSkillNeed, 400);

    public static CheatValue<float> VigorDmgPercent => 
        GetFloatValue(s => s.VigorDmgPercent, s => s.VigorDmgPercent, 0.02f);

    public static CheatValue<float> VigorArmorAdd => 
        GetFloatValue(s => s.VigorArmorAdd, s => s.VigorArmorAdd, 1f);

    public static CheatValue<float> VigorMountArmorAdd => 
        GetFloatValue(s => s.VigorMountArmorAdd, s => s.VigorMountArmorAdd, 1f);

    public static CheatValue<float> VigorShieldEndurancePercent => 
        GetFloatValue(s => s.VigorShieldEndurancePercent, s => s.VigorShieldEndurancePercent, 1f);

    public static CheatValue<float> VigorFinalDmgAdd => 
        GetFloatValue(s => s.VigorFinalDmgAdd, s => s.VigorFinalDmgAdd, 0.334f);

    public static CheatValue<float> VigorDmgTakenReduce => 
        GetFloatValue(s => s.VigorDmgTakenReduce, s => s.VigorDmgTakenReduce, 0.334f);

    public static CheatValue<int> VigorCrushThroughPositive => 
        GetIntValue(s => s.VigorCrushThroughPositive, s => s.VigorCrushThroughPositive, 5);

    public static CheatValue<int> VigorCrushThroughNegative => 
        GetIntValue(s => s.VigorCrushThroughNegative, s => s.VigorCrushThroughNegative, 10);

    public static CheatValue<float> IntelligenceAmmoAddPercent => 
        GetFloatValue(s => s.IntelligenceAmmoAddPercent, s => s.IntelligenceAmmoAddPercent, 0.1f);

    public static CheatValue<int> ControlAmmoNoConsumeRate => 
        GetIntValue(s => s.ControlAmmoNoConsumeRate, s => s.ControlAmmoNoConsumeRate, 5);

    public static CheatValue<float> ControlDropDmgReducePercent => 
        GetFloatValue(s => s.ControlDropDmgReducePercent, s => s.ControlDropDmgReducePercent, 0.05f);

    public static CheatValue<float> ControlAimStabilityPercent => 
        GetFloatValue(s => s.ControlAimStabilityPercent, s => s.ControlAimStabilityPercent, 0.1f);

    public static CheatValue<float> ControlMountManeuverPercent => 
        GetFloatValue(s => s.ControlMountManeuverPercent, s => s.ControlMountManeuverPercent, 0.05f);

    public static CheatValue<int> ControlCritRate => 
        GetIntValue(s => s.ControlCritRate, s => s.ControlCritRate, 2);

    public static CheatValue<int> ControlExemptionRate => 
        GetIntValue(s => s.ControlExemptionRate, s => s.ControlExemptionRate, 2);

    public static CheatValue<int> ControlPenetrateRate => 
        GetIntValue(s => s.ControlPenetrateRate, s => s.ControlPenetrateRate, 3);

    public static CheatValue<float> EnduranceHpAddPercent => 
        GetFloatValue(s => s.EnduranceHpAddPercent, s => s.EnduranceHpAddPercent, 0.05f);

    public static CheatValue<float> EnduranceHealRate => 
        GetFloatValue(s => s.EnduranceHealRate, s => s.EnduranceHealRate, 0.05f);

    public static CheatValue<float> EnduranceStaggerPercent => 
        GetFloatValue(s => s.EnduranceStaggerPercent, s => s.EnduranceStaggerPercent, 0.2f);

    public static CheatValue<float> EnduranceWalkSpeedPercent => 
        GetFloatValue(s => s.EnduranceWalkSpeedPercent, s => s.EnduranceWalkSpeedPercent, 0.01f);

    public static CheatValue<float> EnduranceMountSpeedPercent => 
        GetFloatValue(s => s.EnduranceMountSpeedPercent, s => s.EnduranceMountSpeedPercent, 0.025f);

    public static CheatValue<float> CunningPrisonerRecruitSpeedPercent => 
        GetFloatValue(s => s.CunningPrisonerRecruitSpeedPercent, s => s.CunningPrisonerRecruitSpeedPercent, 0.1f);

    public static CheatValue<float> CunningPrisonerCapacityPercent => 
        GetFloatValue(s => s.CunningPrisonerCapacityPercent, s => s.CunningPrisonerCapacityPercent, 0.1f);

    public static CheatValue<float> CunningRaidSpeedPercent => 
        GetFloatValue(s => s.CunningRaidSpeedPercent, s => s.CunningRaidSpeedPercent, 0.1f);

    public static CheatValue<float> CunningPartySpeedAdd => 
        GetFloatValue(s => s.CunningPartySpeedAdd, s => s.CunningPartySpeedAdd, 0.1f);

    public static CheatValue<float> CunningCompanionCapacityAdd => 
        GetFloatValue(s => s.CunningCompanionCapacityAdd, s => s.CunningCompanionCapacityAdd, 0.2f);


    public static CheatValue<float> SocialBoundary => 
        GetFloatValue(s => s.SocialBoundary, s => s.SocialBoundary, 3.5f);

    public static CheatValue<float> SocialHearthAdd => 
        GetFloatValue(s => s.SocialHearthAdd, s => s.SocialHearthAdd, 0.25f);

    public static CheatValue<float> SocialSettlementLoyaltyAdd => 
        GetFloatValue(s => s.SocialSettlementLoyaltyAdd, s => s.SocialSettlementLoyaltyAdd, 0.25f);

    public static CheatValue<float> SocialMilitiaAdd => 
        GetFloatValue(s => s.SocialMilitiaAdd, s => s.SocialMilitiaAdd, 0.5f);

    public static CheatValue<float> SocialRecruitSpeedPercent => 
        GetFloatValue(s => s.SocialRecruitSpeedPercent, s => s.SocialRecruitSpeedPercent, 0.05f);

    public static CheatValue<float> SocialTaxPercent => 
        GetFloatValue(s => s.SocialTaxPercent, s => s.SocialTaxPercent, 0.05f);

    public static CheatValue<float> SocialWorkshopProductionPercent => 
        GetFloatValue(s => s.SocialWorkshopProductionPercent, s => s.SocialWorkshopProductionPercent, 0.1f);

    public static CheatValue<float> SocialCompanionCapacityAdd => 
        GetFloatValue(s => s.SocialCompanionCapacityAdd, s => s.SocialCompanionCapacityAdd, 0.2f);

    public static CheatValue<float> IntelligenceBoundary => 
        GetFloatValue(s => s.IntelligenceBoundary, s => s.IntelligenceBoundary, 3.5f);

    public static CheatValue<float> IntelligenceExpRate => 
        GetFloatValue(s => s.IntelligenceExpRate, s => s.IntelligenceExpRate, 0.05f);

    public static CheatValue<float> IntelligenceSiegeEndurancePercent => 
        GetFloatValue(s => s.IntelligenceSiegeEndurancePercent, s => s.IntelligenceSiegeEndurancePercent, 0.1f);

    public static CheatValue<float> IntelligenceWallEndurancePercent => 
        GetFloatValue(s => s.IntelligenceWallEndurancePercent, s => s.IntelligenceWallEndurancePercent, 0.1f);

    public static CheatValue<float> IntelligenceBallistaAdd => 
        GetFloatValue(s => s.IntelligenceBallistaAdd, s => s.IntelligenceBallistaAdd, 0.334f);

    public static CheatValue<float> IntelligenceLeaderSettlementFoodPercent => 
        GetFloatValue(s => s.IntelligenceLeaderSettlementFoodPercent, s => s.IntelligenceLeaderSettlementFoodPercent, 0.5f);

    public static CheatValue<float> IntelligenceGovernorSettlementFoodPercent => 
        GetFloatValue(s => s.IntelligenceGovernorSettlementFoodPercent, s => s.IntelligenceGovernorSettlementFoodPercent, 1f);

    public static CheatValue<float> IntelligenceProsperityFoodCostReducePercent => 
        GetFloatValue(s => s.IntelligenceProsperityFoodCostReducePercent, s => s.IntelligenceProsperityFoodCostReducePercent, 0.075f);

    public static CheatValue<float> IntelligenceGarrisonWageReducePercent => 
        GetFloatValue(s => s.IntelligenceGarrisonWageReducePercent, s => s.IntelligenceGarrisonWageReducePercent, 0.05f);

    public static CheatValue<float> IntelligenceWorkshopProductionPercent => 
        GetFloatValue(s => s.IntelligenceWorkshopProductionPercent, s => s.IntelligenceWorkshopProductionPercent, 0.25f);

    public static CheatValue<bool> EnableDailyGainXp => 
        GetBoolValue(s => s.EnableDailyGainXp, s => s.EnableDailyGainXp);

    public static CheatValue<float> CombatAttributeRatePlayer => 
        GetFloatValue(s => s.CombatAttributeRatePlayer, s => s.CombatAttributeRatePlayer, 0f);

    public static CheatValue<float> CombatAttributeRateClanMember => 
        GetFloatValue(s => s.CombatAttributeRateClanMember, s => s.CombatAttributeRateClanMember, 0f);

    public static CheatValue<float> CombatAttributeRateOther => 
        GetFloatValue(s => s.CombatAttributeRateOther, s => s.CombatAttributeRateOther, 0f);

    public static CheatValue<float> StrategyAttributeRatePlayer => 
        GetFloatValue(s => s.StrategyAttributeRatePlayer, s => s.StrategyAttributeRatePlayer, 0f);

    public static CheatValue<float> StrategyAttributeRateClanMember => 
        GetFloatValue(s => s.StrategyAttributeRateClanMember, s => s.StrategyAttributeRateClanMember, 0f);

    public static CheatValue<float> StrategyAttributeRateOther => 
        GetFloatValue(s => s.StrategyAttributeRateOther, s => s.StrategyAttributeRateOther, 0f);

    public static CheatValue<bool> TestMode => 
        GetBoolValue(s => s.TestMode, s => s.TestMode);
}