using System.Collections.Generic;
using Character.Villains;
using Common;
using Common.StatusEffects;
using UnityEngine;

namespace Raid.UI.VillainFrames
{
    public class VillainStatusEffectUI : MonoBehaviour, IEditable
    {
        [SerializeField] private List<StatusEffectUI> buffList;
        [SerializeField] private List<StatusEffectUI> deBuffList;

        private static VillainBehaviour Villain => RaidDirector.Villain;


        public void Initialize()
        {
            var table = Villain.StatusEffectTable;
            
            table.OnEffectAdded.Add("OnAddedUI", OnAdded);
            table.OnEffectRemoved.Add("OnRemovedUI", OnRemoved);
            
            UpdateUI();
        }
        
        
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
            
            var table = Villain.StatusEffectTable;
            
            table.Iterate(OnAdded);
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
