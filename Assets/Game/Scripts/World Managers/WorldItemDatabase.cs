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
        [SerializeField] Sprite guardianBuffQuickSlotIcon;
        [SerializeField] Sprite windBuffQuickSlotIcon;
        [SerializeField] Sprite sageBuffQuickSlotIcon;
        [SerializeField] Sprite warBuffQuickSlotIcon;
        [SerializeField] GameObject guardianBuffFlaskPrefab;
        [SerializeField] GameObject windBuffFlaskPrefab;
        [SerializeField] GameObject sageBuffFlaskPrefab;
        [SerializeField] GameObject warBuffFlaskPrefab;
        [SerializeField] GameObject guardianBuffPotionVFX;
        [SerializeField] GameObject windBuffPotionVFX;
        [SerializeField] GameObject sageBuffPotionVFX;
        [SerializeField] GameObject warBuffPotionVFX;

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
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance.MergeDatabase(this);
                Destroy(gameObject);
                return;
            }

            RegisterDefaultBuffCharms();
            RebuildItemList(true);
        }

        private void RebuildItemList(bool assignSequentialIDs)
        {
            items.Clear();

            foreach (var weapon in weapons)
                AddUniqueItemToDatabase(weapon);

            foreach (var item in headEquipment)
                AddUniqueItemToDatabase(item);

            foreach (var item in bodyEquipment)
                AddUniqueItemToDatabase(item);

            foreach (var item in legEquipment)
                AddUniqueItemToDatabase(item);

            foreach (var item in handEquipment)
                AddUniqueItemToDatabase(item);

            foreach(var item in ashesOfWar)
                AddUniqueItemToDatabase(item);

            foreach (var item in spells)
                AddUniqueItemToDatabase(item);

            foreach (var item in projectiles)
                AddUniqueItemToDatabase(item);

            foreach (var item in quickSlotItems)
                AddUniqueItemToDatabase(item);

            foreach (var item in upgradeMaterials)
                AddUniqueItemToDatabase(item);

            if (!assignSequentialIDs)
                return;

            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        private void MergeDatabase(WorldItemDatabase database)
        {
            if (database == null)
                return;

            RegisterDefaultBuffCharms();

            foreach (var weapon in database.weapons)
                RegisterItemTemplate(weapon);

            foreach (var item in database.headEquipment)
                RegisterItemTemplate(item);

            foreach (var item in database.bodyEquipment)
                RegisterItemTemplate(item);

            foreach (var item in database.legEquipment)
                RegisterItemTemplate(item);

            foreach (var item in database.handEquipment)
                RegisterItemTemplate(item);

            foreach (var item in database.ashesOfWar)
                RegisterItemTemplate(item);

            foreach (var item in database.spells)
                RegisterItemTemplate(item);

            foreach (var item in database.projectiles)
                RegisterItemTemplate(item);

            foreach (var item in database.quickSlotItems)
                RegisterItemTemplate(item);

            foreach (var item in database.upgradeMaterials)
                RegisterItemTemplate(item);
        }

        public void RegisterItemTemplate(Item item)
        {
            if (item == null)
                return;

            if (item is WeaponItem weapon)
                AddUniqueTypedItem(weapons, weapon);
            else if (item is HeadEquipmentItem head)
                AddUniqueTypedItem(headEquipment, head);
            else if (item is BodyEquipmentItem body)
                AddUniqueTypedItem(bodyEquipment, body);
            else if (item is LegEquipmentItem legs)
                AddUniqueTypedItem(legEquipment, legs);
            else if (item is HandEquipmentItem hands)
                AddUniqueTypedItem(handEquipment, hands);
            else if (item is AshOfWar ashOfWar)
                AddUniqueTypedItem(ashesOfWar, ashOfWar);
            else if (item is SpellItem spell)
                AddUniqueTypedItem(spells, spell);
            else if (item is RangedProjectileItem projectile)
                AddUniqueTypedItem(projectiles, projectile);
            else if (item is QuickSlotItem quickSlotItem)
                AddUniqueTypedItem(quickSlotItems, quickSlotItem);
            else if (item is UpgradeMaterial upgradeMaterial)
                AddUniqueTypedItem(upgradeMaterials, upgradeMaterial);

            AddUniqueItemToDatabase(item);
        }

        private void AddUniqueTypedItem<T>(List<T> list, T item) where T : Item
        {
            if (list == null || item == null)
                return;

            if (list.Any(existingItem => ItemsMatch(existingItem, item)))
                return;

            list.Add(item);
        }

        private void AddUniqueItemToDatabase(Item item)
        {
            if (item == null)
                return;

            if (items.Any(existingItem => ItemsMatch(existingItem, item)))
                return;

            items.Add(item);
        }

        private bool ItemsMatch(Item a, Item b)
        {
            if (a == null || b == null)
                return false;

            if (a == b)
                return true;

            if (a.itemID == b.itemID && a.GetType() == b.GetType())
                return true;

            return !string.IsNullOrWhiteSpace(a.itemName) &&
                   a.itemName == b.itemName &&
                   a.GetType() == b.GetType();
        }

        private void RegisterDefaultBuffCharms()
        {
            if (quickSlotItems == null)
                quickSlotItems = new List<QuickSlotItem>();

            if (defaultBuffCharms == null)
                defaultBuffCharms = new List<BuffCharmItem>();

            if (defaultBuffCharms.Count == 0)
            {
                defaultBuffCharms.Add(CreateDefaultBuffCharm(
                    "Guardian Charm",
                    "Temporary blessing that raises maximum health.",
                    guardianBuffQuickSlotIcon,
                    45f,
                    maxHealthBonus: 80,
                    purchasePrice: 180,
                    sellPrice: 90));

                defaultBuffCharms.Add(CreateDefaultBuffCharm(
                    "Wind Charm",
                    "Temporary blessing that raises maximum stamina and stamina recovery.",
                    windBuffQuickSlotIcon,
                    45f,
                    maxStaminaBonus: 45,
                    staminaRegenerationBonusPercentage: 25f,
                    purchasePrice: 180,
                    sellPrice: 90));

                defaultBuffCharms.Add(CreateDefaultBuffCharm(
                    "Sage Charm",
                    "Temporary blessing that raises maximum mana.",
                    sageBuffQuickSlotIcon,
                    45f,
                    maxFocusPointsBonus: 50,
                    purchasePrice: 180,
                    sellPrice: 90));

                defaultBuffCharms.Add(CreateDefaultBuffCharm(
                    "War Charm",
                    "Temporary blessing that empowers all outgoing weapon damage.",
                    warBuffQuickSlotIcon,
                    35f,
                    outgoingDamageBonusPercentage: 20f,
                    purchasePrice: 220,
                    sellPrice: 110));
            }

            for (int i = 0; i < defaultBuffCharms.Count; i++)
            {
                if (defaultBuffCharms[i] != null && !quickSlotItems.Contains(defaultBuffCharms[i]))
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

            buffCharm.SetRuntimeItemModel(GetDefaultBuffFlaskPrefab(itemName));
            buffCharm.SetRuntimeUseItemVFX(GetDefaultBuffPotionVFXPrefab(itemName));

            return buffCharm;
        }

        private GameObject GetDefaultBuffFlaskPrefab(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return null;

            string normalizedName = itemName.ToLowerInvariant();

            if (normalizedName.Contains("guardian"))
                return guardianBuffFlaskPrefab;

            if (normalizedName.Contains("wind"))
                return windBuffFlaskPrefab;

            if (normalizedName.Contains("sage"))
                return sageBuffFlaskPrefab;

            if (normalizedName.Contains("war"))
                return warBuffFlaskPrefab;

            return null;
        }

        private GameObject GetDefaultBuffPotionVFXPrefab(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return null;

            string normalizedName = itemName.ToLowerInvariant();

            if (normalizedName.Contains("guardian"))
                return guardianBuffPotionVFX;

            if (normalizedName.Contains("wind"))
                return windBuffPotionVFX;

            if (normalizedName.Contains("sage"))
                return sageBuffPotionVFX;

            if (normalizedName.Contains("war"))
                return warBuffPotionVFX;

            return null;
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

        public Item CreateItemInstance(SerializableItem serializableItem)
        {
            if (serializableItem == null)
                return null;

            Item item = GetItemByID(serializableItem.itemID);

            if (item == null && !string.IsNullOrWhiteSpace(serializableItem.itemName))
                item = items.FirstOrDefault(databaseItem => databaseItem != null && databaseItem.itemName == serializableItem.itemName);

            if (item == null)
            {
                
                return null;
            }

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

            if (weapon == null && !string.IsNullOrWhiteSpace(serializableWeapon.itemName))
            {
                WeaponItem weaponByName = weapons.FirstOrDefault(databaseWeapon => databaseWeapon != null && databaseWeapon.itemName == serializableWeapon.itemName);

                if (weaponByName != null)
                {
                    weapon = Instantiate(weaponByName);
                    
                }
            }

            if (weapon == null)
            {
                
                return Instantiate(unarmedWeapon);
            }

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

        public Item GetItemFromSerializedData(SerializableItem serializableItem)
        {
            Item item = CreateItemInstance(serializableItem);

            if (item == null)
                return null;

            item.currentItemAmount = Mathf.Max(1, serializableItem.itemAmount);
            return item;
        }
    }
}
