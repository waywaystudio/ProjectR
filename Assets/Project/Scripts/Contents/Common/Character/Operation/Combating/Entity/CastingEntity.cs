using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    [Serializable]
    public class CastingEntity : BaseEntity
    {
        private bool onCasting;
        [ShowInInspector]
        private float remainTimer;
        [ShowInInspector]
        private float CastingTime { get; set; }
        private float CastingTick { get; set; }
        private float RemainTimer
        {
            get => remainTimer; 
            set => remainTimer = Mathf.Max(0, value);
        }
        
        public override bool IsReady => !onCasting;

        private void StartCasting() => StartCoroutine(Casting());
        private void BreakCasting()
        {
            onCasting = false;
            
            ResetRemainTimer();
            StopAllCoroutines();
        }
        
        private IEnumerator Casting()
        {
            if (onCasting)
            {
                Debug.Log("Casting Interrupted");
                Skill.InterruptedSkill();
            }

            onCasting = true;

            while (RemainTimer > 0f)
            {
                RemainTimer -= CastingTick;
                yield return null;
            }
            
            onCasting = false;
            
            ResetRemainTimer();
        }
        
        private void ResetRemainTimer() => RemainTimer = CastingTime;

        protected override void Awake()
        {
            base.Awake();

            CastingTime = SkillData.CastingTime;
            RemainTimer = CastingTime;
            CastingTick = Time.deltaTime;
        }

        private void OnEnable()
        {
            Skill.OnStarted.Register(InstanceID, StartCasting);
            Skill.OnInterrupted.Register(InstanceID, BreakCasting);
        }

        private void OnDisable()
        {
            Skill.OnStarted.UnRegister(InstanceID);
            Skill.OnInterrupted.UnRegister(InstanceID);
        }

        private void Reset()
        {
            flag = EntityType.Casting;
        }
    }
}
