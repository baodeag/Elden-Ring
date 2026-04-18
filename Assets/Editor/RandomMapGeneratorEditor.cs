using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

namespace baodeag
{
    [CustomEditor(typeof(RandomMapGenerator))]
    public class RandomMapGeneratorEditor : UnityEditor.Editor
    {
        private const string WorldLocationRendererPrefabPath = "Assets/Prefabs/World Managers/World Location Renderer.prefab";
        private const int RoomPreloadPreviousRadius = 2;
        private const int RoomPreloadNextRadius = 4;
        private const int LoadAllRoomsWhenRoomCountAtMost = 6;
        private const float SceneTriggerPaddingXZ = 12f;
        private const float SceneTriggerPaddingY = 6f;

        // ── Foldout states ────────────────────────────────────────────────
        private bool foldTileset = true;
        private bool foldStructure = true;
        private bool foldProps = true;
        private bool foldLights = true;
        private bool foldGameplay = true;
        private bool foldConfig = true;
        private bool foldZones = false;
        private bool foldExport = false;

        // ── Colors ────────────────────────────────────────────────────────
        private static readonly Color colorGenerate = new Color(0.2f, 0.75f, 0.35f);
        private static readonly Color colorClear = new Color(0.85f, 0.3f, 0.25f);
        private static readonly Color colorExport = new Color(0.25f, 0.55f, 0.85f);
        private static readonly Color colorHeader = new Color(0.18f, 0.18f, 0.22f);
        private static readonly Color colorSection = new Color(0.22f, 0.22f, 0.28f);

        // ── Serialized properties ─────────────────────────────────────────
        private SerializedProperty propTileset;
        private SerializedProperty propConfig;
        private SerializedProperty propWorldSceneName;
        private SerializedProperty propAreaName;
        private SerializedProperty propGeneratedSiteOfGraceID;

