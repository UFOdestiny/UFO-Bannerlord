using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using UFO.Extension;
using UFO.Setting;

internal class MiscPatchs
{
    [HarmonyPatch(typeof(Location), "GetLocationCharacter", new Type[] { typeof(Hero) })]
    internal class GetLocationCharacterPrefixPatch
    {
        private static bool Prefix(ref Location __instance, ref LocationCharacter __result, Hero hero)
        {
            if (!SettingsManager.EnableEverYoung.Value)
            {
                return true;
            }
            List<LocationCharacter> value = Traverse.Create(__instance).Field("_characterList").GetValue<List<LocationCharacter>>();
            if (value == null)
            {
                __result = null;
                return false;
            }
            __result = value.Find((LocationCharacter x) => x.Character == hero.CharacterObject);
            return false;
        }
    }

    [HarmonyPatch(typeof(Location), "CanAIExit")]
    internal class CanAIExitPrefixPatch
    {
        private static bool Prefix(ref Location __instance, ref bool __result, LocationCharacter character)
        {
            if (!SettingsManager.EnableEverYoung.Value)
            {
                return true;
            }
            if (__instance == null)
            {
                __result = false;
                return false;
            }
            string value = Traverse.Create(__instance).Field("_aiCanExit").GetValue<string>();
            if (value == null)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(EncyclopediaHeroPageVM), "Refresh")]
    internal class EncyclopediaHeroPageVMPostfixPatch
    {
        private static void Postfix(EncyclopediaHeroPageVM __instance)
        {
            if (SettingsManager.EnableEverYoung.Value)
            {
                int index = 0;
                Hero value = Traverse.Create(__instance).Field("_hero").GetValue<Hero>();
                if (value.Culture != null)
                {
                    index = 1;
                }
                __instance.Stats[index].Value = ((int)value.TrueAge()).ToString();
            }
        }
    }

    [HarmonyPatch(typeof(Hero), "SetBirthDay")]
    internal class SetBirthDayPrefixPatch
    {
        private static bool Prefix(ref Hero __instance, CampaignTime birthday)
        {
            if (!SettingsManager.EnableEverYoung.Value)
            {
                return true;
            }
            int num = (int)birthday.ElapsedYearsUntilNow;
            if (num < 18)
            {
                return true;
            }
            if (__instance.TrueAge() != __instance.Age)
            {
                if (__instance.GetSkillValue(DefaultSkills.Athletics) == 0)
                {
                    return false;
                }
                if (num == (int)__instance.Age)
                {
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Hero), "Age", MethodType.Getter)]
    internal class AgePrefixPatch
    {
        private static bool Prefix(ref Hero __instance, ref float __result)
        {
            if (!SettingsManager.EnableEverYoung.Value)
            {
                return true;
            }
            if (!__instance.IsAlive)
            {
                __result = (float)(__instance.DeathDay - __instance.BirthDay).ToYears;
            }
            else
            {
                int num = __instance.AgeScale();
                float elapsedYearsUntilNow = __instance.BirthDay.ElapsedYearsUntilNow;
                if (elapsedYearsUntilNow <= 22f)
                {
                    __result = elapsedYearsUntilNow;
                }
                else if (num < 0)
                {
                    __result = 22f;
                }
                else
                {
                    __result = (22f * (float)(num - 1) + elapsedYearsUntilNow) / (float)num;
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ConversationHelper), "GetHeroRelationToHeroTextShort")]
    internal class GetHeroRelationToHeroTextShortPostfixPatch
    {
        private static void Postfix(ref string __result, ref Hero queriedHero, ref Hero baseHero, bool uppercaseFirst)
        {
            if (!SettingsManager.EnableEverYoung.Value)
            {
                return;
            }
            TextObject textObject = null;
            if (!baseHero.Siblings.Contains(queriedHero))
            {
                return;
            }
            float num = queriedHero.TrueAge();
            float num2 = baseHero.TrueAge();
            textObject = ((num == num2) ? ((!queriedHero.IsFemale) ? GameTexts.FindText("str_twin_male") : GameTexts.FindText("str_twin_female")) : ((!queriedHero.IsFemale) ? ((num > num2) ? GameTexts.FindText("str_bigbrother") : GameTexts.FindText("str_littlebrother")) : ((num > num2) ? GameTexts.FindText("str_bigsister") : GameTexts.FindText("str_littlesister"))));
            __result = textObject.ToString();
            if (!char.IsLower(__result[0]) != uppercaseFirst)
            {
                char[] array = __result.ToCharArray();
                __result = (uppercaseFirst ? array[0].ToString().ToUpper() : array[0].ToString().ToLower());
                for (int i = 1; i < array.Count(); i++)
                {
                    __result += array[i];
                }
            }
        }
    }

    [HarmonyPatch(typeof(CharacterObject), "TroopWage", MethodType.Getter)]
    internal class TroopWagePrefixPatch
    {
        private static bool Prefix(ref CharacterObject __instance, ref int __result)
        {
            if (!SettingsManager.EnableEverYoung.Value)
            {
                return true;
            }
            if (!__instance.IsHero)
            {
                return true;
            }
            if (!__instance.HeroObject.IsWanderer)
            {
                __result = 0;
            }
            else
            {
                __result = 2 + __instance.Level * 2;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(CampaignEvents), "OnHeroJoinedParty")]
    internal class OnHeroJoinedPartyPostfixPatch
    {
        private static void Postfix(ref Hero hero, ref MobileParty mobileParty)
        {
            hero.AddBothBranchPerks();
        }
    }

    [HarmonyPatch(typeof(AgingCampaignBehavior), "DailyTickHero")]
    internal class DailyTickHeroPostfixPatch
    {
        private static void Postfix(ref AgingCampaignBehavior __instance, ref Hero hero)
        {
            hero.AddBothBranchPerks();
            if (!SettingsManager.EnableDailyGainXp.Value)
            {
                return;
            }
            MethodInfo method = typeof(HeroDeveloper).GetMethod("GainRawXp", BindingFlags.Instance | BindingFlags.NonPublic);
            int attributeValue = hero.GetAttributeValue(DefaultCharacterAttributes.Intelligence);
            try
            {
                method.Invoke(hero.HeroDeveloper, new object[2]
                {
                    hero.Level * attributeValue * attributeValue * 2,
                    true
                });
            }
            catch (Exception)
            {
            }
        }
    }

    [HarmonyPatch(typeof(CraftingCampaignBehavior), "HourlyTick")]
    internal class HourlyTickPostfixPatch
    {
        private static void Postfix(ref CraftingCampaignBehavior __instance, ref Dictionary<Hero, object> ____heroCraftingRecords)
        {
            if (!SettingsManager.EnableDailyGainXp.Value)
            {
                return;
            }
            MethodInfo method = typeof(CraftingCampaignBehavior).GetMethod("GetStaminaHourlyRecoveryRate", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (KeyValuePair<Hero, object> ____heroCraftingRecord in ____heroCraftingRecords)
            {
                Hero key = ____heroCraftingRecord.Key;
                int maxHeroCraftingStamina = __instance.GetMaxHeroCraftingStamina(key);
                Traverse traverse = Traverse.Create(____heroCraftingRecord.Value);
                Traverse<int> traverse2 = traverse.Field<int>("CraftingStamina");
                if (traverse2.Value < maxHeroCraftingStamina)
                {
                    int num = (int)method.Invoke(__instance, new object[1] { key });
                    num = (num - 4) / 2;
                    traverse2.Value = MathF.Min(maxHeroCraftingStamina, traverse2.Value + num);
                }
            }
        }
    }

}
