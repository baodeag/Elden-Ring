using baodeag;
using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class SerializableQuickSlotItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;
        //[SerializeField] public int maxFlaskCharges;
        //[SerializeField] public int flaskHealAmount;

        public QuickSlotItem GetQuickSlotItem()
        {
            QuickSlotItem quickSlotItem = WorldItemDatabase.Instance.GetQuickSlotItemFromSerializedData(this);
            return quickSlotItem;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}
