# Elden Ring Project Roadmap

Tai lieu nay luu cac hang muc can lam de hoan thien he thong game.
Cap nhat file nay moi khi chot them tinh nang, doi uu tien, hoac hoan thanh dau viec.

## North Star

Muc tieu gameplay chinh:
- Nguoi choi chon 1 trong 5 nhan vat khoi dau.
- Vao game, danh quai de farm rune.
- Dung rune de nang cap nhan vat, vu khi, trang bi.
- Vuot qua boss cua tung map de mo khoa map tiep theo.
- Moi map sau kho hon map truoc, quai manh hon, thu thach hon.
- Vuot qua du 5 map thi chien thang game.

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
  - UI volume.
- Graphics settings:
  - [x] Resolution.
  - [x] Fullscreen/windowed.
  - [x] Quality preset.
  - VSync.
- Control settings:
  - [x] Mouse/controller sensitivity.
  - Key binding neu can.
- Gameplay settings:
  - Camera invert.
  - Lock-on behavior.
  - UI/HUD toggle options.
- [x] Save/load settings data giua cac lan mo game.
- [x] Settings menu trong title menu.
- [ ] Settings menu trong in-game menu/pause.

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
- Menu settings.
- Menu character select.
- Progress UI:
  - Map current.
  - Boss defeated.
  - Maps unlocked.
- Win screen / lose flow / transition screen.

### Save Data Expansion
- Them vao save:
  - Starting character da chon.
  - Maps unlocked.
  - Bosses defeated by map.
  - Current progression tier.
  - Shop state neu can.
  - Settings data neu save theo profile.

## Suggested Build Order

### Phase 1. Foundation
- Tao `GameProgressionManager`.
- Mo rong `CharacterSaveData` cho progression 5 map.
- Dinh nghia data cho 5 map.
- Dinh nghia data cho 5 starting characters.

### Phase 2. Starting Character Flow
- Hoan thien UI chon 5 nhan vat.
- Noi class selection vao new game flow.
- Save/load dung starting character data.

### Phase 3. Settings
- [x] Tao settings menu trong title menu.
- [x] Luu settings persistent.
- [ ] Mo rong settings menu vao in-game pause/menu.

### Phase 4. In-Game Shop
- [x] Tao merchant/shop data co ban.
- [x] Tao UI mua/ban.
- [x] Noi inventory + rune vao giao dich.
- [x] Sua loi spam sell khi item da het van nhan rune.
- [ ] Gan merchant NPC/scene vao world that va dat merchantID/progressionTier cho tung shop.
- [ ] Dien du lieu custom stock that cho tung merchant trong inspector.
- [ ] Chot bang gia item economy thuc te sau khi playtest.

### Phase 5. Main Loop Progression
- Boss clear -> unlock map sau.
- Teleport sang map moi.
- Difficulty ramp theo map.
- Win condition map 5.

### Phase 6. Out-of-Game Shop
- Tam hoan.
- Da bo khoi title screen.
- Chi can quay lai phase nay neu sau nay ban muon cosmetic/unlock shop ngoai game.

## Concrete Task Backlog

### Immediate
- [ ] Tao he thong progression 5 map.
- [ ] Thiet ke 5 starting characters.
- [ ] Them man hinh chon nhan vat dau game.
- [x] Them menu settings trong title menu.
- [ ] Them menu settings trong in-game.
- [x] Them shop trong game.
- [x] Khoa movement/attack/interaction khi mo shop.
- [x] Ho tro ESC de back tung lop menu.
- [x] Sua loi spam sell duplicate rune.
- [ ] Dinh nghia dieu kien thang boss -> sang map moi.
- [ ] Tao man chien thang khi hoan thanh map 5.

### Next
- [ ] Mo rong save data cho map progression va starting character.
- [ ] Them data balance day du cho rune, level up, shop price sau khi playtest progression.
- [ ] Them UI progression va thong bao unlock map.
- [ ] Tang do kho quai/boss theo tung map.
- [ ] Gan merchant NPC/scene vao ShopInteractable va custom stock cho tung shop.
- [ ] Refactor them phan layout shop con tao runtime sang prefab neu can.

### Later
- [ ] Can nhac co lam lai shop ngoai main menu hay khong.
- [ ] Them reward/doc quyen theo tung map.
- [ ] Them endgame/New Game+ neu can.

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

## Current Priority Recommendation

Thu tu toi uu de tranh phai lam lai:
1. Starting character system
2. Save data mo rong
3. In-game settings menu
4. Progression 5 map
5. Hoan thien merchant stock va shop balance
6. Can nhac lai out-of-game shop sau

## Current State Snapshot

- In-game shop: da hoat dong.
- Character menu shop: da giu lai.
- Start game/title menu shop: da bo.
- Shop UI: da dua vao `Player UI Manager.prefab`, nhung van con mot vai phan layout tao bang code can polish sau.
- Shop transaction:
  - Buy hoat dong.
  - Sell hoat dong.
  - Da sua bug spam ban item de nhan rune lap lai.
- Menu navigation:
  - Dang mo menu con co the bam ESC de quay lui tung lop ve character menu va gameplay.

