using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections.Generic;

namespace baodeag
{
    public class PlayerManager : CharacterManager
    {
        private const int DefaultStartingVigor = 15;
        private const int DefaultStartingMind = 10;
        private const int DefaultStartingEndurance = 10;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        [HideInInspector] public PlayerBodyManager playerBodyManager;
        [HideInInspector] public PlayerShopManager playerShopManager;

        [Header("Area")]
        public WorldLocationSceneSet areaCurrentlyIn;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerBodyManager = GetComponent<PlayerBodyManager>();
            playerShopManager = GetComponent<PlayerShopManager>();

            if (playerShopManager == null)
                playerShopManager = gameObject.AddComponent<PlayerShopManager>();
        }

        protected override void Update()
        {
            base.Update();

            //if we dont own this player, dont do anything
            if (!IsOwner)
                return;

            //handle movement
            playerLocomotionManager.HandleAllMovement();

            //regen stamina
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if(!IsOwner)
                return;

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            //if we own this player, set the camera's player to this
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                PlayerUIManager.instance.localPlayer = this;
                WorldSaveGameManager.instance.player = this;

                //update the total amount of health when the stat linked to either changes
                playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusPointsValue;

                //update the total amount of build up we can endure based on vitality level
                playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewMaxBuildUpCapacityValue;

                //updates ui stat bars when a stats change (health or stamina)
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointValue;

                //update ui build up bars when build up changes
                playerNetworkManager.poisonBuildUp.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewPoisonBuildUpAmount;
                playerNetworkManager.bleedBuildUp.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewBleedBuildUpAmount;
                playerNetworkManager.frostBiteBuildUp.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewFrostBuildUpAmount;

                playerNetworkManager.SetNewMaxHealthValue(0, playerNetworkManager.vigor.Value); 
                playerNetworkManager.SetNewMaxStaminaValue(0, playerNetworkManager.endurance.Value);
                playerNetworkManager.SetNewMaxFocusPointsValue(0, playerNetworkManager.mind.Value);

                //reset camera rotation to standard when aiming is disabled
                playerNetworkManager.isAiming.OnValueChanged += playerNetworkManager.OnIsAimingChanged;
            }

            //only update the floating hp bar if this character is not the local players character
            if (!IsOwner)
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;

            // body type
            playerNetworkManager.isMale.OnValueChanged += playerNetworkManager.OnIsMaleChanged;
            playerNetworkManager.hairColorRed.OnValueChanged += playerNetworkManager.OnHairColorRedChanged;
            playerNetworkManager.hairColorGreen.OnValueChanged += playerNetworkManager.OnHairColorGreenChanged;
            playerNetworkManager.hairColorBlue.OnValueChanged += playerNetworkManager.OnHairColorBlueChanged;

            //stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.OnHPChanged;
            playerNetworkManager.currentFocusPoints.OnValueChanged += playerNetworkManager.OnFocusPointsChanged;
            playerNetworkManager.maxFocusPoints.OnValueChanged += playerNetworkManager.OnMaxFocusPointsChanged;

            //status effects
            playerNetworkManager.isPoisoned.OnValueChanged += playerNetworkManager.OnIsPoisonedChanged;
            playerNetworkManager.isBleeding.OnValueChanged += playerNetworkManager.OnIsBleedingChanged;
            playerNetworkManager.isFrostBitten.OnValueChanged += playerNetworkManager.OnIsFrostBittenChanged;
            playerNetworkManager.isFrozen.OnValueChanged += playerNetworkManager.OnIsFrozenChanged;

            //lock on
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

            //body
            playerNetworkManager.hairStyleID.OnValueChanged += playerNetworkManager.OnHairStyleIDChanged;
            playerNetworkManager.hairColorRed.OnValueChanged += playerNetworkManager.OnHairColorRedChanged;
            playerNetworkManager.hairColorGreen.OnValueChanged += playerNetworkManager.OnHairColorGreenChanged;
            playerNetworkManager.hairColorBlue.OnValueChanged += playerNetworkManager.OnHairColorBlueChanged;

