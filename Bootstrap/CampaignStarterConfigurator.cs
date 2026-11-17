using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using UFO.Behavior;

namespace UFO.Bootstrap;

internal static class CampaignStarterConfigurator
{
    internal static void Configure(Game game, IGameStarter starter)
    {
        if (game.GameType is not Campaign || starter is not CampaignGameStarter campaignStarter)
            return;

        campaignStarter.AddBehavior(new SavingWeaponProperties.CustomBehavior());
        campaignStarter.AddBehavior(new AddMoney());
    }

    internal static void ConfigureCampaignStarter(IGameStarter starter)
    {
        if (starter is CampaignGameStarter campaignStarter)
            campaignStarter.AddBehavior(new RecruitExileClan());
    }
}
