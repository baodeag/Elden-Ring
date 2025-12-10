using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace baodeag
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Previous Poise Damage Taken")]
        public float previousPoiseDamageTaken;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Attack Flags")]
        public bool canPerformRollingAttack = false;
        public bool canPerformBackstepAttack = false;
        public bool canBlock = true;
        public bool canBeBackstabbed = true;

        [Header("Critical Attack")]
        private Transform riposteReceiverTransform;
        private Transform backstabReceiverTransform;
        [SerializeField] float criticalAttackDistanceCheck = 0.7f;
        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (character.IsOwner)
            {
                if (newTarget != null)
                {
                    currentTarget = newTarget;
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }
        }

        //used to attempt critical attacks like backstabs and ripostes
        public virtual void AttemptCriticalAttack()
        {
            //we can't perform a critical attack if we're already performing an action
            if (character.isPerformingAction)
                return;

            //we can't perform a critical attack if we don't have enough stamina
            if (character.characterNetworkManager.currentStamina.Value <= 0)
                return;

            //aim a raycast from our lock on transform forward to see if we hit a ripostable or backstabable enemy
            RaycastHit[] hits = Physics.RaycastAll(character.characterCombatManager.lockOnTransform.position, 
                character.transform.TransformDirection(Vector3.forward), criticalAttackDistanceCheck, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i=0; i <hits.Length; i++)
            {
                //check each of the hits 1 by 1, giving them thier own variable
                RaycastHit hit = hits[i];

                CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

                if (targetCharacter != null)
                {
                    //if the character is the one attempting the critical attack, go to the next hit in the array of totals hit
                    if (targetCharacter == character)
                        continue;

                    if (!WorldUtilityManager.Instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                        continue;

                    Vector3 directionFromCharacterToTarget = character.transform.position - targetCharacter.transform.position;
                    float targetViewableAngle = Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                    if (targetCharacter.characterNetworkManager.isRipostable.Value)
                    {
                        if (targetViewableAngle >= -60 && targetViewableAngle <= 60)
                        {
                            //we can riposte this target
                            AttemptRiposte(hit);
                            return;
                        }
                    }

                    if (targetCharacter.characterCombatManager.canBeBackstabbed)
                    {
                        if (targetViewableAngle <= 180 && targetViewableAngle >=145)
                        {
                            //we can backstab this target
                            AttemptBackstab(hit);
                            return;
                        }
                        if (targetViewableAngle >= -180 && targetViewableAngle <= -145)
                        {
                            //we can backstab this target
                            AttemptBackstab(hit);
                            return;
                        }
                    }
                }
            }
        }

        public virtual void AttemptRiposte(RaycastHit hit)
        {
            
        }

        public virtual void AttemptBackstab(RaycastHit hit)
        {

        }

        public virtual void ApplyCriticalDamage()
        {
            character.characterEffectsManager.PlayCriticalBloodSplatterVFX(character.characterCombatManager.lockOnTransform.position);
            character.characterSoundFXManager.PlayCriticalStrikeSoundFX();

            if (character.IsOwner)
                character.characterNetworkManager.currentHealth.Value -= pendingCriticalDamage;
        }

        public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter, Vector3 ripostePosition)
        {
            float timer = 0;

            while (timer < 0.5f)
            {
                timer += Time.deltaTime;

                if (riposteReceiverTransform == null)
                {
                    GameObject riposteTransformObject = new GameObject("Riposte Transform");
                    riposteTransformObject.transform.parent = transform;
                    riposteTransformObject.transform.position = Vector3.zero;
                    riposteReceiverTransform = riposteTransformObject.transform;
                }

                riposteReceiverTransform.localPosition = ripostePosition;
                enemyCharacter.transform.position = riposteReceiverTransform.position;
                transform.rotation = Quaternion.LookRotation(-enemyCharacter.transform.forward);
                yield return null;
            }
        }

        public IEnumerator ForceMoveEnemyCharacterToBackstabPosition(CharacterManager enemyCharacter, Vector3 backstabPosition)
        {
            float timer = 0;

            while (timer < 0.5f)
            {
                timer += Time.deltaTime;

                if (riposteReceiverTransform == null)
                {
                    GameObject backstabTransformObject = new GameObject("Backstab Transform");
                    backstabTransformObject.transform.parent = transform;
                    backstabTransformObject.transform.position = Vector3.zero;
                    backstabReceiverTransform = backstabTransformObject.transform;
                }

                backstabReceiverTransform.localPosition = backstabPosition;
                enemyCharacter.transform.position = backstabReceiverTransform.position;
                transform.rotation = Quaternion.LookRotation(enemyCharacter.transform.forward);
                yield return null;
            }
        }

        public void EnableIsInvulnerable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;
        }

        public void DisableIsInvulnerable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = false;
        }

        public void EnableIsParrying()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isParrying.Value = true;
        }

        public void DisableIsParrying()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isParrying.Value = false;
        }

        public void EnableIsRipostable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isRipostable.Value = true;
        }

        public void EnableCanDoRollingAttack()
        {
            canPerformRollingAttack = true;
        }

        public void DisableCanDoRollingAttack()
        {
            canPerformRollingAttack = false;
        }

        public void EnableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = true;
        }

        public void DisableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = false;
        }

        public virtual void EnableCanDoCombo()
        {

        }

        public virtual void DisableCanDoCombo()
        {

        }

        public virtual void CloseAllDamageColliders()
        {

        }
    }
}
