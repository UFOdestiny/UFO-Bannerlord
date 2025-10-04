using TaleWorlds.CampaignSystem.Settlements;
namespace UFO.Extension
{
	public static class SettlementExtensions
	{
		public static bool IsPlayerTown(this Town town)
		{
			return town?.Owner?.Owner?.IsHumanPlayerCharacter == true;
		}

		public static bool IsPlayerVillage(this Village village)
		{
			return village?.Owner?.Owner?.IsHumanPlayerCharacter == true;
		}

		public static bool IsPlayerSettlement(this Settlement settlement)
		{
			return settlement?.Owner?.IsHumanPlayerCharacter == true;
		}
	}
}