using System.Collections;
using Character.Projector;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    public class LivingBomb : DamageOverTimeEffect
    {
        [SerializeField] private float radius = 12f;
        [SerializeField] private SphereProjector projector;

        public override void OnOverride() { }
        
        public override void Active(ICombatTaker taker)
        {
            base.Active(taker);
            
            projector.OnActivated.Invoke();
            projector.SetTaker(taker);
        }
        
        public override void Dispel()
        {
            projector.OnInterrupted.Invoke();
            
            base.Dispel();
        }
        
        protected override void Complete()
        {
            projector.OnCompleted.Invoke();
            
            base.Complete();
        }
        
        protected override void End()
        {
            projector.OnEnded.Invoke();
            
            base.End();
        }

        protected override void Init()
        {
            projector.Initialize(0.25f, radius);
        }

        protected override IEnumerator Effectuating(ICombatTaker taker)
        {
            ProcessTime.Value = duration;

            while (ProcessTime.Value > 0)
            {
                ProcessTime.Value -= Time.deltaTime;
                
                yield return null;
            }
            
            UpdateDoT();
            
            // 범위안에 있는 Character를 모두 구한 후,
            taker.TakeDamage(this);

            Complete();
        }
    }
}
