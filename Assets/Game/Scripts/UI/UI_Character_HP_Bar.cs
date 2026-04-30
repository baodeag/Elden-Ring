using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace baodeag
{
    public class UI_Character_HP_Bar : UI_StatBar
    {
        private CharacterManager character;
        private AICharacterManager aiCharacter;
        private PlayerManager playerCharacter;

        [SerializeField] bool displayCharacterNameOnDamage = false;
        [SerializeField] float defaultTimeBeforeBarHides = 3;
        [SerializeField] float hideTimer = 0;
        public int currentDamageTaken = 0;
        [SerializeField] TextMeshProUGUI characterName;
        [SerializeField] TextMeshProUGUI characterDamage;
        [HideInInspector] public int oldHealthValue = 0;

        protected override void Awake()
        {
            base.Awake();

            character = GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                aiCharacter = character as AICharacterManager;
                playerCharacter = character as PlayerManager;
            }
        }

        private bool EnsureUiReferences()
        {
            if (slider == null)
                slider = GetComponent<Slider>();

            if (barFillImage == null && slider != null && slider.fillRect != null)
                barFillImage = slider.fillRect.GetComponent<Image>();

            if (barFillImage != null && barFillColor.a <= 0f)
                barFillColor = barFillImage.color;

            return character != null && character.characterNetworkManager != null && slider != null;
        }

        protected override void Start()
        {
            base.Start();

            gameObject.SetActive(false);
        }

        public override void SetStat(int newValue)
        {
            if (!EnsureUiReferences())
                return;

            if (character.characterNetworkManager.isBurning.Value)
            {
                if (barFillImage != null)
                    barFillImage.color = WorldUtilityManager.Instance.GetBurningColor();
            }
            else if (character.characterNetworkManager.isPoisoned.Value)
            {
                if (barFillImage != null)
                    barFillImage.color = WorldUtilityManager.Instance.GetPoisonedColor();
            }
            else
            {
                if (barFillImage != null)
                    barFillImage.color = barFillColor;
            }

            if (displayCharacterNameOnDamage && characterName != null)
            {
                characterName.enabled = true;

                if (aiCharacter != null)
                    characterName.text = aiCharacter.characterName;

                if (playerCharacter != null)
                    characterName.text = playerCharacter.playerNetworkManager.characterName.Value.ToString();
            }

            //call this here incase max health changes from buffs/debuffs
            slider.maxValue = character.characterNetworkManager.maxHealth.Value;

            //total the dmg taken whilst the bar is active
            currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHealthValue - newValue));

            if (currentDamageTaken < 0)
            {
                currentDamageTaken = Mathf.Abs(currentDamageTaken);
                if (characterDamage != null)
                    characterDamage.text = "+ " + currentDamageTaken.ToString();
            }
            else
            {
                if (characterDamage != null)
                    characterDamage.text = "- " + currentDamageTaken.ToString();
            }

            slider.value = newValue;

            if (character.characterNetworkManager.currentHealth.Value != character.characterNetworkManager.maxHealth.Value)
            {
                hideTimer = defaultTimeBeforeBarHides;
                gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (Camera.main != null)
                transform.LookAt(transform.position + Camera.main.transform.forward);

            if (hideTimer > 0)
            {
                hideTimer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            currentDamageTaken = 0;
        }
    }
}
