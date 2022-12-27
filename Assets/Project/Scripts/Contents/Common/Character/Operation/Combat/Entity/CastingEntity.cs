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

        public float OriginalCastingTime { get => originalCastingTime; set => originalCastingTime = value; }
        public float CastingTime => originalCastingTime * CharacterUtility.GetHasteValue(Sender.StatTable.Haste);
        public float CastingProgress { get; private set; }
        private float CastingTick { get => castingTick; set => castingTick = value; }

        public override bool IsReady => !onCasting;
        

        private void StartCasting() => StartCoroutine(Casting());
        private void BreakCasting()
        {
            onCasting = false;
            
            ResetProgress();
            StopAllCoroutines();
        }
        
        private IEnumerator Casting()
        {
            onCasting = true;

            while (CastingProgress < CastingTime)
            {
                CastingProgress += CastingTick;
                yield return null;
            }
            
            onCasting = false;
            
            ResetProgress();
        }
        
        private void ResetProgress() => CastingProgress = 0f;

        protected override void Awake()
        {
            base.Awake();
            CastingTick = Time.deltaTime;
        }

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
    }
}
