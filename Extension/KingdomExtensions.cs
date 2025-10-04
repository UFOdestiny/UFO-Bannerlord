using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
namespace UFO.Extension
{
	public static class KingdomExtensions
	{
		public static bool IsPlayerKingdom(this Clan clan)
		{
			return clan?.Kingdom?.IsPlayerKingdom() == true;
		}

		public static bool IsPlayerKingdom(this Kingdom kingdom)
		{
			return kingdom?.Clans?.Any((Clan x) => x.IsPlayerClan()) == true;
		}

		public static bool IsPlayerKingdom(this PartyBase party)
		{
			return party?.Owner?.Clan?.IsPlayerKingdom() == true;
		}
	}
}