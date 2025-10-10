using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Base.PerCampaign;
using System;
using UFO.Extension;

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
        public const bool PlayerAlwaysCrush = true;

        public const bool InfiniteMomentum = false;

        public const bool KeepDaughter = true;

        public const Setting_Language LanguageSetting = Setting_Language.English;

        public const int AddMoneyThreshhold =0;
        public const int AddMoney_count = 0;

        public const int MaxAttr = 10;


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

        public const float WorkshopUpgradeCostPercentage = 100f;

        public const float WorkshopSellingCostMultiplier = 1f;

        public const bool EveryoneBuysWorkshops = false;

        // Hero Enhance Settings

        public const bool EnableEverYoung = true;

        public const int EverYoungSkillNeed = 400;

        public const AutoChoosePerk_Type AutoChoosePerk = AutoChoosePerk_Type.Clan;

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

        public const bool EnableDailyGainXp = true;

        public const float CombatAttributeRatePlayer = 1f;

        public const float CombatAttributeRateClanMember = 0.5f;

        public const float CombatAttributeRateOther = 0f;

        public const float StrategyAttributeRatePlayer = 1f;

        public const float StrategyAttributeRateClanMember = 0.5f;

        public const float StrategyAttributeRateOther = 0f;

        public const bool TestMode = false;

    }

    private static bool IsPerCampaignInstanceLoaded => PerCampaignSettings<BannerlordCheatsPerCampaignSettings>.Instance != null;

    private static BannerlordCheatsGlobalSettings GlobalInstance => GlobalSettings<BannerlordCheatsGlobalSettings>.Instance ?? throw new InvalidOperationException("Should have checked if global instance is loaded!");

    private static BannerlordCheatsPerCampaignSettings PerCampaignInstance => PerCampaignSettings<BannerlordCheatsPerCampaignSettings>.Instance ?? throw new InvalidOperationException("Should have checked if per-campaign instance is loaded!");

    public static CheatValue<bool> EnableHotkeys => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnableHotkeys) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.EnableHotkeys) : (GlobalInstance.EnableHotkeys ? new CheatValue<bool>(isChanged: true, GlobalInstance.EnableHotkeys) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> EnableHotkeyTips => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnableHotkeyTips) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.EnableHotkeyTips) : (GlobalInstance.EnableHotkeyTips ? new CheatValue<bool>(isChanged: true, GlobalInstance.EnableHotkeyTips) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> MapSpeedMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.MapSpeedMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.MapSpeedMultiplier) : ((GlobalInstance.MapSpeedMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.MapSpeedMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> MapVisibilityMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.MapVisibilityMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.MapVisibilityMultiplier) : ((GlobalInstance.MapVisibilityMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.MapVisibilityMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> NpcMapSpeedPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NpcMapSpeedPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.NpcMapSpeedPercentage) : ((GlobalInstance.NpcMapSpeedPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.NpcMapSpeedPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> PartyInvisibleOnMap => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyInvisibleOnMap) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PartyInvisibleOnMap) : (GlobalInstance.PartyInvisibleOnMap ? new CheatValue<bool>(isChanged: true, GlobalInstance.PartyInvisibleOnMap) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> CaravansInvisibleOnMap => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CaravansInvisibleOnMap) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.CaravansInvisibleOnMap) : (GlobalInstance.CaravansInvisibleOnMap ? new CheatValue<bool>(isChanged: true, GlobalInstance.CaravansInvisibleOnMap) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> DamageTakenPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DamageTakenPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.DamageTakenPercentage) : ((GlobalInstance.DamageTakenPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.DamageTakenPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> Invincible => (IsPerCampaignInstanceLoaded && PerCampaignInstance.Invincible) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.Invincible) : (GlobalInstance.Invincible ? new CheatValue<bool>(isChanged: true, GlobalInstance.Invincible) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> PlayerHorseInvincible => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PlayerHorseInvincible) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PlayerHorseInvincible) : (GlobalInstance.PlayerHorseInvincible ? new CheatValue<bool>(isChanged: true, GlobalInstance.PlayerHorseInvincible) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> OneHitKill => (IsPerCampaignInstanceLoaded && PerCampaignInstance.OneHitKill) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.OneHitKill) : (GlobalInstance.OneHitKill ? new CheatValue<bool>(isChanged: true, GlobalInstance.OneHitKill) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> AlwaysCrushThroughShields => (IsPerCampaignInstanceLoaded && PerCampaignInstance.AlwaysCrushThroughShields) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.AlwaysCrushThroughShields) : (GlobalInstance.AlwaysCrushThroughShields ? new CheatValue<bool>(isChanged: true, GlobalInstance.AlwaysCrushThroughShields) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> SliceThroughEveryone => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SliceThroughEveryone) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.SliceThroughEveryone) : (GlobalInstance.SliceThroughEveryone ? new CheatValue<bool>(isChanged: true, GlobalInstance.SliceThroughEveryone) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> HealthRegeneration => (IsPerCampaignInstanceLoaded && PerCampaignInstance.HealthRegeneration != 0f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.HealthRegeneration) : ((GlobalInstance.HealthRegeneration != 0f) ? new CheatValue<float>(isChanged: true, GlobalInstance.HealthRegeneration) : new CheatValue<float>(isChanged: false, 0f));

    public static CheatValue<bool> InfiniteAmmo => (IsPerCampaignInstanceLoaded && PerCampaignInstance.InfiniteAmmo) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.InfiniteAmmo) : (GlobalInstance.InfiniteAmmo ? new CheatValue<bool>(isChanged: true, GlobalInstance.InfiniteAmmo) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> DamageMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DamageMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.DamageMultiplier) : ((GlobalInstance.DamageMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.DamageMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> AlwaysKnockDown => (IsPerCampaignInstanceLoaded && PerCampaignInstance.AlwaysKnockDown) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.AlwaysKnockDown) : (GlobalInstance.AlwaysKnockDown ? new CheatValue<bool>(isChanged: true, GlobalInstance.AlwaysKnockDown) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NeverKnockedBackByAttacks => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NeverKnockedBackByAttacks) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NeverKnockedBackByAttacks) : (GlobalInstance.NeverKnockedBackByAttacks ? new CheatValue<bool>(isChanged: true, GlobalInstance.NeverKnockedBackByAttacks) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NoStuckArrows => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoStuckArrows) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoStuckArrows) : (GlobalInstance.NoStuckArrows ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoStuckArrows) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> InstantCrossbowReload => (IsPerCampaignInstanceLoaded && PerCampaignInstance.InstantCrossbowReload) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.InstantCrossbowReload) : (GlobalInstance.InstantCrossbowReload ? new CheatValue<bool>(isChanged: true, GlobalInstance.InstantCrossbowReload) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<KnockoutOrKilled> PartyKnockoutOrKilled => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, PerCampaignInstance.PartyKnockoutOrKilled.GetValue()) : ((GlobalInstance.PartyKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, GlobalInstance.PartyKnockoutOrKilled.GetValue()) : new CheatValue<KnockoutOrKilled>(isChanged: false, KnockoutOrKilled.Default));

    public static CheatValue<KnockoutOrKilled> CompanionsKnockoutOrKilled => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CompanionsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, PerCampaignInstance.CompanionsKnockoutOrKilled.GetValue()) : ((GlobalInstance.CompanionsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, GlobalInstance.CompanionsKnockoutOrKilled.GetValue()) : new CheatValue<KnockoutOrKilled>(isChanged: false, KnockoutOrKilled.Default));

    public static CheatValue<bool> PartyInvincible => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyInvincible) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PartyInvincible) : (GlobalInstance.PartyInvincible ? new CheatValue<bool>(isChanged: true, GlobalInstance.PartyInvincible) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> PartyHeroesInvincible => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyHeroesInvincible) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PartyHeroesInvincible) : (GlobalInstance.PartyHeroesInvincible ? new CheatValue<bool>(isChanged: true, GlobalInstance.PartyHeroesInvincible) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> PartyDamageTakenPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyDamageTakenPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.PartyDamageTakenPercentage) : ((GlobalInstance.PartyDamageTakenPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.PartyDamageTakenPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> PartyOneHitKill => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyOneHitKill) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PartyOneHitKill) : (GlobalInstance.PartyOneHitKill ? new CheatValue<bool>(isChanged: true, GlobalInstance.PartyOneHitKill) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NoRunningAway => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoRunningAway) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoRunningAway) : (GlobalInstance.NoRunningAway ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoRunningAway) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> PartyHealthRegeneration => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyHealthRegeneration != 0f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.PartyHealthRegeneration) : ((GlobalInstance.PartyHealthRegeneration != 0f) ? new CheatValue<float>(isChanged: true, GlobalInstance.PartyHealthRegeneration) : new CheatValue<float>(isChanged: false, 0f));

    public static CheatValue<bool> PartyInfiniteAmmo => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyInfiniteAmmo) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PartyInfiniteAmmo) : (GlobalInstance.PartyInfiniteAmmo ? new CheatValue<bool>(isChanged: true, GlobalInstance.PartyInfiniteAmmo) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> PartyDamageMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyDamageMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.PartyDamageMultiplier) : ((GlobalInstance.PartyDamageMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.PartyDamageMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> NoFriendlyFire => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoFriendlyFire) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoFriendlyFire) : (GlobalInstance.NoFriendlyFire ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoFriendlyFire) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<KnockoutOrKilled> FriendlyLordsKnockoutOrKilled => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FriendlyLordsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, PerCampaignInstance.FriendlyLordsKnockoutOrKilled.GetValue()) : ((GlobalInstance.FriendlyLordsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, GlobalInstance.FriendlyLordsKnockoutOrKilled.GetValue()) : new CheatValue<KnockoutOrKilled>(isChanged: false, KnockoutOrKilled.Default));

    public static CheatValue<KnockoutOrKilled> EnemyLordsKnockoutOrKilled => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnemyLordsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, PerCampaignInstance.EnemyLordsKnockoutOrKilled.GetValue()) : ((GlobalInstance.EnemyLordsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, GlobalInstance.EnemyLordsKnockoutOrKilled.GetValue()) : new CheatValue<KnockoutOrKilled>(isChanged: false, KnockoutOrKilled.Default));

    public static CheatValue<KnockoutOrKilled> EnemyTroopsKnockoutOrKilled => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnemyTroopsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, PerCampaignInstance.EnemyTroopsKnockoutOrKilled.GetValue()) : ((GlobalInstance.EnemyTroopsKnockoutOrKilled.GetValue() != KnockoutOrKilled.Default) ? new CheatValue<KnockoutOrKilled>(isChanged: true, GlobalInstance.EnemyTroopsKnockoutOrKilled.GetValue()) : new CheatValue<KnockoutOrKilled>(isChanged: false, KnockoutOrKilled.Default));

    public static CheatValue<bool> EnemiesNoRunningAway => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnemiesNoRunningAway) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.EnemiesNoRunningAway) : (GlobalInstance.EnemiesNoRunningAway ? new CheatValue<bool>(isChanged: true, GlobalInstance.EnemiesNoRunningAway) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> EnemyDamagePercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnemyDamagePercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.EnemyDamagePercentage) : ((GlobalInstance.EnemyDamagePercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.EnemyDamagePercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> RenownRewardMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.RenownRewardMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.RenownRewardMultiplier) : ((GlobalInstance.RenownRewardMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.RenownRewardMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> InfluenceRewardMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.InfluenceRewardMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.InfluenceRewardMultiplier) : ((GlobalInstance.InfluenceRewardMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.InfluenceRewardMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> AlwaysWinBattleSimulation => (IsPerCampaignInstanceLoaded && PerCampaignInstance.AlwaysWinBattleSimulation) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.AlwaysWinBattleSimulation) : (GlobalInstance.AlwaysWinBattleSimulation ? new CheatValue<bool>(isChanged: true, GlobalInstance.AlwaysWinBattleSimulation) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NoTroopSacrifice => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoTroopSacrifice) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoTroopSacrifice) : (GlobalInstance.NoTroopSacrifice ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoTroopSacrifice) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<int> BanditHideoutTroopLimit => (IsPerCampaignInstanceLoaded && PerCampaignInstance.BanditHideoutTroopLimit != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.BanditHideoutTroopLimit) : ((GlobalInstance.BanditHideoutTroopLimit != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.BanditHideoutTroopLimit) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<float> CombatZoomMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CombatZoomMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.CombatZoomMultiplier) : ((GlobalInstance.CombatZoomMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.CombatZoomMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<int> ExtraInventoryCapacity => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraInventoryCapacity != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraInventoryCapacity) : ((GlobalInstance.ExtraInventoryCapacity != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraInventoryCapacity) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<bool> NativeItemSpawning => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NativeItemSpawning) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NativeItemSpawning) : (GlobalInstance.NativeItemSpawning ? new CheatValue<bool>(isChanged: true, GlobalInstance.NativeItemSpawning) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<int> ExtraPartyMemberSize => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraPartyMemberSize != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraPartyMemberSize) : ((GlobalInstance.ExtraPartyMemberSize != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraPartyMemberSize) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> ExtraPartyPrisonerSize => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraPartyPrisonerSize != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraPartyPrisonerSize) : ((GlobalInstance.ExtraPartyPrisonerSize != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraPartyPrisonerSize) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> ExtraPartyMorale => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraPartyMorale != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraPartyMorale) : ((GlobalInstance.ExtraPartyMorale != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraPartyMorale) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<bool> InstantEscape => (IsPerCampaignInstanceLoaded && PerCampaignInstance.InstantEscape) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.InstantEscape) : (GlobalInstance.InstantEscape ? new CheatValue<bool>(isChanged: true, GlobalInstance.InstantEscape) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> FoodConsumptionPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FoodConsumptionPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.FoodConsumptionPercentage) : ((GlobalInstance.FoodConsumptionPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.FoodConsumptionPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> TroopWagesPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.TroopWagesPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.TroopWagesPercentage) : ((GlobalInstance.TroopWagesPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.TroopWagesPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> FreeTroopUpgrades => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FreeTroopUpgrades) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.FreeTroopUpgrades) : (GlobalInstance.FreeTroopUpgrades ? new CheatValue<bool>(isChanged: true, GlobalInstance.FreeTroopUpgrades) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> FreeCompanionHiring => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FreeCompanionHiring) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.FreeCompanionHiring) : (GlobalInstance.FreeCompanionHiring ? new CheatValue<bool>(isChanged: true, GlobalInstance.FreeCompanionHiring) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> InstantPrisonerRecruitment => (IsPerCampaignInstanceLoaded && PerCampaignInstance.InstantPrisonerRecruitment) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.InstantPrisonerRecruitment) : (GlobalInstance.InstantPrisonerRecruitment ? new CheatValue<bool>(isChanged: true, GlobalInstance.InstantPrisonerRecruitment) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NoPrisonerEscape => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoPrisonerEscape) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoPrisonerEscape) : (GlobalInstance.NoPrisonerEscape ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoPrisonerEscape) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> PartyHealingMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PartyHealingMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.PartyHealingMultiplier) : ((GlobalInstance.PartyHealingMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.PartyHealingMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<int> ExtraCompanionLimit => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraCompanionLimit != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraCompanionLimit) : ((GlobalInstance.ExtraCompanionLimit != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraCompanionLimit) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> ExtraClanPartyLimit => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraClanPartyLimit != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraClanPartyLimit) : ((GlobalInstance.ExtraClanPartyLimit != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraClanPartyLimit) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> ExtraClanPartySize => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExtraClanPartySize != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.ExtraClanPartySize) : ((GlobalInstance.ExtraClanPartySize != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.ExtraClanPartySize) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<float> RelationGainAfterBattleMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.RelationGainAfterBattleMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.RelationGainAfterBattleMultiplier) : ((GlobalInstance.RelationGainAfterBattleMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.RelationGainAfterBattleMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> PerfectRelationships => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PerfectRelationships) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PerfectRelationships) : (GlobalInstance.PerfectRelationships ? new CheatValue<bool>(isChanged: true, GlobalInstance.PerfectRelationships) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NeverDieOfOldAge => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NeverDieOfOldAge) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NeverDieOfOldAge) : (GlobalInstance.NeverDieOfOldAge ? new CheatValue<bool>(isChanged: true, GlobalInstance.NeverDieOfOldAge) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> BarterOfferAlwaysAccepted => (IsPerCampaignInstanceLoaded && PerCampaignInstance.BarterOfferAlwaysAccepted) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.BarterOfferAlwaysAccepted) : (GlobalInstance.BarterOfferAlwaysAccepted ? new CheatValue<bool>(isChanged: true, GlobalInstance.BarterOfferAlwaysAccepted) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NoBarterCooldown => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoBarterCooldown) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoBarterCooldown) : (GlobalInstance.NoBarterCooldown ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoBarterCooldown) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> ConversationAlwaysSuccessful => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ConversationAlwaysSuccessful) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.ConversationAlwaysSuccessful) : (GlobalInstance.ConversationAlwaysSuccessful ? new CheatValue<bool>(isChanged: true, GlobalInstance.ConversationAlwaysSuccessful) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> PerfectAttraction => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PerfectAttraction) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PerfectAttraction) : (GlobalInstance.PerfectAttraction ? new CheatValue<bool>(isChanged: true, GlobalInstance.PerfectAttraction) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> AllowSameSexMarriage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.AllowSameSexMarriage) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.AllowSameSexMarriage) : (GlobalInstance.AllowSameSexMarriage ? new CheatValue<bool>(isChanged: true, GlobalInstance.AllowSameSexMarriage) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> PregnancyChanceMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.PregnancyChanceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.PregnancyChanceMultiplier) : ((GlobalInstance.PregnancyChanceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.PregnancyChanceMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<int> AdjustPregnancyDuration => (IsPerCampaignInstanceLoaded && PerCampaignInstance.AdjustPregnancyDuration != 36) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.AdjustPregnancyDuration) : ((GlobalInstance.AdjustPregnancyDuration != 36) ? new CheatValue<int>(isChanged: true, GlobalInstance.AdjustPregnancyDuration) : new CheatValue<int>(isChanged: false, 36));

    public static CheatValue<float> KingdomDecisionWeightMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.KingdomDecisionWeightMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.KingdomDecisionWeightMultiplier) : ((GlobalInstance.KingdomDecisionWeightMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.KingdomDecisionWeightMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> NoRelationshipLossOnDecision => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoRelationshipLossOnDecision) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoRelationshipLossOnDecision) : (GlobalInstance.NoRelationshipLossOnDecision ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoRelationshipLossOnDecision) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> NoCrimeRatingForCrimes => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoCrimeRatingForCrimes) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoCrimeRatingForCrimes) : (GlobalInstance.NoCrimeRatingForCrimes ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoCrimeRatingForCrimes) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> DecisionOverrideInfluenceCostPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DecisionOverrideInfluenceCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.DecisionOverrideInfluenceCostPercentage) : ((GlobalInstance.DecisionOverrideInfluenceCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.DecisionOverrideInfluenceCostPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> ExperienceMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.ExperienceMultiplier) : ((GlobalInstance.ExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.ExperienceMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> CompanionExperienceMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CompanionExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.CompanionExperienceMultiplier) : ((GlobalInstance.CompanionExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.CompanionExperienceMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> ClanExperienceMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ClanExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.ClanExperienceMultiplier) : ((GlobalInstance.ClanExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.ClanExperienceMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> LearningRateMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.LearningRateMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.LearningRateMultiplier) : ((GlobalInstance.LearningRateMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.LearningRateMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> CompanionLearningRateMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CompanionLearningRateMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.CompanionLearningRateMultiplier) : ((GlobalInstance.CompanionLearningRateMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.CompanionLearningRateMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> LearningLimitMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.LearningLimitMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.LearningLimitMultiplier) : ((GlobalInstance.LearningLimitMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.LearningLimitMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> TroopExperienceMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.TroopExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.TroopExperienceMultiplier) : ((GlobalInstance.TroopExperienceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.TroopExperienceMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> FreeFocusPointAssignment => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FreeFocusPointAssignment) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.FreeFocusPointAssignment) : (GlobalInstance.FreeFocusPointAssignment ? new CheatValue<bool>(isChanged: true, GlobalInstance.FreeFocusPointAssignment) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> SiegeBuildingSpeedMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SiegeBuildingSpeedMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.SiegeBuildingSpeedMultiplier) : ((GlobalInstance.SiegeBuildingSpeedMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.SiegeBuildingSpeedMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> EnemySiegeBuildingSpeedPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnemySiegeBuildingSpeedPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.EnemySiegeBuildingSpeedPercentage) : ((GlobalInstance.EnemySiegeBuildingSpeedPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.EnemySiegeBuildingSpeedPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> FactionArmyCohesionLossPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FactionArmyCohesionLossPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.FactionArmyCohesionLossPercentage) : ((GlobalInstance.FactionArmyCohesionLossPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.FactionArmyCohesionLossPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> ArmyCohesionLossPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ArmyCohesionLossPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.ArmyCohesionLossPercentage) : ((GlobalInstance.ArmyCohesionLossPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.ArmyCohesionLossPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> ArmyFoodConsumptionPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ArmyFoodConsumptionPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.ArmyFoodConsumptionPercentage) : ((GlobalInstance.ArmyFoodConsumptionPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.ArmyFoodConsumptionPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> VillagesNeverRaided => (IsPerCampaignInstanceLoaded && PerCampaignInstance.VillagesNeverRaided) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.VillagesNeverRaided) : (GlobalInstance.VillagesNeverRaided ? new CheatValue<bool>(isChanged: true, GlobalInstance.VillagesNeverRaided) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> DisguiseAlwaysWorks => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DisguiseAlwaysWorks) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.DisguiseAlwaysWorks) : (GlobalInstance.DisguiseAlwaysWorks ? new CheatValue<bool>(isChanged: true, GlobalInstance.DisguiseAlwaysWorks) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> FreeTroopRecruitment => (IsPerCampaignInstanceLoaded && PerCampaignInstance.FreeTroopRecruitment) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.FreeTroopRecruitment) : (GlobalInstance.FreeTroopRecruitment ? new CheatValue<bool>(isChanged: true, GlobalInstance.FreeTroopRecruitment) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> ItemTradingCostPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ItemTradingCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.ItemTradingCostPercentage) : ((GlobalInstance.ItemTradingCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.ItemTradingCostPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> SellingPriceMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SellingPriceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.SellingPriceMultiplier) : ((GlobalInstance.SellingPriceMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.SellingPriceMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<float> TournamentMaximumBetMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.TournamentMaximumBetMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.TournamentMaximumBetMultiplier) : ((GlobalInstance.TournamentMaximumBetMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.TournamentMaximumBetMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<int> DailyFoodBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailyFoodBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailyFoodBonus) : ((GlobalInstance.DailyFoodBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailyFoodBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> DailyGarrisonBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailyGarrisonBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailyGarrisonBonus) : ((GlobalInstance.DailyGarrisonBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailyGarrisonBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> DailyMilitiaBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailyMilitiaBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailyMilitiaBonus) : ((GlobalInstance.DailyMilitiaBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailyMilitiaBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> DailyProsperityBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailyProsperityBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailyProsperityBonus) : ((GlobalInstance.DailyProsperityBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailyProsperityBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> DailyLoyaltyBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailyLoyaltyBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailyLoyaltyBonus) : ((GlobalInstance.DailyLoyaltyBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailyLoyaltyBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> DailySecurityBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailySecurityBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailySecurityBonus) : ((GlobalInstance.DailySecurityBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailySecurityBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> DailyHearthsBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.DailyHearthsBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.DailyHearthsBonus) : ((GlobalInstance.DailyHearthsBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.DailyHearthsBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<float> GarrisonWagesPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.GarrisonWagesPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.GarrisonWagesPercentage) : ((GlobalInstance.GarrisonWagesPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.GarrisonWagesPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> NeverRequireCivilianEquipment => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NeverRequireCivilianEquipment) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NeverRequireCivilianEquipment) : (GlobalInstance.NeverRequireCivilianEquipment ? new CheatValue<bool>(isChanged: true, GlobalInstance.NeverRequireCivilianEquipment) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> ConstructionPowerMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.ConstructionPowerMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.ConstructionPowerMultiplier) : ((GlobalInstance.ConstructionPowerMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.ConstructionPowerMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<bool> NoBribeToEnterKeep => (IsPerCampaignInstanceLoaded && PerCampaignInstance.NoBribeToEnterKeep) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.NoBribeToEnterKeep) : (GlobalInstance.NoBribeToEnterKeep ? new CheatValue<bool>(isChanged: true, GlobalInstance.NoBribeToEnterKeep) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<bool> SettlementsNeverRebel => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SettlementsNeverRebel) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.SettlementsNeverRebel) : (GlobalInstance.SettlementsNeverRebel ? new CheatValue<bool>(isChanged: true, GlobalInstance.SettlementsNeverRebel) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> SmithingEnergyCostPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SmithingEnergyCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.SmithingEnergyCostPercentage) : ((GlobalInstance.SmithingEnergyCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.SmithingEnergyCostPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<bool> UnlockAllParts => (IsPerCampaignInstanceLoaded && PerCampaignInstance.UnlockAllParts) ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.UnlockAllParts) : (GlobalInstance.UnlockAllParts ? new CheatValue<bool>(isChanged: true, GlobalInstance.UnlockAllParts) : new CheatValue<bool>(isChanged: false, value: false));

    public static CheatValue<float> SmithingDifficultyPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SmithingDifficultyPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.SmithingDifficultyPercentage) : ((GlobalInstance.SmithingDifficultyPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.SmithingDifficultyPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> SmithingCostPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.SmithingCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.SmithingCostPercentage) : ((GlobalInstance.SmithingCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.SmithingCostPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<int> CraftedWeaponHandlingBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CraftedWeaponHandlingBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.CraftedWeaponHandlingBonus) : ((GlobalInstance.CraftedWeaponHandlingBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.CraftedWeaponHandlingBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> CraftedWeaponSwingDamageBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CraftedWeaponSwingDamageBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.CraftedWeaponSwingDamageBonus) : ((GlobalInstance.CraftedWeaponSwingDamageBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.CraftedWeaponSwingDamageBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> CraftedWeaponSwingSpeedBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CraftedWeaponSwingSpeedBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.CraftedWeaponSwingSpeedBonus) : ((GlobalInstance.CraftedWeaponSwingSpeedBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.CraftedWeaponSwingSpeedBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> CraftedWeaponThrustDamageBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CraftedWeaponThrustDamageBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.CraftedWeaponThrustDamageBonus) : ((GlobalInstance.CraftedWeaponThrustDamageBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.CraftedWeaponThrustDamageBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<int> CraftedWeaponThrustSpeedBonus => (IsPerCampaignInstanceLoaded && PerCampaignInstance.CraftedWeaponThrustSpeedBonus != 0) ? new CheatValue<int>(isChanged: true, PerCampaignInstance.CraftedWeaponThrustSpeedBonus) : ((GlobalInstance.CraftedWeaponThrustSpeedBonus != 0) ? new CheatValue<int>(isChanged: true, GlobalInstance.CraftedWeaponThrustSpeedBonus) : new CheatValue<int>(isChanged: false, 0));

    public static CheatValue<float> WorkshopBuyingCostPercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.WorkshopBuyingCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.WorkshopBuyingCostPercentage) : ((GlobalInstance.WorkshopBuyingCostPercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.WorkshopBuyingCostPercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> WorkshopDailyExpensePercentage => (IsPerCampaignInstanceLoaded && PerCampaignInstance.WorkshopDailyExpensePercentage != 100f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.WorkshopDailyExpensePercentage) : ((GlobalInstance.WorkshopDailyExpensePercentage != 100f) ? new CheatValue<float>(isChanged: true, GlobalInstance.WorkshopDailyExpensePercentage) : new CheatValue<float>(isChanged: false, 100f));

    public static CheatValue<float> WorkshopSellingCostMultiplier => (IsPerCampaignInstanceLoaded && PerCampaignInstance.WorkshopSellingCostMultiplier != 1f) ? new CheatValue<float>(isChanged: true, PerCampaignInstance.WorkshopSellingCostMultiplier) : ((GlobalInstance.WorkshopSellingCostMultiplier != 1f) ? new CheatValue<float>(isChanged: true, GlobalInstance.WorkshopSellingCostMultiplier) : new CheatValue<float>(isChanged: false, 1f));

    public static CheatValue<AutoChoosePerk_Type> AutoChoosePerk => (IsPerCampaignInstanceLoaded && PerCampaignInstance.AutoChoosePerk.GetValue() != AutoChoosePerk_Type.Clan) ? new CheatValue<AutoChoosePerk_Type>(isChanged: true, PerCampaignInstance.AutoChoosePerk.GetValue()) : ((GlobalInstance.AutoChoosePerk.GetValue() != AutoChoosePerk_Type.Clan) ? new CheatValue<AutoChoosePerk_Type>(isChanged: true, GlobalInstance.AutoChoosePerk.GetValue()) : new CheatValue<AutoChoosePerk_Type>(isChanged: false, AutoChoosePerk_Type.Clan));

    public static CheatValue<Setting_Language> LanguageSetting => (IsPerCampaignInstanceLoaded && PerCampaignInstance.LanguageSetting.GetValue() != Setting_Language.English) ? new CheatValue<Setting_Language>(isChanged: true, PerCampaignInstance.LanguageSetting.GetValue()) : ((GlobalInstance.LanguageSetting.GetValue() != Setting_Language.English) ? new CheatValue<Setting_Language>(isChanged: true, GlobalInstance.LanguageSetting.GetValue()) : new CheatValue<Setting_Language>(isChanged: false, Setting_Language.English));


    //        public static CheatValue<bool> InfiniteMomentum =>
    //(IsPerCampaignInstanceLoaded && PerCampaignInstance.InfiniteMomentum != true)
    //? new CheatValue<bool>(isChanged: true, PerCampaignInstance.InfiniteMomentum)
    //: ((GlobalInstance.InfiniteMomentum != true)
    //? new CheatValue<bool>(isChanged: true, GlobalInstance.InfiniteMomentum)
    //: new CheatValue<bool>(isChanged: false, true));

    public static CheatValue<int> AddMoneyThreshhold =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.AddMoneyThreshhold != 5)
    ? new CheatValue<int>(true, PerCampaignInstance.AddMoneyThreshhold)
    : ((GlobalInstance.AddMoneyThreshhold != 5)
        ? new CheatValue<int>(true, GlobalInstance.AddMoneyThreshhold)
        : new CheatValue<int>(false, 5));

    public static CheatValue<int> MaxAttr =>
    (IsPerCampaignInstanceLoaded && PerCampaignInstance.MaxAttr != 5)
        ? new CheatValue<int>(true, PerCampaignInstance.MaxAttr)
        : ((GlobalInstance.MaxAttr != 5)
            ? new CheatValue<int>(true, GlobalInstance.MaxAttr)
            : new CheatValue<int>(false, 5));

    public static CheatValue<int> AddMoney_count =>
    (IsPerCampaignInstanceLoaded && PerCampaignInstance.AddMoney_count != 5)
        ? new CheatValue<int>(true, PerCampaignInstance.AddMoney_count)
        : ((GlobalInstance.AddMoney_count != 5)
            ? new CheatValue<int>(true, GlobalInstance.AddMoney_count)
            : new CheatValue<int>(false, 5));

    public static CheatValue<bool> KeepDaughter =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.KeepDaughter != true)
? new CheatValue<bool>(isChanged: true, PerCampaignInstance.KeepDaughter)
: ((GlobalInstance.KeepDaughter != true)
? new CheatValue<bool>(isChanged: true, GlobalInstance.KeepDaughter)
: new CheatValue<bool>(isChanged: false, true));


    public static CheatValue<bool> PlayerAlwaysCrush =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.PlayerAlwaysCrush != true)
? new CheatValue<bool>(isChanged: true, PerCampaignInstance.PlayerAlwaysCrush)
: ((GlobalInstance.PlayerAlwaysCrush != true)
? new CheatValue<bool>(isChanged: true, GlobalInstance.PlayerAlwaysCrush)
: new CheatValue<bool>(isChanged: false, true));

    public static CheatValue<bool> EnableEverYoung =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.EnableEverYoung != true)
    ? new CheatValue<bool>(isChanged: true, PerCampaignInstance.EnableEverYoung)
    : ((GlobalInstance.EnableEverYoung != true)
        ? new CheatValue<bool>(isChanged: true, GlobalInstance.EnableEverYoung)
        : new CheatValue<bool>(isChanged: false, true));

    public static CheatValue<int> EverYoungSkillNeed =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.EverYoungSkillNeed != 400)
            ? new CheatValue<int>(isChanged: true, PerCampaignInstance.EverYoungSkillNeed)
            : ((GlobalInstance.EverYoungSkillNeed != 400)
                ? new CheatValue<int>(isChanged: true, GlobalInstance.EverYoungSkillNeed)
                : new CheatValue<int>(isChanged: false, 400));

    public static CheatValue<float> VigorDmgPercent =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorDmgPercent != 0.02f)
    ? new CheatValue<float>(true, PerCampaignInstance.VigorDmgPercent)
    : ((GlobalInstance.VigorDmgPercent != 0.02f)
        ? new CheatValue<float>(true, GlobalInstance.VigorDmgPercent)
        : new CheatValue<float>(false, 0.02f));

    public static CheatValue<float> VigorArmorAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorArmorAdd != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.VigorArmorAdd)
            : ((GlobalInstance.VigorArmorAdd != 1f)
                ? new CheatValue<float>(true, GlobalInstance.VigorArmorAdd)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> VigorMountArmorAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorMountArmorAdd != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.VigorMountArmorAdd)
            : ((GlobalInstance.VigorMountArmorAdd != 1f)
                ? new CheatValue<float>(true, GlobalInstance.VigorMountArmorAdd)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> VigorShieldEndurancePercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorShieldEndurancePercent != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.VigorShieldEndurancePercent)
            : ((GlobalInstance.VigorShieldEndurancePercent != 1f)
                ? new CheatValue<float>(true, GlobalInstance.VigorShieldEndurancePercent)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> VigorFinalDmgAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorFinalDmgAdd != 0.334f)
            ? new CheatValue<float>(true, PerCampaignInstance.VigorFinalDmgAdd)
            : ((GlobalInstance.VigorFinalDmgAdd != 0.334f)
                ? new CheatValue<float>(true, GlobalInstance.VigorFinalDmgAdd)
                : new CheatValue<float>(false, 0.334f));

    public static CheatValue<float> VigorDmgTakenReduce =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorDmgTakenReduce != 0.334f)
            ? new CheatValue<float>(true, PerCampaignInstance.VigorDmgTakenReduce)
            : ((GlobalInstance.VigorDmgTakenReduce != 0.334f)
                ? new CheatValue<float>(true, GlobalInstance.VigorDmgTakenReduce)
                : new CheatValue<float>(false, 0.334f));

    public static CheatValue<int> VigorCrushThroughPositive =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorCrushThroughPositive != 5)
            ? new CheatValue<int>(true, PerCampaignInstance.VigorCrushThroughPositive)
            : ((GlobalInstance.VigorCrushThroughPositive != 5)
                ? new CheatValue<int>(true, GlobalInstance.VigorCrushThroughPositive)
                : new CheatValue<int>(false, 5));

    public static CheatValue<int> VigorCrushThroughNegative =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.VigorCrushThroughNegative != 10)
            ? new CheatValue<int>(true, PerCampaignInstance.VigorCrushThroughNegative)
            : ((GlobalInstance.VigorCrushThroughNegative != 10)
                ? new CheatValue<int>(true, GlobalInstance.VigorCrushThroughNegative)
                : new CheatValue<int>(false, 10));

    public static CheatValue<float> IntelligenceAmmoAddPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceAmmoAddPercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceAmmoAddPercent)
            : ((GlobalInstance.IntelligenceAmmoAddPercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceAmmoAddPercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<int> ControlAmmoNoConsumeRate =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlAmmoNoConsumeRate != 5)
            ? new CheatValue<int>(true, PerCampaignInstance.ControlAmmoNoConsumeRate)
            : ((GlobalInstance.ControlAmmoNoConsumeRate != 5)
                ? new CheatValue<int>(true, GlobalInstance.ControlAmmoNoConsumeRate)
                : new CheatValue<int>(false, 5));

    public static CheatValue<float> ControlDropDmgReducePercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlDropDmgReducePercent != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.ControlDropDmgReducePercent)
            : ((GlobalInstance.ControlDropDmgReducePercent != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.ControlDropDmgReducePercent)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> ControlAimStabilityPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlAimStabilityPercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.ControlAimStabilityPercent)
            : ((GlobalInstance.ControlAimStabilityPercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.ControlAimStabilityPercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> ControlMountManeuverPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlMountManeuverPercent != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.ControlMountManeuverPercent)
            : ((GlobalInstance.ControlMountManeuverPercent != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.ControlMountManeuverPercent)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<int> ControlCritRate =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlCritRate != 2)
            ? new CheatValue<int>(true, PerCampaignInstance.ControlCritRate)
            : ((GlobalInstance.ControlCritRate != 2)
                ? new CheatValue<int>(true, GlobalInstance.ControlCritRate)
                : new CheatValue<int>(false, 2));

    public static CheatValue<int> ControlExemptionRate =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlExemptionRate != 2)
            ? new CheatValue<int>(true, PerCampaignInstance.ControlExemptionRate)
            : ((GlobalInstance.ControlExemptionRate != 2)
                ? new CheatValue<int>(true, GlobalInstance.ControlExemptionRate)
                : new CheatValue<int>(false, 2));

    public static CheatValue<int> ControlPenetrateRate =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.ControlPenetrateRate != 3)
            ? new CheatValue<int>(true, PerCampaignInstance.ControlPenetrateRate)
            : ((GlobalInstance.ControlPenetrateRate != 3)
                ? new CheatValue<int>(true, GlobalInstance.ControlPenetrateRate)
                : new CheatValue<int>(false, 3));

    public static CheatValue<float> EnduranceHpAddPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnduranceHpAddPercent != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.EnduranceHpAddPercent)
            : ((GlobalInstance.EnduranceHpAddPercent != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.EnduranceHpAddPercent)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> EnduranceHealRate =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnduranceHealRate != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.EnduranceHealRate)
            : ((GlobalInstance.EnduranceHealRate != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.EnduranceHealRate)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> EnduranceStaggerPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnduranceStaggerPercent != 0.2f)
            ? new CheatValue<float>(true, PerCampaignInstance.EnduranceStaggerPercent)
            : ((GlobalInstance.EnduranceStaggerPercent != 0.2f)
                ? new CheatValue<float>(true, GlobalInstance.EnduranceStaggerPercent)
                : new CheatValue<float>(false, 0.2f));

    public static CheatValue<float> EnduranceWalkSpeedPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnduranceWalkSpeedPercent != 0.01f)
            ? new CheatValue<float>(true, PerCampaignInstance.EnduranceWalkSpeedPercent)
            : ((GlobalInstance.EnduranceWalkSpeedPercent != 0.01f)
                ? new CheatValue<float>(true, GlobalInstance.EnduranceWalkSpeedPercent)
                : new CheatValue<float>(false, 0.01f));

    public static CheatValue<float> EnduranceMountSpeedPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.EnduranceMountSpeedPercent != 0.025f)
            ? new CheatValue<float>(true, PerCampaignInstance.EnduranceMountSpeedPercent)
            : ((GlobalInstance.EnduranceMountSpeedPercent != 0.025f)
                ? new CheatValue<float>(true, GlobalInstance.EnduranceMountSpeedPercent)
                : new CheatValue<float>(false, 0.025f));

    public static CheatValue<float> CunningPrisonerRecruitSpeedPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CunningPrisonerRecruitSpeedPercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CunningPrisonerRecruitSpeedPercent)
            : ((GlobalInstance.CunningPrisonerRecruitSpeedPercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.CunningPrisonerRecruitSpeedPercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> CunningPrisonerCapacityPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CunningPrisonerCapacityPercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CunningPrisonerCapacityPercent)
            : ((GlobalInstance.CunningPrisonerCapacityPercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.CunningPrisonerCapacityPercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> CunningRaidSpeedPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CunningRaidSpeedPercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CunningRaidSpeedPercent)
            : ((GlobalInstance.CunningRaidSpeedPercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.CunningRaidSpeedPercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> CunningPartySpeedAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CunningPartySpeedAdd != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CunningPartySpeedAdd)
            : ((GlobalInstance.CunningPartySpeedAdd != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.CunningPartySpeedAdd)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> CunningCompanionCapacityAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CunningCompanionCapacityAdd != 0.2f)
            ? new CheatValue<float>(true, PerCampaignInstance.CunningCompanionCapacityAdd)
            : ((GlobalInstance.CunningCompanionCapacityAdd != 0.2f)
                ? new CheatValue<float>(true, GlobalInstance.CunningCompanionCapacityAdd)
                : new CheatValue<float>(false, 0.2f));


    public static CheatValue<float> SocialBoundary =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialBoundary != 3.5f)
    ? new CheatValue<float>(true, PerCampaignInstance.SocialBoundary)
    : ((GlobalInstance.SocialBoundary != 3.5f)
        ? new CheatValue<float>(true, GlobalInstance.SocialBoundary)
        : new CheatValue<float>(false, 3.5f));

    public static CheatValue<float> SocialHearthAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialHearthAdd != 0.25f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialHearthAdd)
            : ((GlobalInstance.SocialHearthAdd != 0.25f)
                ? new CheatValue<float>(true, GlobalInstance.SocialHearthAdd)
                : new CheatValue<float>(false, 0.25f));

    public static CheatValue<float> SocialSettlementLoyaltyAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialSettlementLoyaltyAdd != 0.25f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialSettlementLoyaltyAdd)
            : ((GlobalInstance.SocialSettlementLoyaltyAdd != 0.25f)
                ? new CheatValue<float>(true, GlobalInstance.SocialSettlementLoyaltyAdd)
                : new CheatValue<float>(false, 0.25f));

    public static CheatValue<float> SocialMilitiaAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialMilitiaAdd != 0.5f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialMilitiaAdd)
            : ((GlobalInstance.SocialMilitiaAdd != 0.5f)
                ? new CheatValue<float>(true, GlobalInstance.SocialMilitiaAdd)
                : new CheatValue<float>(false, 0.5f));

    public static CheatValue<float> SocialRecruitSpeedPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialRecruitSpeedPercent != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialRecruitSpeedPercent)
            : ((GlobalInstance.SocialRecruitSpeedPercent != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.SocialRecruitSpeedPercent)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> SocialTaxPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialTaxPercent != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialTaxPercent)
            : ((GlobalInstance.SocialTaxPercent != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.SocialTaxPercent)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> SocialWorkshopProductionPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialWorkshopProductionPercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialWorkshopProductionPercent)
            : ((GlobalInstance.SocialWorkshopProductionPercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.SocialWorkshopProductionPercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> SocialCompanionCapacityAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.SocialCompanionCapacityAdd != 0.2f)
            ? new CheatValue<float>(true, PerCampaignInstance.SocialCompanionCapacityAdd)
            : ((GlobalInstance.SocialCompanionCapacityAdd != 0.2f)
                ? new CheatValue<float>(true, GlobalInstance.SocialCompanionCapacityAdd)
                : new CheatValue<float>(false, 0.2f));

    public static CheatValue<float> IntelligenceBoundary =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceBoundary != 3.5f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceBoundary)
            : ((GlobalInstance.IntelligenceBoundary != 3.5f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceBoundary)
                : new CheatValue<float>(false, 3.5f));

    public static CheatValue<float> IntelligenceExpRate =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceExpRate != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceExpRate)
            : ((GlobalInstance.IntelligenceExpRate != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceExpRate)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> IntelligenceSiegeEndurancePercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceSiegeEndurancePercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceSiegeEndurancePercent)
            : ((GlobalInstance.IntelligenceSiegeEndurancePercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceSiegeEndurancePercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> IntelligenceWallEndurancePercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceWallEndurancePercent != 0.1f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceWallEndurancePercent)
            : ((GlobalInstance.IntelligenceWallEndurancePercent != 0.1f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceWallEndurancePercent)
                : new CheatValue<float>(false, 0.1f));

    public static CheatValue<float> IntelligenceBallistaAdd =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceBallistaAdd != 0.334f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceBallistaAdd)
            : ((GlobalInstance.IntelligenceBallistaAdd != 0.334f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceBallistaAdd)
                : new CheatValue<float>(false, 0.334f));

    public static CheatValue<float> IntelligenceLeaderSettlementFoodPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceLeaderSettlementFoodPercent != 0.5f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceLeaderSettlementFoodPercent)
            : ((GlobalInstance.IntelligenceLeaderSettlementFoodPercent != 0.5f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceLeaderSettlementFoodPercent)
                : new CheatValue<float>(false, 0.5f));

    public static CheatValue<float> IntelligenceGovernorSettlementFoodPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceGovernorSettlementFoodPercent != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceGovernorSettlementFoodPercent)
            : ((GlobalInstance.IntelligenceGovernorSettlementFoodPercent != 1f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceGovernorSettlementFoodPercent)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> IntelligenceProsperityFoodCostReducePercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceProsperityFoodCostReducePercent != 0.075f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceProsperityFoodCostReducePercent)
            : ((GlobalInstance.IntelligenceProsperityFoodCostReducePercent != 0.075f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceProsperityFoodCostReducePercent)
                : new CheatValue<float>(false, 0.075f));

    public static CheatValue<float> IntelligenceGarrisonWageReducePercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceGarrisonWageReducePercent != 0.05f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceGarrisonWageReducePercent)
            : ((GlobalInstance.IntelligenceGarrisonWageReducePercent != 0.05f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceGarrisonWageReducePercent)
                : new CheatValue<float>(false, 0.05f));

    public static CheatValue<float> IntelligenceWorkshopProductionPercent =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.IntelligenceWorkshopProductionPercent != 0.25f)
            ? new CheatValue<float>(true, PerCampaignInstance.IntelligenceWorkshopProductionPercent)
            : ((GlobalInstance.IntelligenceWorkshopProductionPercent != 0.25f)
                ? new CheatValue<float>(true, GlobalInstance.IntelligenceWorkshopProductionPercent)
                : new CheatValue<float>(false, 0.25f));

    public static CheatValue<bool> EnableDailyGainXp =>
(IsPerCampaignInstanceLoaded && PerCampaignInstance.EnableDailyGainXp != true)
    ? new CheatValue<bool>(true, PerCampaignInstance.EnableDailyGainXp)
    : ((GlobalInstance.EnableDailyGainXp != true)
        ? new CheatValue<bool>(true, GlobalInstance.EnableDailyGainXp)
        : new CheatValue<bool>(false, true));

    public static CheatValue<float> CombatAttributeRatePlayer =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CombatAttributeRatePlayer != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CombatAttributeRatePlayer)
            : ((GlobalInstance.CombatAttributeRatePlayer != 1f)
                ? new CheatValue<float>(true, GlobalInstance.CombatAttributeRatePlayer)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> CombatAttributeRateClanMember =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CombatAttributeRateClanMember != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CombatAttributeRateClanMember)
            : ((GlobalInstance.CombatAttributeRateClanMember != 1f)
                ? new CheatValue<float>(true, GlobalInstance.CombatAttributeRateClanMember)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> CombatAttributeRateOther =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.CombatAttributeRateOther != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.CombatAttributeRateOther)
            : ((GlobalInstance.CombatAttributeRateOther != 1f)
                ? new CheatValue<float>(true, GlobalInstance.CombatAttributeRateOther)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> StrategyAttributeRatePlayer =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.StrategyAttributeRatePlayer != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.StrategyAttributeRatePlayer)
            : ((GlobalInstance.StrategyAttributeRatePlayer != 1f)
                ? new CheatValue<float>(true, GlobalInstance.StrategyAttributeRatePlayer)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> StrategyAttributeRateClanMember =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.StrategyAttributeRateClanMember != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.StrategyAttributeRateClanMember)
            : ((GlobalInstance.StrategyAttributeRateClanMember != 1f)
                ? new CheatValue<float>(true, GlobalInstance.StrategyAttributeRateClanMember)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<float> StrategyAttributeRateOther =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.StrategyAttributeRateOther != 1f)
            ? new CheatValue<float>(true, PerCampaignInstance.StrategyAttributeRateOther)
            : ((GlobalInstance.StrategyAttributeRateOther != 1f)
                ? new CheatValue<float>(true, GlobalInstance.StrategyAttributeRateOther)
                : new CheatValue<float>(false, 1f));

    public static CheatValue<bool> TestMode =>
        (IsPerCampaignInstanceLoaded && PerCampaignInstance.TestMode != false)
            ? new CheatValue<bool>(true, PerCampaignInstance.TestMode)
            : ((GlobalInstance.TestMode != false)
                ? new CheatValue<bool>(true, GlobalInstance.TestMode)
                : new CheatValue<bool>(false, false));

}