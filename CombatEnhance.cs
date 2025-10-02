using UFO;
using System;
using System.Collections.Generic;
using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.MountAndBlade.Agent;
using static TaleWorlds.CampaignSystem.CharacterDevelopment.DefaultPerks;



internal class CombatAttrEnhance
{
    [HarmonyPatch(typeof(DefaultCharacterStatsModel), "MaxHitpoints")]
    internal class MaxHitpointsPostfixPatch
    {
        private static void Postfix(ref ExplainedNumber __result, ref CharacterObject character, ref bool includeDescriptions)
        {
            float num = character.CombatEnhanceRate();
            if (num != 0f)
            {
                Hero heroObject = character.HeroObject;
                int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Endurance);
                float value = (float)attributeValue * settings.EnduranceHpAddPercent * num;
                __result.AddFactor(value, DefaultCharacterAttributes.Endurance.Name);
            }
        }
    }

    [HarmonyPatch(typeof(DefaultPartyHealingModel), "GetHeroesEffectedHealingAmount")]
    internal class GetHeroesEffectedHealingAmountPostfixPatch
    {
        private static void Postfix(ref int __result, ref Hero hero, ref float healingRate)
        {
            float num = hero.CombatEnhanceRate();
            if (num != 0f)
            {
                int attributeValue = hero.GetAttributeValue(DefaultCharacterAttributes.Endurance);
                __result = (int)((float)__result * (1f + (float)attributeValue * settings.EnduranceHealRate * num));
            }
        }
    }

    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "DecideCrushedThrough")]
    internal class DecideCrushedThroughPostfixPatch
    {
        private static void Postfix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
        {
            if (settings.TestMode)
            {
                return;
            }
            if (attackerAgent.IsPlayerControlled)
            {
                __result = true;
                return;
            }

            float num = attackerAgent.CombatEnhanceRate();
            float num2 = defenderAgent.CombatEnhanceRate();
            if (num == 0f && num2 == 0f)
            {
                return;
            }
            int num3 = 0;
            int num4 = 0;
            if (num > 0f)
            {
                CharacterObject characterObject = attackerAgent.Character as CharacterObject;
                Hero heroObject = characterObject.HeroObject;
                num3 = (int)((float)heroObject.GetAttributeValue(DefaultCharacterAttributes.Vigor) * num);
            }
            if (num2 > 0f)
            {
                CharacterObject characterObject2 = defenderAgent.Character as CharacterObject;
                Hero heroObject2 = characterObject2.HeroObject;
                num4 = (int)((float)heroObject2.GetAttributeValue(DefaultCharacterAttributes.Vigor) * num2);
            }
            int num5 = num3 - num4;
            if (num5 > 0 && !__result)
            {
                if (MBRandom.RandomInt(100) < num5 * settings.VigorCrushThroughPositive)
                {
                    __result = true;
                }
            }
            else if (((num5 < 0) & __result) && MBRandom.RandomInt(100) < -num5 * settings.VigorCrushThroughNegative)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "DecideMissileWeaponFlags")]
    internal class DecideMissileWeaponFlagsPostfixPatch
    {
        private static void Postfix(ref Agent attackerAgent, MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
        {
            float num = attackerAgent.CombatEnhanceRate();
            if (num != 0f)
            {
                CharacterObject characterObject = attackerAgent.Character as CharacterObject;
                Hero heroObject = characterObject.HeroObject;
                int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Control);
                int num2 = (int)((float)(attributeValue * settings.ControlPenetrateRate) * num);
                if (MBRandom.RandomInt(100) < num2)
                {
                    missileWeaponFlags |= WeaponFlags.CanPenetrateShield;
                }
            }
        }
    }

    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
    internal class SandboxCalculateDamagePrefixPatch
    {
        private static bool Prefix(ref AttackInformation attackInformation, ref AttackCollisionData collisionData, ref MissionWeapon weapon, ref float baseDamage)
        {
            return CalculateDamage(ref attackInformation, ref collisionData, ref weapon, ref baseDamage);
        }
    }

    [HarmonyPatch(typeof(CustomAgentApplyDamageModel), "CalculateDamage")]
    internal class CalculateDamagePrefixPatch
    {
        private static bool Prefix(ref AttackInformation attackInformation, ref AttackCollisionData collisionData, ref MissionWeapon weapon, ref float baseDamage)
        {
            return CalculateDamage(ref attackInformation, ref collisionData, ref weapon, ref baseDamage);
        }
    }

    [HarmonyPatch(typeof(Agent), "Health", MethodType.Setter)]
    internal class SetHealthPrefixPatch
    {
        private static bool Prefix(ref float value)
        {
            if (!settings.TestMode)
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Agent), "Die")]
    internal class setDiePrefixPatch
    {
        private static bool Prefix(Blow b, Agent.KillInfo overrideKillInfo = Agent.KillInfo.Invalid)
        {
            if (!settings.TestMode)
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateStaggerThresholdDamage")]
    internal class CalculateStaggerThresholdDamagePostfixPatch
    {
        private static void Postfix(ref float __result, Agent defenderAgent)
        {
            float num = defenderAgent.CombatEnhanceRate();
            if (num != 0f)
            {
                CharacterObject characterObject = defenderAgent.Character as CharacterObject;
                Hero heroObject = characterObject.HeroObject;
                int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Endurance);
                __result *= 1f + (float)attributeValue * settings.EnduranceStaggerPercent * num;
            }
        }
    }

    [HarmonyPatch(typeof(SandboxAgentStatCalculateModel), "SetPerkAndBannerEffectsOnAgent")]
    internal class SetPerkAndBannerEffectsOnAgentPostfixPatch
    {
        private static void Postfix(ref Agent agent, CharacterObject agentCharacter, ref AgentDrivenProperties agentDrivenProperties, ref WeaponComponentData equippedWeaponComponent)
        {
            float num = agentCharacter.CombatEnhanceRate();
            if (num != 0f)
            {
                Hero heroObject = agentCharacter.HeroObject;
                int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Vigor);
                int attributeValue2 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Control);
                int attributeValue3 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Endurance);
                float num2 = 0f;
                if (!agent.HasMount && agentCharacter.GetPerkValue(DefaultPerks.Athletics.IgnorePain))
                {
                    num2 += DefaultPerks.Athletics.IgnorePain.PrimaryBonus;
                }
                float num3 = 1f + 0.01f * num2;
                float num4 = (float)attributeValue * settings.VigorArmorAdd * num3 * num;
                agentDrivenProperties.ArmorHead += num4;
                agentDrivenProperties.ArmorTorso += num4;
                agentDrivenProperties.ArmorArms += num4;
                agentDrivenProperties.ArmorLegs += num4;
                agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 1f - (float)attributeValue2 * settings.ControlAimStabilityPercent * num;
                agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1f - (float)attributeValue2 * settings.ControlAimStabilityPercent * num;
                agentDrivenProperties.WeaponUnsteadyBeginTime *= 1f + (float)attributeValue2 * settings.ControlAimStabilityPercent * num;
                agentDrivenProperties.WeaponUnsteadyEndTime *= 1f + (float)attributeValue2 * settings.ControlAimStabilityPercent * num;
                agentDrivenProperties.HandlingMultiplier *= 1f + (float)attributeValue2 * settings.ControlAimStabilityPercent * num;
                agentDrivenProperties.CombatMaxSpeedMultiplier *= 1f + (float)attributeValue3 * settings.EnduranceWalkSpeedPercent * num;
                agentDrivenProperties.MaxSpeedMultiplier *= 1f + (float)attributeValue3 * settings.EnduranceWalkSpeedPercent * num;
            }
        }
    }

    [HarmonyPatch(typeof(SandboxAgentStatCalculateModel), "UpdateHorseStats")]
    internal class UpdateHorseStatsPostfixPatch
    {
        private static void Postfix(ref Agent agent, ref AgentDrivenProperties agentDrivenProperties)
        {
            float num = agent.CombatEnhanceRate();
            if (num != 0f)
            {
                CharacterObject characterObject = agent.Character as CharacterObject;
                Hero heroObject = characterObject.HeroObject;
                int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Vigor);
                int attributeValue2 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Control);
                int attributeValue3 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Endurance);
                agentDrivenProperties.ArmorTorso += (float)attributeValue * settings.VigorArmorAdd * num;
                agentDrivenProperties.MountManeuver *= 1f + (float)attributeValue2 * settings.ControlMountManeuverPercent * num;
                agentDrivenProperties.MountSpeed *= 1f + (float)attributeValue3 * settings.EnduranceMountSpeedPercent * num;
            }
        }
    }

    [HarmonyPatch(typeof(AgentStatCalculateModel), "CalculateAILevel")]
    internal class CalculateAILevelPostfixPatch
    {
        private static void Postfix(ref float __result, Agent agent, int relevantSkillLevel)
        {
            if (!UFO.Extensions.CombatEnhanceDisable())
            {
                __result /= 2f;
                if (agent.Character is CharacterObject { HeroObject: { } heroObject })
                {
                    int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                    __result += (float)attributeValue * 0.1f;
                    __result = Math.Min(__result, 1f);
                }
            }
        }
    }

    [HarmonyPatch(typeof(AgentStatCalculateModel), "SetAiRelatedProperties")]
    internal class SetAiRelatedPropertiesPatch
    {
        private static void Postfix(ref Agent agent, ref AgentDrivenProperties agentDrivenProperties, ref WeaponComponentData equippedItem, ref WeaponComponentData secondaryItem)
        {
            if (UFO.Extensions.CombatEnhanceDisable() || !(agent.Character is CharacterObject characterObject))
            {
                return;
            }
            float num = 0.3f + (float)characterObject.Level / 100f;
            Hero heroObject = characterObject.HeroObject;
            if (heroObject != null)
            {
                float num2 = heroObject.CombatEnhanceRate();
                if (num2 > 0f)
                {
                    float num3 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Cunning);
                    float num4 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                    num += num3 * 0.03f * num2;
                    num += num4 * 0.03f * num2;
                }
            }
            agentDrivenProperties.AIBlockOnDecideAbility *= num;
            agentDrivenProperties.AIParryOnDecideAbility *= num;
            agentDrivenProperties.AiShootFreq *= num;
            agentDrivenProperties.AiRangedHorsebackMissileRange *= num;
            agentDrivenProperties.AiFacingMissileWatch /= num;
            agentDrivenProperties.AiWaitBeforeShootFactor /= num;
            agentDrivenProperties.AiRandomizedDefendDirectionChance /= num;
            agentDrivenProperties.AiShooterError /= num;
            agentDrivenProperties.AiRangerLeadErrorMin /= num;
            agentDrivenProperties.AiRangerLeadErrorMax /= num;
            agentDrivenProperties.AiRangerVerticalErrorMultiplier /= num;
            agentDrivenProperties.AiRangerHorizontalErrorMultiplier /= num;
        }
    }

    [HarmonyPatch(typeof(SandboxAgentStatCalculateModel), "InitializeMissionEquipment")]
    internal class InitializeMissionEquipmentPostfixPatch
    {
        private static void Postfix(ref Agent agent)
        {
            if (agent == null)
            {
                return;
            }
            float num = 0f;
            object obj = agent.Origin?.BattleCombatant;
            if (obj != null)
            {
                PartyBase partyBase = (PartyBase)obj;
                MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
                if (mobileParty != null)
                {
                    Hero effectiveQuartermaster = mobileParty.EffectiveQuartermaster;
                    if (effectiveQuartermaster != null)
                    {
                        float num2 = effectiveQuartermaster.CombatEnhanceRate();
                        int attributeValue = effectiveQuartermaster.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
                        num = (float)attributeValue * settings.IntelligenceAmmoAddPercent * num2;
                    }
                }
            }
            float num3 = agent.CombatEnhanceRate();
            if (num3 == 0f)
            {
                return;
            }
            CharacterObject characterObject = agent.Character as CharacterObject;
            Hero heroObject = characterObject.HeroObject;
            int attributeValue2 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Vigor);
            MissionEquipment equipment = agent.Equipment;
            for (int i = 0; i < 5; i++)
            {
                EquipmentIndex equipmentIndex = (EquipmentIndex)i;
                MissionWeapon missionWeapon = equipment[equipmentIndex];
                if (missionWeapon.IsEmpty)
                {
                    continue;
                }
                WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
                if (currentUsageItem == null)
                {
                    continue;
                }
                if (currentUsageItem.IsConsumable)
                {
                    short amount = missionWeapon.Amount;
                    short num4 = (short)((float)amount * (1f + num));
                    if (amount != num4)
                    {
                        equipment.SetAmountOfSlot(equipmentIndex, num4, addOverflowToMaxAmount: true);
                    }
                }
                else if (currentUsageItem.IsShield)
                {
                    short hitPoints = missionWeapon.HitPoints;
                    short num5 = (short)((float)hitPoints * (1f + (float)attributeValue2 * settings.VigorShieldEndurancePercent * num3));
                    if (hitPoints != num5)
                    {
                        equipment.SetHitPointsOfSlot(equipmentIndex, num5, addOverflowToMaxHitPoints: true);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Agent), "OnWeaponAmountChange")]
    internal class OnWeaponAmountChangePrefixPatch
    {
        private static bool Prefix(ref Agent __instance, ref EquipmentIndex slotIndex, ref short amount)
        {
            if (__instance == null)
            {
                return true;
            }
            float num = __instance.CombatEnhanceRate();
            if (num == 0f)
            {
                return true;
            }
            if (settings.ControlAmmoNoConsumeRate == 0)
            {
                return true;
            }
            MissionEquipment equipment = __instance.Equipment;
            short amount2 = equipment[slotIndex].Amount;
            if (amount2 > amount)
            {
                CharacterObject characterObject = __instance.Character as CharacterObject;
                Hero heroObject = characterObject.HeroObject;
                int num2 = (int)((float)(heroObject.GetAttributeValue(DefaultCharacterAttributes.Control) * settings.ControlAmmoNoConsumeRate) * num);
                if (MBRandom.RandomInt(100) >= num2)
                {
                    return true;
                }
                __instance.SetWeaponAmountInSlot(slotIndex, amount2, enforcePrimaryItem: true);
                return false;
            }
            return true;
        }
    }

    private static readonly InformationMessage critStrikeMsg = new InformationMessage(new TextObject("{=he_crit_strike_msg}CritStrike!").ToString(), Colors.Red);

    private static readonly InformationMessage exemptionMsg = new InformationMessage(new TextObject("{=he_exemption_msg}Exemption!").ToString(), Colors.Cyan);

    private static readonly InformationMessage beCritStrikeMsg = new InformationMessage(new TextObject("{=he_be_crit_strike_msg}Under CritStrike!").ToString(), Colors.Magenta);

    private static readonly InformationMessage beExemptedMsg = new InformationMessage(new TextObject("{=he_be_exempted_msg}Opponent exempt your damage!").ToString(), Colors.Magenta);

    private static readonly ISettings settings = GlobalSettings<Settings>.Instance;

    private static bool CalculateDamage(ref AttackInformation attackInformation, ref AttackCollisionData collisionData, ref MissionWeapon weapon, ref float baseDamage)
    {
        CharacterObject characterObject = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerRiderAgentCharacter : attackInformation.AttackerAgentCharacter) as CharacterObject;
        CharacterObject characterObject2 = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
        float num = characterObject.CombatEnhanceRate();
        float num2 = characterObject2.CombatEnhanceRate();
        if (num == 0f && num2 == 0f)
        {
            return true;
        }
        int num3 = 0;
        int num4 = 0;
        if (num > 0f)
        {
            Hero heroObject = characterObject.HeroObject;
            int attributeValue = heroObject.GetAttributeValue(DefaultCharacterAttributes.Vigor);
            num3 = heroObject.GetAttributeValue(DefaultCharacterAttributes.Control);
            baseDamage *= 1f + (float)attributeValue * settings.VigorDmgPercent * num;
            baseDamage += (float)attributeValue * settings.VigorFinalDmgAdd * num;
        }
        if (num2 > 0f)
        {
            Hero heroObject2 = characterObject2.HeroObject;
            num4 = heroObject2.GetAttributeValue(DefaultCharacterAttributes.Control);
            int attributeValue2 = heroObject2.GetAttributeValue(DefaultCharacterAttributes.Vigor);
            baseDamage -= (float)attributeValue2 * settings.VigorDmgTakenReduce * num2;
            if (collisionData.IsFallDamage)
            {
                baseDamage *= 1f - (float)num4 * settings.ControlDropDmgReducePercent * num2;
            }
        }
        int num5 = (int)((float)num3 * num - (float)num4 * num2);
        if (num5 > 0)
        {
            int num6 = num5 * settings.ControlCritRate;
            if (MBRandom.RandomInt(100) < num6)
            {
                baseDamage *= 2f;
                if (characterObject != null && characterObject.IsPlayerCharacter)
                {
                    InformationManager.DisplayMessage(critStrikeMsg);
                }
                else if (characterObject2 != null && characterObject2.IsPlayerCharacter)
                {
                    InformationManager.DisplayMessage(beCritStrikeMsg);
                }
            }
        }
        else if (num5 < 0)
        {
            int num7 = -num5 * settings.ControlExemptionRate;
            if (MBRandom.RandomInt(100) < num7)
            {
                baseDamage = 0f;
                if (characterObject != null && characterObject.IsPlayerCharacter)
                {
                    InformationManager.DisplayMessage(beExemptedMsg);
                }
                else if (characterObject2 != null && characterObject2.IsPlayerCharacter)
                {
                    InformationManager.DisplayMessage(exemptionMsg);
                }
            }
        }
        baseDamage = MathF.Max(0f, baseDamage);
        return true;
    }
}
