using System;
using System.Collections;
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

        private float CastingTime => originalCastingTime * CharacterUtility.GetHasteValue(Cb.CombatValue.Haste);
        private float CastingTick { get => castingTick; set => castingTick = value; }
        private float RemainTimer
        {
            get => remainTimer; 
            set => remainTimer = Mathf.Max(0, value);
        }
        
        public override bool IsReady => !onCasting;

        public override void SetEntity()
        {
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
                AssignedSkill.InterruptedSkill();
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
            AssignedSkill.OnStarted.Register(InstanceID, StartCasting);
            AssignedSkill.OnInterrupted.Register(InstanceID, BreakCasting);
        }

        private void OnDisable()
        {
            AssignedSkill.OnStarted.Unregister(InstanceID);
            AssignedSkill.OnInterrupted.Unregister(InstanceID);
        }

        
        private void Reset()
        {
            flag = EntityType.Casting;

            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName); 
            originalCastingTime = skillData.CastingTime;
        }
    }
}
