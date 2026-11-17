using System.Reflection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.ScreenSystem;
namespace UFO.Extension
{
    public static class Reflection
    {
        private static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static T GetViewModel<T>(this ScreenBase screen)
        {
            FieldInfo field = screen.GetType().GetField("_dataSource", BindingFlags);
            return (T)field.GetValue(screen);
        }

        public static SPItemVM GetSelectedItem(this SPInventoryVM inventoryVM)
        {
            FieldInfo field = inventoryVM.GetType().GetField("_selectedItem", BindingFlags);
            return (SPItemVM)field.GetValue(inventoryVM);
        }
    }
}
