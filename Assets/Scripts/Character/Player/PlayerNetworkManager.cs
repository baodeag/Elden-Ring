using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace baodeag
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>(
            "Character", 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
    }
}
