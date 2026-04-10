# Elden Ring Project Roadmap

Tai lieu nay luu cac hang muc can lam de hoan thien he thong game.
Cap nhat file nay moi khi chot them tinh nang, doi uu tien, hoac hoan thanh dau viec.

> **Cap nhat lan cuoi: 2026-04-10 (sau khi ra soat lai project, chot fix class armor leg/runtime mapping, va bo sung HUD feedback cho buff charm bang prefab UI)**

---

## North Star

Muc tieu gameplay chinh:
- Nguoi choi chon 1 trong 5 nhan vat khoi dau.
- Vao game, danh quai de farm rune.
- Dung rune de nang cap nhan vat, vu khi, trang bi.
- Vuot qua boss cua tung map de mo khoa map tiep theo.
- Moi map sau kho hon map truoc, quai manh hon, thu thach hon.
- Vuot qua du 5 map thi chien thang game.

---

## Trang Thai Hien Tai (2026-04-10)

### Da Hoan Thanh
- [x] Core character system (Player + AI): stats, combat, locomotion, animation, network
- [x] AI enemy system: states, patrol, attack actions, spawner
- [x] Boss character (Durk): `AIBossCharacterManager`, `AIDurkCharacterManager`, combat + sound
- [x] Rune reward on boss death (da fix trong conversation truoc)
- [x] Shop system: `ShopInteractable`, `ShopInventory`, `ShopStockEntry`
- [x] Shop UI: `PlayerUIShopManager` trong `Player UI Manager.prefab`
- [x] Mua/ban item bang rune, save merchant stock
- [x] Lock input gameplay khi mo shop, ESC de back
- [x] Fix bug spam sell duplicate rune
- [x] Settings menu (title screen + in-game character menu): `GameSettingsManager`, `TitleScreenSettingsMenuManager`, `PlayerUISettingsManager`
- [x] Save/load settings persistent
- [x] Character save data day du: stats, equipment, inventory, runes, boss, sites of grace, merchant stock, dialogue
- [x] Level up UI: `PlayerUILevelUpManager`
- [x] Weapon upgrade UI: `PlayerUIWeaponUpgradeManager`
- [x] Equipment inventory UI: `PlayerUIEquipmentManager`
- [x] HUD (HP/Stamina/FP bars, boss HP bar, status effects): `PlayerUIHudManager`
- [x] Site of Grace system: `WorldLocationManager`, rest mechanic
- [x] World scene progression foundation: `World_01` -> `World_05` da co trong build settings va co the transition duoc
- [x] Main Menu scene: Title screen + load menu + settings
- [x] Character creation flow da mo rong du 5 class: `Knight`, `Ranger`, `Vanguard`, `Mystic`, `Confessor`
- [x] Preview class trong new game flow da co ten class, stat summary, loadout summary, va apply equipment khi hover/chon
- [x] `Knight` da duoc giu lai outfit goc; cac class con lai da co data outfit rieng theo mesh set Polygon Hero
- [x] 5 nut chon class trong character creation da dung UI co san trong `Title Screen Canvas`, khong con generate runtime button bang code
- [x] Class review panel (title/subtitle/description/stats/loadout/hint) da dung UI co san trong `Title Screen Canvas`, khong con runtime-generated overlay
- [x] Da sua lai mapping nut `Mystic` / `Confessor` de khop dung class ID
- [x] Da fix runtime class armor leg visibility cho cac class ngoai `Knight`; preview va vao game khong con bi mat chan do mapping/model activation
- [x] He thong buff charm da hoat dong day du cho 4 buff mac dinh (HP / Stamina / FP / Damage), moi buff co timed effect rieng va co save/load buff dang chay
- [x] HUD buff feedback da co popup va active buff icon bang prefab UI trong `Player UI Manager.prefab`

