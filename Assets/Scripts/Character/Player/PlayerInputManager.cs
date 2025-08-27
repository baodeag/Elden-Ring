using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace baodeag
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        PlayerControls playerControls;

        [SerializeField] Vector2 movementInput;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //when scene changes, call OnSceneChange
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false; //disable player controls by default
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //if we are loading into our world scene, enable our player controls
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            //otherwise disable them
            //this is so our player cant move around in menus or other scenes
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>(); // Get movement input
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            //when this object is destroyed, unsubscribe from the event
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
