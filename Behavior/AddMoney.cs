using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using UFO.Setting;

namespace UFO.Behavior;

public class AddMoney : CampaignBehaviorBase
{
    private int gold = 100000;

    public override void RegisterEvents()
    {
        CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
    }

    private void DailyTick()
    {
        if (Hero.MainHero.Gold< SettingsManager.AddMoneyThreshhold.Value)
        {
            Hero.MainHero.ChangeHeroGold(SettingsManager.AddMoney_count.Value);
        }

        InformationManager.DisplayMessage(
            new InformationMessage($"+ {gold} gold!", Colors.White)
        );
    }

    public override void SyncData(IDataStore dataStore)
    {
    }
}
