using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class OldCastingModule : OldCombatModule, IReady
    {
        [SerializeField] private float originalCastingTime;

        private bool onCasting;
        private float castingTick;
        private Coroutine routineBuffer;

        public bool IsReady => !onCasting;
        public float OriginalCastingTime { get => originalCastingTime; set => originalCastingTime = value; }
        public float CastingTime => OriginalCastingTime * CharacterUtility.GetHasteValue(CombatObject.Provider.StatTable.Haste);
        public OldObservable<float> CastingProgress { get; private set; } = new();


        private void StartCasting() => routineBuffer = StartCoroutine(Casting());
        private void BreakCasting()
        {
            if (!onCasting) return;
            
            onCasting = false;
            
            ResetProgress();
            
            if (routineBuffer != null) StopCoroutine(routineBuffer);
        }
        
        private IEnumerator Casting()
        {
            onCasting = true;

            while (CastingProgress.Value < CastingTime)
            {
                CastingProgress.Value += castingTick;
                yield return null;
            }
            
            ResetProgress();
            
            onCasting = false;
        }
        
        private void ResetProgress() => CastingProgress.Value = 0f;

        protected override void Awake()
        {
            base.Awake();
            
            castingTick = Time.deltaTime;
            
            CombatObject.OnActivated.Register(InstanceID, StartCasting);
            CombatObject.OnCanceled.Register(InstanceID, BreakCasting);
            CombatObject.ReadyCheckList.Add(this);
        }


#if UNITY_EDITOR
        public void SetUpValue(float castingTime)
        {
            Flag                = CombatModuleType.Casting;
            OriginalCastingTime = castingTime;
        }
#endif
    }
}