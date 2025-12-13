using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Items/Spells/Fire Ball")]
    public class FireBallSpell : SpellItem
    {
        [Header("Projectile Velocity")]
        [SerializeField] float upwardVelocity = 3;
        [SerializeField] float forwardVelocity = 15;
        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);

            if (!CanICastThisSpell(player))
                return;

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);
            }
        }

        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellCastWarmUpFX);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiatedWarmUpSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedWarmUpSpellFX.transform.localPosition = Vector3.zero;
            instantiatedWarmUpSpellFX.transform.localRotation = Quaternion.identity;

            player.playerEffectsManager.activeSpellWarmUpFX = instantiatedWarmUpSpellFX;

        }
        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            //destroy any warm up fx remaining from spell
            if (player.IsOwner)
                player.playerCombatManager.DestroyAllCurrentActionFX();

            //get any colliders from the caster
            Collider[] characterColiders = player.GetComponentsInChildren<Collider>();
            Collider characterCollisionCollider = player.GetComponent<Collider>();

            //instantiate spell projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleasedSpellFX = Instantiate(spellCastReleaseFX);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            //use the list of colliders from the caster and now apply the ignore physics with the colliders from the projectile
            instantiatedReleasedSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleasedSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleasedSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleasedSpellFX.transform.parent = null;

            //set the projectile velocity and direction
            

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleasedSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiatedReleasedSpellFX.transform.forward = forwardDirection;
            }

            Rigidbody spellRigidBody = instantiatedReleasedSpellFX.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedReleasedSpellFX.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedReleasedSpellFX.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            spellRigidBody.linearVelocity = totalVelocity;
        }

        public override bool CanICastThisSpell(PlayerManager player)
        {
            if (player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isJumping.Value)
                return false;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return false;


            return true;
        }
    }
}
