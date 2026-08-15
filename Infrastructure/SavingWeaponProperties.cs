using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

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

        public override TextObject TitleText
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string SoundEventPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public WeaponProperties(TextObject description)
            : base(description)
        {
        }
    }

    public class CustomBehavior : CampaignBehaviorBase
    {
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("WeaponPropertiesList", ref WeaponPropertiesList);
        }

        public override void RegisterEvents()
        {
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
