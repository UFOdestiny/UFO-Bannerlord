using UFO;
using System.Collections.Generic;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;

internal class Settings : AttributeGlobalSettings<Settings>, ISettings
{
    public override string Id => "UFO";

    public override string DisplayName => "UFO's Mod";

    public override string FormatType { get; } = "json2";

    [SettingPropertyInteger("{=he_pey}Enable Ever Young", 0, 1000, "0", HintText = "{=he_pey_hint}Player will be ever young ignore skill need setting. Close This will disable all ever young functions. If you met mod conflict you can disable it", Order = 0, RequireRestart = false)]
    public bool EnableEverYoung { get; set; } = true;

    [SettingPropertyInteger("{=he_eysn}Ever Young Skill Need", 0, 1000, "0", HintText = "{=he_eysn_hint}Clan members will be ever young after Athletics + Charm >= this value.", Order = 1, RequireRestart = false)]
    public int EverYoungSkillNeed { get; set; } = 400;

    [SettingPropertyDropdown("{=he_acap}Auto choose all perks", HintText = "{=he_acap_hint}Auto choose all perks, need skill level supports.", Order = 2, RequireRestart = false)]
    public Dropdown<string> AutoChoosePerk { get; set; } = new Dropdown<string>(new string[4] { "{=he_choice_a}All", "{=he_choice_co}Clan members Only", "{=he_choice_po}Player Only", "{=he_choice_no}No one" }, 0);

    [SettingPropertyBool("{=he_geed}Gain exp every day", HintText = "{=he_geed_hint}Hero will gain role exp(lv x int x int x 2) every day.", Order = 7, RequireRestart = false)]
    public bool EnableDailyGainXp { get; set; } = true;

