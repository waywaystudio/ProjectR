using System;
using System.Collections.Generic;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.StatusEffect
{
    using Buff;
    using DeBuff;
    
    [Serializable]
    public class StatusEffecting : MonoBehaviour
    {
        private CharacterBehaviour cb;
        
        [ShowInInspector]
        private Dictionary<string, BaseStatusEffect> statusEffectTable = new ();

        public void TryAdd(ICombatProvider provider)
        {
            if (statusEffectTable.ContainsKey(provider.ActionName))
            {
                // Implement Compare
                return;
            }

            BaseStatusEffect statusEffect;
            
            switch (provider.ActionName)
            {
                case "CorruptionDeBuff": statusEffect = GenerateStatusEffect<CorruptionDeBuff>(provider); break;
                case "BloodDrainBuff" : statusEffect = GenerateStatusEffect<BloodDrainBuff>(provider); break;
                case "RoarDeBuff" : statusEffect = GenerateStatusEffect<RoarDeBuff>(provider); break;
                case "FireballDeBuff" : statusEffect = GenerateStatusEffect<FireballDeBuff>(provider); break;
                
                default: return;
            }

            statusEffect.InvokeRoutine = StartCoroutine(statusEffect.MainAction());

            statusEffectTable.TryAdd(provider.ActionName, statusEffect);
        }

        public bool TryRemove(ICombatProvider provider) => TryRemove(provider.ActionName);
        public bool TryRemove(string key)
        {
            if (!statusEffectTable.TryGetValue(key, out var statusEffect)) return false;

            // TODO. 수정했다...근데 맞나? 테스트해봐야 한다.
            if (statusEffect.InvokeRoutine != null)
            {
                StopCoroutine(statusEffect.InvokeRoutine);
            }

            statusEffectTable.TryRemove(key);

            return true;
        }
        

        private T GenerateStatusEffect<T>(ICombatProvider provider) where T : BaseStatusEffect, new()
        {
            var statusEffectData = MainData.GetStatusEffectData(provider.ActionName);
            
            return new T
            {
                ActionName = provider.ActionName,
                ID = statusEffectData.ID,
                Duration = statusEffectData.Duration,
                TakerInfo = cb,
                ProviderInfo = provider,
                BaseData = statusEffectData,
                Callback = () => TryRemove(provider),
            };
        }

        private void Awake()
        {
            cb = GetComponentInParent<CharacterBehaviour>();
            cb.OnTakeDeBuff.Register(GetInstanceID(), TryAdd);
            cb.OnTakeBuff.Register(GetInstanceID(), TryAdd);
        }
    }
}
