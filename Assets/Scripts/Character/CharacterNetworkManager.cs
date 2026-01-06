using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>
            (true,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Position")]
        // if you owner this object, you can write to this variable
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>
            (Vector3.zero, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>
            (Quaternion.identity,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime =0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<bool> isMoving = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>
            (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>
            (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new NetworkVariable<float>
            (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Target")]
        public NetworkVariable<ulong> currentTargetNetworkObjectID = new NetworkVariable<ulong>
            (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Flag")]
        public NetworkVariable<bool> isBlocking = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParrying = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParryable = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isAttacking = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isInvulnerable = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isLockedOn = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isJumping = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingAttack = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isRipostable = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isBeingCriticallyDamaged = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);


        [Header("Resources")]
        public NetworkVariable<int> currentHealth = new NetworkVariable<int>
            (400,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new NetworkVariable<int>
            (400,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>
            (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>
            (0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentFocusPoints = new NetworkVariable<int>
            (400,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxFocusPoints = new NetworkVariable<int>
            (400,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> vitality = new NetworkVariable<int>
            (1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> endurance = new NetworkVariable<int>
            (1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> mind = new NetworkVariable<int>
            (1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> strength = new NetworkVariable<int>
            (1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [Header("Stats Modifiers")]
        public NetworkVariable<int> strengthModifier = new NetworkVariable<int>
            (1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);



        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void CheckHP(int oldValue, int newValue)
        {
            if(currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());
            }

            if(character.IsOwner)
            {
                if(currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }

        public virtual void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isDead", character.isDead.Value);
        }

        public void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            if (!IsOwner)
            {
                character.characterCombatManager.currentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();

            }
        }

        public void OnIsLockedOnChanged(bool old, bool isLockedOn)
        {
            if (!isLockedOn)
            {
                character.characterCombatManager.currentTarget = null;
            }
        }

        public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isChargingAttack", isChargingAttack.Value);
        }

        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isMoving", isMoving.Value);
        }

        public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            gameObject.SetActive(isActive.Value);
        }

        public virtual void OnIsBlockingChanged(bool oldStatus, bool newStarus)
        {
            character.animator.SetBool("isBlocking", isBlocking.Value);
        }

        //used to cancel fx when poise is broken
        [ServerRpc]
        public void DestroyAllCurrentActionFXServerRpc()
        {
            if (IsServer)
            {
                DestroyAllCurrentActionFXClientRpc();
            }
        }

        [ClientRpc]
        public virtual void DestroyAllCurrentActionFXClientRpc()
        {
            if (character.characterEffectsManager.activeSpellWarmUpFX != null)
                Destroy(character.characterEffectsManager.activeSpellWarmUpFX);

            if (character.characterEffectsManager.activeDrawnProjectileFX != null)
                Destroy(character.characterEffectsManager.activeDrawnProjectileFX);

            if (character.characterEffectsManager.activeQuickSlotItemFX != null)
                Destroy(character.characterEffectsManager.activeQuickSlotItemFX);
        }

        //A server rpc is a function called from a client, to the server(in our case the host)
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //if this char is the host/server, then activate the client rpc
            if(IsServer)
            {
                //tell all clients to play the animation
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        //a client rpc is sent to all clients present, from the server/host
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //we make sure to not run the function on the char who sent it(so we dont play an animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        [ServerRpc]
        public void NotifyTheServerOfInstantActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //if this char is the host/server, then activate the client rpc
            if (IsServer)
            {
                //tell all clients to play the animation
                PlayInstantActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        //a client rpc is sent to all clients present, from the server/host
        [ClientRpc]
        public void PlayInstantActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //we make sure to not run the function on the char who sent it(so we dont play an animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformInstantActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformInstantActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.Play(animationID);
        }

        //attack animation

        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //if this char is the host/server, then activate the client rpc
            if (IsServer)
            {
                //tell all clients to play the animation
                PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //we make sure to not run the function on the char who sent it(so we dont play an animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        //damage
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfCharacterDamageServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            if (IsServer)
            {
                NotifyTheServerOfCharacterDamageClientRpc(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);

            }
        }

        [ClientRpc]
        public void NotifyTheServerOfCharacterDamageClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            ProcessCharacterDamageFromServer(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);

        }

        public void ProcessCharacterDamageFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        //critical damage (riposte)
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfRiposteServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage)
        {
            if (IsServer)
            {
                NotifyTheServerOfRiposteClientRpc(damagedCharacterID, 
                    characterCausingDamageID, 
                    criticalDamageAnimation, 
                    weaponID,
                    physicalDamage, 
                    magicDamage, 
                    fireDamage, 
                    lightningDamage, 
                    holyDamage, 
                    poiseDamage);

            }
        }

        [ClientRpc]
        public void NotifyTheServerOfRiposteClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage)
        {
            ProcessRiposteFromServer(damagedCharacterID, 
                characterCausingDamageID,
                criticalDamageAnimation,
                weaponID,
                physicalDamage, 
                magicDamage, 
                fireDamage, 
                lightningDamage, 
                holyDamage, 
                poiseDamage);

        }

        public void ProcessRiposteFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            WeaponItem weapon= WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value = true;

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);

            //move the enemy to the proper ripost position
            StartCoroutine(damagedCharacter.characterCombatManager.ForceMoveEnemyCharacterToRipostePosition(
                characterCausingDamage, WorldUtilityManager.Instance.GetRipostingPositionBasedOnWeaponClass(weapon.weaponClass)));
        }

        //critical damage (backstab)
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfBackstabServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage)
        {
            if (IsServer)
            {
                NotifyTheServerOfBackstabClientRpc(damagedCharacterID,
                    characterCausingDamageID,
                    criticalDamageAnimation,
                    weaponID,
                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    lightningDamage,
                    holyDamage,
                    poiseDamage);

            }
        }

        [ClientRpc]
        public void NotifyTheServerOfBackstabClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage)
        {
            ProcessBackstabFromServer(damagedCharacterID,
                characterCausingDamageID,
                criticalDamageAnimation,
                weaponID,
                physicalDamage,
                magicDamage,
                fireDamage,
                lightningDamage,
                holyDamage,
                poiseDamage);

        }

        public void ProcessBackstabFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poiseDamage)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value = true;

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
            damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);

            //move the enemy to the proper backstab position
            StartCoroutine(characterCausingDamage.characterCombatManager.ForceMoveEnemyCharacterToBackstabPosition(
                damagedCharacter, WorldUtilityManager.Instance.GetBackstabPositionBasedOnWeaponClass(weapon.weaponClass)));
        }

        //parry
        [ServerRpc(RequireOwnership = false)]
        public void NotifyServerOfParryServerRpc(ulong parriedClientID)
        {
            if (IsServer)
            {
                NotifyServerOfParryClientRpc(parriedClientID);
            }
        }

        [ClientRpc]
        protected void NotifyServerOfParryClientRpc(ulong parriedClientID)
        {
            ProcessParryFromServer(parriedClientID);
        }

        protected void ProcessParryFromServer(ulong parriedClient)
        {
            CharacterManager parriedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[parriedClient].gameObject.GetComponent<CharacterManager>();

            if (parriedCharacter == null)
                return;

            //close all damage colliders on the parried character
            if (parriedCharacter.IsOwner)
            {
                parriedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parried_01", true);
            }
        }
    }

}