        private void OnEnable()
        {
            propTileset = serializedObject.FindProperty("tileset");
            propConfig = serializedObject.FindProperty("config");
            propWorldSceneName = serializedObject.FindProperty("worldSceneName");
            propAreaName = serializedObject.FindProperty("areaName");
            propGeneratedSiteOfGraceID = serializedObject.FindProperty("generatedSiteOfGraceID");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            RandomMapGenerator gen = (RandomMapGenerator)target;

            // ── Header banner ─────────────────────────────────────────────
            DrawBanner();

            EditorGUILayout.Space(4);

            // ── Tileset ───────────────────────────────────────────────────
            foldTileset = DrawFoldout(foldTileset, "🧱  TILESET (Kéo Prefab Vào Đây)", colorSection);
            if (foldTileset)
            {
                EditorGUI.indentLevel++;

                foldStructure = DrawFoldout(foldStructure, "Cấu trúc (Structure)", new Color(0.3f, 0.3f, 0.4f));
                if (foldStructure)
                {
                    EditorGUI.indentLevel++;
                    DrawPrefabArray(propTileset.FindPropertyRelative("floorPrefabs"), "Floor Prefabs", "Sàn gạch/đá (1x1 tile, scale đúng 1:1)");
                    DrawPrefabArray(propTileset.FindPropertyRelative("wallPrefabs"), "Wall Prefabs", "Tường (lấp cạnh tile, chiều cao = wallHeight)");
                    DrawPrefabArray(propTileset.FindPropertyRelative("wallArchPrefabs"), "Wall Arch Prefabs", "Mái/vòm đặt trên đỉnh tường, local +Z quay về phía sàn");
                    DrawPrefabArray(propTileset.FindPropertyRelative("wallArchCornerPrefabs"), "Wall Arch Corner Prefabs", "Mái/vòm góc vuông đặt ở góc có 3 ô nền xung quanh");
                    DrawPrefabArray(propTileset.FindPropertyRelative("wallArchOuterCornerPrefabs"), "Wall Arch Outer Corner Prefabs", "Mái/vòm góc vuông đặt ở góc chỉ có 1 ô nền xung quanh");
                    DrawPrefabArray(propTileset.FindPropertyRelative("ceilingPrefabs"), "Ceiling Prefabs", "Trần phẳng đặt song song với nền ở Y nền + 10, chỉ trên phần ruột sau khi trừ biên nền");
                    DrawPrefabArray(propTileset.FindPropertyRelative("pillarPrefabs"), "Pillar Prefabs", "Cột góc phòng");
                    DrawPrefabArray(propTileset.FindPropertyRelative("doorwayPrefabs"), "Doorway Prefabs", "Cổng nối hành lang – phòng");
                    DrawPrefabArray(propTileset.FindPropertyRelative("stairPrefabs"), "Stair Prefabs", "Cầu thang (tuỳ chọn)");
                    EditorGUI.indentLevel--;
                }

                foldProps = DrawFoldout(foldProps, "Trang trí / Props", new Color(0.3f, 0.3f, 0.4f));
                if (foldProps)
                {
                    EditorGUI.indentLevel++;
                    DrawPrefabArray(propTileset.FindPropertyRelative("propPrefabs"), "Prop Prefabs", "Đồ vật (bàn, hòm, thùng, …)");
                    DrawPrefabArray(propTileset.FindPropertyRelative("decorationPrefabs"), "Decoration Prefabs", "Trang trí tường / góc");
                    DrawPrefabArray(propTileset.FindPropertyRelative("ruinPrefabs"), "Ruin Prefabs", "Mảnh vỡ, gạch đổ");
                    EditorGUI.indentLevel--;
                }

                foldLights = DrawFoldout(foldLights, "Ánh sáng (Lights & Effects)", new Color(0.3f, 0.3f, 0.4f));
                if (foldLights)
                {
                    EditorGUI.indentLevel++;
                    DrawPrefabArray(propTileset.FindPropertyRelative("torchPrefabs"), "Torch Prefabs", "Đuốc gắn tường");
                    DrawPrefabArray(propTileset.FindPropertyRelative("lanternPrefabs"), "Lantern Prefabs", "Đèn lồng treo");
                    DrawPrefabArray(propTileset.FindPropertyRelative("ambientLightPrefabs"), "Ambient Light Prefabs", "Point light ambient trung tâm phòng");
                    EditorGUI.indentLevel--;
                }

                foldGameplay = DrawFoldout(foldGameplay, "Gameplay Objects", new Color(0.3f, 0.3f, 0.4f));
                if (foldGameplay)
                {
                    EditorGUI.indentLevel++;
                    DrawPrefabArray(propTileset.FindPropertyRelative("enemySpawnerPrefabs"), "Enemy Spawner Prefabs", "AI Spawner thường");
                    DrawPrefabArray(propTileset.FindPropertyRelative("eliteSpawnerPrefabs"), "Elite Spawner Prefabs", "AI Spawner elite (phòng giữa)");
                    EditorGUILayout.PropertyField(propTileset.FindPropertyRelative("bossPrefab"), new GUIContent("Boss Prefab", "Boss đặt ở phòng cuối"));
                    EditorGUILayout.PropertyField(propTileset.FindPropertyRelative("siteOfGracePrefab"), new GUIContent("Site of Grace Prefab", "Checkpoint đặt ở phòng đầu"));
                    EditorGUILayout.PropertyField(propTileset.FindPropertyRelative("fogWallPrefab"), new GUIContent("Fog Wall Prefab", "Fog wall trước phòng boss"));
                    EditorGUILayout.PropertyField(propTileset.FindPropertyRelative("playerSpawnPointPrefab"), new GUIContent("Player Spawn Point Prefab", "Điểm spawn player (phòng đầu)"));
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(6);

            // ── Config ────────────────────────────────────────────────────
            foldConfig = DrawFoldout(foldConfig, "⚙️  CẤU HÌNH MAP", colorSection);
            if (foldConfig)
            {
                EditorGUI.indentLevel++;

                // ── Auto-detect info box ──
                DrawConfigSection("🤖 Auto Tile Size (Tự động)", () =>
                {
                    SerializedProperty wallHProp = propConfig.FindPropertyRelative("wallHeight");
                    SerializedProperty wallTProp = propConfig.FindPropertyRelative("wallThickness");

                    EditorGUILayout.HelpBox(
                        "Floor tile size được đo tự động từ Renderer.bounds khi bấm Generate.\n" +
                        "Không cần nhập tay — dùng đúng kích thước mesh thực.",
                        MessageType.Info);

                    // Chỉ để lại Wall Height và Wall Thickness (vẫn phải nhập đúng)
                    EditorGUILayout.PropertyField(wallHProp, new GUIContent("Wall Height (units)",
                        "Chiều cao thực của prefab tường (phải khớp mesh). VD: tường cao 5m → nhập 5."));
                    EditorGUILayout.PropertyField(wallTProp, new GUIContent("Wall Thickness (units)",
                        "Độ dày prefab tường. VD: 0.5"));
                });

                DrawConfigSection("Số ô (Tiles)", () =>
                {
                    SerializedProperty tileSizeProp = propConfig.FindPropertyRelative("tileSize");
                    SerializedProperty wProp = propConfig.FindPropertyRelative("mapWidth");
                    SerializedProperty hProp = propConfig.FindPropertyRelative("mapHeight");

                    EditorGUILayout.PropertyField(wProp, new GUIContent("Map Width  (tiles)", "Số tile theo X"));
                    EditorGUILayout.PropertyField(hProp, new GUIContent("Map Height (tiles)", "Số tile theo Z"));

                    float realW = wProp.intValue * tileSizeProp.floatValue;
                    float realH = hProp.intValue * tileSizeProp.floatValue;
                    EditorGUILayout.LabelField($"→ Kích thước thực: {realW:F1} × {realH:F1} units", EditorStyles.miniLabel);
                });

                DrawConfigSection("Phòng (Rooms)", () =>
                {
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("minRoomSize"), new GUIContent("Min Room Size (tiles)"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("maxRoomSize"), new GUIContent("Max Room Size (tiles)"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("maxRooms"), new GUIContent("Max Rooms"));
                });

                DrawConfigSection("Phân chia Zone (Additive Scene)", () =>
                {
                    SerializedProperty zx = propConfig.FindPropertyRelative("zoneGridX");
                    SerializedProperty zz = propConfig.FindPropertyRelative("zoneGridZ");
                    EditorGUILayout.PropertyField(zx, new GUIContent("Zone Grid X", "Chia map thành N cột zone"));
                    EditorGUILayout.PropertyField(zz, new GUIContent("Zone Grid Z", "Chia map thành N hàng zone"));

                    int zoneCount = zx.intValue * zz.intValue;
                    SerializedProperty wProp = propConfig.FindPropertyRelative("mapWidth");
                    SerializedProperty hProp = propConfig.FindPropertyRelative("mapHeight");
                    float ts = propConfig.FindPropertyRelative("tileSize").floatValue;
                    float zoneW = (float)wProp.intValue / zx.intValue;
                    float zoneH = (float)hProp.intValue / zz.intValue;

                    EditorGUILayout.HelpBox(
                        $"→ Tổng {zoneCount} zone, mỗi zone ≈ {zoneW:F0} × {zoneH:F0} tiles ({zoneW * ts:F1} × {zoneH * ts:F1} units)\n" +
                        $"→ Mỗi zone sẽ thành 4 sub-scene: _Structure, _Props, _Effects, _Spawners",
                        MessageType.Info);
                });

                DrawConfigSection("Mật độ nội thất", () =>
                {
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("propDensity"), new GUIContent("Prop Density", "Xác suất đặt prop mỗi tile bên trong phòng"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("decorationDensity"), new GUIContent("Decoration Density", "Xác suất đặt decoration tường"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("torchDensity"), new GUIContent("Torch Density", "Xác suất đặt đuốc tường"));
                });

                DrawConfigSection("Torch Lights", () =>
                {
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("torchWallSpacing"), new GUIContent("Torch Wall Spacing", "Place one torch every N wall tiles"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("torchLightRange"), new GUIContent("Torch Light Range", "Generated torch point light range"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("torchLightIntensity"), new GUIContent("Torch Light Intensity", "Generated torch point light intensity"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("torchLightColor"), new GUIContent("Torch Light Color", "Generated torch point light color"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("chandelierLightRange"), new GUIContent("Chandelier Light Range", "Generated chandelier point light range"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("chandelierLightIntensity"), new GUIContent("Chandelier Light Intensity", "Generated chandelier point light intensity"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("chandelierLightColor"), new GUIContent("Chandelier Light Color", "Generated chandelier point light color"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("useWorld01LightingMode"), new GUIContent("Use World_01 Lighting Mode", "Apply Assets/System/World Lighting Settings.lighting after generating"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("markGeneratedMapForBake"), new GUIContent("Mark Generated Map For Bake", "Set generated renderers to Contribute GI and generated lights to Mixed"));
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("autoBakeNavMeshAfterGenerate"), new GUIContent("Auto Bake NavMesh", "Bake NavMesh on generated Structure/Floors after generating"));
                });

                DrawConfigSection("Random Prefab Variants", () =>
                {
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("randomizePrefabVariants"),
                        new GUIContent("Randomize Prefab Variants", "Bật để mỗi floor/wall/wall arch/ceiling chọn ngẫu nhiên prefab trong array. Tắt để luôn dùng prefab đầu tiên."));
                });

                DrawConfigSection("Seed", () =>
                {
                    EditorGUILayout.PropertyField(propConfig.FindPropertyRelative("useRandomSeed"), new GUIContent("Use Random Seed"));
                    SerializedProperty seedProp = propConfig.FindPropertyRelative("seed");
                    if (!propConfig.FindPropertyRelative("useRandomSeed").boolValue)
                        EditorGUILayout.PropertyField(seedProp, new GUIContent("Seed"));
                    else
                        EditorGUILayout.LabelField("Seed", $"{seedProp.intValue}  (last used)", EditorStyles.miniLabel);
                });

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(6);

            // ── Output naming ─────────────────────────────────────────────
            DrawSectionHeader("📂  THÔNG TIN XUẤT RA");
            EditorGUILayout.PropertyField(propWorldSceneName, new GUIContent("World Scene Name", "Tên scene thế giới (World_02, World_03…)"));
            EditorGUILayout.PropertyField(propAreaName, new GUIContent("Area Name", "Tên khu vực (Area_02, Area_03…)"));

            EditorGUILayout.Space(4);
            DrawConfigSection("Site Of Grace", () =>
            {
                EditorGUILayout.PropertyField(propGeneratedSiteOfGraceID, new GUIContent("Generated Site Of Grace ID", "ID ghi vao SiteOfGraceInteractable khi generate"));
            });

            EditorGUILayout.Space(2);

            if (GUILayout.Button("Apply World_01 Lighting Mode", GUILayout.Height(28)))
            {
                gen.ApplyWorld01LightingMode();
                EditorUtility.SetDirty(gen.gameObject);
            }

            if (GUILayout.Button("Mark Generated Map For Bake", GUILayout.Height(28)))
            {
                gen.MarkGeneratedMapForBake();
                EditorUtility.SetDirty(gen.gameObject);
            }

            if (GUILayout.Button("Bake Generated NavMesh", GUILayout.Height(30)))
            {
                gen.BakeGeneratedNavMesh();
                EditorUtility.SetDirty(gen.gameObject);
            }

            if (GUILayout.Button("Bake Generated Lighting", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Bake lighting?",
                    "Apply World_01 lighting settings, mark generated objects for bake, then start Unity lighting bake.", "Bake", "Cancel"))
                {
                    gen.BakeGeneratedMapLighting();
                }
            }

            EditorGUILayout.Space(8);

            // ── Action buttons ────────────────────────────────────────────
            DrawSectionHeader("🎮  HÀNH ĐỘNG");
            EditorGUILayout.Space(4);

            // Generate button
            GUI.backgroundColor = colorGenerate;
            if (GUILayout.Button("▶  TẠO MAP RANDOM", GUILayout.Height(44)))
            {
                if (Application.isPlaying)
                {
                    gen.GenerateMap();
                }
                else
                {
                    Undo.RecordObject(gen.gameObject, "Generate Random Map");
                    gen.GenerateMap();
                    EditorUtility.SetDirty(gen.gameObject);
                    EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
                }
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space(2);

            // Clear button
            GUI.backgroundColor = colorClear;
            if (GUILayout.Button("🗑  XOÁ MAP", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Xoá Map?",
                    "Xoá toàn bộ object được sinh ra?\n(Không thể Undo nếu đã Save Scene)", "Xoá", "Huỷ"))
                {
                    Undo.RecordObject(gen.gameObject, "Clear Generated Map");
                    gen.ClearMap();
                    EditorUtility.SetDirty(gen.gameObject);
                    EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
                }
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space(8);

            // ── Zone / Export section ─────────────────────────────────────
            foldExport = DrawFoldout(foldExport, "📦  XUẤT RA SUB-SCENE (Additive)", colorSection);
            if (foldExport)
            {
                EditorGUILayout.HelpBox(
                    "Sau khi tạo map, bấm nút dưới để tự động:\n" +
                    "1. Tạo thư mục Scenes/[AreaName]/\n" +
                    "2. Với mỗi zone: tạo 4 scene con (_Structure, _Props, _Effects, _Spawners)\n" +
                    "3. Di chuyển object đúng loại vào đúng scene\n" +
                    "4. Lưu tất cả scene\n" +
                    "⚠️ Scene cần được thêm vào Build Settings thủ công (hoặc dùng script riêng)",
                    MessageType.Info);

                EditorGUILayout.Space(4);

                GUI.backgroundColor = colorExport;
                if (GUILayout.Button("🏗  XUẤT MAP THÀNH SUB-SCENES", GUILayout.Height(38)))
                {
                    if (gen.generatedZones == null || gen.generatedZones.Count == 0)
                    {
                        EditorUtility.DisplayDialog("Chưa có map",
                            "Hãy bấm 'Tạo Map Random' trước, sau đó mới xuất scene.", "OK");
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("Xuất sub-scene?",
                            $"Sẽ tạo {gen.generatedZones.Count * 4} sub-scene cho {gen.areaName}.\nThao tác này có thể mất vài phút.", "Xuất", "Huỷ"))
                        {
                            ExportZonesToScenes(gen);
                        }
                    }
                }
                GUI.backgroundColor = Color.white;

                // Zone preview
                if (gen.generatedZones != null && gen.generatedZones.Count > 0)
                {
                    EditorGUILayout.Space(4);
                    foldZones = DrawFoldout(foldZones, $"  Zone Preview ({gen.generatedZones.Count} zones)", new Color(0.25f, 0.25f, 0.32f));
                    if (foldZones)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var zone in gen.generatedZones)
                        {
                            EditorGUILayout.LabelField($"• {zone.zoneName}", $"{zone.objects.Count} objects", EditorStyles.miniLabel);
                        }
                        EditorGUI.indentLevel--;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        // ── Export Logic ──────────────────────────────────────────────────

        private void ExportZonesToScenes(RandomMapGenerator gen)
        {
            if (!PrepareEditorForSubSceneExport())
                return;

            string scenesRoot = "Assets/Scenes";
            string areaFolder = $"{scenesRoot}/{gen.areaName}";

            if (!AssetDatabase.IsValidFolder(areaFolder))
                AssetDatabase.CreateFolder(scenesRoot, gen.areaName);

            // Category mapping: tag hierarchy root name → sub-scene suffix
            var categoryMap = new Dictionary<string, string>
            {
                { "Structure", "_Structure" },
                { "Props",     "_Props"     },
                { "Effects",   "_Effects"   },
                { "Spawners",  "_Spawners"  },
                { "Gameplay",  "_Spawners"  }, // gameplay objects vào cùng spawners scene
            };

            int totalScenes = gen.generatedZones.Count * 4;
            int done = 0;
            int exportedRootCount = 0;
            List<string> exportedScenePaths = new List<string>();
            List<string> exportedSceneNames = new List<string>();

            try
            {
                Dictionary<string, Dictionary<string, List<GameObject>>> exportGroups = BuildExportGroupsFromGeneratedHierarchy(gen, categoryMap);

                foreach (var zone in gen.generatedZones)
                {
                    // Với mỗi zone, tạo 4 sub-scene
                    exportGroups.TryGetValue(zone.zoneName, out Dictionary<string, List<GameObject>> zoneGroups);

                    var subSceneGroups = new Dictionary<string, List<GameObject>>
                    {
                        { "_Structure", GetExportGroup(zoneGroups, "_Structure") },
                        { "_Props",     GetExportGroup(zoneGroups, "_Props")     },
                        { "_Effects",   GetExportGroup(zoneGroups, "_Effects")   },
                        { "_Spawners",  GetExportGroup(zoneGroups, "_Spawners")  },
                    };

                    foreach (var kvp in subSceneGroups)
                    {
                        string sceneName = $"{zone.zoneName}{kvp.Key}";
                        string scenePath = $"{areaFolder}/{sceneName}.unity";

                        // Tạo scene mới
                        if (!TryCreateAdditiveExportScene(sceneName, out Scene newScene))
                            return;

                        // Move objects vào scene mới
                        exportedRootCount += MoveObjectsToSceneWithFolders(kvp.Value, newScene);

                        // Lưu scene
                        EnsureWorldLocationRenderer(newScene, kvp.Key);
                        EditorSceneManager.SaveScene(newScene, scenePath);
                        EditorSceneManager.CloseScene(newScene, false);
                        exportedScenePaths.Add(scenePath);
                        exportedSceneNames.Add(sceneName);

                        done++;
                        float progress = (float)done / totalScenes;
                        EditorUtility.DisplayProgressBar(
                            "Xuất sub-scene…",
                            $"Tạo {sceneName} ({done}/{totalScenes})",
                            progress);
                    }
                }

                if (exportedRootCount < 10)
                {
                    Debug.LogError($"[RandomMapGeneratorEditor] Export stopped before cleanup because only {exportedRootCount} root object(s) were exported. Generate the map again and export after fixing the hierarchy grouping.");
                    EditorUtility.DisplayDialog(
                        "Export chua du map",
                        $"Chi xuat duoc {exportedRootCount} root object. Tool se khong xoa map trong scene chinh de tranh mat map. Hay Generate Map lai roi Export lai.",
                        "OK");
                    return;
                }

                SetupWorldLocationSceneSetsAndTriggers(gen);
                CleanupExportedMapFromMainScene(gen);
                DisableWorldAdditiveSceneBootstrap(gen);
                AddScenesToBuildSettings(exportedScenePaths);
                EditorSceneManager.SaveScene(gen.gameObject.scene);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog(
                    "Xuất thành công!",
                    $"Đã tạo {totalScenes} sub-scene trong:\n{areaFolder}\n\n" +
                    "⚠️ Nhớ thêm các scene vào Build Settings và cập nhật WorldSceneManager nếu cần.",
                    "OK");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[RandomMapGeneratorEditor] Export lỗi: {ex}");
                EditorUtility.DisplayDialog("Lỗi xuất scene", ex.Message, "OK");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
            }
        }

        private bool PrepareEditorForSubSceneExport()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "Khong the export",
                    "Hay dung Play Mode truoc khi xuat sub-scene.",
                    "OK");
                return false;
            }

            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                StageUtility.GoToMainStage();

            return true;
        }

        private Dictionary<string, Dictionary<string, List<GameObject>>> BuildExportGroupsFromGeneratedHierarchy(
            RandomMapGenerator gen,
            Dictionary<string, string> categoryMap)
        {
            Dictionary<string, Dictionary<string, List<GameObject>>> groups = new Dictionary<string, Dictionary<string, List<GameObject>>>();
            Transform generatedRoot = GetGeneratedRoot(gen);

            if (generatedRoot == null)
                return groups;

            List<Transform> allChildren = new List<Transform>();
            CollectChildren(generatedRoot, allChildren);
            HashSet<GameObject> exportRoots = new HashSet<GameObject>();

            for (int i = 0; i < allChildren.Count; i++)
            {
                GameObject exportRoot = GetExportRootForSubScene(allChildren[i].gameObject);

                if (exportRoot == null || !exportRoots.Add(exportRoot))
                    continue;

                string category = GetCategoryFromHierarchy(exportRoot);
                string suffix = categoryMap.TryGetValue(category, out string mapped) ? mapped : "_Props";
                string zoneName = GetZoneNameForObject(gen, exportRoot);

                if (string.IsNullOrEmpty(zoneName))
                    continue;

                if (!groups.TryGetValue(zoneName, out Dictionary<string, List<GameObject>> zoneGroups))
                {
                    zoneGroups = new Dictionary<string, List<GameObject>>();
                    groups[zoneName] = zoneGroups;
                }

                if (!zoneGroups.TryGetValue(suffix, out List<GameObject> objects))
                {
                    objects = new List<GameObject>();
                    zoneGroups[suffix] = objects;
                }

                objects.Add(exportRoot);
            }

            return groups;
        }

        private List<GameObject> GetExportGroup(Dictionary<string, List<GameObject>> zoneGroups, string suffix)
        {
            if (zoneGroups != null && zoneGroups.TryGetValue(suffix, out List<GameObject> objects))
                return objects;

            return new List<GameObject>();
        }

        private int MoveObjectsToSceneWithFolders(List<GameObject> objects, Scene scene)
        {
            if (objects == null || !scene.IsValid())
                return 0;

            Dictionary<string, Transform> folderRoots = new Dictionary<string, Transform>();
            int movedCount = 0;

            for (int i = 0; i < objects.Count; i++)
            {
                GameObject go = objects[i];

                if (go == null)
                    continue;

                string folderName = GetExportFolderName(go);
                Transform folder = GetOrCreateExportFolder(folderRoots, scene, folderName);

                go.transform.SetParent(null, true);
                SceneManager.MoveGameObjectToScene(go, scene);

                if (folder != null)
                    go.transform.SetParent(folder, true);

                movedCount++;
            }

            return movedCount;
        }

        private Transform GetOrCreateExportFolder(Dictionary<string, Transform> folderRoots, Scene scene, string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                folderName = "Misc";

            if (folderRoots.TryGetValue(folderName, out Transform folder) && folder != null)
                return folder;

            GameObject folderObject = new GameObject(folderName);
            SceneManager.MoveGameObjectToScene(folderObject, scene);
            folderObject.transform.SetParent(null, true);
            folderRoots[folderName] = folderObject.transform;
            return folderObject.transform;
        }

        private string GetExportFolderName(GameObject go)
        {
            Transform t = go != null ? go.transform : null;

            while (t != null)
            {
                string name = t.name;

                if (name == "Floors")
                    return "Floors";
                if (name == "Walls")
                    return "Walls";
                if (name == "WallArches" || name == "Ceilings" || name == "Roofs")
                    return "Roofs";
                if (name == "Pillars")
                    return "Pillars";
                if (name == "Doorways" || name == "Doors")
                    return "Doors";
                if (name == "Stairs")
                    return "Stairs";
                if (name == "Props")
                    return "Props";
                if (name == "Decorations")
                    return "Decorations";
                if (name == "Effects" || name == "Lights")
                    return "Lights";
                if (name == "Spawners")
                    return "Spawners";
                if (name == "Gameplay")
                    return "Gameplay";
                if (name == "Boss")
                    return "Boss";

                t = t.parent;
            }

            return "Misc";
        }

        private Transform GetGeneratedRoot(RandomMapGenerator gen)
        {
            if (gen == null)
                return null;

            foreach (Transform child in gen.transform)
            {
                if (child != null && child.name.StartsWith("[Generated]", System.StringComparison.OrdinalIgnoreCase))
                    return child;
            }

            return null;
        }

        private void CollectChildren(Transform root, List<Transform> children)
        {
            if (root == null)
                return;

            foreach (Transform child in root)
            {
                children.Add(child);
                CollectChildren(child, children);
            }
        }

        private string GetZoneNameForObject(RandomMapGenerator gen, GameObject go)
        {
            if (gen == null || go == null || gen.generatedZones == null)
                return string.Empty;

            Bounds objectBounds = GetObjectBounds(go);
            string centerZoneName = GetZoneNameContainingPosition(gen, objectBounds.center);

            if (!string.IsNullOrEmpty(centerZoneName))
                return centerZoneName;

            GeneratedZoneInfo bestRoomZone = null;
            float bestRoomOverlapArea = 0f;

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null)
                    continue;

                float roomOverlapArea = zone.GetRoomOverlapAreaXZ(objectBounds);

                if (roomOverlapArea > bestRoomOverlapArea)
                {
                    bestRoomOverlapArea = roomOverlapArea;
                    bestRoomZone = zone;
                }
            }

            if (bestRoomZone != null)
                return bestRoomZone.zoneName;

            GeneratedZoneInfo bestCoverageZone = null;
            float bestCoverageOverlapArea = 0f;

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null)
                    continue;

                float coverageOverlapArea = zone.GetMaxOverlapAreaXZ(objectBounds);

                if (coverageOverlapArea > bestCoverageOverlapArea)
                {
                    bestCoverageOverlapArea = coverageOverlapArea;
                    bestCoverageZone = zone;
                }
            }

            if (bestCoverageZone != null)
                return bestCoverageZone.zoneName;

            Vector3 position = objectBounds.center;
            GeneratedZoneInfo nearestZone = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null)
                    continue;

                float distance = zone.SqrDistanceTo(position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestZone = zone;
                }
            }

            if (nearestZone != null)
                return nearestZone.zoneName;

            return string.Empty;
        }

        private string GetZoneNameContainingPosition(RandomMapGenerator gen, Vector3 position)
        {
            GeneratedZoneInfo containingZone = null;
            float containingDistance = float.MaxValue;

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null || !zone.ContainsPosition(position))
                    continue;

                float distance = zone.SqrDistanceToCoverageCenter(position);

                if (distance < containingDistance)
                {
                    containingDistance = distance;
                    containingZone = zone;
                }
            }

            return containingZone != null ? containingZone.zoneName : string.Empty;
        }

