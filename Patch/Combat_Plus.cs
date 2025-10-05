using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Combat;


// CRUSH THROUGH EVERYONE

[HarmonyPatch(typeof(CustomAgentApplyDamageModel), "DecideCrushedThrough")]
internal class DecideCrushedThroughPostfixPatch_c
{
    private static void Postfix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
    {
        if (SettingsManager.TestMode.Value)
        {
            return;
        }
        if (SettingsManager.PlayerAlwaysCrush.Value && attackerAgent.IsPlayer())
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
        if (SettingsManager.TestMode.Value)
        {
            return;
        }
        if (SettingsManager.PlayerAlwaysCrush.Value && attackerAgent.IsPlayer())
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
public class CutThroughEveryoneLogic : MissionLogic
{
    private struct SliceMetadatum
    {
        public HashSet<Agent.UsageDirection> SliceDirections;
    }

    private static readonly SliceMetadatum BladeSliceMetadatum = new SliceMetadatum
    {
        SliceDirections = new HashSet<Agent.UsageDirection>
        {
            Agent.UsageDirection.AttackUp,
            Agent.UsageDirection.AttackDown,
            Agent.UsageDirection.AttackLeft,
            Agent.UsageDirection.AttackRight
        }
    };

    private static readonly SliceMetadatum PolearmSliceMetadatum = new SliceMetadatum
    {
        SliceDirections = new HashSet<Agent.UsageDirection>
        {
            Agent.UsageDirection.AttackUp,
            Agent.UsageDirection.AttackDown,
            Agent.UsageDirection.AttackLeft,
            Agent.UsageDirection.AttackRight
        }
    };

    private static readonly SliceMetadatum AxeSliceMetadatum = new SliceMetadatum
    {
        SliceDirections = new HashSet<Agent.UsageDirection>
        {
            Agent.UsageDirection.AttackLeft,
            Agent.UsageDirection.AttackRight
        }
    };

    private static readonly IReadOnlyDictionary<WeaponClass, SliceMetadatum> WeaponClassSliceMetadata = new Dictionary<WeaponClass, SliceMetadatum>
    {
        [WeaponClass.Dagger] = BladeSliceMetadatum,
        [WeaponClass.OneHandedSword] = BladeSliceMetadatum,
        [WeaponClass.TwoHandedSword] = BladeSliceMetadatum,
        [WeaponClass.OneHandedAxe] = AxeSliceMetadatum,
        [WeaponClass.TwoHandedAxe] = AxeSliceMetadatum,
        [WeaponClass.LowGripPolearm] = PolearmSliceMetadatum,
        [WeaponClass.OneHandedPolearm] = PolearmSliceMetadatum,
        [WeaponClass.TwoHandedPolearm] = PolearmSliceMetadatum
    };

    public static bool ShouldCutThrough(AttackCollisionData collisionData, Agent attacker, Agent victim)
    {

        return attacker.IsPlayer() && SettingsManager.PlayerAlwaysCrush.Value;

    }

    private static bool DoPreflightChecksPass(AttackCollisionData collisionData, Agent attacker, Agent victim)
    {
        bool result = false;
        if (victim != null && attacker != null && attacker.WieldedWeapon.Item != null && ((int)victim.Health == 0) && (attacker.Team != victim.Team) && (attacker.IsMainAgent))
        {
            result = true;
        }
        return result;
    }
}


[HarmonyPatch(typeof(Mission))]
internal static class CutThroughEveryonePatch
{
    private static MeleeCollisionReaction meleeCollisionReaction;

    [HarmonyPostfix]
    [HarmonyPatch("DecideWeaponCollisionReaction")]
    private static void DecideWeaponCollisionReactionPostfix(Mission __instance, Blow registeredBlow, ref AttackCollisionData collisionData, Agent attacker, Agent defender, bool isFatalHit, bool isShruggedOff, ref MeleeCollisionReaction colReaction)
    {
        meleeCollisionReaction = colReaction;
        if (CutThroughEveryoneLogic.ShouldCutThrough(collisionData, attacker, defender))
        {
            colReaction = MeleeCollisionReaction.SlicedThrough;
            meleeCollisionReaction = MeleeCollisionReaction.SlicedThrough;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("MeleeHitCallback")]
    private static void MeleeHitCallbackPostfix(ref AttackCollisionData collisionData, Agent attacker, Agent victim, GameEntity realHitEntity, ref float inOutMomentumRemaining, ref MeleeCollisionReaction colReaction, CrushThroughState crushThroughState, Vec3 blowDir, Vec3 swingDir, ref HitParticleResultData hitParticleResultData, bool crushedThroughWithoutAgentCollision)
    {
        if (meleeCollisionReaction != colReaction && meleeCollisionReaction == MeleeCollisionReaction.SlicedThrough)
        {
            colReaction = MeleeCollisionReaction.SlicedThrough;
        }
        int num = collisionData.InflictedDamage + collisionData.AbsorbedByArmor;
        if (num >= 1 && CutThroughEveryoneLogic.ShouldCutThrough(collisionData, attacker, victim))
        {
            float num2 = (float)collisionData.InflictedDamage / (float)num;
            inOutMomentumRemaining = num2;
        }
    }
}
