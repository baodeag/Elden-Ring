# Elden Ring Project Roadmap

Tai lieu nay luu cac hang muc can lam de hoan thien he thong game.
Cap nhat file nay moi khi chot them tinh nang, doi uu tien, hoac hoan thanh dau viec.

> **Cap nhat lan cuoi: 2026-04-20 (cap nhat weapon/armor content pass: import Polygon Fantasy Hero + Polygon Dungeon weapons, tao item data, collider/damage setup, shield setup, va dong bo held pivot theo `Weapon_Axe_01`)**

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

## Trang Thai Hien Tai (2026-04-20)

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
- [x] Da import va tao prefab/item data cho weapon tu `PolygonFantasyHeroCharacters` va `PolygonDungeon`
- [x] Weapon prefab moi da co `WeaponManager`, damage collider, model/pivot hierarchy, item asset, va duoc add vao `World Item Database`
- [x] Shield prefab moi da setup theo mau `Weapon_Medium_Shield_01`
- [x] Non-shield melee weapon da dong bo held `Weapon Pivot` theo transform `Weapon_Axe_01` da duoc canh tay hop ly
- [x] Da tach rieng nhom khong dong bo transform melee: `Shield`, `Bow`, `Unarmed`
- [x] Armor tu `PolygonFantasyHeroCharacters` da duoc doc/lay vao pipeline content pass

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
- [ ] Playtest visual tung nhom weapon moi tren player animation thuc te (straight sword, axe, mace, staff/spear, dagger/knife, greatsword)
- [ ] Balance chi so sat thuong/weight/price cho weapon moi sau khi da co prefab/item data
- [ ] Content pass that cho `World_02` -> `World_05`: entry grace trong scene, boss/spawner/layout rieng, navmesh/lighting neu can
- [ ] Playtest end-to-end progression 5 map tren Unity sau khi scene data duoc setup that

### Da Xac Minh Khi Doc Lai Project (2026-04-11)
- [x] `ProjectSettings/EditorBuildSettings.asset` da co `Main_Menu_01` va `World_01` -> `World_05` theo dung build index `0 -> 5`
- [x] `Assets/Data/Game Progression Config.asset` da map dung:
  - `Limgrave Gate` -> scene `1`, boss `0`, entry grace `0`
  - `Storm Path` -> scene `2`, boss `1`, entry grace `100`
  - `Ashen Keep` -> scene `3`, boss `2`, entry grace `200`
  - `Black Vault` -> scene `4`, boss `3`, entry grace `300`
  - `Erdtree Ascent` -> scene `5`, boss `4`, entry grace `400`
- [x] `AIBossCharacterManager` va `EventTriggerBossFight` co auto-assign `bossID = sceneBuildIndex - 1` cho world scene `1 -> 5`
- [x] `WorldSceneManager` resolve scene name tu build index va fallback sang `SceneManager.LoadScene` neu Netcode scene load khong start
- [x] `ShopInventory` co `autoScaleShopTierFromProgression`, `shopTierOffset`, `requiredProgressionTier`, limited stock save key theo merchant
- [x] `Assets/Docs/MERCHANT_SETUP_CHECKLIST.md` da detect merchant prefab `Merchant_AI_Dummy_01` voi `merchantID = merchant_ai_dummy_01`
- [ ] `Assets/Docs/MAP_CONTENT_PASS_CHECKLIST.md` can cap nhat note cu ve `entrySiteOfGraceID`; config asset hien da la `0 / 100 / 200 / 300 / 400`, khong con la `0` cho ca 5 map

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
- [x] Da tao hang loat weapon prefab moi trong `Assets/Prefabs/Items/Weapons` tu Polygon Fantasy Hero va Polygon Dungeon
- [x] Da tao item asset tuong ung trong `Assets/Data/Items/Weapons/Melee Weapons/Polygon Fantasy Hero Generated` va `Polygon Dungeon Generated`
- [x] Da add generated weapons vao `Assets/Prefabs/World Managers/World Item Database.prefab`
- [x] Da setup box damage collider khop mesh theo do dai/rong chinh cua weapon, tach can/phan sat thuong theo kha nang mesh bounds
- [x] Da setup shield generated theo component/model transform cua `Weapon_Medium_Shield_01`
- [x] Da dong bo held pivot cho 80 melee weapon non-shield theo transform `Weapon_Axe_01` da duoc canh bang tay
- [x] `dotnet build '.\Elden Ring.sln'` pass sau weapon pivot sync, chi con warning cu khong lien quan

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

