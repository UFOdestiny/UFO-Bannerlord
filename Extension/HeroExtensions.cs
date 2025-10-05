using TaleWorlds.CampaignSystem;
namespace UFO.Extension
{
    public static class HeroExtensions
    {
        public static bool IsPlayer(this Hero hero)
        {
            return hero?.IsHumanPlayerCharacter ?? false;
        }

        public static bool IsPlayerCompanion(this Hero hero)
        {
            return hero?.IsPlayerCompanion ?? false;
        }

        public static bool IsPlayerClan(this Hero hero)
        {
            return hero?.Clan?.IsPlayerClan() == true;
        }
    }
}