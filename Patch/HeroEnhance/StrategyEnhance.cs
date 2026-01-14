using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using UFO.Extension;
using UFO.Setting;

internal class StrategyAttrEnhance
{
    [HarmonyPatch(typeof(DefaultPartySpeedCalculatingModel), "CalculateBaseSpeed")]
    internal class CalculateBaseSpeedPostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, MobileParty mobileParty, bool includeDescriptions, int additionalTroopOnFootCount, int additionalTroopOnHorseCount)
        {
            Hero effectiveScout = mobileParty.EffectiveScout;
            float num = effectiveScout.StrategyEnhanceRate();
            if (num != 0f)
            {
                int attributeValue = effectiveScout.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                __result.Add((float)attributeValue * SettingsManager.CunningPartySpeedAdd.Value * num, DefaultCharacterAttributes.Cunning.Name);
            }
        }
    }

    [HarmonyPatch(typeof(DefaultPrisonerRecruitmentCalculationModel), "GetConformityChangePerHour")]
    internal class GetConformityChangePerHourPostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, PartyBase party, CharacterObject troopToBoost)
        {
            Hero leaderHero = party.LeaderHero;
            float num = leaderHero.StrategyEnhanceRate();
            if (num != 0f)
            {
                int attributeValue = leaderHero.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                int result = (int)((float)__result.ResultNumber * (1f + (float)attributeValue * SettingsManager.CunningPrisonerRecruitSpeedPercent.Value * num));
                __result = new ExplainedNumber(result);
            }
        }
    }

    [HarmonyPatch(typeof(DefaultPartySizeLimitModel), "AddMobilePartyLeaderPrisonerSizePerkEffects")]
    internal class AddMobilePartyLeaderPrisonerSizePerkEffectsPostfixPatch
    {
        private static void Postfix(PartyBase party, ref ExplainedNumber result)
        {
            Hero leaderHero = party.LeaderHero;
            float num = leaderHero.StrategyEnhanceRate();
            if (num != 0f)
            {
                int attributeValue = leaderHero.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                result.AddFactor((float)attributeValue * SettingsManager.CunningPrisonerCapacityPercent.Value * num, DefaultCharacterAttributes.Cunning.Name);
            }
        }
    }

    [HarmonyPatch(typeof(DefaultRaidModel), "CalculateHitDamage")]
    internal class CalculateHitDamagePostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, MapEventSide attackerSide, float settlementHitPoints)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                return;
            }
            foreach (MapEventParty party in attackerSide.Parties)
            {
                MobileParty mobileParty = party.Party.MobileParty;
                if (mobileParty != null)
                {
                    Hero leaderHero = mobileParty.LeaderHero;
                    float num = leaderHero.StrategyEnhanceRate();
                    if (num != 0f)
                    {
                        int attributeValue = leaderHero.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                        float multiplier = 1f + (float)attributeValue * SettingsManager.CunningRaidSpeedPercent.Value;
                        __result = new ExplainedNumber(__result.ResultNumber * multiplier);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementLoyaltyModel), "GetSettlementLoyaltyChangeDueToGovernorPerks")]
    internal class DailyTickPartyPostfixPatch
    {
        private static void Postfix(Town town, ref ExplainedNumber explainedNumber)
        {
            if (!HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                Hero owner = town.Settlement.Owner;
                float num = owner.StrategyEnhanceRate();
                float num2 = 0f;
                if (num > 0f)
                {
                    num2 = (float)owner.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num2 = ((!(num2 > 0f)) ? (num2 / num) : (num2 * num));
                    explainedNumber.Add(num2 * SettingsManager.SocialSettlementLoyaltyAdd.Value, owner.Name);
                }
                Hero governor = town.Governor;
                float num3 = governor.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    float num4 = (float)governor.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num4 = ((!(num4 > 0f)) ? (num4 / num3) : (num4 * num3));
                    explainedNumber.Add(num4 * SettingsManager.SocialSettlementLoyaltyAdd.Value, governor.Name);
                }
                else if (num > 0f)
                {
                    explainedNumber.Add(num2 * SettingsManager.SocialSettlementLoyaltyAdd.Value * 0.5f, owner.Name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementProsperityModel), "CalculateHearthChange")]
    internal class CalculateHearthChangeInternalPostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, Village village)
        {
            if (!HeroEnhanceExtensions.StrategyEnhanceDisable() && village.Bound != null)
            {
                Hero owner = village.Bound.Owner;
                float num = owner.StrategyEnhanceRate();
                float num2 = 0f;
                if (num > 0f)
                {
                    num2 = (float)owner.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num2 = ((!(num2 > 0f)) ? (num2 / num) : (num2 * num));
                    __result.Add(num2 * SettingsManager.SocialHearthAdd.Value, owner.Name);
                }
                Hero governor = village.Bound.Town.Governor;
                float num3 = governor.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    float num4 = (float)governor.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num4 = ((!(num4 > 0f)) ? (num4 / num3) : (num4 * num3));
                    __result.Add(num4 * SettingsManager.SocialHearthAdd.Value, governor.Name);
                }
                else if (num > 0f)
                {
                    __result.Add(num2 * SettingsManager.SocialHearthAdd.Value * 0.5f, owner.Name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementMilitiaModel), "CalculateMilitiaChange")]
    internal class CalculateMilitiaChangePostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, Settlement settlement)
        {
            if (!HeroEnhanceExtensions.StrategyEnhanceDisable() && settlement != null)
            {
                Hero owner = settlement.Owner;
                float num = owner.StrategyEnhanceRate();
                float num2 = 0f;
                if (num > 0f)
                {
                    num2 = (float)owner.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num2 = ((!(num2 > 0f)) ? (num2 / num) : (num2 * num));
                    __result.Add(num2 * SettingsManager.SocialMilitiaAdd.Value, owner.Name);
                }
                Hero hero = null;
                if (settlement.IsVillage)
                {
                    hero = settlement.Village.Bound.Town.Governor;
                }
                else if (settlement.IsFortification)
                {
                    hero = settlement.Town.Governor;
                }
                float num3 = hero.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    float num4 = (float)hero.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num4 = ((!(num4 > 0f)) ? (num4 / num3) : (num4 * num3));
                    __result.Add(num4 * SettingsManager.SocialMilitiaAdd.Value, hero.Name);
                }
                else if (num > 0f)
                {
                    __result.Add(num2 * SettingsManager.SocialMilitiaAdd.Value * 0.5f, owner.Name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultPersuasionModel), "GetDefaultSuccessChance")]
    internal class GetDefaultSuccessChancePostfixPatch
    {
        private static void Postfix(ref float __result, PersuasionOptionArgs optionArgs, float difficultyMultiplier)
        {
            float num = Hero.MainHero.StrategyEnhanceRate();
            if (num != 0f)
            {
                int attributeValue = Hero.MainHero.GetAttributeValue(DefaultCharacterAttributes.Social);
                __result += (float)attributeValue * 0.02f * num;
                if (__result > 1f)
                {
                    __result = 1f;
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultClanTierModel), "GetCompanionLimit")]
    internal class GetCompanionLimitPostfixPatch
    {
        private static void Postfix(ref int __result, Clan clan)
        {
            if (!HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                Hero leader = clan.Leader;
                float num = leader.StrategyEnhanceRate();
                if (num != 0f)
                {
                    int attributeValue = leader.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                    __result += (int)((float)attributeValue * SettingsManager.CunningCompanionCapacityAdd.Value * num);
                    int attributeValue2 = leader.GetAttributeValue(DefaultCharacterAttributes.Social);
                    __result += (int)((float)attributeValue2 * SettingsManager.SocialCompanionCapacityAdd.Value * num);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultVolunteerModel), "GetDailyVolunteerProductionProbability")]
    internal class GetDailyVolunteerProductionProbabilityPostfixPatch
    {
        private static void Postfix(ref float __result, Settlement settlement)
        {
            if (!HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                float num = 1f;
                Hero owner = settlement.Owner;
                float num2 = owner.StrategyEnhanceRate();
                float num3 = 0f;
                if (num2 > 0f)
                {
                    num3 = (float)owner.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num3 = ((!(num3 > 0f)) ? (num3 / num2) : (num3 * num2));
                    num += num3 * SettingsManager.SocialRecruitSpeedPercent.Value;
                }
                Hero hero = null;
                if (settlement.IsVillage)
                {
                    hero = settlement.Village.Bound.Town.Governor;
                }
                else if (settlement.IsFortification)
                {
                    hero = settlement.Town.Governor;
                }
                float num4 = hero.StrategyEnhanceRate();
                if (num4 > 0f)
                {
                    float num5 = (float)hero.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num5 = ((!(num5 > 0f)) ? (num5 / num4) : (num5 * num4));
                    num += num5 * SettingsManager.SocialRecruitSpeedPercent.Value;
                }
                else if (num2 > 0f)
                {
                    num += num3 * SettingsManager.SocialRecruitSpeedPercent.Value * 0.5f;
                }
                __result *= num;
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementTaxModel), "CalculateTownTax")]
    internal class CalculateTownTaxPostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, Town town, bool includeDescriptions = false)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable() || (!town.IsTown && !town.IsCastle))
            {
                return;
            }
            Hero governor = town.Governor;
            float num = governor.StrategyEnhanceRate();
            if (num > 0f)
            {
                float num2 = (float)governor.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                num2 = ((!(num2 > 0f)) ? (num2 / num) : (num2 * num));
                __result.AddFactor(num2 * SettingsManager.SocialTaxPercent.Value, governor.Name);
            }
            else if (town.Settlement.Owner != null)
            {
                Hero owner = town.Settlement.Owner;
                float num3 = owner.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    float num4 = (float)owner.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num4 = ((!(num4 > 0f)) ? (num4 / num3) : (num4 * num3));
                    __result.AddFactor(num4 * SettingsManager.SocialTaxPercent.Value * 0.5f, owner.Name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultPartyWageModel), "GetTotalWage")]
    public static class GetTotalWagePostfixPatch
    {
        public static void Postfix(ref MobileParty mobileParty, ref bool includeDescriptions, ref ExplainedNumber __result)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable() || !mobileParty.IsGarrison)
            {
                return;
            }
            Settlement currentSettlement = mobileParty.CurrentSettlement;
            if (currentSettlement == null || (!currentSettlement.IsTown && !currentSettlement.IsCastle))
            {
                return;
            }
            Hero governor = currentSettlement.Town.Governor;
            float num = governor.StrategyEnhanceRate();
            if (num > 0f)
            {
                float num2 = (float)governor.GetAttributeValue(DefaultCharacterAttributes.Intelligence) - SettingsManager.IntelligenceBoundary.Value;
                num2 = ((!(num2 > 0f)) ? (num2 / num) : (num2 * num));
                __result.AddFactor(num2 * (0f - SettingsManager.IntelligenceGarrisonWageReducePercent.Value), governor.Name);
            }
            else if (currentSettlement.Owner != null)
            {
                Hero owner = currentSettlement.Owner;
                float num3 = owner.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    float num4 = (float)currentSettlement.Owner.GetAttributeValue(DefaultCharacterAttributes.Intelligence) - SettingsManager.IntelligenceBoundary.Value;
                    num4 = ((!(num4 > 0f)) ? (num4 / num3) : (num4 * num3));
                    __result.AddFactor(num4 * (0f - SettingsManager.IntelligenceGarrisonWageReducePercent.Value) * 0.5f, currentSettlement.Owner.Name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(WorkshopsCampaignBehavior), "RunTownWorkshop")]
    internal class RunTownWorkshopPrefixPatch
    {
        private static bool Prefix(WorkshopsCampaignBehavior __instance, ref Town townComponent, ref Workshop workshop)
        {
            WorkshopType workshopType = workshop.WorkshopType;
            bool flag = false;
            for (int i = 0; i < workshopType.Productions.Count; i++)
            {
                float productionProgress = workshop.GetProductionProgress(i);
                productionProgress += Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, workshopType.Productions[i].ConversionSpeed, includeDescriptions: false).ResultNumber;
                if (productionProgress >= 1f)
                {
                    bool flag2 = true;
                    while (flag2 && productionProgress >= 1f)
                    {
                        flag2 = ((workshop.Owner != Hero.MainHero) ? ((bool)tickNotableWorkshop.Invoke(__instance, new object[2]
                        {
                            workshopType.Productions[i],
                            workshop
                        })) : ((bool)tickPlayerWorkshop.Invoke(__instance, new object[2]
                        {
                            workshopType.Productions[i],
                            workshop
                        })));
                        if (flag2)
                        {
                            flag = true;
                        }
                        productionProgress -= 1f;
                    }
                }
                workshop.SetProgress(i, productionProgress);
            }
            if (flag)
            {
                workshop.UpdateLastRunTime();
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(DefaultWorkshopModel), "GetEffectiveConversionSpeedOfProduction")]
    internal class GetEffectiveConversionSpeedOfProductionPostfixPatch
    {
        private static void Postfix(ref DefaultWorkshopModel __instance, ref ExplainedNumber __result, ref Workshop workshop, ref float speed, ref bool includeDescription)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                return;
            }
            Settlement settlement = workshop.Settlement;
            Hero governor = settlement.Town.Governor;
            float num = 0f;
            float num2 = governor.StrategyEnhanceRate();
            if (num2 > 0f)
            {
                float num3 = (float)governor.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                num3 = ((!(num3 > 0f)) ? (num3 / num2) : (num3 * num2));
                num += num3 * SettingsManager.SocialWorkshopProductionPercent.Value;
            }
            else if (settlement.Owner != null)
            {
                Hero owner = settlement.Owner;
                float num4 = owner.StrategyEnhanceRate();
                if (num4 > 0f)
                {
                    float num5 = (float)settlement.Owner.GetAttributeValue(DefaultCharacterAttributes.Social) - SettingsManager.SocialBoundary.Value;
                    num5 = ((!(num5 > 0f)) ? (num5 / num4) : (num5 * num4));
                    num += num5 * SettingsManager.SocialWorkshopProductionPercent.Value * 0.5f;
                }
            }
            if (workshop.Owner != null)
            {
                float num6 = workshop.Owner.StrategyEnhanceRate();
                if (num6 > 0f)
                {
                    float num7 = (float)workshop.Owner.GetAttributeValue(DefaultCharacterAttributes.Intelligence) - SettingsManager.IntelligenceBoundary.Value;
                    num7 = ((!(num7 > 0f)) ? (num7 / num6) : (num7 * num6));
                    num += num7 * SettingsManager.IntelligenceWorkshopProductionPercent.Value;
                }
            }
            if (num != 0f)
            {
                __result.AddFactor(num);
            }
        }
    }

    [HarmonyPatch(typeof(Hero), "AddSkillXp")]
    internal class AddSkillXpPrefixPatch
    {
        private static bool Prefix(ref Hero __instance, ref SkillObject skill, ref float xpAmount)
        {
            if (SettingsManager.TestMode.Value && (
                skill.Attributes != null && (
                    skill.Attributes.Contains(DefaultCharacterAttributes.Vigor) ||
                    skill.Attributes.Contains(DefaultCharacterAttributes.Control) ||
                    skill.Attributes.Contains(DefaultCharacterAttributes.Endurance)
                )
            ))
            {
                xpAmount = 0f;
            }
            float num = __instance.StrategyEnhanceRate();
            if (num == 0f)
            {
                return true;
            }
            int attributeValue = __instance.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
            float num2 = (float)attributeValue * SettingsManager.IntelligenceExpRate.Value * num;
            float num3 = 1f + num2;
            if (skill.Attributes != null && (
                skill.Attributes.Contains(DefaultCharacterAttributes.Cunning) ||
                skill.Attributes.Contains(DefaultCharacterAttributes.Social) ||
                skill.Attributes.Contains(DefaultCharacterAttributes.Intelligence)
            ))
            {
                num3 += num2;
            }
            xpAmount *= num3;
            return true;
        }
    }

    [HarmonyPatch(typeof(HeroDeveloper), "GainRawXp")]
    internal class GainRawXpPrefixPatch
    {
        private static bool Prefix(ref HeroDeveloper __instance, ref float rawXp)
        {
            float num = __instance.Hero.StrategyEnhanceRate();
            if (num == 0f)
            {
                return true;
            }
            int attributeValue = __instance.Hero.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
            rawXp *= 1f + (float)attributeValue * SettingsManager.IntelligenceExpRate.Value * num;
            return true;
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementFoodModel), "CalculateTownFoodStocksChange")]
    internal class CalculateTownFoodStocksChangePostfixPatch
    {
        private static void Postfix(ref DefaultSettlementFoodModel __instance, ref ExplainedNumber __result, ref Town town)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                return;
            }
            Hero owner = town.Settlement.Owner;
            float num = owner.StrategyEnhanceRate();
            float num2 = 0f;
            if (num > 0f)
            {
                num2 = (float)owner.GetAttributeValue(DefaultCharacterAttributes.Intelligence) - SettingsManager.IntelligenceBoundary.Value;
                num2 = ((!(num2 > 0f)) ? (num2 / num) : (num2 * num));
                __result.Add(num2 * SettingsManager.IntelligenceLeaderSettlementFoodPercent.Value, owner.Name);
            }
            float num3 = (0f - town.Prosperity) / (float)__instance.NumberOfProsperityToEatOneFood;
            float num4 = 0f;
            Hero governor = town.Governor;
            float num5 = governor.StrategyEnhanceRate();
            if (num5 > 0f)
            {
                if (town.IsUnderSiege && governor.GetPerkValue(DefaultPerks.Medicine.TriageTent))
                {
                    num4 += DefaultPerks.Medicine.TriageTent.SecondaryBonus;
                }
                if (governor.GetPerkValue(DefaultPerks.Steward.MasterOfWarcraft))
                {
                    num4 += DefaultPerks.Steward.MasterOfWarcraft.SecondaryBonus;
                }
                num3 += num3 * num4;
                float num6 = (float)governor.GetAttributeValue(DefaultCharacterAttributes.Intelligence) - SettingsManager.IntelligenceBoundary.Value;
                num6 = ((!(num6 > 0f)) ? (num6 / num5) : (num6 * num5));
                __result.Add(num6 * SettingsManager.IntelligenceGovernorSettlementFoodPercent.Value, governor.Name);
                float value = (0f - num3) * num6 * SettingsManager.IntelligenceProsperityFoodCostReducePercent.Value + num6 * 1f;
                __result.Add(value, governor.Name);
            }
            else if (num > 0f)
            {
                float value2 = ((0f - num3) * num2 * SettingsManager.IntelligenceProsperityFoodCostReducePercent.Value + num2 * 1f) * 0.5f;
                __result.Add(value2, owner.Name);
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSiegeEventModel), "GetPrebuiltSiegeEnginesOfSettlement")]
    internal class GetPrebuiltSiegeEnginesOfSettlementPostfixPatch
    {
        private static void Postfix(ref IEnumerable<SiegeEngineType> __result, ref Settlement settlement)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                return;
            }
            int num = 0;
            Hero governor = settlement.Town.Governor;
            float num2 = governor.StrategyEnhanceRate();
            if (num2 > 0f)
            {
                num = (int)((float)governor.GetAttributeValue(DefaultCharacterAttributes.Intelligence) * SettingsManager.IntelligenceBallistaAdd.Value * num2);
            }
            else if (settlement.Owner != null)
            {
                float num3 = settlement.Owner.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    num = (int)((float)settlement.Owner.GetAttributeValue(DefaultCharacterAttributes.Intelligence) * SettingsManager.IntelligenceBallistaAdd.Value * num3 * 0.5f);
                }
            }
            if (num > 0)
            {
                List<SiegeEngineType> list = (List<SiegeEngineType>)__result;
                while (num-- > 0)
                {
                    list.Add(DefaultSiegeEngineTypes.Ballista);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSiegeEventModel), "GetPrebuiltSiegeEnginesOfSiegeCamp")]
    internal class GetPrebuiltSiegeEnginesOfSiegeCampPostfixPatch
    {
        private static void Postfix(ref IEnumerable<SiegeEngineType> __result, ref BesiegerCamp besiegerCamp)
        {
            Hero effectiveEngineer = besiegerCamp.LeaderParty.EffectiveEngineer;
            float num = effectiveEngineer.StrategyEnhanceRate();
            if (num > 0f)
            {
                List<SiegeEngineType> list = (List<SiegeEngineType>)__result;
                int num2 = (int)((float)effectiveEngineer.GetAttributeValue(DefaultCharacterAttributes.Intelligence) * SettingsManager.IntelligenceBallistaAdd.Value * num);
                while (num2-- > 0)
                {
                    list.Add(DefaultSiegeEngineTypes.Ballista);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultSiegeEventModel), "GetSiegeEngineHitPoints")]
    internal class GetSiegeEngineHitPointsPostfixPatch
    {
        private static void Postfix(ref DefaultSiegeEventModel __instance, ref float __result, ref SiegeEvent siegeEvent, ref SiegeEngineType siegeEngine, ref BattleSideEnum battleSide)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                return;
            }
            Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
            MobileParty effectiveSiegePartyForSide = __instance.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
            if (battleSide == BattleSideEnum.Defender)
            {
                if (besiegedSettlement.Town.Governor != null)
                {
                    float num = besiegedSettlement.Town.Governor.StrategyEnhanceRate();
                    if (num > 0f)
                    {
                        int attributeValue = besiegedSettlement.Town.Governor.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                        __result *= 1f + (float)attributeValue * SettingsManager.IntelligenceSiegeEndurancePercent.Value * num;
                    }
                }
                else if (besiegedSettlement.Owner != null)
                {
                    float num2 = besiegedSettlement.Owner.StrategyEnhanceRate();
                    if (num2 > 0f)
                    {
                        int attributeValue2 = besiegedSettlement.Owner.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                        __result *= 1f + (float)attributeValue2 * SettingsManager.IntelligenceSiegeEndurancePercent.Value * num2 * 0.5f;
                    }
                }
            }
            else if (battleSide == BattleSideEnum.Attacker && effectiveSiegePartyForSide != null)
            {
                Hero effectiveEngineer = effectiveSiegePartyForSide.EffectiveEngineer;
                float num3 = effectiveEngineer.StrategyEnhanceRate();
                if (num3 > 0f)
                {
                    int attributeValue3 = effectiveEngineer.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                    __result *= 1f + (float)attributeValue3 * SettingsManager.IntelligenceSiegeEndurancePercent.Value * num3;
                }
            }
        }
    }

    [HarmonyPatch(typeof(DefaultWallHitPointCalculationModel), "CalculateMaximumWallHitPointInternal")]
    internal class CalculateMaximumWallHitPointPostfixPatch
    {
        private static void Postfix(ref float __result, ref Town town)
        {
            if (HeroEnhanceExtensions.StrategyEnhanceDisable())
            {
                return;
            }
            if (town.Governor != null)
            {
                float num = town.Governor.StrategyEnhanceRate();
                if (num > 0f)
                {
                    int attributeValue = town.Governor.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                    __result *= 1f + (float)attributeValue * SettingsManager.IntelligenceWallEndurancePercent.Value * num;
                }
            }
            else if (town.Settlement.Owner != null)
            {
                float num2 = town.Settlement.Owner.StrategyEnhanceRate();
                if (num2 > 0f)
                {
                    int attributeValue2 = town.Settlement.Owner.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                    __result *= 1f + (float)attributeValue2 * SettingsManager.IntelligenceWallEndurancePercent.Value * num2 * 0.5f;
                }
            }
        }
    }

    private static readonly MethodInfo tickPlayerWorkshop = typeof(WorkshopsCampaignBehavior).GetMethod("TickOneProductionCycleForPlayerWorkshop", BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly MethodInfo tickNotableWorkshop = typeof(WorkshopsCampaignBehavior).GetMethod("TickOneProductionCycleForNotableWorkshop", BindingFlags.Instance | BindingFlags.NonPublic);

}
