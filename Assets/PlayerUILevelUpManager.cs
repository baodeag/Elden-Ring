using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace baodeag
{
    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        [Header("Levels")]
        [SerializeField] int[] playerLevels = new int[100];
        [SerializeField] int baseLevelCost = 83;
        [SerializeField] int totalLevelUpCost = 0;

        [Header("Character Stats")]
        [SerializeField] TextMeshProUGUI characterLevelText;
        [SerializeField] TextMeshProUGUI runesHeldText;
        [SerializeField] TextMeshProUGUI runesNeededText;
        [SerializeField] TextMeshProUGUI vigorLevelText;
        [SerializeField] TextMeshProUGUI mindLevelText;
        [SerializeField] TextMeshProUGUI enduranceLevelText;
        [SerializeField] TextMeshProUGUI strengthLevelText;
        [SerializeField] TextMeshProUGUI dexterityLevelText;
        [SerializeField] TextMeshProUGUI intelligenceLevelText;
        [SerializeField] TextMeshProUGUI faithLevelText;

        [Header("Projected Character Stats")]
        [SerializeField] TextMeshProUGUI projectedCharacterLevelText;
        [SerializeField] TextMeshProUGUI projectedRunesHeldText;
        [SerializeField] TextMeshProUGUI projectedVigorLevelText;
        [SerializeField] TextMeshProUGUI projectedMindLevelText;
        [SerializeField] TextMeshProUGUI projectedEnduranceLevelText;
        [SerializeField] TextMeshProUGUI projectedStrengthLevelText;
        [SerializeField] TextMeshProUGUI projectedDexterityLevelText;
        [SerializeField] TextMeshProUGUI projectedIntelligenceLevelText;
        [SerializeField] TextMeshProUGUI projectedFaithLevelText;

        [Header("Sliders")]
        public CharacterAttribute currentSelectedAttribute;
        public Slider vigorSlider;
        public Slider mindSlider;
        public Slider enduranceSlider;
        public Slider strengthSlider;
        public Slider dexteritySlider;
        public Slider intelligenceSlider;
        public Slider faithSlider;

        [Header("Button")]
        [SerializeField] Button confirmLevelsButton;

        private void Awake()
        {
            SetAllLevelsCost();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();

            SetCurrentStats();
        }

        //this is called when opening the menu
        private void SetCurrentStats()
        {
            //character level
            characterLevelText.text = PlayerUIManager.instance.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();
            projectedCharacterLevelText.text = PlayerUIManager.instance.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();

            //runes held
            runesHeldText.text = PlayerUIManager.instance.localPlayer.playerStatsManager.runes.ToString();
            projectedRunesHeldText.text = PlayerUIManager.instance.localPlayer.playerStatsManager.runes.ToString();
            runesNeededText.text = "0";

            //attributes
            vigorLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.vigor.Value.ToString();
            projectedVigorLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.vigor.Value.ToString();
            vigorSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.vigor.Value;

            mindLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.mind.Value.ToString();
            projectedMindLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.mind.Value.ToString();
            mindSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.mind.Value;

            enduranceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.endurance.Value.ToString();
            projectedEnduranceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.endurance.Value.ToString();
            enduranceSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.endurance.Value;

            strengthLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.strength.Value.ToString();
            projectedStrengthLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.strength.Value.ToString();
            strengthSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.strength.Value;

            dexterityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.dexterity.Value.ToString();
            projectedDexterityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.dexterity.Value.ToString();
            dexteritySlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.dexterity.Value;

            intelligenceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.intelligence.Value.ToString();
            projectedIntelligenceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.intelligence.Value.ToString();
            intelligenceSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.intelligence.Value;

            faithLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.faith.Value.ToString();
            projectedFaithLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.faith.Value.ToString();
            faithSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.faith.Value;

            vigorSlider.Select();
            vigorSlider.OnSelect(null);
        }

        //this is called everytime a level slider is changed
        public void UpdateSliderBasedOnCurrentlySelectedAttributes()
        {
            PlayerManager player = PlayerUIManager.instance.localPlayer;

            switch (currentSelectedAttribute)
            {
                case CharacterAttribute.Vigor:
                    projectedVigorLevelText.text = vigorSlider.value.ToString();
                    break;
                case CharacterAttribute.Mind:
                    projectedMindLevelText.text = mindSlider.value.ToString();
                    break;
                case CharacterAttribute.Endurance:
                    projectedEnduranceLevelText.text = enduranceSlider.value.ToString();
                    break;
                case CharacterAttribute.Strength:
                    projectedStrengthLevelText.text = strengthSlider.value.ToString();
                    break;
                case CharacterAttribute.Dexterity:
                    projectedDexterityLevelText.text = dexteritySlider.value.ToString();
                    break;
                case CharacterAttribute.Intelligence:
                    projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
                    break;
                case CharacterAttribute.Faith:
                    projectedFaithLevelText.text = faithSlider.value.ToString();
                    break;
                default:
                    break;
            }

            //pass our current level and our projected level to set our cost for leveling up
            CalculateLevelCost(
                player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(), 
                player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true));

            projectedCharacterLevelText.text = player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true).ToString();
            runesNeededText.text = totalLevelUpCost.ToString();

            //check cost
            if (totalLevelUpCost > player.playerStatsManager.runes)
            {
                //disable confirm buuton so player cant level up
                confirmLevelsButton.interactable = false;
            }
            else
            {
                confirmLevelsButton.interactable = true;
            }
        }

        public void ConfirmLevels()
        {
            PlayerManager player = PlayerUIManager.instance.localPlayer;

            //deduct cost from total runes
            player.playerStatsManager.runes -= totalLevelUpCost;

            //set new stats
            player.playerNetworkManager.vigor.Value = Mathf.RoundToInt(vigorSlider.value);
            player.playerNetworkManager.mind.Value = Mathf.RoundToInt(mindSlider.value);
            player.playerNetworkManager.endurance.Value = Mathf.RoundToInt(enduranceSlider.value);
            player.playerNetworkManager.strength.Value = Mathf.RoundToInt(strengthSlider.value);
            player.playerNetworkManager.dexterity.Value = Mathf.RoundToInt(dexteritySlider.value);
            player.playerNetworkManager.intelligence.Value = Mathf.RoundToInt(intelligenceSlider.value);
            player.playerNetworkManager.faith.Value = Mathf.RoundToInt(faithSlider.value);

            SetCurrentStats();
        }

        private void SetAllLevelsCost()
        {
            for (int i = 0; i < playerLevels.Length; i++)
            {
                if (i == 0)
                    continue;

                playerLevels[i] = baseLevelCost + (50 * i);
            }
        }

        private void CalculateLevelCost(int currentLevel, int projectedLevel)
        {
            //dont want to charge for levels that the player already has, so skip those
            //ex, if the player is currently level 10 and they want to go to level 12, only want to charge for levels 11 and 12, not 1-10
            int totalCost = 0;

            for (int i = 0; i < projectedLevel; i++)
            {
                //dont charge until get past current level
                if (i < currentLevel)
                    continue;

                totalCost += playerLevels[i];
            }

            totalLevelUpCost = totalCost;

            projectedRunesHeldText.text = (PlayerUIManager.instance.localPlayer.playerStatsManager.runes - totalCost).ToString();

            if (totalCost > PlayerUIManager.instance.localPlayer.playerStatsManager.runes)
            {
                projectedRunesHeldText.color = Color.red;
            }
            else
            {
                projectedRunesHeldText.color = Color.white;
            }
        }
    }
}
