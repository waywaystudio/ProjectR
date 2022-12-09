using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class CoolTimeEntity : BaseEntity, IReadyRequired
    {
        private float remainTimer;

        public override bool IsReady => remainTimer <= 0.0f;
        [ShowInInspector]
        public float CoolTime { get; set; }
        public float CoolTimeTick { get; set; }
        [ShowInInspector]
        public float RemainTimer
        {
            get => remainTimer;
            set => remainTimer = Mathf.Max(0, value);
        }


        protected void SetEntity()
        {
            CoolTime = SkillData.BaseCoolTime;
            RemainTimer = CoolTime;
        }
        
        private void UpdateStatus() => IsReady.OnFalse(() => RemainTimer -= CoolTimeTick);
        private void ResetRemainTime() => RemainTimer = CoolTime;

        protected override void Awake()
        {
            base.Awake();

            SetEntity();
            CoolTimeTick = Time.deltaTime;
        }

        private void OnEnable()
        {
            Cb.OnUpdate += UpdateStatus;
            Skill.OnCompleted += ResetRemainTime;
        }
        
        private void OnDisable()
        { 
            Cb.OnUpdate -= UpdateStatus;
            Skill.OnCompleted -= ResetRemainTime;
        }

        private void Reset()
        {
            flag = EntityType.CoolTime;
            SetEntity();
        }
    }
}
