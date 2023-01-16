using UnityEngine;

namespace Character.Combat.Skill
{
    public class CommonAttack : SkillObject
    {
        private void OnCommonAttackHit()
        {
            if (TargetModule && DamageModule)
            {
                TargetModule.TakeDamage(DamageModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnHit.Register(InstanceID, OnCommonAttackHit);
        }
    }
}