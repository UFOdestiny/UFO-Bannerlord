using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using UFO.Behavior;
using UFO.Extension;
using UFO.Model;
using UFO.Setting;

namespace UFO;

internal class SubModule : MBSubModuleBase
{
    private bool PatchesApplied = false;

    protected override void OnSubModuleLoad()
    {
        base.OnSubModuleLoad();
    }

    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Loaded", Colors.Green));
        L10N.LoadLanguage();
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStart(game, gameStarterObject);

        ReplaceModel<DefaultCharacterDevelopmentModel, ModifiedCharacterDevelopmentModel>(gameStarterObject);

        if (game.GameType is Campaign)
        {
            CampaignGameStarter starter = (CampaignGameStarter)gameStarterObject;
            starter.AddBehavior(Activator.CreateInstance<SavingWeaponProperties.CustomBehavior>());
            starter.AddBehavior(Activator.CreateInstance<AddMoney>());
        }
    }

    protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
    {
        base.InitializeGameStarter(game, starterObject);
        if (starterObject is CampaignGameStarter campaignGameStarter)
        {
            campaignGameStarter.AddBehavior(new RecruitExileClan());
        }
    }

    public override void OnGameInitializationFinished(Game game)
    {
        base.OnGameInitializationFinished(game);

        if (!(game.GameType is Campaign) || PatchesApplied)
        {
            return;
        }

        Harmony patcher = new Harmony("UFO");

        //UNPATCH(patcher);

        PATCH(patcher, ref PatchesApplied);

        //PatchInspector.PatchInformation();

        //InformationManager.DisplayMessage(new InformationMessage("UFO's Mod Patch Applied", Colors.Green));
    }

    // Utils
    internal static void PATCH(Harmony patcher, ref bool PatchesApplied)
    {
        Assembly assembly = typeof(SubModule).Assembly;
        Type[] typesFromAssembly = AccessTools.GetTypesFromAssembly(assembly);
        List<string> list = new List<string>();
        List<string> Shokuho_list = new List<string>();
        Type[] array = typesFromAssembly;
        foreach (Type type in array)
        {
            try
            {
                //PatchInfo(type);
                new PatchClassProcessor(patcher, type).Patch();
            }
            catch (HarmonyException)
            {
                if (getNamespace(type) == "Shokuho")
                {
                    Shokuho_list.Add(type.Name);
                }
                else
                {
                    list.Add(type.Name);
                }
            }
        }

        PatchesApplied = true;
        if (list.Any())
        {
            InformationManager.ShowInquiry(new InquiryData(L10N.GetText("ModFailedLoadWarningTitle"), L10N.GetTextFormat("ModFailedLoadWarningMessage", string.Join(Environment.NewLine, list)), isAffirmativeOptionShown: true, isNegativeOptionShown: false, L10N.GetText("ModWarningMessageConfirm"), null, null, null));
        }

        if (Shokuho_list.Any())
        {
            InformationManager.DisplayMessage(new InformationMessage("Shokuho Not Loaded", Colors.Red));
        }
    }

    internal static string getNamespace(Type type)
    {
        var patchAttrs = type.GetCustomAttributes(typeof(HarmonyPatch), false);
        foreach (HarmonyPatch attr in patchAttrs)
        {
            string targetSpace = attr.info.declaringType?.Namespace ?? "UnknownMethod";
            string largerSpace = targetSpace.Contains('.') ? targetSpace.Split('.')[0] : targetSpace;
            return largerSpace;
        }
        return "Unknown Namespace";
    }

    internal static void PatchInfo(Type type)
    {
        var patchAttrs = type.GetCustomAttributes(typeof(HarmonyPatch), false);
        foreach (HarmonyPatch attr in patchAttrs)
        {
            string targetType = attr.info.declaringType?.FullName ?? "UnknownType";
            string targetMethod = attr.info.methodName ?? "UnknownMethod";
            string targetSpace = attr.info.declaringType?.Namespace ?? "UnknownMethod";
            string largerSpace = targetSpace.Contains('.') ? targetSpace.Split('.')[0] : targetSpace;
            InformationManager.DisplayMessage(
                new InformationMessage($"[{largerSpace}] {type.Name} → {targetType} → {targetMethod}")
            );
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
        //string location = "C:\\Users\\Administrator\\Desktop";
        string directoryName = Path.GetDirectoryName(location);
        string text = Path.Combine(directoryName, path);
        InformationManager.DisplayMessage(new InformationMessage(text, Colors.Red));
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

    protected void ReplaceModel<TBaseType, TChildType>(IGameStarter gameStarterObject) where TBaseType : GameModel where TChildType : TBaseType
    {
        if (!(gameStarterObject.Models is IList<GameModel> models))
        {
            return;
        }
        bool found = false;
        for (int index = 0; index < models.Count; index++)
        {
            if (models[index] is TBaseType)
            {
                found = true;
                models[index] = Activator.CreateInstance<TChildType>();
            }
        }
        if (!found)
        {
            gameStarterObject.AddModel(Activator.CreateInstance<TChildType>());
        }
    }

}


