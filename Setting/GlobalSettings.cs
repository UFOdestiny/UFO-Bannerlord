using JetBrains.Annotations;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;
using System.Reflection;
using System.Text.RegularExpressions;
using UFO.Localization;

namespace UFO.Setting
{
    [UsedImplicitly]
    public class BannerlordCheatsGlobalSettings : AttributeGlobalSettings<BannerlordCheatsGlobalSettings>
    {
        public override string Id { get; } = $"UFO_v{Assembly.GetExecutingAssembly().GetName().Version.Major}_Global";

        public override string DisplayName { get; }

        public override string FolderName => "UFO_G";

        public override string FormatType => "json2";

        public BannerlordCheatsGlobalSettings()
        {
            string text;
            try
            {
                text = L10N.GetText("ModName");
            }
            catch
            {
                text = "Cheats";
            }
            string text2;
            try
            {
                text2 = L10N.GetText("Global");
            }
            catch
            {
                text2 = "Global";
            }
            string input = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            input = Regex.Replace(input, "(?:\\.0)+$", string.Empty);
            if (!input.Contains("."))
            {
                input += ".0";
            }
            DisplayName = text + " " + input + " (" + text2 + ")";
        }


        // UFO's
        [LocalizedSettingPropertyGroup("UFOs", GroupOrder = 0)]
        [LocalizedSettingPropertyBool("PlayerAlwaysCrush")]
        public bool PlayerAlwaysCrush { get; set; } = true;


        // Cheat

        [LocalizedSettingPropertyGroup("General", GroupOrder = 1)]
        [LocalizedSettingPropertyBool("EnableHotkeys")]
        public bool EnableHotkeys { get; set; } = false;

        [LocalizedSettingPropertyGroup("General")]
        [LocalizedSettingPropertyBool("EnableHotkeyTips")]
        public bool EnableHotkeyTips { get; set; } = false;

        [LocalizedSettingPropertyGroup("Map", GroupOrder = 2)]
        [LocalizedSettingPropertyFloatingInteger("MapSpeedMultiplier", 1f, 100f)]
        public float MapSpeedMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Map")]
        [LocalizedSettingPropertyFloatingInteger("MapVisibilityMultiplier", 1f, 100f)]
        public float MapVisibilityMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Map")]
        [LocalizedSettingPropertyPercent("NpcMapSpeedPercentage")]
        public float NpcMapSpeedPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Map")]
        [LocalizedSettingPropertyBool("PartyInvisibleOnMap")]
        public bool PartyInvisibleOnMap { get; set; } = false;

