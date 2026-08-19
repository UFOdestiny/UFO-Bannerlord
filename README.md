# UFO's Cheat Mods Bundle For Bannerlord [v1.4.8]

The project is compiled and its module dependencies are declared for Bannerlord v1.4.8.

## Project structure

- `Bootstrap/`: Bannerlord module lifecycle and game-starter configuration.
- `Behaviors/`: campaign behaviors.
- `Patches/`: Harmony patches grouped by campaign, combat, characters, progression, inventory, smithing, and UI.
- `Patching/`: patch discovery and registration.
- `Settings/`, `Localization/`, `Diagnostics/`: configuration, translations, and failure reporting.
- `Extensions/`, `Infrastructure/`, `Models/`: shared game helpers and model replacements.
- `Module/ModuleData/`: game content grouped as ranged ammunition, crafted items, crafting pieces, templates, weapon descriptions, and language resources.

## Harmony compatibility audit

`Tools/HarmonyApiAudit` reads the compiled module and the installed game DLL metadata without starting Bannerlord. It verifies every declared Harmony target and the parameter names used for Harmony binding.

```powershell
dotnet restore Tools/HarmonyApiAudit/HarmonyApiAudit.csproj --configfile Tools/HarmonyApiAudit/NuGet.Config
dotnet run --project Tools/HarmonyApiAudit/HarmonyApiAudit.csproj -- Module/bin/Win64_Shipping_Client/UFO.dll "E:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\bin\Win64_Shipping_Client"
```

## ModuleData audit

`Tools/ModuleDataAudit/Validate-ModuleData.ps1` validates XML syntax, `SubModule.xml` registrations, custom IDs, Native crafting references, and language file registrations against the installed game.

```powershell
./Tools/ModuleDataAudit/Validate-ModuleData.ps1 -GameRoot "E:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord"
```

## Naval DLC MCM tools

When Naval DLC is installed and loaded, the local MCM page includes actions to grant ships, unlock figureheads, and add ship upgrade pieces to the player's current ships. The input fields accept multiple IDs separated by spaces, commas, or semicolons.

- Ship IDs are defined by `Modules/NavalDLC/ModuleData/ship_hulls.xml` (`ShipHull id`).
- Figurehead IDs are defined by `Modules/NavalDLC/AssetPackages/nested_prefabs_packed.xml` (the figurehead prefab/entity name).
- Normal ship upgrade-piece IDs are defined by `Modules/NavalDLC/ModuleData/ship_upgrade_pieces.xml` (`ShipUpgradePiece id`).

The three all-actions grant all normal playable hulls, unlock the complete built-in figurehead catalogue, and add the highest-value normal upgrade available for each ship slot. Story and quest hulls are excluded from the all-ships action.

My original goal was to learn how to make mods. It just so happened that the 'crush through' feature I wanted stopped working, so I decompiled it and combined it with several well-known mods. I noticed that many people were also sad about some wanted features no longer working, so I decided to release this mod directly. 

# Original Mods

I want to give full credit to the original authors of the mods.
- [Bannerlord Cheats Reload](https://www.nexusmods.com/mountandblade2bannerlord/mods/6446)
- [Xorberax's Legacy](https://www.nexusmods.com/mountandblade2bannerlord/mods/3462)
- [Hero Enhancement](https://www.nexusmods.com/mountandblade2bannerlord/mods/4827)
- [招募灭国后的流亡家族 (Recruit Exile Clans)](https://steamcommunity.com/sharedfiles/filedetails/?id=3255329103)
- [Keep Your Daughters](https://www.nexusmods.com/mountandblade2bannerlord/mods/5148)
- [Super Throwing Collection](https://steamcommunity.com/sharedfiles/filedetails/?id=2885230883)
- [loongspear](https://steamcommunity.com/sharedfiles/filedetails/?id=3017866291)
- [拥有《穿透/穿盾/破盾/击倒/爆炸》功能的箭矢 (Super OP Arrows)](https://bbs.mountblade.com.cn/download_1580.html)

# Steam Workshop
- [UFO's Cheat Mods Bundle](https://steamcommunity.com/sharedfiles/filedetails/?id=3583201039)

# Language Support
- Russian: [MaG3ro](https://steamcommunity.com/id/MaG3ro)
