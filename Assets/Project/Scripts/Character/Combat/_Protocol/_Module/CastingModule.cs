using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CastingModule : CombatModule
    {
        [SerializeField] private float castingTime;
        
        private float tick;
        private Coroutine routineBuffer;

        public ICombatProvider Provider { get; set; }
        public bool OnCasting { get; private set; }
        public Observable<float> CastingProgress { get; } = new();

        public override void Initialize(CombatComponent combatComponent)
        {
            tick       = Time.deltaTime;
            Provider   = combatComponent.Provider;
            
            var actionName = $"{combatComponent.ActionCode.ToString()} Casting";
            
            combatComponent.OnActivated.Register(actionName, StartCasting);
            combatComponent.OnCanceled.Register(actionName, BreakCasting);
        }


        private void StartCasting()
        {
            OnCasting     = true;
            routineBuffer = StartCoroutine(Casting());
        }
        
        private void BreakCasting()
        {
            if (!OnCasting) return;
            
            ResetProgress();
            StopCoroutine(routineBuffer);
        }
        
        private IEnumerator Casting()
        {
            var endTime = castingTime * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            while (CastingProgress.Value < endTime)
            {
                CastingProgress.Value += tick;
                yield return null;
            }
            
            ResetProgress();
        }

        private void ResetProgress()
        {
            OnCasting             = false;
            CastingProgress.Value = 0f;
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue(float castingTime)
        {
            moduleType       = CombatModuleType.Casting;
            this.castingTime = castingTime;
        }
#endif
    }
}
