using HarmonyLib;
using MCM.Abstractions.Base.Global;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Combat;
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

        return attacker.IsPlayer();

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
            inOutMomentumRemaining = num2 ;
        }
    }
}
