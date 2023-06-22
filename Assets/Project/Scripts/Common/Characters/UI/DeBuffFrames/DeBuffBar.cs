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
            
            Cb.DynamicStatEntry.StatusEffectTable.Iterator(effect =>
            {
                if (!TryGetEmptySlot(out var slot)) return;

                slot.Register(effect);
            });
        }

        private void OnEnable()
        {
            UpdateDeBuffUI();
            
            Cb.DynamicStatEntry.StatusEffectTable.AddListener(UpdateDeBuffUI);
        }

        private void OnDisable()
        {
            slotList.ForEach(slot => slot.Unregister());
            
            Cb.DynamicStatEntry.StatusEffectTable.RemoveListener(UpdateDeBuffUI);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
#endif
    }
}
