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

        private void OnEnable()
        {
            propTileset = serializedObject.FindProperty("tileset");
            propConfig = serializedObject.FindProperty("config");
            propWorldSceneName = serializedObject.FindProperty("worldSceneName");
            propAreaName = serializedObject.FindProperty("areaName");
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

            try
            {
                foreach (var zone in gen.generatedZones)
                {
                    // Với mỗi zone, tạo 4 sub-scene
                    var subSceneGroups = new Dictionary<string, List<GameObject>>
                    {
                        { "_Structure", new List<GameObject>() },
                        { "_Props",     new List<GameObject>() },
                        { "_Effects",   new List<GameObject>() },
                        { "_Spawners",  new List<GameObject>() },
                    };

                    foreach (var go in zone.objects)
                    {
                        if (go == null) continue;
                        string category = GetCategoryFromHierarchy(go);
                        string suffix = "_Props"; // default

                        if (categoryMap.TryGetValue(category, out string mapped))
                            suffix = mapped;

                        subSceneGroups[suffix].Add(go);
                    }

                    foreach (var kvp in subSceneGroups)
                    {
                        string sceneName = $"{zone.zoneName}{kvp.Key}";
                        string scenePath = $"{areaFolder}/{sceneName}.unity";

                        // Tạo scene mới
                        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                        newScene.name = sceneName;

                        // Move objects vào scene mới
                        foreach (var go in kvp.Value)
                        {
                            if (go == null) continue;
                            SceneManager.MoveGameObjectToScene(go, newScene);
                        }

                        // Lưu scene
                        EditorSceneManager.SaveScene(newScene, scenePath);
                        EditorSceneManager.CloseScene(newScene, false);

                        done++;
                        float progress = (float)done / totalScenes;
                        EditorUtility.DisplayProgressBar(
                            "Xuất sub-scene…",
                            $"Tạo {sceneName} ({done}/{totalScenes})",
                            progress);
                    }
                }

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

        /// <summary>Xác định category dựa vào tên cha gần nhất trong hierarchy.</summary>
        private string GetCategoryFromHierarchy(GameObject go)
        {
            Transform t = go.transform.parent;
            while (t != null)
            {
                string name = t.name;
                if (name == "Structure" || name == "Floors" || name == "Walls" || name == "Ceilings" || name == "Pillars" || name == "Doorways")
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
