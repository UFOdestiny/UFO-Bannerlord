using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using UFO.Behavior;
using UFO.Bootstrap;
using UFO.Diagnostics;
using UFO.Extension;
using UFO.Model;
using UFO.Patching;
using UFO.Setting;

namespace UFO;

internal class SubModule : MBSubModuleBase
{
    private bool PatchesApplied = false;

    protected override void OnSubModuleLoad()
    {
        base.OnSubModuleLoad();
    }

    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Loaded", Colors.Green));
        L10N.LoadLanguage();
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStart(game, gameStarterObject);

        if (game.GameType is Campaign)
        {
            CampaignStarterConfigurator.Configure(game, gameStarterObject);
        }
    }

    protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
    {
        base.InitializeGameStarter(game, starterObject);
        CampaignStarterConfigurator.ConfigureCampaignStarter(starterObject);
    }

    public override void OnGameInitializationFinished(Game game)
    {
        base.OnGameInitializationFinished(game);

        if (!(game.GameType is Campaign) || PatchesApplied)
        {
            return;
        }

        Harmony patcher = new Harmony("UFO");

        //UNPATCH(patcher);

        var failedPatches = PatchBootstrapper.Apply(patcher, typeof(SubModule).Assembly);
        NavalDlcCompatibility.Apply(patcher);
        PatchesApplied = true;
        if (failedPatches.Any())
            InformationManager.ShowInquiry(new InquiryData(L10N.GetText("ModFailedLoadWarningTitle"), L10N.GetTextFormat("ModFailedLoadWarningMessage", string.Join(Environment.NewLine, failedPatches)), true, false, L10N.GetText("ModWarningMessageConfirm"), null, null, null));

        //PatchInspector.PatchInformation();

        //InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Patch Applied", Colors.Green));
    }

    internal static void LogError(Exception e, Type type)
    {
        string text;
        try
        {
            text = ModDiagnostics.WriteError(e, type);
        }
        catch
        {
            return;
        }
        try
        {
            InformationManager.ShowInquiry(new InquiryData(L10N.GetText("ModExceptionTitle"), L10N.GetTextFormat("ModExceptionMessage", text), isAffirmativeOptionShown: true, isNegativeOptionShown: false, L10N.GetText("ModWarningMessageConfirm"), null, null, null));
        }
        catch
        {
            try
            {
                Message.Show(L10N.GetTextFormat("ModExceptionMessage", text), Colors.Red);
            }
            catch
            {
            }
        }
    }

}


