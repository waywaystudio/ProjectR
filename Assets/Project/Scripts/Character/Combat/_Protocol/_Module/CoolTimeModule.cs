using System.Collections;
using Core;
using UnityEngine;
// ReSharper disable NotAccessedField.Local

namespace Character.Combat
{
    public class CoolTimeModule : CombatModule
    {
        [SerializeField] private float coolTime;

        private float tick;
        private Coroutine routineBuffer;
        
        public bool IsReady => RemainTime.Value <= 0.0f;
        public Observable<float> RemainTime { get; } = new();

        public override void Initialize(CombatComponent combatComponent)
        {
            tick = Time.deltaTime;

            var actionName = $"{combatComponent.ActionCode.ToString()} CoolTime";
            
            combatComponent.OnCompleted.Register(actionName, Cooling);
        }
        
        
        private void Cooling()
        {
            routineBuffer = StartCoroutine(CoolingRoutine());
        }
        
        private IEnumerator CoolingRoutine()
        {
            RemainTime.Value = coolTime;

            while (RemainTime.Value > 0)
            {
                RemainTime.Value -= tick;
                yield return null;
            }

            RemainTime.Value = 0f;
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue(float coolTime)
        {
            moduleType    = CombatModuleType.CoolTime;
            this.coolTime = coolTime;
        }
#endif
    }
}