        private Bounds GetObjectBounds(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
                return new Bounds(go.transform.position, Vector3.one);

            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                    bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private Vector3 GetObjectCenter(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
                return go.transform.position;

            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                    bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds.center;
        }

        private void CleanupExportedMapFromMainScene(RandomMapGenerator gen)
        {
            if (gen == null)
                return;

            List<GameObject> generatedRoots = new List<GameObject>();

            foreach (Transform child in gen.transform)
            {
                if (child != null && child.name.StartsWith("[Generated]", System.StringComparison.OrdinalIgnoreCase))
                    generatedRoots.Add(child.gameObject);
            }

            for (int i = 0; i < generatedRoots.Count; i++)
            {
                if (generatedRoots[i] != null)
                    DestroyImmediate(generatedRoots[i]);
            }

            EditorUtility.SetDirty(gen);
            EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
        }

        private void UpdateWorldAdditiveSceneBootstrap(RandomMapGenerator gen, List<string> sceneNames)
        {
            if (gen == null)
                return;

            System.Type bootstrapType = System.Type.GetType("baodeag.WorldAdditiveSceneBootstrap, Assembly-CSharp");

            if (bootstrapType == null)
            {
                Debug.LogWarning("[RandomMapGeneratorEditor] WorldAdditiveSceneBootstrap is not compiled yet. Let Unity refresh scripts, then export again to write the additive scene list.");
                return;
            }

            Component bootstrap = gen.GetComponent(bootstrapType);

            if (bootstrap == null)
                bootstrap = gen.gameObject.AddComponent(bootstrapType);

            System.Reflection.MethodInfo setter = bootstrapType.GetMethod("SetAdditiveScenes");
            setter?.Invoke(bootstrap, new object[] { sceneNames });

            EditorUtility.SetDirty(bootstrap);
            EditorUtility.SetDirty(gen.gameObject);
            EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
        }

        private void DisableWorldAdditiveSceneBootstrap(RandomMapGenerator gen)
        {
            if (gen == null)
                return;

            WorldAdditiveSceneBootstrap bootstrap = gen.GetComponent<WorldAdditiveSceneBootstrap>();

            if (bootstrap == null)
                return;

            SerializedObject serializedBootstrap = new SerializedObject(bootstrap);
            SerializedProperty additiveScenesProperty = serializedBootstrap.FindProperty("additiveScenesToLoad");
            SerializedProperty loadOnStartProperty = serializedBootstrap.FindProperty("loadOnStart");

            if (additiveScenesProperty != null)
                additiveScenesProperty.arraySize = 0;

            if (loadOnStartProperty != null)
                loadOnStartProperty.boolValue = false;

            serializedBootstrap.ApplyModifiedProperties();
            EditorUtility.SetDirty(bootstrap);
            EditorUtility.SetDirty(gen.gameObject);
            EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
        }

        private void AddScenesToBuildSettings(List<string> scenePaths)
        {
            if (scenePaths == null || scenePaths.Count == 0)
                return;

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            for (int i = 0; i < scenePaths.Count; i++)
            {
                string path = scenePaths[i];

                if (string.IsNullOrWhiteSpace(path))
                    continue;

                int existingIndex = scenes.FindIndex(scene => scene.path == path);

                if (existingIndex >= 0)
                {
                    scenes[existingIndex] = new EditorBuildSettingsScene(path, true);
                    continue;
                }

                scenes.Add(new EditorBuildSettingsScene(path, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private void SetupWorldLocationSceneSetsAndTriggers(RandomMapGenerator gen)
        {
            if (gen == null || gen.generatedZones == null || gen.generatedZones.Count == 0)
                return;

            Dictionary<string, WorldLocationSceneSet> sceneSetsByZone = CreateWorldLocationSceneSets(gen);
            AssignRequiredNeighborLocations(gen, sceneSetsByZone);
            CreateWorldLocationTriggers(gen, sceneSetsByZone);
        }

        private Dictionary<string, WorldLocationSceneSet> CreateWorldLocationSceneSets(RandomMapGenerator gen)
        {
            const string dataRoot = "Assets/Data";
            const string locationFolder = "Assets/Data/World Locations";
            Dictionary<string, WorldLocationSceneSet> sceneSetsByZone = new Dictionary<string, WorldLocationSceneSet>();

            if (!AssetDatabase.IsValidFolder(dataRoot))
                AssetDatabase.CreateFolder("Assets", "Data");

            if (!AssetDatabase.IsValidFolder(locationFolder))
                AssetDatabase.CreateFolder(dataRoot, "World Locations");

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null || string.IsNullOrWhiteSpace(zone.zoneName))
                    continue;

                string assetPath = $"{locationFolder}/{zone.zoneName}.asset";
                WorldLocationSceneSet sceneSet = AssetDatabase.LoadAssetAtPath<WorldLocationSceneSet>(assetPath);

                if (sceneSet == null)
                {
                    sceneSet = ScriptableObject.CreateInstance<WorldLocationSceneSet>();
                    AssetDatabase.CreateAsset(sceneSet, assetPath);
                }

                sceneSet.scenesRequiredForThisLocation.Clear();
                sceneSet.scenesRequiredForThisLocation.Add($"{zone.zoneName}_Structure");
                sceneSet.scenesRequiredForThisLocation.Add($"{zone.zoneName}_Props");
                sceneSet.scenesRequiredForThisLocation.Add($"{zone.zoneName}_Effects");
                sceneSet.scenesRequiredForThisLocation.Add($"{zone.zoneName}_Spawners");
                EditorUtility.SetDirty(sceneSet);
                sceneSetsByZone[zone.zoneName] = sceneSet;
            }

            AssetDatabase.SaveAssets();
            return sceneSetsByZone;
        }

        private void AssignRequiredNeighborLocations(RandomMapGenerator gen, Dictionary<string, WorldLocationSceneSet> sceneSetsByZone)
        {
            if (sceneSetsByZone == null || sceneSetsByZone.Count == 0)
                return;

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null || !sceneSetsByZone.TryGetValue(zone.zoneName, out WorldLocationSceneSet sceneSet))
                    continue;

                List<WorldLocationSceneSet> requiredLocations = new List<WorldLocationSceneSet>();

                int firstPreloadIndex = gen.generatedZones.Count <= LoadAllRoomsWhenRoomCountAtMost
                    ? 0
                    : Mathf.Max(0, i - RoomPreloadPreviousRadius);
                int lastPreloadIndex = gen.generatedZones.Count <= LoadAllRoomsWhenRoomCountAtMost
                    ? gen.generatedZones.Count - 1
                    : Mathf.Min(gen.generatedZones.Count - 1, i + RoomPreloadNextRadius);

                for (int neighborIndex = firstPreloadIndex;
                     neighborIndex <= lastPreloadIndex;
                     neighborIndex++)
                {
                    if (neighborIndex == i)
                        continue;

                    GeneratedZoneInfo neighborZone = gen.generatedZones[neighborIndex];

                    if (neighborZone != null &&
                        sceneSetsByZone.TryGetValue(neighborZone.zoneName, out WorldLocationSceneSet neighborSceneSet) &&
                        !requiredLocations.Contains(neighborSceneSet))
                    {
                        requiredLocations.Add(neighborSceneSet);
                    }
                }

                SerializedObject serializedSceneSet = new SerializedObject(sceneSet);
                SerializedProperty requiredLocationsProperty = serializedSceneSet.FindProperty("requiredLocations");
                requiredLocationsProperty.arraySize = requiredLocations.Count;

                for (int j = 0; j < requiredLocations.Count; j++)
                {
                    requiredLocationsProperty.GetArrayElementAtIndex(j).objectReferenceValue = requiredLocations[j];
                }

                serializedSceneSet.ApplyModifiedProperties();
                EditorUtility.SetDirty(sceneSet);
            }

            AssetDatabase.SaveAssets();
        }

        private void CreateWorldLocationTriggers(RandomMapGenerator gen, Dictionary<string, WorldLocationSceneSet> sceneSetsByZone)
        {
            if (sceneSetsByZone == null || sceneSetsByZone.Count == 0)
                return;

            Transform triggerRoot = GetOrCreateSceneRoot(gen.gameObject.scene, "_SCENE_TRIGGERS").transform;
            ClearAreaSceneTriggers(triggerRoot, gen.areaName);

            for (int i = 0; i < gen.generatedZones.Count; i++)
            {
                GeneratedZoneInfo zone = gen.generatedZones[i];

                if (zone == null || !sceneSetsByZone.TryGetValue(zone.zoneName, out WorldLocationSceneSet sceneSet))
                    continue;

                Bounds triggerBounds = GetZoneTriggerBounds(zone);
                GameObject trigger = new GameObject($"Scene_Trigger_{zone.zoneName}");
                SceneManager.MoveGameObjectToScene(trigger, gen.gameObject.scene);
                trigger.transform.SetParent(triggerRoot, true);
                trigger.transform.position = triggerBounds.center;
                trigger.transform.rotation = Quaternion.identity;
                trigger.transform.localScale = Vector3.one;
                trigger.layer = 11;

                BoxCollider collider = trigger.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                collider.size = triggerBounds.size;
                collider.center = Vector3.zero;

                EventTriggerLoadScene loadTrigger = trigger.AddComponent<EventTriggerLoadScene>();
                SerializedObject serializedTrigger = new SerializedObject(loadTrigger);
                serializedTrigger.FindProperty("area").objectReferenceValue = sceneSet;
                serializedTrigger.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(loadTrigger);
                EditorUtility.SetDirty(trigger);
            }

            EditorSceneManager.MarkSceneDirty(gen.gameObject.scene);
        }

        private GameObject GetOrCreateSceneRoot(Scene scene, string rootName)
        {
            GameObject[] roots = scene.GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i] != null && roots[i].name == rootName)
                    return roots[i];
            }

