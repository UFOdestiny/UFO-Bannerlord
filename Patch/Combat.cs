using HarmonyLib;
using JetBrains.Annotations;
using SandBox.GameComponents;
using StoryMode.GameComponents;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using UFO.Extension;
using UFO.Setting;
namespace UFO.Patch.Combat;


[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "DecideAgentKnockedDownByBlow")]
public static class AKD_S
{
    public static void Postfix(Agent attackerAgent, Agent victimAgent, AttackCollisionData collisionData, WeaponComponentData attackerWeapon, ref Blow blow)
    {
        try
        {
            if (attackerAgent.IsPlayer() && SettingsManager.AlwaysKnockDown.IsChanged)
            {
                blow.BlowFlag &= ~BlowFlags.ShrugOff;
                blow.BlowFlag |= BlowFlags.KnockDown;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(AKD_S));
        }
    }
}



[HarmonyPatch(typeof(DefaultCombatSimulationModel), "SimulateHit")]
public static class AWBS
{
    public static void Postfix(CharacterObject strikerTroop, CharacterObject struckTroop, PartyBase strikerParty, PartyBase struckParty, float strikerAdvantage, MapEvent battle, ref int __result)
    {
        try
        {
            if (struckParty.IsPlayerParty() && SettingsManager.AlwaysWinBattleSimulation.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(AWBS));
        }
    }
}



[HarmonyPatch(typeof(DefaultBanditDensityModel), "GetPlayerMaximumTroopCountForHideoutMission")]
public static class BHTL
{
    public static void Postfix(MobileParty party, ref int __result)
    {
        try
        {
            if (SettingsManager.BanditHideoutTroopLimit.IsChanged)
            {
                __result += SettingsManager.BanditHideoutTroopLimit.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(BHTL));
        }
    }
}


[HarmonyPatch(typeof(SandboxBattleMoraleModel), "CalculateCasualtiesFactor")]
public static class CCF
{
    public static void Postfix(BattleSideEnum battleSide, ref float __result)
    {
        try
        {
            if (SettingsManager.EnemiesNoRunningAway.IsChanged && Mission.Current.PlayerTeam.Side != battleSide)
            {
                __result = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CCF));
        }
    }
}



[HarmonyPatch(typeof(SandboxBattleMoraleModel), "CalculateMaxMoraleChangeDueToAgentIncapacitated")]
public static class CMMCDTAI
{
    public static bool Prefix(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow, ref (float, float) __result)
    {
        try
        {
            if (SettingsManager.EnemiesNoRunningAway.IsChanged && Mission.Current.PlayerTeam.Side != affectedAgent.Team.Side)
            {
                __result = (0f, 0f);
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CMMCDTAI));
            return true;
        }
    }
}


public static class CompanionsKnockoutOrKilled
{
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        try
        {
            if (effectedAgent.IsPlayerCompanion() && SettingsManager.CompanionsKnockoutOrKilled.IsChanged)
            {
                if (SettingsManager.CompanionsKnockoutOrKilled.Value == KnockoutOrKilled.Killed)
                {
                    __result = 1f;
                }
                else if (SettingsManager.CompanionsKnockoutOrKilled.Value == KnockoutOrKilled.Knockout)
                {
                    __result = 0f;
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CompanionsKnockoutOrKilled));
        }
    }
}

[HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class CompanionsKnockoutOrKilled_Default
{
    public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        CompanionsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


[HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class CompanionsKnockoutOrKilled_Sandbox
{
    public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        CompanionsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


[HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class CompanionsKnockoutOrKilled_StoryMode
{
    public static void Postfix(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        CompanionsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}

[HarmonyPatch(typeof(SandboxAgentStatCalculateModel), "GetMaxCameraZoom")]
public static class CZM
{
    public static void Postfix(Agent agent, ref float __result)
    {
        try
        {
            if (agent.IsPlayer() && SettingsManager.CombatZoomMultiplier.IsChanged)
            {
                __result *= SettingsManager.CombatZoomMultiplier.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CZM));
        }
    }
}



[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class DM_S
{
    public static void Postfix(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.IsAttackerPlayer && !attackInformation.IsFriendlyFire && SettingsManager.DamageMultiplier.IsChanged)
            {
                __result *= SettingsManager.DamageMultiplier.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DM_S));
        }
    }
}


[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class DTP_S
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.IsVictimPlayer && SettingsManager.DamageTakenPercentage.IsChanged)
            {
                float num = SettingsManager.DamageTakenPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * __result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DTP_S));
        }
    }
}


