using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using System;
using System.Runtime;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Combat;


// CRUSH THROUGH EVERYONE
public class CrushThroughEveryoneLogic
{
    public static bool ShouldCrushThrough(Agent attackerAgent)
    {
        if ((attackerAgent.IsPlayer() && SettingsManager.PlayerAlwaysCrush.Value) || 
            (PlayerPartyCombatants.Contains(attackerAgent) && SettingsManager.PlayerPartyCrush.Value) ||
            (attackerAgent.IsPlayerEnemy() && SettingsManager.EnemyCrush.Value))
        {
            return true;
        }
        return false;
    }
}

[HarmonyPatch(typeof(CustomAgentApplyDamageModel), "DecideCrushedThrough")]
internal class DecideCrushedThroughPostfixPatch_c
{
    private static void Postfix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
    {
        if (defenderAgent == null)
        {
            return;
        }

        if (CrushThroughEveryoneLogic.ShouldCrushThrough(attackerAgent))
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
            if (MBRandom.RandomInt(100) < num5 * SettingsManager.VigorCrushThroughPositive.Value)
            {
                __result = true;
            }
        }
        else if (((num5 < 0) & __result) && MBRandom.RandomInt(100) < -num5 * SettingsManager.VigorCrushThroughNegative.Value)
        {
            __result = false;
        }
    }
}

[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "DecideCrushedThrough")]
internal class DecideCrushedThroughPostfixPatch_s
{
    private static void Postfix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
    {
        if (defenderAgent == null)
        {
            return;
        }

        if (CrushThroughEveryoneLogic.ShouldCrushThrough(attackerAgent))
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
            if (MBRandom.RandomInt(100) < num5 * SettingsManager.VigorCrushThroughPositive.Value)
            {
                __result = true;
            }
        }
        else if (((num5 < 0) & __result) && MBRandom.RandomInt(100) < -num5 * SettingsManager.VigorCrushThroughNegative.Value)
        {
            __result = false;
        }
    }
}



// CUT THROUGH EVERYONE
internal static class PlayerPartyCombatants
{
    internal static bool Contains(Agent agent)
    {
        if (agent?.Team == null || Mission.Current?.PlayerTeam == null || agent.Team != Mission.Current.PlayerTeam)
            return false;

        return agent.IsPlayer()
            || (agent.Origin != null
                && agent.Origin.TryGetParty(out var party)
                && party == MobileParty.MainParty?.Party);
    }
}

public static class CutThroughEveryoneLogic
{
    public static bool ShouldCutThrough(AttackCollisionData collisionData, Agent attacker, Agent victim)
    {
        return attacker != null
            && victim != null
            && collisionData.IsColliderAgent
            && attacker.WieldedWeapon.Item != null
            && ((attacker.IsPlayer() && SettingsManager.SliceThroughEveryone.Value)
                || (PlayerPartyCombatants.Contains(attacker) && SettingsManager.PlayerPartySliceThroughEveryone.Value)
                || (attacker.IsPlayerEnemy() && SettingsManager.SliceThroughEveryone_enemy.Value));
    }
}


[HarmonyPatch(typeof(MissionCombatMechanicsHelper))]
internal static class CutThroughEveryonePatchCollision
{
    [HarmonyPostfix]
    [HarmonyPatch("DecideWeaponCollisionReaction")]
    private static void Postfix(
        ref Blow registeredBlow,
        ref AttackCollisionData collisionData,
        Agent attacker, Agent defender,
        ref MissionWeapon attackerWeapon,
        bool isFatalHit, bool isShruggedOff, float momentumRemaining,
        ref MeleeCollisionReaction colReaction)
    {
        if (CutThroughEveryoneLogic.ShouldCutThrough(collisionData, attacker, defender))
        {
            colReaction = MeleeCollisionReaction.SlicedThrough;
        }
    }
}


[HarmonyPatch(typeof(Mission))]
internal static class CutThroughEveryonePatchMeleeHit
{
    [HarmonyPostfix]
    [HarmonyPatch("MeleeHitCallback")]
    private static void Postfix(ref AttackCollisionData collisionData,
        Agent attacker, Agent victim, GameEntity realHitEntity, ref float inOutMomentumRemaining,
        ref MeleeCollisionReaction colReaction, CrushThroughState crushThroughState,
        Vec3 blowDir, Vec3 swingDir, ref HitParticleResultData hitParticleResultData,
        bool crushedThroughWithoutAgentCollision)
    {
        int num = collisionData.InflictedDamage + collisionData.AbsorbedByArmor;
        if (num >= 1 && CutThroughEveryoneLogic.ShouldCutThrough(collisionData, attacker, victim))
        {
            float num2 = (float)collisionData.InflictedDamage / (float)num;
            inOutMomentumRemaining = 1f - (1f - num2) * SettingsManager.PlayerMomentumDecayMultiplier.Value;
        }
    }
}


[HarmonyPatch(typeof(AgentApplyDamageModel), "CalculateDefaultRemainingMomentum")]
internal class CalculateDefaultRemainingMomentumPatch
{
    private static void Postfix(ref float __result, float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
    {
        if (attacker?.IsPlayer() == true && victim != null && collisionData.IsColliderAgent)
        {
            float decayMultiplier = SettingsManager.PlayerMomentumDecayMultiplier.Value;
            __result = originalMomentum - (originalMomentum - __result) * decayMultiplier;
        }

        if (isCrushThrough && attacker.IsPlayer() && SettingsManager.PlayerAlwaysCrush.Value)
        {
            __result *= 2;

        }
    }
}



[HarmonyPatch(typeof(MissionCombatMechanicsHelper), "GetDefendCollisionResults")]
internal class MissionCombatMechanicsHelperGetDefendCollisionResultsPatch
{
    private static void Postfix(Agent attackerAgent, Agent defenderAgent,
        CombatCollisionResult collisionResult,
        int attackerWeaponSlotIndex, bool isAlternativeAttack,
        StrikeType strikeType, Agent.UsageDirection attackDirection,
        float collisionDistanceOnWeapon, float attackProgress,
        bool attackIsParried, bool isPassiveUsageHit, bool isHeavyAttack,
        ref float defenderStunPeriod, ref float attackerStunPeriod,
        ref bool crushedThrough, ref bool chamber)
    {
        if ((attackerAgent.IsPlayer() && SettingsManager.UnblockableThrust_player.Value) ||
            (attackerAgent.IsPlayerAlly() && SettingsManager.UnblockableThrust_ally.Value) ||
            (attackerAgent.IsPlayerEnemy() && SettingsManager.UnblockableThrust_enemy.Value))
        {

            if (strikeType == StrikeType.Thrust && collisionResult == CombatCollisionResult.Blocked && defenderAgent != null)
            {
                EquipmentIndex wieldedOffhandItemIndex = defenderAgent.GetOffhandWieldedItemIndex();
                if (wieldedOffhandItemIndex == EquipmentIndex.None || !defenderAgent.Equipment[wieldedOffhandItemIndex].CurrentUsageItem.IsShield)
                {
                    crushedThrough = true;
                }
            }
        }

    }
}
