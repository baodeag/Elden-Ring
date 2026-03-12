using UnityEngine;

namespace baodeag
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        public AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        [Header("Foot Steps")]
        [SerializeField] protected AudioClip[] footSteps;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }

        public virtual void PlayDamageGruntSoundFX()
        {
            if (damageGrunts.Length >0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }

        public virtual void PlayAttackGruntSoundFX()
        {
            if (attackGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        }

        public virtual void PlayFootStepSoundFX()
        {
            if (footSteps.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
        }

        public virtual void PlayStanceBreakSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.stanceBreakSFX);
        }

        public virtual void PlayCriticalStrikeSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.criticalStrikeSFX);
        }

        public virtual void PlayBlockSoundFX()
        {

        }
    }
}
