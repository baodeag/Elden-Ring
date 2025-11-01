using Unity.Collections;
using Unity.Netcode;
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
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(
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
            Debug.Log("Vitality changed from " + oldVitality + " to " + newVitality + ", updating max health");
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

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadRightWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newId);
            }
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newId);
            }
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newId));
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

            if (player.IsOwner)
                return;

            if (player.playerCombatManager.currentWeaponBeingUsed != null)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
        }

        public override void OnIsBlockingChanged(bool oldStatus, bool newStarus)
        {
            base.OnIsBlockingChanged(oldStatus, newStarus);

            if (IsOwner)
            {
                player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsorption;
                player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
                player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsorption;
                player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsorption;
            }
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
    }
}
