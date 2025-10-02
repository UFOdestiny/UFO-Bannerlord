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

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            Harmony patcher = new("UFO");
            patcher.Unpatch(AccessTools.Method(typeof(Mission), "UpdateMomentumRemaining"), HarmonyPatchType.Postfix, "mod.bannerlord.shokuho");
            patcher.PatchAll();
            InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Patch Applied", Colors.Green));

        }

        //protected override void OnGameStart(Game game, IGameStarter starter)
        //{
        //    base.OnGameStart(game, starter);
        //    PatchInspector.DumpMissionUpdateMomentumRemainingPatches();
        //}

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            _settings = GlobalSettings<Settings>.Instance;
            InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Loaded", Colors.Green));
        }
    }

}