### Weapon / Armor Content
- Weapon content:
  - [x] Import weapon tu Polygon Fantasy Hero.
  - [x] Import weapon tu Polygon Dungeon.
  - [x] Tao prefab trong `Assets/Prefabs/Items/Weapons`.
  - [x] Tao item data trong `Assets/Data/Items/Weapons/Melee Weapons`.
  - [x] Add weapon moi vao `World Item Database`.
  - [x] Add `WeaponManager` + damage collider.
  - [x] Setup shield theo mau `Weapon_Medium_Shield_01`.
  - [x] Dong bo held pivot cho melee weapon theo `Weapon_Axe_01`.
  - [ ] Playtest tung weapon family tren animation idle/attack/roll/backstab/riposte.
  - [ ] Balance damage/stamina/price/upgrade scaling cho weapon moi.
- Armor content:
  - [x] Doc va lay armor tu `PolygonFantasyHeroCharacters`.
  - [ ] Chot armor set dung cho 5 starting class va merchant/drop pool.
  - [ ] Ra soat visual male/female/preview/in-game sau khi gan vao player.

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

### Phase 2B. Weapon / Armor Content Pass *(DA CO NEN TANG, CAN PLAYTEST/BALANCE)*
- [x] Import va tao prefab/item data cho Polygon Fantasy Hero weapons.
- [x] Import va tao prefab/item data cho Polygon Dungeon weapons.
- [x] Setup damage collider cho weapon moi.
- [x] Setup shield theo mau `Weapon_Medium_Shield_01`.
- [x] Dong bo held transform melee weapon theo `Weapon_Axe_01`.
- [ ] Playtest weapon moi tren player: idle, walk/run, attack, two-hand, back slot, riposte/backstab.
- [ ] Chia weapon moi vao shop/drop/loadout theo progression tier.
- [ ] Balance stat item: base damage, stamina cost, weight, price, upgrade curve.
- [ ] Hoan thien armor set/loadout cho class va merchant/drop pool.

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
- [ ] **[P1]** Playtest nhanh weapon moi sau pivot sync: equip `Weapon_Axe_01`, sword, staff/spear, mace/hammer, dagger/knife, greatsword de xac nhan khong con lech sau lung/player hand.
- [ ] **[P1]** Xac minh two-hand/back-slot behavior cho weapon moi, vi pivot cam tay da on nhung slot sau lung co transform rieng theo `WeaponClass`.
- [ ] **[P1]** Mo tung scene `World_02` -> `World_05` trong Unity va xac minh/gan dung entry `siteOfGraceID` theo config `100 / 200 / 300 / 400`.
- [ ] **[P1]** Xac minh boss, fog wall, wake trigger trong tung world scene dang dung `bossID = sceneBuildIndex - 1`.
- [ ] **[P1]** Lam content pass playable cho `World_02` -> `World_05`: layout toi thieu khac nhau, spawner khac nhau, boss arena khac nhau, entry spawn an toan.
- [ ] **[P1]** Playtest flow lien tuc: New Game -> boss map 1 -> `World_02` -> ... -> boss map 5 -> `Victory Achieved` -> ve title.
- [ ] **[P1]** Playtest save/load tai tung map sau khi vua transition, dam bao `currentMapIndex` va `lastSiteOfGraceRestedAt` khong sai.
- [ ] **[P2]** Cap nhat `Assets/Docs/MAP_CONTENT_PASS_CHECKLIST.md` de bo note cu ve entry grace `0` cho ca 5 map.
- [ ] **[P2]** Dien merchant custom stock cho cac NPC trong `World_01`/prefab va quyet dinh co tiep tuc `useGlobalPurchasableItems` hay chuyen sang curated stock.
- [ ] **[P2]** Gan/ra soat `merchantID`, `autoScaleShopTierFromProgression`, `shopTierOffset`, `requiredProgressionTier` cho merchant stock that.
- [ ] **[P2]** Chot bang gia economy pass 1: rune drop, level up cost, shop price, weapon upgrade cost.
- [ ] **[P2]** Gan weapon/armor moi vao shop/drop/loadout theo map tier.
- [ ] **[P2]** Balance weapon moi: damage, stamina modifier, price, upgrade pacing.
- [ ] **[P3]** Chot UI copy + visual polish cho man hinh 5 class.
- [ ] **[P3]** Ra soat lai visual 5 class sau khi fix leg/runtime mapping (preview, vao game, male/female neu can).
- [ ] **[P3]** Them progression UI day du: current map, unlocked maps, boss defeated state.
- [ ] **[P3]** Thiet ke win screen/victory scene rieng thay vi chi popup + return title.

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
- [x] Ra soat lai roadmap/project ngay 2026-04-11 va xac nhan config progression/build settings dang khop voi muc tieu 5 map.
- [x] Xac nhan merchant prefab hien co da co `merchantID`, auto tier scaling, va checklist setup rieng.
- [x] Import/setup weapon moi tu Polygon Fantasy Hero + Polygon Dungeon vao prefab/item database.
- [x] Setup damage collider va `WeaponManager` cho weapon generated.
- [x] Setup shield generated theo mau `Weapon_Medium_Shield_01`.
- [x] Dong bo pivot cam tay cho melee weapon non-shield theo `Weapon_Axe_01` da canh bang tay.
- [x] Build pass sau content/pivot sync weapon.

