using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class SerializableItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID = -1;
        [SerializeField] public int itemAmount = 1;

        public Item GetItem()
        {
            if (WorldItemDatabase.Instance == null)
                return null;

            return WorldItemDatabase.Instance.GetItemFromSerializedData(this);
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
