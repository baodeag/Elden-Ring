# Polygon Hero Armor Workflow

Tai lieu nay mo ta chinh xac cach project hien tai lay giap tu addon `PolygonFantasyHeroCharacters` va bien no thanh armor co the equip trong game.

---

## Tom Tat Nhanh

Project nay KHONG equip truc tiep prefab giap cua addon.

Pipeline dang dung la:

1. Lay ten cac mesh modular tu `PolygonFantasyHeroCharacters`.
2. Tao cac asset `EquipmentModel` de map `EquipmentModelType` + ten mesh male/female.
3. Gom nhieu `EquipmentModel` vao mot `ArmorItem`.
4. Dang ky `ArmorItem` vao `World Item Database`.
5. Khi equip, `PlayerEquipmentManager` bat dung child object tren `Player.prefab` theo ten mesh.

Noi cach khac:

- Addon cho ban mesh modular.
- Project cua ban bat/tat cac mesh co san tren player bang ten object.

---

## Nguon Mesh Goc

Nguon mesh modular:

- `Assets/Addons/PolygonFantasyHeroCharacters/Models/ModularCharacters.fbx`
- `Assets/Addons/PolygonFantasyHeroCharacters/FixedScale/Models/ModularCharacters.fbx`

Project hien tai dang dat ten mesh theo convention cua POLYGON, vi du:

- `Chr_Torso_Male_22`
- `Chr_Torso_Female_22`
- `Chr_ArmUpperRight_Male_06`
- `Chr_LegLeft_Male_16`
- `Chr_Head_No_Elements_Male_06`

Ban can dung DUNG ten nay trong `EquipmentModel`.

---

## Player Dang Lay Giap O Dau

`Player.prefab` da co san cac group mesh de bat/tat runtime:

- `hatsObject`
- `hoodsObject`
- `faceCoversObject`
- `helmetAccessoriesObject`
- `backAccessoriesObject`
- `rightShoulderObject`
- `rightElbowObject`
- `rightKneeObject`
- `leftShoulderObject`
- `leftElbowObject`
- `leftKneeObject`
- `maleFullHelmetObject`
- `maleFullBodyObject`
- `maleRightUpperArmObject`
- `maleRightLowerArmObject`
- `maleRightHandObject`
- `maleLeftUpperArmObject`
- `maleLeftLowerArmObject`
- `maleLeftHandObject`
- `maleHipsObject`
- `maleRightLegObject`
- `maleLeftLegObject`
- `femaleFullHelmetObject`
- `femaleFullBodyObject`
- `femaleRightUpperArmObject`
- `femaleRightLowerArmObject`
- `femaleRightHandObject`
- `femaleLeftUpperArmObject`
- `femaleLeftLowerArmObject`
- `femaleLeftHandObject`
- `femaleHipsObject`
- `femaleRightLegObject`
- `femaleLeftLegObject`

Nguon tham chieu:

- [Player.prefab](/D:/UnitySetup/Project/Elden%20Ring/Assets/Prefabs/Character/Player.prefab)
- [PlayerEquipmentManager.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/Character/Player/PlayerEquipmentManager.cs)

`InitializeArmorModels()` trong `PlayerEquipmentManager` quet toan bo child cua cac group tren va luu vao array. Sau do `LoadHeadEquipment`, `LoadBodyEquipment`, `LoadLegEquipment`, `LoadHandEquipment` se bat model theo ten da khai bao trong `EquipmentModel`.

---

## Asset Chinh Trong Pipeline

### 1. `EquipmentModel`

Asset nay dung de noi:

- model nay la loai gi
- ten mesh male la gi
- ten mesh female la gi

File:

- [EquipmentModel.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/Items/Equipment%20Models/EquipmentModel.cs)

3 field can dien:

- `equipmentModelType`
- `maleEquipmentName`
- `femaleEquipmentName`

### 2. `ArmorItem`

Moi mon giap la 1 `ArmorItem`:

- `HeadEquipmentItem`
- `BodyEquipmentItem`
- `LegEquipmentItem`
- `HandEquipmentItem`

File:

- [ArmorItem.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/Items/Equipment/ArmorItem.cs)

Quan trong nhat:

- `equipmentModels`

Mien la mon giap nay co du danh sach `equipmentModels`, game se biet can bat mesh nao khi equip.

### 3. `World Item Database`

Neu ban khong add item vao database, item do se khong vao he thong item/save/load/equip dung cach.

File:

- [World Item Database.prefab](/D:/UnitySetup/Project/Elden%20Ring/Assets/Prefabs/World%20Managers/World%20Item%20Database.prefab)
- [WorldItemDatabase.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/World%20Managers/WorldItemDatabase.cs)

