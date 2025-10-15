using HarmonyLib;
using JetBrains.Annotations;
using SandBox.GauntletUI;
using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using UFO.Extension;
using UFO.Setting;
namespace UFO.Patch;


[HarmonyPatch(typeof(EncyclopediaPageVM), "OnTick")]
public class EnableHotkeysAddEncyclopediaTroops
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        if (!ILInjection.SearchInjection(instructions.ToMBList(), "EnableHotkeysEncyclopedia"))
        {
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ILInjection), "EnableHotkeysEncyclopedia"));
            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;
            }
            yield break;
        }
        foreach (CodeInstruction instruction2 in instructions)
        {
            yield return instruction2;
        }
    }

    public static void Handler(EncyclopediaPageVM __instance)
    {
        try
        {
            if (__instance is EncyclopediaUnitPageVM && __instance.Obj is CharacterObject characterObject && SettingsManager.EnableHotkeys.Value)
            {
                if (Keys.IsKeyPressed(InputKey.H, InputKey.LeftShift, InputKey.LeftControl))
                {
                    AddTroops(characterObject, 10);
                }
                else if (Keys.IsKeyPressed(InputKey.H, InputKey.LeftControl))
                {
                    AddTroops(characterObject, 1);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysAddEncyclopediaTroops));
        }
    }

    public static void AddTroops(CharacterObject characterObject, int count)
    {
        PartyBase.MainParty.AddMember(characterObject, count);
        string text = string.Format(L10N.GetText("AddTroopsFromEncyclopediaMessage"), count, characterObject.Name);
        Message.Show(text);
    }
}



[HarmonyPatch(typeof(GameManagerBase), "OnTick")]
public static class EnableHotkeysAddItems
{
    //private static bool patched;

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        if (!ILInjection.SearchInjection(instructions.ToMBList(), "EnableHotkeysGameManagerBase"))
        {
            //patched = true;
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ILInjection), "EnableHotkeysGameManagerBase"));
            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;
            }
            yield break;
        }
        foreach (CodeInstruction instruction2 in instructions)
        {
            yield return instruction2;
        }
    }

    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletInventoryScreen && SettingsManager.EnableHotkeys.Value)
            {
                if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.LeftShift, InputKey.H))
                {
                    AddItems(100);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.H))
                {
                    AddItems(1);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysAddItems));
        }
    }

    public static void AddItems(int count)
    {
        GauntletInventoryScreen screen = ScreenManager.TopScreen as GauntletInventoryScreen;
        SPInventoryVM viewModel = screen.GetViewModel<SPInventoryVM>();
        SPItemVM selectedItem = viewModel.GetSelectedItem();
        if (selectedItem != null)
        {
            int num = PartyBase.MainParty.ItemRoster.FindIndexOfElement(selectedItem.ItemRosterElement.EquipmentElement);
            if (num >= 0)
            {
                PartyBase.MainParty.ItemRoster.AddToCounts(selectedItem.ItemRosterElement.EquipmentElement, count);
                selectedItem.ItemCount += count;
                string text = string.Format(L10N.GetText("AddItemsMessage"), count, selectedItem.ItemDescription);
                Message.Show(text);
            }
        }
    }
}



public static class EnableHotkeysChangePlayerCharacter
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Handler(EncyclopediaPageVM __instance)
    {
        try
        {
            if (!(__instance is EncyclopediaHeroPageVM))
            {
                return;
            }
            object obj = __instance.Obj;
            Hero hero = obj as Hero;
            if (hero != null && SettingsManager.EnableHotkeys.Value && Keys.IsKeyPressed(InputKey.H, InputKey.LeftControl))
            {
                InformationManager.ShowInquiry(new InquiryData(L10N.GetTextFormat("ChangePlayerMessageTitle", hero.Name), L10N.GetText("ChangePlayerMessage"), isAffirmativeOptionShown: true, isNegativeOptionShown: true, L10N.GetText("ChangePlayerConfirm"), L10N.GetText("Cancel"), delegate
                {
                    ChangePlayerCharacterAction.Apply(hero);
                }, null), pauseGameActiveState: true, prioritize: true);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysKillCharacter));
        }
    }
}



