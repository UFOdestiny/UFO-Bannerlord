using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;
using HarmonyLib;

internal static class ManagedRegressionChecks
{
    internal static void Run(Assembly mod)
    {
        var propertiesType = mod.GetType("SavingWeaponProperties+WeaponProperties", true);
        var core = Assembly.Load("TaleWorlds.Core");
        var weaponType = core.GetType("TaleWorlds.Core.WeaponComponentData", true);
        var apply = propertiesType.GetMethod("ApplyTo", BindingFlags.Instance | BindingFlags.NonPublic);
        var names = new[] { "Handling", "SwingDamage", "SwingSpeed", "ThrustDamage", "ThrustSpeed" };
        for (var usage = 0; usage < 2; usage++)
        {
            var record = Activator.CreateInstance(propertiesType, new object[] { null });
            propertiesType.GetProperty("UsageIndex").SetValue(record, usage);
            var weapon = FormatterServices.GetUninitializedObject(weaponType);
            for (var index = 0; index < names.Length; index++)
            {
                propertiesType.GetProperty(names[index]).SetValue(record, 100 + usage * 20 + index);
                weaponType.GetProperty(names[index]).SetValue(weapon, 40 + index);
            }
            // Run restoration twice: loading must restore exact saved values,
            // including different secondary-usage stats, without accumulating.
            for (var repeat = 0; repeat < 2; repeat++)
            {
                apply.Invoke(record, new[] { weapon });
                for (var index = 0; index < names.Length; index++)
                    if ((int)weaponType.GetProperty(names[index]).GetValue(weapon) != 100 + usage * 20 + index)
                        throw new InvalidOperationException("Crafted weapon restoration changed " + names[index]);
            }
        }
        var ownerType = mod.GetType("SavingWeaponProperties", true);
        var list = (IList)ownerType.GetField("WeaponPropertiesList").GetValue(null);
        list.Add(Activator.CreateInstance(propertiesType, new object[] { null }));
        Activator.CreateInstance(mod.GetType("SavingWeaponProperties+CustomBehavior", true));
        if (((IList)ownerType.GetField("WeaponPropertiesList").GetValue(null)).Count != 0)
            throw new InvalidOperationException("Weapon records leaked into a new campaign.");
        Console.WriteLine("PASS regression: crafted stat restore, repeated restore, secondary usage, campaign isolation.");
        ValidatePatchLifecycle(mod);
        ValidateXmlEnums(core, Path.Combine(Directory.GetParent(Path.GetDirectoryName(mod.Location)).Parent.FullName, "ModuleData"));
    }

    private static void ValidatePatchLifecycle(Assembly mod)
    {
        const string owner = "UFO.lifecycle-regression";
        var harmony = new Harmony(owner);
        var bootstrap = mod.GetType("UFO.Patching.PatchBootstrapper", true).GetMethod("Apply", BindingFlags.Static | BindingFlags.NonPublic);
        var campaign = Assembly.Load("TaleWorlds.CampaignSystem");
        var age = campaign.GetType("TaleWorlds.CampaignSystem.Hero", true).GetProperty("Age").GetMethod;
        var hp = campaign.GetType("TaleWorlds.CampaignSystem.GameComponents.DefaultCharacterStatsModel", true).GetMethod("MaxHitpoints");
        bool HasOwner(MethodBase target) => Harmony.GetPatchInfo(target)?.Owners.Contains(owner) == true;
        void Apply(bool early)
        {
            var failures = (IEnumerable)bootstrap.Invoke(null, new object[] { harmony, mod, early });
            if (failures.Cast<object>().Any()) throw new InvalidOperationException("Lifecycle patch installation failed.");
        }
        for (var session = 0; session < 2; session++)
        {
            Apply(true);
            if (harmony.GetPatchedMethods().Count() != 1 || HasOwner(age) || HasOwner(hp))
                throw new InvalidOperationException("Gameplay patches became active before save deserialization completed.");
            Apply(false);
            if (!HasOwner(age) || !HasOwner(hp)) throw new InvalidOperationException("Gameplay patches were not activated after loading.");
            harmony.UnpatchAll(owner);
            if (HasOwner(age) || HasOwner(hp)) throw new InvalidOperationException("Gameplay patches leaked into the next load.");
        }
        var settings = mod.GetType("UFO.Setting.SettingsManager", true);
        if ((bool)settings.GetProperty("IsPerCampaignInstanceLoaded", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null))
            throw new InvalidOperationException("Campaign settings were queried before a campaign existed.");
        Console.WriteLine("PASS regression: early/late patch isolation, teardown, second-load isolation, MCM load guard.");
    }

    private static void ValidateXmlEnums(Assembly core, string moduleData)
    {
        var contracts = new[]
        {
            ("//WeaponFlag/@value | //WeaponFlags/@*", "TaleWorlds.Core.WeaponFlags"),
            ("//Weapon/@weapon_class | //WeaponDescription/@weapon_class", "TaleWorlds.Core.WeaponClass"),
            ("//@damage_type | //@thrust_damage_type | //@swing_damage_type", "TaleWorlds.Core.DamageTypes"),
            ("//CraftingPiece/@piece_type | //Piece/@Type | //PieceData/@piece_type", "TaleWorlds.Core.CraftingPiece+PieceTypes"),
            ("//StatData/@stat_type", "TaleWorlds.Core.CraftingTemplate+CraftingStatTypes"),
            ("//Item/@Type | //CraftingTemplate/@item_type", "TaleWorlds.Core.ItemObject+ItemTypeEnum"),
            ("//Material/@id", "TaleWorlds.Core.CraftingMaterials")
        };
        var count = 0;
        foreach (var file in Directory.GetFiles(moduleData, "*.xml", SearchOption.AllDirectories))
        {
            var document = new XmlDocument();
            document.Load(file);
            foreach (var contract in contracts)
            foreach (XmlAttribute attribute in document.SelectNodes(contract.Item1))
            {
                var enumType = core.GetType(contract.Item2, true);
                var value = attribute.OwnerElement.Name == "WeaponFlags" ? attribute.Name : attribute.Value;
                if (!Enum.IsDefined(enumType, value)) throw new InvalidOperationException($"Unknown XML enum {contract.Item2}.{value} in {file}");
                count++;
            }
        }
        Console.WriteLine($"PASS ModuleData enums: {count} values checked against installed Core.dll.");
    }
}
