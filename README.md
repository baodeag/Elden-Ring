# ⚔️ Eternal Hollow

**Eternal Hollow** is a 3D dark fantasy action RPG built in Unity, inspired by soulslike combat loops: explore dangerous worlds, defeat enemies, collect runes, upgrade your character, and conquer bosses to unlock the next map.

## 🕯️ Game Overview

The player begins by creating a character and choosing one of five starting classes:

- **KNIGHT** - balanced melee starter
- **RANGER** - fast bow skirmisher
- **VANGUARD** - heavy bruiser with strong early melee pressure
- **MYSTIC** - ranged magic caster
- **CONFESSOR** - faith-based melee hybrid

The main objective is to progress through **5 maps**, defeat each map boss, unlock the next area, and finally clear the last boss to win the game.

## 🎮 Core Gameplay

- Explore dungeon-style fantasy environments
- Fight AI enemies and bosses with stamina-based combat
- Use light attacks, heavy attacks, blocking, dodging, jumping, sneaking, lock-on, ranged weapons, and spells
- Collect runes from combat and use them for progression
- Upgrade stats, weapons, armor, and equipment
- Rest at **Sites of Grace**
- Interact with NPCs, merchants, pickups, elevators, dialogue, and world transitions
- Save and load character progress across multiple save slots

## 🧰 Technologies Used

- **Unity 6** `6000.0.26f1`
- **C#**
- **Universal Render Pipeline (URP) 17**
- **Unity Input System**
- **Netcode for GameObjects**
- **Unity Relay & Authentication**
- **Unity AI Navigation**
- **UGUI / TextMeshPro**
- **Unity Post Processing**
- **ProBuilder**
- **ParrelSync** for multiplayer/local clone testing
- Custom ScriptableObject-based systems for items, weapons, AI states, spells, shops, progression, and save data

## ✨ Main Features

- 🧍 Character creation with class preview, stats, loadout, hair, color, body type, and name
- ⚔️ Melee, ranged, shield, spell, and two-hand weapon systems
- 🛡️ Armor and equipment model swapping
- 🧠 AI enemy state system with idle, patrol, pursuit, investigation, combat stance, and attack states
- 👑 Boss fight triggers, boss HP UI, rewards, and map unlock progression
- 🧪 Status effects: poison, bleed, frost, fire, buffs, stamina damage, blocked damage, and critical damage
- 🛒 Merchant shop system with buying, selling, stock, rune prices, and progression scaling
- 💾 Save/load system for stats, equipment, inventory, runes, bosses defeated, unlocked maps, merchants, dialogue, and active buffs
- 🌍 5-world progression structure: `World_01` to `World_05`
- 🎚️ Settings menu for audio, graphics, resolution, fullscreen, quality, and sensitivity
- 🌐 Singleplayer and multiplayer menu flow with host/join support

## 🗺️ Progression Loop

1. Choose a starting class
2. Enter the first world
3. Fight enemies and collect runes
4. Find items, weapons, armor, and upgrade materials
5. Rest and level up at Sites of Grace
6. Defeat the map boss
7. Unlock the next world
8. Repeat until all 5 maps are cleared

## ⌨️ Keyboard & Mouse Controls

| Action | Input |
|---|---|
| Move | `W A S D` |
| Camera | Mouse movement |
| Light Attack / Right-hand Action | Left Mouse Button |
| Block / Left-hand Action | Right Mouse Button |
| Heavy Attack / Weapon Skill | `F` |
| Left Trigger Action | `C` |
| Dodge | `Alt` |
| Jump | `Space` |
| Sprint | Hold `Shift` |
| Sneak | `Ctrl` |
| Interact | `E` |
| Two-hand Weapon Modifier | Hold `E` |
| Lock On | `Q` |
| Switch Lock-on Target | `,` / `.` |
| Switch Right Weapon | Right Arrow |
| Switch Left Weapon | Left Arrow |
| Switch Quick Slot Item | Down Arrow |
| Use Quick Slot Item | `X` |
| Character Menu | `Esc` |

## 🚀 How To Run

1. Install **Unity 6000.0.26f1**.
2. Clone this repository.
3. Open the project folder in Unity Hub.
4. Let Unity restore all packages from `Packages/manifest.json`.
5. Open the main menu scene:

   ```text
   Assets/Game/Scenes/Main_Menu_01.unity
