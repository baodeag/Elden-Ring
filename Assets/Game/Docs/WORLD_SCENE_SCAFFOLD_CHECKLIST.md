# World Scene Scaffold Checklist

Generated scaffold scenes:
- `Assets/Scenes/World_02.unity`
- `Assets/Scenes/World_03.unity`
- `Assets/Scenes/World_04.unity`
- `Assets/Scenes/World_05.unity`

These are currently cloned from `World_01` as playable foundation scenes.

Current intended build index mapping:
- Map 1 -> `World_01` -> build index `1`
- Map 2 -> `World_02` -> build index `2`
- Map 3 -> `World_03` -> build index `3`
- Map 4 -> `World_04` -> build index `4`
- Map 5 -> `World_05` -> build index `5`

Next manual pass in Unity for each cloned scene:
- Rename or reposition the entry `Site Of Grace` if needed.
- Set unique `siteOfGraceID` values if you want different entry points per map.
- Replace duplicated boss/spawner layout with map-specific content.
- Rebuild lighting/navmesh/occlusion if you want each scene to be production quality.
- Verify the correct boss in each scene matches `bossID` in `Game Progression Config`.

Right now this scaffold is meant to make progression and scene transition testable end-to-end, not to be final level design.
