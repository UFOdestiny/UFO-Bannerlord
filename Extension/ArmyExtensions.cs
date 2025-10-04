using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace UFO.Extension
{

    public static class ArmyExtensions
    {
        public static bool IsPlayerArmy(this Army army)
        {
            if (army == null || army.Parties == null)
            {
                return false;
            }
            return army?.Parties?.Any((MobileParty x) => x.IsPlayerParty()) == true;
        }

        public static bool IsPlayerArmy(this MobileParty party)
        {
            if (party == null || party.Army == null)
            {
                return false;
            }
            return party?.Army?.IsPlayerArmy() == true;
        }

        public static bool IsOfPlayerKingdom(this Army army)
        {
            if (army == null || army.Kingdom == null)
            {
                return false;
            }
            return army?.Kingdom?.IsPlayerKingdom() == true;
        }
    }
}