"""Synchronize UFO localization files from Chinese.resx and generate translations."""
import json
import sys
import time
import urllib.parse
import urllib.request
import xml.etree.ElementTree as ET
from copy import deepcopy
from pathlib import Path


ROOT = Path(__file__).resolve().parents[2] / "Module" / "bin" / "Win64_Shipping_Client"
SOURCE = ROOT / "Chinese.resx"


def entries(tree):
    return {node.attrib["name"]: node for node in tree.getroot().findall("data")}


def translate(texts, target_language):
    cache = ROOT / f".{target_language}.translation-cache.json"
    translated = json.loads(cache.read_text(encoding="utf-8")) if cache.exists() else []
    for start in range(len(translated), len(texts)):
        text = texts[start]
        batch = [text]
        query = urllib.parse.urlencode([( "client", "gtx"), ("sl", "zh-CN"), ("tl", target_language), ("dt", "t"), ("q", text)])
        url = "https://translate.googleapis.com/translate_a/single?" + query
        with urllib.request.urlopen(url, timeout=30) as response:
            payload = json.loads(response.read().decode("utf-8"))
        result = "".join(piece[0] for piece in payload[0])
        translated.append(result)
        cache.write_text(json.dumps(translated, ensure_ascii=False), encoding="utf-8")
        if (start + 1) % 25 == 0 or start + 1 == len(texts): print(f"{target_language}: {start + 1}/{len(texts)}", flush=True)
        time.sleep(0.05)
    cache.unlink(missing_ok=True)
    return translated


def write_translation(source_tree, language, output_name):
    values = [node.findtext("value", default="") for node in source_tree.getroot().findall("data")]
    output_tree = deepcopy(source_tree)
    for node, value in zip(output_tree.getroot().findall("data"), translate(values, language)):
        node.find("value").text = value
    ET.indent(output_tree, space="  ")
    output_tree.write(ROOT / output_name, encoding="utf-8", xml_declaration=True)


def synchronize_existing(source_tree):
    source_entries = entries(source_tree)
    fallback_values = {
        "UFOs_GroupName": {"English": "UFO General Settings", "Other": "UFO General Settings", "Portuguese": "Configurações gerais do UFO"},
        "Enum_Setting_Language_Spanish": {"Chinese": "西班牙语", "English": "Spanish", "Other": "Spanish", "Portuguese": "Espanhol", "Russian": "Испанский", "Spanish": "Español", "Japanese": "スペイン語"},
        "Enum_Setting_Language_Japanese": {"Chinese": "日语", "English": "Japanese", "Other": "Japanese", "Portuguese": "Japonês", "Russian": "Японский", "Spanish": "Japonés", "Japanese": "日本語"},
    }
    for language in ("Chinese", "English", "Other", "Portuguese", "Russian", "Spanish", "Japanese"):
        path = ROOT / f"{language}.resx"
        tree = ET.parse(path)
        current = entries(tree)
        required_entries = dict(source_entries)
        for key in ("Enum_Setting_Language_Spanish", "Enum_Setting_Language_Japanese"):
            required_entries[key] = deepcopy(source_entries["Enum_Setting_Language_English"])
            required_entries[key].attrib["name"] = key
        for key, source_node in required_entries.items():
            if key not in current:
                node = deepcopy(source_node)
                node.find("value").text = fallback_values.get(key, {}).get(language, source_node.findtext("value", default=""))
                tree.getroot().append(node)
        ET.indent(tree, space="  ")
        tree.write(path, encoding="utf-8", xml_declaration=True)


if __name__ == "__main__":
    source_tree = ET.parse(SOURCE)
    synchronize_existing(source_tree)
    if not (ROOT / "Spanish.resx").exists():
        write_translation(source_tree, "es", "Spanish.resx")
    if not (ROOT / "Japanese.resx").exists():
        write_translation(source_tree, "ja", "Japanese.resx")