### Chua Co / Chua Hoan Thanh
- [ ] Chot visual polish cuoi cho character selection (class outfit fidelity, model preview consistency, UI wording)
- [x] Multiple world scenes foundation (`World_01` -> `World_05`) da scaffold va add vao build settings
- [x] Boss clear -> unlock map -> load scene/teleport sang map tiep theo da hoat dong
- [x] Difficulty ramp theo tung map (nen tang da co)
- [ ] Win screen / victory scene rieng
- [ ] Custom merchant stock da dien du lieu trong inspector
- [ ] Economy balance (rune drop, level up cost, shop price)
- [ ] UI progression day du (map current, map unlocked, victory screen)
- [ ] Merchant data pass that trong world/prefab (merchantID, stock, required tier)
- [ ] Ra soat them 1 vong visual polish cho class-specific armor set sau khi da fix leg/runtime mapping (male/female, preview/in-game)

### Moi Hoan Thanh Trong Code
- [x] `GameProgressionManager` singleton theo doi `startingClassID`, `currentMapIndex`, `mapsUnlocked`, `gameWon`
- [x] `GameProgressionConfig` ScriptableObject de cau hinh map progression bang asset
- [x] Mo rong `CharacterSaveData` voi progression data
- [x] `TitleScreenManager` luu starting class da chon khi New Game
- [x] Boss clear -> unlock next map -> save progression -> thong bao `Map Unlocked`
- [x] Co support same-scene progression bang `entrySiteOfGraceID`
- [x] Co support scene transition neu map sau dung scene build index khac
- [x] Co popup `Victory Achieved` + flow quay ve title sau khi thang boss map cuoi
- [x] Co runtime warning neu map progression data chua duoc setup trong Inspector
- [x] Da sua popup `Map Unlocked` hien thi on dinh sau boss death
- [x] `GameProgressionManager` dong bo `Map Definitions` tu `GameProgressionConfig` ngay trong editor
- [x] `Game Progression Config.asset` da duoc tao va co the dung de test progression ngay
- [x] `Game Progression Config.asset` da duoc chot entry grace cho 5 map: `0 / 100 / 200 / 300 / 400`
- [x] Difficulty ramp nen tang: moi map co `enemyHealthMultiplier` + `enemyDamageMultiplier`
- [x] Shop progression tier co the auto-scale theo `currentMapIndex`
- [x] Co `ShopInventoryEditor` helper de setup merchant nhanh hon trong Inspector
- [x] Co tool generate checklist merchant tu prefab/scene
- [x] `WorldSceneManager` load map moi on dinh hon (resolve scene name + fallback single scene load)
- [x] Loading/teleport handoff sang map moi da duoc giu den khi world/grace san sang
- [x] Khi doi map, AI/NPC/boss cua map cu duoc cleanup truoc khi load map moi
- [x] `TitleScreenManager` da support du 5 class va cap nhat preview theo class dang hover/chon
- [x] Da co review panel/summary cho class selection de hien archetype, stat tom tat, va loadout
- [x] Da chuyen class selection tu runtime-generated button sang serialized UI button trong `Main_Menu_01`
- [x] Da chuyen class review summary tu runtime-generated overlay sang serialized UI panel trong `Main_Menu_01`
- [x] Da scaffold item/armor data cho class outfit moi trong `Assets/Data/Items/Armor`
- [x] Da bo sung tai lieu workflow armor Polygon Hero de tiep tuc setup armor set nhat quan
- [x] Da them he thong quick-slot buff charm cho player hoat dong ca offline/online, co buff HP/Stamina/FP/damage/stamina regen va co luu trang thai buff dang chay
- [x] Da fix issue runtime class leg visibility bang cach map/model activation on dinh hon tren player runtime hierarchy
- [x] Da bo sung HUD buff popup + active buff icon su dung serialized prefab UI, khong con phu thuoc icon tao runtime
- [x] Da tach effectID rieng cho tung buff charm de `Guardian / Wind / Sage / War` hoat dong doc lap

---

## Core Features To Build

