using HarmonyLib;
using System;

using System.Linq;

using TaleWorlds.CampaignSystem;

using TaleWorlds.Core;
using TaleWorlds.Library;

using TaleWorlds.MountAndBlade;

using UFO.Bootstrap;
using UFO.Diagnostics;
using UFO.Extension;

using UFO.Patching;

namespace UFO;

internal class SubModule : MBSubModuleBase
{
    private bool PatchesApplied = false;
    private bool EarlyPatchesApplied;

    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        ModDiagnostics.InstallRuntimeTrace();
        L10N.LoadLanguage();
        InformationManager.DisplayMessage(new InformationMessage(L10N.GetText("ModLoaded"), Colors.Green));
        ApplyEarlyPatches();
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStart(game, gameStarterObject);
        ModDiagnostics.BeginGameLoad();
        ApplyEarlyPatches();

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

        ApplyPatches();
        ModDiagnostics.CompleteGameLoad();
    }

    public override void OnGameEnd(Game game)
    {
        // A second load in the same process must also deserialize without gameplay patches.
        new Harmony("UFO").UnpatchAll("UFO");
        new Harmony("UFO.Bootstrap").UnpatchAll("UFO.Bootstrap");
        PatchesApplied = false;
        EarlyPatchesApplied = false;
        ModDiagnostics.CompleteGameLoad();
        base.OnGameEnd(game);
    }

    private void ApplyEarlyPatches()
    {
        if (EarlyPatchesApplied) return;
        var failures = PatchBootstrapper.Apply(new Harmony("UFO.Bootstrap"), typeof(SubModule).Assembly, earlyOnly: true);
        EarlyPatchesApplied = failures.Count == 0;
        ModDiagnostics.TraceRuntime("Bootstrap patches installed; failures=" + failures.Count);
    }

    private void ApplyPatches()
    {

        if (PatchesApplied)
        {
            return;
        }

        Harmony patcher = new Harmony("UFO");

        ApplyEarlyPatches();
        var failedPatches = PatchBootstrapper.Apply(patcher, typeof(SubModule).Assembly, earlyOnly: false)
            .Concat(NavalDlcCompatibility.Apply(patcher)).ToArray();
        PatchesApplied = true;
        ModDiagnostics.TraceRuntime("Gameplay patches installed after initialization; failures=" + failedPatches.Length);
        if (failedPatches.Any())
            InformationManager.ShowInquiry(new InquiryData(L10N.GetText("ModFailedLoadWarningTitle"), L10N.GetTextFormat("ModFailedLoadWarningMessage", string.Join(Environment.NewLine, failedPatches)), true, false, L10N.GetText("ModWarningMessageConfirm"), null, null, null));

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

