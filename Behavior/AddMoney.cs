using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

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
        if (Hero.MainHero.Gold<10000000)
        {
            Hero.MainHero.ChangeHeroGold(gold);
        }

        //InformationManager.DisplayMessage(
        //    new InformationMessage($"You got {gold} gold!", new Color(1f, 0f, 0f, 1f))
        //);
    }

    public override void SyncData(IDataStore dataStore)
    {
    }
}
