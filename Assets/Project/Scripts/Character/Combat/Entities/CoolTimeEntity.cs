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

        public override bool IsReady => RemainTime.Value <= 0.0f;
        public float CoolTime { get => coolTime; set => coolTime = value; }
        public Observable<float> RemainTime { get; } = new();
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
            RemainTime.Value = CoolTime;

            while (RemainTime.Value > 0)
            {
                RemainTime.Value -= CoolTimeTick;
                yield return null;
            }

            RemainTime.Value = 0f;
        }

        private void OnEnable() => OnCompleted.Register(InstanceID, ResetTimer);
        private void OnDisable() => OnCompleted.Unregister(InstanceID);
    }
}
