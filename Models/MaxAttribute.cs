using TaleWorlds.CampaignSystem.GameComponents;
using UFO.Setting;
using HarmonyLib;
namespace UFO.Model;

[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), nameof(DefaultCharacterDevelopmentModel.MaxAttribute), MethodType.Getter)]
internal static class MaxAttributePatch
{
    private static void Postfix(ref int __result) => __result = SettingsManager.MaxAttr.Value;
}
