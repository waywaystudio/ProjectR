using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Character.Combat
{
    using StatusEffects;
    
    public class StatusEffectModule : CombatModule
    {
        [SerializeField] private DataIndex statusEffectID;
        [SerializeField] private List<StatusEffectObject> statusEffectPool;
        
        public ICombatProvider Provider => CombatObject.Provider;


        public void Effectuate(ICombatTaker taker)
        {
            // Pooling.Get use statusEffectCode
            var effect = GetEffect();
            effect.Effectuate(Provider, taker);
        }


        private StatusEffectObject GetEffect()
        {
            if (statusEffectPool.HasElement()) return statusEffectPool[0];
            
            Debug.LogWarning("Pool Empty!");
            return null;
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            statusEffectPool ??= GetComponentsInChildren<StatusEffectObject>().ToList();
        }


#if UNITY_EDITOR
        public void SetUpValue(DataIndex effectCode)
        {
            Flag             = ModuleType.StatusEffect;
            statusEffectID = effectCode;
            
            if (statusEffectPool.IsNullOrEmpty())
                statusEffectPool = GetComponentsInChildren<StatusEffectObject>().ToList();
        }
#endif
    }
}