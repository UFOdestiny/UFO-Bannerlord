using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using Shokuho.CustomCampaign.CustomLocations.models;
using Shokuho.ShokuhoCustomCampaign.Models;
using System;
using System.Collections.Generic;
using System.Runtime;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using UFO;


internal class CombatAttrEnhance_Shokuho
{
    private static readonly ISettings settings = GlobalSettings<Settings>.Instance;

    [HarmonyPatch(typeof(ShokuhoCustomAgentApplyDamageModel), "DecideCrushedThrough")]
    internal class DecideCrushedThroughPostfixPatch_c
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

    [HarmonyPatch(typeof(ShokuhoSandboxAgentApplyDamageModel), "DecideCrushedThrough")]
    internal class DecideCrushedThroughPostfixPatch_s
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

    [HarmonyPatch(typeof(Mission), "UpdateMomentumRemaining")]
    internal class WeaponMultipleCutThroughGetMomentumRemainingPatch
    {
        private static void Postfix(float __state, ref float momentumRemaining, Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
        {
            if (isCrushThrough || !collisionData.IsColliderAgent)
            {
                return;
            }
            int inflictedDamage = b.InflictedDamage;
            if (inflictedDamage <= 20)
            {
                momentumRemaining = 0f;
                return;
            }
            if (momentumRemaining <= 0f)
            {
                momentumRemaining = __state;
            }
            momentumRemaining *= ((inflictedDamage <= 50) ? 0.4f : 0.85f);
        }
    }



}
