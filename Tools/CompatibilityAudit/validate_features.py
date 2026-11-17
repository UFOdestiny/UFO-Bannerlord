"""Audit exposed settings, active consumers, and fallback localization.

This is source coverage, not evidence that a feature was exercised in-game.
"""
from pathlib import Path
import argparse
import json
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]


def uncomment(text):
    # Preserve strings (including URLs) while dropping disabled C# code.
    return re.sub(r'@?"(?:""|\\.|[^"\\])*"|//[^\n]*|/\*.*?\*/',
                  lambda m: "" if m[0].startswith(("//", "/*")) else m[0],
                  text, flags=re.S)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--source-audit", type=Path)
    parser.add_argument("--targets", type=Path)
    parser.add_argument("--update-readme", action="store_true")
    args = parser.parse_args()
    sources = {p: uncomment(p.read_text(encoding="utf-8-sig"))
               for folder in ("Patches", "Extensions", "Behaviors", "Bootstrap", "Models", "Infrastructure", "Patching", "Localization")
               for p in (ROOT / folder).rglob("*.cs")}
    resources = {}
    issues = []
    for p in (ROOT / "Module/bin/Win64_Shipping_Client").glob("*.resx"):
        items = ET.parse(p).getroot().findall("data")
        values = {item.attrib["name"]: item.findtext("value", "") for item in items}
        if len(values) != len(items):
            issues.append(f"Duplicate resource key: {p.name}")
        resources[p.stem] = values
    sources[ROOT / "SubModule.cs"] = uncomment((ROOT / "SubModule.cs").read_text(encoding="utf-8-sig"))
    english = resources["English"]
    chinese = resources["Chinese"]
    manager = uncomment((ROOT / "Settings/SettingsManager.cs").read_text(encoding="utf-8-sig"))
    settings = re.findall(r"public static CheatValue<[^>]+>\s+(\w+)", manager)
    consumers = {name: [p.relative_to(ROOT).as_posix() for p, text in sources.items()
                        if re.search(r"\bSettingsManager\." + name + r"\b", text)] for name in settings}
    for name, files in consumers.items():
        if not files:
            issues.append(f"No active consumer: {name}")
    exposed = {}
    groups = {}
    for file in ("GlobalSettings.cs", "PerCampaignSettings.cs"):
        text = uncomment((ROOT / "Settings" / file).read_text(encoding="utf-8-sig"))
        for kind, key in re.findall(r'\[LocalizedSettingProperty(\w+)\("([^"]+)"', text):
            if kind == "Group":
                group = key
                required = [key + "_GroupName"]
            else:
                exposed.setdefault(key, set()).add(file)
                groups[key] = group
                required = [key + "_Name", key + "_Desc"]
                if kind == "Button": required.append(key + "_Button")
            for resource_key in required:
                if not english.get(resource_key):
                    issues.append(f"Missing English fallback: {resource_key}")
    for key in set(settings) - exposed.keys():
        issues.append(f"Setting is not exposed: {key}")
    required_runtime = set()
    for text in sources.values():
        required_runtime.update(re.findall(r'(?:GetText|GetTextFormat)\("([^"]+)"', text))
        required_runtime.update(re.findall(r'"(HotkeyTips\w+)"', text))
    for key in required_runtime:
        if key not in english: issues.append(f"Missing runtime localization: {key}")
    for language, values in resources.items():
        if values.keys() != english.keys(): issues.append(f"Localization key set differs: {language}")
        for key, value in values.items():
            if not value.strip() or '\ufffd' in value: issues.append(f"Empty or corrupt localization: {language}/{key}")
            if sorted(re.findall(r'\{\d+\}', value)) != sorted(re.findall(r'\{\d+\}', english.get(key, ''))):
                issues.append(f"Localization placeholders differ: {language}/{key}")
    if args.source_audit and args.targets:
        update_readme(args, exposed, groups, chinese, issues)
    print(f"Feature audit: {len(settings)} settings, {len(exposed)} MCM entries, {len(resources)} resource files.")
    for issue in issues:
        print("FAIL " + issue)
    print(f"Feature audit issues: {len(issues)}")
    return bool(issues)


