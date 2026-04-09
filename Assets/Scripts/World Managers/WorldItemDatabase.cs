using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace baodeag
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        public GameObject pickUpItemPrefab;

        [Header("Upgrade Stones")]
        public UpgradeMaterial smallUpgradeStone;
        public UpgradeMaterial mediumUpgradeStone;
        public UpgradeMaterial largeUpgradeStone;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Head Equipment")]
        [SerializeField] List<HeadEquipmentItem> headEquipment = new List<HeadEquipmentItem>();

        [Header("Body Equipment")]
        [SerializeField] List<BodyEquipmentItem> bodyEquipment = new List<BodyEquipmentItem>();

        [Header("Leg Equipment")]
        [SerializeField] List<LegEquipmentItem> legEquipment = new List<LegEquipmentItem>();

        [Header("Hand Equipment")]
        [SerializeField] List<HandEquipmentItem> handEquipment = new List<HandEquipmentItem>();

        [Header("Ash Of War")]
        [SerializeField] List<AshOfWar> ashesOfWar = new List<AshOfWar>();

        [Header("Spells")]
        [SerializeField] List<SpellItem> spells = new List<SpellItem>();

        [Header("Projectiles")]
        [SerializeField] List<RangedProjectileItem> projectiles = new List<RangedProjectileItem>();

        [Header("Quick Slot")]
        [SerializeField] List<QuickSlotItem> quickSlotItems = new List<QuickSlotItem>();
        [SerializeField] List<BuffCharmItem> defaultBuffCharms = new List<BuffCharmItem>();

        [Header("Upgrade Materials")]
        [SerializeField] List<UpgradeMaterial> upgradeMaterials = new List<UpgradeMaterial>();

        //a list of every item in the game
        [Header("Items")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            RegisterDefaultBuffCharms();

            //add all of our weapons to the item list
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            foreach (var item in headEquipment)
            {
                items.Add(item);
            }

            foreach (var item in bodyEquipment)
            {
                items.Add(item);
            }

            foreach (var item in legEquipment)
            {
                items.Add(item);
            }

            foreach (var item in handEquipment)
            {
                items.Add(item);
            }

            foreach(var item in ashesOfWar)
            {
                items.Add(item);
            }

            foreach (var item in spells)
            {
                items.Add(item); 
            }

            foreach (var item in projectiles)
            {
                items.Add(item);
            }

            foreach (var item in quickSlotItems)
            {
                items.Add(item);
            }

            foreach (var item in upgradeMaterials)
            {
                items.Add(item);
            }


            //assign all of our items a unique item id
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        private void RegisterDefaultBuffCharms()
        {
            if (quickSlotItems == null)
                quickSlotItems = new List<QuickSlotItem>();

            if (defaultBuffCharms == null)
                defaultBuffCharms = new List<BuffCharmItem>();

            if (defaultBuffCharms.Count > 0)
                return;

            Sprite placeholderIcon = quickSlotItems.Count > 0 ? quickSlotItems[0].itemIcon : null;

            defaultBuffCharms.Add(CreateDefaultBuffCharm(
                "Guardian Charm",
                "Temporary blessing that raises maximum health.",
                placeholderIcon,
                45f,
                maxHealthBonus: 80,
                purchasePrice: 180,
                sellPrice: 90));

            defaultBuffCharms.Add(CreateDefaultBuffCharm(
                "Wind Charm",
                "Temporary blessing that raises maximum stamina and stamina recovery.",
                placeholderIcon,
                45f,
                maxStaminaBonus: 45,
                staminaRegenerationBonusPercentage: 25f,
                purchasePrice: 180,
                sellPrice: 90));

            defaultBuffCharms.Add(CreateDefaultBuffCharm(
                "Sage Charm",
                "Temporary blessing that raises maximum mana.",
                placeholderIcon,
                45f,
                maxFocusPointsBonus: 50,
                purchasePrice: 180,
                sellPrice: 90));

            defaultBuffCharms.Add(CreateDefaultBuffCharm(
                "War Charm",
                "Temporary blessing that empowers all outgoing weapon damage.",
                placeholderIcon,
                35f,
                outgoingDamageBonusPercentage: 20f,
                purchasePrice: 220,
                sellPrice: 110));

            for (int i = 0; i < defaultBuffCharms.Count; i++)
            {
                if (defaultBuffCharms[i] != null)
                    quickSlotItems.Add(defaultBuffCharms[i]);
            }
        }

        private BuffCharmItem CreateDefaultBuffCharm(
            string itemName,
            string itemDescription,
            Sprite icon,
            float durationSeconds,
            int maxHealthBonus = 0,
            int maxStaminaBonus = 0,
            int maxFocusPointsBonus = 0,
            float staminaRegenerationBonusPercentage = 0f,
            float outgoingDamageBonusPercentage = 0f,
            int purchasePrice = 100,
            int sellPrice = 50)
        {
            BuffCharmItem buffCharm = ScriptableObject.CreateInstance<BuffCharmItem>();
            buffCharm.name = itemName;
            buffCharm.InitializeRuntimeBuff(
                itemName,
                itemDescription,
                icon,
                durationSeconds,
                maxHealthBonus,
                maxStaminaBonus,
                maxFocusPointsBonus,
                staminaRegenerationBonusPercentage,
                outgoingDamageBonusPercentage,
                2,
                5,
                purchasePrice,
                sellPrice);

            return buffCharm;
        }

        public List<BuffCharmItem> GetDefaultBuffCharms()
        {
            return defaultBuffCharms;
        }

        //item database

        public Item GetItemByID(int ID)
        {
            return items.FirstOrDefault(item => item.itemID == ID);
        }

        public Item CreateItemInstance(int itemID)
        {
            Item item = GetItemByID(itemID);

            if (item == null)
                return null;

            return Instantiate(item);
        }

        public List<Item> GetPurchasableItems()
        {
            return items.Where(item => item != null && item.canBePurchased).ToList();
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

        public HeadEquipmentItem GetHeadEquipmentByID(int ID)
        {
            return headEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public BodyEquipmentItem GetBodyEquipmentByID(int ID)
        {
            return bodyEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public LegEquipmentItem GetLegEquipmentByID(int ID)
        {
            return legEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public HandEquipmentItem GetHandEquipmentByID(int ID)
        {
            return handEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public AshOfWar GetAshOfWarByID(int ID)
        {
            return ashesOfWar.FirstOrDefault(item => item.itemID == ID);
        }

        public SpellItem GetSpellByID(int ID)
        {
            return spells.FirstOrDefault(item => item.itemID == ID);
        }

        public RangedProjectileItem GetProjectileByID(int ID)
        {
            return projectiles.FirstOrDefault(item => item.itemID == ID);
        }

        public QuickSlotItem GetQuickSlotItemByID(int ID)
        {
            return quickSlotItems.FirstOrDefault(item => item.itemID == ID);
        }

        public UpgradeMaterial GetUpgradeMaterialByID(int ID)
        {
            return upgradeMaterials.FirstOrDefault(item => item.itemID == ID);
        }

        //item serialization

        public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializableWeapon)
        {
            WeaponItem weapon = null;

            if (GetWeaponByID(serializableWeapon.itemID))
                weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID));

            if (weapon == null)
                return Instantiate(unarmedWeapon);

            if (GetAshOfWarByID(serializableWeapon.ashOfWarID))
            {
                AshOfWar ashOfWar = Instantiate(GetAshOfWarByID(serializableWeapon.ashOfWarID));
                weapon.ashOfWarAction = ashOfWar;
            }

            weapon.upgradeLevel = (UpgradeLevel)serializableWeapon.upgradeLevel;

            return weapon;
        }

        public RangedProjectileItem GetRangedProjectileFromSerializedData(SerializableRangedProjectile serializableProjectile)
        {
            RangedProjectileItem rangedProjectile = null;

            if (GetProjectileByID(serializableProjectile.itemID))
            {
                rangedProjectile = Instantiate(GetProjectileByID(serializableProjectile.itemID));
                rangedProjectile.currentAmmoAmount = serializableProjectile.itemAmount;
            }              

            return rangedProjectile;
        }

        public FlaskItem GetFlaskFromSerializedData(SerializableFlask serializableFlask)
        {
            FlaskItem flask = null;

            if (GetQuickSlotItemByID(serializableFlask.itemID))
                flask = Instantiate(GetQuickSlotItemByID(serializableFlask.itemID)) as FlaskItem;

            return flask;
        }

        public QuickSlotItem GetQuickSlotItemFromSerializedData(SerializableQuickSlotItem serializableQuickSlotItem)
        {
            QuickSlotItem quickSlotItem = null;

            if (GetQuickSlotItemByID(serializableQuickSlotItem.itemID))
            {
                quickSlotItem = Instantiate(GetQuickSlotItemByID(serializableQuickSlotItem.itemID));
                quickSlotItem.itemAmount = serializableQuickSlotItem.itemAmount;
            }

            return quickSlotItem;
        }
    }
}
