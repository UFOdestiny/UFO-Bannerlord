using HarmonyLib;
using MCM.Abstractions.Base.Global;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;

namespace UFO
{
    internal class SubModule : MBSubModuleBase
    {
        private ISettings _settings;

        private bool PatchesApplied = false;

        public override void OnGameInitializationFinished(Game game)
        {
            if (game.GameType is Campaign && !PatchesApplied)
            {
                PatchesApplied = true;
                base.OnGameInitializationFinished(game);
                new Harmony("UFO").PatchAll();
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            _settings = GlobalSettings<Settings>.Instance;
            InformationManager.DisplayMessage(new InformationMessage("UFO's Mod loaded", Colors.Green));
        }
    }

}
