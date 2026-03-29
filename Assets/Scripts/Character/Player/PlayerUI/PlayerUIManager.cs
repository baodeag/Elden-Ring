using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

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
        [HideInInspector] public PlayerUIShopManager playerUIShopManager;
        [HideInInspector] public PlayerUISettingsManager playerUISettingsManager;

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false;
        public bool popUpWindowIsOpen = false;

        private readonly List<PlayerUIMenu> menuNavigationStack = new List<PlayerUIMenu>();

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
            playerUIShopManager = GetComponentInChildren<PlayerUIShopManager>(true);
            playerUISettingsManager = GetComponentInChildren<PlayerUISettingsManager>(true);

            if (playerUIShopManager == null)
                playerUIShopManager = gameObject.AddComponent<PlayerUIShopManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            ApplyAudioSettings();
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

            if (playerUIShopManager != null)
                playerUIShopManager.CloseMenuAfterFixedFrame();

            if (playerUISettingsManager != null)
                playerUISettingsManager.CloseMenuAfterFixedFrame();

            menuNavigationStack.Clear();
        }

        public void OpenMenuAsRoot(PlayerUIMenu menu)
        {
            if (menu == null)
                return;

            CloseAllMenuWindowsImmediate();
            menuNavigationStack.Clear();
            menu.OpenMenu();
            menuNavigationStack.Add(menu);
        }

        public void TransitionToMenu(PlayerUIMenu fromMenu, PlayerUIMenu toMenu)
        {
            if (toMenu == null)
                return;

            if (fromMenu != null)
            {
                EnsureMenuTracked(fromMenu);

                if (fromMenu.IsMenuOpen())
                    fromMenu.CloseMenu();
            }

            RemoveTrackedMenu(toMenu);
            toMenu.OpenMenu();
            menuNavigationStack.Add(toMenu);
        }

        public bool CloseCurrentMenuStep()
        {
            PlayerUIMenu currentMenu = GetTopOpenMenu();

            if (currentMenu == null)
                return false;

            currentMenu.CloseMenu();
            RemoveTrackedMenu(currentMenu);

            PlayerUIMenu previousMenu = GetTrackedPreviousMenu();

            if (previousMenu != null && !previousMenu.IsMenuOpen())
                previousMenu.OpenMenu();

            return true;
        }

        public void RefreshMenuWindowState()
        {
            menuWindowIsOpen =
                IsMenuOpen(playerUICharacterMenuManager) ||
                IsMenuOpen(playerUIEquipmentManager) ||
                IsMenuOpen(playerUISiteOfGraceManager) ||
                IsMenuOpen(playerUITeleportLocationManager) ||
                IsMenuOpen(playerUILevelUpManager) ||
                IsMenuOpen(playerUIWeaponUpgradeManager) ||
                IsMenuOpen(playerUIShopManager) ||
                IsMenuOpen(playerUISettingsManager);

            if (PlayerInputManager.instance != null)
                PlayerInputManager.instance.SuppressGameplayInputs(menuWindowIsOpen);
        }

        private bool IsMenuOpen(PlayerUIMenu menu)
        {
            return menu != null && menu.IsMenuOpen();
        }

        private void CloseAllMenuWindowsImmediate()
        {
            if (playerUICharacterMenuManager != null && playerUICharacterMenuManager.IsMenuOpen())
                playerUICharacterMenuManager.CloseMenu();

            if (playerUIEquipmentManager != null && playerUIEquipmentManager.IsMenuOpen())
                playerUIEquipmentManager.CloseMenu();

            if (playerUISiteOfGraceManager != null && playerUISiteOfGraceManager.IsMenuOpen())
                playerUISiteOfGraceManager.CloseMenu();

            if (playerUITeleportLocationManager != null && playerUITeleportLocationManager.IsMenuOpen())
                playerUITeleportLocationManager.CloseMenu();

            if (playerUILevelUpManager != null && playerUILevelUpManager.IsMenuOpen())
                playerUILevelUpManager.CloseMenu();

            if (playerUIWeaponUpgradeManager != null && playerUIWeaponUpgradeManager.IsMenuOpen())
                playerUIWeaponUpgradeManager.CloseMenu();

            if (playerUIShopManager != null && playerUIShopManager.IsMenuOpen())
                playerUIShopManager.CloseMenu();

            if (playerUISettingsManager != null && playerUISettingsManager.IsMenuOpen())
                playerUISettingsManager.CloseMenu();
        }

        private PlayerUIMenu GetTopOpenMenu()
        {
            if (IsMenuOpen(playerUISettingsManager))
                return playerUISettingsManager;

            if (IsMenuOpen(playerUIShopManager))
                return playerUIShopManager;

            if (IsMenuOpen(playerUIWeaponUpgradeManager))
                return playerUIWeaponUpgradeManager;

            if (IsMenuOpen(playerUITeleportLocationManager))
                return playerUITeleportLocationManager;

            if (IsMenuOpen(playerUILevelUpManager))
                return playerUILevelUpManager;

            if (IsMenuOpen(playerUIEquipmentManager))
                return playerUIEquipmentManager;

            if (IsMenuOpen(playerUISiteOfGraceManager))
                return playerUISiteOfGraceManager;

            if (IsMenuOpen(playerUICharacterMenuManager))
                return playerUICharacterMenuManager;

            return null;
        }

        private PlayerUIMenu GetTrackedPreviousMenu()
        {
            for (int i = menuNavigationStack.Count - 1; i >= 0; i--)
            {
                if (menuNavigationStack[i] != null)
                    return menuNavigationStack[i];
            }

            return null;
        }

        private void EnsureMenuTracked(PlayerUIMenu menu)
        {
            if (menu == null)
                return;

            if (!menuNavigationStack.Contains(menu))
                menuNavigationStack.Add(menu);
        }

        private void RemoveTrackedMenu(PlayerUIMenu menu)
        {
            if (menu == null)
                return;

            for (int i = menuNavigationStack.Count - 1; i >= 0; i--)
            {
                if (menuNavigationStack[i] == menu)
                    menuNavigationStack.RemoveAt(i);
            }
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

        public void ApplyAudioSettings()
        {
            if (audioSource == null || !GameSettingsManager.HasInstance)
                return;

            audioSource.volume = GameSettingsManager.Instance.GetEffectiveSFXVolume();
        }
    }
}
