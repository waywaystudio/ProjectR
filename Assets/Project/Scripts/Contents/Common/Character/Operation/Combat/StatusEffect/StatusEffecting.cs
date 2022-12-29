using System;
using System.Collections.Generic;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.StatusEffect
{
    using Buff;
    using DeBuff;
    
    [Serializable]
    public class StatusEffecting : MonoBehaviour
    {
        private CharacterBehaviour cb;
        private int instanceID;

        [ShowInInspector] public Dictionary<string, BaseStatusEffect> BuffTable { get; set; } = new();
        [ShowInInspector] public Dictionary<string, BaseStatusEffect> DeBuffTable { get; set; } = new();

        public void TryAdd(ICombatProvider provider)
        {
            BaseStatusEffect statusEffect;
            
            switch (provider.ActionName)
            {
                case "BloodDrainBuff" : statusEffect = GenerateStatusEffect<BloodDrainBuff>(provider); break;
                case "CorruptionDeBuff": statusEffect = GenerateStatusEffect<CorruptionDeBuff>(provider); break;
                case "FireballDeBuff" : statusEffect = GenerateStatusEffect<FireballDeBuff>(provider); break;
                case "FuryBuff" : statusEffect = GenerateStatusEffect<FuryBuff>(provider); break;
                case "RoarDeBuff" : statusEffect = GenerateStatusEffect<RoarDeBuff>(provider); break;
                
                default: return;
            }

            var targetTable = statusEffect.IsBuff
                ? BuffTable
                : DeBuffTable;
            
            if (targetTable.ContainsKey(provider.ActionName))
            {
                // Implement Compare
                return;
            }

            statusEffect.InvokeRoutine = StartCoroutine(statusEffect.MainAction());
            targetTable.TryAdd(provider.ActionName, statusEffect);
        }

        public bool TryRemove(ICombatProvider provider) => TryRemove(provider.ActionName);
        public bool TryRemove(string key)
        {
            var targetTable = key.EndsWith("DeBuff")
                ? DeBuffTable
                : BuffTable;
            
            if (!targetTable.TryGetValue(key, out var statusEffect)) return false;
            
            if (statusEffect.InvokeRoutine != null) 
                StopCoroutine(statusEffect.InvokeRoutine);

            targetTable.TryRemove(key);

            return true;
        }
        

        private T GenerateStatusEffect<T>(ICombatProvider provider) where T : BaseStatusEffect, new()
        {
            var statusEffectData = MainData.GetStatusEffect(provider.ActionName.ToEnum<IDCode>());
            
            return new T
            {
                BaseData = statusEffectData,
                ID = statusEffectData.ID,
                IsBuff = statusEffectData.IsBuff,
                Duration = statusEffectData.Duration,
                CombatValue = statusEffectData.CombatValue,
                Sender = provider,
                ActionName = provider.ActionName,
                TakerInfo = cb,
                Callback = () => TryRemove(provider),
            };
        }

        private void Awake()
        {
            instanceID = GetInstanceID();
            
            cb = GetComponentInParent<CharacterBehaviour>();
            cb.OnTakeStatusEffect.Register(instanceID, TryAdd);
        }
    }
}
