# Weapon Preview Workflow

Tai lieu nay mo ta workflow de review va dat lai ten cho toan bo weapon data dua tren model/prefab that su dang dung trong game.

---

## Tool Moi

Editor tool moi:

- `Tools/Weapons/Export Weapon Previews + Manifest`
- `Tools/Weapons/Apply Weapon Names From Override CSV`
- `Tools/Weapons/Apply Auto Suggested Weapon Names`
- `Tools/Weapons/Sync Weapon Asset File Names`
- `Tools/Weapons/Export Weapon Material Colors + Render Prompts`

Script:

- [WeaponPreviewExporter.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Game/Scripts/Editor/WeaponPreviewExporter.cs)

---

## Tool Se Tao Ra Gi

Sau khi chay:

- `Assets/Game/Docs/WeaponPreviewExports/Previews/*.png`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_preview_manifest.csv`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_preview_manifest.json`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_name_overrides.csv`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_material_color_manifest.csv`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_material_color_manifest.json`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_render_prompts_material_locked.csv`
- `Assets/Game/Docs/WeaponPreviewExports/weapon_render_prompts_material_locked.json`

Manifest se gom:

- `assetPath`
- `currentObjectName`
- `currentDisplayName`
- `suggestedDisplayName`
- `itemType`
- `weaponClass`
- `weaponModelType`
- `prefabName`
- `meshNames`
- `previewPath`
- `namingNotes`

---

## Cach Review

1. Chay `Tools/Weapons/Export Weapon Previews + Manifest`.
2. Mo folder `Assets/Game/Docs/WeaponPreviewExports/Previews/`.
3. Doi chieu preview PNG voi `weapon_preview_manifest.csv`.
4. Sua cot `displayName` trong `weapon_name_overrides.csv`.
5. Chay `Tools/Weapons/Apply Weapon Names From Override CSV`.

Luu y:

- Khi apply ten, tool se doi ca `itemName`, `m_Name`, va asset filename de tranh warning `Main Object Name does not match filename`.
- Neu da co data doi ten tu truoc do, co the chay `Tools/Weapons/Sync Weapon Asset File Names` de dong bo lai filename ma khong doi display name.

Neu muon lay ngay mot pass ten tu dong ban dau:

- Chay `Tools/Weapons/Apply Auto Suggested Weapon Names`

Neu muon khoa prompt theo mau prefab thay vi chi dua vao preview:

- Chay `Tools/Weapons/Export Weapon Material Colors + Render Prompts`
- Tool nay doc material color truc tiep tu prefab/model renderer
- Output se co `colorNotes` va prompt uu tien `exact color blocking`

Khuyen nghi:

- Dung `Apply Auto Suggested Weapon Names` chi de tao first pass.
- Sau do review bang preview PNG va apply lai bang file override CSV.

---

## Muc Tieu Dat Ten

Naming nen:

- ngan
- de doc trong shop
- bam vao silhouette/model that
- giong style item/armor hien co trong project

Vi du:

- `Straight Sword`
- `Broadsword`
- `Greatsword`
- `Buckler`
- `Heater Shield`
- `Round Shield`
- `Goblin Spear`
- `Crystal Halberd`

---

## Ghi Chu

Tool nay khong sua stat, ID, hay database wiring.
No chi phuc vu:

- preview model
- tao manifest review
- apply display name vao `WeaponItem` asset
- dong bo asset filename voi main object name