[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class EDP_S
{
    public static void Postfix(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.AttackerAgentOrigin != null && attackInformation.AttackerAgentOrigin.IsOnPlayerEnemySide() && SettingsManager.EnemyDamagePercentage.IsChanged)
            {
                float num = SettingsManager.EnemyDamagePercentage.Value / 100f;
                int num2 = (int)Math.Round(num * __result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EDP_S));
        }
    }
}


public static class EnemyLordsKnockoutOrKilled
{
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        try
        {
            if (effectedAgent.IsHero && effectedAgent.IsPlayerEnemy() && SettingsManager.EnemyLordsKnockoutOrKilled.IsChanged)
            {
                if (SettingsManager.EnemyLordsKnockoutOrKilled.Value == KnockoutOrKilled.Killed)
                {
                    __result = 1f;
                }
                else if (SettingsManager.EnemyLordsKnockoutOrKilled.Value == KnockoutOrKilled.Knockout)
                {
                    __result = 0f;
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnemyLordsKnockoutOrKilled));
        }
    }
}

[HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class EnemyLordsKnockoutOrKilled_Default
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        EnemyLordsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


[HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class EnemyLordsKnockoutOrKilled_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        EnemyLordsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}

[HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class EnemyLordsKnockoutOrKilled_StoryMode
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        EnemyLordsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


public static class EnemyTroopsKnockoutOrKilled
{
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        try
        {
            if (!effectedAgent.IsHero && effectedAgent.IsPlayerEnemy() && SettingsManager.EnemyTroopsKnockoutOrKilled.IsChanged)
            {
                if (SettingsManager.EnemyTroopsKnockoutOrKilled.Value == KnockoutOrKilled.Killed)
                {
                    __result = 1f;
                }
                else if (SettingsManager.EnemyTroopsKnockoutOrKilled.Value == KnockoutOrKilled.Knockout)
                {
                    __result = 0f;
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnemyTroopsKnockoutOrKilled));
        }
    }
}


[HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class EnemyTroopsKnockoutOrKilled_Default
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        EnemyTroopsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}

[HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class EnemyTroopsKnockoutOrKilled_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        EnemyTroopsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


[HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class EnemyTroopsKnockoutOrKilled_StoryMode
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        EnemyTroopsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}



[HarmonyPatch(typeof(SandboxBattleMoraleModel), "CalculateMoraleChangeToCharacter")]
public static class ENRA
{
    public static void Postfix(Agent agent, float maxMoraleChange, ref float __result)
    {
        try
        {
            if (agent.IsPlayerEnemy() && SettingsManager.EnemiesNoRunningAway.IsChanged)
            {
                __result = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ENRA));
        }
    }
}



[HarmonyPatch(typeof(SandboxBattleMoraleModel), "CanPanicDueToMorale")]
public static class ENRACP
{
    public static void Postfix(Agent agent, ref bool __result)
    {
        try
        {
            if (agent.IsPlayerEnemy() && SettingsManager.EnemiesNoRunningAway.IsChanged)
            {
                __result = false;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ENRACP));
        }
    }
}


public static class FriendlyLordsKnockoutOrKilled
{
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        try
        {
            if (effectedAgent.IsHero && effectedAgent.IsPlayerAlly() && effectedAgent.Origin.TryGetParty(out var party) && !party.IsPlayerParty() && SettingsManager.FriendlyLordsKnockoutOrKilled.IsChanged)
            {
                if (SettingsManager.FriendlyLordsKnockoutOrKilled.Value == KnockoutOrKilled.Killed)
                {
                    __result = 1f;
                }
                else if (SettingsManager.FriendlyLordsKnockoutOrKilled.Value == KnockoutOrKilled.Knockout)
                {
                    __result = 0f;
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FriendlyLordsKnockoutOrKilled));
        }
    }
}


[HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class FriendlyLordsKnockoutOrKilled_Default
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        FriendlyLordsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}

[HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class FriendlyLordsKnockoutOrKilled_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        FriendlyLordsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


[HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class FriendlyLordsKnockoutOrKilled_StoryMode
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        FriendlyLordsKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


[HarmonyPatch(typeof(TacticCoordinatedRetreat), "GetTacticWeight")]
public static class GTW
{
    public static bool Prefix(ref TacticCoordinatedRetreat __instance, ref float __result)
    {
        try
        {
            if (SettingsManager.EnemiesNoRunningAway.IsChanged && Mission.Current.PlayerTeam.Side != __instance.Team.Side)
            {
                __result = 0f;
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CMMCDTAI));
            return true;
        }
    }
}


public static class HealthRegeneration
{
    private static int? LastSet;

    public static void Handler()
    {
        try
        {
            if (Mission.Current == null || Agent.Main == null || MBCommon.IsPaused || !SettingsManager.HealthRegeneration.IsChanged)
            {
                return;
            }
            int second = DateTime.Now.Second;
            if (!LastSet.HasValue || second != LastSet)
            {
                LastSet = second;
                float health = Agent.Main.Health;
                float healthLimit = Agent.Main.HealthLimit;
                if (health < healthLimit)
                {
                    float num = SettingsManager.HealthRegeneration.Value / healthLimit * 100f;
                    float val = (float)Math.Round(health + num);
                    Agent.Main.Health = Math.Min(healthLimit, val);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(HealthRegeneration));
        }
    }
}


[HarmonyPatch(typeof(Mission), "OnAgentShootMissile")]
public static class InfiniteAmmo
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void OnAgentShootMissile(ref Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, bool isPrimaryWeaponShot, int forcedMissileIndex)
    {
        try
        {
            if (!shooterAgent.IsPlayer() || !SettingsManager.InfiniteAmmo.IsChanged)
            {
                return;
            }
            for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
            {
                MissionWeapon missionWeapon = shooterAgent.Equipment[equipmentIndex];
                if (missionWeapon.IsAnyConsumable() && missionWeapon.Amount <= missionWeapon.ModifiedMaxAmount)
                {
                    shooterAgent.SetWeaponAmountInSlot(equipmentIndex, missionWeapon.ModifiedMaxAmount, enforcePrimaryItem: true);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(InfiniteAmmo));
        }
    }
}



[HarmonyPatch(typeof(DefaultBattleRewardModel), "CalculateInfluenceGain")]
public static class InfluenceRewardMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateInfluenceGain(PartyBase party, float influenceValueOfBattle, float contributionShare, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.InfluenceRewardMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.InfluenceRewardMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(InfluenceRewardMultiplier));
        }
    }
}


[HarmonyPatch(typeof(SandboxAgentStatCalculateModel), "UpdateAgentStats")]
public static class InstantCrossbowReload
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static void UpdateAgentStats(Agent agent, ref AgentDrivenProperties agentDrivenProperties)
    {
        try
        {
            if (agent.IsPlayer() && SettingsManager.InstantCrossbowReload.IsChanged)
            {
                agentDrivenProperties.ReloadSpeed = 10f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(InstantCrossbowReload));
        }
    }
}



[HarmonyPatch(typeof(AgentStatCalculateModel), "SetAllWeaponInaccuracy")]
public static class InstantCrossbowReloadSpeed
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool SetAllWeaponInaccuracy(Agent agent, ref AgentDrivenProperties agentDrivenProperties, int equippedIndex, WeaponComponentData equippedWeaponComponent)
    {
        try
        {
            if (agent.IsPlayer() && SettingsManager.InstantCrossbowReload.IsChanged)
            {
                agentDrivenProperties.ReloadSpeed = 10f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(InstantCrossbowReload));
        }
        return true;
    }
}


[HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
public static class Invincible
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Invulnerable(ref Agent __instance, ref Agent.MortalityState __result)
    {
        try
        {
            if (__instance.IsPlayer() && SettingsManager.Invincible.IsChanged)
            {
                __result = Agent.MortalityState.Invulnerable;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(Invincible));
        }
    }
}



public static class NeverKnockedBackByAttacks
{
    public static void DecideAgentKnockedByBlow(Agent attackerAgent, Agent victimAgent, AttackCollisionData collisionData, WeaponComponentData attackerWeapon, ref Blow blow)
    {
        try
        {
            if (victimAgent.IsPlayer() && SettingsManager.NeverKnockedBackByAttacks.IsChanged)
            {
                blow.BlowFlag &= ~BlowFlags.KnockDown;
                blow.BlowFlag &= ~BlowFlags.KnockBack;
                blow.BlowFlag |= BlowFlags.ShrugOff;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NeverKnockedBackByAttacks));
        }
    }
}



