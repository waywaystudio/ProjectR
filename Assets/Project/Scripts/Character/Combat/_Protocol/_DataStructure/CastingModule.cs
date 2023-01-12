using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CastingModule : Module, ICastingModule, IOnStarted, IOnInterrupted, IReady
    {
        [SerializeField] private float originalCastingTime;

        private bool onCasting;
        private float castingTick;
        private Coroutine routineBuffer;

        public ActionTable OnStarted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public bool IsReady => !onCasting;
        
        public float OriginalCastingTime { get => originalCastingTime; set => originalCastingTime = value; }
        public float CastingTime => OriginalCastingTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
        public float CastingProgress { get; private set; }


        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);

            castingTick = Time.deltaTime;
            OnStarted.Register(InstanceID, StartCasting);
            OnInterrupted.Register(InstanceID, BreakCasting);
        }


        private void StartCasting() => routineBuffer = StartCoroutine(Casting());
        private void BreakCasting()
        {
            onCasting = false;
            
            ResetProgress();
            
            if (routineBuffer != null) StopCoroutine(routineBuffer);
        }
        
        private IEnumerator Casting()
        {
            onCasting = true;

            while (CastingProgress < CastingTime)
            {
                CastingProgress += castingTick;
                yield return null;
            }
            
            ResetProgress();
            
            onCasting = false;
        }
        
        private void ResetProgress() => CastingProgress = 0f;



#if UNITY_EDITOR
        public void SetUpValue(float castingTime)
        {
            Flag                = ModuleType.Casting;
            OriginalCastingTime = castingTime;
        }
#endif
    }
}
