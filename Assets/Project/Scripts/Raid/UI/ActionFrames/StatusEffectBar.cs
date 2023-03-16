using System;
using System.Collections.Generic;
using Character.Adventurers;
using Common;
using Raid.UI.ActionFrames.StatusEffectIconBars;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class StatusEffectBar : MonoBehaviour
    {
        [SerializeField] private List<DeBuffActionSlot> deBuffActionSlotList;
        [SerializeField] private List<BuffActionSlot> buffActionSlotList;

        private Adventurer ab;

        public void Initialize(Adventurer ab) => OnFocusChanged(ab);
        public void OnFocusChanged(Adventurer ab)
        {
            if (this.ab != null)
            {
                this.ab.OnDeBuffTaken.Unregister("RegisterUI");
                
                // Clear Previous StatusEffect.
                buffActionSlotList.ForEach(buff => buff.Unregister());
                deBuffActionSlotList.ForEach(deBuff => deBuff.Unregister());
            }

            if (ab.IsNullOrEmpty()) return;

            this.ab = ab;

            ab.OnDeBuffTaken.Unregister("RegisterUI");
            ab.OnDeBuffTaken.Register("RegisterUI", OnRegisterStatusEffect);

            UpdateStatusEffect(ab);
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

        private void UpdateStatusEffect(Adventurer ab)
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
        

        public void SetUp()
        {
            GetComponentsInChildren(true, deBuffActionSlotList);
            GetComponentsInChildren(true, buffActionSlotList);
        }
    }
}
