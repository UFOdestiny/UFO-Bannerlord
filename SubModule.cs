using HarmonyLib;
using MCM.Abstractions.Base.Global;
using SandBox.GameComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using UFO.Extension;

namespace UFO
{
    internal class SubModule : MBSubModuleBase
    {
        private bool PatchesApplied = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Loaded", Colors.Green));
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (!(game.GameType is Campaign) || PatchesApplied)
            {
                return;
            }

            Harmony patcher = new Harmony("UFO");

            PATCH(patcher, ref PatchesApplied);

            //PatchInspector.PatchInformation();

            //InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Patch Applied", Colors.Green));
        }


        internal static void PATCH(Harmony patcher, ref bool PatchesApplied)
        {
            Assembly assembly = typeof(SubModule).Assembly;
            Type[] typesFromAssembly = AccessTools.GetTypesFromAssembly(assembly);
            List<string> list = new List<string>();
            Type[] array = typesFromAssembly;
            foreach (Type type in array)
            {
                try
                {
                    new PatchClassProcessor(patcher, type).Patch();
                }
                catch (HarmonyException)
                {
                    list.Add(type.Name);
                }
            }

            PatchesApplied = true;
            if (list.Any())
            {
                InformationManager.ShowInquiry(new InquiryData(L10N.GetText("ModFailedLoadWarningTitle"), L10N.GetTextFormat("ModFailedLoadWarningMessage", string.Join(Environment.NewLine, list)), isAffirmativeOptionShown: true, isNegativeOptionShown: false, L10N.GetText("ModWarningMessageConfirm"), null, null, null));
            }
        }

        internal static void UNPATCH(Harmony patcher)
        {
            var methodsToUnpatch = new List<MethodBase>
            {
                AccessTools.Method(typeof(Mission), "DecideWeaponCollisionReaction"),
            };

            foreach (var method in methodsToUnpatch)
            {
                patcher.Unpatch(method, HarmonyPatchType.Postfix, "mod.bannerlord.shokuho");
            }

            foreach (var method in methodsToUnpatch)
            {
                patcher.Unpatch(method, HarmonyPatchType.Prefix, "mod.bannerlord.shokuho");
            }
        }


        internal static void LogError(Exception e, Type type)
        {
            string text;
            try
            {
                text = CreateErrorFile(e, type);
            }
            catch
            {
                return;
            }
            try
            {
                InformationManager.ShowInquiry(new InquiryData(L10N.GetText("ModExceptionTitle"), L10N.GetTextFormat("ModExceptionMessage", text), isAffirmativeOptionShown: true, isNegativeOptionShown: false, L10N.GetText("ModWarningMessageConfirm"), null, null, null));
            }
            catch
            {
                try
                {
                    Message.Show(L10N.GetTextFormat("ModExceptionMessage", text), Colors.Red);
                }
                catch
                {
                }
            }
        }

        private static string CreateErrorFile(Exception e, Type type = null)
        {
            string path = $"Error-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
            string location = Assembly.GetAssembly(typeof(SubModule)).Location;
            string directoryName = Path.GetDirectoryName(location);
            string text = Path.Combine(directoryName, path);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Thanks a lot for helping to improve this mod!");
            stringBuilder.AppendLine("You could drop the contents of this file into https://pastebin.com/ and post a link to the file");
            stringBuilder.AppendLine("in the NexusMods posts page at https://www.nexusmods.com/mountandblade2bannerlord/mods/1839?tab=posts");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Modules:");
            foreach (ModuleInfo module in ModuleHelper.GetModules())
            {
                stringBuilder.AppendLine($"{module.Name} {module.Version}");
            }
            if (type != null)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Harmony Patch:");
                HarmonyPatch customAttribute = type.GetCustomAttribute<HarmonyPatch>();
                stringBuilder.AppendLine("Type: " + type.FullName);
                stringBuilder.AppendLine("Declaring Type: " + customAttribute.info.declaringType.FullName);
                stringBuilder.AppendLine("Method: " + customAttribute.info.methodName);
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Exception:");
            stringBuilder.AppendLine(e.ToString());
            File.WriteAllText(text, stringBuilder.ToString());
            return text;
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (game.GameType is Campaign)
            {
                ((CampaignGameStarter)gameStarterObject).AddBehavior(Activator.CreateInstance<SavingWeaponProperties.CustomBehavior>());
            }
        }


    }

}