        [LocalizedSettingPropertyGroup("Map")]
        [LocalizedSettingPropertyBool("CaravansInvisibleOnMap")]
        public bool CaravansInvisibleOnMap { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player", GroupOrder = 3)]
        [LocalizedSettingPropertyPercent("DamageTakenPercentage")]
        public float DamageTakenPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("Invincible")]
        public bool Invincible { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("PlayerHorseInvincible")]
        public bool PlayerHorseInvincible { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("OneHitKill")]
        public bool OneHitKill { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("AlwaysCrushThroughShields")]
        public bool AlwaysCrushThroughShields { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("SliceThroughEveryone")]
        public bool SliceThroughEveryone { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyPercent("HealthRegeneration")]
        public float HealthRegeneration { get; set; } = 0f;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("InfiniteAmmo")]
        public bool InfiniteAmmo { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyFloatingInteger("DamageMultiplier", 1f, 10f)]
        public float DamageMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("AlwaysKnockDown")]
        public bool AlwaysKnockDown { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("NeverKnockedBackByAttacks")]
        public bool NeverKnockedBackByAttacks { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("NoStuckArrows")]
        public bool NoStuckArrows { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Player")]
        [LocalizedSettingPropertyBool("InstantCrossbowReload")]
        public bool InstantCrossbowReload { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Party", GroupOrder = 4)]
        [LocalizedSettingPropertyDropdown("PartyKnockoutOrKilled", KnockoutOrKilled.Default)]
        public Dropdown<LocalizedDropdownValue<KnockoutOrKilled>> PartyKnockoutOrKilled { get; set; } = LocalizedDropdownValue<KnockoutOrKilled>.GenerateDropdown(KnockoutOrKilled.Default);

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyDropdown("CompanionsKnockoutOrKilled", KnockoutOrKilled.Default)]
        public Dropdown<LocalizedDropdownValue<KnockoutOrKilled>> CompanionsKnockoutOrKilled { get; set; } = LocalizedDropdownValue<KnockoutOrKilled>.GenerateDropdown(KnockoutOrKilled.Default);

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyBool("PartyInvincible")]
        public bool PartyInvincible { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyBool("PartyHeroesInvincible")]
        public bool PartyHeroesInvincible { get; set; } = false;

        public float PartyDamageTakenPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyBool("PartyOneHitKill")]
        public bool PartyOneHitKill { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyBool("NoRunningAway")]
        public bool NoRunningAway { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyPercent("PartyHealthRegeneration")]
        public float PartyHealthRegeneration { get; set; } = 0f;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyBool("PartyInfiniteAmmo")]
        public bool PartyInfiniteAmmo { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyFloatingInteger("PartyDamageMultiplier", 1f, 10f)]
        public float PartyDamageMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Combat_Party")]
        [LocalizedSettingPropertyBool("NoFriendlyFire")]
        public bool NoFriendlyFire { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Allies", GroupOrder = 5)]
        [LocalizedSettingPropertyDropdown("FriendlyLordsKnockoutOrKilled", KnockoutOrKilled.Default)]
        public Dropdown<LocalizedDropdownValue<KnockoutOrKilled>> FriendlyLordsKnockoutOrKilled { get; set; } = LocalizedDropdownValue<KnockoutOrKilled>.GenerateDropdown(KnockoutOrKilled.Default);

        [LocalizedSettingPropertyGroup("Combat_Enemies", GroupOrder = 6)]
        [LocalizedSettingPropertyDropdown("EnemyLordsKnockoutOrKilled", KnockoutOrKilled.Default)]
        public Dropdown<LocalizedDropdownValue<KnockoutOrKilled>> EnemyLordsKnockoutOrKilled { get; set; } = LocalizedDropdownValue<KnockoutOrKilled>.GenerateDropdown(KnockoutOrKilled.Default);

        [LocalizedSettingPropertyGroup("Combat_Enemies")]
        [LocalizedSettingPropertyDropdown("EnemyTroopsKnockoutOrKilled", KnockoutOrKilled.Default)]
        public Dropdown<LocalizedDropdownValue<KnockoutOrKilled>> EnemyTroopsKnockoutOrKilled { get; set; } = LocalizedDropdownValue<KnockoutOrKilled>.GenerateDropdown(KnockoutOrKilled.Default);

        [LocalizedSettingPropertyGroup("Combat_Enemies")]
        [LocalizedSettingPropertyBool("EnemiesNoRunningAway")]
        public bool EnemiesNoRunningAway { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Enemies")]
        [LocalizedSettingPropertyPercent("EnemyDamagePercentage")]
        public float EnemyDamagePercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Combat_Misc", GroupOrder = 7)]
        [LocalizedSettingPropertyFloatingInteger("RenownRewardMultiplier", 1f, 1000f)]
        public float RenownRewardMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Combat_Misc")]
        [LocalizedSettingPropertyFloatingInteger("InfluenceRewardMultiplier", 1f, 1000f)]
        public float InfluenceRewardMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Combat_Misc")]
        [LocalizedSettingPropertyBool("AlwaysWinBattleSimulation")]
        public bool AlwaysWinBattleSimulation { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Misc")]
        [LocalizedSettingPropertyBool("NoTroopSacrifice")]
        public bool NoTroopSacrifice { get; set; } = false;

        [LocalizedSettingPropertyGroup("Combat_Misc")]
        [LocalizedSettingPropertyInteger("BanditHideoutTroopLimit", 0, 1000)]
        public int BanditHideoutTroopLimit { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Combat_Misc")]
        [LocalizedSettingPropertyFloatingInteger("CombatZoomMultiplier", 1f, 1000f)]
        public float CombatZoomMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Inventory", GroupOrder = 8)]
        [LocalizedSettingPropertyInteger("ExtraInventoryCapacity", 0, 1000000)]
        public int ExtraInventoryCapacity { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Inventory")]
        [LocalizedSettingPropertyBool("NativeItemSpawning")]
        public bool NativeItemSpawning { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party", GroupOrder = 9)]
        [LocalizedSettingPropertyInteger("ExtraPartyMemberSize", 0, 10000)]
        public int ExtraPartyMemberSize { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyInteger("ExtraPartyPrisonerSize", 0, 10000)]
        public int ExtraPartyPrisonerSize { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyInteger("ExtraPartyMorale", 0, 100)]
        public int ExtraPartyMorale { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyBool("InstantEscape")]
        public bool InstantEscape { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyPercent("FoodConsumptionPercentage")]
        public float FoodConsumptionPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyPercent("TroopWagesPercentage")]
        public float TroopWagesPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyBool("FreeTroopUpgrades")]
        public bool FreeTroopUpgrades { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyBool("FreeCompanionHiring")]
        public bool FreeCompanionHiring { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyBool("InstantPrisonerRecruitment")]
        public bool InstantPrisonerRecruitment { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyBool("NoPrisonerEscape")]
        public bool NoPrisonerEscape { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyFloatingInteger("PartyHealingMultiplier", 1f, 100f)]
        public float PartyHealingMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Clan", GroupOrder = 10)]
        [LocalizedSettingPropertyInteger("ExtraCompanionLimit", 0, 100)]
        public int ExtraCompanionLimit { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Clan")]
        [LocalizedSettingPropertyInteger("ExtraClanPartyLimit", 0, 100)]
        public int ExtraClanPartyLimit { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Clan")]
        [LocalizedSettingPropertyInteger("ExtraClanPartySize", 0, 10000)]
        public int ExtraClanPartySize { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Characters", GroupOrder = 11)]
        [LocalizedSettingPropertyFloatingInteger("RelationGainAfterBattleMultiplier", 1f, 100f)]
        public float RelationGainAfterBattleMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("PerfectRelationships")]
        public bool PerfectRelationships { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("NeverDieOfOldAge")]
        public bool NeverDieOfOldAge { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("BarterOfferAlwaysAccepted")]
        public bool BarterOfferAlwaysAccepted { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("NoBarterCooldown")]
        public bool NoBarterCooldown { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("ConversationAlwaysSuccessful")]
        public bool ConversationAlwaysSuccessful { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("PerfectAttraction")]
        public bool PerfectAttraction { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyBool("AllowSameSexMarriage")]
        public bool AllowSameSexMarriage { get; set; } = false;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyFloatingInteger("PregnancyChanceMultiplier", 1f, 100f)]
        public float PregnancyChanceMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Characters")]
        [LocalizedSettingPropertyInteger("AdjustPregnancyDuration", 1, 36)]
        public int AdjustPregnancyDuration { get; set; } = 36;

        [LocalizedSettingPropertyGroup("Kingdom", GroupOrder = 12)]
        [LocalizedSettingPropertyFloatingInteger("KingdomDecisionWeightMultiplier", 1f, 1000f)]
        public float KingdomDecisionWeightMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Kingdom")]
        [LocalizedSettingPropertyBool("NoRelationshipLossOnDecision")]
        public bool NoRelationshipLossOnDecision { get; set; } = false;

        [LocalizedSettingPropertyGroup("Kingdom")]
        [LocalizedSettingPropertyBool("NoCrimeRatingForCrimes")]
        public bool NoCrimeRatingForCrimes { get; set; } = false;

        [LocalizedSettingPropertyGroup("Party")]
        [LocalizedSettingPropertyPercent("DecisionOverrideInfluenceCostPercentage")]
        public float DecisionOverrideInfluenceCostPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Experience", GroupOrder = 13)]
        [LocalizedSettingPropertyFloatingInteger("ExperienceMultiplier", 1f, 100f)]
        public float ExperienceMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyFloatingInteger("CompanionExperienceMultiplier", 1f, 100f)]
        public float CompanionExperienceMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyFloatingInteger("ClanExperienceMultiplier", 1f, 100f)]
        public float ClanExperienceMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyFloatingInteger("LearningRateMultiplier", 1f, 1000f)]
        public float LearningRateMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyFloatingInteger("CompanionLearningRateMultiplier", 1f, 1000f)]
        public float CompanionLearningRateMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyFloatingInteger("LearningLimitMultiplier", 1f, 1000f)]
        public float LearningLimitMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyFloatingInteger("TroopExperienceMultiplier", 1f, 1000f)]
        public float TroopExperienceMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Experience")]
        [LocalizedSettingPropertyBool("FreeFocusPointAssignment")]
        public bool FreeFocusPointAssignment { get; set; } = false;

        [LocalizedSettingPropertyGroup("Sieges", GroupOrder = 14)]
        [LocalizedSettingPropertyFloatingInteger("SiegeBuildingSpeedMultiplier", 1f, 1000f)]
        public float SiegeBuildingSpeedMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Sieges")]
        [LocalizedSettingPropertyPercent("EnemySiegeBuildingSpeedPercentage")]
        public float EnemySiegeBuildingSpeedPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Army", GroupOrder = 15)]
        [LocalizedSettingPropertyPercent("FactionArmyCohesionLossPercentage")]
        public float FactionArmyCohesionLossPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Army")]
        [LocalizedSettingPropertyPercent("ArmyCohesionLossPercentage")]
        public float ArmyCohesionLossPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Army")]
        [LocalizedSettingPropertyPercent("ArmyFoodConsumptionPercentage")]
        public float ArmyFoodConsumptionPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Settlements", GroupOrder = 16)]
        [LocalizedSettingPropertyBool("VillagesNeverRaided")]
        public bool VillagesNeverRaided { get; set; } = false;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyBool("DisguiseAlwaysWorks")]
        public bool DisguiseAlwaysWorks { get; set; } = false;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyBool("FreeTroopRecruitment")]
        public bool FreeTroopRecruitment { get; set; } = false;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyPercent("ItemTradingCostPercentage")]
        public float ItemTradingCostPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyFloatingInteger("SellingPriceMultiplier", 1f, 1000f)]
        public float SellingPriceMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyFloatingInteger("TournamentMaximumBetMultiplier", 1f, 1000f)]
        public float TournamentMaximumBetMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailyFoodBonus", 0, 10000)]
        public int DailyFoodBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailyGarrisonBonus", 0, 10000)]
        public int DailyGarrisonBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailyMilitiaBonus", 0, 10000)]
        public int DailyMilitiaBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailyProsperityBonus", 0, 10000)]
        public int DailyProsperityBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailyLoyaltyBonus", 0, 10000)]
        public int DailyLoyaltyBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailySecurityBonus", 0, 10000)]
        public int DailySecurityBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyInteger("DailyHearthsBonus", 0, 10000)]
        public int DailyHearthsBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyPercent("GarrisonWagesPercentage")]
        public float GarrisonWagesPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyBool("NeverRequireCivilianEquipment")]
        public bool NeverRequireCivilianEquipment { get; set; } = false;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyFloatingInteger("ConstructionPowerMultiplier", 1f, 1000f)]
        public float ConstructionPowerMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyBool("NoBribeToEnterKeep")]
        public bool NoBribeToEnterKeep { get; set; } = false;

        [LocalizedSettingPropertyGroup("Settlements")]
        [LocalizedSettingPropertyBool("SettlementsNeverRebel")]
        public bool SettlementsNeverRebel { get; set; } = false;

        [LocalizedSettingPropertyGroup("Smithing", GroupOrder = 17)]
        [LocalizedSettingPropertyPercent("SmithingEnergyCostPercentage")]
        public float SmithingEnergyCostPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyBool("UnlockAllParts")]
        public bool UnlockAllParts { get; set; } = false;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyPercent("SmithingDifficultyPercentage")]
        public float SmithingDifficultyPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyPercent("SmithingCostPercentage")]
        public float SmithingCostPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyInteger("CraftedWeaponHandlingBonus", 0, 100)]
        public int CraftedWeaponHandlingBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyInteger("CraftedWeaponSwingDamageBonus", 0, 100)]
        public int CraftedWeaponSwingDamageBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyInteger("CraftedWeaponSwingSpeedBonus", 0, 100)]
        public int CraftedWeaponSwingSpeedBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyInteger("CraftedWeaponThrustDamageBonus", 0, 100)]
        public int CraftedWeaponThrustDamageBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Smithing")]
        [LocalizedSettingPropertyInteger("CraftedWeaponThrustSpeedBonus", 0, 100)]
        public int CraftedWeaponThrustSpeedBonus { get; set; } = 0;

        [LocalizedSettingPropertyGroup("Workshops", GroupOrder = 18)]
        [LocalizedSettingPropertyPercent("WorkshopBuyingCostPercentage")]
        public float WorkshopBuyingCostPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Workshops")]
        [LocalizedSettingPropertyPercent("WorkshopDailyExpensePercentage")]
        public float WorkshopDailyExpensePercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Workshops")]
        [LocalizedSettingPropertyPercent("WorkshopUpgradeCostPercentage")]
        public float WorkshopUpgradeCostPercentage { get; set; } = 100f;

        [LocalizedSettingPropertyGroup("Workshops")]
        [LocalizedSettingPropertyFloatingInteger("WorkshopSellingCostMultiplier", 1f, 100f)]
        public float WorkshopSellingCostMultiplier { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Workshops")]
        [LocalizedSettingPropertyBool("EveryoneBuysWorkshops")]
        public bool EveryoneBuysWorkshops { get; set; } = false;


        // Hero Enhance

        [LocalizedSettingPropertyGroup("General_Enhancement", GroupOrder = 19)]
        [LocalizedSettingPropertyDropdown("AutoChoosePerk", AutoChoosePerk_Type.Clan)]
        public Dropdown<LocalizedDropdownValue<AutoChoosePerk_Type>> AutoChoosePerk { get; set; } = LocalizedDropdownValue<AutoChoosePerk_Type>.GenerateDropdown(AutoChoosePerk_Type.Clan);


        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyBool("EnableEverYoung")]
        public bool EnableEverYoung { get; set; } = true;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyBool("EnableDailyGainXp")]
        public bool EnableDailyGainXp { get; set; } = true;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyBool("TestMode")]
        public bool TestMode { get; set; } = false;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyInteger("EverYoungSkillNeed", 0, 1000)]
        public int EverYoungSkillNeed { get; set; } = 400;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement", GroupOrder = 20)]
        [LocalizedSettingPropertyInteger("VigorCrushThroughPositive", 0, 100)]
        public int VigorCrushThroughPositive { get; set; } = 5;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyInteger("VigorCrushThroughNegative", 0, 100)]
        public int VigorCrushThroughNegative { get; set; } = 10;

        [LocalizedSettingPropertyGroup("Control_Enhancement", GroupOrder = 21)]
        [LocalizedSettingPropertyInteger("ControlAmmoNoConsumeRate", 0, 10)]
        public int ControlAmmoNoConsumeRate { get; set; } = 5;

        [LocalizedSettingPropertyGroup("Control_Enhancement")]
        [LocalizedSettingPropertyInteger("ControlCritRate", 0, 100)]
        public int ControlCritRate { get; set; } = 2;

        [LocalizedSettingPropertyGroup("Control_Enhancement")]
        [LocalizedSettingPropertyInteger("ControlExemptionRate", 0, 100)]
        public int ControlExemptionRate { get; set; } = 2;

        [LocalizedSettingPropertyGroup("Control_Enhancement")]
        [LocalizedSettingPropertyInteger("ControlPenetrateRate", 0, 10)]
        public int ControlPenetrateRate { get; set; } = 3;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("VigorDmgPercent", 0f, 1f)]
        public float VigorDmgPercent { get; set; } = 0.02f;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("VigorArmorAdd", 0f, 100f)]
        public float VigorArmorAdd { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("VigorMountArmorAdd", 0f, 100f)]
        public float VigorMountArmorAdd { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("VigorShieldEndurancePercent", 0f, 100f)]
        public float VigorShieldEndurancePercent { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("VigorFinalDmgAdd", 0f, 100f)]
        public float VigorFinalDmgAdd { get; set; } = 0.334f;

        [LocalizedSettingPropertyGroup("Vigor_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("VigorDmgTakenReduce", 0f, 100f)]
        public float VigorDmgTakenReduce { get; set; } = 0.334f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement", GroupOrder = 22)]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceAmmoAddPercent", 0f, 10f)]
        public float IntelligenceAmmoAddPercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Control_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("ControlDropDmgReducePercent", 0f, 1f)]
        public float ControlDropDmgReducePercent { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Control_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("ControlAimStabilityPercent", 0f, 100f)]
        public float ControlAimStabilityPercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Control_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("ControlMountManeuverPercent", 0f, 100f)]
        public float ControlMountManeuverPercent { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Endurance_Enhancement", GroupOrder = 23)]
        [LocalizedSettingPropertyFloatingInteger("EnduranceHpAddPercent", 0f, 100f)]
        public float EnduranceHpAddPercent { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Endurance_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("EnduranceHealRate", 0f, 100f)]
        public float EnduranceHealRate { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Endurance_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("EnduranceStaggerPercent", 0f, 100f)]
        public float EnduranceStaggerPercent { get; set; } = 0.2f;

        [LocalizedSettingPropertyGroup("Endurance_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("EnduranceWalkSpeedPercent", 0f, 100f)]
        public float EnduranceWalkSpeedPercent { get; set; } = 0.01f;

        [LocalizedSettingPropertyGroup("Endurance_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("EnduranceMountSpeedPercent", 0f, 100f)]
        public float EnduranceMountSpeedPercent { get; set; } = 0.025f;

        [LocalizedSettingPropertyGroup("Cunning_Enhancement", GroupOrder = 24)]
        [LocalizedSettingPropertyFloatingInteger("CunningPrisonerRecruitSpeedPercent", 0f, 100f)]
        public float CunningPrisonerRecruitSpeedPercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Cunning_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CunningPrisonerCapacityPercent", 0f, 100f)]
        public float CunningPrisonerCapacityPercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Cunning_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CunningRaidSpeedPercent", 0f, 100f)]
        public float CunningRaidSpeedPercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Cunning_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CunningPartySpeedAdd", 0f, 100f)]
        public float CunningPartySpeedAdd { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Cunning_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CunningCompanionCapacityAdd", 0f, 100f)]
        public float CunningCompanionCapacityAdd { get; set; } = 0.2f;

        [LocalizedSettingPropertyGroup("Social_Enhancement", GroupOrder = 25)]
        [LocalizedSettingPropertyFloatingInteger("SocialBoundary", 0f, 100f)]
        public float SocialBoundary { get; set; } = 3.5f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialHearthAdd", 0f, 100f)]
        public float SocialHearthAdd { get; set; } = 0.25f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialSettlementLoyaltyAdd", 0f, 100f)]
        public float SocialSettlementLoyaltyAdd { get; set; } = 0.25f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialMilitiaAdd", 0f, 100f)]
        public float SocialMilitiaAdd { get; set; } = 0.5f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialRecruitSpeedPercent", 0f, 100f)]
        public float SocialRecruitSpeedPercent { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialTaxPercent", 0f, 100f)]
        public float SocialTaxPercent { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialWorkshopProductionPercent", 0f, 100f)]
        public float SocialWorkshopProductionPercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Social_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("SocialCompanionCapacityAdd", 0f, 100f)]
        public float SocialCompanionCapacityAdd { get; set; } = 0.2f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceBoundary", 0f, 100f)]
        public float IntelligenceBoundary { get; set; } = 3.5f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceExpRate", 0f, 100f)]
        public float IntelligenceExpRate { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceSiegeEndurancePercent", 0f, 100f)]
        public float IntelligenceSiegeEndurancePercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceWallEndurancePercent", 0f, 100f)]
        public float IntelligenceWallEndurancePercent { get; set; } = 0.1f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceBallistaAdd", 0f, 100f)]
        public float IntelligenceBallistaAdd { get; set; } = 0.334f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceLeaderSettlementFoodPercent", 0f, 100f)]
        public float IntelligenceLeaderSettlementFoodPercent { get; set; } = 0.5f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceGovernorSettlementFoodPercent", 0f, 100f)]
        public float IntelligenceGovernorSettlementFoodPercent { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceProsperityFoodCostReducePercent", 0f, 100f)]
        public float IntelligenceProsperityFoodCostReducePercent { get; set; } = 0.075f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceGarrisonWageReducePercent", 0f, 100f)]
        public float IntelligenceGarrisonWageReducePercent { get; set; } = 0.05f;

        [LocalizedSettingPropertyGroup("Intelligence_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("IntelligenceWorkshopProductionPercent", 0f, 100f)]
        public float IntelligenceWorkshopProductionPercent { get; set; } = 0.25f;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CombatAttributeRatePlayer", 0f, 10f)]
        public float CombatAttributeRatePlayer { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CombatAttributeRateClanMember", 0f, 10f)]
        public float CombatAttributeRateClanMember { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("CombatAttributeRateOther", 0f, 10f)]
        public float CombatAttributeRateOther { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("StrategyAttributeRatePlayer", 0f, 10f)]
        public float StrategyAttributeRatePlayer { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("StrategyAttributeRateClanMember", 0f, 10f)]
        public float StrategyAttributeRateClanMember { get; set; } = 1f;

        [LocalizedSettingPropertyGroup("General_Enhancement")]
        [LocalizedSettingPropertyFloatingInteger("StrategyAttributeRateOther", 0f, 10f)]
        public float StrategyAttributeRateOther { get; set; } = 1f;



    }
}