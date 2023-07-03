using System.Collections.Generic;
using Character.Venturers;
using Common;
using Common.StatusEffects;
using UnityEngine;

namespace Raid.UI
{
    public class StatusEffectBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<StatusEffectUI> buffList;
        [SerializeField] private List<StatusEffectUI> deBuffList;

        private VenturerBehaviour CurrentVenturer { get; set; }
        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (CurrentVenturer != null)
            {
                var effectTable = CurrentVenturer.StatusEffectTable;
                
                effectTable.OnEffectAdded.Remove("OnAddedUI");
                effectTable.OnEffectRemoved.Remove("OnRemovedUI");
            }

            var focusVenturerEffectTable = FocusVenturer.StatusEffectTable;
            
            focusVenturerEffectTable.OnEffectAdded.Add("OnAddedUI", OnAdded);
            focusVenturerEffectTable.OnEffectRemoved.Add("OnRemovedUI", OnRemoved);
            
            CurrentVenturer = FocusVenturer;
            UpdateUI();
        }


        private void OnAdded(StatusEffect effect)
        {
            var effectList = effect.Type == StatusEffectType.Buff 
                ? buffList 
                : deBuffList;

            if (!TryGetEmptySlot(effectList, out var effectUI)) return;
            
            var targetSlot = effectList.TryGetElement(effectSlot => effectSlot.StatusEffect == effect);
            
            if (targetSlot is not null) return;
            
            effectUI.Register(effect);
        }

        private void OnRemoved(StatusEffect effect)
        {
            var effectList = effect.Type == StatusEffectType.Buff 
                ? buffList 
                : deBuffList;

            var targetSlot = effectList.TryGetElement(effectSlot => effectSlot.StatusEffect == effect);

            if (targetSlot is null) return;
            
            targetSlot.Unregister();
        }

        private void UpdateUI()
        {
            buffList.ForEach(buffSlot => buffSlot.Unregister());
            deBuffList.ForEach(deBuffSlot => deBuffSlot.Unregister());
            
            var focusVenturerEffectTable = FocusVenturer.StatusEffectTable;
            
            focusVenturerEffectTable.Iterate(OnAdded);
        }
        
        private static bool TryGetEmptySlot(List<StatusEffectUI> targetSlotList, out StatusEffectUI slot)
        {
            return (slot = targetSlotList.Find(item => !item.IsRegistered)) is not null;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            transform.Find("Buffs").GetComponentsInChildren(true, buffList);
            transform.Find("DeBuffs").GetComponentsInChildren(true, deBuffList);
        }
#endif
    }
}
