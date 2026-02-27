using System.Globalization;
using UnityEngine;

namespace baodeag
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Runes")]
        public int runesDroppedOnDeath = 50;

        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalAbsorption;
        public float blockingFireAbsorption;
        public float blockingLightningAbsorption;
        public float blockingMagicAbsorption;
        public float blockingHolyAbsorption;
        public float blockingStability;

        [Header("Armor Absorption")]
        public float armorPhysicalDamageAbsorption;
        public float armorMagicDamageAbsorption;
        public float armorFireDamageAbsorption;
        public float armorLightningDamageAbsorption;
        public float armorHolyDamageAbsorption;

        [Header("Armor Resistances")]
        public float armorImmunity; //resistance to rot and poison
        public float armorRobustness; // bleed and frost
        public float armorFocus; // madness and sleep
        public float armorVitality; // death curse

        [Header("Poise")]
        public float totalPoiseDamage; //how much poise damage this character have taken
        public float offensivePoiseBonus; //the poise bonus gained from using weapons
        public float basePoiseDefense; //the poise bonus gained from armor
        public float defaultPoiseResetTime = 8; //the time it takes for poise damage to reset
        public float poiseResetTimer = 0; //the current timer for poise reset

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            health = vitality * 15;

            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public int CalculateFocusPointsBasedOnMindLevel(int mind)
        {
            int focusPoints = 0;
            focusPoints = mind * 10;
            return Mathf.RoundToInt(focusPoints);
        }

        public int CalculateCharacterLevelBasedOnAttributes()
        {
            int totalAtributes = character.characterNetworkManager.vigor.Value +
                character.characterNetworkManager.endurance.Value +
                character.characterNetworkManager.mind.Value +
                character.characterNetworkManager.strength.Value +
                character.characterNetworkManager.dexterity.Value +
                character.characterNetworkManager.intelligence.Value +
                character.characterNetworkManager.faith.Value;

            int characterLevel = totalAtributes - 70 + 1;

            if (characterLevel < 1) 
                characterLevel = 1;

            return characterLevel;
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

        protected virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                totalPoiseDamage = 0;
            }
        }
    }
}
