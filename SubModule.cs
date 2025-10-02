using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace UFO
{
    internal class SubModule : MBSubModuleBase
    {
        private ISettings _settings;

        private bool PatchesApplied = false;

        public override void OnGameInitializationFinished(Game game)
        {
            Harmony patcher = new("UFO");
            
            if (!PatchesApplied)
            {
                PatchesApplied = true;
                base.OnGameInitializationFinished(game);
                patcher.Unpatch(AccessTools.Method(typeof(Mission), "UpdateMomentumRemaining"), HarmonyPatchType.Postfix, "mod.bannerlord.shokuho");
                patcher.PatchAll();

                //PatchInspector.DumpMissionUpdateMomentumRemainingPatches();
            }

            //PatchInspector.DumpMissionUpdateMomentumRemainingPatches();
            //InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Patch Applied", Colors.Green));

        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            _settings = GlobalSettings<Settings>.Instance;
            InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Loaded", Colors.Green));
        }
    }

}
