using UnityEngine;
using UnityEditor;

namespace baodeag
{
    [CustomEditor(typeof(WorldLocationManager))]
    public class WorldLocationManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WorldLocationManager locationManager = target as WorldLocationManager;

            if (GUILayout.Button("Toggle Game Mode"))
                locationManager.ToggleGameMode();

            if (GUILayout.Button("Toggle Light Bake Mode"))
                locationManager.ToggleLightBakeMode();

            //this makes it so that when you fetch objects using the inspector buttons, the scene will flag as "dirty" so changes can be saved
            EditorUtility.SetDirty(locationManager);
        }
    }
}