def update_readme(args, exposed, groups, chinese, issues):
    audit = json.loads(args.source_audit.read_text(encoding="utf-8-sig"))
    functions = {function["Symbol"]: function for function in audit["Functions"]}
    settings_by_function = {name: set(function["Settings"]) for name, function in functions.items()}
    changed = True
    while changed:
        changed = False
        for name, function in functions.items():
            reached = set(settings_by_function[name])
            for call in function["Calls"]: reached.update(settings_by_function.get(call, []))
            if reached != settings_by_function[name]: settings_by_function[name] = reached; changed = True
    hook_sources = {hook["Symbol"].split("(")[0]: hook for hook in audit["Hooks"]}
    bindings = []
    for line in args.targets.read_text(encoding="utf-8-sig").splitlines():
        if " -> " not in line: continue
        callback, target = line.split(" -> ", 1)
        source = hook_sources.get(callback.replace("+", "."))
        if source is None: issues.append(f"README hook source missing: {callback}"); continue
        bindings.append((callback, target, source))
    bindings.sort(key=lambda row: (row[0], row[1]))
    by_setting = {}
    for index, (_, _, source) in enumerate(bindings, 1):
        for setting in settings_by_function.get(source["Symbol"], source["Settings"]):
            by_setting.setdefault(setting, []).append(index)
    def link(function):
        file = Path(function["File"]).relative_to(ROOT).as_posix()
        name = function["Symbol"].split("(")[0]
        return f"[{name}]({file}#L{function['Line']})"
    def cell(text): return text.replace("|", "\\|").replace("\n", "<br>")
    begin, end = "<!-- BEGIN GENERATED FEATURES -->", "<!-- END GENERATED FEATURES -->"
    lines = [begin, "## 全部功能与配置", "", f"{len(exposed)} 个 MCM 展示项；Hook 编号可跳到下方的精确目标、补丁方法和源码位置。调用关系由 Roslyn 语义模型追踪，覆盖共用 helper。", ""]
    action_targets = {
        "NavalShipId": "NavalGrantShip 的输入框；船体 ID。",
        "NavalFigureheadId": "NavalUnlockFigurehead 的输入框；船首像 ID。",
        "NavalShipUpgradePieceId": "NavalAddShipUpgradePieces 的输入框；升级件 ID。",
        "NavalGrantShip": "NavalDlcCompatibility.GrantShip → NavalDLCCheats.AddShipToPlayer",
        "NavalGrantAllShips": "NavalDlcCompatibility.GrantAllShips → NavalDLCCheats.AddShipToPlayer",
        "NavalUnlockFigurehead": "NavalDlcCompatibility.UnlockFigurehead → NavalDLCCheats.UnlockFigurehead",
        "NavalUnlockAllFigureheads": "NavalDlcCompatibility.UnlockAllFigureheads → NavalDLCCheats.UnlockFigurehead",
        "NavalAddShipUpgradePieces": "NavalDlcCompatibility.AddShipUpgradePieces → NavalDLCHelpers.AddUpgradePiecesToPartyShips",
        "NavalAddAllShipUpgradePieces": "NavalDlcCompatibility.AddAllShipUpgradePieces → NavalDLCHelpers.AddUpgradePiecesToPartyShips"
    }
    events = {"RecruitExileClans": "OnSessionLaunchedEvent → AddPlayerLine 招募对话条件", "AddMoneyThreshhold": "DailyTickEvent", "AddMoney_count": "DailyTickEvent", "LanguageSetting": "OnBeforeInitialModuleScreenSetAsRoot → L10N.LoadLanguage"}
    for group in dict.fromkeys(groups.values()):
        lines += [f"### {chinese.get(group + '_GroupName', group)}", "", "| 功能 / 配置键 | 效果与范围 | Hook / 行为入口 |", "| --- | --- | --- |"]
        for key in sorted(key for key in exposed if groups[key] == group):
            hooks = ", ".join(f"[H{index:03}](#hook-{index:03})" for index in by_setting.get(key, []))
            if key in action_targets:
                hooks = f"[{action_targets[key]}](Patching/NavalDlcCompatibility.cs)"
            if not hooks:
                direct = [function for function in functions.values() if key in function["Settings"] and not function["File"].endswith("SettingsManager.cs")]
                hooks = events.get(key, "") + "<br>" + "<br>".join(link(function) for function in direct)
                if not direct: issues.append(f"README feature entry missing: {key}")
            lines.append(f"| {cell(chinese.get(key + '_Name', key))}<br>`{key}` | {cell(chinese.get(key + '_Desc', ''))} | {hooks} |")
        lines.append("")
    lines += ["## 精确 Hook 索引", "", "目标参数名、类型、ref/out/in 和返回类型取自当前安装 DLL；getter/setter 显示实际方法名。`optional DLC` 入口按程序集是否加载注册。修改游戏接口时，按此表先找目标，再改对应源码，并运行完整验证。", "", "| 编号 | 当前游戏目标 | UFO 实现位置 |", "| --- | --- | --- |"]
    for index, (_, target, source) in enumerate(bindings, 1):
        lines.append(f'| <a id="hook-{index:03}"></a>H{index:03} | `{target}` | {link(source)} |')
    lines += ["", "## 自定义物品与数据入口", "", "物品 XML、锻造模板、部件和武器描述由 `Module/SubModule.xml` 注册；`Module/AssetPackages/pack0.tpac` 提供龙首槊模型。", "", "| 物品 | 机制 / 数据关联 | 定义文件 |", "| --- | --- | --- |"]
    content = {node.attrib['id']:node.attrib['text'] for node in ET.parse(ROOT / 'Module/ModuleData/Languages/CNs/ufo_content-zho-CN.xml').getroot().findall('strings/string')}
    for file in sorted((ROOT / 'Module/ModuleData/Items').glob('*.xml')):
        for item in ET.parse(file).getroot():
            if item.tag not in ('Item', 'CraftedItem'): continue
            name = item.attrib['name']; token = re.match(r'\{=([^}]+)\}', name)
            if token: name = content.get(token[1], name[token.end():])
            if item.tag == 'CraftedItem':
                mechanics = '锻造模板 `' + item.attrib['crafting_template'] + '`；对应 CraftingPieces、CraftingTemplates、WeaponDescriptions 目录。'
            else:
                weapon = item.find('ItemComponent/Weapon'); flags = weapon.find('WeaponFlags') if weapon is not None else None
                mechanics = ', '.join(k for k,v in flags.attrib.items() if v == 'true') if flags is not None else ''
            path = file.relative_to(ROOT).as_posix()
            lines.append(f"| {name}<br>`{item.attrib['id']}` | {mechanics} | [{file.name}]({path}) |")
    lines += ["", end]
    generated = '\n'.join(lines)
    path = ROOT / 'README.md'; text = path.read_text(encoding='utf-8-sig')
    match = re.search(re.escape(begin) + r'.*?' + re.escape(end), text, re.S)
    if match is None: issues.append('README generated section markers missing'); return
    expected = text[:match.start()] + generated + text[match.end():]
    if args.update_readme: path.write_bytes(expected.encode('utf-8'))
    elif text != expected: issues.append('README feature/hook inventory is stale; run validation with -UpdateReadme')


if __name__ == "__main__":
    raise SystemExit(main())
