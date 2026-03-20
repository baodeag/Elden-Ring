using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

namespace baodeag
{
    public class WorldLocationRendererManager : MonoBehaviour
    {
        [Header("Scene I.D")]
        [HideInInspector] public int renderSceneID;

        [Header("Root GameObjects")]
        [SerializeField] public List<GameObject> rootGameObjects = new List<GameObject>();

        [Header("Mesh Renderers")]
        [SerializeField] public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        private Coroutine toggleAllMeshRenderersCoroutine;

        private void Awake()
        {
            //get the scene I.D of the scene this game object is placed in
            renderSceneID = gameObject.scene.buildIndex;
            WorldLocationManager.instance.AddLocationRendererManagerToList(this);
        }

        private void Start()
        {
            if (PlayerUIManager.instance.playerUILoadingScreenManager.LoadingScreenIsActive())
            {
                ToggleRootObjects(true);
            }
            else
            {
                StartCoroutine(EnableRootGameObjectsOverTime());
            }
        }

        //root gameobjects
        public void FindAllRootObjects()
        {
            rootGameObjects = new List<GameObject>();

            GameObject[] rootObjectsInScene = gameObject.scene.GetRootGameObjects();

            for (int i = 0; i < rootObjectsInScene.Length; i++)
            {
                if (rootObjectsInScene[i] == gameObject)
                    continue;

                if (rootGameObjects.Contains(rootObjectsInScene[i]))
                    continue;

                rootGameObjects.Add(rootObjectsInScene[i]);
            }

            //this code will only run in the editor, you need the #if here for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void ToggleRootObjects(bool status)
        {
            for (int i = 0; i < rootGameObjects.Count; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                rootGameObjects[i].SetActive(status);
            }

            //this code will only run in the editor, you need the #if here for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        private IEnumerator EnableRootGameObjectsOverTime()
        {
            for (int i = 0; i < rootGameObjects.Count; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                rootGameObjects[i].SetActive(true);

                yield return new WaitForEndOfFrame();
            }
        }

        //renderers
        public void FindAllMeshRenderers()
        {
            MeshRenderer[] allMeshRenderers = FindObjectsByType<MeshRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            meshRenderers = new List<MeshRenderer>();

            for (int i = 0; i < allMeshRenderers.Length; i++)
            {
                if (allMeshRenderers[i].gameObject.scene != gameObject.scene)
                    continue;

                if (!meshRenderers.Contains(allMeshRenderers[i]))
                    meshRenderers.Add(allMeshRenderers[i]);
            }

            //this code will only run in the editor, you need the #if here for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void ToggleMeshRenderers(bool status)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                meshRenderers[i].enabled = status;
            }

            //this code will only run in the editor, you need the #if here for that check
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void ToggleAllMeshRenderersOverTime(bool status)
        {
            if (toggleAllMeshRenderersCoroutine != null)
                StopCoroutine(toggleAllMeshRenderersCoroutine);

            toggleAllMeshRenderersCoroutine = StartCoroutine(ToggleAllMeshRenderersOverTimeCoroutine(status));
        }

        private IEnumerator ToggleAllMeshRenderersOverTimeCoroutine(bool status)
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                meshRenderers[i].enabled = status;


            }
        }
    }
}