            GameObject root = new GameObject(rootName);
            SceneManager.MoveGameObjectToScene(root, scene);
            return root;
        }

        private void ClearAreaSceneTriggers(Transform triggerRoot, string areaName)
        {
            if (triggerRoot == null)
                return;

            List<GameObject> triggersToDelete = new List<GameObject>();

            foreach (Transform child in triggerRoot)
            {
                if (child != null && child.name.StartsWith($"Scene_Trigger_{areaName}_", System.StringComparison.OrdinalIgnoreCase))
                    triggersToDelete.Add(child.gameObject);
            }

            for (int i = 0; i < triggersToDelete.Count; i++)
            {
                if (triggersToDelete[i] != null)
                    DestroyImmediate(triggersToDelete[i]);
            }
        }

        private Bounds GetZoneTriggerBounds(GeneratedZoneInfo zone)
        {
            Bounds triggerBounds = zone.zoneBounds;

            if (zone.zoneVolumeObject != null)
            {
                BoxCollider collider = zone.zoneVolumeObject.GetComponent<BoxCollider>();

                if (collider != null)
                    triggerBounds = collider.bounds;
                else
                    triggerBounds = GetObjectBounds(zone.zoneVolumeObject);
            }

            if (zone.coverageBounds != null)
            {
                for (int i = 0; i < zone.coverageBounds.Count; i++)
                {
                    triggerBounds.Encapsulate(zone.coverageBounds[i]);
                }
            }

            triggerBounds.Expand(new Vector3(SceneTriggerPaddingXZ, SceneTriggerPaddingY, SceneTriggerPaddingXZ));
            return triggerBounds;
        }

        private bool TryCreateAdditiveExportScene(string sceneName, out Scene newScene)
        {
            try
            {
                newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                newScene.name = sceneName;
                return true;
            }
            catch (System.InvalidOperationException ex)
            {
                newScene = default;
                Debug.LogError($"[RandomMapGeneratorEditor] Cannot create additive export scene '{sceneName}': {ex}");
                EditorUtility.DisplayDialog(
                    "Khong the tao sub-scene",
                    "Unity dang o Prefab Mode/Preview Stage hoac mot editor mode khong cho tao additive scene. Hay thoat Prefab Mode, chon object Random Map Generator trong scene World_02, roi export lai.",
                    "OK");
                return false;
            }
        }

        /// <summary>Xác định category dựa vào tên cha gần nhất trong hierarchy.</summary>
        [MenuItem("Tools/Random Map Generator/Normalize Area_02 Sub Scenes")]
        public static void NormalizeArea02SubScenes()
        {
            NormalizeSubScenesInFolder("Assets/Scenes/Area_02");
        }

        private static void NormalizeSubScenesInFolder(string folderPath)
        {
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($"[RandomMapGeneratorEditor] Folder not found: {folderPath}");
                return;
            }

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { folderPath });
            int normalizedCount = 0;

            for (int i = 0; i < sceneGuids.Length; i++)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);

                if (string.IsNullOrWhiteSpace(scenePath))
                    continue;

                string suffix = GetSceneCategorySuffix(scenePath);

                if (!ShouldCreateWorldLocationRenderer(suffix))
                    continue;

                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                EnsureWorldLocationRenderer(scene, suffix);
                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);
                normalizedCount++;
            }

            AssetDatabase.Refresh();
            Debug.Log($"[RandomMapGeneratorEditor] Normalized {normalizedCount} Area_02 sub-scene(s) with World Location Renderer.");
        }

        private static string GetSceneCategorySuffix(string scenePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(scenePath);

            if (fileName.EndsWith("_Structure", System.StringComparison.OrdinalIgnoreCase))
                return "_Structure";
            if (fileName.EndsWith("_Props", System.StringComparison.OrdinalIgnoreCase))
                return "_Props";
            if (fileName.EndsWith("_Effects", System.StringComparison.OrdinalIgnoreCase))
                return "_Effects";
            if (fileName.EndsWith("_Spawners", System.StringComparison.OrdinalIgnoreCase))
                return "_Spawners";

            return string.Empty;
        }

        private static void EnsureWorldLocationRenderer(Scene scene, string categorySuffix)
        {
            if (!scene.IsValid() || !ShouldCreateWorldLocationRenderer(categorySuffix))
                return;

            WorldLocationRendererManager rendererManager = null;
            GameObject rendererObject = null;
            GameObject[] roots = scene.GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i] == null)
                    continue;

                WorldLocationRendererManager existing = roots[i].GetComponent<WorldLocationRendererManager>();

                if (existing == null)
                    continue;

                rendererManager = existing;
                rendererObject = roots[i];
                break;
            }

            if (rendererManager == null)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(WorldLocationRendererPrefabPath);

                if (prefab != null)
                {
                    rendererObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                }
                else
                {
                    rendererObject = new GameObject("World Location Renderer");
                    rendererObject.AddComponent<WorldLocationRendererManager>();
                }

                if (rendererObject == null)
                    return;

                rendererObject.name = "World Location Renderer";
                SceneManager.MoveGameObjectToScene(rendererObject, scene);
                rendererManager = rendererObject.GetComponent<WorldLocationRendererManager>();
            }

            if (rendererManager == null)
                return;

            rendererObject.transform.SetParent(null, true);
            rendererObject.transform.SetAsFirstSibling();
            rendererManager.FindAllRootObjects();
            rendererManager.FindAllMeshRenderers();
            EditorUtility.SetDirty(rendererManager);
            EditorUtility.SetDirty(rendererObject);
            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static bool ShouldCreateWorldLocationRenderer(string categorySuffix)
        {
            return categorySuffix == "_Structure" ||
                   categorySuffix == "_Props" ||
                   categorySuffix == "_Effects";
        }

        private string GetCategoryFromHierarchy(GameObject go)
        {
            Transform t = go.transform.parent;
            while (t != null)
            {
                string name = t.name;
                if (name == "Structure" || name == "Floors" || name == "Walls" || name == "WallArches" || name == "Ceilings" || name == "Roofs" || name == "Pillars" || name == "Doorways" || name == "Doors" || name == "Stairs")
                    return "Structure";
                if (name == "Props" || name == "Decorations")
                    return "Props";
                if (name == "Effects" || name == "Lights")
                    return "Effects";
                if (name == "Spawners" || name == "Gameplay" || name == "Boss")
                    return "Spawners";
                t = t.parent;
            }
            return "Props";
        }

        // ── UI Helpers ────────────────────────────────────────────────────

        private GameObject GetExportRootForSubScene(GameObject go)
        {
            if (go == null)
                return null;

            Transform exportRoot = go.transform;

            if (IsSceneVolumeNode(exportRoot))
                return null;

            if (IsGeneratedGroupingNode(exportRoot.name))
                return null;

            while (exportRoot.parent != null &&
                   !IsGeneratedGroupingNode(exportRoot.parent.name) &&
                   !exportRoot.parent.name.StartsWith("[Generated]", System.StringComparison.OrdinalIgnoreCase))
            {
                exportRoot = exportRoot.parent;
            }

            if (IsGeneratedGroupingNode(exportRoot.name))
                return null;

            return exportRoot.gameObject;
        }

        private bool IsSceneVolumeNode(Transform t)
        {
            while (t != null)
            {
                if (t.name == "SceneVolumes" ||
                    t.name.EndsWith("_SceneVolume", System.StringComparison.OrdinalIgnoreCase) ||
                    t.name.EndsWith("_CorridorVolume", System.StringComparison.OrdinalIgnoreCase))
                    return true;

                t = t.parent;
            }

            return false;
        }

        private bool IsGeneratedGroupingNode(string name)
        {
            return name == "Structure" ||
                   name == "Floors" ||
                   name == "Walls" ||
                   name == "WallArches" ||
                   name == "Ceilings" ||
                   name == "Roofs" ||
                   name == "Pillars" ||
                   name == "Doorways" ||
                   name == "Doors" ||
                   name == "Stairs" ||
                   name == "Props" ||
                   name == "Decorations" ||
                   name == "Effects" ||
                   name == "Lights" ||
                   name == "Spawners" ||
                   name == "Gameplay" ||
                   name == "Boss" ||
                   name == "SceneVolumes";
        }

        private void DrawBanner()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 52, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, colorHeader);

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.9f, 0.85f, 0.5f) }
            };

            GUIStyle subStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.6f, 0.6f, 0.7f) }
            };

            GUI.Label(new Rect(rect.x, rect.y + 6, rect.width, 24), "⚔  RANDOM MAP GENERATOR", titleStyle);
            GUI.Label(new Rect(rect.x, rect.y + 28, rect.width, 18), "Tạo dungeon ngẫu nhiên – tự phân chia scene", subStyle);
        }

        private bool DrawFoldout(bool state, string label, Color bgColor)
        {
            Rect rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, bgColor);
            GUIStyle style = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white },
                onNormal = { textColor = Color.white },
            };
            return EditorGUI.Foldout(new Rect(rect.x + 4, rect.y + 2, rect.width - 8, rect.height), state, label, true, style);
        }

        private void DrawSectionHeader(string label)
        {
            Rect rect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, colorHeader);
            GUIStyle s = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = new Color(0.8f, 0.75f, 0.4f) },
                padding = new RectOffset(6, 0, 2, 0)
            };
            GUI.Label(rect, label, s);
        }

        private void DrawConfigSection(string label, System.Action drawContent)
        {
            EditorGUILayout.Space(2);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = new Color(0.75f, 0.85f, 1f) },
                fontSize = 11
            };
            EditorGUILayout.LabelField(label, headerStyle);
            EditorGUI.indentLevel++;
            drawContent();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(2);
        }

        private void DrawPrefabArray(SerializedProperty arrayProp, string label, string tooltip)
        {
            EditorGUILayout.PropertyField(arrayProp, new GUIContent(label, tooltip), true);

            if (arrayProp.arraySize == 0)
            {
                EditorGUILayout.HelpBox($"⚠ {label}: Chưa có prefab nào – sẽ bỏ qua loại này khi generate.", MessageType.Warning);
            }
        }
    }
}
