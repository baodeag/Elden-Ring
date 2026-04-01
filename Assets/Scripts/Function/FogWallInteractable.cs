using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace baodeag
{
    public class FogWallInteractable : Interactable
    {
        [Header("Fog")]
        [SerializeField] GameObject[] fogGameObjects;

        [Header("Collision")]
        [SerializeField] Collider fogWallCollider;

        [Header("ID")]
        public int fogWallID;

        [Header("Sound")]
        private  AudioSource fogWallAudioSource;
        [SerializeField] AudioClip fogWallSFX;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>
            (false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            AutoAssignFogWallIDFromWorldScene();

            fogWallAudioSource = gameObject.AddComponent<AudioSource>();
        }

        private void OnValidate()
        {
            AutoAssignFogWallIDFromWorldScene();
        }

        private void AutoAssignFogWallIDFromWorldScene()
        {
            int sceneBuildIndex = gameObject.scene.buildIndex;

            if (sceneBuildIndex < 1 || sceneBuildIndex > 5)
                return;

            fogWallID = sceneBuildIndex - 1;
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            Quaternion targetRotation = GetPassThroughRotation(player);
            player.transform.rotation = targetRotation;

            if (player.IsOwner)
            {
                player.characterNetworkManager.networkRotation.Value = targetRotation;
            }

            AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);
            player.playerAnimatorManager.PlayTargetActionAnimation("Pass_Through_Fog_01", true);
            StartCoroutine(ReEnableInteractionAfterPassThrough());
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, isActive.Value);
            isActive.OnValueChanged += OnIsActiveChanged;
            WorldObjectManager.instance.AddFogWallToList(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActive.OnValueChanged -= OnIsActiveChanged;
            WorldObjectManager.instance.RemoveFogWallFromList(this);
        }

        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (isActive.Value)
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(true);
                }
            }
            else
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(false);
                }

            }
        }

        //when a server rpc does not require ownership, it can be called by any client
        [ServerRpc(RequireOwnership = false)]
        private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)
        {
            if (IsServer)
            {
                AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
            }
        }

        [ClientRpc]
        private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

            fogWallAudioSource.PlayOneShot(fogWallSFX);

            if (player != null)
            {
                Vector3 passDirection = GetPassDirection(player);
                StartCoroutine(DisableCollisionForTime(player));
                StartCoroutine(MovePlayerThroughFogWall(player, passDirection));
            }
        }

        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

            yield return new WaitForSeconds(3);

            Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
        }

        private IEnumerator ReEnableInteractionAfterPassThrough()
        {
            yield return new WaitForSeconds(3);

            if (interactableCollider != null)
            {
                interactableCollider.enabled = true;
            }
        }

        private IEnumerator MovePlayerThroughFogWall(PlayerManager player, Vector3 passDirection)
        {
            const float passDuration = 0.4f;
            const float passDistance = 2.25f;

            if (player.characterController == null)
                yield break;

            float elapsed = 0f;
            float moveSpeed = passDistance / passDuration;

            while (elapsed < passDuration)
            {
                float delta = moveSpeed * Time.deltaTime;
                player.characterController.Move(passDirection * delta);

                if (player.IsOwner)
                {
                    player.characterNetworkManager.networkPosition.Value = player.transform.position;
                    player.characterNetworkManager.networkRotation.Value = player.transform.rotation;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private Quaternion GetPassThroughRotation(PlayerManager player)
        {
            Vector3 passDirection = GetPassDirection(player);
            return Quaternion.LookRotation(passDirection);
        }

        private Vector3 GetPassDirection(PlayerManager player)
        {
            Vector3 wallForward = transform.forward;
            wallForward.y = 0;

            if (wallForward.sqrMagnitude <= Mathf.Epsilon)
            {
                wallForward = Vector3.forward;
            }

            wallForward.Normalize();

            Vector3 directionFromWallToPlayer = player.transform.position - transform.position;
            directionFromWallToPlayer.y = 0;

            if (directionFromWallToPlayer.sqrMagnitude <= Mathf.Epsilon)
            {
                directionFromWallToPlayer = wallForward;
            }

            directionFromWallToPlayer.Normalize();

            return Vector3.Dot(directionFromWallToPlayer, wallForward) >= 0 ? -wallForward : wallForward;
        }
    }
}
