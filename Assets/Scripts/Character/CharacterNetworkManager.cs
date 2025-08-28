using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [Header("Position")]
        // if you owner this object, you can write to this variable
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>
            (Vector3.zero, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>
            (Quaternion.identity,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime =0.1f;
        public float networkRotationSmoothTime = 0.1f;
    }

}
