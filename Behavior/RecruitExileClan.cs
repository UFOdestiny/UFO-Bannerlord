using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
namespace UFO.Behavior;

internal class RecruitExileClan : CampaignBehaviorBase
{
    public override void SyncData(IDataStore dataStore)
    {
    }

    public override void RegisterEvents()
    {
        CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, JoinChat);
    }

    private void JoinChat(CampaignGameStarter starter)
    {
        string JoinMyKindom = L10N.GetText("JoinMyKindom");
        string OK = L10N.GetText("JoinMyKindom_OK");
        starter.AddPlayerLine("Dialog_Join1", "hero_main_options", "Dialog_Join", JoinMyKindom, checkcondition, null);
        starter.AddDialogLine("Dialog_Join2", "Dialog_Join", "close_window", OK, null, joinkingdom);

        //starter.AddPlayerLine("Dialog_Join1", "hero_main_options", "Dialog_Join1", "{=Dialog_Join1}Join my Kingdom,It's the only way for your clan to survive.", checkcondition, null);
        //starter.AddDialogLine("Dialog_Join2", "Dialog_Join1", "close_window", "{=Dialog_Join2}OK.", null, joinkingdom);
    }

    private bool checkcondition()
    {
        Clan clan = Hero.OneToOneConversationHero.Clan;
        if (clan == null)
        {
            return false;
        }
        bool flag = clan.Kingdom == null;
        Hero mainHero = Hero.MainHero;
        Clan clan2 = mainHero.Clan;
        bool flag2 = clan2.Kingdom != null;
        bool isClanLeader = Hero.OneToOneConversationHero.IsClanLeader;
        bool flag3 = clan.IsRebelClan || clan.IsClanTypeMercenary;
        if (flag && flag2 && isClanLeader && !flag3)
        {
            return true;
        }
        return false;
    }

    private void joinkingdom()
    {
        Hero mainHero = Hero.MainHero;
        Kingdom kingdom = mainHero.Clan?.Kingdom;
        Kingdom newKingdom = kingdom;
        Clan clan = Hero.OneToOneConversationHero.Clan;
        ChangeKingdomAction.ApplyByJoinToKingdom(clan, newKingdom);
        ChangeRelationAction.ApplyPlayerRelation(Hero.OneToOneConversationHero, 30);
    }
}
