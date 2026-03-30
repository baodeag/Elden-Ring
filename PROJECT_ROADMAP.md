# Elden Ring Project Roadmap

Tai lieu nay luu cac hang muc can lam de hoan thien he thong game.
Cap nhat file nay moi khi chot them tinh nang, doi uu tien, hoac hoan thanh dau viec.

> **Cap nhat lan cuoi: 2026-03-30 (sau progression foundation pass)**

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

## Trang Thai Hien Tai (2026-03-30)

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
- [x] World scene: 1 map dang hoat dong (World_01 / Area_01)
- [x] Main Menu scene: Title screen + load menu + settings

### Chua Co / Chua Hoan Thanh
- [ ] Starting character selection screen polish/UI data day du (nen tang da co)
- [ ] Multiple world scenes (chi co World_01, can World_02 -> 05)
- [ ] Boss clear -> teleport sang map tiep theo bang scene/data that cho tung map
- [ ] Difficulty ramp theo tung map
- [ ] Win screen / victory scene rieng
- [ ] Custom merchant stock da dien du lieu trong inspector
- [ ] Economy balance (rune drop, level up cost, shop price)
- [ ] UI progression day du (map current, map unlocked, victory screen)

### Moi Hoan Thanh Trong Code
- [x] `GameProgressionManager` singleton theo doi `startingClassID`, `currentMapIndex`, `mapsUnlocked`, `gameWon`
- [x] Mo rong `CharacterSaveData` voi progression data
- [x] `TitleScreenManager` luu starting class da chon khi New Game
- [x] Boss clear -> unlock next map -> save progression -> thong bao `Map Unlocked`
- [x] Co support same-scene progression bang `entrySiteOfGraceID`
- [x] Co support scene transition neu map sau dung scene build index khac
- [x] Co popup `Victory Achieved` + flow quay ve title sau khi thang boss map cuoi
- [x] Co runtime warning neu map progression data chua duoc setup trong Inspector

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
- Out-of-game shop:
  - [ ] Tam hoan.
  - [x] Da bo khoi start game/title menu de giu flow gon hon.
  - [ ] Se xem xet lai sau khi xong progression 5 map va settings.
- Data:
  - [x] Shop item list co ban.
  - [ ] Price table balance day du.
  - [ ] Merchant/shop category.
  - [ ] Rules mo khoa item theo progress.

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
  - Teleport/chuyen scene sang map moi sau khi thang boss.
  - Luu trang thai da pha dao boss nao, da mo khoa map nao.
- Difficulty ramp:
  - Map 1 -> 5 tang dan:
    - HP quai.
    - Damage quai.
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
  - Difficulty multiplier.
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
  - [ ] Tiep tuc polish layout bang prefab thay vi runtime code o mot so phan con lai.
- Menu settings:
  - [x] Title menu settings da hoat dong.
  - [x] Character menu settings da hoat dong.
  - [ ] Co the can them 1 pass polish UI/prefab trong world.
- Menu character select.
- Progress UI:
  - Map current.
  - Boss defeated.
  - Maps unlocked.
- Win screen / lose flow / transition screen.

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
- [ ] Polish UI data hien thi cho du 5 class.

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
- [ ] Gan merchant NPC/scene vao world that va dat merchantID/progressionTier cho tung shop.
- [ ] Dien du lieu custom stock that cho tung merchant trong inspector.
- [ ] Chot bang gia item economy thuc te sau khi playtest.

### Phase 5. Main Loop Progression *(DA CO FOUNDATION)*
- [x] Tao `GameProgressionManager` singleton.
- [x] Them data 5 map (`MapProgressionDefinition` trong manager).
- [x] Boss clear event -> ghi `bossesDefeated` -> unlock next map -> trigger transition/pending entry.
- [x] `WorldSceneManager` load map theo progression state.
- [ ] Difficulty multiplier: scale HP/damage quai theo map index.
- [ ] Tao/dung World_02 -> 05 scenes (co the dung bo map don gian truoc, polish sau).
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
- [ ] **[P1]** Dien data that cho 5 map trong Inspector: `mapName`, `sceneBuildIndex`, `bossID`, `entrySiteOfGraceID`.
- [ ] **[P2]** Polish/hoan thien UI hien thi 5 class trong new game flow.
- [x] **[P2]** Boss clear event hook vao unlock map tiep theo + scene transition.
- [ ] **[P3]** Dien merchant custom stock cho cac NPC trong World_01 inspector.
- [ ] **[P3]** Balance rune drop + shop price sau playtest co ban.

### Completed Recently
- [x] Them menu settings trong title menu.
- [x] Them menu settings trong in-game character menu.
- [x] Them shop trong game.
- [x] Khoa movement/attack/interaction khi mo shop.
- [x] Ho tro ESC de back tung lop menu.
- [x] Sua loi spam sell duplicate rune.
- [x] Fix rune reward on boss (Durk) death.

### Next (Lam Sau Immediate)
- [ ] Mo rong save data cho map progression va starting character.
- [ ] Them data balance day du cho rune, level up, shop price sau khi playtest progression.
- [ ] Them UI progression va thong bao unlock map.
- [ ] Tang do kho quai/boss theo tung map.
- [ ] Gan merchant NPC/scene vao ShopInteractable va custom stock cho tung shop.
- [ ] Refactor them phan layout shop con tao runtime sang prefab neu can.

### Later (Lam Sau Cung)
- [ ] Can nhac co lam lai shop ngoai main menu hay khong.
- [ ] Them reward/doc quyen theo tung map.
- [ ] Them endgame/New Game+ neu can.
- [ ] World_02 -> 05 map design va scene build.

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
1. **[P1] GameProgressionManager + mo rong SaveData** (nen tang cho moi thu sau)
2. **[P1] 5 Starting class data + man hinh chon nhan vat**
3. **[P2] Boss clear -> unlock map -> scene transition**
4. **[P3] Hoan thien merchant stock va shop balance**
5. **[P4] Difficulty ramp + tao scene World_02 -> 05**
6. **[P5] Win screen + polish UI/settings neu can**

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
| Starting character selection | **Chua co** |
| GameProgressionManager (5 map) | **Chua co** |
| Boss gate -> unlock -> teleport | **Chua co** |
| Map 2-5 scenes | **Chua co** |
| Difficulty ramp | **Chua co** |
| Win screen | **Chua co** |
| Economy balance | **Chua co** |
| Merchant NPC trong world | **Can data** |
