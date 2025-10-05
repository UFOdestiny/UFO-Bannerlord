using HarmonyLib;
using JetBrains.Annotations;
using System;
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
namespace UFO.Patch;

[HarmonyPatch(typeof(DefaultInventoryCapacityModel), "CalculateInventoryCapacity")]
public static class ExtraInventoryCapacity
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateInventoryCapacity(MobileParty mobileParty, bool includeDescriptions, int additionalTroops, int additionalSpareMounts, int additionalPackAnimals, bool includeFollowers, ref ExplainedNumber __result)
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
    typeof(MobileParty),
    typeof(bool),
    typeof(bool),
    typeof(CharacterObject),
    typeof(InventoryManager.InventoryCategoryType),
    typeof(IMarketData),
    typeof(bool),
    typeof(TextObject),
    typeof(TroopRoster),
    typeof(InventoryLogic.CapacityData)
})]
public static class NativeItemSpawning
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Initialize(ref ItemRoster leftItemRoster, ref MobileParty party, ref bool isTrading, ref bool isSpecialActionsPermitted, ref CharacterObject initialCharacterOfRightRoster, ref InventoryManager.InventoryCategoryType merchantItemType, ref IMarketData marketData, ref bool useBasePrices, ref TextObject leftRosterName, ref TroopRoster leftMemberRoster, ref InventoryLogic.CapacityData otherSideCapacityData)
    {
        try
        {
            if (party.IsPlayerParty() && !isTrading && !Game.Current.CheatMode && SettingsManager.NativeItemSpawning.IsChanged)
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
