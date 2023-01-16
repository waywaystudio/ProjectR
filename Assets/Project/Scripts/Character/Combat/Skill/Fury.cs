using UnityEngine;

namespace Character.Combat.Skill
{
    public class Fury : SkillObject
    {
        private void OnFuryActivated()
        {
            Debug.Log("FuryActivated");
            
            if (TargetModule && StatusEffectModule)
            {
                
                TargetModule.TakeStatusEffect(StatusEffectModule);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnActivated.Register(InstanceID, OnFuryActivated);
        }
    }
}
