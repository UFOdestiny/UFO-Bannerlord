using System.Reflection;

internal static class ReflectionContracts
{
    // Private and late-bound APIs bypass the C# compiler. Keep these contracts
    // alongside the Harmony audit so an update cannot silently disable a feature.
    internal static int Validate(MetadataLoadContext context, IEnumerable<string> paths, List<string> findings)
    {
        var assemblies = new List<Assembly>();
        foreach (var path in paths)
        {
            try { assemblies.Add(context.LoadFromAssemblyPath(path)); }
            catch (BadImageFormatException) { }
            catch (FileLoadException) { }
        }
        Type? Find(string name) => assemblies.Select(a => a.GetType(name, false)).FirstOrDefault(t => t is not null);
        var count = 0;
        foreach (var (typeName, member, kind, expected) in Contracts)
        {
            if (typeName.StartsWith("NavalDLC.") && !assemblies.Any(a => a.GetName().Name == "NavalDLC")) continue;
            count++;
            var type = Find(typeName);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            MemberInfo? resolved = null;
            for (var current = type; current is not null && resolved is null; current = current.BaseType)
                resolved = current.GetMember(member, kind, flags | BindingFlags.DeclaredOnly).SingleOrDefault();
            if (resolved is null)
            {
                findings.Add($"REFLECTION MISSING: {typeName}.{member}");
                continue;
            }
            var actual = resolved switch
            {
                FieldInfo field => TypeKey(field.FieldType),
                PropertyInfo property => TypeKey(property.PropertyType) + (property.SetMethod is null ? " (read only)" : ""),
                MethodInfo method => string.Join(",", method.GetParameters().Select(p => TypeKey(p.ParameterType))),
                _ => ""
            };
            if (actual != expected) findings.Add($"REFLECTION TYPE: {typeName}.{member} expected '{expected}', found '{actual}'");
        }
        return count;
    }

    private static string TypeKey(Type type) => type.IsGenericType
        ? type.GetGenericTypeDefinition().FullName + "[" + string.Join(",", type.GetGenericArguments().Select(TypeKey)) + "]"
        : type.FullName ?? type.Name;

    private static readonly (string Type, string Member, MemberTypes Kind, string Expected)[] Contracts =
    [
        ("SandBox.GauntletUI.GauntletInventoryScreen", "_dataSource", MemberTypes.Field, "TaleWorlds.CampaignSystem.ViewModelCollection.Inventory.SPInventoryVM"),
        ("SandBox.GauntletUI.GauntletPartyScreen", "_dataSource", MemberTypes.Field, "TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyVM"),
        ("SandBox.GauntletUI.GauntletCharacterDeveloperScreen", "_dataSource", MemberTypes.Field, "TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.CharacterDeveloperVM"),
        ("TaleWorlds.CampaignSystem.ViewModelCollection.Inventory.SPInventoryVM", "_selectedItem", MemberTypes.Field, "TaleWorlds.CampaignSystem.ViewModelCollection.Inventory.SPItemVM"),
        ("TaleWorlds.CampaignSystem.Settlements.Locations.Location", "_characterList", MemberTypes.Field, "System.Collections.Generic.List`1[TaleWorlds.CampaignSystem.Settlements.Locations.LocationCharacter]"),
        ("TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages.EncyclopediaHeroPageVM", "_hero", MemberTypes.Field, "TaleWorlds.CampaignSystem.Hero"),
        ("TaleWorlds.CampaignSystem.Hero", "_defaultAge", MemberTypes.Field, "System.Single"),
        ("TaleWorlds.CampaignSystem.Hero", "_birthDay", MemberTypes.Field, "TaleWorlds.CampaignSystem.CampaignTime"),
        ("TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.WeaponDesignVM", "TrySetSecondaryUsageIndex", MemberTypes.Method, "System.Int32"),
        ("TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.WeaponDesignVM", "RefreshStats", MemberTypes.Method, ""),
        ("TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.WeaponDesignVM", "_craftingBehavior", MemberTypes.Field, "TaleWorlds.CampaignSystem.CampaignBehaviors.ICraftingCampaignBehavior"),
        ("TaleWorlds.Core.WeaponComponentData", "Handling", MemberTypes.Property, "System.Int32"),
        ("TaleWorlds.Core.WeaponComponentData", "SwingDamage", MemberTypes.Property, "System.Int32"),
        ("TaleWorlds.Core.WeaponComponentData", "SwingSpeed", MemberTypes.Property, "System.Int32"),
        ("TaleWorlds.Core.WeaponComponentData", "ThrustDamage", MemberTypes.Property, "System.Int32"),
        ("TaleWorlds.Core.WeaponComponentData", "ThrustSpeed", MemberTypes.Property, "System.Int32"),
        ("NavalDLC.NavalDLCCheats", "AddShipToPlayer", MemberTypes.Method, "System.Collections.Generic.List`1[System.String]"),
        ("NavalDLC.NavalDLCCheats", "UnlockFigurehead", MemberTypes.Method, "System.Collections.Generic.List`1[System.String]"),
        ("NavalDLC.NavalDLCHelpers", "AddUpgradePiecesToPartyShips", MemberTypes.Method, "TaleWorlds.CampaignSystem.Party.MobileParty,System.Collections.Generic.Dictionary`2[System.String,System.String],TaleWorlds.CampaignSystem.Naval.Figurehead")
    ];
}
