using Character.Graphic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class SkillComponent : CombatComponent
    {
        [SerializeField] private SkillData data;
        [SerializeField] protected AnimationModel animationModel;
        // [SerializeField] private SkillForge forge;


        [Button]
        public void UseSkill()
        {
            if (!moduleController.IsReadyToActive()) return;

            animationModel.OnHit.Unregister("SkillHit");
            animationModel.OnHit.Register("SkillHit", OnHit.Invoke);
            OnActivated.Invoke();
        }
        

        protected override void Init()
        {
            
        }

        protected virtual void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnCompleted.Register("IdleAnimation", animationModel.Idle);
        }

        protected void PlayAnimation()
        {
            animationModel.Play(data.AnimationKey, 0, false, data.CastingTime, OnCompleted.Invoke);
        }

        public void SetUp()
        {
            data.SetUp(ActionCode);
        }
        
        // private void ShowDB();
    }
}

