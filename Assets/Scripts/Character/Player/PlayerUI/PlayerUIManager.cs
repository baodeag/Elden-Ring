using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;
        [HideInInspector] public PlayerManager localPlayer;
        private AudioSource audioSource;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUICharacterMenuManager playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager playerUIEquipmentManager;
        [HideInInspector] public PlayerUISiteOfGraceManager playerUISiteOfGraceManager;
        [HideInInspector] public PlayerUITeleportLocationManager playerUITeleportLocationManager;
        [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager;
        [HideInInspector] public PlayerUILevelUpManager playerUILevelUpManager;
        [HideInInspector] public PlayerUIWeaponUpgradeManager playerUIWeaponUpgradeManager;

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false;
        public bool popUpWindowIsOpen = false;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            audioSource = GetComponent<AudioSource>();

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
            playerUISiteOfGraceManager = GetComponentInChildren<PlayerUISiteOfGraceManager>();
            playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
            playerUILevelUpManager = GetComponentInChildren<PlayerUILevelUpManager>();
            playerUIWeaponUpgradeManager = GetComponentInChildren<PlayerUIWeaponUpgradeManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                //We must first shut down, because we have started as a host during the title screen
                NetworkManager.Singleton.Shutdown();
                //We then restart, as a client
                NetworkManager.Singleton.StartClient();
            }
        }

        public void CloseAllMenuWindows()
        {
            playerUICharacterMenuManager.CloseMenuAfterFixedFrame();
            playerUIEquipmentManager.CloseMenuAfterFixedFrame();
            playerUISiteOfGraceManager.CloseMenuAfterFixedFrame();
            playerUITeleportLocationManager.CloseMenuAfterFixedFrame();
            playerUILevelUpManager.CloseMenuAfterFixedFrame();
            playerUIWeaponUpgradeManager.CloseMenuAfterFixedFrame();
        }

        //ui sfx
        public void PlayUnableToContinueSFX()
        {
            if (WorldSoundFXManager.instance.unableToContinueUISFX == null)
                return;

            audioSource.PlayOneShot(WorldSoundFXManager.instance.unableToContinueUISFX);
        }

        public void PlayConfirmSFX()
        {
            if (WorldSoundFXManager.instance.confirmUISFX == null)
                return;

            audioSource.PlayOneShot(WorldSoundFXManager.instance.confirmUISFX);
        }

        public void PlayHoverSFX()
        {
            if (WorldSoundFXManager.instance.hoverUISFX == null)
                return;

            audioSource.PlayOneShot(WorldSoundFXManager.instance.hoverUISFX);
        }
    }
}
