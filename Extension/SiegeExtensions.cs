using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
namespace UFO.Extension
{
	public static class SiegeExtensions
	{
		public static bool IsPlayerSide(this ISiegeEventSide side)
		{
			return side?.GetInvolvedPartiesForEventType()?.Any((PartyBase x) => x.IsPlayerParty()) == true;
		}
	}
}