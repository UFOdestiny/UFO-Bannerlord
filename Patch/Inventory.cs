using HarmonyLib;
using Helpers;
using JetBrains.Annotations;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using UFO.Extension;
using UFO.Setting;
using static Helpers.InventoryScreenHelper;
using static TaleWorlds.CampaignSystem.Inventory.InventoryLogic;
namespace UFO.Patch;

[HarmonyPatch(typeof(DefaultInventoryCapacityModel), "CalculateInventoryCapacity")]
public static class ExtraInventoryCapacity
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateInventoryCapacity(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions, int additionalTroops, int additionalSpareMounts, int additionalPackAnimals, bool includeFollowers,
        ref ExplainedNumber __result)
    {
        try
        {
            if (mobileParty.IsPlayerParty() && SettingsManager.ExtraInventoryCapacity.IsChanged)
            {
                __result.Add(SettingsManager.ExtraInventoryCapacity.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraInventoryCapacity));
        }
    }
}


[HarmonyPatch(typeof(InventoryLogic), "Initialize", new Type[]
{
        typeof(ItemRoster),
        typeof(ItemRoster),
        typeof(TroopRoster),
        typeof(bool),
        typeof(bool),
        typeof(CharacterObject),
        typeof(InventoryScreenHelper.InventoryCategoryType),
        typeof(IMarketData),
        typeof(bool),
        typeof(InventoryScreenHelper.InventoryMode),
        typeof(TextObject),
        typeof(TroopRoster),
        typeof(CapacityData)
})]
public static class NativeItemSpawning
{
    [HarmonyPostfix]
    public static void Postfix(ItemRoster leftItemRoster,
            ItemRoster rightItemRoster,
            TroopRoster rightMemberRoster,
            bool isTrading,
            bool isSpecialActionsPermitted,
            CharacterObject initialCharacterOfRightRoster,
            InventoryScreenHelper.InventoryCategoryType merchantItemType,
            IMarketData marketData,
            bool useBasePrices,
            InventoryScreenHelper.InventoryMode inventoryMode,
            TextObject leftRosterName = null,
            TroopRoster leftMemberRoster = null,
            CapacityData otherSideCapacityData = null)
    {
        try
        {
            //var ownerPartyProperty = typeof(TroopRoster).GetProperty("OwnerParty", BindingFlags.Instance | BindingFlags.NonPublic);
            //var ownerParty = ownerPartyProperty?.GetValue(leftMemberRoster) as PartyBase;
            //ownerParty != null &&

            if (!isTrading && !Game.Current.CheatMode && SettingsManager.NativeItemSpawning.Value)
            {
                MBReadOnlyList<ItemObject> objectTypeList = Game.Current.ObjectManager.GetObjectTypeList<ItemObject>();
                for (int i = 0; i != objectTypeList.Count; i++)
                {
                    ItemObject item = objectTypeList[i];
                    leftItemRoster.AddToCounts(item, 10);
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NativeItemSpawning));
        }
    }
}
