using System.Linq;
using TaleWorlds.CampaignSystem;

namespace UFO.Extension
{
    public static class KingdomExtensions
    {

        public static bool IsPlayerKingdom(this Kingdom kingdom)
        {
            return kingdom?.Clans?.Any((Clan x) => x.IsPlayerClan()) == true;
        }
    }
}
