using System;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class CoolTimeEntity : EntityAttribution, IReadyRequired
    {
        private float remainCoolTime;

        public bool IsReady => remainCoolTime <= 0.0f;
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

        public void SetEntity()
        {
            CoolTime = SkillData.BaseCoolTime;
        }

        private void Awake()
        {
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
