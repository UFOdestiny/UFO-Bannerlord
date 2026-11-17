# UFO's Cheat Mods Bundle — Bannerlord v1.5.7

骑马与砍杀 II：霸主单机组合修改器。通过 MCM 配置 Harmony 补丁、战役行为、热键和自定义武器。支持沙盒、剧情战役和自定义战斗；海战 DLC 为可选项。本文件是项目唯一的 README，下面逐项列出全部功能、实际 hook 和修改位置。

## 安装、依赖与配置

本次针对本机 Bannerlord **v1.5.7**、Naval DLC **v1.3.7** 更新。已安装前置：Harmony 模块 v2.4.2.248（程序集 2.4.2.0）、ButterLib 2.12.0、UIExtenderEx 3.0.3、MCM 5.13.1。

将 `dist/UFO-v1.5.7.zip` 中的 `UFO/` 完整覆盖到游戏 `Modules/`，并在启动器中启用所需前置和 UFO。必须同时更新 XML、语言与资源，不能只替换 DLL。这里不包含前置模组的 DLL。

设置位于 MCM 的全局与当前战役页面。战役值**不等于默认值时**覆盖全局值；设回默认值时继续继承全局。因此，本地的 `false` 或默认数字不能覆盖全局已启用的同一设置。带重启标记的选项需重启游戏。既有 MCM 配置 ID 与存档类型 ID 保持不变。

海战赠船、船首像、升级件等按钮需要正在进行的战役和已加载的 DLC。多个 ID 可用空格、逗号、分号分隔。所有船体操作排除命名中含 storyline、quest、burning、nested 的船体；升级件每个槽位选一个。

## 构建、检查和维护

Visual Studio 打开 `UFO.sln`，选择 **Release / x64**；也可使用 .NET SDK。主模块目标为 .NET Framework 4.8 / C# 12，需要 4.8 targeting pack；辅助检查工具使用本机 .NET 10 SDK。

```powershell
# 全量编译、源码/接口/资源检查、刷新本 README 的功能和 hook 索引，并打包
./Tools/CompatibilityAudit/Validate-Compatibility.ps1 -UpdateReadme -Package

# 检查现有 README 是否仍与源码一致；不刷新文档
./Tools/CompatibilityAudit/Validate-Compatibility.ps1

# 单独编译，默认仅写当前项目
dotnet build UFO.csproj -c Release -p:Platform=x64
```

`GameFolder` 默认指向本机 Steam 游戏目录，`WorkshopRoot` 从该路径推导。可覆盖 `HarmonyAssemblyPath`、`ButterLibAssemblyPath`、`UIExtenderExAssemblyPath`、`McmAssemblyPath`，直接引用实际安装的 DLL，不使用旧 NuGet API。显式传入 `-p:DeployToGameFolder=true` 才会复制完整模块到游戏目录；该步骤不先删除目录。

| 目录 / 文件 | 更新时的职责 |
| --- | --- |
| `SubModule.cs`、`Bootstrap/` | 生命周期、分阶段注册与退出时卸载补丁、添加战役行为。 |
| `Patches/`、`Models/MaxAttribute.cs` | 原版 Harmony hook；下方 H 编号索引给出准确目标和实现行。 |
| `Patching/NavalDlcCompatibility.cs`、`OptionalPatchAttribute.cs` | 可选 DLC hook、重载筛选及海战 MCM 操作；缺失目标进入失败清单。 |
| `Behaviors/` | 每日补钱、流亡家族招募；使用 CampaignEvents 和对话入口。 |
| `Settings/` | MCM 页面、默认值、全局与战役取值规则；新增设置同步两个页面和 SettingsManager。 |
| `Localization/`、`Module/bin/Win64_Shipping_Client/*.resx` | MCM 与运行时消息；七份语言表保持同一组键和格式占位符。 |
| `Infrastructure/` | 锻造私有 API 和武器属性存读档。 |
| `Extensions/`、`Diagnostics/` | 玩家/部队归属等公共逻辑和异常报告。 |
| `Module/SubModule.xml`、`Module/ModuleData/`、`Module/AssetPackages/` | 模块依赖、内容注册、翻译和武器资源。 |
| `Tools/SourceAudit/` | 用 Roslyn 检查无引用声明与无用 using，保留框架、接口、反射入口，并导出调用关系。 |
| `Tools/HarmonyApiAudit/` | 核对目标方法、重载、参数/结果/字段类型和私有反射合同；支持类型与方法查询。 |
| `Tools/PatchSmoke/` | 独立 .NET Framework 进程生成真实 Harmony 包装方法；锻造托管回归、XML 枚举检查。 |
| `Tools/ModuleDataAudit/`、`Tools/CompatibilityAudit/` | XML 注册/引用、设置与本地化覆盖、README 同步、完整编译和打包。 |
| `SteamWorkshop/image.png`、`SteamWorkshop/*.xml` | 500×500 预览图和创意工坊发布配置；本地检查不会上传。 |

升级游戏时：先修改 hook 的目标/参数及反射合同，再核对 DLC 是否调用 BaseModel，避免重复应用；同步本地化描述；运行 `-UpdateReadme -Package`。工具日志、逐条目标和 DLL 指纹写入 `obj/compatibility/`，不会创建额外说明文档。

## v1.5.7 的关键适配与清理

- 自动战斗 `SimulateHit` 增加 `BattleEnvironment`；新增船只重载的伤害与乘员伤亡保护。
- 耐力日常治疗迁移到 `Hero.Heal(addXp: true)`；属性上限仅补丁原版 getter，保留海战成长模型链。
- 修复旧存档加载阶段的初始化风险：主菜单与读档前仅启用属性上限 getter；其余游戏功能补丁在 `OnGameInitializationFinished` 注册。`OnGameEnd` 按 UFO 所有者卸载，避免返回菜单后再次读档时遗留补丁。战役设置在 `Campaign.GameStarted` 后读取，加载中使用全局值。
- 恢复玩家/同伴学习倍率和每日驻军增长；海战通用倍率依赖已补丁的 BaseModel，避免重复计算。
- 海战奖励只作用于海战；海战自定义战斗的破防接入独立模型。
- 锻造普通品质也显示加成；各使用模式分别保存，原版重建后恢复绝对值，重复恢复不叠加；新战役清空旧列表。旧存档只有主模式记录时，仅能恢复原来记录的数据。保存类型 ID 57671680、已有字段编号保持不变，使用模式为新增字段 12。
- 百分比回血按最大生命值计算并保留小数。两个旧 IL 注入改为等效的 tick Prefix，删除注入探测和废弃代码。
- 军需官的智力弹药加成也应用于普通士兵；安全识别战役部队，并限制 short 数值以防弹药/盾耐久溢出。异常报告支持一个类上有多个 HarmonyPatch 声明。
- 清除未引用的默认值常量、旧 helper、旧角色切换代码、重复操作符、无用 using 和注释掉的旧实现；保留反射、MCM、存档和 Harmony 所需入口。

## 本地化与龙首槊文字

存在两套语言入口：MCM 的语言设置使用 `*.resx`；游戏物品、锻造模板和使用方式使用 `ModuleData/Languages/`，跟随**游戏语言**。`Other.resx` 是可编辑的英语模板。

本轮补齐日语、葡萄牙语、俄语、西班牙语的海战译文，更新驻军限制和海战奖励说明；启动、暴击、补钱和热键提示纳入语言表。移除已废弃的角色切换提示和无用翻译；CTRL+A 的提示改为配置的属性上限，并补充 CTRL+F/G。七种语言按键、非空文本、Unicode 替换字符和 `{0}` 等格式占位符均受检查。

龙首槊上游来源为 Steam Workshop `3017866291`。`GameText/module_strings` 注册、锻造模板文本和中文“（锻造）龙首槊”已整合；龙首槊、帝皇标枪的使用方式也有对应 GameText。上游资源包与本项目资源相同，保留 UFO 的强化数值。后续更新时一起检查 `SubModule.xml`、`module_strings.xml`、`Languages/CNs/`、模板与武器描述。

## 验证范围

Release x64 编译、213 个启用 DLC 的 hook / 192 个排除 DLC 的 hook、19 / 16 个反射合同、两阶段补丁注册/卸载/二次加载隔离、战役设置加载保护、锻造恢复与跨战役隔离、12 个 ModuleData XML、284 个 XML 枚举值，以及完整设置/本地化覆盖均由全量脚本检查。ZIP 逐文件 SHA-256 比对 Module。

本机旧存档加载曾发生闪退，调整初始化时机后，用户反馈目前恢复正常；2026-11-17 本机运行日志确认读档初始化完成、UFO 补丁注册失败数为 0、武器恢复完成。尚未完成全部功能的游戏内回归。诊断日志位于实际安装目录 `Modules/UFO/bin/Win64_Shipping_Client/UFO-runtime-*.log`，记录加载阶段、补丁失败数和异常；first-chance 异常可能已被游戏处理，不能单凭其存在认定为闪退原因。日志仅写入本地，不自动上传。

这些是源码、托管执行和真实补丁生成检查，**不是游戏内全功能实测**。游戏内仍应回归：新建沙盒/剧情与旧存档、普通/海战自定义战斗、每日经济与城镇增长、招募婚姻与 perk、各界面热键、锻造后读档、龙首槊中文界面、海战赠船与改装、风暴与自动战斗。排除 DLC 测试通过从独立检查进程移除 DLC 程序集实现。

`Module/Video/` 的两个媒体文件是可选的跳过片头素材，当前模块没有自动安装它们的 hook。手动使用时，对应原版 `Modules/Native/Videos/`；使用其他大模组时需核对其实际 Videos 路径。

## 来源与致谢

