using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable NotAccessedField.Local

namespace Common.Character.Operation.Combat.Entity
{
    public class CoolTimeEntity : BaseEntity
    {
        [SerializeField] private float coolTime;
        
        private float remainTimer;
        private Coroutine resetCoroutine;

        public override bool IsReady => remainTimer <= 0.0f;
        public float CoolTime { get => coolTime; set => coolTime = value; }
        public float CoolTimeTick { get; set; }
        [ShowInInspector]
        public float RemainTimer
        {
            get => remainTimer;
            set => remainTimer = Mathf.Max(0, value);
        }


        private void ResetTimer() => resetCoroutine = StartCoroutine(ResetTimerRoutine());
        private IEnumerator ResetTimerRoutine()
        {
            RemainTimer = CoolTime;

            while (RemainTimer > 0)
            {
                RemainTimer -= CoolTimeTick;
                yield return null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            CoolTimeTick = Time.deltaTime;
        }

        private void OnEnable() => OnCompleted.Register(InstanceID, ResetTimer);
        private void OnDisable() => OnCompleted.Unregister(InstanceID);
    }
}
