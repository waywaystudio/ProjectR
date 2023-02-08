using System.Collections;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class GlobalCoolTimeModule : CombatModule
    {
        [SerializeField] private float globalCoolTime = 1.5f;

        private float tick;
        private Coroutine routineBuffer;
        
        public bool IsReady => RemainTime.Value <= 0.0f;
        public Observable<float> RemainTime { get; } = new();

        public override void Initialize(CombatComponent combatComponent)
        {
            tick = Time.deltaTime;

            var actionName = $"{combatComponent.ActionCode.ToString()} GlobalCoolTime";
            
            combatComponent.OnActivated.Register(actionName, Cooling);
        }
        
        
        private void Cooling()
        {
            routineBuffer = StartCoroutine(CoolingRoutine());
        }
        
        private IEnumerator CoolingRoutine()
        {
            RemainTime.Value = globalCoolTime;

            while (RemainTime.Value > 0)
            {
                RemainTime.Value -= tick;
                yield return null;
            }

            RemainTime.Value = 0f;
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue()
        {
            moduleType = CombatModuleType.GlobalCoolTime;
        }
#endif
    }
}