    [SettingPropertyFloatingInteger("{=he_carp}Combat Attribute Rate - Player", 0f, 10f, "0.00", HintText = "{=he_carp_hint}All combat effects will multiple it", Order = 8, RequireRestart = false)]
    public float CombatAttributeRatePlayer { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=he_carcm}Combat Attribute Rate - Clan Member", 0f, 10f, "0.00", HintText = "{=he_carcm_hint}all combat effects will multiple it", Order = 9, RequireRestart = false)]
    public float CombatAttributeRateClanMember { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=he_caro}Combat Attribute Rate - Other", 0f, 10f, "0.00", HintText = "{=he_caro_hint}All combat effects will multiple it", Order = 10, RequireRestart = false)]
    public float CombatAttributeRateOther { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=he_sarp}Strategy Attribute Rate - Player", 0f, 10f, "0.00", HintText = "{=he_sarp_hint}Al strategy effects will multiple it", Order = 11, RequireRestart = false)]
    public float StrategyAttributeRatePlayer { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=he_sarcm}Strategy Attribute Rate - Clan Member", 0f, 10f, "0.00", HintText = "{=he_sarcm_hint}All strategy effects will multiple it", Order = 12, RequireRestart = false)]
    public float StrategyAttributeRateClanMember { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=he_saro}Strategy Attribute Rate - Other", 0f, 10f, "0.00", HintText = "{=he_saro_hint}All strategy effects will multiple it", Order = 13, RequireRestart = false)]
    public float StrategyAttributeRateOther { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=he_vdp}Vigor Damage Percent", 0f, 1f, "0.00", HintText = "{=he_vdp_hint}each vigor increases x% damage", Order = 20, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement", GroupOrder = 1)]
    public float VigorDmgPercent { get; set; } = 0.02f;

    [SettingPropertyFloatingInteger("{=he_vaa}Vigor Armor Add", 0f, 100f, "0.00", HintText = "{=he_vaa_hint}each vigor increases x armor", Order = 21, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public float VigorArmorAdd { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=vmaa}Vigor Mount Armor Add", 0f, 100f, "0.00", HintText = "{=he_vmaa_hint}each vigor increases x mount armor", Order = 22, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public float VigorMountArmorAdd { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=vsep}Vigor Shield Endurance Percent", 0f, 100f, "0.00", HintText = "{=he_vsep_hint}each vigor increases x shield endurance", Order = 23, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public float VigorShieldEndurancePercent { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=vfda}Vigor Final Damage Add", 0f, 100f, "0.00", HintText = "{=he_vfda_hint}each vigor increases x final damage", Order = 24, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public float VigorFinalDmgAdd { get; set; } = 0.334f;

    [SettingPropertyFloatingInteger("{=vdtr}Vigor Damage Taken Reduce", 0f, 100f, "0.00", HintText = "{=he_vdtr_hint}each vigor reduce x damage taken", Order = 25, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public float VigorDmgTakenReduce { get; set; } = 0.334f;

    [SettingPropertyInteger("{=vctp}Vigor Crush Through Positive", 0, 100, "0", HintText = "{=he_vctp_hint}each vigor cal positive gain x% percent to crush through enemy block", Order = 26, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public int VigorCrushThroughPositive { get; set; } = 5;

    [SettingPropertyInteger("{=vctn}Vigor Crush Through Negative", 0, 100, "0", HintText = "{=he_vctn_hint}each vigor cal negative gain your block x% to prevent crush through from enemy", Order = 27, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ve}Vigor Enhancement")]
    public int VigorCrushThroughNegative { get; set; } = 10;

    [SettingPropertyInteger("{=cancr}Control Ammo No Consume Rate", 0, 10, "0", HintText = "{=he_cancr_hint}each control increases x% chance to avoid consume ammo", Order = 30, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement", GroupOrder = 2)]
    public int ControlAmmoNoConsumeRate { get; set; } = 5;

    [SettingPropertyFloatingInteger("{=cddrp}Control Drop Dmg Reduce Percent", 0f, 1f, "0.00", HintText = "{=he_cddrp_hint}each control reduce x% fallen damage", Order = 31, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement")]
    public float ControlDropDmgReducePercent { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=casp}Control Aim Stability Percent", 0f, 100f, "0.00", HintText = "{=he_casp_hint}each control increases x% aim steady", Order = 32, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement")]
    public float ControlAimStabilityPercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=cmmp}Control Mount Maneuver Percent", 0f, 100f, "0.00", HintText = "{=he_cmmp_hint}each control increases x% mount maneuver", Order = 33, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement")]
    public float ControlMountManeuverPercent { get; set; } = 0.05f;

    [SettingPropertyInteger("{=ccr}Control Crit Rate", 0, 100, "0", HintText = "{=he_ccr_hint}each control increases x% crit strike rate", Order = 34, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement")]
    public int ControlCritRate { get; set; } = 2;

    [SettingPropertyInteger("{=cer}Control Exemption Rate", 0, 100, "0", HintText = "{=he_cer_hint}each control increases x% exemption rate", Order = 35, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement")]
    public int ControlExemptionRate { get; set; } = 2;

    [SettingPropertyInteger("{=cpr}Control Penetrate Rate", 0, 10, "0", HintText = "{=he_cpr_hint}each control gain you x% to penetrate enemy's shield", Order = 36, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ce}Control Enhancement")]
    public int ControlPenetrateRate { get; set; } = 3;

    [SettingPropertyFloatingInteger("{=ehap}Endurance Hp Add Percent", 0f, 100f, "0.00", HintText = "{=he_ehap_hint}each endurance increases x% maxium hp", Order = 40, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ee}Endurance Enhancement", GroupOrder = 3)]
    public float EnduranceHpAddPercent { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=ehr}Endurance Heal Rate", 0f, 100f, "0.00", HintText = "{=he_ehr_hint}each endurance increases x% hp recover speed", Order = 41, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ee}Endurance Enhancement")]
    public float EnduranceHealRate { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=esp}Endurance Stagger Percent", 0f, 100f, "0.00", HintText = "{=he_esp_hint}each endurance increases x% stagger resist", Order = 42, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ee}Endurance Enhancement")]
    public float EnduranceStaggerPercent { get; set; } = 0.2f;

    [SettingPropertyFloatingInteger("{=ewsp}Endurance Walk Speed Percent", 0f, 100f, "0.00", HintText = "{=he_ewsp_hint}each endurance increases x% walk speed", Order = 43, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ee}Endurance Enhancement")]
    public float EnduranceWalkSpeedPercent { get; set; } = 0.01f;

    [SettingPropertyFloatingInteger("{=emsp}EnduranceMountSpeedPercent", 0f, 100f, "0.00", HintText = "{=he_emsp_hint}each endurance increases  x% mount speed", Order = 44, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ee}Endurance Enhancement")]
    public float EnduranceMountSpeedPercent { get; set; } = 0.025f;

    [SettingPropertyFloatingInteger("{=cprsp}Cunning Prisoner Recruit Speed Percent", 0f, 100f, "0.00", HintText = "{=he_cprsp_hint}each cunning increases x% prisoner recruit speed", Order = 50, RequireRestart = false)]
    [SettingPropertyGroup("{=he_cue}Cunning Enhancement", GroupOrder = 4)]
    public float CunningPrisonerRecruitSpeedPercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=cpcp}Cunning Prisoner Capacity Percent", 0f, 100f, "0.00", HintText = "{=he__hint}each cunning increases x% prisoner capacity", Order = 51, RequireRestart = false)]
    [SettingPropertyGroup("{=he_cue}Cunning Enhancement")]
    public float CunningPrisonerCapacityPercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=crsp}Cunning Raid Speed Percent", 0f, 100f, "0.00", HintText = "{=he_crsp_hint}each cunning increases x% raiding speed(party leader)", Order = 52, RequireRestart = false)]
    [SettingPropertyGroup("{=he_cue}Cunning Enhancement")]
    public float CunningRaidSpeedPercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=cpsa}Cunning Party Speed Add", 0f, 100f, "0.00", HintText = "{=he_cpsa_hint}each cunning increases x party speed(scout)", Order = 53, RequireRestart = false)]
    [SettingPropertyGroup("{=he_cue}Cunning Enhancement")]
    public float CunningPartySpeedAdd { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=ccca}Cunning Companion Capacity Add", 0f, 100f, "0.00", HintText = "{=he_ccca_hint}each cunning increases x companion capacity(clan leader)", Order = 54, RequireRestart = false)]
    [SettingPropertyGroup("{=he_cue}Cunning Enhancement")]
    public float CunningCompanionCapacityAdd { get; set; } = 0.2f;

    [SettingPropertyFloatingInteger("{=he_sb}Social boundary", 0f, 100f, "0.00", HintText = "{=he_sb_hint}Set the calculation boundary of social", Order = 60, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement", GroupOrder = 5)]
    public float SocialBoundary { get; set; } = 3.5f;

    [SettingPropertyFloatingInteger("{=sha}Social Hearth Add", 0f, 100f, "0.00", HintText = "{=he_sha_hint}each social increases x hearth growth(clan leader and governor)", Order = 61, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialHearthAdd { get; set; } = 0.25f;

    [SettingPropertyFloatingInteger("{=ssla}Social Settlement Loyalty Add", 0f, 100f, "0.00", HintText = "{=he_ssla_hint}each social increases x settlement loyalty(clan leader and governor)", Order = 62, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialSettlementLoyaltyAdd { get; set; } = 0.25f;

    [SettingPropertyFloatingInteger("{=sma}Social Militia Add", 0f, 100f, "0.00", HintText = "{=he_sma_hint}each social increases x millitia growth(clan leader and governor)", Order = 63, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialMilitiaAdd { get; set; } = 0.5f;

    [SettingPropertyFloatingInteger("{=srsp}Social Recruit Speed Percent", 0f, 100f, "0.00", HintText = "{=he_srsp_hint}each social increases x% settlement recruitment refresh rate(clan leader and governor)", Order = 64, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialRecruitSpeedPercent { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=stp}Social Tax Percent", 0f, 100f, "0.00", HintText = "{=he_stp_hint}each social increases x% tax income(governor)", Order = 65, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialTaxPercent { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=swpp}Social Workshop Production Percent", 0f, 100f, "0.00", HintText = "{=he_swpp_hint}each social increases x% workshop production(governor)", Order = 66, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialWorkshopProductionPercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=scca}Social Companion Capacity Add", 0f, 100f, "0.00", HintText = "{=he_scca_hint}each social increases x companion capacity(clan leader)", Order = 67, RequireRestart = false)]
    [SettingPropertyGroup("{=he_se}Social Enhancement")]
    public float SocialCompanionCapacityAdd { get; set; } = 0.2f;

    [SettingPropertyFloatingInteger("{=ier}Intelligence Exp Rate", 0f, 100f, "0.00", HintText = "{=he_ier_hint}each intelligence increases x% role exp gain and skill exp gain t, and double the effect on cunning, social and intelligence skill exp", Order = 70, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement", GroupOrder = 6)]
    public float IntelligenceExpRate { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=iaap}Intelligence Ammo Add Percent", 0f, 10f, "0.00", HintText = "{=he_iaap_hint}each intelligence increases x% max ammo to party(quartermaster)", Order = 80, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceAmmoAddPercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=isep}Intelligence Siege Endurance Percent", 0f, 100f, "0.00", HintText = "{=he_isep_hint}each intelligence increases x% siege weapon endurance", Order = 71, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceSiegeEndurancePercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=iwep}Intelligence Wall Endurance Percent", 0f, 100f, "0.00", HintText = "{=he_iwep_hint}each intelligence increases x% wall endurance", Order = 72, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceWallEndurancePercent { get; set; } = 0.1f;

    [SettingPropertyFloatingInteger("{=iba}Intelligence Ballista Add", 0f, 100f, "0.00", HintText = "{=he_iba_hint}each intelligence increases x ballista", Order = 73, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceBallistaAdd { get; set; } = 0.334f;

    [SettingPropertyFloatingInteger("{=he_ib}Intelligence boundary", 0f, 100f, "0.00", HintText = "{=he_ib_hint}Set the calculation boundary of intelligence", Order = 74, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceBoundary { get; set; } = 3.5f;

    [SettingPropertyFloatingInteger("{=ilsfp}Intelligence Leader Settlement Food Percent", 0f, 100f, "0.00", HintText = "{=he_ilsfp_hint}each intelligence increases x settlement food growth(clan leader)", Order = 75, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceLeaderSettlementFoodPercent { get; set; } = 0.5f;

    [SettingPropertyFloatingInteger("{=igsfp}Intelligence Governor Settlement Food Percent", 0f, 100f, "0.00", HintText = "{=he_igsfp_hint}each intelligence increases x settlement food growth(governor)", Order = 76, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceGovernorSettlementFoodPercent { get; set; } = 1f;

    [SettingPropertyFloatingInteger("{=ipfcrp}Intelligence Prosperity Food Cost Reduce Percent", 0f, 100f, "0.00", HintText = "{=he_ipfcrp_hint}each intelligence reduce x% prosperity food consume(governor)", Order = 77, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceProsperityFoodCostReducePercent { get; set; } = 0.075f;

    [SettingPropertyFloatingInteger("{=igwrp}Intelligence Garrison Wage Reduce Percent", 0f, 100f, "0.00", HintText = "{=he_igwrp_hint}each intelligence reduce x% garrison wage(governor)", Order = 78, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceGarrisonWageReducePercent { get; set; } = 0.05f;

    [SettingPropertyFloatingInteger("{=iwpp}Intelligence Workshop Production Percent", 0f, 100f, "0.00", HintText = "{=he_iwpp_hint}each intelligence increases x% workshop production(owner)", Order = 79, RequireRestart = false)]
    [SettingPropertyGroup("{=he_ie}Intelligence Enhancement")]
    public float IntelligenceWorkshopProductionPercent { get; set; } = 0.25f;

    [SettingPropertyBool("{=he_test}Test Mode", HintText = "{=he_test_hint}Every one will be full hp after under attack", Order = 99, RequireRestart = false)]
    public bool TestMode { get; set; } = false;
}
