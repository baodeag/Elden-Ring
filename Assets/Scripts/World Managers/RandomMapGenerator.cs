using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace baodeag
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Data structures
    // ─────────────────────────────────────────────────────────────────────────

    [System.Serializable]
    public class MapTileset
    {
        [Header("── Cấu trúc ──")]
        public GameObject[] floorPrefabs;           // Sàn (scale 1x1, Y=0)
        public GameObject[] wallPrefabs;            // Tường (thẳng đứng)
        public GameObject[] wallArchPrefabs;        // Mảnh cung/vòm trên đỉnh tường (SM_Env_Ceiling_Stone_Curved_01)
        public GameObject[] ceilingPrefabs;         // Trần (scale 1x1, cao)
        public GameObject[] pillarPrefabs;          // Cột góc phòng
        public GameObject[] doorwayPrefabs;         // Cổng nối phòng
        public GameObject[] stairPrefabs;           // Cầu thang (nếu cần)

        [Header("── Trang trí / Props ──")]
        public GameObject[] propPrefabs;            // Đồ vật (bàn, hòm, thùng…)
        public GameObject[] decorationPrefabs;      // Trang trí tường / góc
        public GameObject[] ruinPrefabs;            // Mảnh vỡ, đổ nát

        [Header("── Ánh sáng ──")]
        public GameObject[] torchPrefabs;           // Đuốc tường
        public GameObject[] lanternPrefabs;         // Đèn treo
        public GameObject[] ambientLightPrefabs;    // Point light ambient

        [Header("── Gameplay ──")]
        public GameObject[] enemySpawnerPrefabs;    // AI Spawner thường
        public GameObject[] eliteSpawnerPrefabs;    // AI Spawner elite
        public GameObject bossPrefab;               // Boss (1 boss/map)
        public GameObject siteOfGracePrefab;        // Site of Grace / checkpoint
        public GameObject fogWallPrefab;            // Fog wall trước boss
        public GameObject playerSpawnPointPrefab;   // Điểm spawn player
    }

    [System.Serializable]
    public class MapGenerationConfig
    {
        [Header("── Kích thước Prefab ──")]
        [Tooltip("Kích thước thực của 1 tile sàn theo trục X và Z (Unity units).\nVí dụ: prefab floor rộng 5m → nhập 5. Phải khớp với mesh prefab để không bị dính.")]
        [Range(0.25f, 10f)] public float tileSize = 5f;
        [Tooltip("Độ dày của prefab tường (Unity units). Thường bằng tileSize hoặc nhỏ hơn.")]
        [Range(0.1f, 5f)] public float wallThickness = 0.5f;
        [Tooltip("Chiều cao thực của prefab tường (Unity units). Phải khớp với mesh.")]
        [Range(1f, 20f)] public float wallHeight = 5f;

        [Header("── Số ô (tiles) ──")]
        [Tooltip("Số tile theo trục X — kích thước map thực = mapWidth × tileSize")]
        [Range(10, 200)] public int mapWidth = 30;
        [Tooltip("Số tile theo trục Z — kích thước map thực = mapHeight × tileSize")]
        [Range(10, 200)] public int mapHeight = 30;

        [Header("── Phòng ──")]
        [Range(3, 30)] public int minRoomSize = 4;
        [Range(5, 50)] public int maxRoomSize = 10;
        [Range(3, 20)] public int maxRooms = 8;

        [Header("── Phân chia khu (Area Zones) ──")]
        [Tooltip("Chia map thành NxN ô zone để quản lý theo additive scene")]
        [Range(1, 5)] public int zoneGridX = 2;
        [Range(1, 5)] public int zoneGridZ = 2;

        [Header("── Mật độ nội thất ──")]
        [Range(0f, 1f)] public float propDensity = 0.15f;
        [Range(0f, 1f)] public float decorationDensity = 0.2f;
        [Range(0f, 1f)] public float torchDensity = 0.3f;

        [Header("── Seed ──")]
        public bool useRandomSeed = true;
        public int seed = 42;
    }

    [System.Serializable]
    public class GeneratedZoneInfo
    {
        public string zoneName;
        public Bounds zoneBounds;           // world-space bounds
        public List<GameObject> objects;    // tất cả object trong zone này

        public GeneratedZoneInfo(string name, Bounds bounds)
        {
            zoneName = name;
            zoneBounds = bounds;
            objects = new List<GameObject>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Main generator
    // ─────────────────────────────────────────────────────────────────────────

    public class RandomMapGenerator : MonoBehaviour
    {
        private const float WallYSink = 0.23764f;
        private const float CeilingHeightAboveFloor = 10f;

        // ── Inspector ──────────────────────────────────────────────────────

        [Header("═══ TILESET ═══")]
        public MapTileset tileset = new MapTileset();

        [Header("═══ CẤU HÌNH MAP ═══")]
        public MapGenerationConfig config = new MapGenerationConfig();

        [Header("═══ THÔNG TIN XUẤT RA ═══")]
        [Tooltip("Tên world scene (World_02, World_03…) dùng để đặt tên sub-scene khi export")]
        public string worldSceneName = "World_02";
        [Tooltip("Tên khu vực (Area_02, Area_03…) dùng để đặt tên sub-scene")]
        public string areaName = "Area_02";

        // ── Runtime dữ liệu nội bộ ───────────────────────────────────────

        private System.Random rng;
        private bool[,] floorMap;       // true = floor tile
        private List<RectInt> rooms = new List<RectInt>();
        private Transform generatedRoot;

        // Tự động phát hiện sau khi đo prefab floor thực tế
        private float detectedStepX = 5f;     // khoảng cách thực giữa 2 tile theo X
        private float detectedStepZ = 5f;     // khoảng cách thực giữa 2 tile theo Z
        private float detectedFloorTopY = 0f; // Y của mặt trên sàn (floor surface)
        private float detectedWallYBase = 0f; // Y để đáy wall nằm trên sàn
        private float detectedWallTopY  = 5f; // Y của đỉnh wall trong world space
        private float detectedWallWidthOfs = 0f; // offset tâm mesh wall theo local X (chiều rộng)
        private float detectedWallArchWidthOfs = 0f; // offset tâm mesh mái/vòm theo local X

        // Được lưu sau khi dựng sàn: tile coord → bounds thực của mesh trong world space
        private Dictionary<Vector2Int, Bounds> floorBounds = new Dictionary<Vector2Int, Bounds>();

        [HideInInspector] public List<GeneratedZoneInfo> generatedZones = new List<GeneratedZoneInfo>();

        // ── Public API ────────────────────────────────────────────────────

        /// <summary>Tạo toàn bộ map. Gọi từ Editor hoặc runtime.</summary>
        public void GenerateMap()
        {
            ClearMap();

            // Khởi tạo seed
            int usedSeed = config.useRandomSeed ? Random.Range(0, int.MaxValue) : config.seed;
            config.seed = usedSeed;      // lưu lại để biết seed đã dùng
            rng = new System.Random(usedSeed);
            Random.InitState(usedSeed);

            // Root object
            generatedRoot = new GameObject($"[Generated] {areaName}").transform;
            generatedRoot.SetParent(transform);
            generatedRoot.localPosition = Vector3.zero;

            // 0. Tự động đo kích thước thực của prefab → dùng làm bước tile và Y nền
            DetectTileSize();
            DetectWallYBase();
            DetectWallArchOffset();

            // 1. Tạo bản đồ phòng (BSP-like)
            floorMap = new bool[config.mapWidth, config.mapHeight];
            rooms.Clear();
            PlaceRooms();
            ConnectRoomsWithCorridors();

            // 2. Dựng geometry
            BuildFloors();
            BuildWalls();
            BuildWallArches();   // vòm/cung trên đỉnh tường
            BuildCeilings();
            BuildPillars();

            // 3. Populate
            PlaceDoorways();
            PopulateProps();
            PopulateDecorations();
            PlaceLights();

            // 4. Gameplay objects
            PlacePlayerSpawn();
            PlaceEnemySpawners();
            PlaceSiteOfGrace();
            PlaceBossRoom();

            // 5. Phân chia zone
            BuildZones();

            Debug.Log($"[RandomMapGenerator] Map '{areaName}' generated. Seed={usedSeed}, Rooms={rooms.Count}, Zones={generatedZones.Count}");
        }

        /// <summary>Xoá toàn bộ map đã tạo.</summary>
        public void ClearMap()
        {
            generatedZones.Clear();
            rooms.Clear();
            floorBounds.Clear();
            floorMap = null;

            // Xoá tất cả object con có tên bắt đầu bằng "[Generated]"
            List<Transform> toDestroy = new List<Transform>();
            foreach (Transform child in transform)
            {
                if (child.name.StartsWith("[Generated]"))
                    toDestroy.Add(child);
            }

            foreach (var t in toDestroy)
            {
#if UNITY_EDITOR
                DestroyImmediate(t.gameObject);
#else
                Destroy(t.gameObject);
#endif
            }

            generatedRoot = null;
        }

        // ── Room placement (simple random BSP) ───────────────────────────

        private void PlaceRooms()
        {
            int attempts = config.maxRooms * 10;
            int placed = 0;

            for (int i = 0; i < attempts && placed < config.maxRooms; i++)
            {
                int w = rng.Next(config.minRoomSize, config.maxRoomSize + 1);
                int h = rng.Next(config.minRoomSize, config.maxRoomSize + 1);
                int x = rng.Next(1, config.mapWidth - w - 1);
                int z = rng.Next(1, config.mapHeight - h - 1);

                RectInt candidate = new RectInt(x, z, w, h);

                bool overlaps = false;
                foreach (var r in rooms)
                {
                    RectInt expanded = new RectInt(r.x - 1, r.y - 1, r.width + 2, r.height + 2);
                    if (expanded.Overlaps(candidate)) { overlaps = true; break; }
                }

                if (!overlaps)
                {
                    rooms.Add(candidate);
                    CarveRoom(candidate);
                    placed++;
                }
            }
        }

        private void CarveRoom(RectInt room)
        {
            for (int x = room.x; x < room.x + room.width; x++)
                for (int z = room.y; z < room.y + room.height; z++)
                    floorMap[x, z] = true;
        }

        private void ConnectRoomsWithCorridors()
        {
            for (int i = 1; i < rooms.Count; i++)
            {
                Vector2Int a = RoomCenter(rooms[i - 1]);
                Vector2Int b = RoomCenter(rooms[i]);
                CarveHCorridor(a.x, b.x, a.y);
                CarveVCorridor(a.y, b.y, b.x);
            }
        }

        private void CarveHCorridor(int x1, int x2, int z)
        {
            int minX = Mathf.Min(x1, x2);
            int maxX = Mathf.Max(x1, x2);
            for (int x = minX; x <= maxX; x++)
                SetFloor(x, z, 2);
        }

        private void CarveVCorridor(int z1, int z2, int x)
        {
            int minZ = Mathf.Min(z1, z2);
            int maxZ = Mathf.Max(z1, z2);
            for (int z = minZ; z <= maxZ; z++)
                SetFloor(x, z, 2);
        }

        private void SetFloor(int x, int z, int halfWidth = 1)
        {
            for (int dx = -halfWidth + 1; dx < halfWidth; dx++)
                for (int dz = -halfWidth + 1; dz < halfWidth; dz++)
                {
                    int nx = x + dx, nz = z + dz;
                    if (nx >= 0 && nx < config.mapWidth && nz >= 0 && nz < config.mapHeight)
                        floorMap[nx, nz] = true;
                }
        }

        private Vector2Int RoomCenter(RectInt r) => new Vector2Int(r.x + r.width / 2, r.y + r.height / 2);

        /// <summary>
        /// Đo kích thước thực của floor prefab bằng cách spawn tạm 1 instance,
        /// lấy Renderer.bounds, rồi destroy ngay.
        /// Kết quả lưu vào detectedStepX/Z — dùng cho mọi tính toán vị trí tile sau đó.
        /// </summary>
        private void DetectTileSize()
        {
            // Fallback về config.tileSize nếu chưa có prefab
            detectedStepX = config.tileSize;
            detectedStepZ = config.tileSize;
            detectedFloorTopY = 0f;

            if (tileset.floorPrefabs == null || tileset.floorPrefabs.Length == 0) return;
            GameObject prefab = tileset.floorPrefabs[0];
            if (prefab == null) return;

            // Spawn tạm tại gốc toạ độ
            GameObject temp = SpawnSingle(prefab, Vector3.zero, Quaternion.identity, generatedRoot);
            if (temp == null) return;

            Bounds b = GetWorldBounds(temp);

            // Size.x và Size.z chính là bước tile thực cần dùng
            if (b.size.x > 0.01f) detectedStepX = b.size.x;
            if (b.size.z > 0.01f) detectedStepZ = b.size.z;

            // Mặt trên sàn = bounds.max.y của floor khi đặt tại Y=0
            detectedFloorTopY = b.max.y;

#if UNITY_EDITOR
            DestroyImmediate(temp);
#else
            Destroy(temp);
#endif

            Debug.Log($"[RandomMapGenerator] Floor: stepX={detectedStepX:F3}, stepZ={detectedStepZ:F3}, surface Y={detectedFloorTopY:F3}");
        }

        /// <summary>
        /// Đo vị trí Y cần đặt wall để đáy wall nằm đúng trên mặt sàn.
        /// Hoạt động với cả pivot-center lẫn pivot-bottom của wall prefab.
        /// </summary>
        private void DetectWallYBase()
        {
            detectedWallYBase = detectedFloorTopY;
            detectedWallWidthOfs = 0f;
            detectedWallTopY = detectedFloorTopY + config.wallHeight; // fallback

            if (tileset.wallPrefabs == null || tileset.wallPrefabs.Length == 0) return;
            GameObject prefab = tileset.wallPrefabs[0];
            if (prefab == null) return;

            GameObject temp = SpawnSingle(prefab, Vector3.zero, Quaternion.identity, generatedRoot);
            if (temp == null) return;

            Bounds b = GetWorldBounds(temp);
            Vector3 pivotPos = temp.transform.position;

            // pivotToBottom: khoảng cách từ pivot (Y=0) đến đáy wall
            float pivotToBottom  = b.min.y - pivotPos.y;
            detectedWallYBase    = detectedFloorTopY - pivotToBottom;

            // đỉnh wall = YBase + khoảng từ pivot đến top (khi spawn tại origin)
            float pivotToTop     = b.max.y - pivotPos.y;
            detectedWallTopY     = detectedWallYBase + pivotToTop;

            // ── Width offset: tâm mesh theo local X lệch bao nhiêu so với pivot? ──
            // Ví dụ: wall pivot ở góc trái (X=0), mesh từ 0→5 → tâm mesh tại X=2.5 → ofs=+2.5
            //          wall pivot ở tâm (X=2.5), mesh từ -2.5→2.5 → tâm mesh tại X=0  → ofs=0
            // Sau khi xoay 90°, offset này thành lệch theo world Z (hoặc X).
            // Khi đặt wall, ta cần tử trừ đi offset này để giữ tâm mesh ở đúng vị trí.
            float meshCenterX = (b.max.x + b.min.x) * 0.5f;       // world X của tâm mesh
            detectedWallWidthOfs = meshCenterX - pivotPos.x;       // offset từ pivot đến tâm mesh (local X)

#if UNITY_EDITOR
            DestroyImmediate(temp);
#else
            Destroy(temp);
#endif

            Debug.Log($"[RandomMapGenerator] Wall detected: Y base={detectedWallYBase:F3} | top={detectedWallTopY:F3} | widthOffset={detectedWallWidthOfs:F3}");
        }

        private void DetectWallArchOffset()
        {
            detectedWallArchWidthOfs = 0f;

            if (tileset.wallArchPrefabs == null || tileset.wallArchPrefabs.Length == 0) return;
            GameObject prefab = tileset.wallArchPrefabs[0];
            if (prefab == null) return;

            GameObject temp = SpawnSingle(prefab, Vector3.zero, Quaternion.identity, generatedRoot);
            if (temp == null) return;

            Bounds b = GetWorldBounds(temp);
            Vector3 pivotPos = temp.transform.position;
            float meshCenterX = (b.max.x + b.min.x) * 0.5f;
            detectedWallArchWidthOfs = meshCenterX - pivotPos.x;

#if UNITY_EDITOR
            DestroyImmediate(temp);
#else
            Destroy(temp);
#endif

            Debug.Log($"[RandomMapGenerator] Wall arch detected: widthOffset={detectedWallArchWidthOfs:F3}");
        }

        // ── Coordinate helpers ────────────────────────────────────────────

        /// <summary>Chuyển toạ độ tile (integer) sang world position dùng bước tile thực.</summary>
        private Vector3 T(int x, int z, float y = 0f)
            => new Vector3(x * detectedStepX, y, z * detectedStepZ);

        /// <summary>Toạ độ cạnh tile: offset 0.5 bước tile từ tâm.</summary>
        private Vector3 TEdge(int tileX, int tileZ, int dirX, int dirZ, float y = 0f)
            => new Vector3(
                tileX * detectedStepX + dirX * detectedStepX * 0.5f,
                y,
                tileZ * detectedStepZ + dirZ * detectedStepZ * 0.5f
            );

        // ── Geometry: Floor first, then Walls from bounds ─────────────────

        /// <summary>
        /// Bước 1: Đặt toàn bộ floor tile, ghi lại bounds mesh thực của từng floor.
        /// </summary>
        private void BuildFloors()
        {
            if (tileset.floorPrefabs == null || tileset.floorPrefabs.Length == 0) return;
            Transform parent = GetOrCreateChild("Structure/Floors");
            floorBounds.Clear();

            for (int x = 0; x < config.mapWidth; x++)
            {
                for (int z = 0; z < config.mapHeight; z++)
                {
                    if (!floorMap[x, z]) continue;

                    // Đặt floor tại vị trí tile (x * tileSize, 0, z * tileSize)
                    GameObject go = SpawnPrefab(tileset.floorPrefabs, T(x, z, 0f), Quaternion.identity, parent);
                    if (go == null) continue;

                    // Lấy bounds mesh thực tằ cả child renderers
                    Bounds b = GetWorldBounds(go);
                    floorBounds[new Vector2Int(x, z)] = b;
                }
            }
        }

        /// <summary>
        /// Bước 2: Với mỗi floor đã dựng, kiểm tra 4 cạnh.
        /// Nếu cạnh không có floor liền kề → đặt tường tại đườc cạnh đó của Bounds.
        /// Vị trí lấy từ bounds.max/min nên không phụ thuộc pivot.
        /// </summary>
        private void BuildWalls()
        {
            if (tileset.wallPrefabs == null || tileset.wallPrefabs.Length == 0) return;
            if (floorBounds.Count == 0) return;
            Transform parent = GetOrCreateChild("Structure/Walls");

            // Hướng kiểm tra: +X, -X, +Z, -Z (trong tile-space)
            Vector2Int[] dirs =
            {
                new Vector2Int( 1,  0),   // right (+X)
                new Vector2Int(-1,  0),   // left  (-X)
                new Vector2Int( 0,  1),   // front (+Z)
                new Vector2Int( 0, -1),   // back  (-Z)
            };

            // Rotation: width wall phải nằm dọc theo cạnh mà nó che
            // Wall prefab 5×5×0.5 (X×Y×Z, pivot center):
            //   rotY=90 : local X(5) → world +Z  → dùng cho cạnh +X và -X
            //   rotY=0  : local X(5) → world +X  → dùng cho cạnh +Z và -Z
            float[] rotY = { 90f, 270f, 0f, 180f };

            HashSet<string> placed = new HashSet<string>();

            foreach (var kv in floorBounds)
            {
                Vector2Int tile    = kv.Key;
                Bounds      bounds = kv.Value;

                for (int d = 0; d < 4; d++)
                {
                    Vector2Int neighbor = tile + dirs[d];

                    // Có floor liền kề → không cần tường
                    if (floorBounds.ContainsKey(neighbor)) continue;

                    // XZ: cạnh bounds floor + bù offset width của wall mesh
                    // detectedWallWidthOfs = độ lệch tâm mesh wall khỏi pivot theo local X
                    //
                    // Unity rotY=90°:  local +X → World -Z  (KHÔNG phải +Z!)
                    // Unity rotY=270°: local +X → World +Z
                    // Unity rotY=0°:   local +X → World +X
                    // Unity rotY=180°: local +X → World -X
                    //
                    // Wall pivot ở cạnh trái (local X=0): mesh chạy từ pivot → pivot + widthOfs*2
                    // Để tâm mesh trùng với tâm tile:
                    //   rotY= 90°: pivot.z + ofs*(-1) = center.z  → wz = center.z + ofs  (local X → -Z, nên cộng)
                    //   rotY=270°: pivot.z + ofs*(+1) = center.z  → wz = center.z - ofs  (local X → +Z, nên trừ)
                    //   rotY=  0°: pivot.x + ofs*(+1) = center.x  → wx = center.x - ofs
                    //   rotY=180°: pivot.x + ofs*(-1) = center.x  → wx = center.x + ofs
                    float wx, wz;
                    switch (d)
                    {
                        case 0: // right (+X), rotY=90°: local X → World -Z → wz = center.z + ofs
                            wx = bounds.max.x;
                            wz = bounds.center.z + detectedWallWidthOfs;
                            break;
                        case 1: // left (-X), rotY=270°: local X → World +Z → wz = center.z - ofs
                            wx = bounds.min.x;
                            wz = bounds.center.z - detectedWallWidthOfs;
                            break;
                        case 2: // front (+Z), rotY=0°: local X → World +X → wx = center.x - ofs
                            wx = bounds.center.x - detectedWallWidthOfs;
                            wz = bounds.max.z;
                            break;
                        default: // back (-Z), rotY=180°: local X → World -X → wx = center.x + ofs
                            wx = bounds.center.x + detectedWallWidthOfs;
                            wz = bounds.min.z;
                            break;
                    }

                    Vector3 wallPos = new Vector3(wx, detectedWallYBase - WallYSink, wz);

                    // Dedup: key làm tròn 2 chữ số
                    string key = $"{wx:F2}_{wz:F2}_{d}";
                    if (placed.Contains(key)) continue;
                    placed.Add(key);

                    SpawnPrefab(tileset.wallPrefabs, wallPos, Quaternion.Euler(0, rotY[d], 0), parent);
                }
            }
        }

        /// <summary>
        /// Đặt mái/vòm lên đúng các cạnh tường thẳng, bỏ qua góc và xoay phần nhô về phía sàn.
        /// </summary>
        private void BuildWallArches()
        {
            if (tileset.wallArchPrefabs == null || tileset.wallArchPrefabs.Length == 0) return;
            if (floorBounds.Count == 0) return;
            Transform parent = GetOrCreateChild("Structure/WallArches");

            Vector2Int[] dirs =
            {
                new Vector2Int( 1,  0),
                new Vector2Int(-1,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 0, -1),
            };

            HashSet<string> placed = new HashSet<string>();

            foreach (var kv in floorBounds)
            {
                Vector2Int tile = kv.Key;
                Bounds bounds = kv.Value;

                for (int d = 0; d < dirs.Length; d++)
                {
                    Vector2Int outward = dirs[d];
                    if (!HasBoundaryWall(tile, outward)) continue;
                    if (HasPerpendicularWallAtEitherEnd(tile, outward)) continue;

                    Vector3 inward = new Vector3(-outward.x, 0f, -outward.y);
                    Quaternion rotation = Quaternion.LookRotation(inward, Vector3.up);
                    Vector3 archPos = GetWallArchEdgePosition(bounds, d, GetWallArchY(), rotation);
                    string key = $"{archPos.x:F2}_{archPos.z:F2}_{d}";
                    if (placed.Contains(key)) continue;
                    placed.Add(key);

                    SpawnPrefab(tileset.wallArchPrefabs, archPos, rotation, parent);
                }
            }
        }

        private bool HasBoundaryWall(Vector2Int floorTile, Vector2Int outward)
        {
            if (!floorBounds.ContainsKey(floorTile)) return false;
            return !floorBounds.ContainsKey(floorTile + outward);
        }

        private bool HasPerpendicularWallAtEitherEnd(Vector2Int floorTile, Vector2Int outward)
        {
            GetWallEndpoints2(floorTile, outward, out Vector2Int endA, out Vector2Int endB);

            Vector2Int[] dirs =
            {
                new Vector2Int( 1,  0),
                new Vector2Int(-1,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 0, -1),
            };

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dz = -1; dz <= 1; dz++)
                {
                    Vector2Int candidateTile = new Vector2Int(floorTile.x + dx, floorTile.y + dz);
                    if (!floorBounds.ContainsKey(candidateTile)) continue;

                    foreach (Vector2Int candidateOutward in dirs)
                    {
                        if (!IsPerpendicular(outward, candidateOutward)) continue;
                        if (!HasBoundaryWall(candidateTile, candidateOutward)) continue;

                        GetWallEndpoints2(candidateTile, candidateOutward, out Vector2Int candidateEndA, out Vector2Int candidateEndB);
                        if (SharesEndpoint(endA, candidateEndA, candidateEndB) ||
                            SharesEndpoint(endB, candidateEndA, candidateEndB))
                            return true;
                    }
                }
            }

            return false;
        }

        private bool IsPerpendicular(Vector2Int a, Vector2Int b)
        {
            return a.x * b.x + a.y * b.y == 0;
        }

        private bool SharesEndpoint(Vector2Int endpoint, Vector2Int otherA, Vector2Int otherB)
        {
            return endpoint == otherA || endpoint == otherB;
        }

        private void GetWallEndpoints2(Vector2Int floorTile, Vector2Int outward, out Vector2Int a, out Vector2Int b)
        {
            int centerX2 = floorTile.x * 2;
            int centerZ2 = floorTile.y * 2;

            if (outward.x != 0)
            {
                int edgeX2 = centerX2 + outward.x;
                a = new Vector2Int(edgeX2, centerZ2 - 1);
                b = new Vector2Int(edgeX2, centerZ2 + 1);
            }
            else
            {
                int edgeZ2 = centerZ2 + outward.y;
                a = new Vector2Int(centerX2 - 1, edgeZ2);
                b = new Vector2Int(centerX2 + 1, edgeZ2);
            }
        }

        private Vector3 GetWallEdgePosition(Bounds bounds, int directionIndex, float y)
        {
            float wx, wz;
            switch (directionIndex)
            {
                case 0:
                    wx = bounds.max.x;
                    wz = bounds.center.z + detectedWallWidthOfs;
                    break;
                case 1:
                    wx = bounds.min.x;
                    wz = bounds.center.z - detectedWallWidthOfs;
                    break;
                case 2:
                    wx = bounds.center.x - detectedWallWidthOfs;
                    wz = bounds.max.z;
                    break;
                default:
                    wx = bounds.center.x + detectedWallWidthOfs;
                    wz = bounds.min.z;
                    break;
            }

            return new Vector3(wx, y, wz);
        }

        private Vector3 GetWallArchEdgePosition(Bounds bounds, int directionIndex, float y, Quaternion rotation)
        {
            Vector3 edgeCenter;
            switch (directionIndex)
            {
                case 0:
                    edgeCenter = new Vector3(bounds.max.x, y, bounds.center.z);
                    break;
                case 1:
                    edgeCenter = new Vector3(bounds.min.x, y, bounds.center.z);
                    break;
                case 2:
                    edgeCenter = new Vector3(bounds.center.x, y, bounds.max.z);
                    break;
                default:
                    edgeCenter = new Vector3(bounds.center.x, y, bounds.min.z);
                    break;
            }

            return edgeCenter - rotation * Vector3.right * detectedWallArchWidthOfs;
        }

        /// <summary>
        /// Bước 3: Đặt trần dựa vào tâm xích tam bounds floor.
        /// </summary>
        private void BuildCeilings()
        {
            if (tileset.ceilingPrefabs == null || tileset.ceilingPrefabs.Length == 0) return;
            if (floorBounds.Count == 0) return;
            Transform parent = GetOrCreateChild("Structure/Ceilings");

            foreach (var kv in floorBounds)
            {
                Vector2Int tile = kv.Key;
                if (!IsInteriorFloorTile(tile)) continue;

                // Tâm XZ giống floor, Y = wallHeight
                Vector3 pos = T(tile.x, tile.y, CeilingHeightAboveFloor);
                SpawnPrefab(tileset.ceilingPrefabs, pos, Quaternion.identity, parent);
            }
        }

        private float GetWallArchY()
        {
            return detectedWallYBase - WallYSink + 5f;
        }

        private bool IsInteriorFloorTile(Vector2Int tile)
        {
            if (!floorBounds.ContainsKey(tile)) return false;

            for (int dx = -1; dx <= 1; dx++)
                for (int dz = -1; dz <= 1; dz++)
                    if ((dx != 0 || dz != 0) && !floorBounds.ContainsKey(tile + new Vector2Int(dx, dz)))
                        return false;

            return true;
        }

        private void BuildPillars()
        {
            if (tileset.pillarPrefabs == null || tileset.pillarPrefabs.Length == 0) return;
            Transform parent = GetOrCreateChild("Structure/Pillars");

            // Đặt cột ở 4 góc mỗi phòng
            foreach (var room in rooms)
            {
                PlacePillarAt(room.x, room.y, parent);
                PlacePillarAt(room.x + room.width - 1, room.y, parent);
                PlacePillarAt(room.x, room.y + room.height - 1, parent);
                PlacePillarAt(room.x + room.width - 1, room.y + room.height - 1, parent);
            }
        }

        private void PlacePillarAt(int x, int z, Transform parent)
        {
            SpawnPrefab(tileset.pillarPrefabs, T(x, z, 0f), Quaternion.identity, parent);
        }

        // ── Doorways ──────────────────────────────────────────────────────

        private void PlaceDoorways()
        {
            if (tileset.doorwayPrefabs == null || tileset.doorwayPrefabs.Length == 0) return;
            Transform parent = GetOrCreateChild("Structure/Doorways");

            // Đặt doorway ở trung điểm hành lang nối 2 phòng liên tiếp
            for (int i = 1; i < rooms.Count; i++)
            {
                Vector2Int a = RoomCenter(rooms[i - 1]);
                Vector2Int b = RoomCenter(rooms[i]);
                // Dùng tileSize để tính trung điểm world position
                Vector3 mid = new Vector3((a.x + b.x) * 0.5f * config.tileSize, 0f, (a.y + b.y) * 0.5f * config.tileSize);
                float angle = Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
                SpawnPrefab(tileset.doorwayPrefabs, mid, Quaternion.Euler(0, angle, 0), parent);
            }
        }

        // ── Props / Decorations / Lights ──────────────────────────────────

        private void PopulateProps()
        {
            if (tileset.propPrefabs == null || tileset.propPrefabs.Length == 0) return;
            Transform parent = GetOrCreateChild("Props");

            foreach (var room in rooms)
            {
                // Bỏ qua phòng đầu (spawn) và phòng cuối (boss)
                int idx = rooms.IndexOf(room);
                if (idx == 0 || idx == rooms.Count - 1) continue;

                for (int x = room.x + 1; x < room.x + room.width - 1; x++)
                {
                    for (int z = room.y + 1; z < room.y + room.height - 1; z++)
                    {
                        if ((float)rng.NextDouble() < config.propDensity)
                        {
                            // Nhân tileSize, offset nhỏ để không đứng đúng giữa tile
                            Vector3 pos = new Vector3(
                                x * config.tileSize + RandomOffset() * config.tileSize,
                                0f,
                                z * config.tileSize + RandomOffset() * config.tileSize
                            );
                            float rot = rng.Next(0, 4) * 90f;
                            SpawnPrefab(tileset.propPrefabs, pos, Quaternion.Euler(0, rot, 0), parent);
                        }
                    }
                }
            }
        }

        private void PopulateDecorations()
        {
            if (tileset.decorationPrefabs == null || tileset.decorationPrefabs.Length == 0) return;
            Transform parent = GetOrCreateChild("Props/Decorations");

            int[] dx = { 1, -1, 0, 0 };
            int[] dz = { 0, 0, 1, -1 };
            float[] rotY = { 0f, 180f, 90f, 270f };

            for (int x = 1; x < config.mapWidth - 1; x++)
            {
                for (int z = 1; z < config.mapHeight - 1; z++)
                {
                    if (!floorMap[x, z]) continue;
                    if ((float)rng.NextDouble() > config.decorationDensity) continue;

                    // Trang trí gắn tường – offset ra gần tường đúng tileSize
                    for (int d = 0; d < 4; d++)
                    {
                        int nx = x + dx[d], nz = z + dz[d];
                        if (nx < 0 || nx >= config.mapWidth || nz < 0 || nz >= config.mapHeight) continue;
                        if (!floorMap[nx, nz])
                        {
                            Vector3 pos = new Vector3(
                                x * config.tileSize + dx[d] * config.tileSize * 0.45f,
                                config.wallHeight * 0.3f,
                                z * config.tileSize + dz[d] * config.tileSize * 0.45f
                            );
                            SpawnPrefab(tileset.decorationPrefabs, pos, Quaternion.Euler(0, rotY[d] + 180f, 0), parent);
                            break;
                        }
                    }
                }
            }
        }

        private void PlaceLights()
        {
            Transform parent = GetOrCreateChild("Effects/Lights");

            // Đuốc tường
            if (tileset.torchPrefabs != null && tileset.torchPrefabs.Length > 0)
            {
                int[] dx = { 1, -1, 0, 0 };
                int[] dz = { 0, 0, 1, -1 };
                float[] rotY = { 0f, 180f, 90f, 270f };

                for (int x = 1; x < config.mapWidth - 1; x++)
                {
                    for (int z = 1; z < config.mapHeight - 1; z++)
                    {
                        if (!floorMap[x, z]) continue;
                        if ((float)rng.NextDouble() > config.torchDensity) continue;

                        for (int d = 0; d < 4; d++)
                        {
                            int nx = x + dx[d], nz = z + dz[d];
                            if (nx < 0 || nx >= config.mapWidth || nz < 0 || nz >= config.mapHeight) continue;
                            if (!floorMap[nx, nz])
                            {
                                Vector3 pos = new Vector3(
                                    x * config.tileSize + dx[d] * config.tileSize * 0.42f,
                                    config.wallHeight * 0.45f,
                                    z * config.tileSize + dz[d] * config.tileSize * 0.42f
                                );
                                SpawnPrefab(tileset.torchPrefabs, pos, Quaternion.Euler(0, rotY[d] + 180f, 0), parent);
                                break;
                            }
                        }
                    }
                }
            }

            // Đèn ambient trung tâm mỗi phòng
            if (tileset.ambientLightPrefabs != null && tileset.ambientLightPrefabs.Length > 0)
            {
                foreach (var room in rooms)
                {
                    Vector2Int c = RoomCenter(room);
                    SpawnPrefab(tileset.ambientLightPrefabs, T(c.x, c.y, config.wallHeight - 0.5f), Quaternion.identity, parent);
                }
            }
        }

        // ── Gameplay objects ──────────────────────────────────────────────

        private void PlacePlayerSpawn()
        {
            if (rooms.Count == 0) return;
            Transform parent = GetOrCreateChild("Gameplay");
            Vector2Int c = RoomCenter(rooms[0]);

            if (tileset.playerSpawnPointPrefab != null)
                SpawnSingle(tileset.playerSpawnPointPrefab, T(c.x, c.y, 0f), Quaternion.identity, parent);
            else
            {
                // Tạo empty marker
                GameObject marker = new GameObject("PlayerSpawnPoint");
                marker.transform.SetParent(parent);
                marker.transform.position = T(c.x, c.y, 0f);
            }
        }

        private void PlaceEnemySpawners()
        {
            if (rooms.Count <= 2) return;
            Transform parent = GetOrCreateChild("Spawners");

            for (int i = 1; i < rooms.Count - 1; i++)
            {
                var room = rooms[i];
                bool isElite = i == rooms.Count / 2; // phòng giữa = elite

                GameObject[] spawnerPool = (isElite && tileset.eliteSpawnerPrefabs != null && tileset.eliteSpawnerPrefabs.Length > 0)
                    ? tileset.eliteSpawnerPrefabs
                    : tileset.enemySpawnerPrefabs;

                if (spawnerPool == null || spawnerPool.Length == 0) continue;

                // 1-2 spawner mỗi phòng
                int count = rng.Next(1, 3);
                for (int s = 0; s < count; s++)
                {
                    int sx = rng.Next(room.x + 1, room.x + room.width - 1);
                    int sz = rng.Next(room.y + 1, room.y + room.height - 1);
                    SpawnPrefab(spawnerPool, T(sx, sz, 0f), Quaternion.identity, parent);
                }
            }
        }

        private void PlaceSiteOfGrace()
        {
            if (tileset.siteOfGracePrefab == null || rooms.Count == 0) return;
            Transform parent = GetOrCreateChild("Gameplay");
            Vector2Int c = RoomCenter(rooms[0]);
            // Đặt lệch 1.5 tile so với spawn
            Vector3 pos = T(c.x, c.y, 0f) + new Vector3(config.tileSize * 1.5f, 0f, config.tileSize * 1.5f);
            SpawnSingle(tileset.siteOfGracePrefab, pos, Quaternion.identity, parent);
        }

        private void PlaceBossRoom()
        {
            if (rooms.Count == 0) return;
            Transform parent = GetOrCreateChild("Gameplay/Boss");
            RectInt bossRoom = rooms[rooms.Count - 1];
            Vector2Int entry = RoomCenter(rooms.Count > 1 ? rooms[rooms.Count - 2] : rooms[0]);
            Vector2Int bossCenter = RoomCenter(bossRoom);

            // Fog wall ở lối vào phòng boss – world position dùng tileSize
            if (tileset.fogWallPrefab != null)
            {
                Vector3 fogPos = new Vector3(
                    (entry.x + bossCenter.x) * 0.5f * config.tileSize,
                    0f,
                    (entry.y + bossCenter.y) * 0.5f * config.tileSize
                );
                float fogAngle = Mathf.Atan2(bossCenter.x - entry.x, bossCenter.y - entry.y) * Mathf.Rad2Deg;
                SpawnSingle(tileset.fogWallPrefab, fogPos, Quaternion.Euler(0, fogAngle, 0), parent);
            }

            // Boss
            if (tileset.bossPrefab != null)
                SpawnSingle(tileset.bossPrefab, T(bossCenter.x, bossCenter.y, 0f), Quaternion.identity, parent);
        }

        // ── Zone splitting ────────────────────────────────────────────────

        private void BuildZones()
        {
            generatedZones.Clear();

            // World-space kích thước map thực = tile count × tileSize
            float worldW = config.mapWidth * config.tileSize;
            float worldH = config.mapHeight * config.tileSize;
            float zoneW = worldW / config.zoneGridX;
            float zoneH = worldH / config.zoneGridZ;

            // Lấy tất cả objects đã sinh ra
            List<Transform> allObjects = new List<Transform>();
            CollectAllChildren(generatedRoot, allObjects);

            for (int zx = 0; zx < config.zoneGridX; zx++)
            {
                for (int zz = 0; zz < config.zoneGridZ; zz++)
                {
                    float minX = zx * zoneW;
                    float minZ = zz * zoneH;
                    Bounds zoneBounds = new Bounds(
                        new Vector3(minX + zoneW * 0.5f, config.wallHeight * 0.5f, minZ + zoneH * 0.5f),
                        new Vector3(zoneW, config.wallHeight + 2f, zoneH)
                    );

                    string zoneName = $"{areaName}_Zone_{zx}_{zz}";
                    var zone = new GeneratedZoneInfo(zoneName, zoneBounds);

                    foreach (var t in allObjects)
                    {
                        if (t == null) continue;
                        if (zoneBounds.Contains(t.position))
                            zone.objects.Add(t.gameObject);
                    }

                    generatedZones.Add(zone);
                }
            }

            Debug.Log($"[RandomMapGenerator] Split into {generatedZones.Count} zones.");
        }

        private void CollectAllChildren(Transform root, List<Transform> result)
        {
            if (root == null) return;
            foreach (Transform child in root)
            {
                result.Add(child);
                CollectAllChildren(child, result);
            }
        }

        // ── Utility ───────────────────────────────────────────────────────

        private Transform GetOrCreateChild(string path)
        {
            string[] parts = path.Split('/');
            Transform current = generatedRoot;
            foreach (var part in parts)
            {
                Transform found = null;
                foreach (Transform c in current)
                    if (c.name == part) { found = c; break; }

                if (found == null)
                {
                    found = new GameObject(part).transform;
                    found.SetParent(current);
                    found.localPosition = Vector3.zero;
                }
                current = found;
            }
            return current;
        }

        private GameObject SpawnPrefab(GameObject[] pool, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (pool == null || pool.Length == 0) return null;
            GameObject prefab = pool[rng.Next(0, pool.Length)];
            if (prefab == null) return null;
            return SpawnSingle(prefab, position, rotation, parent);
        }

        private GameObject SpawnSingle(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (prefab == null) return null;
#if UNITY_EDITOR
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
#else
            return Instantiate(prefab, position, rotation, parent);
#endif
        }

        private float RandomOffset() => (float)(rng.NextDouble() * 0.4 - 0.2);

        /// <summary>
        /// Lấy Renderer.bounds trong world space của toàn bộ mesh trong GameObject (kể cả children).
        /// Không phụ thuộc vị trí pivot — dùng để xác định cạnh thực của floor prefab.
        /// </summary>
        private Bounds GetWorldBounds(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
            {
                // Không có renderer: fallback dùng tileSize để ước lượng bounds
                float half = config.tileSize * 0.5f;
                return new Bounds(
                    go.transform.position,
                    new Vector3(config.tileSize, 0.1f, config.tileSize)
                );
            }

            Bounds combined = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                combined.Encapsulate(renderers[i].bounds);

            return combined;
        }
    }
}
