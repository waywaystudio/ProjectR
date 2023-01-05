using System.Collections;
using Core;
using UnityEngine;
// ReSharper disable NotAccessedField.Local

namespace Character.Combat.Entities
{
    public class CoolTimeEntity : BaseEntity
    {
        [SerializeField] private float coolTime;
        
        private float coolTimeTick;
        private Coroutine routineBuffer;

        public override bool IsReady => RemainTimer.Value <= 0.0f;
        public float CoolTime { get => coolTime; set => coolTime = value; }
        public Observable<float> RemainTimer { get; } = new();
        public float CoolTimeTick
        {
            get
            {
                if (coolTimeTick == 0.0f)
                    coolTimeTick = Time.deltaTime;

                return coolTimeTick;
            }
        }


        private void ResetTimer() => routineBuffer = StartCoroutine(ResetTimerRoutine());
        private IEnumerator ResetTimerRoutine()
        {
            RemainTimer.Value = CoolTime;

            while (RemainTimer.Value > 0)
            {
                RemainTimer.Value -= CoolTimeTick;
                yield return null;
            }

            RemainTimer.Value = 0f;
        }

        private void OnEnable() => OnCompleted.Register(InstanceID, ResetTimer);
        private void OnDisable() => OnCompleted.Unregister(InstanceID);
    }
}
