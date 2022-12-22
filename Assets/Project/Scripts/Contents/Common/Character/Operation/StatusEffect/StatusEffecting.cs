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

        [ShowInInspector] public Dictionary<string, BaseStatusEffect> BuffTable { get; set; } = new();
        [ShowInInspector] public Dictionary<string, BaseStatusEffect> DeBuffTable { get; set; } = new();

        public void TryAdd(ICombatProvider provider)
        {
            if (BuffTable.ContainsKey(provider.ActionName))
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

            if (statusEffect.IsBuff) BuffTable.TryAdd(provider.ActionName, statusEffect);
            else 
                DeBuffTable.TryAdd(provider.ActionName, statusEffect);
           
        }

        public bool TryRemove(ICombatProvider provider) => TryRemove(provider.ActionName);
        public bool TryRemove(string key)
        {
            if (!BuffTable.TryGetValue(key, out var statusEffect)) return false;

            // TODO. 수정했다...근데 맞나? 테스트해봐야 한다.
            if (statusEffect.InvokeRoutine != null)
            {
                StopCoroutine(statusEffect.InvokeRoutine);
            }

            BuffTable.TryRemove(key);

            return true;
        }
        

        private T GenerateStatusEffect<T>(ICombatProvider provider) where T : BaseStatusEffect, new()
        {
            var statusEffectData = MainData.GetStatusEffectData(provider.ActionName);
            
            return new T
            {
                BaseData = statusEffectData,
                ID = statusEffectData.ID,
                IsBuff = statusEffectData.IsBuff,
                Duration = statusEffectData.Duration,
                ProviderInfo = provider,
                ActionName = provider.ActionName,
                TakerInfo = cb,
                Callback = () => TryRemove(provider),
            };
        }

        private void Awake()
        {
            cb = GetComponentInParent<CharacterBehaviour>();
            cb.OnTakeStatusEffect.Register(GetInstanceID(), TryAdd);
        }
    }
}
