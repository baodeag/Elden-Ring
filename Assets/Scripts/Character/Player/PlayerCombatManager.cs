using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;

namespace baodeag
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;
        public ProjectileSlot currentProjectileBeingUsed;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = false;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
            }           
        }

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }

        //critical attacks
        public override void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            //if for some reason the target character is null, return
            if (targetCharacter == null)
                return;

            //if some how since the initial check the character can no longer be riposted, return
            if (!targetCharacter.characterNetworkManager.isRipostable.Value)
                return;

            //if somebody else is already performing a critical strike on the character, return
            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            MeleeWeaponItem riposteWeapon;
            MeleeWeaponDamageCollider riposteCollider;

            //check if we are two handing left weapon or right weapon

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                riposteWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

            //whilist performing a critical strike, you cannot be damaged
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            damageEffect.physicalDamage = riposteCollider.physicalDamage;
            damageEffect.holyDamage = riposteCollider.holyDamage;
            damageEffect.fireDamage = riposteCollider.fireDamage;
            damageEffect.lightningDamage = riposteCollider.lightningDamage;
            damageEffect.magicDamage = riposteCollider.magicDamage;
            damageEffect.poiseDamage = riposteCollider.poiseDamage;

            damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.lightningDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifier;

            targetCharacter.characterNetworkManager.NotifyTheServerOfRiposteServerRpc(
                targetCharacter.NetworkObjectId,
                character.NetworkObjectId, 
                "Riposted_01",
                riposteWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.holyDamage,
                damageEffect.fireDamage,
                damageEffect.lightningDamage,
                damageEffect.magicDamage,
                damageEffect.poiseDamage);
        }

        public override void AttemptBackstab(RaycastHit hit)
        {
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            //if for some reason the target character is null, return
            if (targetCharacter == null)
                return;

            //if some how since the initial check the character can no longer be riposted, return
            if (!targetCharacter.characterCombatManager.canBeBackstabbed)
                return;

            //if somebody else is already performing a critical strike on the character, return
            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            MeleeWeaponItem backstabWeapon;
            MeleeWeaponDamageCollider backstabCollider;

            //check if we are two handing left weapon or right weapon

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                backstabWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                backstabCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                backstabWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                backstabCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Backstab_01", true);

            //whilist performing a critical strike, you cannot be damaged
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            damageEffect.physicalDamage = backstabCollider.physicalDamage;
            damageEffect.holyDamage = backstabCollider.holyDamage;
            damageEffect.fireDamage = backstabCollider.fireDamage;
            damageEffect.lightningDamage = backstabCollider.lightningDamage;
            damageEffect.magicDamage = backstabCollider.magicDamage;
            damageEffect.poiseDamage = backstabCollider.poiseDamage;

            damageEffect.physicalDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.holyDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.fireDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.lightningDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.magicDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.poiseDamage *= backstabWeapon.backstab_Attack_01_Modifier;

            targetCharacter.characterNetworkManager.NotifyTheServerOfBackstabServerRpc(
                targetCharacter.NetworkObjectId,
                character.NetworkObjectId,
                "Backstabbed_01",
                backstabWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.holyDamage,
                damageEffect.fireDamage,
                damageEffect.lightningDamage,
                damageEffect.magicDamage,
                damageEffect.poiseDamage);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
                return;

            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducted = 0;

            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightJumpingAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyJumpingAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            Debug.Log("Stamina Deducted: " + staminaDeducted);
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if (player.IsOwner)
            {
                PlayerCamera.instance.SetLockCameraHeight();
            }
        }

        //animation event call
        public override void EnableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {

            }
        }

        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.canComboWithMainHandWeapon = false;

        }

        //projectile
        public void ReleaseArrow()
        {
            if (player.IsOwner)
                player.playerNetworkManager.hasArrowNotched.Value = false;

            //destroy warm up projectile 
            if (player.playerEffectsManager.activeDrawnProjectileFX != null)
                Destroy(player.playerEffectsManager.activeDrawnProjectileFX);

            //play release arrow sfx
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.releaseArrowSFX));

            //animate the bow
            Animator bowAnimator;

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponSlot.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponSlot.GetComponentInChildren<Animator>();
            }

            //animate the bow
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_Fire_01");

            if (!player.IsOwner)
                return;

            //the projectile we are firing
            RangedProjectileItem projectileItem = null;

            switch (currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    projectileItem = player.playerInventoryManager.mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = player.playerInventoryManager.secondaryProjectile;
                    break;
                default:
                    break;
            }

            if (projectileItem == null)
                return;

            if (projectileItem.currentAmmoAmount <= 0)
                return;

            Transform projectileInstantiateLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            //substract ammo
            projectileItem.currentAmmoAmount -= 1;
            
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
            if (player.playerCombatManager.currentTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position 
                    - projectileGameObject.transform.position);

                projectileGameObject.transform.rotation = arrowRotation;
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

        //spell
        public void InstantiateSpellWarmUpFX()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.InstantiateWarmUpSpellFX(player);
        }

        public void SuccessfullyCastSpell()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpell(player);
        }

        public void SuccessfullyChargeSpell()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyChargeSpell(player);
        }

        public void SuccessfullyCastSpellFullCharge()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpellFullCharge(player);
        }

        public WeaponItem SelectWeaponToPerformAshOfWar()
        {
            WeaponItem selectedWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerNetworkManager.SetCharacterActionHand(false);
            player.playerNetworkManager.currentWeaponBeingUsed.Value = selectedWeapon.itemID;
            return selectedWeapon;
        } 
    }
}
