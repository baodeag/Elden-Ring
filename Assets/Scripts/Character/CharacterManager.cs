using UnityEngine;

namespace baodeag
{
    public class CharacterManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
        protected virtual void Update()
        {

        }
    }
}
