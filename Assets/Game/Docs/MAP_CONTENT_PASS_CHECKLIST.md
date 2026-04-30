# Map Content Pass Checklist

Tai lieu nay dung de hoan thien phan map dang do cho `World_02` -> `World_05`.
Muc tieu la bien cac scaffold scene clone tu `World_01` thanh cac map test that, co progression dung, spawn dung, boss dung, va de playtest end-to-end on dinh.

---

## Muc Tieu

- Moi map co entry `Site Of Grace` rieng.
- Moi map co `siteOfGraceID` rieng, khong dung chung `0`.
- Moi map co boss dung voi `bossID` trong `Game Progression Config`.
- Moi map co layout/spawner/boss arena du khac de test progression that.
- Giet boss map truoc thi vao dung map sau, dung diem spawn sau.

---

## Config Hien Tai Can Nho

Build index hien tai:
- Map 1 -> `World_01` -> build index `1`
- Map 2 -> `World_02` -> build index `2`
- Map 3 -> `World_03` -> build index `3`
- Map 4 -> `World_04` -> build index `4`
- Map 5 -> `World_05` -> build index `5`

Map config hien tai:
- Map 1 `Limgrave Gate` -> `bossID = 0`
- Map 2 `Storm Path` -> `bossID = 1`
- Map 3 `Ashen Keep` -> `bossID = 2`
- Map 4 `Black Vault` -> `bossID = 3`
- Map 5 `Erdtree Ascent` -> `bossID = 4`

Luu y:
- Hien tai `entrySiteOfGraceID` trong config dang de `0` cho ca 5 map.
- Neu khong doi ID that trong scene va config, progression van co the chay nhung spawn handoff se rat de sai.

---

## De Xuat ID De Khoi Bi Trung

Neu chua co convention khac, co the dung tam:
- `World_01` -> entry grace `0`
- `World_02` -> entry grace `100`
- `World_03` -> entry grace `200`
- `World_04` -> entry grace `300`
- `World_05` -> entry grace `400`

Khong bat buoc phai dung bo so nay, nhung nen giu quy uoc cach nhau ro rang de de debug.

---

## Checklist Chung Cho Moi Map

### 1. Entry Spawn
- [ ] Mo scene world trong Unity.
- [ ] Tim object `Interactable Site Of Grace`.
- [ ] Doi ten object cho ro map, vi du `Interactable Site Of Grace (World_02 Entry)`.
- [ ] Dat `siteOfGraceID` moi, khong trung map khac.
- [ ] Dat lai vi tri/huong nhin spawn neu can.
- [ ] Test teleport den grace do co dat player vao vi tri an toan khong.

### 2. Boss
- [ ] Xac nhan boss trong scene co `bossID` dung voi map.
- [ ] Xac nhan `FogWall` neu co cung dung `fogWallID` theo boss.
- [ ] Xac nhan event trigger danh thuc boss dung `bossID`.
- [ ] Xac nhan boss arena khong con la clone nguyen xi neu da bat dau content pass.

### 3. Combat Space
- [ ] Doi hoac chinh layout co khac biet toi thieu de test.
- [ ] Xem lai duong di tu spawn den boss.
- [ ] Dam bao khong co dia hinh gay ket player ngay khi vao map.
- [ ] Xem lai navmesh neu co thay doi geometry lon.

### 4. AI / Spawner
- [ ] Chinh so luong quai cho hop progression map.
- [ ] Doi vi tri spawner de tranh trung lap hoan toan voi `World_01`.
- [ ] Dam bao khong spawn quai de len entry grace.
- [ ] Test 1 vong combat tu spawn toi boss.

### 5. Visual / Technical
- [ ] Rebuild lighting neu map da doi layout dang ke.
- [ ] Rebuild navmesh neu map da doi geometry.
- [ ] Recheck occlusion/probe neu can.
- [ ] Chay scene xem co missing reference/pink material/error nao khong.

---

## Checklist Theo Tung Map

### World_02 - Storm Path
- [ ] Doi entry grace ID sang ID rieng cho `World_02`.
- [ ] Cap nhat `entrySiteOfGraceID` cua Map 2 trong `Game Progression Config`.
- [ ] Xac nhan boss trong scene dung `bossID = 1`.
- [ ] Chinh toi thieu 1 trong cac thu sau de map khong con la clone y het:
  - duong vao boss
  - cum quai dau map
  - khu vuc entry
  - boss arena