            //equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentQuickSlotItemID.OnValueChanged += playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            playerNetworkManager.isChugging.OnValueChanged += playerNetworkManager.OnIsChuggingChanged;
            playerNetworkManager.currentSpellID.OnValueChanged += playerNetworkManager.OnCurrentSpellIDChange;
            playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;

            playerNetworkManager.headEquipmentID.OnValueChanged += playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged += playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged += playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged += playerNetworkManager.OnHandEquipmentChanged;

            playerNetworkManager.mainProjectileID.OnValueChanged += playerNetworkManager.OnMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged += playerNetworkManager.OnSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged += playerNetworkManager.OnIsHoldingArrowChanged;

            //spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged += playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged += playerNetworkManager.OnIsChargingLeftSpellChanged;

            //two hand
            playerNetworkManager.isTwoHandingWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

            //flags
            playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;
            playerNetworkManager.isSneaking.OnValueChanged += playerNetworkManager.OnIsSneakingChanged;

            //upon connecting, if we are the owner of this player, but not the server, reload our character data to this newly instantiated character
            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

            //if we own this player, set the camera's player to this
            if (IsOwner)
            {
                //update the total amount of health when the sts linked to either changes
                playerNetworkManager.vigor.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusPointsValue;

                //update the total amount of build up we can endure based on vitality level
                playerNetworkManager.vigor.OnValueChanged -= playerNetworkManager.SetNewMaxBuildUpCapacityValue;

                //updates ui stat bars when a stats change (health or stamina)
                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointValue;

                //update ui build up bars when build up changes
                playerNetworkManager.poisonBuildUp.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewPoisonBuildUpAmount;
                playerNetworkManager.bleedBuildUp.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewBleedBuildUpAmount;
                playerNetworkManager.frostBiteBuildUp.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewFrostBuildUpAmount;

                //reset camera rotation to standard when aiming is disabled
                playerNetworkManager.isAiming.OnValueChanged -= playerNetworkManager.OnIsAimingChanged;
            }

            if (!IsOwner)
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;

            // body type
            playerNetworkManager.isMale.OnValueChanged -= playerNetworkManager.OnIsMaleChanged;

            //stats
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.OnHPChanged;
            playerNetworkManager.currentFocusPoints.OnValueChanged -= playerNetworkManager.OnFocusPointsChanged;
            playerNetworkManager.maxFocusPoints.OnValueChanged -= playerNetworkManager.OnMaxFocusPointsChanged;

            //status effects
            playerNetworkManager.isPoisoned.OnValueChanged -= playerNetworkManager.OnIsPoisonedChanged;
            playerNetworkManager.isBleeding.OnValueChanged -= playerNetworkManager.OnIsBleedingChanged;
            playerNetworkManager.isFrostBitten.OnValueChanged -= playerNetworkManager.OnIsFrostBittenChanged;
            playerNetworkManager.isFrozen.OnValueChanged -= playerNetworkManager.OnIsFrozenChanged;

            //lock on
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;

            //body
            playerNetworkManager.hairStyleID.OnValueChanged -= playerNetworkManager.OnHairStyleIDChanged;
            playerNetworkManager.hairColorRed.OnValueChanged -= playerNetworkManager.OnHairColorRedChanged;
            playerNetworkManager.hairColorGreen.OnValueChanged -= playerNetworkManager.OnHairColorGreenChanged;
            playerNetworkManager.hairColorBlue.OnValueChanged -= playerNetworkManager.OnHairColorBlueChanged;

            //equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentQuickSlotItemID.OnValueChanged -= playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            playerNetworkManager.isChugging.OnValueChanged -= playerNetworkManager.OnIsChuggingChanged;
            playerNetworkManager.currentSpellID.OnValueChanged -= playerNetworkManager.OnCurrentSpellIDChange;


            playerNetworkManager.headEquipmentID.OnValueChanged -= playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged -= playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged -= playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged -= playerNetworkManager.OnHandEquipmentChanged;

            playerNetworkManager.mainProjectileID.OnValueChanged -= playerNetworkManager.OnMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged -= playerNetworkManager.OnSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged -= playerNetworkManager.OnIsHoldingArrowChanged;

            //spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged -= playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged -= playerNetworkManager.OnIsChargingLeftSpellChanged;

