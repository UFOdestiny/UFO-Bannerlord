using MCM.Common;

internal interface ISettings
{
    bool EnableEverYoung { get; set; }

    int EverYoungSkillNeed { get; set; }

    Dropdown<string> AutoChoosePerk { get; set; }

    float VigorDmgPercent { get; set; }

    float VigorArmorAdd { get; set; }

    float VigorMountArmorAdd { get; set; }

    float VigorShieldEndurancePercent { get; set; }

    float VigorFinalDmgAdd { get; set; }

    float VigorDmgTakenReduce { get; set; }

    int VigorCrushThroughPositive { get; set; }

    int VigorCrushThroughNegative { get; set; }

    float IntelligenceAmmoAddPercent { get; set; }

    int ControlAmmoNoConsumeRate { get; set; }

    float ControlDropDmgReducePercent { get; set; }

    float ControlAimStabilityPercent { get; set; }

    float ControlMountManeuverPercent { get; set; }

    int ControlCritRate { get; set; }

    int ControlExemptionRate { get; set; }

    int ControlPenetrateRate { get; set; }

    float EnduranceHpAddPercent { get; set; }

    float EnduranceHealRate { get; set; }

    float EnduranceStaggerPercent { get; set; }

    float EnduranceWalkSpeedPercent { get; set; }

    float EnduranceMountSpeedPercent { get; set; }

    float CunningPrisonerRecruitSpeedPercent { get; set; }

    float CunningPrisonerCapacityPercent { get; set; }

    float CunningRaidSpeedPercent { get; set; }

    float CunningPartySpeedAdd { get; set; }

    float CunningCompanionCapacityAdd { get; set; }

    float SocialBoundary { get; set; }

    float SocialHearthAdd { get; set; }

    float SocialSettlementLoyaltyAdd { get; set; }

    float SocialMilitiaAdd { get; set; }

    float SocialRecruitSpeedPercent { get; set; }

    float SocialTaxPercent { get; set; }

    float SocialWorkshopProductionPercent { get; set; }

    float SocialCompanionCapacityAdd { get; set; }

    float IntelligenceBoundary { get; set; }

    float IntelligenceExpRate { get; set; }

    float IntelligenceSiegeEndurancePercent { get; set; }

    float IntelligenceWallEndurancePercent { get; set; }

    float IntelligenceBallistaAdd { get; set; }

    float IntelligenceLeaderSettlementFoodPercent { get; set; }

    float IntelligenceGovernorSettlementFoodPercent { get; set; }

    float IntelligenceProsperityFoodCostReducePercent { get; set; }

    float IntelligenceGarrisonWageReducePercent { get; set; }

    float IntelligenceWorkshopProductionPercent { get; set; }

    bool EnableDailyGainXp { get; set; }

    float CombatAttributeRatePlayer { get; set; }

    float CombatAttributeRateClanMember { get; set; }

    float CombatAttributeRateOther { get; set; }

    float StrategyAttributeRatePlayer { get; set; }

    float StrategyAttributeRateClanMember { get; set; }

    float StrategyAttributeRateOther { get; set; }

    bool TestMode { get; set; }
}
