using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace UFO.Extension
{
    public static class ClanExtensions
    {
        public static bool IsPlayerClan(this Clan clan)
        {
            if (clan == null || clan.Leader == null)
            {
                return false;
            }
            return clan?.Leader?.IsHumanPlayerCharacter == true;
        }

        public static bool IsPlayerClan(this PartyBase party)
        {
            if (party == null || party.Owner == null || party.Owner.Clan == null)
            {
                return false;
            }
            return IsPlayerClan(party?.Owner?.Clan) == true;
        }
    }
}