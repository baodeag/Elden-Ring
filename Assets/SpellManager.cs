using UnityEngine;

namespace baodeag
{
    public class SpellManager : MonoBehaviour
    {
        [Header("Spell Target")]
        [SerializeField] protected CharacterManager spellTarget;

        [Header("VFX")]
        [SerializeField] protected GameObject impactParticle; // the particle effect when fireball hits a target
        [SerializeField] protected GameObject impactParticleFullCharge; // the particle effect when fireball hits a target with full charge

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }
    }
}
