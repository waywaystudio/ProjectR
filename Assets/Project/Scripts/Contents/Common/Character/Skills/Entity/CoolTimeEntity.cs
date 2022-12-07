using System;
using Common.Character.Skills.Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class CoolTimeEntity : EntityAttribution, IReadyRequired
    {
        private float remainCoolTime;

        public override bool IsReady => remainCoolTime <= 0.0f;
        public float CoolTime { get; set; }
        public float CoolTimeTick { get; set; }

        public float RemainCoolTime
        {
            get => remainCoolTime;
            set => remainCoolTime = Mathf.Max(0, value);
        }

        public void UpdateStatus()
        {
            if (IsReady) return;
            
            RemainCoolTime -= CoolTimeTick;
        }

        protected override void SetEntity()
        {
            CoolTime = SkillData.BaseCoolTime;
        }

        protected override void Awake()
        {
            base.Awake();
            
            CoolTimeTick = Time.deltaTime;
        }

        private void OnEnable()
        {
            Cb.OnUpdate += UpdateStatus;
        }

        private void OnDisable()
        {
            Cb.OnUpdate -= UpdateStatus;
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            Flag = EntityType.CoolTime;

            SetEntity();
        }
#endif
    }
}
