using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace UFO
{
    public class AddMoney : CampaignBehaviorBase
    {
        private int gold = 0;

        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        private void DailyTick()
        {
            gold += 1;
            Hero.MainHero.ChangeHeroGold(gold);
            InformationManager.DisplayMessage(
                new InformationMessage($"You got {gold} gold!", new Color(1f, 0f, 0f, 1f))
            );
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("gold", ref gold);
        }
    }
}
