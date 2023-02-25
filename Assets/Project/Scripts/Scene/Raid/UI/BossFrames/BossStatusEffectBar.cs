using System;
using System.Collections.Generic;
using Character;
using Core;
using Raid.UI.ActionFrames.StatusEffectIconBars;
using UnityEngine;

namespace Raid.UI.BossFrames
{
    public class BossStatusEffectBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<DeBuffActionSlot> deBuffActionSlotList;
        [SerializeField] private List<BuffActionSlot> buffActionSlotList;
        
        private MonsterBehaviour mb;

        public void Initialize()
        {
            mb = RaidDirector.Boss;

            mb.OnTakeStatusEffect.Unregister("RegisterUI");
            mb.OnTakeStatusEffect.Register("RegisterUI", OnRegisterStatusEffect);

            UpdateStatusEffect(mb);
        }
        
        
        private void OnRegisterStatusEffect(StatusEffectEntity seEntity)
        {
            if (seEntity.IsOverride) return;

            switch (seEntity.Effect.Type)
            {
                case StatusEffectType.Buff:
                {
                    foreach (var buff in buffActionSlotList)
                    {
                        if (buff.isActiveAndEnabled) continue;
            
                        buff.Register(seEntity.Effect);
                        break;
                    }

                    break;
                }
                case StatusEffectType.DeBuff:
                {
                    foreach (var deBuff in deBuffActionSlotList)
                    {
                        if (deBuff.isActiveAndEnabled) continue;
            
                        deBuff.Register(seEntity.Effect);
                        break;
                    }

                    break;
                }
                
                case StatusEffectType.None: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        private void UpdateStatusEffect(MonsterBehaviour ab)
        {
            // Add ab.StatusEffects
            ab.DynamicStatEntry.BuffTable.Values.ForEach(effect =>
            {
                if (!TryGetEmptyBuff(out var buffSlot)) return;
                
                buffSlot.Register(effect);
            });
            
            // Add ab.StatusEffects
            ab.DynamicStatEntry.DeBuffTable.Values.ForEach(effect =>
            {
                if (!TryGetEmptyDeBuff(out var deBuffSlot)) return;
                
                deBuffSlot.Register(effect);
            });
        }
        
        private bool TryGetEmptyBuff(out BuffActionSlot buffSlot)
        {
            buffSlot = null;
            
            foreach (var buff in buffActionSlotList)
            {
                if (buff.isActiveAndEnabled) continue;

                buffSlot = buff;
                break;
            }

            return buffSlot is not null;
        }
        
        private bool TryGetEmptyDeBuff(out DeBuffActionSlot deBuffSlot)
        {
            deBuffSlot = null;
            
            foreach (var deBuff in deBuffActionSlotList)
            {
                if (deBuff.isActiveAndEnabled) continue;

                deBuffSlot = deBuff;
                break;
            }

            return deBuffSlot is not null;
        }
        
        private void Awake()
        {
            GetComponentsInChildren(true, deBuffActionSlotList);
            GetComponentsInChildren(true, buffActionSlotList);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, deBuffActionSlotList);
            GetComponentsInChildren(true, buffActionSlotList);
        }
#endif
    }
}