- [ ] Test flow: giet boss map 1 -> sang `World_02` -> spawn dung entry grace moi.

### World_03 - Ashen Keep
- [ ] Doi entry grace ID sang ID rieng cho `World_03`.
- [ ] Cap nhat `entrySiteOfGraceID` cua Map 3 trong `Game Progression Config`.
- [ ] Xac nhan boss trong scene dung `bossID = 2`.
- [ ] Tang them ap luc so voi `World_02`:
  - nhieu quai hon
  - duong di hep hon
  - arena boss nguy hiem hon
- [ ] Test flow: giet boss map 2 -> sang `World_03` -> spawn dung grace.

### World_04 - Black Vault
- [ ] Doi entry grace ID sang ID rieng cho `World_04`.
- [ ] Cap nhat `entrySiteOfGraceID` cua Map 4 trong `Game Progression Config`.
- [ ] Xac nhan boss trong scene dung `bossID = 3`.
- [ ] Tang nhan dien map:
  - silhouette/layout khac ro
  - trap/choke point neu muon
  - boss arena de nhan biet hon
- [ ] Test flow: giet boss map 3 -> sang `World_04` -> spawn dung grace.

### World_05 - Erdtree Ascent
- [ ] Doi entry grace ID sang ID rieng cho `World_05`.
- [ ] Cap nhat `entrySiteOfGraceID` cua Map 5 trong `Game Progression Config`.
- [ ] Xac nhan boss trong scene dung `bossID = 4`.
- [ ] Lam map co cam giac final area toi thieu bang layout/props/lighting.
- [ ] Test flow: giet boss map 4 -> sang `World_05` -> spawn dung grace.
- [ ] Test them flow ket thuc: giet boss map 5 -> hien `Victory Achieved` -> quay ve title.

---

## Checklist Cap Nhat Config

Sau khi chot ID that trong Unity:
- [ ] Mo `Assets/Data/Game Progression Config.asset`
- [ ] Map 1 giu `entrySiteOfGraceID` dung voi `World_01`
- [ ] Map 2 doi thanh ID entry cua `World_02`
- [ ] Map 3 doi thanh ID entry cua `World_03`
- [ ] Map 4 doi thanh ID entry cua `World_04`
- [ ] Map 5 doi thanh ID entry cua `World_05`
- [ ] Recheck `sceneBuildIndex` van la `1 -> 5`
- [ ] Recheck `bossID` van la `0 -> 4`

---

## Checklist Test End-To-End

### Test A - Progression lien tuc
- [ ] New Game.
- [ ] Vao `World_01`.
- [ ] Giet boss map 1.
- [ ] Xac nhan popup unlock.
- [ ] Vao `World_02` dung grace moi.
- [ ] Lap lai den `World_05`.

### Test B - Save Load
- [ ] O moi map, save sau khi vua spawn vao map moi.
- [ ] Thoat game va load lai.
- [ ] Xac nhan `currentMapIndex` dung.
- [ ] Xac nhan player spawn tai grace cua map hien tai.

### Test C - Boss State
- [ ] Boss da giet khong hoi sinh sai.
- [ ] Fog wall khong bi bat sai sau khi boss da chet.
- [ ] Boss chua giet thi van co the kich hoat binh thuong.

### Test D - Grace / Teleport
- [ ] Teleport menu khong bi nham grace giua cac map do trung ID.
- [ ] Rest tai grace moi cap nhat dung `lastSiteOfGraceRestedAt`.

---

## Definition Of Done

Co the xem pass map tam on khi:
- `World_02` -> `World_05` khong con dung chung entry grace ID voi `World_01`.
- Moi map co boss dung `bossID` trong config.
- Giet boss map n thi vao dung map n+1.
- Save/load khong lam mat progression hoac spawn sai map.
- Map clone da co it nhat 1 pass layout/spawner khac nhau de playtest.

---

## Ghi Chu Lam Viec

- Uu tien lam dung progression va spawn truoc.
- Lighting/navmesh polish lam sau cung neu can.
- Neu thieu asset/map design, chi can lam "playable distinct test map" truoc, khong can polished.
