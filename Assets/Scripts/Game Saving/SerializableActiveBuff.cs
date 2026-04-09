using UnityEngine;

namespace baodeag
{
    [System.Serializable]
    public class SerializableActiveBuff
    {
        [SerializeField] public int sourceItemID = -1;
        [SerializeField] public float timeRemaining = 0f;
    }
}