### 1. Shop System
- In-game shop:
  - [x] NPC merchant UI co nen tang.
  - [x] Mua item bang rune.
  - [x] Ban item tu inventory.
  - [x] Hien gia, so luong so huu, va mo ta item.
  - [x] Khoa input gameplay khi dang mo shop.
  - [x] Ho tro quay lui menu bang ESC.
  - [x] Save trang thai stock mua/ban cho merchant neu shop dung limited stock.
  - [x] Co support custom stock rieng cho tung merchant.
  - [x] Co nen tang balance gia mua/ban theo progression tier cua shop.
  - [x] Co support auto-scale `shopProgressionTier` theo progression map, van co the them offset cho merchant.
- Out-of-game shop:
  - [ ] Tam hoan.
  - [x] Da bo khoi start game/title menu de giu flow gon hon.
  - [ ] Se xem xet lai sau khi xong progression 5 map va settings.
- Data:
  - [x] Shop item list co ban.
  - [ ] Price table balance day du.
  - [ ] Merchant/shop category.
  - [ ] Rules mo khoa item theo progress.
  - [x] Co tool editor/checklist ho tro setup merchant data.

### 2. Settings Menu
- Audio settings:
  - [x] Master volume.
  - [x] Music volume.
  - [x] SFX volume.
  - [ ] UI volume.
- Graphics settings:
  - [x] Resolution.
  - [x] Fullscreen/windowed.
  - [x] Quality preset.
  - [ ] VSync.
- Control settings:
  - [x] Mouse/controller sensitivity.
  - [ ] Key binding neu can.
- Gameplay settings:
  - [ ] Camera invert.
  - [ ] Lock-on behavior.
  - [ ] UI/HUD toggle options.
- [x] Save/load settings data giua cac lan mo game.
- [x] Settings menu trong title menu.
- [x] Settings menu trong in-game character menu.
- [ ] Settings menu trong pause menu rieng neu can.

### 3. Starting Character Selection
- Tao 5 nhan vat khoi dau.
- Moi nhan vat can:
  - Ten/class identity.
  - Bo stat khoi dau.
  - Vu khi khoi dau.
  - Armor khoi dau.
  - Quick slot item khoi dau.
  - Mo ta uu/nhuoc diem.
- Man hinh chon nhan vat dau game:
  - Preview model.
  - Preview stat.
  - Preview equipment.
  - Xac nhan lua chon.
- Save class da chon vao save data.

### 4. Main Game Loop
- Loop co ban:
  - Spawn vao map.
  - Di chuyen, danh quai, nhat/do loot.
  - Farm rune.
  - Rest/tro ve Site of Grace.
  - Nhan vat duoc nang cap hoac nang cap vu khi.
  - Tham hiem tiep.
  - Gap boss map.
  - Thang boss -> mo khoa map moi.
- Progression:
  - Boss gate cho tung map.
  - Dieu kien unlock map tiep theo.
  - [x] Teleport/chuyen scene sang map moi sau khi thang boss.
  - [x] Luu trang thai da pha dao boss nao, da mo khoa map nao.
- Difficulty ramp:
  - Map 1 -> 5 tang dan:
    - HP quai. *(da co nen tang multiplier theo map)*
    - Damage quai. *(da co nen tang multiplier theo map)*
    - So luong quai.
    - AI complexity.
    - Boss complexity.
- Victory condition:
  - Thang boss map 5 -> hien man chien thang game.
  - Co credits / win screen / option New Game+ neu muon.

---

## Supporting Systems Needed

### Economy And Progression
- Rune balance cho quai thuong, elite, boss.
- Bang gia item va upgrade.
- Level up cost scaling.
- Weapon upgrade cost scaling.
- Reward pacing de khong bi qua de hoac qua kho.

### Map Progression Data
- Dinh nghia 5 map ro rang:
  - Map ID.
  - Scene chinh.
  - Boss cua map.
  - Dieu kien mo khoa.
  - Diem spawn/teleport.
  - Difficulty multiplier. *(da co `enemyHealthMultiplier` + `enemyDamageMultiplier`)*
