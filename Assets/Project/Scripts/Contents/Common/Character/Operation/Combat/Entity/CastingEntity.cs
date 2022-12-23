using System;
using System.Collections;
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

        private float CastingTime => originalCastingTime * CharacterUtility.GetHasteValue(Sender.StatTable.Haste);
        private float CastingTick { get => castingTick; set => castingTick = value; }
        private float RemainTimer
        {
            get => remainTimer; 
            set => remainTimer = Mathf.Max(0, value);
        }
        
        public override bool IsReady => !onCasting;

        public override void SetEntity()
        {
            originalCastingTime = Data.CastingTime;
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

        private void OnEnable()
        {
            OnStarted.Register(InstanceID, StartCasting);
            OnInterrupted.Register(InstanceID, BreakCasting);
        }

        private void OnDisable()
        {
            OnStarted.Unregister(InstanceID);
            OnInterrupted.Unregister(InstanceID);
        }

        
        private void Reset()
        {
            flag = EntityType.Casting;
            SetEntity();
        }
    }
}