public static class EnableHotkeysCharacterAttributes
{
    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletCharacterDeveloperScreen && SettingsManager.EnableHotkeys.Value)
            {
                if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.A))
                {
                    CharacterDeveloperVM viewModel = ScreenManager.TopScreen.GetViewModel<CharacterDeveloperVM>();
                    Hero hero = viewModel.CurrentCharacter.Hero;
                    SetMaximum(hero, DefaultCharacterAttributes.Control);
                    SetMaximum(hero, DefaultCharacterAttributes.Cunning);
                    SetMaximum(hero, DefaultCharacterAttributes.Endurance);
                    SetMaximum(hero, DefaultCharacterAttributes.Intelligence);
                    SetMaximum(hero, DefaultCharacterAttributes.Social);
                    SetMaximum(hero, DefaultCharacterAttributes.Vigor);
                    viewModel.RefreshValues();
                    string text = string.Format(L10N.GetText("SetAllAttributesMessage"), hero.Name);
                    Message.Show(text);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.D1))
                {
                    AddPoint(DefaultCharacterAttributes.Vigor);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.D2))
                {
                    AddPoint(DefaultCharacterAttributes.Control);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.D3))
                {
                    AddPoint(DefaultCharacterAttributes.Endurance);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.D4))
                {
                    AddPoint(DefaultCharacterAttributes.Cunning);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.D5))
                {
                    AddPoint(DefaultCharacterAttributes.Social);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.D6))
                {
                    AddPoint(DefaultCharacterAttributes.Intelligence);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysCharacterAttributes));
        }
    }

    public static void SetMaximum(Hero hero, CharacterAttribute attribute)
    {
        int changeAmount = Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute - hero.GetAttributeValue(attribute);
        hero.HeroDeveloper.AddAttribute(attribute, changeAmount, checkUnspentPoints: false);
    }

    public static void AddPoint(CharacterAttribute attribute)
    {
        CharacterDeveloperVM viewModel = ScreenManager.TopScreen.GetViewModel<CharacterDeveloperVM>();
        Hero hero = viewModel.CurrentCharacter.Hero;
        int attributeValue = hero.GetAttributeValue(attribute);
        if (attributeValue < Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute)
        {
            hero.HeroDeveloper.AddAttribute(attribute, 1, checkUnspentPoints: false);
            viewModel.RefreshValues();
            string text = string.Format(L10N.GetText("AddAttributePointMessage"), attribute.Name, hero.Name);
            Message.Show(text);
        }
    }
}



public static class EnableHotkeysCharacterPoints
{
    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletCharacterDeveloperScreen && SettingsManager.EnableHotkeys.Value)
            {
                if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.F))
                {
                    CharacterDeveloperVM viewModel = ScreenManager.TopScreen.GetViewModel<CharacterDeveloperVM>();
                    Hero hero = viewModel.CurrentCharacter.Hero;
                    hero.HeroDeveloper.UnspentFocusPoints++;
                    viewModel.CurrentCharacter.UnspentCharacterPoints++;
                    string text = string.Format(L10N.GetText("AddUnspentFocusPointMessage"), hero.Name);
                    Message.Show(text);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.G))
                {
                    CharacterDeveloperVM viewModel2 = ScreenManager.TopScreen.GetViewModel<CharacterDeveloperVM>();
                    Hero hero2 = viewModel2.CurrentCharacter.Hero;
                    hero2.HeroDeveloper.UnspentAttributePoints++;
                    viewModel2.CurrentCharacter.UnspentAttributePoints++;
                    string text2 = string.Format(L10N.GetText("AddUnspentAttributePointMessage"), hero2.Name);
                    Message.Show(text2);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysCharacterPoints));
        }
    }
}

public static class EnableHotkeysInfluence
{
    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletClanScreen && Keys.IsKeyPressed(InputKey.LeftControl, InputKey.X) && SettingsManager.EnableHotkeys.Value)
            {
                Hero.MainHero.AddInfluenceWithKingdom(1000f);
                Message.Show(L10N.GetText("AddInfluenceMessage"));
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysInfluence));
        }
    }
}


