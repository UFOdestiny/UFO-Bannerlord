using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using Shokuho.CustomCampaign.CustomLocations.models;
using Shokuho.ShokuhoCustomCampaign.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Shokuho.Combat;
internal class CombatAttrEnhance_Shokuho
{

    [HarmonyPatch]
    internal class DecideCrushedThroughPrefixPatch_c
    {
        private static bool Prepare()
        {
            return AccessTools.TypeByName("Shokuho.ShokuhoCustomCampaign.Models.ShokuhoCustomAgentApplyDamageModel") != null;
        }
        static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("Shokuho.ShokuhoCustomCampaign.Models.ShokuhoCustomAgentApplyDamageModel");
            return AccessTools.Method(type, "DecideCrushedThrough");
        }

        private static bool Prefix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
        {
            if (SettingsManager.TestMode.Value)
            {
                //return;
            }
            if (SettingsManager.PlayerAlwaysCrush.Value && attackerAgent.IsPlayerControlled)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch]
    internal class DecideCrushedThroughPrefixPatch_s
    {
        private static bool Prepare()
        {
            return AccessTools.TypeByName("Shokuho.CustomCampaign.CustomLocations.models.ShokuhoSandboxAgentApplyDamageModel") != null;
        }
        static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("Shokuho.CustomCampaign.CustomLocations.models.ShokuhoSandboxAgentApplyDamageModel");
            return AccessTools.Method(type, "DecideCrushedThrough");
        }
        private static bool Prefix(ref bool __result, Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
        {
            if (SettingsManager.TestMode.Value)
            {
                //return;
            }
            if (SettingsManager.PlayerAlwaysCrush.Value && attackerAgent.IsPlayerControlled)
            {
                __result = true;
                return false;
            }
            return true;
        }

    }

}
