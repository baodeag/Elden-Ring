using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace baodeag
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;

        [Header("Music")]
        [SerializeField] AudioClip bossIntroClip;
        [SerializeField] AudioClip bossBattleLoopClip;

        [Header("Status")]
        [SerializeField] bool autoWakeOnSpawn = false;
        public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        [SerializeField] List<FogWallInteractable> fogWalls;
        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;

        [Header("Phase Shift")]
        public float minimumHealthPercentageToShift = 50;
        [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
        [SerializeField] CombatStanceState phase02CombatStanceState;

        [Header("States")]
        public BossSleepState sleepState;

        protected override void Awake()
        {
            base.Awake();
            AutoAssignBossIDFromWorldScene();
        }

        private void OnValidate()
        {
            AutoAssignBossIDFromWorldScene();
        }

        private void AutoAssignBossIDFromWorldScene()
        {
            int sceneBuildIndex = gameObject.scene.buildIndex;

            if (sceneBuildIndex < 1 || sceneBuildIndex > 5)
                return;

            bossID = sceneBuildIndex - 1;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
            OnBossFightIsActiveChanged(false, bossFightIsActive.Value);

            if (IsOwner)
            {
                sleepState = Instantiate(sleepState);
                currentState = sleepState;

                if (autoWakeOnSpawn && !hasBeenDefeated.Value)
                {
                    StartCoroutine(AutoWakeWhenReady());
                }
            }

            if (IsServer)
            {
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                else
                {
                    hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];
                    sleepState.hasBeenAwakened = hasBeenAwakened.Value;
                }

                StartCoroutine(InitializeBossWorldState());
            }

            if (!hasBeenAwakened.Value)
            {
                animator.Play(sleepAnimation);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
        }

        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.instance.fogWalls.Count == 0)
                yield return new WaitForEndOfFrame();

            RefreshFogWallsFromWorldObjectManager();
        }

        private void RefreshFogWallsFromWorldObjectManager()
        {
            if (WorldObjectManager.instance == null || WorldObjectManager.instance.fogWalls == null)
                return;

            fogWalls = new List<FogWallInteractable>();

            foreach (var fogWall in WorldObjectManager.instance.fogWalls)
            {
                if (fogWall != null && fogWall.fogWallID == bossID)
                    fogWalls.Add(fogWall);
            }
        }

        private IEnumerator InitializeBossWorldState()
        {
            yield return StartCoroutine(GetFogWallsFromWorldObjectManager());
            ApplyBossWorldState();
        }

        private IEnumerator AutoWakeWhenReady()
        {
            while (fogWalls == null)
                yield return null;

            yield return null;

            if (!hasBeenDefeated.Value)
            {
                WakeBoss();
            }
        }

        private void ApplyBossWorldState()
        {
            if (fogWalls == null || fogWalls.Count == 0)
                RefreshFogWallsFromWorldObjectManager();

            if (fogWalls == null)
                return;

            bool shouldEnableFogWalls = hasBeenAwakened.Value && !hasBeenDefeated.Value;

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = shouldEnableFogWalls;
            }

            if (hasBeenDefeated.Value)
            {
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("Great Foe Felled");
            bool shouldLoadNextScene = false;
            int nextSceneBuildIndex = -1;
            int unlockedMapIndex = -1;
            bool gameWon = false;

            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                bossFightIsActive.Value = false;

                foreach (var fogWall in fogWalls)
                {
                    fogWall.isActive.Value = false;
                }

                //reset any flags here that need to be reset on death

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                hasBeenDefeated.Value = true;

                aiCharacterCombatManager.AwardRunesOnDeath(PlayerUIManager.instance.localPlayer);

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

                shouldLoadNextScene = GameProgressionManager.Instance.RegisterBossDefeat(bossID, out nextSceneBuildIndex, out unlockedMapIndex, out gameWon);
                bool canContinueProgression = shouldLoadNextScene || unlockedMapIndex >= 0;

                int entrySiteOfGraceID = GameProgressionManager.Instance.GetEntrySiteOfGraceIDForCurrentMap();

                if (entrySiteOfGraceID >= 0)
                {
                    WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt = entrySiteOfGraceID;
                    PlayerUIManager.instance.localPlayer.playerNetworkManager.lastSiteOfGraceUsed.Value = entrySiteOfGraceID;
                }

                if (WorldGameSessionManager.instance != null)
                {
                    WorldGameSessionManager.instance.ScheduleMapTransition(
                        shouldLoadNextScene,
                        nextSceneBuildIndex,
                        gameWon,
                        unlockedMapIndex);
                }

                if (gameWon)
                {
                    BroadcastVictoryAchievedClientRpc(canContinueProgression, 0f);
                }

                WorldSaveGameManager.instance.SaveGame();
                ApplyBossWorldState();
            }

            yield break;
        }

        public void WakeBoss()
        {
            if (IsOwner)
            {
                if (fogWalls == null || fogWalls.Count == 0)
                    RefreshFogWallsFromWorldObjectManager();

                if (!hasBeenAwakened.Value)
                {
                    characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
                }

                bossFightIsActive.Value = true;
                hasBeenAwakened.Value = true;
                aiCharacterNetworkManager.isAwake.Value = true;
                currentState = idle;

                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }
                else
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }

                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }

                ApplyBossWorldState();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestWakeBossServerRpc()
        {
            if (IsServer)
                WakeBoss();
        }

        [ClientRpc]
        private void BroadcastVictoryAchievedClientRpc(bool canContinueProgression, float delay)
        {
            if (WorldGameSessionManager.instance != null)
            {
                WorldGameSessionManager.instance.HandleSessionVictory(canContinueProgression, delay);
            }
            else if (PlayerUIManager.instance != null)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendVictoryPopUpDelayed("Victory Achieved", delay);
            }
        }

        private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (bossFightIsActive.Value)
            {
                WorldSoundFXManager.instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

                //create  a hp bar for each boss that is in the fight
                GameObject bossHealthBar =
                Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarObject, PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
                bossHPBar.EnableBossHPBar(this);
                PlayerUIManager.instance.playerUIHudManager.currentBossHealthBar = bossHPBar;
            }
            else
            {
                WorldSoundFXManager.instance.StopBossMusic();
            }
        }

        public void PhaseShift()
        {
            characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
            combatStance = Instantiate(phase02CombatStanceState);
            currentState = combatStance;
        }

        public override void ActivateCharacter(PlayerManager player)
        {
            if (hasBeenDefeated.Value)
            {
                DeactivateCharacter(player);
                return;
            }

            aiCharacterCombatManager.AddPlayerToPlayersWithinRange(player);

            if (player.IsLocalPlayer)
            {

            }

            if (beacon != null)
            {
                beacon.gameObject.transform.position = transform.position;
                beacon.gameObject.SetActive(true);
            }

            if (!NetworkManager.Singleton.IsHost)
                return;

            if (aiCharacterCombatManager.playersWithinActivationRange.Count > 0)
            {
                aiCharacterNetworkManager.isActive.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }
    }
}
