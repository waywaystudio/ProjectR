using System.Collections.Generic;
using UnityEngine;

namespace Common.Characters.UI.DeBuffFrames
{
    public class DeBuffBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<DeBuffSlot> slotList = new();

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        private bool TryGetEmptySlot(out DeBuffSlot slot)
        {
            return (slot = slotList.Find(item => !item.IsRegistered)) is not null;
        }

        private void UpdateDeBuffUI()
        {
            slotList.ForEach(slot => slot.Unregister());
            Cb.DynamicStatEntry.DeBuffTable.ForEach(effect =>
            {
                if (!TryGetEmptySlot(out var slot)) return;

                slot.Register(effect.Value);
            });
        }

        private void OnEnable()
        {
            UpdateDeBuffUI();
            
            Cb.DynamicStatEntry.DeBuffTable.OnEffectChanged.Register("UpdateDeBuffUI", UpdateDeBuffUI);
        }

        private void OnDisable()
        {
            slotList.ForEach(slot => slot.Unregister());
            
            Cb.DynamicStatEntry.DeBuffTable.OnEffectChanged.Unregister("UpdateDeBuffUI");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
#endif
    }
}