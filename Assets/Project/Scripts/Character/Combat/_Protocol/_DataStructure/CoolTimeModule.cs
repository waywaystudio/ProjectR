using System.Collections;
using Core;
using UnityEngine;

// ReSharper disable NotAccessedField.Local

namespace Character.Combat
{
    public class CoolTimeModule : Module, ICoolModule, IOnCompleted, IReady
    {
        [SerializeField] private float coolTime;

        private float tickBuffer;
        private Coroutine routineBuffer;

        public ActionTable OnCompleted { get; } = new();
        public bool IsReady => RemainTime.Value <= 0.0f;
        public float OriginalCoolTime { get => coolTime; set => coolTime = value; }
        public Observable<float> RemainTime { get; } = new();


        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);

            tickBuffer = Time.deltaTime;
            OnCompleted.Register(InstanceID, Cooling);
        }


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



#if UNITY_EDITOR
        public void SetUpValue(float coolTime)
        {
            Flag     = ModuleType.CoolTime;
            OriginalCoolTime = coolTime;
        }
#endif
    }
}
