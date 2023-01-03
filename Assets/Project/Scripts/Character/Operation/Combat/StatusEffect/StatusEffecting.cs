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

        [ShowInInspector] public Dictionary<IDCode, BaseStatusEffect> BuffTable { get; set; } = new();
        [ShowInInspector] public Dictionary<IDCode, BaseStatusEffect> DeBuffTable { get; set; } = new();

        public void TryAdd(ICombatProvider provider)
        {
            BaseStatusEffect statusEffect;
            
            switch (provider.ActionCode)
            {
                case IDCode.BloodDrainBuff : statusEffect = GenerateStatusEffect<BloodDrainBuff>(provider); break;
                case IDCode.CorruptionDeBuff : statusEffect = GenerateStatusEffect<CorruptionDeBuff>(provider); break;
                case IDCode.FireballDeBuff : statusEffect = GenerateStatusEffect<FireballDeBuff>(provider); break;
                case IDCode.FuryBuff : statusEffect = GenerateStatusEffect<FuryBuff>(provider); break;
                case IDCode.RoarDeBuff : statusEffect = GenerateStatusEffect<RoarDeBuff>(provider); break;
                
                default: return;
            }

            var targetTable = statusEffect.IsBuff
                ? BuffTable
                : DeBuffTable;
            
            if (targetTable.ContainsKey(provider.ActionCode))
            {
                // Implement Compare
                return;
            }

            statusEffect.InvokeRoutine = StartCoroutine(statusEffect.MainAction());
            targetTable.TryAdd(provider.ActionCode, statusEffect);
        }

        public bool TryRemove(ICombatProvider provider) => TryRemove(provider.ActionCode);
        public bool TryRemove(IDCode key)
        {
            var targetTable = key.ToString().EndsWith("DeBuff")
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
            var statusEffectData = MainData.GetStatusEffect(provider.ActionCode);
            
            return new T
            {
                BaseData = statusEffectData,
                ID = statusEffectData.ID,
                IsBuff = statusEffectData.IsBuff,
                Duration = statusEffectData.Duration,
                CombatValue = statusEffectData.CombatValue,
                Sender = provider,
                ActionCode = provider.ActionCode,
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