- [Bannerlord Cheats Reload](https://www.nexusmods.com/mountandblade2bannerlord/mods/6446)
- [Xorberax's Legacy](https://www.nexusmods.com/mountandblade2bannerlord/mods/3462)
- [Hero Enhancement](https://www.nexusmods.com/mountandblade2bannerlord/mods/4827)
- [招募灭国后的流亡家族](https://steamcommunity.com/sharedfiles/filedetails/?id=3255329103)
- [Keep Your Daughters](https://www.nexusmods.com/mountandblade2bannerlord/mods/5148)
- [Super Throwing Collection](https://steamcommunity.com/sharedfiles/filedetails/?id=2885230883)
- [loongspear](https://steamcommunity.com/sharedfiles/filedetails/?id=3017866291)
- [Super OP Arrows](https://bbs.mountblade.com.cn/download_1580.html)
- 原俄语翻译：MaG3ro。
- [UFO 创意工坊页面](https://steamcommunity.com/sharedfiles/filedetails/?id=3583201039)

<!-- BEGIN GENERATED FEATURES -->
## 全部功能与配置

233 个 MCM 展示项；Hook 编号可跳到下方的精确目标、补丁方法和源码位置。调用关系由 Roslyn 语义模型追踪，覆盖共用 helper。

### 战斗 - 玩家

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 总是击晕<br>`AlwaysKnockDown` | 每次攻击始终击晕敌人。 | [H038](#hook-038) |
| 伤害倍数<br>`DamageMultiplier` | 玩家在战斗中造成的伤害的倍数。倍数1表示默认伤害。如果“一击必杀”处于激活状态，则本修改无效。 | [H065](#hook-065) |
| 受伤百分比<br>`DamageTakenPercentage` | 玩家每次受到的攻击会造成多少伤害。100％表示完整伤害，50％表示一半伤害，0％表示玩家无敌。 | [H066](#hook-066) |
| 血量再生<br>`HealthRegeneration` | 玩家在战斗中每秒恢复其总生命值的百分比。0％表示没有再生。 | [H133](#hook-133) |
| 无限弹药<br>`InfiniteAmmo` | 远程和投掷武器弹药无限。 | [H089](#hook-089) |
| 弩矢瞬间装填<br>`InstantCrossbowReload` | 如果启用，弩矢的装填几乎瞬间完成。 | [H091](#hook-091), [H092](#hook-092) |
| 无敌<br>`Invincible` | 玩家无敌，任何攻击都无法对玩家造成伤害。 | [H093](#hook-093) |
| 别打断我！（无硬直）<br>`NeverKnockedBackByAttacks` | 如果启用，你将不会被敌人的攻击打断（无硬直）。 | [H095](#hook-095) |
| 无箭停留<br>`NoStuckArrows` | 如果启用，射在玩家上的箭将不会卡住。 | [H098](#hook-098) |
| 一击必杀<br>`OneHitKill` | 一刀杀死敌人。 | [H102](#hook-102) |
| 玩家坐骑无敌<br>`PlayerHorseInvincible` | 玩家坐骑，任何攻击都无法对玩家坐骑造成伤害。 | [H111](#hook-111) |
| 玩家动量衰减倍率<br>`PlayerMomentumDecayMultiplier` | 仅在玩家近战命中单位时生效。1.00 为游戏默认衰减；0.00 为命中单位后不衰减。不会改变墙体或场景碰撞。 | [H046](#hook-046), [H064](#hook-064) |
| 攻击穿透: 玩家<br>`SliceThroughEveryone` | 玩家每次攻击时都能穿透敌人。 | [H063](#hook-063), [H064](#hook-064) |
| 刺击无法格挡: 玩家<br>`UnblockableThrust_player` | 玩家刺击无法格挡，除非使用盾防御（矛，剑的刺击） | [H094](#hook-094) |

### 常用功能

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 每天加钱阈值<br>`AddMoneyThreshhold` | 每天钱低于这个数字就会加钱 | DailyTickEvent<br>[UFO.Behavior.AddMoney.DailyTick](Behaviors/AddMoney.cs#L14) |
| 每天加钱数目<br>`AddMoney_count` | 每天加这个数目的钱（如果低于加钱阈值） | DailyTickEvent<br>[UFO.Behavior.AddMoney.DailyTick](Behaviors/AddMoney.cs#L14) |
| Language(语言)<br>`LanguageSetting` | 选择设置的语言 | OnBeforeInitialModuleScreenSetAsRoot → L10N.LoadLanguage<br>[L10N.LoadLanguage](Localization/L10N.cs#L13) |
| 作弊商城<br>`NativeItemSpawning` | 启用作弊商城，打开物品栏以查看并获取游戏中的物品。 | [H159](#hook-159) |
| 永远破格挡: 玩家<br>`PlayerAlwaysCrush` | 玩家永远破格挡 | [H046](#hook-046), [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |

### 家族

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 额外家族部队限制<br>`ExtraClanPartyLimit` | 增加家族的部队最大数量。 | [H136](#hook-136) |
| 额外家族部队人数<br>`ExtraClanPartySize` | 增加你的家族部队的最大部队人数。 | [H137](#hook-137) |
| 额外同伴限制<br>`ExtraCompanionLimit` | 增加家族中同伴的最大数量。 | [H138](#hook-138) |
| 女性结婚留在家族<br>`KeepDaughter` | 玩家家族的女性结婚后会留在玩家的家族中 | [H152](#hook-152) |

### 常规

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 启用热键提示<br>`EnableHotkeyTips` | 可在任何能够使用热键的界面，查看可用的作弊热键列表。 | [H130](#hook-130), [H131](#hook-131) |
| 启用热键<br>`EnableHotkeys` | 在仓库界面，部队界面，家族界面等中启用作弊热键。 | [H130](#hook-130), [H131](#hook-131), [H132](#hook-132), [H133](#hook-133) |
| 属性点上限<br>`MaxAttr` | 六项基础属性点的上限，默认 10。 | [H029](#hook-029) |

### 地图

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 隐形商队<br>`CaravansInvisibleOnMap` | 敌方部队不会追踪或者攻击玩家的商队。 | [H036](#hook-036) |
| 地图速度倍数<br>`MapSpeedMultiplier` | 玩家在地图上速度的倍数。倍数1表示默认速度。 | [H155](#hook-155) |
| 地图视野倍数<br>`MapVisibilityMultiplier` | 玩家在地图上的视野倍数。倍数1表示默认视野。警告：会对性能产生负面影响！ | [H156](#hook-156) |
| NPC部队速度<br>`NpcMapSpeedPercentage` | 地图上NPC部队速度降低至一定百分比。100％表示默认速度，50％表示半速，0％表示不能移动。 | [H168](#hook-168) |
| 隐形部队<br>`PartyInvisibleOnMap` | 敌方不会在地图上追踪或攻击玩家部队。 | [H174](#hook-174) |

### 战斗 - 部队

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 同伴被击晕或被杀死<br>`CompanionsKnockoutOrKilled` | 你部队的同伴，在战败后，要么总是被击晕，要么总是被杀死，取决于设置。该选项默认为被击晕概率。 | [H060](#hook-060), [H061](#hook-061), [H062](#hook-062) |
| 没有友军伤害<br>`NoFriendlyFire` | 完全禁用玩家部队的友军误伤。 | [H096](#hook-096) |
| 永不逃跑<br>`NoRunningAway` | 玩家部队在战斗中不会逃跑。 | [H097](#hook-097) |
| 部队伤害倍数<br>`PartyDamageMultiplier` | 玩家部队在战斗中造成的伤害的倍数。倍数1表示默认伤害。如果“一击必杀”处于激活状态，则本修改无效。 | [H103](#hook-103) |
| 部队血量再生<br>`PartyHealthRegeneration` | 玩家部队在战斗中每秒恢复其总生命值的百分比。0％表示没有再生。 | [H133](#hook-133) |
| 部队英雄无敌<br>`PartyHeroesInvincible` | 玩家部队英雄无敌，任何攻击都无法对玩家部队英雄造成伤害。不影响普通士兵。 | [H104](#hook-104) |
| 部队无限弹药<br>`PartyInfiniteAmmo` | 玩家部队远程和投掷武器弹药无限。 | [H105](#hook-105) |
| 部队无敌<br>`PartyInvincible` | 玩家部队无敌，任何攻击都无法对玩家部队造成伤害。 | [H106](#hook-106) |
| 玩家士兵被击晕或被杀死<br>`PartyKnockoutOrKilled` | 你部队的士兵，在战败后，要么总是被击晕，要么总是被杀死，取决于设置。该选项默认为被击晕概率。 | [H107](#hook-107), [H108](#hook-108), [H109](#hook-109) |
| 部队一击必杀<br>`PartyOneHitKill` | 玩家部队一刀杀死敌人。 | [H110](#hook-110) |
| 永远破格挡: 己方英雄与士兵<br>`PlayerPartyCrush` | 仅作用于玩家本人及玩家主队中、与玩家同一战斗阵营的英雄和士兵；不影响参战友军。 | [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 攻击穿透: 己方英雄与士兵<br>`PlayerPartySliceThroughEveryone` | 仅作用于玩家本人及玩家主队中、与玩家同一战斗阵营的英雄和士兵；所有有效攻击都会尝试穿透，不影响参战友军。 | [H063](#hook-063), [H064](#hook-064) |
| 刺击无法格挡: 友军<br>`UnblockableThrust_ally` | 友军刺击无法格挡，除非使用盾防御（矛，剑的刺击） | [H094](#hook-094) |

### 战斗 - 盟友

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 友方领主被击晕或被杀死<br>`FriendlyLordsKnockoutOrKilled` | 友方领主，在战败后，要么总是被击晕，要么总是被杀死，取决于设置。该选项默认为被击晕概率。 | [H085](#hook-085), [H086](#hook-086), [H087](#hook-087) |

### 战斗 - 敌人

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 敌人永不逃跑<br>`EnemiesNoRunningAway` | 敌人在战斗中不会逃跑。 | [H041](#hook-041), [H042](#hook-042), [H043](#hook-043), [H044](#hook-044), [H071](#hook-071), [H072](#hook-072), [H073](#hook-073), [H074](#hook-074), [H078](#hook-078), [H079](#hook-079), [H080](#hook-080), [H081](#hook-081), [H088](#hook-088) |
| 永远破格挡: 敌人<br>`EnemyCrush` | 敌人永远破格挡 | [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 敌人伤害百分比<br>`EnemyDamagePercentage` | 在战斗中敌人对玩家造成伤害的百分比。100%为满伤害。 | [H070](#hook-070) |
| 敌方领主被击晕或被杀死<br>`EnemyLordsKnockoutOrKilled` | 敌方领主，在战败后，要么总是被击晕，要么总是被杀死，取决于设置。该选项默认为被击晕概率。 | [H075](#hook-075), [H076](#hook-076), [H077](#hook-077) |
| 敌方士兵被击晕或被杀死<br>`EnemyTroopsKnockoutOrKilled` | 敌方部队的士兵，在战败后，要么总是被击晕，要么总是被杀死，取决于设置。该选项默认为被击晕概率。 | [H082](#hook-082), [H083](#hook-083), [H084](#hook-084) |
| 攻击穿透: 敌人<br>`SliceThroughEveryone_enemy` | 敌人每次攻击时都能穿透敌人。 | [H063](#hook-063), [H064](#hook-064) |
| 刺击无法格挡: 敌人<br>`UnblockableThrust_enemy` | 敌人刺击无法格挡，除非使用盾防御（矛，剑的刺击） | [H094](#hook-094) |

### 战斗 - 杂项

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 坐镇指挥必胜<br>`AlwaysWinBattleSimulation` | 坐镇指挥时，敌人不会对玩家部队造成伤害。 | [H039](#hook-039), [H202](#hook-202) |
| 进攻藏身处额外部队<br>`BanditHideoutTroopLimit` | 进攻藏身处是，可加入玩家进攻的额外部队。值0表示默认部队限制。 | [H040](#hook-040) |
| 战场缩放倍数<br>`CombatZoomMultiplier` | 在战斗中战场面积缩放倍数。 1表示默认缩放。 | [H045](#hook-045) |
| 影响力奖励倍数<br>`InfluenceRewardMultiplier` | 赢得战斗后，获得的影响力奖励的倍数。倍数1表示默认影响力。 | [H090](#hook-090) |
| 没有部队牺牲<br>`NoTroopSacrifice` | 逃离、突入、突围时不会牺牲部队。 | [H099](#hook-099), [H100](#hook-100), [H101](#hook-101) |
| 声望奖励倍数<br>`RenownRewardMultiplier` | 赢得战斗或者竞技大赛后，获得的声望奖励的倍数。倍数1表示默认声望。 | [H114](#hook-114), [H115](#hook-115) |

### 物品栏

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 额外物品栏<br>`ExtraInventoryCapacity` | 玩家部队的物品栏容量。 | [H139](#hook-139) |

### 部队

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 推翻决策影响力花费百分比<br>`DecisionOverrideInfluenceCostPercentage` | 推翻王国决策所需花费的影响力。100％表示完整影响力，0％表示无需影响力。 | [H128](#hook-128) |
| 额外部队人数<br>`ExtraPartyMemberSize` | 增加玩家部队的最大规模。 | [H137](#hook-137) |
| 额外部队士气<br>`ExtraPartyMorale` | 增加额外的部队士气。 | [H140](#hook-140) |
| 额外部队俘虏人数<br>`ExtraPartyPrisonerSize` | 增加部队可押送的最大俘虏人数。 | [H141](#hook-141) |
| 食物消耗百分比<br>`FoodConsumptionPercentage` | 玩家部队需要多少食物。100％表示完整食物消费，0％表示没有食物消费。 | [H143](#hook-143) |
| 免费同伴招聘<br>`FreeCompanionHiring` | 玩家同伴招募是免费的。 | [H144](#hook-144) |
| 免费部队升级<br>`FreeTroopUpgrades` | 玩家部队升级是免费的。 | [H147](#hook-147) |
| 瞬间逃跑<br>`InstantEscape` | 玩家被另一方抓住后立即逃跑。 | [H149](#hook-149) |
| 立即招募俘虏<br>`InstantPrisonerRecruitment` | 可以立即招募俘虏。 | [H150](#hook-150) |
| 俘虏永不逃跑<br>`NoPrisonerEscape` | 俘虏不能逃离部队。 | [H163](#hook-163) |
| 部队治疗速度倍数<br>`PartyHealingMultiplier` | 玩家部队治疗速度的倍数。倍数1表示默认速度。 | [H172](#hook-172), [H173](#hook-173) |
| 部队工资百分比<br>`TroopWagesPercentage` | 玩家需要支付部队多少工资。100％表示全额工资，0％表示没有工资。 | [H187](#hook-187) |

### 角色

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 调整怀孕时间<br>`AdjustPregnancyDuration` | 调整游戏中所有角色的怀孕时间。怀孕的默认时长是36天. | [H032](#hook-032) |
| 允许同性结婚<br>`AllowSameSexMarriage` | 如果启用，你可以与性别相同的角色结婚. | [H033](#hook-033) |
| 谈判提案始终成功<br>`BarterOfferAlwaysAccepted` | 谈判提案始终被其他角色接受。 | [H034](#hook-034) |
| 对话总是成功<br>`ConversationAlwaysSuccessful` | 对话和说服总是成功。 | [H035](#hook-035) |
| 永不老死<br>`NeverDieOfOldAge` | 如果启用，主角依然会变老生病，但不会因此而死亡。 | [H158](#hook-158) |
| 没有谈判冷却<br>`NoBarterCooldown` | 谈判没有冷却时间。 | [H157](#hook-157) |
| 魅力非凡<br>`PerfectAttraction` | 如果启用，你的潜在配偶将被你100%吸引。 | [H169](#hook-169) |
| 完美人际关系<br>`PerfectRelationships` | 玩家和所有角色的关系都设置为100。 | [H171](#hook-171) |
| 怀孕概率倍数<br>`PregnancyChanceMultiplier` | 玩家角色或玩家配偶怀孕概率的倍数。不会影响无法怀孕的角色！倍数1表示默认怀孕概率。 | [H170](#hook-170) |
| 战后好感增益倍数<br>`RelationGainAfterBattleMultiplier` | 受助领主战后，对你的好感增益倍数。默认为1。 | [H175](#hook-175) |

### 王国

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 王国投票权重倍数<br>`KingdomDecisionWeightMultiplier` | 玩家对王国决策投票的权重倍数，换句话说就是决策通过的可能性。 | [H153](#hook-153) |
| 无法无天<br>`NoCrimeRatingForCrimes` | 非法行为不会导致犯罪等级。 | [H162](#hook-162) |
| 决策无关系损失<br>`NoRelationshipLossOnDecision` | 不会损失决策反对方的关系。 | [H164](#hook-164), [H165](#hook-165), [H166](#hook-166) |
| 招募流亡家族<br>`RecruitExileClans` | 允许在对话中邀请未加入王国、非雇佣兵的家族领袖加入玩家所属王国。默认开启。 | OnSessionLaunchedEvent → AddPlayerLine 招募对话条件<br>[UFO.Behavior.RecruitExileClan.checkcondition](Behaviors/RecruitExileClan.cs#L25) |

### 经验

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 家族成员经验倍数<br>`ClanExperienceMultiplier` | 玩家家族成员的经验增益倍数。默认为1。 | [H037](#hook-037) |
| 同伴经验倍数<br>`CompanionExperienceMultiplier` | 同伴经验获取的倍数。倍数1表示默认经验。 | [H116](#hook-116) |
| 同伴学习效率倍数<br>`CompanionLearningRateMultiplier` | 玩家同伴技能学习效率的倍数，1 表示默认效率。 | [H148](#hook-148) |
| 经验倍数<br>`ExperienceMultiplier` | 玩家经验获取的倍数。倍数1表示默认经验。 | [H135](#hook-135) |
| 专精点自由分配<br>`FreeFocusPointAssignment` | 技能分配专精等级不会花费专精点。 | [H145](#hook-145) |
| 学习上限倍数<br>`LearningLimitMultiplier` | 所有技能学习上限的倍数。倍数1表示默认学习上限。 | [H154](#hook-154) |
| 学习效率倍数<br>`LearningRateMultiplier` | 玩家学习效率的倍数。倍数1表示默认效率。 | [H148](#hook-148) |
| 部队经验倍数<br>`TroopExperienceMultiplier` | 玩家部队经验的倍数。倍数1表示默认经验。 | [H186](#hook-186) |

### 围城

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 敌方围城器械建造速度百分比<br>`EnemySiegeBuildingSpeedPercentage` | 围攻时敌方可以多快建造围城器械。100％表示全速，0％表示不可建造。 | [H134](#hook-134) |
| 围城器械建造速度倍数<br>`SiegeBuildingSpeedMultiplier` | 攻城一方的围城器械建造速度的倍数。倍数1表示默认速度。 | [H178](#hook-178) |

### 军队

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 军团凝聚力损失百分比<br>`ArmyCohesionLossPercentage` | 玩家加入的军团损失多少凝聚力。100％表示完整凝聚力损失，0％表示凝聚力被冻结。 | [H030](#hook-030) |
| 食物消费百分比<br>`ArmyFoodConsumptionPercentage` | 玩家加入的军团需要多少食物。100％表示完整食物消费，0％表示没有食物消费。 | [H031](#hook-031) |
| 派系军团凝聚力损失百分比<br>`FactionArmyCohesionLossPercentage` | 玩家派系的军团损失多少凝聚力。100％表示完整凝聚力损失，0％表示凝聚力被冻结。 | [H142](#hook-142) |

### 定居点

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 建造速度倍数<br>`ConstructionPowerMultiplier` | 玩家城镇建建造速度的倍数。倍数1表示默认建造速度。 | [H117](#hook-117) |
| 每日食物奖励<br>`DailyFoodBonus` | 玩家城堡和城镇的食物库存每日按照一定数值增长。 | [H120](#hook-120) |
| 每日驻军奖励<br>`DailyGarrisonBonus` | 玩家城镇和城堡的每日驻军增长增加指定数值，仍受原版容量与事件条件限制。 | [H121](#hook-121) |
| 每日户数奖励<br>`DailyHearthsBonus` | 玩家村庄的户数每日按照一定数值增长。 | [H122](#hook-122) |
| 每日忠诚度奖励<br>`DailyLoyaltyBonus` | 玩家城堡和城镇的忠诚度每日按照一定数值增长。 | [H123](#hook-123) |
| 每日民兵额外奖励<br>`DailyMilitiaBonus` | 玩家村庄和城镇的民兵每日按照一定数值增长。 | [H124](#hook-124), [H125](#hook-125) |
| 每日繁荣度奖励<br>`DailyProsperityBonus` | 玩家城堡和城镇的繁荣度每日按照一定数值增长。 | [H126](#hook-126) |
| 每日治安度奖励<br>`DailySecurityBonus` | 玩家城堡和城镇的治安度每日按照一定数值增长。 | [H127](#hook-127) |
| 伪装永远有效<br>`DisguiseAlwaysWorks` | 伪装潜入城镇永远成功。 | [H129](#hook-129) |
| 免费招募<br>`FreeTroopRecruitment` | 免费招募自定居的新兵到部队。 | [H146](#hook-146) |
| 驻军工资百分比<br>`GarrisonWagesPercentage` | 玩家需要支付驻军多少工资。100％表示全额工资，0％表示没有工资。 | [H187](#hook-187) |
| 商品购买价格百分比<br>`ItemTradingCostPercentage` | 商品需要花多少钱才能买到。100％表示全价，50％表示半价，0％表示全部免费。 | [H151](#hook-151) |
| 无需日常装备<br>`NeverRequireCivilianEquipment` | 穿着战斗装备进入任何场景。 | [H160](#hook-160) |
| 无需贿赂<br>`NoBribeToEnterKeep` | 如果启用，则无需贿赂即可进入城堡内庭。 | [H161](#hook-161) |
| 销售价格倍数<br>`SellingPriceMultiplier` | 玩家出售物品的价格倍数。倍数1表示默认价格。 | [H177](#hook-177) |
| 定居点永不叛变<br>`SettlementsNeverRebel` | 如果启用，玩家所拥有的的定居点永不叛变。 | [H176](#hook-176) |
| 竞技大赛最大投注的倍数<br>`TournamentMaximumBetMultiplier` | 玩家能在竞技大赛下注的最大金额的倍数。1代表默认倍数。 | [H184](#hook-184) |
| 城镇起始资金增加(千)<br>`Town_Init_Gold_Extra` | 城镇起始资金增加(千)（只对新战役生效） | [H185](#hook-185) |
| 村庄起始资金增加(千)<br>`Village_Init_Gold_Extra` | 村庄起始资金增加(千)（只对新战役生效） | [H189](#hook-189) |
| 玩家村庄永不被掠夺<br>`VillagesNeverRaided` | 如果启用，玩家的村庄永远不会被敌方领主烧村。 | [H190](#hook-190) |
| 名人-工匠增加<br>`xArt` | 每个定居点名人-工匠增加数量（只对新战役生效） | [H167](#hook-167) |
| 名人-帮派头子增加<br>`xGang` | 每个定居点名人-帮派头子增加数量（只对新战役生效） | [H167](#hook-167) |
| 名人-商人增加<br>`xMerch` | 每个定居点名人-商人增加数量（只对新战役生效） | [H167](#hook-167) |
| 名人-头人增加<br>`xRural` | 每个定居点名人-头人增加数量（只对新战役生效） | [H167](#hook-167) |
| 名人-村庄贵族增加<br>`xVill` | 每个定居点名人-村庄贵族增加数量（只对新战役生效） | [H167](#hook-167) |

### 打铁

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 打造武器操控性加成<br>`CraftedWeaponHandlingBonus` | 玩家打造的任何武器，其操控性的加成。 | [H118](#hook-118), [H119](#hook-119) |
| 打造武器挥砍伤害加成<br>`CraftedWeaponSwingDamageBonus` | 玩家打造的任何武器，其挥砍伤害的加成。 | [H118](#hook-118), [H119](#hook-119) |
| 打造武器挥击速度加成<br>`CraftedWeaponSwingSpeedBonus` | 玩家打造的任何武器，其挥击速度的加成。 | [H118](#hook-118), [H119](#hook-119) |
| 打造武器穿刺伤害加成<br>`CraftedWeaponThrustDamageBonus` | 玩家打造的任何武器，其穿刺伤害的加成。 | [H118](#hook-118), [H119](#hook-119) |
| 打造武器刺击速度加成<br>`CraftedWeaponThrustSpeedBonus` | 玩家打造的任何武器，其刺击速度的加成。 | [H118](#hook-118), [H119](#hook-119) |
| 材料消耗百分比<br>`SmithingCostPercentage` | 制作武器需要多少材料。100％表示完整材料消耗，0％表示没有材料消耗。 | [H179](#hook-179) |
| 难度百分比<br>`SmithingDifficultyPercentage` | 武器设计有多难。100％表示完整难度，0％表示没有困难。 | [H180](#hook-180) |
| 耐力消耗百分比<br>`SmithingEnergyCostPercentage` | 锻造、熔炼、精炼消耗多少耐力。100％表示完整耐力消耗，0％表示没有耐力消耗。 | [H181](#hook-181), [H182](#hook-182), [H183](#hook-183) |
| 解锁所有零件<br>`UnlockAllParts` | 所有锻造零件均已解锁。 | [H188](#hook-188) |

### 工坊

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 购价百分比<br>`WorkshopBuyingCostPercentage` | 购买工厂需要多少钱。100％表示全价，0％表示免费。 | [H191](#hook-191) |
| 每日开支百分比<br>`WorkshopDailyExpensePercentage` | 工坊每日开支的百分比。100％表示完整费用，0％表示没有费用。 | [H192](#hook-192) |
| 售价倍数<br>`WorkshopSellingCostMultiplier` | 出售工坊赚得的倍数。倍数1表示默认售价。 | [H193](#hook-193) |

### 总体增强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 自动获得两个分支的技能<br>`AutoChoosePerk` | 需要技能等级满足要求 | [H002](#hook-002), [H006](#hook-006) |
| 战斗增强系数-玩家家族成员<br>`CombatAttributeRateClanMember` | 战斗加强效果会乘以这个系数，负面则除以这个值 | [H047](#hook-047), [H048](#hook-048), [H049](#hook-049), [H050](#hook-050), [H051](#hook-051), [H052](#hook-052), [H053](#hook-053), [H054](#hook-054), [H055](#hook-055), [H057](#hook-057), [H058](#hook-058), [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 战斗增强系数-其他人<br>`CombatAttributeRateOther` | 战斗加强效果会乘以这个系数，负面则除以这个值 | [H047](#hook-047), [H048](#hook-048), [H049](#hook-049), [H050](#hook-050), [H051](#hook-051), [H052](#hook-052), [H053](#hook-053), [H054](#hook-054), [H055](#hook-055), [H057](#hook-057), [H058](#hook-058), [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 战斗增强系数-玩家<br>`CombatAttributeRatePlayer` | 战斗加强效果会乘以这个系数，负面则除以这个值 | [H047](#hook-047), [H048](#hook-048), [H049](#hook-049), [H050](#hook-050), [H051](#hook-051), [H052](#hook-052), [H053](#hook-053), [H054](#hook-054), [H055](#hook-055), [H057](#hook-057), [H058](#hook-058), [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 开启长生不老<br>`EnableEverYoung` | 玩家无视技能等级要求直接长生不老，关闭这个可以关闭所有长生不老相关功能，遇到其他MOD冲突可以试试关这个 | [H001](#hook-001), [H003](#hook-003), [H004](#hook-004), [H005](#hook-005), [H007](#hook-007), [H008](#hook-008) |
| 家族长生不老技能等级(跑动+魅力)需求<br>`EverYoungSkillNeed` | 家族成员的跑动+魅力如果大于或等于这个值将获得长生不老能力 | [H001](#hook-001) |
| 战略增强系数-玩家家族成员<br>`StrategyAttributeRateClanMember` | 战略加强效果会乘以这个系数，负面则除以这个值 | [H009](#hook-009), [H010](#hook-010), [H011](#hook-011), [H012](#hook-012), [H013](#hook-013), [H014](#hook-014), [H015](#hook-015), [H016](#hook-016), [H017](#hook-017), [H018](#hook-018), [H019](#hook-019), [H020](#hook-020), [H021](#hook-021), [H022](#hook-022), [H023](#hook-023), [H024](#hook-024), [H025](#hook-025), [H026](#hook-026), [H027](#hook-027), [H028](#hook-028) |
| 战略增强系数-其他人<br>`StrategyAttributeRateOther` | 战略加强效果会乘以这个系数，负面则除以这个值 | [H009](#hook-009), [H010](#hook-010), [H011](#hook-011), [H012](#hook-012), [H013](#hook-013), [H014](#hook-014), [H015](#hook-015), [H016](#hook-016), [H017](#hook-017), [H018](#hook-018), [H019](#hook-019), [H020](#hook-020), [H021](#hook-021), [H022](#hook-022), [H023](#hook-023), [H024](#hook-024), [H025](#hook-025), [H026](#hook-026), [H027](#hook-027), [H028](#hook-028) |
| 战略增强系数-玩家<br>`StrategyAttributeRatePlayer` | 战略加强效果会乘以这个系数，负面则除以这个值 | [H009](#hook-009), [H010](#hook-010), [H011](#hook-011), [H012](#hook-012), [H013](#hook-013), [H014](#hook-014), [H015](#hook-015), [H016](#hook-016), [H017](#hook-017), [H018](#hook-018), [H019](#hook-019), [H020](#hook-020), [H021](#hook-021), [H022](#hook-022), [H023](#hook-023), [H024](#hook-024), [H025](#hook-025), [H026](#hook-026), [H027](#hook-027), [H028](#hook-028) |
| 测试模式<br>`TestMode` | 所有人在战斗中无限血量 | [H010](#hook-010), [H056](#hook-056), [H059](#hook-059) |

### 活力加强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 活力护甲加成<br>`VigorArmorAdd` | 每点活力增加的护甲值 | [H057](#hook-057), [H058](#hook-058) |
| 活力抵御破格挡概率<br>`VigorCrushThroughNegative` | 每超过对手1点活力增加的抵御破格挡概率 | [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 活力破格挡概率<br>`VigorCrushThroughPositive` | 每超过对手1点活力增加的破格挡概率 | [H067](#hook-067), [H068](#hook-068), [H069](#hook-069) |
| 活力伤害加成百分比<br>`VigorDmgPercent` | 每点活力增加的伤害百分比 | [H054](#hook-054) |
| 活力伤害减免<br>`VigorDmgTakenReduce` | 每点活力减免来自敌人的伤害值 | [H054](#hook-054) |
| 活力强制伤害加成<br>`VigorFinalDmgAdd` | 每点活力增加的对敌人的强制伤害值 | [H054](#hook-054) |
| 活力盾牌强化<br>`VigorShieldEndurancePercent` | 每点活力增加的盾牌耐久百分比 | [H051](#hook-051) |

### 控制加强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 控制瞄准稳定性加成<br>`ControlAimStabilityPercent` | 每点控制增加的瞄准稳定性百分比 | [H057](#hook-057) |
| 控制弹药免耗概率<br>`ControlAmmoNoConsumeRate` | 每点控制使得每次射击有概率不会消耗弹药 | [H053](#hook-053) |
| 控制暴击概率<br>`ControlCritRate` | 每超过对手1点控制增加的暴击(双倍伤害)概率 | [H054](#hook-054) |
| 控制跌落伤害减免<br>`ControlDropDmgReducePercent` | 每点控制减免的跌落伤害百分比 | [H054](#hook-054) |
| 控制豁免概率<br>`ControlExemptionRate` | 每超过对手1点控制增加的豁免(完全免伤)概率 | [H054](#hook-054) |
| 控制坐骑操控性加成<br>`ControlMountManeuverPercent` | 每点控制增加的坐骑操控性百分比 | [H058](#hook-058) |
| 控制远程穿盾概率<br>`ControlPenetrateRate` | 每点控制增加的远程穿盾概率 | [H049](#hook-049) |

### 智力加强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 智力弹药量加成<br>`IntelligenceAmmoAddPercent` | 每点智力增加的队伍弹药上限百分比(军需官) | [H051](#hook-051) |
| 智力攻守城弩炮加成<br>`IntelligenceBallistaAdd` | 每点智力增加攻守城时初始弩炮数量(进攻方的工程师，或者守城方总督) | [H025](#hook-025), [H026](#hook-026) |
| 智力阈值<br>`IntelligenceBoundary` | 设置智力效果计算阈值 | [H016](#hook-016), [H024](#hook-024), [H028](#hook-028) |
| 智力经验加成<br>`IntelligenceExpRate` | 每点智力增加的经验获取率，对狡诈，社交，智力系技能效果翻倍 | [H010](#hook-010), [H019](#hook-019) |
| 智力守军工资加成<br>`IntelligenceGarrisonWageReducePercent` | 每点智力降低守军薪水 | [H028](#hook-028) |
| 智力定居点食物加成-总督<br>`IntelligenceGovernorSettlementFoodPercent` | 每点智力增加定居点食物产出 | [H016](#hook-016) |
| 智力定居点食物加成-家族领袖<br>`IntelligenceLeaderSettlementFoodPercent` | 每点智力增加定居点食物产出 | [H016](#hook-016) |
| 智力繁荣度消耗食物加成<br>`IntelligenceProsperityFoodCostReducePercent` | 每点智力降低因为繁荣度消耗的食物百分比 | [H016](#hook-016) |
| 智力攻城武器耐久加成<br>`IntelligenceSiegeEndurancePercent` | 每点智力增加的攻城武器耐久百分比(工程师) | [H027](#hook-027) |
| 智力城墙耐久加成<br>`IntelligenceWallEndurancePercent` | 每点智力增加的城墙耐久百分比(总督) | [H014](#hook-014) |
| 智力工坊产量加成<br>`IntelligenceWorkshopProductionPercent` | 每点智力增加的工坊产量百分比(拥有者) | [H024](#hook-024) |

### 耐力加强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 耐力生命恢复率加成<br>`EnduranceHealRate` | 每点耐力增加的生命恢复(大地图)百分比 | [H050](#hook-050) |
| 耐力生命值加成<br>`EnduranceHpAddPercent` | 每点耐力增加的生命上限百分比 | [H052](#hook-052) |
| 耐力骑马速度加成<br>`EnduranceMountSpeedPercent` | 每点耐力增加的骑马速度百分比 | [H058](#hook-058) |
| 耐力抗硬直加成<br>`EnduranceStaggerPercent` | 每点耐力增加的抗硬直百分比 | [H048](#hook-048) |
| 耐力步行速度加成<br>`EnduranceWalkSpeedPercent` | 每点耐力增加的步行速度百分比 | [H057](#hook-057) |

### 狡诈加强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 狡诈同伴上限加成<br>`CunningCompanionCapacityAdd` | 每点狡诈增加的同伴上限(家族领袖) | [H020](#hook-020) |
| 狡诈队伍速度加成<br>`CunningPartySpeedAdd` | 每点狡诈增加的队伍速度(斥候) | [H011](#hook-011) |
| 狡诈俘虏上限加成<br>`CunningPrisonerCapacityPercent` | 每点狡诈增加的俘虏上限(部队指挥官) | [H009](#hook-009) |
| 狡诈招募俘虏速度加成<br>`CunningPrisonerRecruitSpeedPercent` | 每点狡诈增加的俘虏招募速度百分比(部队指挥官) | [H021](#hook-021) |
| 狡诈劫掠速度加成<br>`CunningRaidSpeedPercent` | 每点狡诈增加的劫掠速度百分比(部队指挥官) | [H013](#hook-013) |

### 社交加强

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 社交阈值<br>`SocialBoundary` | 设置社交效果计算阈值 | [H012](#hook-012), [H015](#hook-015), [H017](#hook-017), [H018](#hook-018), [H022](#hook-022), [H024](#hook-024) |
| 社交同伴上限加成<br>`SocialCompanionCapacityAdd` | 每点社交增加的同伴上限(家族领袖) | [H020](#hook-020) |
| 社交定居点户数增长加成<br>`SocialHearthAdd` | 每点社交增加的定居点户数增长值(家族领袖和总督) | [H012](#hook-012) |
| 社交定居点民兵加成<br>`SocialMilitiaAdd` | 每点社交增加的定居点民兵(家族领袖和总督) | [H015](#hook-015) |
| 社交招募池刷新速度<br>`SocialRecruitSpeedPercent` | 每点社交增加的定居点招募池刷新速度(家族领袖和总督) | [H022](#hook-022) |
| 社交定居点忠诚度加成<br>`SocialSettlementLoyaltyAdd` | 每点社交增加的定居点忠诚度(家族领袖和总督) | [H018](#hook-018) |
| 社交税收加成<br>`SocialTaxPercent` | 每点社交增加的定居点税收(总督) | [H017](#hook-017) |
| 社交工坊产量加成<br>`SocialWorkshopProductionPercent` | 每单社交增加的工坊产量百分比(总督) | [H024](#hook-024) |

### 弓弩 - 玩家主队

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 加速度精度惩罚<br>`PlayerPartyRangedAccelerationPenaltyPercentage` | 调整 WeaponExternalAccelerationAccuracyPenalty。数值越低，受外部加速度的惩罚越小。 | [H113](#hook-113) |
| AI 水平瞄准误差<br>`PlayerPartyRangedAiHorizontalErrorPercentage` | 调整 AI 水平方向的瞄准误差。数值越低，射击越精准。 | [H113](#hook-113) |
| AI 运动目标提前量误差<br>`PlayerPartyRangedAiLeadErrorPercentage` | 调整 AI 对移动目标的提前量最小与最大误差。数值越低，对移动预判越准确。 | [H113](#hook-113) |
| AI 基础瞄准误差<br>`PlayerPartyRangedAiShooterErrorPercentage` | 调整主队弓弩兵的 AI 基础射击误差。数值越低，射击越精准。 | [H113](#hook-113) |
| AI 垂直瞄准误差<br>`PlayerPartyRangedAiVerticalErrorPercentage` | 调整 AI 垂直方向的瞄准误差。数值越低，射击越精准。 | [H113](#hook-113) |
| 最佳精度等待时间<br>`PlayerPartyRangedBestAccuracyWaitPercentage` | 调整 WeaponBestAccuracyWaitTime。数值越低，越快达到最高精度。 | [H113](#hook-113) |
| 移动精度惩罚<br>`PlayerPartyRangedMovementPenaltyPercentage` | 调整 WeaponMaxMovementAccuracyPenalty。数值越低，移动时的散布惩罚越小。 | [H113](#hook-113) |
| 转向精度惩罚<br>`PlayerPartyRangedRotationPenaltyPercentage` | 调整 WeaponRotationalAccuracyPenaltyInRadians。数值越低，转向时的惩罚越小。 | [H113](#hook-113) |
| 瞄准不稳惩罚<br>`PlayerPartyRangedUnsteadyPenaltyPercentage` | 调整 WeaponMaxUnsteadyAccuracyPenalty。数值越低，瞄准不稳时的惩罚越小。 | [H113](#hook-113) |

### 弓弩 - 射击节奏

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| AI 射击间隔<br>`PlayerPartyRangedAiShootIntervalPercentage` | 调整 AIWaitBeforeShootFactor。数值越低，AI 士兵射击越频繁。 | [H113](#hook-113) |

### 近战 - 玩家主队

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 武器操控<br>`PlayerPartyMeleeHandlingMultiplier` | 调整主队近战兵的 HandlingMultiplier。1.000 为原版数值。 | [H112](#hook-112) |
| 踢击眩晕时长<br>`PlayerPartyMeleeKickStunMultiplier` | 调整主队近战兵的 KickStunDurationMultiplier。1.000 为原版数值。 | [H112](#hook-112) |
| 副手防御速度<br>`PlayerPartyMeleeOffhandDefenseSpeedMultiplier` | 近战武器装备时调整 OffhandWeaponDefendSpeedMultiplier。1.000 为原版数值。 | [H112](#hook-112) |
| 盾击眩晕时长<br>`PlayerPartyMeleeShieldBashStunMultiplier` | 调整主队近战兵的 ShieldBashStunDurationMultiplier。1.000 为原版数值。 | [H112](#hook-112) |
| 挥砍速度<br>`PlayerPartyMeleeSwingSpeedMultiplier` | 调整主队近战兵的 SwingSpeedMultiplier。1.000 为原版数值。 | [H112](#hook-112) |
| 刺击准备速度<br>`PlayerPartyMeleeThrustReadySpeedMultiplier` | 仅在使用近战武器时调整 ThrustOrRangedReadySpeedMultiplier。1.000 为原版数值。 | [H112](#hook-112) |

### 近战 - AI 防御

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| AI 格挡判断<br>`PlayerPartyMeleeAiBlockDecisionMultiplier` | 调整主队 AI 近战兵的 AIBlockOnDecideAbility。1.000 为原版数值。 | [H112](#hook-112) |
| AI 招架判断<br>`PlayerPartyMeleeAiParryDecisionMultiplier` | 调整主队 AI 近战兵的 AIParryOnDecideAbility。1.000 为原版数值。 | [H112](#hook-112) |
| AI 盾牌防御判断<br>`PlayerPartyMeleeAiShieldDefenseMultiplier` | 调整主队 AI 近战兵的 AiDefendWithShieldDecisionChanceValue。1.000 为原版数值。 | [H112](#hook-112) |

### 海战 DLC

| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |
| --- | --- | --- |
| 添加全部船只升级部件<br>`NavalAddAllShipUpgradePieces` | 为每个升级槽添加可用价值最高的普通部件。 | [NavalDlcCompatibility.AddAllShipUpgradePieces → NavalDLCHelpers.AddUpgradePiecesToPartyShips](Patching/NavalDlcCompatibility.cs) |
| 添加船只升级部件<br>`NavalAddShipUpgradePieces` | 为玩家当前全部船只添加输入的升级部件；同一槽位仅保留一个部件。 | [NavalDlcCompatibility.AddShipUpgradePieces → NavalDLCHelpers.AddUpgradePiecesToPartyShips](Patching/NavalDlcCompatibility.cs) |
| 玩家船只额外弹药<br>`NavalAdditionalAmmo` | 为玩家所属船只增加额外弹药。 | [H194](#hook-194) |
| 海战奖励倍数<br>`NavalBattleRewardMultiplier` | 在通用奖励倍率之外，额外调整玩家海战获得的声望与影响力；不影响陆战。 | [H200](#hook-200), [H201](#hook-201) |
| 玩家船只海战伤害<br>`NavalBattleShipDamagePercentage` | 玩家所属船只在海战中受到的伤害；0% 为免疫。 | [H195](#hook-195) |
| 战役航行速度倍数<br>`NavalCampaignSpeedMultiplier` | 调整玩家所属船只提供的战役航行速度加成。 | [H196](#hook-196) |
| 玩家船只船员容量倍数<br>`NavalCrewCapacityMultiplier` | 调整玩家所属船只的船员容量加成。 | [H197](#hook-197) |
| 玩家海战部署上限倍数<br>`NavalDeploymentLimitMultiplier` | 调整玩家在海战中可部署的船只数量。 | [H198](#hook-198) |
| 船首像标识符<br>`NavalFigureheadId` | ID 定义文件：Mount & Blade II Bannerlord/Modules/NavalDLC/AssetPackages/nested_prefabs_packed.xml（船首像实体名）。多个 ID 可用空格、逗号或分号分隔。 | [NavalUnlockFigurehead 的输入框；船首像 ID。](Patching/NavalDlcCompatibility.cs) |
| 舰队派遣最低兵力<br>`NavalFleetMinimumTroopPercentage` | 调整派遣船只所需的最低兵力；0% 为无要求。 | [H199](#hook-199) |
| 给予全部船只<br>`NavalGrantAllShips` | 给予所有正常可用船体；剧情和任务船只不会加入。 | [NavalDlcCompatibility.GrantAllShips → NavalDLCCheats.AddShipToPlayer](Patching/NavalDlcCompatibility.cs) |
| 给予船只<br>`NavalGrantShip` | 将输入的船只标识符对应船只加入玩家舰队。 | [NavalDlcCompatibility.GrantShip → NavalDLCCheats.AddShipToPlayer](Patching/NavalDlcCompatibility.cs) |
| 玩家船只划桨力倍数<br>`NavalOarForceMultiplier` | 调整玩家所属船只的划桨推力。 | [H203](#hook-203) |
| 玩家船只帆力倍数<br>`NavalSailForceMultiplier` | 调整玩家所属船只的帆力。 | [H204](#hook-204) |
| 玩家船只海上损耗<br>`NavalSeaAttritionPercentage` | 航行时每小时受到的船只损伤；0% 为无损耗。 | [H205](#hook-205) |
| 船只战斗系数倍数<br>`NavalShipCombatFactorMultiplier` | 调整所有阵营的 Naval DLC 船只战斗系数。 | [H206](#hook-206) |
| 船只标识符<br>`NavalShipId` | ID 定义文件：Mount & Blade II Bannerlord/Modules/NavalDLC/ModuleData/ship_hulls.xml（ShipHull id）。多个 ID 可用空格、逗号或分号分隔。 | [NavalGrantShip 的输入框；船体 ID。](Patching/NavalDlcCompatibility.cs) |
| 船只购买价格倍数<br>`NavalShipPurchaseCostMultiplier` | 调整玩家购买船只时的价格。 | [H207](#hook-207) |
| 船只维修费用倍数<br>`NavalShipRepairCostMultiplier` | 调整玩家所属船只的维修费用。 | [H208](#hook-208) |
| 船只升级费用倍数<br>`NavalShipUpgradeCostMultiplier` | 调整玩家所属船只的升级部件费用。 | [H209](#hook-209) |
| 船只升级部件标识符<br>`NavalShipUpgradePieceId` | ID 定义文件：Mount & Blade II Bannerlord/Modules/NavalDLC/ModuleData/ship_upgrade_pieces.xml（ShipUpgradePiece id）。会装备至玩家当前所有船只的对应槽位。 | [NavalAddShipUpgradePieces 的输入框；升级件 ID。](Patching/NavalDlcCompatibility.cs) |
| 玩家船只风暴伤害<br>`NavalStormDamagePercentage` | 玩家所属船只受到的风暴伤害；0% 为风暴免疫。 | [H210](#hook-210) |
| 风暴出现频率倍数<br>`NavalStormFrequencyMultiplier` | 调整战役地图中的风暴生成概率；0 为不再生成新风暴。 | [H211](#hook-211) |
| 风暴强度倍数<br>`NavalStormStrengthMultiplier` | 调整全局风暴强度与风眼强度。 | [H212](#hook-212), [H213](#hook-213) |
| 解锁全部船首像<br>`NavalUnlockAllFigureheads` | 解锁 Naval DLC 内置的全部船首像。 | [NavalDlcCompatibility.UnlockAllFigureheads → NavalDLCCheats.UnlockFigurehead](Patching/NavalDlcCompatibility.cs) |
| 解锁船首像<br>`NavalUnlockFigurehead` | 解锁输入的船首像标识符。 | [NavalDlcCompatibility.UnlockFigurehead → NavalDLCCheats.UnlockFigurehead](Patching/NavalDlcCompatibility.cs) |

## 精确 Hook 索引

目标参数名、类型、ref/out/in 和返回类型取自当前安装 DLL；getter/setter 显示实际方法名。`optional DLC` 入口按程序集是否加载注册。修改游戏接口时，按此表先找目标，再改对应源码，并运行完整验证。

| 编号 | 当前游戏目标 | UFO 实现位置 |
| --- | --- | --- |
| <a id="hook-001"></a>H001 | `TaleWorlds.CampaignSystem.Hero.get_Age() : Single` | [MiscPatchs.AgePrefixPatch.Prefix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L88) |
| <a id="hook-002"></a>H002 | `TaleWorlds.CampaignSystem.CampaignBehaviors.AgingCampaignBehavior.DailyTickHero(Hero hero) : Void` | [MiscPatchs.DailyTickHeroPostfixPatch.Postfix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L186) |
| <a id="hook-003"></a>H003 | `TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages.EncyclopediaHeroPageVM.Refresh() : Void` | [MiscPatchs.EncyclopediaHeroPageVMPostfixPatch.Postfix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L41) |
| <a id="hook-004"></a>H004 | `TaleWorlds.CampaignSystem.Conversation.ConversationHelper.GetHeroRelationToHeroTextShort(Hero queriedHero, Hero baseHero, Boolean uppercaseFirst) : String` | [MiscPatchs.GetHeroRelationToHeroTextShortPostfixPatch.Postfix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L122) |
| <a id="hook-005"></a>H005 | `TaleWorlds.CampaignSystem.Settlements.Locations.Location.GetLocationCharacter(Hero hero) : LocationCharacter` | [MiscPatchs.GetLocationCharacterPrefixPatch.Prefix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L21) |
| <a id="hook-006"></a>H006 | `TaleWorlds.CampaignSystem.CampaignEvents.OnHeroJoinedParty(Hero hero, MobileParty mobileParty) : Void` | [MiscPatchs.OnHeroJoinedPartyPostfixPatch.Postfix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L177) |
| <a id="hook-007"></a>H007 | `TaleWorlds.CampaignSystem.Hero.SetBirthDay(CampaignTime birthday) : Void` | [MiscPatchs.SetBirthDayPrefixPatch.Prefix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L59) |
| <a id="hook-008"></a>H008 | `TaleWorlds.CampaignSystem.CharacterObject.get_TroopWage() : Int32` | [MiscPatchs.TroopWagePrefixPatch.Prefix](Patches/Characters/Attributes/CharacterAttributePatches.cs#L152) |
| <a id="hook-009"></a>H009 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartySizeLimitModel.AddMobilePartyLeaderPrisonerSizePerkEffects(PartyBase party, ref ExplainedNumber result) : Void` | [StrategyAttrEnhance.AddMobilePartyLeaderPrisonerSizePerkEffectsPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L53) |
| <a id="hook-010"></a>H010 | `TaleWorlds.CampaignSystem.Hero.AddSkillXp(SkillObject skill, Single xpAmount) : Void` | [StrategyAttrEnhance.AddSkillXpPrefixPatch.Prefix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L393) |
| <a id="hook-011"></a>H011 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartySpeedCalculatingModel.CalculateBaseSpeed(MobileParty mobileParty, Boolean includeDescriptions, Int32 additionalTroopOnFootCount, Int32 additionalTroopOnHorseCount) : ExplainedNumber` | [StrategyAttrEnhance.CalculateBaseSpeedPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L22) |
| <a id="hook-012"></a>H012 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSettlementProsperityModel.CalculateHearthChange(Village village, Boolean includeDescriptions) : ExplainedNumber` | [StrategyAttrEnhance.CalculateHearthChangeInternalPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L127) |
| <a id="hook-013"></a>H013 | `TaleWorlds.CampaignSystem.GameComponents.DefaultRaidModel.CalculateHitDamage(MapEventSide attackerSide, Single settlementHitPoints) : ExplainedNumber` | [StrategyAttrEnhance.CalculateHitDamagePostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L68) |
| <a id="hook-014"></a>H014 | `TaleWorlds.CampaignSystem.GameComponents.DefaultWallHitPointCalculationModel.CalculateMaximumWallHitPointInternal(Town town) : Single` | [StrategyAttrEnhance.CalculateMaximumWallHitPointPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L591) |
| <a id="hook-015"></a>H015 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSettlementMilitiaModel.CalculateMilitiaChange(Settlement settlement, Boolean includeDescriptions) : ExplainedNumber` | [StrategyAttrEnhance.CalculateMilitiaChangePostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L159) |
| <a id="hook-016"></a>H016 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSettlementFoodModel.CalculateTownFoodStocksChange(Town town, Boolean includeMarketStocks, Boolean includeDescriptions) : ExplainedNumber` | [StrategyAttrEnhance.CalculateTownFoodStocksChangePostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L445) |
| <a id="hook-017"></a>H017 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSettlementTaxModel.CalculateTownTax(Town town, Boolean includeDescriptions) : ExplainedNumber` | [StrategyAttrEnhance.CalculateTownTaxPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L279) |
| <a id="hook-018"></a>H018 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSettlementLoyaltyModel.GetSettlementLoyaltyChangeDueToGovernorPerks(Town town, ref ExplainedNumber explainedNumber) : Void` | [StrategyAttrEnhance.DailyTickPartyPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L95) |
| <a id="hook-019"></a>H019 | `TaleWorlds.CampaignSystem.CharacterDevelopment.HeroDeveloper.GainRawXp(Single rawXp, Boolean shouldNotify) : Void` | [StrategyAttrEnhance.GainRawXpPrefixPatch.Prefix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L429) |
| <a id="hook-020"></a>H020 | `TaleWorlds.CampaignSystem.GameComponents.DefaultClanTierModel.GetCompanionLimit(Clan clan) : Int32` | [StrategyAttrEnhance.GetCompanionLimitPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L217) |
| <a id="hook-021"></a>H021 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPrisonerRecruitmentCalculationModel.GetConformityChangePerHour(PartyBase party, CharacterObject troopToBoost) : ExplainedNumber` | [StrategyAttrEnhance.GetConformityChangePerHourPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L37) |
| <a id="hook-022"></a>H022 | `TaleWorlds.CampaignSystem.GameComponents.DefaultVolunteerModel.GetDailyVolunteerProductionProbability(Hero hero, Int32 index, Settlement settlement) : Single` | [StrategyAttrEnhance.GetDailyVolunteerProductionProbabilityPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L237) |
| <a id="hook-023"></a>H023 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPersuasionModel.GetDefaultSuccessChance(PersuasionOptionArgs optionArgs, Single difficultyMultiplier) : Single` | [StrategyAttrEnhance.GetDefaultSuccessChancePostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L199) |
| <a id="hook-024"></a>H024 | `TaleWorlds.CampaignSystem.GameComponents.DefaultWorkshopModel.GetEffectiveConversionSpeedOfProduction(Workshop workshop, Single speed, Boolean includeDescription) : ExplainedNumber` | [StrategyAttrEnhance.GetEffectiveConversionSpeedOfProductionPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L346) |
| <a id="hook-025"></a>H025 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSiegeEventModel.GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement) : IEnumerable<SiegeEngineType>` | [StrategyAttrEnhance.GetPrebuiltSiegeEnginesOfSettlementPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L492) |
| <a id="hook-026"></a>H026 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSiegeEventModel.GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp besiegerCamp) : IEnumerable<SiegeEngineType>` | [StrategyAttrEnhance.GetPrebuiltSiegeEnginesOfSiegeCampPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L527) |
| <a id="hook-027"></a>H027 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSiegeEventModel.GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide) : Single` | [StrategyAttrEnhance.GetSiegeEngineHitPointsPostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L546) |
| <a id="hook-028"></a>H028 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyWageModel.GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, Boolean includeDescriptions) : ExplainedNumber` | [StrategyAttrEnhance.GetTotalWagePostfixPatch.Postfix](Patches/Progression/Attributes/StrategyAttributePatches.cs#L310) |
| <a id="hook-029"></a>H029 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCharacterDevelopmentModel.get_MaxAttribute() : Int32` | [UFO.Model.MaxAttributePatch.Postfix](Models/MaxAttribute.cs#L9) |
| <a id="hook-030"></a>H030 | `TaleWorlds.CampaignSystem.Army.get_DailyCohesionChange() : Single` | [UFO.Patch.ACLP.Postfix](Patches/Campaign/Army/ArmyCampaignPatches.cs#L14) |
| <a id="hook-031"></a>H031 | `TaleWorlds.CampaignSystem.GameComponents.DefaultMobilePartyFoodConsumptionModel.CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption) : ExplainedNumber` | [UFO.Patch.AFCP.Postfix](Patches/Campaign/Army/ArmyCampaignPatches.cs#L34) |
| <a id="hook-032"></a>H032 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPregnancyModel.get_PregnancyDurationInDays() : Single` | [UFO.Patch.APD.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L20) |
| <a id="hook-033"></a>H033 | `TaleWorlds.CampaignSystem.GameComponents.DefaultMarriageModel.IsCoupleSuitableForMarriage(Hero firstHero, Hero secondHero) : Boolean` | [UFO.Patch.ASSM.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L39) |
| <a id="hook-034"></a>H034 | `TaleWorlds.CampaignSystem.BarterSystem.BarterManager.IsOfferAcceptable(BarterData args, Hero hero, PartyBase party) : Boolean` | [UFO.Patch.BOAA.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L84) |
| <a id="hook-035"></a>H035 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPersuasionModel.GetChances(PersuasionOptionArgs optionArgs, out Single successChance, out Single critSuccessChance, out Single critFailChance, out Single failChance, Single difficultyMultiplier) : Void` | [UFO.Patch.CAS.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L103) |
| <a id="hook-036"></a>H036 | `TaleWorlds.CampaignSystem.Party.MobileParty.get_ShouldBeIgnored() : Boolean` | [UFO.Patch.CaravansInvisibleOnMap.ShouldBeIgnored](Patches/Campaign/Map/CampaignMapPatches.cs#L99) |
| <a id="hook-037"></a>H037 | `TaleWorlds.CampaignSystem.CharacterDevelopment.HeroDeveloper.AddSkillXp(SkillObject skill, Single rawXp, Boolean isAffectedByFocusFactor, Boolean shouldNotify) : Void` | [UFO.Patch.ClanExperienceMultiplier.AddSkillXp](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L40) |
| <a id="hook-038"></a>H038 | `TaleWorlds.MountAndBlade.MissionCombatMechanicsHelper.DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow) : Boolean` | [UFO.Patch.Combat.AKD_S.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L24) |
| <a id="hook-039"></a>H039 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCombatSimulationModel.SimulateHit(CharacterObject strikerTroop, CharacterObject struckTroop, PartyBase strikerParty, PartyBase struckParty, Single strikerAdvantage, MapEvent battle, BattleEnvironment battleEnvironment, Single strikerSideMorale, Single struckSideMorale) : ExplainedNumber` | [UFO.Patch.Combat.AWBS.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L55) |
| <a id="hook-040"></a>H040 | `TaleWorlds.CampaignSystem.GameComponents.DefaultBanditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty party, Boolean isAssault) : Int32` | [UFO.Patch.Combat.BHTL.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L80) |
| <a id="hook-041"></a>H041 | `SandBox.GameComponents.SandboxBattleMoraleModel.CalculateCasualtiesFactor(BattleSideEnum battleSide) : Single` | [UFO.Patch.Combat.CCF.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L99) |
| <a id="hook-042"></a>H042 | `TaleWorlds.MountAndBlade.CustomBattleMoraleModel.CalculateCasualtiesFactor(BattleSideEnum battleSide) : Single` | [UFO.Patch.Combat.CCF2.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L118) |
| <a id="hook-043"></a>H043 | `SandBox.GameComponents.SandboxBattleMoraleModel.CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow) : ValueTuple<Single, Single>` | [UFO.Patch.Combat.CMMCDTAI.Prefix](Patches/Combat/Battle/BattleRulesPatches.cs#L137) |
| <a id="hook-044"></a>H044 | `TaleWorlds.MountAndBlade.CustomBattleMoraleModel.CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow) : ValueTuple<Single, Single>` | [UFO.Patch.Combat.CMMCDTAI2.Prefix](Patches/Combat/Battle/BattleRulesPatches.cs#L159) |
| <a id="hook-045"></a>H045 | `SandBox.GameComponents.SandboxAgentStatCalculateModel.GetMaxCameraZoom(Agent agent) : Single` | [UFO.Patch.Combat.CZM.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L233) |
| <a id="hook-046"></a>H046 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDefaultRemainingMomentum(Single originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, Boolean isCrushThrough) : Single` | [UFO.Patch.Combat.CalculateDefaultRemainingMomentumPatch.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L206) |
| <a id="hook-047"></a>H047 | `TaleWorlds.MountAndBlade.AgentStatCalculateModel.CalculateAILevel(Agent agent, Int32 relevantSkillLevel) : Single` | [UFO.Patch.Combat.CombatAttrEnhance.CalculateAILevelPostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L261) |
| <a id="hook-048"></a>H048 | `SandBox.GameComponents.SandboxAgentApplyDamageModel.CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow) : Single` | [UFO.Patch.Combat.CombatAttrEnhance.CalculateStaggerThresholdDamagePostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L191) |
| <a id="hook-049"></a>H049 | `SandBox.GameComponents.SandboxAgentApplyDamageModel.DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.DecideMissileWeaponFlagsPostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L136) |
| <a id="hook-050"></a>H050 | `TaleWorlds.CampaignSystem.Hero.Heal(Int32 healAmount, Boolean addXp) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.GetHeroesEffectedHealingAmountPostfixPatch.Prefix](Patches/Combat/Attributes/CombatAttributePatches.cs#L121) |
| <a id="hook-051"></a>H051 | `SandBox.GameComponents.SandboxAgentStatCalculateModel.InitializeMissionEquipment(Agent agent) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.InitializeMissionEquipmentPostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L316) |
| <a id="hook-052"></a>H052 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCharacterStatsModel.MaxHitpoints(CharacterObject character, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.Combat.CombatAttrEnhance.MaxHitpointsPostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L98) |
| <a id="hook-053"></a>H053 | `TaleWorlds.MountAndBlade.Agent.OnWeaponAmountChange(EquipmentIndex slotIndex, Int16 amount) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.OnWeaponAmountChangePrefixPatch.Prefix](Patches/Combat/Attributes/CombatAttributePatches.cs#L384) |
| <a id="hook-054"></a>H054 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.CombatAttrEnhance.SandboxCalculateDamagePrefixPatch.Prefix](Patches/Combat/Attributes/CombatAttributePatches.cs#L156) |
| <a id="hook-055"></a>H055 | `TaleWorlds.MountAndBlade.AgentStatCalculateModel.SetAiRelatedProperties(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedItem, WeaponComponentData secondaryItem) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.SetAiRelatedPropertiesPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L279) |
| <a id="hook-056"></a>H056 | `TaleWorlds.MountAndBlade.Agent.set_Health(Single value) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.SetHealthPrefixPatch.Prefix](Patches/Combat/Attributes/CombatAttributePatches.cs#L165) |
| <a id="hook-057"></a>H057 | `SandBox.GameComponents.SandboxAgentStatCalculateModel.SetPerkAndBannerEffectsOnAgent(Agent agent, CharacterObject agentCharacter, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.SetPerkAndBannerEffectsOnAgentPostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L207) |
| <a id="hook-058"></a>H058 | `SandBox.GameComponents.SandboxAgentStatCalculateModel.UpdateHorseStats(Agent agent, AgentDrivenProperties agentDrivenProperties) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.UpdateHorseStatsPostfixPatch.Postfix](Patches/Combat/Attributes/CombatAttributePatches.cs#L241) |
| <a id="hook-059"></a>H059 | `TaleWorlds.MountAndBlade.Agent.Die(Blow b, KillInfo overrideKillInfo) : Void` | [UFO.Patch.Combat.CombatAttrEnhance.setDiePrefixPatch.Prefix](Patches/Combat/Attributes/CombatAttributePatches.cs#L178) |
| <a id="hook-060"></a>H060 | `TaleWorlds.MountAndBlade.DefaultAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.CompanionsKnockoutOrKilled_Default.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L206) |
| <a id="hook-061"></a>H061 | `SandBox.GameComponents.SandboxAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.CompanionsKnockoutOrKilled_Sandbox.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L215) |
| <a id="hook-062"></a>H062 | `StoryMode.GameComponents.StoryModeAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.CompanionsKnockoutOrKilled_StoryMode.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L224) |
| <a id="hook-063"></a>H063 | `TaleWorlds.MountAndBlade.MissionCombatMechanicsHelper.DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, Boolean isFatalHit, Boolean isShruggedOff, Single momentumRemaining, out MeleeCollisionReaction colReaction) : Void` | [UFO.Patch.Combat.CutThroughEveryonePatchCollision.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L166) |
| <a id="hook-064"></a>H064 | `TaleWorlds.MountAndBlade.Mission.MeleeHitCallback(ref AttackCollisionData collisionData, Agent attacker, Agent victim, GameEntity realHitEntity, ref Single inOutMomentumRemaining, ref MeleeCollisionReaction colReaction, CrushThroughState crushThroughState, Vec3 blowDir, Vec3 swingDir, ref HitParticleResultData hitParticleResultData, Boolean crushedThroughWithoutAgentCollision) : Void` | [UFO.Patch.Combat.CutThroughEveryonePatchMeleeHit.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L186) |
| <a id="hook-065"></a>H065 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.DM_S.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L252) |
| <a id="hook-066"></a>H066 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.DTP_S.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L272) |
| <a id="hook-067"></a>H067 | `NavalDLC.GameComponents.NavalDLCCustomAgentApplyDamageModel.DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, Single totalAttackEnergy, UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, Boolean isPassiveUsage) : Boolean (optional DLC)` | [UFO.Patch.Combat.DecideCrushedThroughPostfixPatch_c.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L34) |
| <a id="hook-068"></a>H068 | `TaleWorlds.MountAndBlade.CustomAgentApplyDamageModel.DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, Single totalAttackEnergy, UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, Boolean isPassiveUsage) : Boolean` | [UFO.Patch.Combat.DecideCrushedThroughPostfixPatch_c.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L34) |
| <a id="hook-069"></a>H069 | `SandBox.GameComponents.SandboxAgentApplyDamageModel.DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, Single totalAttackEnergy, UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, Boolean isPassiveUsage) : Boolean` | [UFO.Patch.Combat.DecideCrushedThroughPostfixPatch_s.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L86) |
| <a id="hook-070"></a>H070 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.EDP_S.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L295) |
| <a id="hook-071"></a>H071 | `SandBox.GameComponents.SandboxBattleMoraleModel.CalculateMoraleChangeToCharacter(Agent agent, Single maxMoraleChange) : Single` | [UFO.Patch.Combat.ENRA.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L432) |
| <a id="hook-072"></a>H072 | `TaleWorlds.MountAndBlade.CustomBattleMoraleModel.CalculateMoraleChangeToCharacter(Agent agent, Single maxMoraleChange) : Single` | [UFO.Patch.Combat.ENRA2.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L451) |
| <a id="hook-073"></a>H073 | `SandBox.GameComponents.SandboxBattleMoraleModel.CanPanicDueToMorale(Agent agent) : Boolean` | [UFO.Patch.Combat.ENRACP.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L470) |
| <a id="hook-074"></a>H074 | `TaleWorlds.MountAndBlade.CustomBattleMoraleModel.CanPanicDueToMorale(Agent agent) : Boolean` | [UFO.Patch.Combat.ENRACP2.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L489) |
| <a id="hook-075"></a>H075 | `TaleWorlds.MountAndBlade.DefaultAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.EnemyLordsKnockoutOrKilled_Default.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L341) |
| <a id="hook-076"></a>H076 | `SandBox.GameComponents.SandboxAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.EnemyLordsKnockoutOrKilled_Sandbox.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L352) |
| <a id="hook-077"></a>H077 | `StoryMode.GameComponents.StoryModeAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.EnemyLordsKnockoutOrKilled_StoryMode.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L363) |
| <a id="hook-078"></a>H078 | `TaleWorlds.MountAndBlade.BehaviorRetreat.GetAiWeight() : Single` | [UFO.Patch.Combat.EnemyRetreatBehaviorPatch.Prefix](Patches/Combat/Morale/RetreatPatches.cs#L34) |
| <a id="hook-079"></a>H079 | `TaleWorlds.MountAndBlade.BehaviorRetreatToCastle.GetAiWeight() : Single` | [UFO.Patch.Combat.EnemyRetreatToCastleBehaviorPatch.Prefix](Patches/Combat/Morale/RetreatPatches.cs#L46) |
| <a id="hook-080"></a>H080 | `TaleWorlds.MountAndBlade.BehaviorRetreatToKeep.GetAiWeight() : Single` | [UFO.Patch.Combat.EnemyRetreatToKeepBehaviorPatch.Prefix](Patches/Combat/Morale/RetreatPatches.cs#L58) |
| <a id="hook-081"></a>H081 | `TaleWorlds.MountAndBlade.TacticBreachWalls.ShouldRetreat(List<SiegeLane> lanes, Int32 insideFormationCount) : Boolean` | [UFO.Patch.Combat.EnemySiegeTacticalRetreatPatch.Prefix](Patches/Combat/Morale/RetreatPatches.cs#L76) |
| <a id="hook-082"></a>H082 | `TaleWorlds.MountAndBlade.DefaultAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.EnemyTroopsKnockoutOrKilled_Default.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L399) |
| <a id="hook-083"></a>H083 | `SandBox.GameComponents.SandboxAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.EnemyTroopsKnockoutOrKilled_Sandbox.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L410) |
| <a id="hook-084"></a>H084 | `StoryMode.GameComponents.StoryModeAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.EnemyTroopsKnockoutOrKilled_StoryMode.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L421) |
| <a id="hook-085"></a>H085 | `TaleWorlds.MountAndBlade.DefaultAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.FriendlyLordsKnockoutOrKilled_Default.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L533) |
| <a id="hook-086"></a>H086 | `SandBox.GameComponents.SandboxAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.FriendlyLordsKnockoutOrKilled_Sandbox.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L544) |
| <a id="hook-087"></a>H087 | `StoryMode.GameComponents.StoryModeAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.FriendlyLordsKnockoutOrKilled_StoryMode.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L555) |
| <a id="hook-088"></a>H088 | `TaleWorlds.MountAndBlade.TacticCoordinatedRetreat.GetTacticWeight() : Single` | [UFO.Patch.Combat.GTW.Prefix](Patches/Combat/Battle/BattleRulesPatches.cs#L566) |
| <a id="hook-089"></a>H089 | `TaleWorlds.MountAndBlade.Mission.OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, Boolean hasRigidBody, Boolean isPrimaryWeaponShot, Int32 forcedMissileIndex) : Void` | [UFO.Patch.Combat.InfiniteAmmo.OnAgentShootMissile](Patches/Combat/Battle/BattleRulesPatches.cs#L621) |
| <a id="hook-090"></a>H090 | `TaleWorlds.CampaignSystem.GameComponents.DefaultBattleRewardModel.CalculateInfluenceGain(PartyBase winnerParty, Single influenceValueOfBattleForWinnerSide, Single contributionShareOfWinnerParty, Single influenceMultiplierForWinnerSide, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.Combat.InfluenceRewardMultiplier.CalculateInfluenceGain](Patches/Combat/Battle/BattleRulesPatches.cs#L650) |
| <a id="hook-091"></a>H091 | `SandBox.GameComponents.SandboxAgentStatCalculateModel.UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties) : Void` | [UFO.Patch.Combat.InstantCrossbowReload.UpdateAgentStats](Patches/Combat/Battle/BattleRulesPatches.cs#L671) |
| <a id="hook-092"></a>H092 | `TaleWorlds.MountAndBlade.AgentStatCalculateModel.SetAllWeaponInaccuracy(Agent agent, AgentDrivenProperties agentDrivenProperties, Int32 equippedIndex, WeaponComponentData equippedWeaponComponent) : Void` | [UFO.Patch.Combat.InstantCrossbowReloadSpeed.SetAllWeaponInaccuracy](Patches/Combat/Battle/BattleRulesPatches.cs#L692) |
| <a id="hook-093"></a>H093 | `TaleWorlds.MountAndBlade.Agent.get_CurrentMortalityState() : MortalityState` | [UFO.Patch.Combat.Invincible.Invulnerable](Patches/Combat/Battle/BattleRulesPatches.cs#L714) |
| <a id="hook-094"></a>H094 | `TaleWorlds.MountAndBlade.MissionCombatMechanicsHelper.GetDefendCollisionResults(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, Int32 attackerWeaponSlotIndex, Boolean isAlternativeAttack, StrikeType strikeType, UsageDirection attackDirection, Single collisionDistanceOnWeapon, Single attackProgress, Boolean attackIsParried, Boolean isPassiveUsageHit, Boolean isHeavyAttack, ref Single defenderStunPeriod, ref Single attackerStunPeriod, ref Boolean crushedThrough, ref Boolean chamber) : Void` | [UFO.Patch.Combat.MissionCombatMechanicsHelperGetDefendCollisionResultsPatch.Postfix](Patches/Combat/Weapons/WeaponInteractionPatches.cs#L224) |
| <a id="hook-095"></a>H095 | `SandBox.GameComponents.SandboxAgentApplyDamageModel.DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow) : Boolean` | [UFO.Patch.Combat.NeverKnockedBackByAttacks_Sandbox.DecideAgentKnockedByBlow](Patches/Combat/Battle/BattleRulesPatches.cs#L755) |
| <a id="hook-096"></a>H096 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.NoFriendlyFire_Sandbox.CalculateDamage](Patches/Combat/Battle/BattleRulesPatches.cs#L784) |
| <a id="hook-097"></a>H097 | `SandBox.GameComponents.SandboxBattleMoraleModel.CalculateMoraleChangeToCharacter(Agent agent, Single maxMoraleChange) : Single` | [UFO.Patch.Combat.NoRunningAway.CalculateMoraleChangeToCharacter](Patches/Combat/Battle/BattleRulesPatches.cs#L795) |
| <a id="hook-098"></a>H098 | `TaleWorlds.MountAndBlade.Mission.HandleMissileCollisionReaction(Int32 missileIndex, MissileCollisionReaction collisionReaction, MatrixFrame attachLocalFrame, Boolean isAttachedFrameLocal, Agent attackerAgent, Agent attachedAgent, Boolean attachedToShield, SByte attachedBoneIndex, MissionObject attachedMissionObject, Vec3 bounceBackVelocity, Vec3 bounceBackAngularVelocity, Int32 forcedSpawnIndex) : Void` | [UFO.Patch.Combat.NoStuckArrows.HandleMissileCollisionReaction](Patches/Combat/Battle/BattleRulesPatches.cs#L816) |
| <a id="hook-099"></a>H099 | `TaleWorlds.CampaignSystem.GameComponents.DefaultTroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent) : ExplainedNumber` | [UFO.Patch.Combat.NoTroopSacrificeBreakIn.GetLostTroopCountForBreakingInBesiegedSettlement](Patches/Combat/Battle/BattleRulesPatches.cs#L837) |
| <a id="hook-100"></a>H100 | `TaleWorlds.CampaignSystem.GameComponents.DefaultTroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, Boolean isBreakingOutFromPort) : ExplainedNumber` | [UFO.Patch.Combat.NoTroopSacrificeBreakOut.GetLostTroopCountForBreakingOutOfBesiegedSettlement](Patches/Combat/Battle/BattleRulesPatches.cs#L858) |
| <a id="hook-101"></a>H101 | `TaleWorlds.CampaignSystem.GameComponents.DefaultTroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent) : Int32` | [UFO.Patch.Combat.NoTroopSacrificeRunaway.Postfix](Patches/Combat/Battle/BattleRulesPatches.cs#L881) |
| <a id="hook-102"></a>H102 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.OneHitKill_Sandbox.CalculateDamage](Patches/Combat/Battle/BattleRulesPatches.cs#L919) |
| <a id="hook-103"></a>H103 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.PartyDamageMultiplier_Sandbox.CalculateDamage](Patches/Combat/Battle/BattleRulesPatches.cs#L948) |
| <a id="hook-104"></a>H104 | `TaleWorlds.MountAndBlade.Agent.get_CurrentMortalityState() : MortalityState` | [UFO.Patch.Combat.PartyHeroesInvincible.Invulnerable](Patches/Combat/Battle/BattleRulesPatches.cs#L999) |
| <a id="hook-105"></a>H105 | `TaleWorlds.MountAndBlade.Mission.OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, Boolean hasRigidBody, Boolean isPrimaryWeaponShot, Int32 forcedMissileIndex) : Void` | [UFO.Patch.Combat.PartyInfiniteAmmo.OnAgentShootMissile](Patches/Combat/Battle/BattleRulesPatches.cs#L1020) |
| <a id="hook-106"></a>H106 | `TaleWorlds.MountAndBlade.Agent.get_CurrentMortalityState() : MortalityState` | [UFO.Patch.Combat.PartyInvincible.Invulnerable](Patches/Combat/Battle/BattleRulesPatches.cs#L1049) |
| <a id="hook-107"></a>H107 | `TaleWorlds.MountAndBlade.DefaultAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.PartyKnockoutOrKilled_Default.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L1095) |
| <a id="hook-108"></a>H108 | `SandBox.GameComponents.SandboxAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.PartyKnockoutOrKilled_Sandbox.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L1106) |
| <a id="hook-109"></a>H109 | `StoryMode.GameComponents.StoryModeAgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out Single useSurgeryProbability) : Single` | [UFO.Patch.Combat.PartyKnockoutOrKilled_StoryMode.GetAgentStateProbability](Patches/Combat/Battle/BattleRulesPatches.cs#L1117) |
| <a id="hook-110"></a>H110 | `TaleWorlds.MountAndBlade.ComponentInterfaces.AgentApplyDamageModel.CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, Single baseDamage) : Single` | [UFO.Patch.Combat.PartyOneHitKill_Sandbox.CalculateDamage](Patches/Combat/Battle/BattleRulesPatches.cs#L1146) |
| <a id="hook-111"></a>H111 | `TaleWorlds.MountAndBlade.Agent.get_CurrentMortalityState() : MortalityState` | [UFO.Patch.Combat.PlayerHorseInvincible.Invulnerable](Patches/Combat/Battle/BattleRulesPatches.cs#L1157) |
| <a id="hook-112"></a>H112 | `TaleWorlds.MountAndBlade.AgentStatCalculateModel.SetAllWeaponInaccuracy(Agent agent, AgentDrivenProperties agentDrivenProperties, Int32 equippedIndex, WeaponComponentData equippedWeaponComponent) : Void` | [UFO.Patch.Combat.PlayerPartyMeleeProperties.SetAllWeaponInaccuracy](Patches/Combat/Weapons/PlayerPartyMeleePatches.cs#L39) |
| <a id="hook-113"></a>H113 | `TaleWorlds.MountAndBlade.AgentStatCalculateModel.SetAllWeaponInaccuracy(Agent agent, AgentDrivenProperties agentDrivenProperties, Int32 equippedIndex, WeaponComponentData equippedWeaponComponent) : Void` | [UFO.Patch.Combat.PlayerPartyRangedWeaponProperties.SetAllWeaponInaccuracy](Patches/Combat/Weapons/PlayerPartyRangedAccuracyPatches.cs#L39) |
| <a id="hook-114"></a>H114 | `TaleWorlds.CampaignSystem.GameComponents.DefaultBattleRewardModel.CalculateRenownGain(PartyBase winnerParty, Single renownValueOfBattleForWinnerSide, Single contributionShareOfWinnerParty, Single renownMultiplierForWinnerSide, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.Combat.RenownRewardMultiplierBattle.CalculateRenownGain](Patches/Combat/Battle/BattleRulesPatches.cs#L1178) |
| <a id="hook-115"></a>H115 | `TaleWorlds.CampaignSystem.GameComponents.DefaultTournamentModel.GetRenownReward(Hero winner, Town town) : Int32` | [UFO.Patch.Combat.RenownRewardMultiplierTournament.GetRenownReward](Patches/Combat/Battle/BattleRulesPatches.cs#L1199) |
| <a id="hook-116"></a>H116 | `TaleWorlds.CampaignSystem.CharacterDevelopment.HeroDeveloper.AddSkillXp(SkillObject skill, Single rawXp, Boolean isAffectedByFocusFactor, Boolean shouldNotify) : Void` | [UFO.Patch.CompanionExperienceMultiplier.AddSkillXp](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L54) |
| <a id="hook-117"></a>H117 | `TaleWorlds.CampaignSystem.GameComponents.DefaultBuildingConstructionModel.CalculateDailyConstructionPower(Town town, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.ConstructionPowerMultiplier.CalculateDailyConstructionPower](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L31) |
| <a id="hook-118"></a>H118 | `TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.WeaponDesignVM.GetResultPropertyList(CraftingSecondaryUsageItemVM usageItem) : MBBindingList<WeaponDesignResultPropertyItemVM>` | [UFO.Patch.CraftedWeaponModifierBonus.Prefix](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L20) |
| <a id="hook-119"></a>H119 | `TaleWorlds.CampaignSystem.CampaignEvents.OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, Boolean isCraftingOrderItem) : Void` | [UFO.Patch.CraftedWeaponModifierSave.Prefix](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L92) |
| <a id="hook-120"></a>H120 | `TaleWorlds.CampaignSystem.Settlements.Town.get_FoodChange() : Single` | [UFO.Patch.DailyFoodBonus.FoodChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L52) |
| <a id="hook-121"></a>H121 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSettlementGarrisonModel.CalculateBaseGarrisonChange(Settlement settlement, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.DailyGarrisonGrowthBonus.Postfix](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L20) |
| <a id="hook-122"></a>H122 | `TaleWorlds.CampaignSystem.Settlements.Village.get_HearthChange() : Single` | [UFO.Patch.DailyHearthsBonus.HearthChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L73) |
| <a id="hook-123"></a>H123 | `TaleWorlds.CampaignSystem.Settlements.Town.get_LoyaltyChange() : Single` | [UFO.Patch.DailyLoyaltyBonus.LoyaltyChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L94) |
| <a id="hook-124"></a>H124 | `TaleWorlds.CampaignSystem.Settlements.Town.get_MilitiaChange() : Single` | [UFO.Patch.DailyMilitiaBonusTown.MilitiaChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L115) |
| <a id="hook-125"></a>H125 | `TaleWorlds.CampaignSystem.Settlements.Village.get_MilitiaChange() : Single` | [UFO.Patch.DailyMilitiaBonusVillage.MilitiaChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L136) |
| <a id="hook-126"></a>H126 | `TaleWorlds.CampaignSystem.Settlements.Town.get_ProsperityChange() : Single` | [UFO.Patch.DailyProsperityBonus.ProsperityChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L157) |
| <a id="hook-127"></a>H127 | `TaleWorlds.CampaignSystem.Settlements.Town.get_SecurityChange() : Single` | [UFO.Patch.DailySecurityBonus.SecurityChange](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L178) |
| <a id="hook-128"></a>H128 | `TaleWorlds.CampaignSystem.GameComponents.DefaultClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(DecisionOutcome popularOption, DecisionOutcome overridingOption, KingdomDecision decision) : Int32` | [UFO.Patch.DecisionOverrideInfluenceCostPercentage.GetInfluenceRequiredToOverrideKingdomDecision](Patches/Campaign/Kingdom/KingdomCampaignPatches.cs#L15) |
| <a id="hook-129"></a>H129 | `TaleWorlds.CampaignSystem.GameComponents.DefaultDisguiseDetectionModel.CalculateDisguiseDetectionProbability(Settlement settlement) : Single` | [UFO.Patch.DisguiseAlwaysWorks.CalculateDisguiseDetectionProbability](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L199) |
| <a id="hook-130"></a>H130 | `SandBox.View.Map.MapScreen.OpenEncyclopedia() : Void` | [UFO.Patch.EnableHotkeyTipsEncyclopedia.OpenEncyclopedia](Patches/UI/Hotkeys/HotkeyScreenPatches.cs#L400) |
| <a id="hook-131"></a>H131 | `TaleWorlds.ScreenSystem.ScreenManager.PushScreen(ScreenBase screen) : Void` | [UFO.Patch.EnableHotkeyTipsScreens.PushScreen](Patches/UI/Hotkeys/HotkeyScreenPatches.cs#L421) |
| <a id="hook-132"></a>H132 | `TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages.EncyclopediaPageVM.OnTick() : Void` | [UFO.Patch.EnableHotkeysAddEncyclopediaTroops.Prefix](Patches/UI/Hotkeys/HotkeyScreenPatches.cs#L26) |
| <a id="hook-133"></a>H133 | `TaleWorlds.Core.GameManagerBase.OnTick(Single dt) : Void` | [UFO.Patch.EnableHotkeysAddItems.Prefix](Patches/UI/Hotkeys/HotkeyScreenPatches.cs#L62) |
| <a id="hook-134"></a>H134 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSiegeEventModel.GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side) : Single` | [UFO.Patch.EnemySiegeBuildingSpeedPercentage.GetConstructionProgressPerHour](Patches/Campaign/Siege/SiegeCampaignPatches.cs#L16) |
| <a id="hook-135"></a>H135 | `TaleWorlds.CampaignSystem.CharacterDevelopment.HeroDeveloper.AddSkillXp(SkillObject skill, Single rawXp, Boolean isAffectedByFocusFactor, Boolean shouldNotify) : Void` | [UFO.Patch.ExperienceMultiplier.AddSkillXp](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L68) |
| <a id="hook-136"></a>H136 | `TaleWorlds.CampaignSystem.GameComponents.DefaultClanTierModel.GetPartyLimitForTier(Clan clan, Int32 clanTierToCheck) : Int32` | [UFO.Patch.ExtraClanPartyLimit.Postfix](Patches/Campaign/Clan/ClanCampaignPatches.cs#L40) |
| <a id="hook-137"></a>H137 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartySizeLimitModel.GetPartyMemberSizeLimit(PartyBase party, Boolean isCurrentlyAtSea, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.ExtraClanPartySize.Postfix](Patches/Campaign/Clan/ClanCampaignPatches.cs#L59) |
| <a id="hook-138"></a>H138 | `TaleWorlds.CampaignSystem.GameComponents.DefaultClanTierModel.GetCompanionLimit(Clan clan) : Int32` | [UFO.Patch.ExtraCompanionLimit.Postfix](Patches/Campaign/Clan/ClanCampaignPatches.cs#L90) |
| <a id="hook-139"></a>H139 | `TaleWorlds.CampaignSystem.GameComponents.DefaultInventoryCapacityModel.CalculateInventoryCapacity(MobileParty mobileParty, Boolean isCurrentlyAtSea, Boolean includeDescriptions, Int32 additionalTroops, Int32 additionalSpareMounts, Int32 additionalPackAnimals, Boolean includeFollowers) : ExplainedNumber` | [UFO.Patch.ExtraInventoryCapacity.CalculateInventoryCapacity](Patches/Inventory/Trading/InventoryTradingPatches.cs#L22) |
| <a id="hook-140"></a>H140 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyMoraleModel.GetEffectivePartyMorale(MobileParty mobileParty, Boolean includeDescription) : ExplainedNumber` | [UFO.Patch.ExtraPartyMorale.GetEffectivePartyMorale](Patches/Campaign/Party/PartyCampaignPatches.cs#L19) |
| <a id="hook-141"></a>H141 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartySizeLimitModel.GetPartyPrisonerSizeLimit(PartyBase party, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.ExtraPartyPrisonerSize.Postfix](Patches/Campaign/Party/PartyCampaignPatches.cs#L40) |
| <a id="hook-142"></a>H142 | `TaleWorlds.CampaignSystem.Army.get_DailyCohesionChange() : Single` | [UFO.Patch.FACLP.Postfix](Patches/Campaign/Army/ArmyCampaignPatches.cs#L53) |
| <a id="hook-143"></a>H143 | `TaleWorlds.CampaignSystem.GameComponents.DefaultMobilePartyFoodConsumptionModel.CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption) : ExplainedNumber` | [UFO.Patch.FoodConsumptionPercentage.CalculateDailyFoodConsumptionf](Patches/Campaign/Party/PartyCampaignPatches.cs#L59) |
| <a id="hook-144"></a>H144 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCompanionHiringPriceCalculationModel.GetCompanionHiringPrice(Hero companion) : Int32` | [UFO.Patch.FreeCompanionHiring.GetCompanionHiringPrice](Patches/Campaign/Party/PartyCampaignPatches.cs#L80) |
| <a id="hook-145"></a>H145 | `TaleWorlds.CampaignSystem.CharacterDevelopment.HeroDeveloper.GetRequiredFocusPointsToAddFocus(SkillObject skill) : Int32` | [UFO.Patch.FreeFocusPointAssignment.GetRequiredFocusPointsToAddFocus](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L82) |
| <a id="hook-146"></a>H146 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyWageModel.GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, Boolean withoutItemCost) : ExplainedNumber` | [UFO.Patch.FreeTroopRecruitment.GetTroopRecruitmentCost](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L220) |
| <a id="hook-147"></a>H147 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyTroopUpgradeModel.GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget) : ExplainedNumber` | [UFO.Patch.FreeTroopUpgrades.GetGoldCostForUpgrade](Patches/Campaign/Party/PartyCampaignPatches.cs#L101) |
| <a id="hook-148"></a>H148 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCharacterDevelopmentModel.CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, Int32 focusValue, Int32 skillValue, SkillObject skill, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.HeroLearningRateMultipliers.Postfix](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L18) |
| <a id="hook-149"></a>H149 | `TaleWorlds.CampaignSystem.CampaignBehaviors.PlayerCaptivityCampaignBehavior.CheckCaptivityChange(Single dt) : Void` | [UFO.Patch.InstantEscape.CheckCaptivityChange](Patches/Campaign/Party/PartyCampaignPatches.cs#L122) |
| <a id="hook-150"></a>H150 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase party, CharacterObject character) : Int32` | [UFO.Patch.InstantPrisonerRecruitment.CalculateRecruitableNumber](Patches/Campaign/Party/PartyCampaignPatches.cs#L143) |
| <a id="hook-151"></a>H151 | `TaleWorlds.CampaignSystem.GameComponents.DefaultTradeItemPriceFactorModel.GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, Boolean isSelling, Single inStoreValue, Single supply, Single demand) : Int32` | [UFO.Patch.ItemTradingCostPercentage.GetPrice](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L241) |
| <a id="hook-152"></a>H152 | `TaleWorlds.CampaignSystem.GameComponents.DefaultMarriageModel.GetClanAfterMarriage(Hero firstHero, Hero secondHero) : Clan` | [UFO.Patch.KeepDaughter.Postfix](Patches/Campaign/Clan/ClanCampaignPatches.cs#L15) |
| <a id="hook-153"></a>H153 | `TaleWorlds.CampaignSystem.Election.DecisionOutcome.get_TotalSupportPoints() : Single` | [UFO.Patch.KingdomDecisionWeightMultiplier.Getter](Patches/Campaign/Kingdom/KingdomCampaignPatches.cs#L38) |
| <a id="hook-154"></a>H154 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCharacterDevelopmentModel.CalculateLearningLimit(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, Int32 focusValue, SkillObject skill, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.LearningLimitMultiplier.CalculateLearningLimit](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L103) |
| <a id="hook-155"></a>H155 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartySpeedCalculatingModel.CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed) : ExplainedNumber` | [UFO.Patch.MapSpeedMultiplier.CalculateFinalSpeed](Patches/Campaign/Map/CampaignMapPatches.cs#L15) |
| <a id="hook-156"></a>H156 | `TaleWorlds.CampaignSystem.GameComponents.DefaultMapVisibilityModel.GetPartySpottingRange(MobileParty party, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.MapVisibilityMultiplier.GetPartySpottingRange](Patches/Campaign/Map/CampaignMapPatches.cs#L36) |
| <a id="hook-157"></a>H157 | `TaleWorlds.CampaignSystem.BarterSystem.BarterManager.CanPlayerBarterWithHero(Hero hero) : Boolean` | [UFO.Patch.NBC.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L125) |
| <a id="hook-158"></a>H158 | `TaleWorlds.CampaignSystem.Actions.KillCharacterAction.ApplyByOldAge(Hero victim, Boolean showNotification) : Void` | [UFO.Patch.NDOOA.Prefix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L144) |
| <a id="hook-159"></a>H159 | `TaleWorlds.CampaignSystem.Inventory.InventoryLogic.Initialize(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, Boolean isTrading, Boolean isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryCategoryType merchantItemType, IMarketData marketData, Boolean useBasePrices, InventoryMode inventoryMode, TextObject leftRosterName, TroopRoster leftMemberRoster, CapacityData otherSideCapacityData) : Void` | [UFO.Patch.NativeItemSpawning.Postfix](Patches/Inventory/Trading/InventoryTradingPatches.cs#L59) |
| <a id="hook-160"></a>H160 | `TaleWorlds.MountAndBlade.Mission.get_DoesMissionRequireCivilianEquipment() : Boolean` | [UFO.Patch.NeverRequireCivilianEquipment.DoesMissionRequireCivilianEquipment](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L264) |
| <a id="hook-161"></a>H161 | `TaleWorlds.CampaignSystem.GameComponents.DefaultBribeCalculationModel.IsBribeNotNeededToEnterKeep(Settlement settlement) : Boolean` | [UFO.Patch.NoBribeToEnterKeep.IsBribeNotNeededToEnterKeep](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L285) |
| <a id="hook-162"></a>H162 | `TaleWorlds.CampaignSystem.Actions.ChangeCrimeRatingAction.Apply(IFaction faction, Single deltaCrimeRating, Boolean showNotification) : Void` | [UFO.Patch.NoCrimeRatingForCrimes.Apply](Patches/Campaign/Kingdom/KingdomCampaignPatches.cs#L59) |
| <a id="hook-163"></a>H163 | `TaleWorlds.CampaignSystem.Actions.EndCaptivityAction.ApplyByEscape(Hero character, Hero facilitator, Boolean showNotification) : Void` | [UFO.Patch.NoPrisonerEscape.ApplyByEscape](Patches/Campaign/Party/PartyCampaignPatches.cs#L164) |
| <a id="hook-164"></a>H164 | `TaleWorlds.CampaignSystem.GameComponents.DefaultDiplomacyModel.GetRelationCostOfDisbandingArmy(Boolean isLeaderParty) : Int32` | [UFO.Patch.NoRelationshipLossOnDecisionArmyDisband.GetRelationCostOfDisbandingArmy](Patches/Campaign/Kingdom/KingdomCampaignPatches.cs#L80) |
| <a id="hook-165"></a>H165 | `TaleWorlds.CampaignSystem.GameComponents.DefaultDiplomacyModel.GetRelationCostOfExpellingClanFromKingdom() : Int32` | [UFO.Patch.NoRelationshipLossOnDecisionExpellingClan.GetRelationCostOfExpellingClanFromKingdom](Patches/Campaign/Kingdom/KingdomCampaignPatches.cs#L101) |
| <a id="hook-166"></a>H166 | `TaleWorlds.CampaignSystem.Election.KingdomElection.GetRelationChangeWithSponsor(Hero opposerOrSupporter, SupportWeights supportWeight, Boolean isOpposingSides) : Int32` | [UFO.Patch.NoRelationshipLossOnDecisionKingdomDecision.GetRelationChangeWithSponsor](Patches/Campaign/Kingdom/KingdomCampaignPatches.cs#L122) |
| <a id="hook-167"></a>H167 | `TaleWorlds.CampaignSystem.GameComponents.DefaultNotableSpawnModel.GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation) : Int32` | [UFO.Patch.NotableCount.Postfix](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L385) |
| <a id="hook-168"></a>H168 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartySpeedCalculatingModel.CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed) : ExplainedNumber` | [UFO.Patch.NpcMapSpeedPercentage.CalculateFinalSpeed](Patches/Campaign/Map/CampaignMapPatches.cs#L57) |
| <a id="hook-169"></a>H169 | `TaleWorlds.CampaignSystem.GameComponents.DefaultRomanceModel.GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest) : Int32` | [UFO.Patch.PA.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L165) |
| <a id="hook-170"></a>H170 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPregnancyModel.GetDailyChanceOfPregnancyForHero(Hero hero) : Single` | [UFO.Patch.PCM.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L184) |
| <a id="hook-171"></a>H171 | `TaleWorlds.CampaignSystem.Hero.GetRelation(Hero otherHero) : Int32` | [UFO.Patch.PR.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L203) |
| <a id="hook-172"></a>H172 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyHealingModel.GetDailyHealingHpForHeroes(PartyBase party, Boolean isPrisoners, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.PartyHealingMultiplierHeroes.GetDailyHealingHpForHeroes](Patches/Campaign/Party/PartyCampaignPatches.cs#L187) |
| <a id="hook-173"></a>H173 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyHealingModel.GetDailyHealingForRegulars(PartyBase party, Boolean isPrisoners, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.PartyHealingMultiplierTroops.GetDailyHealingForRegulars](Patches/Campaign/Party/PartyCampaignPatches.cs#L209) |
| <a id="hook-174"></a>H174 | `TaleWorlds.CampaignSystem.Party.MobileParty.get_ShouldBeIgnored() : Boolean` | [UFO.Patch.PartyInvisibleOnMap.ShouldBeIgnored](Patches/Campaign/Map/CampaignMapPatches.cs#L78) |
| <a id="hook-175"></a>H175 | `TaleWorlds.CampaignSystem.GameComponents.DefaultBattleRewardModel.GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero) : Int32` | [UFO.Patch.RGABM.Postfix](Patches/Characters/Relationships/CharacterRelationshipPatches.cs#L222) |
| <a id="hook-176"></a>H176 | `TaleWorlds.CampaignSystem.CampaignBehaviors.RebellionsCampaignBehavior.CheckRebellionEvent(Settlement settlement) : Boolean` | [UFO.Patch.RebellionChancePercentage.CheckRebellionEvent](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L306) |
| <a id="hook-177"></a>H177 | `TaleWorlds.CampaignSystem.GameComponents.DefaultTradeItemPriceFactorModel.GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, Boolean isSelling, Single inStoreValue, Single supply, Single demand) : Int32` | [UFO.Patch.SellingPriceMultiplier.GetPrice](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L327) |
| <a id="hook-178"></a>H178 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSiegeEventModel.GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side) : Single` | [UFO.Patch.SiegeBuildingSpeedMultiplier.GetConstructionProgressPerHour](Patches/Campaign/Siege/SiegeCampaignPatches.cs#L52) |
| <a id="hook-179"></a>H179 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel.GetSmithingCostsForWeaponDesign(WeaponDesign weaponDesign) : Int32[]` | [UFO.Patch.SmithingCostPercentage.GetSmithingCostsForWeaponDesign](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L124) |
| <a id="hook-180"></a>H180 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel.GetCraftingPartDifficulty(CraftingPiece craftingPiece) : Int32` | [UFO.Patch.SmithingDifficultyPercentage.GetCraftingPartDifficulty](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L150) |
| <a id="hook-181"></a>H181 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel.GetEnergyCostForRefining(ref RefiningFormula refineFormula, Hero hero) : Int32` | [UFO.Patch.SmithingEnergyCostPercentageRefining.GetEnergyCostForRefining](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L173) |
| <a id="hook-182"></a>H182 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel.GetEnergyCostForSmelting(ItemObject item, Hero hero) : Int32` | [UFO.Patch.SmithingEnergyCostPercentageSmelting.GetEnergyCostForSmelting](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L196) |
| <a id="hook-183"></a>H183 | `TaleWorlds.CampaignSystem.GameComponents.DefaultSmithingModel.GetEnergyCostForSmithing(ItemObject item, Hero hero) : Int32` | [UFO.Patch.SmithingEnergyCostPercentageSmithing.GetEnergyCostForSmithing](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L219) |
| <a id="hook-184"></a>H184 | `SandBox.Tournaments.MissionLogics.TournamentBehavior.GetMaximumBet() : Int32` | [UFO.Patch.TournamentMaximumBetMultiplier.GetMaximumBet](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L348) |
| <a id="hook-185"></a>H185 | `TaleWorlds.CampaignSystem.Settlements.Town.OnInit() : Void` | [UFO.Patch.Town_OnInit_Patch.Postfix](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L429) |
| <a id="hook-186"></a>H186 | `TaleWorlds.CampaignSystem.GameComponents.DefaultCombatXpModel.GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, Int32 damage, Boolean isFatal, MissionTypeEnum missionType) : ExplainedNumber` | [UFO.Patch.TroopExperienceMultiplier.Postfix](Patches/Progression/Experience/ExperienceProgressionPatches.cs#L135) |
| <a id="hook-187"></a>H187 | `TaleWorlds.CampaignSystem.GameComponents.DefaultPartyWageModel.GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, Boolean includeDescriptions) : ExplainedNumber` | [UFO.Patch.TroopWagesPercentage.Postfix](Patches/Campaign/Party/PartyCampaignPatches.cs#L231) |
| <a id="hook-188"></a>H188 | `TaleWorlds.CampaignSystem.CampaignBehaviors.CraftingCampaignBehavior.IsOpened(CraftingPiece craftingPiece, CraftingTemplate craftingTemplate) : Boolean` | [UFO.Patch.UnlockAllParts.IsOpened](Patches/Smithing/Crafting/SmithingCraftingPatches.cs#L242) |
| <a id="hook-189"></a>H189 | `TaleWorlds.CampaignSystem.Settlements.Village.OnInit() : Void` | [UFO.Patch.Village_OnInit_Patch.Postfix](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L420) |
| <a id="hook-190"></a>H190 | `TaleWorlds.CampaignSystem.Actions.StartBattleAction.ApplyStartRaid(MobileParty attackerParty, Settlement settlement) : Void` | [UFO.Patch.VillagesNeverRaided.ApplyStartRaid](Patches/Campaign/Settlement/SettlementCampaignPatches.cs#L370) |
| <a id="hook-191"></a>H191 | `TaleWorlds.CampaignSystem.GameComponents.DefaultWorkshopModel.GetCostForPlayer(Workshop workshop) : Int32` | [UFO.Patch.WorkshopBuyingCostPercentage.GetCostForPlayer](Patches/Campaign/Workshop/WorkshopCampaignPatches.cs#L14) |
| <a id="hook-192"></a>H192 | `TaleWorlds.CampaignSystem.Settlements.Workshops.Workshop.get_Expense() : Int32` | [UFO.Patch.WorkshopDailyExpensePercentage.Expense](Patches/Campaign/Workshop/WorkshopCampaignPatches.cs#L36) |
| <a id="hook-193"></a>H193 | `TaleWorlds.CampaignSystem.GameComponents.DefaultWorkshopModel.GetCostForNotable(Workshop workshop) : Int32` | [UFO.Patch.WorkshopSellingCostMultiplier.GetSellingCost](Patches/Campaign/Workshop/WorkshopCampaignPatches.cs#L58) |
| <a id="hook-194"></a>H194 | `NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel.GetAdditionalAmmoBonus(Ship ship) : Int32 (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.AdditionalAmmo](Patching/NavalDlcCompatibility.cs#L203) |
| <a id="hook-195"></a>H195 | `NavalDLC.GameComponents.NavalDLCCampaignShipDamageModel.GetShipDamage(Ship ship, Ship rammingShip, Single rawDamage) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.BattleShipDamage](Patching/NavalDlcCompatibility.cs#L207) |
| <a id="hook-196"></a>H196 | `NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel.GetCampaignSpeedBonusFactor(Ship ship) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.CampaignSpeedBonus](Patching/NavalDlcCompatibility.cs#L193) |
| <a id="hook-197"></a>H197 | `NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel.GetCrewCapacityBonusFactor(Ship ship) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.CrewCapacity](Patching/NavalDlcCompatibility.cs#L199) |
| <a id="hook-198"></a>H198 | `NavalDLC.GameComponents.NavalDLCShipDeploymentModel.GetShipDeploymentLimit(MobileParty party) : Int32 (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.DeploymentLimit](Patching/NavalDlcCompatibility.cs#L222) |
| <a id="hook-199"></a>H199 | `NavalDLC.GameComponents.NavalDLCFleetManagementModel.get_MinimumTroopCountRequiredToSendShips() : Int32 (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.FleetMinimumTroops](Patching/NavalDlcCompatibility.cs#L224) |
| <a id="hook-200"></a>H200 | `NavalDLC.GameComponents.NavalDLCBattleRewardModel.CalculateInfluenceGain(PartyBase winnerParty, Single influenceValueOfBattleForWinnerSide, Single contributionShareOfWinnerParty, Single influenceMultiplierForWinnerSide, Boolean includeDescriptions) : ExplainedNumber (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.NavalInfluence](Patching/NavalDlcCompatibility.cs#L230) |
| <a id="hook-201"></a>H201 | `NavalDLC.GameComponents.NavalDLCBattleRewardModel.CalculateRenownGain(PartyBase winnerParty, Single renownValueOfBattleForWinnerSide, Single contributionShareOfWinnerParty, Single renownMultiplierForWinnerSide, Boolean includeDescriptions) : ExplainedNumber (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.NavalRenown](Patching/NavalDlcCompatibility.cs#L228) |
| <a id="hook-202"></a>H202 | `NavalDLC.GameComponents.NavalDLCCombatSimulationModel.SimulateHit(Ship strikerShip, Ship struckShip, PartyBase strikerParty, PartyBase struckParty, SiegeEngineType siegeEngine, Single strikerAdvantage, MapEvent battle, out Int32 troopCasualties) : ExplainedNumber (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.NavalSimulationProtection](Patching/NavalDlcCompatibility.cs#L239) |
| <a id="hook-203"></a>H203 | `NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel.GetMaxOarForceFactor(Ship ship) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.OarForce](Patching/NavalDlcCompatibility.cs#L195) |
| <a id="hook-204"></a>H204 | `NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel.GetSailForceFactor(Ship ship) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.SailForce](Patching/NavalDlcCompatibility.cs#L197) |
| <a id="hook-205"></a>H205 | `NavalDLC.GameComponents.NavalDLCCampaignShipDamageModel.GetHourlyShipDamage(MobileParty owner, Ship ship) : Int32 (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.SeaAttrition](Patching/NavalDlcCompatibility.cs#L205) |
| <a id="hook-206"></a>H206 | `NavalDLC.GameComponents.NavalDLCCampaignShipParametersModel.GetDefaultCombatFactor(ShipHull shipHull) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.ShipCombatFactor](Patching/NavalDlcCompatibility.cs#L201) |
| <a id="hook-207"></a>H207 | `NavalDLC.GameComponents.NavalDLCShipCostModel.GetShipTradeValue(Ship ship, PartyBase seller, PartyBase buyer) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.ShipPurchaseCost](Patching/NavalDlcCompatibility.cs#L216) |
| <a id="hook-208"></a>H208 | `NavalDLC.GameComponents.NavalDLCShipCostModel.GetShipRepairCost(Ship ship, PartyBase owner) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.ShipRepairCost](Patching/NavalDlcCompatibility.cs#L218) |
| <a id="hook-209"></a>H209 | `NavalDLC.GameComponents.NavalDLCShipCostModel.GetShipUpgradePieceCost(Ship ship, ShipUpgradePiece piece, PartyBase owner) : Int32 (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.ShipUpgradeCost](Patching/NavalDlcCompatibility.cs#L220) |
| <a id="hook-210"></a>H210 | `NavalDLC.GameComponents.NavalDLCStormModel.GetPositionDamageForStorm(Storm storm, Vec2 shipPosition, Ship ship) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.StormDamage](Patching/NavalDlcCompatibility.cs#L209) |
| <a id="hook-211"></a>H211 | `NavalDLC.GameComponents.NavalDLCStormModel.GetHourlyStormSpawnChanceForPosition(Vec2 position) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.StormFrequency](Patching/NavalDlcCompatibility.cs#L211) |
| <a id="hook-212"></a>H212 | `NavalDLC.GameComponents.NavalDLCStormModel.GetHourlyIntensityChangeForStorm(Storm storm) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.StormStrength](Patching/NavalDlcCompatibility.cs#L213) |
| <a id="hook-213"></a>H213 | `NavalDLC.GameComponents.NavalDLCStormModel.GetMaximumWeatherStrengthAtEye(Storm storm) : Single (optional DLC)` | [UFO.Patching.NavalDlcCompatibility.StormStrength](Patching/NavalDlcCompatibility.cs#L213) |

## 自定义物品与数据入口

物品 XML、锻造模板、部件和武器描述由 `Module/SubModule.xml` 注册；`Module/AssetPackages/pack0.tpac` 提供龙首槊模型。

| 物品 | 机制 / 数据关联 | 定义文件 |
| --- | --- | --- |
| 帝皇标枪<br>`super_javelin` | 锻造模板 `Javelin_tmp`；对应 CraftingPieces、CraftingTemplates、WeaponDescriptions 目录。 | [javelin.xml](Module/ModuleData/Items/javelin.xml) |
| 龙首槊<br>`loongspear` | 锻造模板 `loongspear1`；对应 CraftingPieces、CraftingTemplates、WeaponDescriptions 目录。 | [loongspear.xml](Module/ModuleData/Items/loongspear.xml) |
| 霸王箭<br>`bawangjian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, CanPenetrateShield, BonusAgainstShield, CanKnockDown, AffectsAreaBig, MultiplePenetration | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 高爆箭<br>`gaobaojian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, CanKnockDown, AffectsAreaBig | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 聚能箭<br>`junengjian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, BonusAgainstShield, CanKnockDown | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 贯透箭<br>`guantoujian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, CanPenetrateShield, CanKnockDown, MultiplePenetration | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 爆炸箭<br>`baozhajian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, AffectsAreaBig | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 爆裂箭<br>`baoliejian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, BonusAgainstShield, AffectsAreaBig | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 爆穿箭<br>`baochuanjian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, CanPenetrateShield, MultiplePenetration, AffectsAreaBig | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 破盾箭<br>`podunjian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, BonusAgainstShield | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 穿盾箭<br>`chuandunjian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, CanPenetrateShield | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 穿透箭<br>`chuantoujian` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, Burning, LeavesTrail, MultiplePenetration | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 扈从穿透箭<br>`chuantoujian2` | Consumable, AmmoSticksWhenShot, AmmoBreaksOnBounceBack, LeavesTrail, CanPenetrateShield, MultiplePenetration | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 毁灭矢<br>`huimieshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, CanPenetrateShield, BonusAgainstShield, CanKnockDown, AffectsAreaBig, MultiplePenetration, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 高爆矢<br>`gaobaoshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, CanKnockDown, AffectsAreaBig, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 聚能矢<br>`junengshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, BonusAgainstShield, CanKnockDown, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 贯穿矢<br>`guanchuanshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, CanPenetrateShield, CanKnockDown, MultiplePenetration, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 爆炸矢<br>`baozhashi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, AffectsAreaBig, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 爆裂矢<br>`baolieshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, BonusAgainstShield, AffectsAreaBig, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 爆穿矢<br>`baochuanshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, CanPenetrateShield, MultiplePenetration, AffectsAreaBig, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 破盾矢<br>`podunshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, BonusAgainstShield, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 穿盾矢<br>`chuandunshi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, CanPenetrateShield, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 穿透矢<br>`chuantoushi` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, MultiplePenetration, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |
| 扈从穿透矢<br>`chuantoushi2` | AttachAmmoToVisual, Consumable, AmmoSticksWhenShot, MultiplePenetration, AmmoBreaksOnBounceBack | [ranged_ammunition.xml](Module/ModuleData/Items/ranged_ammunition.xml) |

<!-- END GENERATED FEATURES -->
