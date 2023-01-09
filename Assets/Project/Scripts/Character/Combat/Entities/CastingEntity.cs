using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class CastingEntity : BaseEntity
    {
        [SerializeField] private float originalCastingTime;

        private bool onCasting;
        private float castingTick;

        public float OriginalCastingTime { get => originalCastingTime; set => originalCastingTime = value; }
        public float CastingTime => OriginalCastingTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
        public float CastingProgress { get; private set; }
        public override bool IsReady => !onCasting;

        private Coroutine RoutineBuffer { get; set; }

        private float CastingTick
        {
            get
            {
                if (castingTick == 0.0f)
                    castingTick = Time.deltaTime;
                
                return castingTick;
            }
        }

        private void StartCasting() => RoutineBuffer = StartCoroutine(Casting());
        private void BreakCasting()
        {
            onCasting = false;
            
            ResetProgress();
            
            if (RoutineBuffer != null) StopCoroutine(RoutineBuffer);
        }
        
        private IEnumerator Casting()
        {
            onCasting = true;

            while (CastingProgress < CastingTime)
            {
                CastingProgress += CastingTick;
                yield return null;
            }
            
            ResetProgress();
            
            onCasting = false;
        }
        
        private void ResetProgress() => CastingProgress = 0f;

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
        
        
#if UNITY_EDITOR
        public void SetUpValue(float castingTime)
        {
            OriginalCastingTime = castingTime;
            Flag                = EntityType.Casting;
        }
#endif
    }
}