            //two hand
            playerNetworkManager.isTwoHandingWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

            //flags
            playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
            playerNetworkManager.isSneaking.OnValueChanged -= playerNetworkManager.OnIsSneakingChanged;
        }

        private void OnClientConnectedCallback(ulong clientID)
        {
            WorldGameSessionManager.instance.AddPlayerToActivePlayersList(this);

            if (IsServer && clientID != NetworkManager.Singleton.LocalClientId)
            {
                WorldAIManager.instance.DespawnAllDeadCharacters();
            }

            //keep a list of active players in the game
            if (!IsServer && IsOwner)
            {
                if (PlayerUIManager.instance != null)
                    PlayerUIManager.instance.playerUILoadingScreenManager.ForceHideLoadingScreen();

                foreach (var player in WorldGameSessionManager.instance.players)
                {
                    if (player != this)
                    {
                        player.LoadOtherPlayerCharacterWhenJoiningServer();
                    }
                }

                StartCoroutine(EmergeAtMostRecentSiteOfGrace());
            }
        }

        private IEnumerator EmergeAtMostRecentSiteOfGrace()
        {
            yield return new WaitForSeconds(1.5f);

            PlayerManager hostPlayer = null;

            while (hostPlayer == null)
            {
                for (int i = 0; i < WorldGameSessionManager.instance.players.Count; i++)
                {
                    if (WorldGameSessionManager.instance.players[i].IsHost)
                    {
                        hostPlayer = WorldGameSessionManager.instance.players[i];
                    }
                }

                yield return null;
            }

            int hostGraceId = hostPlayer.playerNetworkManager.lastSiteOfGraceUsed.Value;
            playerNetworkManager.lastSiteOfGraceUsed.Value = hostGraceId;
            WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = hostGraceId;

            if (WorldObjectManager.instance.sitesOfGrace.Count > hostGraceId
                && WorldObjectManager.instance.sitesOfGrace[hostGraceId] != null)
            {
                WorldObjectManager.instance.sitesOfGrace[hostGraceId].TeleportPlayerToSiteOfGrace(this, false);
            }

            if (IsOwner && PlayerUIManager.instance != null)
            {
                PlayerUIManager.instance.playerUILoadingScreenManager.ForceHideLoadingScreen();
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                playerInteractionManager.ClearInteractionList();
                PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
                WorldGameSessionManager.instance.WaitThenRevivePlayer(this);
            }

            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            if (IsOwner)
            {
                isDead.Value = false;
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.isMale = playerNetworkManager.isMale.Value;

            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            //stats
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
            currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;

            currentCharacterData.vitality = playerNetworkManager.vigor.Value;
            currentCharacterData.mind = playerNetworkManager.mind.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
            currentCharacterData.strength = playerNetworkManager.strength.Value;
            currentCharacterData.dexterity = playerNetworkManager.dexterity.Value;
            currentCharacterData.intelligence = playerNetworkManager.intelligence.Value;
            currentCharacterData.faith = playerNetworkManager.faith.Value;

            currentCharacterData.runes = playerStatsManager.runes;

            //body
            currentCharacterData.hairStyleID = playerNetworkManager.hairStyleID.Value;
            currentCharacterData.hairColorRed = playerNetworkManager.hairColorRed.Value;
            currentCharacterData.hairColorGreen = playerNetworkManager.hairColorGreen.Value;
            currentCharacterData.hairColorBlue = playerNetworkManager.hairColorBlue.Value;

            currentCharacterData.currentHealthFlasksRemaining = playerNetworkManager.remainingHealthFlasks.Value;
            currentCharacterData.currentFocusPointsFlaskRemaining = playerNetworkManager.remainingFocusPointsFlasks.Value;

            //equipment
            currentCharacterData.headEquipment = playerNetworkManager.headEquipmentID.Value;
            currentCharacterData.bodyEquipment = playerNetworkManager.bodyEquipmentID.Value;
            currentCharacterData.legEquipment = playerNetworkManager.legEquipmentID.Value;
            currentCharacterData.handEquipment = playerNetworkManager.handEquipmentID.Value;

            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
            currentCharacterData.rightWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[0]);
            currentCharacterData.rightWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[1]);
            currentCharacterData.rightWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[2]);

            currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
            currentCharacterData.leftWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
            currentCharacterData.leftWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[1]);
            currentCharacterData.leftWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[2]);

            currentCharacterData.quickSlotIndex = playerInventoryManager.quickSlotItemIndex;
            currentCharacterData.quickSlotItem01 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[0]);
            currentCharacterData.quickSlotItem02 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[1]);
            currentCharacterData.quickSlotItem03 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[2]);

            currentCharacterData.mainProjectile = WorldSaveGameManager.instance.GetSerializableRangedProjectileFromRangedProjectileItem(playerInventoryManager.mainProjectile);
            currentCharacterData.secondaryProjectile = WorldSaveGameManager.instance.GetSerializableRangedProjectileFromRangedProjectileItem(playerInventoryManager.secondaryProjectile);

            if (playerInventoryManager.currentSpell != null)
                currentCharacterData.currentSpell = playerInventoryManager.currentSpell.itemID;

            //clear lists before save
            currentCharacterData.weaponsInInventory = new List<SerializableWeapon>();
            currentCharacterData.projectilesInInventory = new List<SerializableRangedProjectile>();
            currentCharacterData.quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            currentCharacterData.headEquipmentInInventory = new List<int>();
            currentCharacterData.bodyEquipmentInInventory = new List<int>();
            currentCharacterData.legEquipmentInInventory = new List<int>();
            currentCharacterData.handEquipmentInInventory = new List<int>();

            for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                WeaponItem weaponInInventory = playerInventoryManager.itemsInInventory[i] as WeaponItem;
                HeadEquipmentItem headEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;
                BodyEquipmentItem bodyEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;
                LegEquipmentItem legEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;
                HandEquipmentItem handEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;
                QuickSlotItem quickSlotItemInInventory = playerInventoryManager.itemsInInventory[i] as QuickSlotItem;
                RangedProjectileItem projectileItemInInventory = playerInventoryManager.itemsInInventory[i] as RangedProjectileItem;

                if (weaponInInventory != null)
                    currentCharacterData.weaponsInInventory.Add(WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));

                if (headEquipmentInInventory != null)
                    currentCharacterData.headEquipmentInInventory.Add(headEquipmentInInventory.itemID);

                if (bodyEquipmentInInventory != null)
                    currentCharacterData.bodyEquipmentInInventory.Add(bodyEquipmentInInventory.itemID);

                if (legEquipmentInInventory != null)
                    currentCharacterData.legEquipmentInInventory.Add(legEquipmentInInventory.itemID);

                if (handEquipmentInInventory != null)
                    currentCharacterData.handEquipmentInInventory.Add(handEquipmentInInventory.itemID);

                if (projectileItemInInventory != null)
                    currentCharacterData.projectilesInInventory.Add(WorldSaveGameManager.instance.GetSerializableRangedProjectileFromRangedProjectileItem(projectileItemInInventory));

                if (quickSlotItemInInventory != null)
                    currentCharacterData.quickSlotItemsInInventory.Add(WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(quickSlotItemInInventory));
            }
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            playerNetworkManager.isMale.Value = currentCharacterData.isMale;

            playerBodyManager.ToggleBodyType(currentCharacterData.isMale); //toggle incase the value is the same as default (onvaluechanged only works when value is changed)

            Vector3 myPosition = new Vector3(
                currentCharacterData.xPosition, 
                currentCharacterData.yPosition, 
                currentCharacterData.zPosition);
            transform.position = myPosition;

            //stats
            int loadedVigor = currentCharacterData.vitality > 0 ? currentCharacterData.vitality : DefaultStartingVigor;
            int loadedMind = currentCharacterData.mind > 0 ? currentCharacterData.mind : DefaultStartingMind;
            int loadedEndurance = currentCharacterData.endurance > 0 ? currentCharacterData.endurance : DefaultStartingEndurance;

            playerNetworkManager.vigor.Value = loadedVigor;
            playerNetworkManager.mind.Value = loadedMind;
            playerNetworkManager.endurance.Value= loadedEndurance;
            playerNetworkManager.strength.Value = currentCharacterData.strength;
            playerNetworkManager.dexterity.Value = currentCharacterData.dexterity;
            playerNetworkManager.intelligence.Value = currentCharacterData.intelligence;
            playerNetworkManager.faith.Value = currentCharacterData.faith;

            //this will be moved when saving and loading is added
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vigor.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.maxFocusPoints.Value = playerStatsManager.CalculateFocusPointsBasedOnMindLevel(playerNetworkManager.mind.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth > 0
                ? Mathf.Min(currentCharacterData.currentHealth, playerNetworkManager.maxHealth.Value)
                : playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina > 0
                ? Mathf.Min(currentCharacterData.currentStamina, playerNetworkManager.maxStamina.Value)
                : playerNetworkManager.maxStamina.Value;
            playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints > 0
                ? Mathf.Min(currentCharacterData.currentFocusPoints, playerNetworkManager.maxFocusPoints.Value)
                : playerNetworkManager.maxFocusPoints.Value;
            playerNetworkManager.buildUpCapacity.Value = playerStatsManager.CalculateBuildUpCapacityBasedOnVitalityLevel(playerNetworkManager.vigor.Value);
            playerNetworkManager.lastSiteOfGraceUsed.Value = currentCharacterData.lastSiteOfGraceRestedAt;
            isDead.Value = false;

            playerStatsManager.runes = currentCharacterData.runes;

            if (IsOwner && PlayerUIManager.instance != null)
                PlayerUIManager.instance.playerUIHudManager.RefreshRuneCountImmediate();

            playerNetworkManager.remainingHealthFlasks.Value = currentCharacterData.currentHealthFlasksRemaining;
            playerNetworkManager.remainingFocusPointsFlasks.Value = currentCharacterData.currentFocusPointsFlaskRemaining;

            //body
            playerNetworkManager.hairStyleID.Value = currentCharacterData.hairStyleID;
            playerNetworkManager.hairColorRed.Value = currentCharacterData.hairColorRed;
            playerNetworkManager.hairColorGreen.Value = currentCharacterData.hairColorGreen;
            playerNetworkManager.hairColorBlue.Value = currentCharacterData.hairColorBlue;

            //equipment

            if (WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterData.headEquipment))
            {
                HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterData.headEquipment));
                playerInventoryManager.headEquipment = headEquipment;
            }
            else
            {
                playerInventoryManager.headEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment))
            {
                BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment));
                playerInventoryManager.bodyEquipment = bodyEquipment;
            }
            else
            {
                playerInventoryManager.bodyEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterData.handEquipment))
            {
                HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterData.handEquipment));
                playerInventoryManager.handEquipment = handEquipment;
            }
            else
            {
                playerInventoryManager.handEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterData.legEquipment))
            {
                LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterData.legEquipment));
                playerInventoryManager.legEquipment = legEquipment;
            }
            else
            {
                playerInventoryManager.legEquipment = null;
            }

            //weapons
            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
            playerInventoryManager.weaponsInRightHandSlots[0] = currentCharacterData.rightWeapon01.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[1] = currentCharacterData.rightWeapon02.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[2] = currentCharacterData.rightWeapon03.GetWeapon();

            playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
            playerInventoryManager.weaponsInLeftHandSlots[0] = currentCharacterData.leftWeapon01.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[1] = currentCharacterData.leftWeapon02.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[2] = currentCharacterData.leftWeapon03.GetWeapon();

            //quick slot items
            playerInventoryManager.quickSlotItemIndex = currentCharacterData.quickSlotIndex;
            playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentCharacterData.quickSlotItem01.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentCharacterData.quickSlotItem02.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentCharacterData.quickSlotItem03.GetQuickSlotItem();
            playerEquipmentManager.LoadQuickSlotEquipment(playerInventoryManager.quickSlotItemsInQuickSlots[playerInventoryManager.quickSlotItemIndex]); //this refreshes the hud

            if (currentCharacterData.rightWeaponIndex >= 0)
            {
                playerInventoryManager.currentRightHandWeapon = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex];
                playerNetworkManager.currentRightHandWeaponID.Value = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
            }

            playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;

            if (currentCharacterData.leftWeaponIndex >= 0)
            {
                playerInventoryManager.currentLeftHandWeapon = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex];
                playerNetworkManager.currentLeftHandWeaponID.Value = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
            }

            if (WorldItemDatabase.Instance.GetSpellByID(currentCharacterData.currentSpell))
            {
                SpellItem currentSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(currentCharacterData.currentSpell));
                playerNetworkManager.currentSpellID.Value = currentSpell.itemID;
            }
            else
            {
                playerNetworkManager.currentSpellID.Value = -1;
            }

            for (int i = 0; i < currentCharacterData.weaponsInInventory.Count; i++)
            {
                WeaponItem weapon = currentCharacterData.weaponsInInventory[i].GetWeapon();
                playerInventoryManager.AddItemToInventory(weapon);
            }

            for (int i = 0; i < currentCharacterData.headEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterData.headEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.bodyEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.legEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterData.legEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.handEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipment = WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterData.handEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.quickSlotItemsInInventory.Count; i++)
            {
                QuickSlotItem quickSlotItem = currentCharacterData.quickSlotItemsInInventory[i].GetQuickSlotItem();
                playerInventoryManager.AddItemToInventory(quickSlotItem);
            }

            for (int i = 0; i < currentCharacterData.projectilesInInventory.Count; i++)
            {
                RangedProjectileItem projectile = currentCharacterData.projectilesInInventory[i].GetProjectile();
                playerInventoryManager.AddItemToInventory(projectile);
            }

            playerEquipmentManager.EquipArmor();
            playerEquipmentManager.LoadMainProjectileEquipment(currentCharacterData.mainProjectile.GetProjectile());
            playerEquipmentManager.LoadSecondaryProjectileEquipment(currentCharacterData.secondaryProjectile.GetProjectile());
        }

        public void LoadOtherPlayerCharacterWhenJoiningServer()
        {
            //sync body type
            playerNetworkManager.OnIsMaleChanged(false, playerNetworkManager.isMale.Value);
            playerNetworkManager.OnHairStyleIDChanged(0, playerNetworkManager.hairStyleID.Value);
            playerNetworkManager.OnHairColorRedChanged(0, playerNetworkManager.hairColorRed.Value);
            playerNetworkManager.OnHairColorGreenChanged(0, playerNetworkManager.hairColorGreen.Value);
            playerNetworkManager.OnHairColorBlueChanged(0, playerNetworkManager.hairColorBlue.Value);

            //sync weapons
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            playerNetworkManager.OnCurrentSpellIDChange(0, playerNetworkManager.currentSpellID.Value);

            //sync armor
            playerNetworkManager.OnHeadEquipmentChanged(0, playerNetworkManager.headEquipmentID.Value);
            playerNetworkManager.OnBodyEquipmentChanged(0, playerNetworkManager.bodyEquipmentID.Value);
            playerNetworkManager.OnLegEquipmentChanged(0, playerNetworkManager.legEquipmentID.Value);
            playerNetworkManager.OnHandEquipmentChanged(0, playerNetworkManager.handEquipmentID.Value);

            //sync projectiles
            playerNetworkManager.OnMainProjectileIDChange(0, playerNetworkManager.mainProjectileID.Value);
            playerNetworkManager.OnSecondaryProjectileIDChange(0, playerNetworkManager.secondaryProjectileID.Value);
            playerNetworkManager.OnIsHoldingArrowChanged(false, playerNetworkManager.isHoldingArrow.Value);

            //sync two hand status
            playerNetworkManager.OnIsTwoHandingRightWeaponChanged(false, playerNetworkManager.isTwoHandingRightWeapon.Value);
            playerNetworkManager.OnIsTwoHandingLeftWeaponChanged(false, playerNetworkManager.isTwoHandingLeftWeapon.Value);

            //sync status effects
            playerNetworkManager.OnIsPoisonedChanged(false, playerNetworkManager.isPoisoned.Value);

            //sync block status
            playerNetworkManager.OnIsBlockingChanged(false, playerNetworkManager.isBlocking.Value);

            //armor

            //lock on
            if (playerNetworkManager.isLockedOn.Value)
            {
                playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
            }
        }

        
    }
}
