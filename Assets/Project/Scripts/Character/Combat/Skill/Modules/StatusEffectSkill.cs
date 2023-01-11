using System.Collections.Generic;
using System.Linq;
using Character.Combat.StatusEffects;
using Core;
using UnityEngine;

namespace Character.Combat.Skill.Modules
{
    public class StatusEffectSkill : SkillModule, IStatusEffectModule, ICombatTable
    {
        [SerializeField] private DataIndex statusEffectID;
        [SerializeField] private List<StatusEffectObject> statusEffectPool; 
        
        public DataIndex StatusEffectID => statusEffectID;
        public StatTable StatTable => Provider.StatTable;
        
        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);

            statusEffectPool ??= GetComponentsInChildren<StatusEffectObject>().ToList();
        }

        public void Effecting(ICombatTaker taker)
        {
            // Pooling.Get use statusEffectCode
            var effect = GetEffect();
            effect.Initialize(Provider, taker);
        }


        private StatusEffectObject GetEffect()
        {
            if (statusEffectPool.HasElement()) return statusEffectPool[0];
            
            Debug.LogWarning("Pool Empty!");
            return null;
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