[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "DecideAgentKnockedBackByBlow")]
public static class NeverKnockedBackByAttacks_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void DecideAgentKnockedByBlow(Agent attackerAgent, Agent victimAgent, AttackCollisionData collisionData, WeaponComponentData attackerWeapon, ref Blow blow)
    {
        NeverKnockedBackByAttacks.DecideAgentKnockedByBlow(attackerAgent, victimAgent, collisionData, attackerWeapon, ref blow);
    }
}

public static class NoFriendlyFire
{
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.AttackerAgentOrigin.TryGetParty(out var party) && party.IsPlayerParty() && attackInformation.IsFriendlyFire && SettingsManager.NoFriendlyFire.IsChanged)
            {
                __result = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoFriendlyFire));
        }
    }
}


[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class NoFriendlyFire_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        NoFriendlyFire.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}



[HarmonyPatch(typeof(SandboxBattleMoraleModel), "CalculateMoraleChangeToCharacter")]
public static class NoRunningAway
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange, ref float __result)
    {
        try
        {
            if (agent.Origin.TryGetParty(out var party) && party.IsPlayerParty() && SettingsManager.NoRunningAway.IsChanged)
            {
                __result = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoRunningAway));
        }
    }
}


[HarmonyPatch(typeof(Mission), "HandleMissileCollisionReaction")]
public static class NoStuckArrows
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static void HandleMissileCollisionReaction(int missileIndex, ref Mission.MissileCollisionReaction collisionReaction, MatrixFrame attachLocalFrame, Agent attackerAgent, Agent attachedAgent, bool attachedToShield, sbyte attachedBoneIndex, MissionObject attachedMissionObject, Vec3 bounceBackVelocity, Vec3 bounceBackAngularVelocity, int forcedSpawnIndex)
    {
        try
        {
            if (attachedAgent.IsPlayer() && collisionReaction == Mission.MissileCollisionReaction.Stick && SettingsManager.NoStuckArrows.IsChanged)
            {
                collisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoStuckArrows));
        }
    }
}


[HarmonyPatch(typeof(DefaultTroopSacrificeModel), "GetLostTroopCountForBreakingInBesiegedSettlement")]
public static class NoTroopSacrificeBreakIn
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, ref int __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.NoTroopSacrifice.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoTroopSacrificeBreakIn));
        }
    }
}


[HarmonyPatch(typeof(DefaultTroopSacrificeModel), "GetLostTroopCountForBreakingOutOfBesiegedSettlement")]
public static class NoTroopSacrificeBreakOut
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, ref int __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.NoTroopSacrifice.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoTroopSacrificeBreakOut));
        }
    }
}


[HarmonyPatch(typeof(DefaultTroopSacrificeModel), "GetNumberOfTroopsSacrificedForTryingToGetAway")]
public static class NoTroopSacrificeRunaway
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum battleSide, MapEvent mapEvent, ref int __result)
    {
        try
        {
            if (mapEvent.IsPlayerMapEvent && battleSide == mapEvent.PlayerSide && SettingsManager.NoTroopSacrifice.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoTroopSacrificeRunaway));
        }
    }
}


public static class OneHitKill
{
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.IsAttackerPlayer && !attackInformation.IsFriendlyFire && SettingsManager.OneHitKill.IsChanged)
            {
                __result = 10000f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(OneHitKill));
        }
    }
}

[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class OneHitKill_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        OneHitKill.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}


public static class PartyDamageMultiplier
{
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.AttackerAgentOrigin.TryGetParty(out var party) && party.IsPlayerParty() && !attackInformation.IsAttackerPlayer && !attackInformation.IsFriendlyFire && SettingsManager.PartyDamageMultiplier.IsChanged)
            {
                __result *= SettingsManager.PartyDamageMultiplier.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyDamageMultiplier));
        }
    }
}



[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class PartyDamageMultiplier_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        PartyDamageMultiplier.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}


public static class PartyDamageTakenPercentage
{
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.VictimAgentOrigin.TryGetParty(out var party) && party.IsPlayerParty() && !attackInformation.IsVictimPlayer && SettingsManager.PartyDamageTakenPercentage.IsChanged)
            {
                float num = SettingsManager.PartyDamageTakenPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * __result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyDamageTakenPercentage));
        }
    }
}


public static class PartyDamageTakenPercentage_Default
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        PartyDamageTakenPercentage.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}


