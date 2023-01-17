using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CastingModule : CombatModule, IReady
    {
        [SerializeField] private float originalCastingTime;

        private bool onCasting;
        private float castingTick;
        private Coroutine routineBuffer;

        public ICombatProvider Provider => CombatObject.Provider;
        public bool IsReady => !onCasting;
        public float OriginalCastingTime { get => originalCastingTime; set => originalCastingTime = value; }
        public float CastingTime => OriginalCastingTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
        public float CastingProgress { get; private set; }


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

            while (CastingProgress < CastingTime)
            {
                CastingProgress += castingTick;
                yield return null;
            }
            
            ResetProgress();
            
            onCasting = false;
        }
        
        private void ResetProgress() => CastingProgress = 0f;

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
            Flag                = ModuleType.Casting;
            OriginalCastingTime = castingTime;
        }
#endif
    }
}
