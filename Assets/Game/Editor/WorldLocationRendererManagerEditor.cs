using UnityEditor;
using UnityEngine;

namespace baodeag
{
    [CustomEditor(typeof(WorldLocationRendererManager))]
    public class WorldLocationRendererManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WorldLocationRendererManager rendererManager = target as WorldLocationRendererManager;

            if (GUILayout.Button("Enable All Renderers"))
            {
                rendererManager.FindAllMeshRenderers();
                rendererManager.ToggleMeshRenderers(true);
            }

            if (GUILayout.Button("Disable All Renderers"))
            {
                rendererManager.FindAllMeshRenderers();
                rendererManager.ToggleMeshRenderers(false);
            }

            if (GUILayout.Button("Enable All GameObjects"))
            {
                rendererManager.FindAllRootObjects();
                rendererManager.ToggleRootObjects(true);
            }

            if (GUILayout.Button("Disable All GameObjects"))
            {
                rendererManager.FindAllRootObjects();
                rendererManager.ToggleRootObjects(false);
            }

            //this makes it so that when you fetch objects using the inspector buttons, the scene will flag as "dirty" so changes can be saved
            EditorUtility.SetDirty(rendererManager);
        }
    }
}