public static class PartyDamageTakenPercentage_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        PartyDamageTakenPercentage.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}


public static class PartyHealthRegeneration
{
    private static int? LastSet;

    public static void Handler()
    {
        try
        {
            if (Mission.Current == null || Mission.Current.PlayerTeam == null || MBCommon.IsPaused || !SettingsManager.PartyHealthRegeneration.IsChanged)
            {
                return;
            }
            int second = DateTime.Now.Second;
            if (LastSet.HasValue && second == LastSet)
            {
                return;
            }
            LastSet = second;
            PartyBase party;
            Agent[] array = Mission.Current.PlayerTeam.ActiveAgents.Where((Agent x) => x.Health > 0f && !x.IsPlayer() && x.Origin.TryGetParty(out party) && party.IsPlayerParty()).ToArray();
            Agent[] array2 = array;
            foreach (Agent agent in array2)
            {
                float health = agent.Health;
                float healthLimit = agent.HealthLimit;
                if (health < healthLimit)
                {
                    float num2 = SettingsManager.PartyHealthRegeneration.Value / healthLimit * 100f;
                    float val = (float)Math.Round(health + num2);
                    agent.Health = Math.Min(healthLimit, val);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyHealthRegeneration));
        }
    }
}


[HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
public static class PartyHeroesInvincible
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Invulnerable(ref Agent __instance, ref Agent.MortalityState __result)
    {
        try
        {
            if (__instance.TryGetHuman(out var character) && !character.IsPlayer() && character.Origin.TryGetParty(out var party) && party.IsPlayerParty() && character.IsHero() && SettingsManager.PartyHeroesInvincible.IsChanged)
            {
                __result = Agent.MortalityState.Invulnerable;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyHeroesInvincible));
        }
    }
}


[HarmonyPatch(typeof(Mission), "OnAgentShootMissile")]
public static class PartyInfiniteAmmo
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void OnAgentShootMissile(ref Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, bool isPrimaryWeaponShot, int forcedMissileIndex)
    {
        try
        {
            if (shooterAgent.IsPlayer() || !shooterAgent.Origin.TryGetParty(out var party) || !party.IsPlayerParty() || !SettingsManager.PartyInfiniteAmmo.IsChanged)
            {
                return;
            }
            for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
            {
                MissionWeapon missionWeapon = shooterAgent.Equipment[equipmentIndex];
                if (missionWeapon.IsAnyConsumable() && missionWeapon.Amount <= missionWeapon.ModifiedMaxAmount)
                {
                    shooterAgent.SetWeaponAmountInSlot(equipmentIndex, missionWeapon.ModifiedMaxAmount, enforcePrimaryItem: true);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyInfiniteAmmo));
        }
    }
}


[HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
public static class PartyInvincible
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Invulnerable(ref Agent __instance, ref Agent.MortalityState __result)
    {
        try
        {
            if (__instance.TryGetHuman(out var character) && character.Origin.TryGetParty(out var party) && party.IsPlayerParty() && !character.IsHero() && SettingsManager.PartyInvincible.IsChanged)
            {
                __result = Agent.MortalityState.Invulnerable;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyInvincible));
        }
    }
}


public static class PartyKnockoutOrKilled
{
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        try
        {
            if (effectedAgent.Origin.TryGetParty(out var party) && party.IsPlayerParty() && !effectedAgent.IsPlayer() && !effectedAgent.IsHero && SettingsManager.PartyKnockoutOrKilled.IsChanged)
            {
                if (SettingsManager.PartyKnockoutOrKilled.Value == KnockoutOrKilled.Killed)
                {
                    __result = 1f;
                }
                else if (SettingsManager.PartyKnockoutOrKilled.Value == KnockoutOrKilled.Knockout)
                {
                    __result = 0f;
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyKnockoutOrKilled));
        }
    }
}


[HarmonyPatch(typeof(DefaultAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class PartyKnockoutOrKilled_Default
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        PartyKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}

[HarmonyPatch(typeof(SandboxAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class PartyKnockoutOrKilled_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        PartyKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}



[HarmonyPatch(typeof(StoryModeAgentDecideKilledOrUnconsciousModel), "GetAgentStateProbability")]
public static class PartyKnockoutOrKilled_StoryMode
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, float useSurgeryProbability, ref float __result)
    {
        PartyKnockoutOrKilled.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, useSurgeryProbability, ref __result);
    }
}


public static class PartyOneHitKill
{
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        try
        {
            if (attackInformation.AttackerAgentOrigin.TryGetParty(out var party) && party.IsPlayerParty() && !attackInformation.IsAttackerPlayer && !attackInformation.IsFriendlyFire && SettingsManager.PartyOneHitKill.IsChanged)
            {
                __result = 10000f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyOneHitKill));
        }
    }
}


