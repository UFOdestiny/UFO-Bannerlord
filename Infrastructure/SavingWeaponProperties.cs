using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;
using UFO.Infrastructure;
using UFO.Diagnostics;

internal class SavingWeaponProperties
{
    public class WeaponProperties : InformationData
    {
        [SaveableProperty(6)]
        public string StringId { get; set; }

        [SaveableProperty(7)]
        public int Handling { get; set; }

        [SaveableProperty(8)]
        public int SwingDamage { get; set; }

        [SaveableProperty(9)]
        public int SwingSpeed { get; set; }

        [SaveableProperty(10)]
        public int ThrustDamage { get; set; }

        [SaveableProperty(11)]
        public int ThrustSpeed { get; set; }

        // Missing in older saves, where the single record describes usage zero.
        [SaveableProperty(12)]
        public int UsageIndex { get; set; }

        public override TextObject TitleText
        {
            get
            {
                return new TextObject(StringId ?? string.Empty);
            }
        }

        public override string SoundEventPath
        {
            get
            {
                return string.Empty;
            }
        }

        public WeaponProperties(TextObject description)
            : base(description)
        {
        }

        internal void ApplyTo(WeaponComponentData weapon) => WeaponDesignAccess.ApplyStatBonuses(weapon,
            Handling - weapon.Handling, SwingDamage - weapon.SwingDamage,
            SwingSpeed - weapon.SwingSpeed, ThrustDamage - weapon.ThrustDamage,
            ThrustSpeed - weapon.ThrustSpeed);
    }

    public class CustomBehavior : CampaignBehaviorBase
    {
        public CustomBehavior() => WeaponPropertiesList = new List<WeaponProperties>();

        public override void SyncData(IDataStore dataStore)
        {
            ModDiagnostics.TraceRuntime("WeaponProperties SyncData started.");
            dataStore.SyncData("WeaponPropertiesList", ref WeaponPropertiesList);
            WeaponPropertiesList ??= new List<WeaponProperties>();
            ModDiagnostics.TraceRuntime("WeaponProperties SyncData completed; records=" + WeaponPropertiesList.Count);
        }

        public override void RegisterEvents()
        {
            // Native reconstructs crafted weapon stats during loading. Restore only
            // after that reconstruction, and set absolute values to avoid stacking.
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, RestoreWeapons);
        }

        private void RestoreWeapons(CampaignGameStarter starter)
        {
            ModDiagnostics.TraceRuntime("Weapon restoration started; records=" + WeaponPropertiesList.Count);
            foreach (var properties in WeaponPropertiesList)
            {
                if (properties == null || string.IsNullOrEmpty(properties.StringId)) continue;
                var item = Game.Current.ObjectManager.GetObject<ItemObject>(properties.StringId);
                if (item == null || properties.UsageIndex < 0 || properties.UsageIndex >= item.Weapons.Count) continue;
                var weapon = item.Weapons[properties.UsageIndex];
                properties.ApplyTo(weapon);
            }
            ModDiagnostics.TraceRuntime("Weapon restoration completed.");
        }
    }

    public class CustomSaveDefiner : SaveableTypeDefiner
    {
        public CustomSaveDefiner()
            : base(57671680)
        {
        }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(WeaponProperties), 1);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(List<WeaponProperties>));
        }
    }

    public static List<WeaponProperties> WeaponPropertiesList = new List<WeaponProperties>();
}
