using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class SerializableWeapon : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public string itemName;
        [SerializeField] public int upgradeLevel;
        [SerializeField] public int ashOfWarID;

        public WeaponItem GetWeapon()
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponFromSerializedData(this);
            return weapon;
        }

        public void OnAfterDeserialize()
        {
            
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}