public static class EnableHotkeysKillCharacter
{
    public static void Handler(EncyclopediaPageVM __instance)
    {
        try
        {
            if (!(__instance is EncyclopediaHeroPageVM))
            {
                return;
            }
            object obj = __instance.Obj;
            Hero hero = obj as Hero;
            if (hero != null && SettingsManager.EnableHotkeys.Value && Keys.IsKeyPressed(InputKey.X, InputKey.LeftControl))
            {
                InformationManager.ShowInquiry(new InquiryData(L10N.GetTextFormat("KillCharacterMessageTitle", hero.Name), L10N.GetText("KillCharacterMessage"), isAffirmativeOptionShown: true, isNegativeOptionShown: true, L10N.GetText("KillCharacterConfirm"), L10N.GetText("Cancel"), delegate
                {
                    KillCharacterAction.ApplyByMurder(hero);
                }, null), pauseGameActiveState: true, prioritize: true);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysKillCharacter));
        }
    }
}


public static class EnableHotkeysMoney
{
    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletInventoryScreen && SettingsManager.EnableHotkeys.Value)
            {
                if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.LeftShift, InputKey.X))
                {
                    Hero.MainHero.ChangeHeroGold(100000);
                    Message.Show(string.Format(L10N.GetText("AddGoldMessage"), 100000));
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.X))
                {
                    Hero.MainHero.ChangeHeroGold(1000);
                    Message.Show(string.Format(L10N.GetText("AddGoldMessage"), 1000));
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysMoney));
        }
    }
}


public static class EnableHotkeysTransferSettlement
{
    public static void Handler(EncyclopediaPageVM __instance)
    {
        try
        {
            if (!(__instance is EncyclopediaSettlementPageVM))
            {
                return;
            }
            object obj = __instance.Obj;
            Settlement settlement = obj as Settlement;
            if (settlement != null && (settlement.IsCastle || settlement.IsTown) && SettingsManager.EnableHotkeys.Value && Keys.IsKeyPressed(InputKey.H, InputKey.LeftControl))
            {
                InformationManager.ShowInquiry(new InquiryData(L10N.GetTextFormat("TransferSettlementMessageTitle", settlement.Name), L10N.GetText("TransferSettlementMessage"), isAffirmativeOptionShown: true, isNegativeOptionShown: true, L10N.GetText("TransferSettlementConfirm"), L10N.GetText("Cancel"), delegate
                {
                    ChangeOwnerOfSettlementAction.ApplyByDefault(Hero.MainHero, settlement);
                }, null), pauseGameActiveState: true, prioritize: true);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysTransferSettlement));
        }
    }
}



public static class EnableHotkeysTroopCount
{
    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletPartyScreen && SettingsManager.EnableHotkeys.Value)
            {
                if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.LeftShift, InputKey.H))
                {
                    AddTroops(10);
                }
                else if (Keys.IsKeyPressed(InputKey.LeftControl, InputKey.H))
                {
                    AddTroops(1);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysTroopCount));
        }
    }

    private static void AddTroops(int count)
    {
        GauntletPartyScreen screen = ScreenManager.TopScreen as GauntletPartyScreen;
        PartyVM viewModel = screen.GetViewModel<PartyVM>();
        PartyCharacterVM currentCharacter = viewModel.CurrentCharacter;
        if (!currentCharacter.IsHero)
        {
            if (currentCharacter.IsPrisoner)
            {
                PartyBase.MainParty.AddPrisoner(currentCharacter.Character, count);
            }
            else
            {
                PartyBase.MainParty.AddMember(currentCharacter.Character, count);
            }
            TroopRosterElement troop = currentCharacter.Troop;
            troop.Number += count;
            currentCharacter.Troop = troop;
            currentCharacter.UpdateTradeData();
            currentCharacter.ThrowOnPropertyChanged();
            string text = string.Format(L10N.GetText("AddTroopsMessage"), count, currentCharacter.Name);
            Message.Show(text);
        }
    }
}


