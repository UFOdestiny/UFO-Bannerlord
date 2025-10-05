using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
namespace UFO.Extension
{
    public static class PartyExtensions
    {
        public static bool IsPlayerParty(this PartyBase party)
        {
            if (party == null)
            {
                return false;
            }
            if (party.MobileParty != null && party.MobileParty.BanditPartyComponent != null && party.MobileParty.BanditPartyComponent.PartyOwner == null)
            {
                return false;
            }
            if (party.Owner != null)
            {
                Hero hero = party?.Owner;
                if (party != null && party.MobileParty?.IsCaravan == true)
                {
                    return false;
                }
                return hero?.IsHumanPlayerCharacter ?? false;
            }
            return false;
        }

        public static bool IsPlayerParty(this MobileParty party)
        {
            return IsPlayerParty(party?.Party) == true;
        }
    }
}