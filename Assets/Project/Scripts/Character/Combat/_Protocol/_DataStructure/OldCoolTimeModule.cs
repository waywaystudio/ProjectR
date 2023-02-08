using System.Collections;
using Core;
using UnityEngine;
// ReSharper disable NotAccessedField.Local

namespace Character.Combat
{
    public class OldCoolTimeModule : OldCombatModule, IReady
    {
        [SerializeField] private float coolTime;

        private float tickBuffer;
        private Coroutine routineBuffer;

        public bool IsReady => RemainTime.Value <= 0.0f;
        public float OriginalCoolTime { get => coolTime; set => coolTime = value; }
        public Observable<float> RemainTime { get; } = new();


        private void Cooling()
        {
            routineBuffer = StartCoroutine(CoolingRoutine());
        }

        private IEnumerator CoolingRoutine()
        {
            RemainTime.Value = OriginalCoolTime;

            while (RemainTime.Value > 0)
            {
                RemainTime.Value -= tickBuffer;
                yield return null;
            }

            RemainTime.Value = 0f;
        }

        protected override void Awake()
        {
            base.Awake();
            
            tickBuffer = Time.deltaTime;
            CombatObject.OnCompleted.Register(InstanceID, Cooling);
            CombatObject.ReadyCheckList.Add(this);
        }

#if UNITY_EDITOR
        public void SetUpValue(float coolTime)
        {
            Flag     = CombatModuleType.CoolTime;
            OriginalCoolTime = coolTime;
        }
#endif
        
    }
}
