using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Equipment Model")]
    public class EquipmentModel : ScriptableObject
    {
        public EquipmentModelType equipmentModelType;
        public string maleEquipmentName;
        public string femaleEquipmentName;

        public void LoadModel(PlayerManager player, bool isMale)
        {
            if (isMale)
            {
                LoadMaleModel(player);
            }
            else
            {
                LoadFemaleModel(player);
            }
        }

        private void LoadMaleModel(PlayerManager player)
        {
            player.playerEquipmentManager.ActivateEquipmentModelByName(maleEquipmentName);
        }

        private void LoadFemaleModel(PlayerManager player)
        {
            player.playerEquipmentManager.ActivateEquipmentModelByName(femaleEquipmentName);
        }
    }
}
