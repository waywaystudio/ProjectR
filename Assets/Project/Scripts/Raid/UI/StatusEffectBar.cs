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
                RemoveStatusEffectUI(CurrentVenturer);
            }

            if (vb == null)
            {
                buffList.ForEach(buffSlot => buffSlot.Unregister());
                deBuffList.ForEach(deBuffSlot => deBuffSlot.Unregister());
                CurrentVenturer = null;
                return;
            }

            var focusVenturerEffectTable = FocusVenturer.StatusEffectTable;
            
            focusVenturerEffectTable.OnEffectAdded.Add("OnAddedUI", OnAdded);
            focusVenturerEffectTable.OnEffectRemoved.Add("OnRemovedUI", OnRemoved);
            
            CurrentVenturer = FocusVenturer;
            UpdateUI();
        }

        // 여기도 딱히...
        // public void OnCommandMode()
        // {
        //     if (CurrentVenturer != null)
        //     {
        //         RemoveStatusEffectUI(CurrentVenturer);
        //     }
        // }


        private void OnAdded(StatusEffect effect)
        {
            var effectList = effect.Type == StatusEffectType.Buff 
                ? buffList 
                : deBuffList;

            if (!TryGetEmptySlot(effectList, out var effectUI)) return;
            
            var isAlreadyHas = effectList.TryGetElement(effectSlot => effectSlot.StatusEffect == effect) is not null;
            if (isAlreadyHas) return;
            
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

        private static void RemoveStatusEffectUI(ICombatEntity vb)
        {
            var effectTable = vb.StatusEffectTable;
                
            effectTable.OnEffectAdded.Remove("OnAddedUI");
            effectTable.OnEffectRemoved.Remove("OnRemovedUI");
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
