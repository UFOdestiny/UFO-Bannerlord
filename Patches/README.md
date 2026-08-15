# Patch layout

Patches are grouped first by the game system that owns the patched API, then by the
specific feature area. This keeps discovery by `PatchBootstrapper` unchanged because
all source files remain part of the same `UFO` assembly.

- `Campaign/*` — world-map, party, clan, kingdom, settlement, siege, army, and workshop rules.
- `Characters/*` — character attributes and relationship, marriage, and conversation rules.
- `Combat/Battle` — battle damage, casualties, rewards, agent state, and simulation rules.
- `Combat/Morale` — morale, routing, and retreat behaviour.
- `Combat/Weapons` — weapon interactions, momentum, crush-through, and cutting rules.
- `Combat/Attributes` — combat attribute enhancements.
- `Inventory/Trading` — inventory capacity and cheat-shop behaviour.
- `Progression/*` — experience and attribute strategy progression.
- `Smithing/Crafting` — crafting costs, parts, and crafted weapon modifiers.
- `UI/Hotkeys` — UI injection and hotkey handlers.
