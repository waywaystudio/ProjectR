using System;
using System.Collections;
using MainGame.Manager.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    [Serializable]
    public class CastingEntity : BaseEntity
    {
        [SerializeField] private float originalCastingTime;
        private float castingTick;
        private bool onCasting;
        private float remainTimer;

        private float CastingTime => originalCastingTime * CombatManager.GetHasteValue(Cb.Haste);
        private float CastingTick { get => castingTick; set => castingTick = value; }
        private float RemainTimer
        {
            get => remainTimer; 
            set => remainTimer = Mathf.Max(0, value);
        }
        
        public override bool IsReady => !onCasting;

        public override void SetEntity()
        {
            originalCastingTime = SkillData.CastingTime;
            RemainTimer = 0;
            CastingTick = Time.deltaTime;
        }

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

            SetEntity();
        }

        private void OnEnable()
        {
            Skill.OnStarted.Register(InstanceID, StartCasting);
            Skill.OnInterrupted.Register(InstanceID, BreakCasting);
        }

        private void OnDisable()
        {
            Skill.OnStarted.Unregister(InstanceID);
            Skill.OnInterrupted.Unregister(InstanceID);
        }

        private void Reset()
        {
            flag = EntityType.Casting;
        }
    }
}
