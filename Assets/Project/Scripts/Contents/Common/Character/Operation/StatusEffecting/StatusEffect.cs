using System;
using System.Collections.Generic;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.StatusEffecting
{
    using Buff;
    using DeBuff;
    
    [Serializable]
    public class StatusEffect : MonoBehaviour
    {
        private CharacterBehaviour cb;
        
        [ShowInInspector]
        public Dictionary<string, BaseStatusEffect> StatusEffectTable = new ();
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public void TryAdd(string key, ICombatProvider provider)
        {
            if (StatusEffectTable.ContainsKey(key))
            {
                // Implement Compare
                return;
            }

            BaseStatusEffect statusEffect;
            
            switch (key)
            {
                case "CorruptionDeBuff": statusEffect = GenerateStatusEffect<CorruptionDeBuff>(key, provider); break;
                case "BloodDrainBuff" : statusEffect = GenerateStatusEffect<BloodDrainBuff>(key, provider); break;
                case "RoarDeBuff" : statusEffect = GenerateStatusEffect<RoarDeBuff>(key, provider); break;
                
                default: return;
            }

            statusEffect.InvokeRoutine = StartCoroutine(statusEffect.MainAction());

            StatusEffectTable.TryAdd(key, statusEffect);
        }

        public T GenerateStatusEffect<T>(string key, ICombatProvider provider) where T : BaseStatusEffect, new()
        {
            var statusEffectData = MainData.GetStatusEffectData(key);
            
            return new T
            {
                Name = key,
                ID = statusEffectData.ID,
                Duration = statusEffectData.Duration,
                TakerInfo = Cb,
                ProviderInfo = provider,
                BaseData = statusEffectData,
                Callback = () => Remove(key),
            };
        }

        public void Remove(string key) => StatusEffectTable.TryRemove(key);
        public bool TryDispel(string key)
        {
            if (!StatusEffectTable.TryGetValue(key, out var statusEffect)) return false;

            // TODO. 수정했다...근데 맞나? 테스트해봐야 한다.
            if (statusEffect.InvokeRoutine != null)
            {
                StopCoroutine(statusEffect.InvokeRoutine);
            }

            StatusEffectTable.TryRemove(key);

            return true;
        }

        private void Awake()
        {
            Cb.OnTakeDeBuff.Register(GetInstanceID(), TryAdd);
            Cb.OnTakeBuff.Register(GetInstanceID(), TryAdd);
        }
    }
}