- He thong world progression manager de theo doi:
  - Map hien tai.
  - Map da mo.
  - Boss da diet.
  - Trang thai chien thang game.

### UI/UX
- Menu shop:
  - [x] Shop UI nam trong `Player UI Manager.prefab`.
  - [x] Co scroll list, item details, buy/sell, close.
  - [x] Khong con mo tu title menu.
  - [ ] Tiep tuc polish layout/presentation cua shop o mot so phan con lai.
  - [x] Co checklist generator de ra merchant nao can setup.
- Menu settings:
  - [x] Title menu settings da hoat dong.
  - [x] Character menu settings da hoat dong.
  - [ ] Co the can them 1 pass polish UI/prefab trong world.
- Menu character select.
- Progress UI:
  - [x] Popup `Map Unlocked`.
  - [x] Popup `Victory Achieved`.
  - [ ] Map current.
  - [ ] Boss defeated.
  - [ ] Maps unlocked.
- Win screen / lose flow / transition screen.
  - [x] Transition loading flow co nen tang on dinh cho map progression.

### Save Data Expansion
- Them vao save:
  - Starting character da chon. *(da co: `startingClassID`)*
  - Maps unlocked. *(da co: `mapsUnlocked`)*
  - Current progression tier/map hien tai. *(da co: `currentMapIndex`)*
  - Shop state neu can. *(da co `merchantStockRemaining`)*
  - Settings data neu save theo profile. *(da co)*

---

## Suggested Build Order

### Phase 1. Foundation (DONE phan lon)
- [x] Core character, AI, combat, inventory systems.
- [x] Shop system.
- [x] Settings menu.
- [x] Save/load data co ban.
- [x] Site of Grace, world locations.
- [x] Tao `GameProgressionManager` theo doi 5 map + starting class.
- [x] Mo rong `CharacterSaveData` them `startingClassID`, `mapsUnlocked[]`, `currentMapIndex`, `gameWon`.

### Phase 2. Starting Character Flow *(DA CO NEN TANG, CAN POLISH)*
- [x] Dinh nghia class data bang `CharacterClass`.
- [x] Preview/apply stat-model-equipment trong character creation flow.
- [x] Noi class selection vao new game flow.
- [x] Mo rong data va flow cho du 5 class co the preview/chon duoc.
- [x] Chuyen 5 nut class sang UI co san trong `Title Screen Canvas`.
- [x] Chuyen class review panel sang UI co san trong `Title Screen Canvas`.
- [ ] Chot UI copy/presentation cho 5 class.
- [ ] Chot visual pass cuoi cho armor preview fidelity sau khi da fix leg/runtime mapping.

### Phase 3. Settings *(DONE co ban)*
- [x] Tao settings menu trong title menu.
- [x] Luu settings persistent.
- [x] Mo rong settings menu vao in-game character menu.
- [ ] Neu can, tach them settings menu cho pause flow rieng.

### Phase 4. In-Game Shop *(DONE co ban, can data)*
- [x] Tao merchant/shop data co ban.
- [x] Tao UI mua/ban.
- [x] Noi inventory + rune vao giao dich.
- [x] Sua loi spam sell khi item da het van nhan rune.
- [ ] Gan merchant NPC/scene vao world that va dat `merchantID`/`shopTierOffset` cho tung shop.
- [ ] Dien du lieu custom stock that cho tung merchant trong inspector.
- [ ] Chot bang gia item economy thuc te sau khi playtest.

### Phase 5. Main Loop Progression *(DA CO FOUNDATION)*
- [x] Tao `GameProgressionManager` singleton.
- [x] Them data 5 map (`MapProgressionDefinition`) + `GameProgressionConfig` asset.
- [x] Boss clear event -> ghi `bossesDefeated` -> unlock next map -> trigger transition/pending entry.
- [x] `WorldSceneManager` load map theo progression state.
- [x] Popup `Map Unlocked` da hien thi duoc trong flow boss clear.
- [x] Difficulty multiplier: scale HP/damage quai theo map index.
- [x] Tao/dung World_02 -> 05 scene scaffold trong build settings de test progression.
- [ ] Pass content/layout/boss/spawner that cho tung scene `World_02` -> `World_05`.
- [x] Win condition khi boss map 5 bi diet.
- [ ] Win screen UI / scene rieng.