---

## Cach Tao 1 Armor Set Moi

Vi du ban muon tao set moi ten `Royal Set`.

### Buoc 1. Chon cac mesh trong addon

Mo:

- `Assets/Addons/PolygonFantasyHeroCharacters/Models/ModularCharacters.fbx`

Tim cac mesh dung cho tung phan:

- Helm
- Body
- Legs
- Hands

Neu set co shoulder/elbow/knee/back/helmet accessory thi ghi nho them cac mesh do.

Ban can note lai ten dung cua mesh, vi du:

- `Chr_Torso_Male_10`
- `Chr_Torso_Female_10`
- `Chr_ShoulderAttachRight_03`
- `Chr_ShoulderAttachLeft_03`
- `Chr_LegRight_Male_08`
- `Chr_LegLeft_Male_08`

### Buoc 2. Tao folder model data cho set moi

Trong `Assets/Data/Items/Armor/_Armor Models/`, tao cac folder nhu:

- `Royal Armor Model Data`
- `Royal Full Helm Model Data`
- `Royal Gauntlet Model Data`
- `Royal Leggings Model Data`

Ban co the follow dung convention cua set Knight hien tai.

### Buoc 3. Tao `EquipmentModel` cho tung mesh con

Trong moi folder tren, tao cac asset `Equipment Model`.

Moi asset se map 1 mesh part.

Vi du body armor:

- `Chr_Torso_10.asset`
- `Chr_ShoulderAttachRight_03.asset`
- `Chr_ShoulderAttachLeft_03.asset`
- `Chr_ArmUpperRight_04.asset`
- `Chr_ArmUpperLeft_04.asset`

Voi moi asset:

- `equipmentModelType`: chon dung loai
- `maleEquipmentName`: ten mesh male
- `femaleEquipmentName`: ten mesh female

Map type thuong gap:

- `FullHelmet`: mesh dau kin
- `Hat`: mu non
- `Hood`: mu choang
- `HelmetAcessorie`: sung, plume, phan gan tren helm
- `FaceCover`: khau trang, mat na phan mat
- `Torso`: than tren
- `Back`: ao choang, phan sau lung
- `RightShoulder`, `LeftShoulder`
- `RightElbow`, `LeftElbow`
- `RightUpperArm`, `LeftUpperArm`
- `RightLowerArm`, `LeftLowerArm`
- `RightHand`, `LeftHand`
- `Hips`, `HipsAttachment`
- `RightLeg`, `LeftLeg`
- `RightKnee`, `LeftKnee`

### Buoc 4. Tao item armor that

Trong `Assets/Data/Items/Armor/<Ten Set>/`, tao:

- `Royal Helm.asset`
- `Royal Armor.asset`
- `Royal Greaves.asset`
- `Royal Gauntlets.asset`

Loai asset:

- `Head Equipment`
- `Body Equipment`
- `Leg Equipment`
- `Hand Equipment`

Sau do dien:

- `itemName`
- `itemIcon`
- `itemDescription`
- stat / absorption / poise
- `equipmentModels`

Keo toan bo `EquipmentModel` lien quan vao `equipmentModels`.

### Buoc 5. Set `HeadEquipmentType` neu la helm

Neu la `HeadEquipmentItem`, chon dung:

- `FullHelmet`
- `Hat`
- `Hood`
- `FaceCover`

Ly do:

- `LoadHeadEquipment()` dua vao `headEquipmentType` de tat head/hair/facial hair dung cach.

### Buoc 6. Add vao database

Mo:

- [World Item Database.prefab](/D:/UnitySetup/Project/Elden%20Ring/Assets/Prefabs/World%20Managers/World%20Item%20Database.prefab)

Them asset moi vao dung list:

- `headEquipment`
- `bodyEquipment`
- `legEquipment`
- `handEquipment`

Neu khong lam buoc nay:

- item khong co `itemID` on dinh
- save/load va equip co the sai

### Buoc 7. Gan vao class / loot / merchant neu can

Sau khi item da ton tai trong database, ban co the:

- gan vao `startingClasses`
- them vao inventory/loot
- them vao merchant stock

---

## Vi Du Thuc Te Tu Set Knight

Set Knight la template tot nhat de ban copy.

### Helm

File item:

