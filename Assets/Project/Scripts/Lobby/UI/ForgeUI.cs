using Common;
using Common.Characters;
using GameEvents;
using UnityEngine;

namespace Lobby.UI
{
    public class ForgeUI : MonoBehaviour, IEditable
    {
        [SerializeField] private GameEvent reloadForgeEvent;
        
        private VenturerType focusVenturer = VenturerType.Knight;
        public void Reload() => reloadForgeEvent.Invoke();

        public VenturerType FocusVenturer
        {
            get => focusVenturer;
            set
            {
                focusVenturer = value;
                reloadForgeEvent.Invoke();
            }
        }

        public VenturerData FocusVenturerData => Camp.GetData(FocusVenturer);

        public int VenturerEthosValue(EthosType type) => FocusVenturerData.GetEthosValue(type);
        public IEquipment VenturerWeapon() => Camp.GetVenturerWeapon(FocusVenturer);
        public IEquipment VenturerArmor() => Camp.GetVenturerArmor(FocusVenturer);


#if UNITY_EDITOR
        private void Start()
        {
            reloadForgeEvent.Invoke();
        }

        public void EditorSetUp()
        {
            // evolveFrame ??= GetComponentInChildren<EvolveFrame>();
        }

        private void Test()
        {
            reloadForgeEvent.Invoke();
        }
#endif
    }
}
