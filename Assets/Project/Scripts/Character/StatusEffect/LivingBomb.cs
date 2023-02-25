using System.Collections;
using Character.Projector;
using Character.Skill;
using Character.Targeting;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    public class LivingBomb : StatusEffectComponent
    {
        [SerializeField] private float interval;
        [SerializeField] private float radius = 12f;
        [SerializeField] private Colliding colliding;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private SphereProjector projector;
        [SerializeField] private ValueCompletion tickPower;
        [SerializeField] private ValueCompletion bombPower;

        public override void OnOverride() { }
        
        public override void Active(ICombatProvider provider, ICombatTaker taker)
        {
            projector.SetTaker(taker);
            
            base.Active(provider, taker);
        }

        protected override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);
            
            tickPower.Initialize(provider, ActionCode);
            bombPower.Initialize(provider, ActionCode);
            projector.Initialize(0.25f, radius);
            projector.AssignTo(this);
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
            
            base.Complete();
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
