using System.Collections;
using Character.Projector;
using Character.Skill;
using Character.Targeting;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    public class LivingBomb : DamageOverTimeEffect
    {
        [SerializeField] private float radius = 12f;
        [SerializeField] private Colliding colliding;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private SphereProjector projector;
        [SerializeField] private ValueCompletion bombPower;

        public override void OnOverride() { }
        
        public override void Active(ICombatProvider provider, ICombatTaker taker)
        {
            base.Active(provider, taker);
            
            bombPower.Initialize(provider, ActionCode);
            
            projector.Initialize(0.25f, radius);
            projector.OnActivated.Invoke();
            projector.SetTaker(taker);
        }
        
        public override void Cancel()
        {
            projector.OnCanceled.Invoke();
            
            base.Cancel();
        }
        
        protected override void Complete()
        {
            colliding.TryGetTakersInSphere
            (
                Taker.Object.transform.position,
                radius,
                360f,
                adventurerLayer,
                out var takerList
            );
            
            bombPower.Damage(takerList);
            projector.OnCompleted.Invoke();
            
            base.Complete();
        }
        
        protected override void End()
        {
            projector.OnEnded.Invoke();
            
            base.End();
        }

        protected override IEnumerator Effectuating()
        {
            var hastedTick = interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            ProgressTime.Value = duration;

            while (ProgressTime.Value > 0)
            {
                var tickBuffer = hastedTick;

                while (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                    
                    yield return null;
                }
                
                tickPower.Damage(Taker);
            }

            Complete();
        }
    }
}