### Phase 6. Out-of-Game Shop *(TAM HOAN)*
- Tam hoan.
- Da bo khoi title screen.
- Chi can quay lai phase nay neu sau nay ban muon cosmetic/unlock shop ngoai game.

---

## Concrete Task Backlog

### Immediate (Lam Ngay)
- [x] **[P1]** Tao `GameProgressionManager` (singleton, theo doi mapIndex, mapsUnlocked, gameWon).
- [x] **[P1]** Mo rong `CharacterSaveData`: them `startingClassID`, `mapsUnlocked`, `currentMapIndex`, `gameWon`.
- [x] **[P1]** Tao `GameProgressionConfig` asset va noi vao `GameProgressionManager`.
- [x] **[P1]** Dien data progression co the test duoc cho 5 map trong `GameProgressionConfig`: `sceneBuildIndex`, `bossID`, `entrySiteOfGraceID`.
- [x] **[P2]** Them `enemyHealthMultiplier` + `enemyDamageMultiplier` vao `GameProgressionConfig` va noi AI scale theo `currentMapIndex`.
- [x] **[P2]** Mo rong new game flow de preview/chon du 5 class.
- [x] **[P2]** Chuyen 5 nut class trong character creation sang UI co san cua `Title Screen Canvas`.
- [x] **[P2]** Chuyen class review text/panel sang UI co san, rut gon copy stat/loadout.
- [ ] **[P2]** Chot UI copy + visual polish cho man hinh 5 class.
- [ ] **[P2]** Ra soat lai visual 5 class sau khi fix leg/runtime mapping (preview, vao game, male/female neu can).
- [x] **[P2]** Boss clear event hook vao unlock map tiep theo + scene transition.
- [ ] **[P3]** Dien merchant custom stock cho cac NPC trong World_01 inspector.
- [ ] **[P3]** Gan `merchantID`, bat/tat auto tier, va dat `shopTierOffset` hop ly cho tung merchant.
- [ ] **[P3]** Chay merchant checklist generator, ra soat merchantID trung/lac, va chot stock cho tung merchant.
- [ ] **[P3]** Balance rune drop + shop price sau playtest co ban.

### Completed Recently
- [x] Them menu settings trong title menu.
- [x] Them menu settings trong in-game character menu.
- [x] Them shop trong game.
- [x] Khoa movement/attack/interaction khi mo shop.
- [x] Ho tro ESC de back tung lop menu.
- [x] Sua loi spam sell duplicate rune.
- [x] Fix rune reward on boss (Durk) death.
- [x] Them progression save data + `GameProgressionManager`.
- [x] Them `GameProgressionConfig` asset de setup map progression.
- [x] Them popup `Map Unlocked`.
- [x] Sua popup unlock hien thi on dinh sau boss death.
- [x] Sua `GameProgressionManager` de editor sync theo `GameProgressionConfig`.
- [x] Sua scene transition/load handoff de boss clear co the sang `World_02` -> `World_05`.
- [x] Chot `entrySiteOfGraceID` cho 5 map trong `Game Progression Config.asset`.
- [x] Cleanup AI/NPC/boss state cua map cu truoc khi load map moi.
- [x] Mo rong man hinh character creation len du 5 class, co review panel va nut class day du.
- [x] Chuyen class selection sang UI button co san trong scene.
- [x] Chuyen class review sang UI panel co san trong scene.
- [x] Sua lai mapping nut `Mystic` / `Confessor` de khop dung class ID.
- [x] Tao/scaffold them class armor data dua tren `PolygonFantasyHeroCharacters`.
- [x] Them tai lieu `POLYGON_HERO_ARMOR_WORKFLOW.md` de ghi ro pipeline lay armor set tu Polygon Hero.
- [x] Fix class armor leg/runtime mapping de preview + vao game khong con mat chan o cac class ngoai `Knight`.
- [x] Hoan thien buff charm IDs rieng cho tung buff de 4 charm hoat dong doc lap.
- [x] Them buff popup + active buff icons vao HUD bang prefab UI.

