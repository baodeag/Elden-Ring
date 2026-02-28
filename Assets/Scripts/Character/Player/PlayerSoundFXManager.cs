using UnityEngine;

namespace baodeag
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void PlayBlockSoundFX()
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));
        }

        public override void PlayFootStepSoundFX()
        {
            base.PlayFootStepSoundFX();

            WorldSoundFXManager.instance.AlertNearbyCharactersToSound(transform.position, 2);
        }
    }
}