[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "CalculateDamage")]
public static class PartyOneHitKill_Sandbox
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDamage(AttackInformation attackInformation, AttackCollisionData collisionData, WeaponComponentData weapon, ref float __result)
    {
        PartyOneHitKill.CalculateDamage(attackInformation, collisionData, weapon, ref __result);
    }
}



[HarmonyPatch(typeof(Agent), "CurrentMortalityState", MethodType.Getter)]
public static class PlayerHorseInvincible
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Invulnerable(ref Agent __instance, ref Agent.MortalityState __result)
    {
        try
        {
            if (__instance.IsMount && __instance.TryGetHuman(out var character) && character.IsPlayer() && SettingsManager.PlayerHorseInvincible.IsChanged)
            {
                __result = Agent.MortalityState.Invulnerable;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PlayerHorseInvincible));
        }
    }
}


[HarmonyPatch(typeof(DefaultBattleRewardModel), "CalculateRenownGain")]
public static class RenownRewardMultiplierBattle
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateRenownGain(PartyBase party, float renownValueOfBattle, float contributionShare, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.RenownRewardMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.RenownRewardMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(RenownRewardMultiplierBattle));
        }
    }
}



[HarmonyPatch(typeof(DefaultTournamentModel), "GetRenownReward")]
public static class RenownRewardMultiplierTournament
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetRenownReward(Hero winner, Town town, ref int __result)
    {
        try
        {
            if (winner.IsPlayer() && SettingsManager.RenownRewardMultiplier.IsChanged)
            {
                __result = (int)Math.Round((float)__result * SettingsManager.RenownRewardMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(RenownRewardMultiplierTournament));
        }
    }
}

//public static class SliceThroughEveryonePassive
//{
//    public static void DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit, ref MeleeCollisionReaction __result)
//    {
//        try
//        {
//            if (attacker.IsPlayer() && SettingsManager.SliceThroughEveryone.IsChanged)
//            {
//                __result = MeleeCollisionReaction.SlicedThrough;
//            }
//        }
//        catch (Exception e)
//        {
//            SubModule.LogError(e, typeof(SliceThroughEveryonePassive));
//        }
//    }
//}


//[HarmonyPatch(typeof(SandboxAgentApplyDamageModel), "DecidePassiveAttackCollisionReaction")]
//public static class SliceThroughEveryonePassive_Sandbox
//{
//    [UsedImplicitly]
//    [HarmonyPostfix]
//    public static void DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit, ref MeleeCollisionReaction __result)
//    {
//        SliceThroughEveryonePassive.DecidePassiveAttackCollisionReaction(attacker, defender, isFatalHit, ref __result);
//    }
//}



//[HarmonyPatch(typeof(Mission), "DecideWeaponCollisionReaction")]
//public static class SliceThroughEveryoneWeapon
//{
//    [UsedImplicitly]
//    [HarmonyPostfix]
//    public static void DecideWeaponCollisionReaction(Blow registeredBlow, AttackCollisionData collisionData, Agent attacker, Agent defender, MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, ref MeleeCollisionReaction colReaction)
//    {
//        try
//        {
//            if (attacker.IsPlayer() && SettingsManager.SliceThroughEveryone.IsChanged)
//            {
//                colReaction = MeleeCollisionReaction.SlicedThrough;
//            }
//        }
//        catch (Exception e)
//        {
//            SubModule.LogError(e, typeof(SliceThroughEveryoneWeapon));
//        }
//    }
//}


//[HarmonyPatch(typeof(Mission), "UpdateMomentumRemaining")]
//public static class SliceThroughEveryoneWeaponMomentum
//{
//    [UsedImplicitly]
//    [HarmonyPrefix]
//    public static bool UpdateMomentumRemaining(float momentumRemaining, Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
//    {
//        if (attacker.IsPlayer() && SettingsManager.SliceThroughEveryone.IsChanged)
//        {
//            return false;
//        }
//        return true;
//    }
//}