### Next (Lam Sau Immediate)
- [ ] Them data balance day du cho rune, level up, shop price sau khi playtest progression.
- [ ] Them UI progression day du ngoai popup (`map current`, `maps unlocked`, `boss defeated`).
- [x] Tang do kho co ban cua quai/boss theo tung map bang multiplier HP/damage.
- [x] Dat `entrySiteOfGraceID` cho 5 map de progression co the teleport dung entry.
- [ ] Gan merchant NPC/scene vao ShopInteractable va custom stock cho tung shop.
- [ ] Refactor them phan layout shop con tao runtime sang prefab neu can.

### Later (Lam Sau Cung)
- [ ] Can nhac co lam lai shop ngoai main menu hay khong.
- [ ] Them reward/doc quyen theo tung map.
- [ ] Them endgame/New Game+ neu can.
- [ ] World_02 -> 05 map design/content pass that (layout, enemy placement, boss arena, grace placement).

---

## Design Decisions To Confirm Later

- Shop ngoai game ban gi:
  - Cosmetic only
  - Unlock item
  - Starter bonus
- Chuyen sang map moi theo cach nao:
  - Tu dong teleport sau boss
  - Mo cong/portal cho nguoi choi tu chon di tiep
- 5 nhan vat la:
  - 5 class co stat/vu khi khac nhau
  - Hay 5 hero co model/ky nang rieng biet hon
- Win game xong thi:
  - Ve title
  - Mo New Game+
  - Mo endless/challenge mode

---

## Current Priority Recommendation

Thu tu toi uu de tranh phai lam lai:
1. **[P1] Chot lai character creation visual pass** (ra soat head/armor fidelity, xac nhan preview/in-game consistency, xong visual 5 class)
2. **[P1] Pass content that cho `World_02` -> `World_05`** (layout, spawner, boss arena, grace placement)
3. **[P2] Hoan thien merchant stock va shop balance**
4. **[P3] Progression UI day du** (`map current`, `maps unlocked`, `boss defeated`)
5. **[P4] Buff charm / combat feedback polish** (icon art set, timer/readability, balancing)

---

## Current State Snapshot

| He Thong | Trang Thai |
|---|---|
| Core player / character | Hoan chinh |
| AI enemy (undead, patrol, attack) | Hoan chinh |
| Boss (Durk) + rune reward | Hoan chinh |
| Shop (buy/sell/save stock) | Hoan chinh |
| Settings (title + in-game) | Hoan chinh |
| Level up UI | Hoan chinh |
| Weapon upgrade UI | Hoan chinh |
| Site of Grace | Hoan chinh |
| Save/load (equipment/rune/boss/stock) | Hoan chinh |
| Starting character selection | **Da co du 5 class + preview + UI scene that, can chot visual polish** |
| GameProgressionManager (5 map) | **Da co** |
| GameProgressionConfig asset | **Da co** |
| Boss gate -> unlock -> teleport | **Da hoat dong qua scene progression** |
| Map 2-5 scenes | **Da co scaffold, can content pass** |
| Difficulty ramp | **Da co foundation data-driven** |
| Popup Map Unlocked / Victory | **Da co** |
| Win screen | **Chua co** |
| Economy balance | **Chua co** |
| Merchant NPC trong world | **Co tool setup, can data pass** |
| Class outfit / armor preview | **Da co data + flow day du, runtime leg mapping da on dinh; con 1 vong visual polish cuoi** |
| Buff charm system | **Da hoat dong + co HUD popup/icon, can polish art/balance** |



