using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace baodeag
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;

        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>(
            "Character", 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);

        [Header("Site Of Grace")]
        public NetworkVariable<int> lastSiteOfGraceUsed = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Flasks")]
        public NetworkVariable<int> remainingHealthFlasks = new NetworkVariable<int>(
            3,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> remainingFocusPointsFlasks = new NetworkVariable<int>(
            3,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChugging = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);


        [Header("Actions")]
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Body")]
        public NetworkVariable<int> hairStyleID = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> hairColorRed = new NetworkVariable<float>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> hairColorGreen = new NetworkVariable<float>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> hairColorBlue = new NetworkVariable<float>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(
            0, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentSpellID = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentQuickSlotItemID = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Two Handing")]
        public NetworkVariable<int> currentWeaponBeingTwoHanded = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingWeapon = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingRightWeapon = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingLeftWeapon = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Spells")]
        public NetworkVariable<bool> isChargingRightSpell = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingLeftSpell = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Armor")]
        public NetworkVariable<bool> isMale = new NetworkVariable<bool>(
            true,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> headEquipmentID = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> bodyEquipmentID = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> legEquipmentID = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> handEquipmentID = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Projectiles")]
        public NetworkVariable<int> mainProjectileID = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> secondaryProjectileID = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasArrowNotched = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isHoldingArrow = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isAiming = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsDeadChanged(oldStatus, newStatus);

            if (player.isDead.Value)
                player.playerCombatManager.CreateDeadSpot(player.transform.position, player.playerStatsManager.runes);

            if (player.isDead.Value && NetworkManager.Singleton.IsServer)
            {
                //remove the boss hp bar from the ui
                if (PlayerUIManager.instance.playerUIHudManager.currentBossHealthBar != null)
                    PlayerUIManager.instance.playerUIHudManager.currentBossHealthBar.RemoveHPBar(1f);

                WorldAIManager.instance.DisableAllBossFights();
            }
        }

        [ServerRpc]
        public void ReportDeathForLoseConditionServerRpc(int mapIndex)
        {
            if (!IsServer || WorldGameSessionManager.instance == null)
                return;

            if (!WorldGameSessionManager.instance.TryRegisterPlayerDeathForLose(player.OwnerClientId, mapIndex, out int deathCount))
                return;

            BroadcastSessionLoseClientRpc(player.OwnerClientId, deathCount, mapIndex);
        }

        [ClientRpc]
        private void BroadcastSessionLoseClientRpc(ulong failedPlayerClientId, int deathCount, int mapIndex)
        {
            if (WorldGameSessionManager.instance == null)
                return;

            WorldGameSessionManager.instance.HandleSessionLose(mapIndex, failedPlayerClientId, deathCount);
        }

        [ServerRpc]
        public void RequestSynchronizedEndGameActionServerRpc(int actionID)
        {
            if (!IsServer || WorldGameSessionManager.instance == null)
                return;

            SessionEndGameActionType action = (SessionEndGameActionType)actionID;
            bool shouldShowLoadingScreen = action != SessionEndGameActionType.ReturnToTitle;

            ExecuteSynchronizedEndGameActionClientRpc(actionID, shouldShowLoadingScreen);

            if (action != SessionEndGameActionType.ReturnToTitle)
                WorldGameSessionManager.instance.ExecuteSynchronizedEndGameAction(action, true);
        }

        [ClientRpc]
        private void ExecuteSynchronizedEndGameActionClientRpc(int actionID, bool shouldShowLoadingScreen)
        {
            SessionEndGameActionType action = (SessionEndGameActionType)actionID;

            if (PlayerUIManager.instance != null && PlayerUIManager.instance.playerUIPopUpManager != null)
                PlayerUIManager.instance.playerUIPopUpManager.DismissEndGameOverlayForTransition(shouldShowLoadingScreen);

            if (action == SessionEndGameActionType.ReturnToTitle && WorldGameSessionManager.instance != null)
                WorldGameSessionManager.instance.ExecuteSynchronizedEndGameAction(action, false);
        }

        public override void OnIsBleedingChanged(bool oldStatus, bool newStatus)
        {
            if (isBleeding.Value)
            {
                GameObject bloodLossVFX = Instantiate(WorldCharacterEffectsManager.instance.bloodLossVFX);
                bloodLossVFX.transform.parent = character.characterCombatManager.lockOnTransform;
                bloodLossVFX.transform.localPosition = Vector3.zero;
                bloodLossVFX.transform.localRotation = Quaternion.identity;

                if (player.IsOwner)
                {
                    PlayerUIManager.instance.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Bleed);
                    isBleeding.Value = false;
                }
            }       
        }

        public override void OnIsPoisonedChanged(bool oldStatus, bool newStatus)
        {
            if (player.IsOwner)
            {
                if (isPoisoned.Value)
                {
                    PlayerUIManager.instance.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Poison);
                }

                RefreshHealthBarStatusColor();
            }

            if (isPoisoned.Value)
            {
                if (character.characterEffectsManager.poisonedVFX != null)
                    return;

                GameObject poisonVFX = Instantiate(WorldCharacterEffectsManager.instance.poisonedVFX);
                poisonVFX.transform.parent = character.characterCombatManager.lockOnTransform;
                poisonVFX.transform.localPosition = Vector3.zero;
                poisonVFX.transform.localRotation = Quaternion.identity;
                character.characterEffectsManager.poisonedVFX = poisonVFX;
            }
            else
            {
                if (character.characterEffectsManager.poisonedVFX == null)
                    return;

                Destroy(character.characterEffectsManager.poisonedVFX);

            }          
        }

        public override void OnIsBurningChanged(bool oldStatus, bool newStatus)
        {
            if (player.IsOwner)
            {
                if (isBurning.Value)
                    PlayerUIManager.instance.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Fire);

                RefreshHealthBarStatusColor();
            }

            base.OnIsBurningChanged(oldStatus, newStatus);
        }

        public override void OnIsFrostBittenChanged(bool oldStatus, bool newStatus)
        {
            if (isFrostBitten.Value)
            {
                if (player.IsOwner)
                    PlayerUIManager.instance.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Frost);

                if (character.characterEffectsManager.frostBiteVFX != null)
                    return;

                GameObject frostBite = Instantiate(WorldCharacterEffectsManager.instance.frostBiteVFX);
                frostBite.transform.parent = character.characterCombatManager.lockOnTransform;
                frostBite.transform.localPosition = Vector3.zero;
                frostBite.transform.localRotation = Quaternion.identity;
                player.playerEffectsManager.frostBiteVFX = frostBite;
            }
            else
            {
                if (character.characterEffectsManager.frostBiteVFX == null)
                    return;

                Destroy(character.characterEffectsManager.frostBiteVFX);

            }
        }

        public override void OnIsFrozenChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsFrozenChanged(oldStatus, newStatus);

            if (!isFrozen.Value && IsOwner)
            {
                isFrostBitten.Value = false;
            }
        }

        private void RefreshHealthBarStatusColor()
        {
            if (PlayerUIManager.instance == null || PlayerUIManager.instance.playerUIHudManager == null)
                return;

            if (isBurning.Value)
            {
                PlayerUIManager.instance.playerUIHudManager.healthBar.SetBarFillColor(WorldUtilityManager.Instance.GetBurningColor());
            }
            else if (isPoisoned.Value)
            {
                PlayerUIManager.instance.playerUIHudManager.healthBar.SetBarFillColor(WorldUtilityManager.Instance.GetPoisonedColor());
            }
            else
            {
                PlayerUIManager.instance.playerUIHudManager.healthBar.ResetBarFillColor();
            }
        }

        public void OnIsSneakingChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isSneaking", isSneaking.Value);
        }

        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if (rightHandedAction)
            {
                isUsingLeftHand.Value = false;
                isUsingRightHand.Value = true;
            }
            else
            {
                isUsingRightHand.Value = false;
                isUsingLeftHand.Value = true;
            }
        }

        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CalculateModifiedMaxHealth();
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateModifiedMaxStamina();
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

        public void SetNewMaxFocusPointsValue(int oldMind, int newMind)
        {
            maxFocusPoints.Value = player.playerStatsManager.CalculateModifiedMaxFocusPoints();
            PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointValue(maxFocusPoints.Value);
            currentFocusPoints.Value = maxFocusPoints.Value;
        }

        public void SetNewMaxBuildUpCapacityValue(int oldVitality, int newVitality)
        {
            buildUpCapacity.Value = player.playerStatsManager.CalculateBuildUpCapacityBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxBuildUpValue(Mathf.RoundToInt(buildUpCapacity.Value));
        }

        public void OnHairStyleIDChanged(int oldValue, int newValue)
        {
            player.playerBodyManager.ToggleHairType(hairStyleID.Value);
        }

        public void OnHairColorRedChanged(float oldValue, float newValue)
        {
            player.playerBodyManager.SetHairColor();
        }

        public void OnHairColorGreenChanged(float oldValue, float newValue)
        {
            player.playerBodyManager.SetHairColor();
        }

        public void OnHairColorBlueChanged(float oldValue, float newValue)
        {
            player.playerBodyManager.SetHairColor();
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            if (!player.IsOwner)
            {
                WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
                player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            }
            
            player.playerEquipmentManager.LoadRightWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);

                if (player.playerInventoryManager.currentRightHandWeapon.weaponClass == WeaponClass.Bow)
                {
                    PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(true);
                }
                else
                {
                    PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(false);
                }
            }
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            if (!player.IsOwner)
            {
                WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
                player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            }

            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);

                if (player.playerInventoryManager.currentLeftHandWeapon.weaponClass == WeaponClass.Bow)
                {
                    PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(true);
                }
                else
                {
                    PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(false);
                }
            }
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem weaponTemplate = WorldItemDatabase.Instance.GetWeaponByID(newID);

            if (weaponTemplate == null)
            {
                weaponTemplate = GetEquippedWeaponByID(newID);
            }

            if (weaponTemplate == null)
            {
                
                return;
            }

            WeaponItem newWeapon = Instantiate(weaponTemplate);
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

            if (player.IsOwner)
                return;

            if (player.playerCombatManager.currentWeaponBeingUsed != null)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
        }

        private WeaponItem GetEquippedWeaponByID(int weaponID)
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null &&
                player.playerInventoryManager.currentRightHandWeapon.itemID == weaponID)
                return player.playerInventoryManager.currentRightHandWeapon;

            if (player.playerInventoryManager.currentLeftHandWeapon != null &&
                player.playerInventoryManager.currentLeftHandWeapon.itemID == weaponID)
                return player.playerInventoryManager.currentLeftHandWeapon;

            if (player.playerInventoryManager.currentTwoHandWeapon != null &&
                player.playerInventoryManager.currentTwoHandWeapon.itemID == weaponID)
                return player.playerInventoryManager.currentTwoHandWeapon;

            return null;
        }

        public void OnCurrentSpellIDChange(int oldID, int newID)
        {
            SpellItem newSpell = null;

            if (WorldItemDatabase.Instance.GetSpellByID(newID))
                newSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(newID));

            if (newSpell != null)
            {
                player.playerInventoryManager.currentSpell = newSpell;

                if (player.IsOwner)
                    PlayerUIManager.instance.playerUIHudManager.SetSpellItemQuickSlotIcon(newID);
            }
        }

        public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)
        {
            QuickSlotItem newQuickSlotItem = null;

            if (player.IsOwner && player.playerInventoryManager.quickSlotItemsInQuickSlots != null)
            {
                int currentIndex = player.playerInventoryManager.quickSlotItemIndex;

                if (currentIndex >= 0 &&
                    currentIndex < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length &&
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[currentIndex] != null &&
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[currentIndex].itemID == newID)
                {
                    newQuickSlotItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[currentIndex];
                }
                else
                {
                    for (int i = 0; i < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length; i++)
                    {
                        QuickSlotItem slottedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[i];

                        if (slottedItem == null || slottedItem.itemID != newID)
                            continue;

                        newQuickSlotItem = slottedItem;
                        break;
                    }
                }
            }

            if (newQuickSlotItem == null && WorldItemDatabase.Instance.GetQuickSlotItemByID(newID))
                newQuickSlotItem = Instantiate(WorldItemDatabase.Instance.GetQuickSlotItemByID(newID));

            if (newQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem = newQuickSlotItem;
            }
            else
            {
                player.playerInventoryManager.currentQuickSlotItem = null;
            }

            if (player.IsOwner)
                PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
        }

        public void OnMainProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.mainProjectile = newProjectile;

            if (player.IsOwner)
                PlayerUIManager.instance.playerUIHudManager.SetMainProjectileQuickSlotIcon(player.playerInventoryManager.mainProjectile);
        }

        public void OnSecondaryProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.secondaryProjectile = newProjectile;

            if (player.IsOwner)
                PlayerUIManager.instance.playerUIHudManager.SetSecondaryProjectileQuickSlotIcon(player.playerInventoryManager.secondaryProjectile);
        }

        public void OnMaxFocusPointsChanged(int oldFP, int newFP)
        {
            if (player.IsOwner)
                PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointValue(newFP);
        }

        public void OnFocusPointsChanged(int oldFP, int newFP)
        {
            if (player.IsOwner)
                PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointValue(oldFP, newFP);
        }

        public void OnIsHoldingArrowChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isHoldingArrow", isHoldingArrow.Value);
        }

        public void OnIsAimingChanged(bool oldStatus, bool newStatus)
        {
            if (!isAiming.Value)
            {
                PlayerCamera.instance.cameraObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.instance.cameraObject.fieldOfView = 60;
                PlayerCamera.instance.cameraObject.nearClipPlane = 0.3f;
                PlayerCamera.instance.cameraPivotTransform.localPosition = new Vector3(0, PlayerCamera.instance.cameraPivotYPositionOffSet, 0); 
                PlayerUIManager.instance.playerUIHudManager.crossHair.SetActive(false);
            }
            else
            {
                PlayerCamera.instance.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.instance.cameraPivotTransform.localEulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.instance.cameraObject.fieldOfView = 40;
                PlayerCamera.instance.cameraObject.nearClipPlane = 1.3f;
                PlayerCamera.instance.cameraPivotTransform.localPosition = Vector3.zero;
                PlayerUIManager.instance.playerUIHudManager.crossHair.SetActive(true);
            }
        }

        public void OnIsChargingRightSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isChargingRightSpell", isChargingRightSpell.Value);
        }

        public void OnIsChargingLeftSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isChargingLeftSpell", isChargingLeftSpell.Value);
        }

        public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsBlockingChanged(oldStatus, newStatus);

            if (IsOwner)
            {
                player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsorption;
                player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
                player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsorption;
                player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsorption;
                player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
            }
        }

        public void OnIsTwoHandingWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingWeapon.Value)
            {
                player.animator.SetBool("isTwoHandingWeapon", false);

                if (IsOwner)
                {
                    isTwoHandingLeftWeapon.Value = false;
                    isTwoHandingRightWeapon.Value = false;
                    currentWeaponBeingTwoHanded.Value = 0;
                }

                player.playerInventoryManager.currentTwoHandWeapon = null;
                player.playerEquipmentManager.UnTwoHandWeapon();
                player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.instance.twoHandingEffect.staticEffectID);
                player.animator.SetBool("isTwoHandingWeapon", false);
                player.animator.CrossFade("Empty", 0.2f);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.instance.twoHandingEffect);
                player.playerEffectsManager.AddStaticEffect(twoHandEffect);
                player.animator.SetBool("isTwoHandingWeapon", true);
            }
        }

        public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingRightWeapon.Value)
                return;

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
                isTwoHandingWeapon.Value = true;
            }

            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
            player.playerEquipmentManager.TwoHandRightWeapon();
        }

        public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingLeftWeapon.Value)
                return;

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
                isTwoHandingWeapon.Value = true;
            }

            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerEquipmentManager.TwoHandLeftWeapon();
        }

        public void OnIsChuggingChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isChuggingFlask", isChugging.Value);
        }

        public void OnHeadEquipmentChanged(int oldValue, int newValue)
        {
            //we already run the logic on the owner side, so there no point running it again here
            if (IsOwner)
                return;

            HeadEquipmentItem equipment = WorldItemDatabase.Instance.GetHeadEquipmentByID(headEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHeadEquipment(null);
            }
        }

        public void OnBodyEquipmentChanged(int oldValue, int newValue)
        {
            //we already run the logic on the owner side, so there no point running it again here
            if (IsOwner)
                return;

            BodyEquipmentItem equipment = WorldItemDatabase.Instance.GetBodyEquipmentByID(bodyEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadBodyEquipment(null);
            }
        }

        public void OnLegEquipmentChanged(int oldValue, int newValue)
        {
            //we already run the logic on the owner side, so there no point running it again here
            if (IsOwner)
                return;

            LegEquipmentItem equipment = WorldItemDatabase.Instance.GetLegEquipmentByID(legEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadLegEquipment(null);
            }
        }

        public void OnHandEquipmentChanged(int oldValue, int newValue)
        {
            //we already run the logic on the owner side, so there no point running it again here
            if (IsOwner)
                return;

            HandEquipmentItem equipment = WorldItemDatabase.Instance.GetHandEquipmentByID(handEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHandEquipment(null);
            }
        }

        public void OnIsMaleChanged(bool oldStatus, bool newStatus)
        {
            player.playerBodyManager.ToggleBodyType(isMale.Value);
        }

        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            if (IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

            if (weapon == null)
                weapon = GetEquippedWeaponByID(weaponID);

            if (weaponAction != null && weapon != null)
            {
                weaponAction.AttemptToPerformAction(player, weapon);
            }
            else
            {
                
            }
        }

        [ClientRpc]
        public override void DestroyAllCurrentActionFXClientRpc()
        {
            if (player.characterEffectsManager.activeSpellWarmUpFX != null)
                Destroy(player.characterEffectsManager.activeSpellWarmUpFX);

            if (player.characterEffectsManager.activeDrawnProjectileFX != null)
                Destroy(player.characterEffectsManager.activeDrawnProjectileFX);

            if (player.characterEffectsManager.activeQuickSlotItemFX != null)
                Destroy(player.characterEffectsManager.activeQuickSlotItemFX);

            if (hasArrowNotched.Value)
            {
                //animate the bow
                Animator bowAnimator;

                if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
                {
                    bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
                }
                else
                {
                    bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
                }

                //animate the bow
                bowAnimator.SetBool("isDrawn", false);
                bowAnimator.Play("Bow_Fire_01");

                if (player.IsOwner)
                    hasArrowNotched.Value = false;
            }
        }

        //draw projectile
        [ServerRpc]
        public void NotifyServerOfDrawnProjectileServerRpc(int projectileID)
        {
            if (IsServer)
            {
                NotifyServerOfDrawnProjectileClientRpc(projectileID);
            }
        }

        [ClientRpc]
        private void NotifyServerOfDrawnProjectileClientRpc(int projectileID)
        {
            Animator bowAnimator;

            if (isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }

            //animate the bow
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_Draw_01");

            //instantiate the arrow
            GameObject arrow = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(projectileID).drawProjectileModel, player.playerEquipmentManager.leftHandWeaponSlot.transform);
            player.playerEffectsManager.activeDrawnProjectileFX = arrow;

            //play sfx
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.notchArrowSFX));
        }

        //release projectile
        [ServerRpc]
        public void NotifyServerOfReleasedProjectileServerRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (IsServer)
            {
                NotifyServerOfReleasedProjectileClientRpc(playerClientID, projectileID, xPosition, yPosition, zPosition, yCharacterRotation);
            }
        }

        [ClientRpc]
        public void NotifyServerOfReleasedProjectileClientRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (playerClientID != NetworkManager.Singleton.LocalClientId)
                PerformReleasedProjectileFromRpc(projectileID, xPosition, yPosition, zPosition, yCharacterRotation);
        }

        private void PerformReleasedProjectileFromRpc(int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            RangedProjectileItem projectileItem = null;

            //the projectile we are firing
            if (WorldItemDatabase.Instance.GetProjectileByID(projectileID) != null)
                projectileItem = WorldItemDatabase.Instance.GetProjectileByID(projectileID);

            if (projectileItem == null)
                return;

            Transform projectileInstantiateLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            //make and update arrow count UI
            projectileInstantiateLocation = player.playerCombatManager.lockOnTransform;
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiateLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            //make formula to set range projectile damage
            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = player;

            //fire an arrow based on 1 of 3 variations
            //1. locked onto a target

            //2.aiming
            if (player.playerNetworkManager.isAiming.Value)
            {
                projectileGameObject.transform.LookAt(new Vector3(xPosition, yPosition, zPosition));
            }
            else
            {
                //2. locked and not aiming
                if (player.playerCombatManager.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position
                        - projectileGameObject.transform.position);

                    projectileGameObject.transform.rotation = arrowRotation;
                }
                //3. unlocked and not aiming
                else
                {
                    player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, yCharacterRotation, player.transform.rotation.z);
                    Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);

                    projectileGameObject.transform.rotation = arrowRotation;
                }
            }


            //get all character colliders and ignore self
            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgnore = new List<Collider>();

            foreach (var item in characterColliders)
                collidersArrowWillIgnore.Add(item);

            foreach (Collider hitBox in collidersArrowWillIgnore)
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;
        }

        [ServerRpc]
        public void HideWeaponsServerRPC()
        {
            if (IsServer)
                HideWeaponsClientRPC();
        }

        [ClientRpc]
        private void HideWeaponsClientRPC()
        {
            if (player.playerEquipmentManager.rightHandWeaponModel != null)
                player.playerEquipmentManager.rightHandWeaponModel.SetActive(false);

            if (player.playerEquipmentManager.leftHandWeaponModel != null)
                player.playerEquipmentManager.leftHandWeaponModel.SetActive(false);
        }

        [ServerRpc]
        public void NotifyServerOfQuickSlotItemActionServerRpc(ulong clientID, int quickSlotItemID)
        {
            NotifyServerOfQuickSlotItemActionClientRpc(clientID, quickSlotItemID);
        }

        [ClientRpc]
        private void NotifyServerOfQuickSlotItemActionClientRpc(ulong clientID, int quickSlotItemID)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                QuickSlotItem item = WorldItemDatabase.Instance.GetQuickSlotItemByID(quickSlotItemID);
                item.AttemptToUseItem(player);
            }
        }

        [ServerRpc]
        public void SyncWeaponUpgradeServerRpc(int equipmentSlot, int upgradedItemID, int newUpgradeLevel)
        {
            ApplyWeaponUpgradeState((EquipmentType)equipmentSlot, upgradedItemID, newUpgradeLevel);
        }

        private void ApplyWeaponUpgradeState(EquipmentType equipmentSlot, int upgradedItemID, int newUpgradeLevel)
        {
            UpgradeLevel upgradedLevel = (UpgradeLevel)newUpgradeLevel;

            switch (equipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    UpdateWeaponUpgradeLevel(player.playerInventoryManager.weaponsInRightHandSlots[0], upgradedItemID, upgradedLevel);
                    break;
                case EquipmentType.RightWeapon02:
                    UpdateWeaponUpgradeLevel(player.playerInventoryManager.weaponsInRightHandSlots[1], upgradedItemID, upgradedLevel);
                    break;
                case EquipmentType.RightWeapon03:
                    UpdateWeaponUpgradeLevel(player.playerInventoryManager.weaponsInRightHandSlots[2], upgradedItemID, upgradedLevel);
                    break;
                case EquipmentType.LeftWeapon01:
                    UpdateWeaponUpgradeLevel(player.playerInventoryManager.weaponsInLeftHandSlots[0], upgradedItemID, upgradedLevel);
                    break;
                case EquipmentType.LeftWeapon02:
                    UpdateWeaponUpgradeLevel(player.playerInventoryManager.weaponsInLeftHandSlots[1], upgradedItemID, upgradedLevel);
                    break;
                case EquipmentType.LeftWeapon03:
                    UpdateWeaponUpgradeLevel(player.playerInventoryManager.weaponsInLeftHandSlots[2], upgradedItemID, upgradedLevel);
                    break;
                default:
                    break;
            }

            UpdateWeaponUpgradeLevel(player.playerInventoryManager.currentRightHandWeapon, upgradedItemID, upgradedLevel);
            UpdateWeaponUpgradeLevel(player.playerInventoryManager.currentLeftHandWeapon, upgradedItemID, upgradedLevel);
            UpdateWeaponUpgradeLevel(player.playerInventoryManager.currentTwoHandWeapon, upgradedItemID, upgradedLevel);
            UpdateWeaponUpgradeLevel(player.playerCombatManager.currentWeaponBeingUsed, upgradedItemID, upgradedLevel);

            if (player.playerEquipmentManager != null)
                player.playerEquipmentManager.RefreshWeaponDamage();
        }

        private void UpdateWeaponUpgradeLevel(WeaponItem weapon, int upgradedItemID, UpgradeLevel upgradedLevel)
        {
            if (weapon == null || weapon.itemID != upgradedItemID)
                return;

            weapon.upgradeLevel = upgradedLevel;
        }
    }
}
