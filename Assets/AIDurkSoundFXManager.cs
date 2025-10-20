using baodeag;
using UnityEngine;

namespace baodead
{
    public class AIDurkSoundFXManager : CharacterSoundFXManager
    {
        [Header("Club Whooshes")]
        public AudioClip[] clubWhooshes;

        [Header("Club Impact")]
        public AudioClip[] clubImpacts;

        [Header("Stomp Impact")]
        public AudioClip[] stompImpacts;

        public virtual void PlayClubImpactSoundFX()
        {
            if (clubImpacts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(clubImpacts));
        }

        public virtual void PlayStompImpactSoundFX()
        {
            if (stompImpacts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(stompImpacts));
        }
    }
}