### Next (Lam Sau Immediate)
- [ ] Playtest weapon generated theo family va tinh chinh rieng cac model neu co mesh/origin qua khac.
- [ ] Gan weapon/armor generated vao merchant/drop/loadout progression.
- [ ] Sua/cap nhat `MAP_CONTENT_PASS_CHECKLIST.md` cho khop config entry grace hien tai.
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

Thu tu toi uu sau khi doc lai project ngay 2026-04-11:
1. **[P1] Content/data pass cho `World_02` -> `World_05`** (entry grace trong scene, boss/fog/wake trigger, layout, spawner, boss arena)
2. **[P1] Playtest progression end-to-end** (boss map 1 -> map 5, save/load tung map, victory popup + return title)
3. **[P2] Hoan thien merchant stock va shop balance** (curated stock, required tier, price/rune pacing)
4. **[P2] Progression UI day du** (`map current`, `maps unlocked`, `boss defeated`, victory screen rieng)
5. **[P3] Character selection visual pass** (class outfit fidelity, preview/in-game consistency, UI wording)
6. **[P3] Weapon/armor content pass** (playtest generated weapons, balance stats, assign shop/drop/loadout tiers)
7. **[P4] Buff charm / combat feedback polish** (icon art set, timer/readability, balancing)

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
| GameProgressionConfig asset | **Da co va da xac minh `sceneBuildIndex`/`bossID`/`entrySiteOfGraceID` khop 5 map** |
| Boss gate -> unlock -> teleport | **Da hoat dong qua scene progression** |
| Build settings 5 world scenes | **Da co `World_01` -> `World_05` o build index `1 -> 5`** |
| Map 2-5 scenes | **Da co scaffold, can scene data/content pass va playtest trong Unity** |
| Difficulty ramp | **Da co foundation data-driven** |
| Popup Map Unlocked / Victory | **Da co** |
| Win screen | **Chua co** |
| Economy balance | **Chua co** |
| Merchant NPC trong world | **Co prefab/checklist va auto tier scaling, can custom stock/data pass** |
| Class outfit / armor preview | **Da co data + flow day du, runtime leg mapping da on dinh; con 1 vong visual polish cuoi** |
| Buff charm system | **Da hoat dong + co HUD popup/icon, can polish art/balance** |
| Generated weapon content | **Da import/setup prefab + item data + database; melee held pivot da dong bo theo `Weapon_Axe_01`; can playtest family/balance** |
| Generated shield content | **Da setup theo mau `Weapon_Medium_Shield_01`; can playtest left-hand/block visual** |
| Armor content | **Da doc/lay tu Polygon Fantasy Hero pipeline; can chot set/drop/shop/class loadout** |



