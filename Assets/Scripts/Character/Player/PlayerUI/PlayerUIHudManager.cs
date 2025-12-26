using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace baodeag
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup[] canvasGroup;

        [Header("Stat Bars")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar focusPointBar;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;
        [SerializeField] Image spellItemQuickSlotIcon;

        [Header("Boss Health Bar")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;

        [Header("Crosshair")]
        public GameObject crossHair;

        public void ToggleHUD(bool status)
        {
            //to do fade in and out over time
            if (status)
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 1;
                }
            }
            else
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 0;
                }
            }
        }

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
            focusPointBar.gameObject.SetActive(false);
            focusPointBar.gameObject.SetActive(true);
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        public void SetNewFocusPointValue(int oldValue, int newValue)
        {
            focusPointBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxFocusPointValue(int maxFocusPoints)
        {
            focusPointBar.SetMaxStat(maxFocusPoints);
        }

        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

            if (weapon == null)
            {
                Debug.Log("Item is null");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.Log("Item has no icon");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }
        
        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is null");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.Log("Item has no icon");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }

        public void SetSpellItemQuickSlotIcon(int spellID)
        {
            SpellItem spell = WorldItemDatabase.Instance.GetSpellByID(spellID);

            if (spell == null)
            {
                Debug.Log("Item is null");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }

            if (spell.itemIcon == null)
            {
                Debug.Log("Item has no icon");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }

            spellItemQuickSlotIcon.sprite = spell.itemIcon;
            spellItemQuickSlotIcon.enabled = true;
        }
    }
}
