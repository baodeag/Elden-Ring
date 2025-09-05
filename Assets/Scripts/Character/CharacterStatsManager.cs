using System.Globalization;
using UnityEngine;

namespace baodeag
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;
        public float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }
        public virtual void RegenerateStamina()
        {
            //only owners can edit their network variables
            if (!character.IsOwner)
                return;

            //we dont want to regen stamina if we are performing an action or sprinting
            if (character.characterNetworkManager.isSprinting.Value)
                return;

            if (character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            //we only want to reset the timer if the action used stamina
            //we dont want to reset it if stamina is being regenerated
            if (currentStaminaAmount < previousStaminaAmount)
            { 
                staminaRegenerationTimer = 0;  
            }
        }
    }
}
