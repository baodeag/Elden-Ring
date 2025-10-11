using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace baodeag
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;
        [SerializeField] bool hasBeenDefeated = false;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                else
                {
                    hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

                    if (hasBeenDefeated)
                    {
                        aiCharacterNetworkManager.isActive.Value = false;
                    }
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                //reset any flags here that need to be reset on death

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                hasBeenDefeated = true;

                if (IsServer)
                {
                    if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                    {
                        WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                        WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                    }
                    else
                    {
                        WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                        WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                        WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                        WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                    }

                    WorldSaveGameManager.instance.SaveGame();
                }
            }

            yield return new WaitForSeconds(5);
        }
    }
}
