using TaleWorlds.CampaignSystem;
namespace UFO.Extension
{
    public static class ExplainedNumberExtensions
    {
        public static void AddMultiplier(this ref ExplainedNumber explainedNumber, float multiplier)
        {
            explainedNumber.AddFactor(multiplier - 1f);
        }

        public static void AddPercentage(this ref ExplainedNumber explainedNumber, float percentage)
        {
            float value = (1f - percentage / 100f) * -1f;
            explainedNumber.AddFactor(value);
        }
    }
}