using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.Forge
{
    public class UpgradeButton : MonoBehaviour, IEditable
    {
        [SerializeField] private Button button;
        [SerializeField] private EquipmentSlotType type;

        private IEquipment TargetEquipment => type == EquipmentSlotType.Weapon
            ? LobbyDirector.UI.Forge.VenturerWeapon()
            : LobbyDirector.UI.Forge.VenturerArmor();
        private GrowMaterialType MaterialType =>
            type == EquipmentSlotType.Weapon
                ? GrowMaterialType.ShardOfVicious
                : GrowMaterialType.StoneOfVicious;

        private int RequireCount
        {
            get
            {
                var upgradeIndex = (TargetEquipment.Tier - 1) * 6 + (TargetEquipment.Level - 1);
                
                return Database.UpgradeSheetData(DataIndex.UpgradeRequirement).LevelList[upgradeIndex];
            }
        }

        private int HoldCount => Camp.GetGrowMaterialCount(MaterialType);
        private bool IsAbleToUpgrade => HoldCount >= RequireCount;

        public void Upgrade()
        {
            if (!IsAbleToUpgrade)
            {
                Debug.Log($"Not Enough Material. Require {RequireCount}, you Have {HoldCount}");
                return;
            }
            
            TargetEquipment.Upgrade();
            Camp.ConsumeGrowMaterial(MaterialType, RequireCount);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            button ??= GetComponent<Button>();
        }
#endif
    }
}
