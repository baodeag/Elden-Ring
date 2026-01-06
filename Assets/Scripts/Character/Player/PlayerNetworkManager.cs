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

        [Header("Actions")]
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(
            false,
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
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

        public void SetNewMaxFocusPointsValue(int oldMind, int newMind)
        {
            maxFocusPoints.Value = player.playerStatsManager.CalculateFocusPointsBasedOnMindLevel(newMind);
            PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointValue(maxFocusPoints.Value);
            currentFocusPoints.Value = maxFocusPoints.Value;
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadRightWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);
            }
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);
            }
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

            if (player.IsOwner)
                return;

            if (player.playerCombatManager.currentWeaponBeingUsed != null)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
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

            if (WorldItemDatabase.Instance.GetQuickSlotItemByID(newID))
                newQuickSlotItem = Instantiate(WorldItemDatabase.Instance.GetQuickSlotItemByID(newID));

            if (newQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem = newQuickSlotItem;

                if (player.IsOwner)
                    PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(newID);
            }
        }

        public void OnMainProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.mainProjectile = newProjectile;
        }

        public void OnSecondaryProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.secondaryProjectile = newProjectile;
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
                if (IsOwner)
                {
                    isTwoHandingLeftWeapon.Value = false;
                    isTwoHandingRightWeapon.Value = false;
                }

                player.playerEquipmentManager.UnTwoHandWeapon();
                player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.instance.twoHandingEffect.staticEffectID);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.instance.twoHandingEffect);
                player.playerEffectsManager.AddStaticEffect(twoHandEffect);
            }

            player.animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon.Value);
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

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(player, WorldItemDatabase.Instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action is null, cant be performed");
            }
        }

        [ClientRpc]
        public override void DestroyAllCurrentActionFXClientRpc()
        {
            base.DestroyAllCurrentActionFXClientRpc();

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
                projectileItem = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(projectileID));

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
    }
}
