# Random Map Generator – Hướng Dẫn Sử Dụng

> **File liên quan:**
> - `Assets/Scripts/World Managers/RandomMapGenerator.cs` — logic runtime
> - `Assets/Editor/RandomMapGeneratorEditor.cs` — Custom Editor UI
> - Thư mục scene xuất ra: `Assets/Scenes/<AreaName>/`

---

## Tổng quan

Hệ thống tạo dungeon ngẫu nhiên theo quy trình:

```
[Bấm Generate]
       ↓
Sinh phòng (BSP random)  →  Kết nối hành lang
       ↓
Dựng geometry (Floor / Wall / Ceiling / Pillar / Doorway)
       ↓
Populate (Prop / Decoration / Torch / Light)
       ↓
Đặt gameplay objects (PlayerSpawn / Spawner / Boss / SiteOfGrace / FogWall)
       ↓
Phân chia Zone (zoneGridX × zoneGridZ)
       ↓
[Bấm Export] → Tạo sub-scene: _Structure / _Props / _Effects / _Spawners
```

Scale **1 tile = 1 Unity unit** → khớp với tỉ lệ 1:1 của World 01.

---

## Cách dùng

### 1. Thêm component vào scene

1. Mở scene `World_01` (hoặc `World_02`…)
2. Tạo một **Empty GameObject** đặt tên `[RandomMapGenerator]`
3. Gắn component **`RandomMapGenerator`**

### 2. Kéo prefab vào các slot

Trong Inspector, mở foldout **TILESET**:

| Slot | Mô tả | Ghi chú |
|---|---|---|
| **Floor Prefabs** | Sàn gạch/đá | Scale 1×1, pivot dưới, Y=0 |
| **Wall Prefabs** | Mảnh tường | Chiều cao khớp `wallHeight` |
| **Ceiling Prefabs** | Trần nhà | Scale 1×1 |
| **Pillar Prefabs** | Cột góc phòng | Tuỳ chọn |
| **Doorway Prefabs** | Cổng nối | Tuỳ chọn |
| **Prop Prefabs** | Hòm, bàn, thùng… | Đặt ngẫu nhiên trong phòng |
| **Decoration Prefabs** | Trang trí tường | Gắn cạnh tường |
| **Torch Prefabs** | Đuốc tường | Kèm Light component |
| **Ambient Light Prefabs** | Point Light | Trung tâm mỗi phòng |
| **Enemy Spawner Prefabs** | `AI Spawner.prefab` | Prefab có sẵn trong project |
| **Elite Spawner Prefabs** | Spawner elite | Phòng giữa map |
| **Boss Prefab** | Boss character | Phòng cuối |
| **Site of Grace Prefab** | Checkpoint | Phòng đầu |
| **Fog Wall Prefab** | Fog wall | Lối vào phòng boss |
| **Player Spawn Point Prefab** | Spawn marker | Phòng đầu (có thể để trống → tạo empty) |

> **Tip:** Dùng prefab từ `Assets/Prefabs/Spawners/AI Spawner.prefab` cho Enemy Spawner.

### 3. Cấu hình map

| Tham số | Mô tả | Gợi ý |
|---|---|---|
| Map Width/Height | Số tile (1 tile = 1 unit) | 60×60 ≈ kích thước World 01 |
| Wall Height | Chiều cao tường (units) | 4 |
| Min/Max Room Size | Kích thước phòng (tiles) | 6–14 |
| Max Rooms | Số phòng tối đa | 8–12 |
| Zone Grid X/Z | Chia map thành NxN zone | 2×2 = 4 zone, mỗi zone → 4 scene |
| Prop/Decoration/Torch Density | Mật độ [0–1] | 0.15 / 0.2 / 0.3 |
| Use Random Seed | Tắt để dùng seed cố định | Bật khi cần map khác nhau |

### 4. Tạo map

Bấm **▶ TẠO MAP RANDOM** — map sẽ xuất hiện ngay trong scene hiện tại dưới object `[Generated] Area_XX`.

### 5. Xuất ra sub-scene

1. Nhập **World Scene Name** và **Area Name** (ví dụ: `World_02`, `Area_02`)
2. Mở foldout **XUẤT RA SUB-SCENE**
3. Bấm **🏗 XUẤT MAP THÀNH SUB-SCENES**
4. Script tự động:
   - Tạo thư mục `Assets/Scenes/Area_02/`
   - Với mỗi zone sinh ra N scene con:
     - `Area_02_Zone_0_0_Structure.unity`
     - `Area_02_Zone_0_0_Props.unity`
     - `Area_02_Zone_0_0_Effects.unity`
     - `Area_02_Zone_0_0_Spawners.unity`
   - Lưu tất cả scene

### 6. Thêm vào Build Settings & WorldSceneManager

Sau khi xuất, cần thêm thủ công:
- Mở **File > Build Settings** → kéo tất cả scene mới vào danh sách
- Cập nhật `WorldSceneManager.cs` với tên scene mới (hoặc load động qua tên)

---

## Quy tắc phân loại object vào sub-scene

| Parent trong hierarchy | → Sub-scene |
|---|---|
| `Structure/`, `Floors`, `Walls`, `Ceilings`, `Pillars`, `Doorways` | `_Structure` |
| `Props/`, `Decorations`, `Ruins` | `_Props` |
| `Effects/`, `Lights` | `_Effects` |
| `Spawners/`, `Gameplay/` | `_Spawners` |

---

## Lưu ý kỹ thuật

- **Scale 1:1**: Mỗi floor tile đặt tại `Vector3(x, 0, z)`. Prefab floor cần pivot chính giữa đáy và scale `1×1×1`. Tường đặt offset 0.5f ra khỏi tile.
- **Seed**: Sau khi Generate, seed được lưu lại vào `config.seed` để bạn có thể tái tạo cùng map.
- **Undo/Clear**: Dùng nút **🗑 XOÁ MAP** để xoá map đã tạo trước khi Generate lại.
- **Additive loading**: Sub-scene sau khi xuất được `WorldSceneManager.LoadAdditiveScenes()` load vào — khớp với quy trình hiện tại của project.
