using System.Collections.Generic;
using HarmonyLib;
using MCM.Abstractions.Base.Global;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using UFO.Setting;

namespace UFO.Extension
{
    public static class HeroEnhanceExtensions
    {
        public static readonly TextObject RaidedText = new TextObject("{=RVas572P}Raided");

        public static int AgeScale(this Hero hero)
        {
            if (Game.Current.PlayerTroop == null)
            {
                return -1;
            }
            if (SettingsManager.EnableEverYoung.Value && hero == Hero.MainHero)
            {
                return -1;
            }
            if (hero.Clan != Clan.PlayerClan)
            {
                return 1;
            }
            int num = hero.GetSkillValue(DefaultSkills.Athletics) + hero.GetSkillValue(DefaultSkills.Charm);
            int num2 = 2;
            if (num >= SettingsManager.EverYoungSkillNeed.Value)
            {
                return -1;
            }
            if (!hero.IsWanderer)
            {
                num2++;
            }
            return num2;
        }

        public static bool CombatEnhanceDisable()
        {
            return SettingsManager.CombatAttributeRatePlayer.Value == 0f && SettingsManager.CombatAttributeRateClanMember.Value == 0f && SettingsManager.CombatAttributeRateOther.Value == 0f;
        }

        public static bool StrategyEnhanceDisable()
        {
            return SettingsManager.StrategyAttributeRatePlayer.Value == 0f && SettingsManager.StrategyAttributeRateClanMember.Value == 0f && SettingsManager.StrategyAttributeRateOther.Value == 0f;
        }

        public static float CombatEnhanceRate(this CharacterObject character)
        {
            if (character == null)
            {
                return 0f;
            }
            Hero heroObject = character.HeroObject;
            return heroObject.CombatEnhanceRate();
        }

        public static float CombatEnhanceRate(this Agent agent)
        {
            if (agent == null)
            {
                return 0f;
            }
            CharacterObject character = agent.Character as CharacterObject;
            return character.CombatEnhanceRate();
        }

        public static float CombatEnhanceRate(this Hero hero)
        {
            if (hero == null)
            {
                return 0f;
            }
            if (hero == Hero.MainHero)
            {
                return SettingsManager.CombatAttributeRatePlayer.Value;
            }
            if (hero.Clan == Clan.PlayerClan)
            {
                return SettingsManager.CombatAttributeRateClanMember.Value;
            }
            return SettingsManager.CombatAttributeRateOther.Value;
        }

        public static float StrategyEnhanceRate(this CharacterObject character)
        {
            if (character == null)
            {
                return 0f;
            }
            Hero heroObject = character.HeroObject;
            return heroObject.StrategyEnhanceRate();
        }

        public static float StrategyEnhanceRate(this Agent agent)
        {
            if (agent == null)
            {
                return 0f;
            }
            CharacterObject character = agent.Character as CharacterObject;
            return character.StrategyEnhanceRate();
        }

        public static float StrategyEnhanceRate(this Hero hero)
        {
            if (hero == null)
            {
                return 0f;
            }
            if (hero == Hero.MainHero)
            {
                return SettingsManager.StrategyAttributeRatePlayer.Value;
            }
            if (hero.Clan == Clan.PlayerClan)
            {
                return SettingsManager.StrategyAttributeRateClanMember.Value;
            }
            return SettingsManager.StrategyAttributeRateOther.Value;
        }

        public static int EnhanceType(this CharacterObject character)
        {
            Hero heroObject = character.HeroObject;
            if (heroObject == null)
            {
                return -1;
            }
            if (heroObject == Hero.MainHero)
            {
                return 2;
            }
            if (heroObject.Clan == Clan.PlayerClan)
            {
                return 1;
            }
            return 0;
        }

        public static int EnhanceType(this Hero hero)
        {
            if (hero == null)
            {
                return -1;
            }
            if (hero == Hero.MainHero)
            {
                return 1;
            }
            if (hero.Clan == Clan.PlayerClan)
            {
                return 0;
            }
            return 2;
        }

        public static void AddBothBranchPerks(this Hero hero)
        {
            if (!hero.IsAlive || (int)SettingsManager.AutoChoosePerk.Value > hero.EnhanceType())
            {
                return;
            }

            foreach (PerkObject item in PerkObject.All)
            {
                SkillObject skill = item.Skill;
                if (hero.GetSkillValue(skill) >= item.RequiredSkillValue && !hero.GetPerkValue(item))
                {
                    hero.HeroDeveloper.AddPerk(item);
                }
            }
        }

        public static float TrueAge(this Hero hero)
        {
            if (CampaignOptions.IsLifeDeathCycleDisabled)
            {
                return Traverse.Create(hero).Field("_defaultAge").GetValue<float>();
            }
            CampaignTime value = Traverse.Create(hero).Field("_birthDay").GetValue<CampaignTime>();
            if (hero.IsAlive)
            {
                return value.ElapsedYearsUntilNow;
            }
            return (float)(hero.DeathDay - value).ToYears;
        }
    }

}