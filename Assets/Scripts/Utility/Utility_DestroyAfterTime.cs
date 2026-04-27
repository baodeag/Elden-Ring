using UnityEngine;

namespace baodeag
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestroyed = 5;

        public void SetLifetime(float lifetime)
        {
            timeUntilDestroyed = lifetime;
        }

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}
