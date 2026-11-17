using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using UFO.Patch.Combat;

namespace UFO.Patch;

public static class ILInjection
{

    public static void EnableHotkeysEncyclopedia(EncyclopediaPageVM __instance)
    {
        EnableHotkeysAddEncyclopediaTroops.Handler(__instance);

        EnableHotkeysKillCharacter.Handler(__instance);
        EnableHotkeysTransferSettlement.Handler(__instance);
    }

    public static void EnableHotkeysGameManagerBase()
    {
        EnableHotkeysAddItems.Handler();
        EnableHotkeysCharacterAttributes.Handler();
        EnableHotkeysCharacterPoints.Handler();
        EnableHotkeysInfluence.Handler();
        EnableHotkeysMoney.Handler();
        EnableHotkeysTroopCount.Handler();
        EnableHotkeysTroopExperience.Handler();
        HealthRegeneration.Handler();
        PartyHealthRegeneration.Handler();
    }

    public static float CalculatePercentageFactor(ExplainedNumber exp, float Percentage)
    {
        float num = exp.ResultNumber * (Percentage / 100f);
        float num2 = exp.SumOfFactors;
        float num3 = (num * 100f - exp.BaseNumber * 100f) / exp.BaseNumber;
        return num3 / 100f - num2;
    }
}