public static class EnableHotkeysTroopExperience
{
    public static void Handler()
    {
        try
        {
            if (ScreenManager.TopScreen is GauntletPartyScreen && Keys.IsKeyPressed(InputKey.LeftControl, InputKey.X) && SettingsManager.EnableHotkeys.Value)
            {
                GauntletPartyScreen screen = ScreenManager.TopScreen as GauntletPartyScreen;
                PartyVM viewModel = screen.GetViewModel<PartyVM>();
                PartyCharacterVM currentCharacter = viewModel.CurrentCharacter;
                if (!currentCharacter.IsHero && currentCharacter.IsUpgradableTroop)
                {
                    int index = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(currentCharacter.Character);
                    int xpAmount = currentCharacter.MaxXP * currentCharacter.Number - currentCharacter.CurrentXP;
                    PartyBase.MainParty.MemberRoster.AddXpToTroopAtIndex(index, xpAmount);
                    TroopRosterElement troop = currentCharacter.Troop;
                    troop.Xp = currentCharacter.MaxXP * currentCharacter.Number;
                    currentCharacter.Troop = troop;
                    currentCharacter.InitializeUpgrades();
                    string text = string.Format(L10N.GetText("AddTroopXpMessage"), currentCharacter.Name);
                    Message.Show(text);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeysTroopExperience));
        }
    }
}



[HarmonyPatch(typeof(MapScreen), "OpenEncyclopedia")]
public static class EnableHotkeyTipsEncyclopedia
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void OpenEncyclopedia()
    {
        try
        {
            if (SettingsManager.EnableHotkeys.Value && SettingsManager.EnableHotkeyTips.Value)
            {
                Message.Show("Encyclopedia Screen Cheat Hotkeys:");
                Message.Show("CTRL + H: Add 1 soldier of the selected troop type to the party.");
                Message.Show("CTRL + SHIFT + H: Add 10 soldiers of the selected troop type to the party.");
                Message.Show("CTRL + X: Kill the selected character.");
                Message.Show("CTRL + H: Change to the selected character.");
                Message.Show("CTRL + H: Transfer ownership of settlement to you.");
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeyTipsEncyclopedia));
        }
    }
}


[HarmonyPatch(typeof(ScreenManager), "PushScreen")]
public static class EnableHotkeyTipsScreens
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void PushScreen(ref ScreenBase screen)
    {
        try
        {
            if (!SettingsManager.EnableHotkeys.Value || !SettingsManager.EnableHotkeyTips.Value)
            {
                return;
            }
            ScreenBase screenBase = screen;
            ScreenBase screenBase2 = screenBase;
            if (!(screenBase2 is GauntletInventoryScreen))
            {
                if (!(screenBase2 is GauntletClanScreen))
                {
                    if (!(screenBase2 is GauntletPartyScreen))
                    {
                        if (screenBase2 is GauntletCharacterDeveloperScreen)
                        {
                            Message.Show("Character Screen Cheat Hotkeys:");
                            Message.Show("CTRL + A: Set all character attributes to 10.");
                            Message.Show("CTRL + (1-6): Add 1 point to the attribute at the given index.");
                        }
                    }
                    else
                    {
                        Message.Show("Party Screen Cheat Hotkeys:");
                        Message.Show("CTRL + H: Add 1 soldier to the selected troop.");
                        Message.Show("CTRL + SHIFT + H: Add 10 soldiers to the selected troop.");
                        Message.Show("CTRL + X: Add experience to the selected troop.");
                    }
                }
                else
                {
                    Message.Show("Clan Screen Cheat Hotkeys:");
                    Message.Show("CTRL + X: Add 1.000 influence.");
                }
            }
            else
            {
                Message.Show("Inventory Screen Cheat Hotkeys:");
                Message.Show("CTRL + X: Add 1.000 gold.");
                Message.Show("CTRL + SHIFT + X: Add 100.000 gold.");
                Message.Show("CTRL + H: Add 1 to the selected item type.");
                Message.Show("CTRL + SHIFT + H: Add 100 to the selected item type.");
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(EnableHotkeyTipsScreens));
        }
    }
}