- [Knight's Helm.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Helm.asset)

No dung:

- [Chr_Head_No_Elements_06.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/_Armor%20Models/Knight's%20Full%20Helm%20Model%20Data/Chr_Head_No_Elements_06.asset)
- [Chr_HeadCoverings_No_Hair_11.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/_Armor%20Models/Knight's%20Full%20Helm%20Model%20Data/Chr_HeadCoverings_No_Hair_11.asset)

### Body

File item:

- [Knight's Armor.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Armor.asset)

No dang dung 7 part:

- `Chr_Torso_22`
- `Chr_ShoulderAttachRight_05`
- `Chr_ShoulderAttachLeft_05`
- `Chr_ElbowAttachRight_01`
- `Chr_ElbowAttachLeft_01`
- `Chr_ArmUpperRight_06`
- `Chr_ArmUpperLeft_06`

Mot part vi du:

- [Chr_Torso_22.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/_Armor%20Models/Knight's%20Armor%20Model%20Data/Chr_Torso_22.asset)

Asset nay map:

- `equipmentModelType = Torso`
- `maleEquipmentName = Chr_Torso_Male_22`
- `femaleEquipmentName = Chr_Torso_Female_22`

### Hands

File item:

- [Knight's Gauntlets.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Gauntlets.asset)

Dang gom:

- `Chr_ArmLowerLeft_16`
- `Chr_ArmLowerRight_16`
- `Chr_HandLeft_12`
- `Chr_HandRight_12`

### Legs

File item:

- [Knight's Leggings.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Leggings.asset)

Dang gom:

- `Chr_Hips_02`
- `Chr_KneeAttachLeft_05`
- `Chr_KneeAttachRight_05`
- `Chr_LegLeft_16`
- `Chr_LegRight_16`

Chi can copy dung pattern nay cho set moi la du.

---

## Cach Lam Nhanh Nhat Trong Unity

Neu muon lam nhanh va it nham:

1. Duplicate 1 set Knight sang set moi.
2. Doi ten folder va ten asset.
3. Trong tung `EquipmentModel`, doi `maleEquipmentName` va `femaleEquipmentName` sang mesh cua set moi.
4. Trong item armor, giu nguyen danh sach part nhung thay reference sang `EquipmentModel` moi.
5. Doi icon/stat/description.
6. Add item vao `World Item Database.prefab`.

Day la cach nhanh nhat vi giu dung structure ma project dang xai.

---

## Loi Hay Gap

### 1. Dien sai ten mesh

Neu `maleEquipmentName` hoac `femaleEquipmentName` sai 1 ky tu:

- game se khong bat duoc mesh
- item equip nhung khong hien model

### 2. Quen them du part

Vi du body armor thuong khong chi co `Torso`, ma con:

- shoulder
- elbow
- upper arm
- back

Neu quen 1 part, nhan vat se lo body goc o cho do.

### 3. Quen add vao `World Item Database`

Neu quen:

- item co the khong co `itemID` on dinh
- save/load va inventory se loi

### 4. Head type sai

Neu helm la full helmet ma de `Hat`:

- head/hair co the khong bi tat dung
- se bi lo toc/xuyen mesh

### 5. Player prefab khong co mesh child do

`EquipmentModel` chi bat object da ton tai tren player.
Neu mesh do khong nam trong group armor cua `Player.prefab`, item se khong hien.

---

## Checklist Moi Khi Them Set Moi

- [ ] Tim du ten mesh male/female trong `ModularCharacters.fbx`
- [ ] Tao folder model data cho set moi
- [ ] Tao `EquipmentModel` cho tung part
- [ ] Chon dung `EquipmentModelType`
- [ ] Tao `Head/Body/Leg/HandEquipmentItem`
- [ ] Gan day du `equipmentModels`
- [ ] Dien `HeadEquipmentType` neu la head gear
- [ ] Add vao `World Item Database.prefab`
- [ ] Equip test tren male
- [ ] Equip test tren female
- [ ] Kiem tra co lo body/toc/xuyen mesh khong

---

## File Can Nho

- [Player.prefab](/D:/UnitySetup/Project/Elden%20Ring/Assets/Prefabs/Character/Player.prefab)
- [PlayerEquipmentManager.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/Character/Player/PlayerEquipmentManager.cs)
- [EquipmentModel.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/Items/Equipment%20Models/EquipmentModel.cs)
- [ArmorItem.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Scripts/Items/Equipment/ArmorItem.cs)
- [World Item Database.prefab](/D:/UnitySetup/Project/Elden%20Ring/Assets/Prefabs/World%20Managers/World%20Item%20Database.prefab)
- [Knight's Armor.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Armor.asset)
- [Knight's Helm.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Helm.asset)
- [Knight's Gauntlets.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Gauntlets.asset)
- [Knight's Leggings.asset](/D:/UnitySetup/Project/Elden%20Ring/Assets/Data/Items/Armor/Knight's%20Set/Knight's%20Leggings.asset)
