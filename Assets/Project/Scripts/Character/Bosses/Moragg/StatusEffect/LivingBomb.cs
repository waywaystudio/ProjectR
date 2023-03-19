using Common;
using Common.Completion;
using Common.Projectors;
using Common.StatusEffect;
using Common.Systems;
using UnityEngine;

namespace Monsters.Moragg.StatusEffect
{
    public class LivingBomb : StatusEffectComponent
    {
        [SerializeField] private SphereProjector projector;
        [SerializeField] private StatusEffectDamageCompletion tickDamage;
        [SerializeField] private CollidingSystem collidingSystem;
        
        [SerializeField] private float interval;
        [SerializeField] private float radius = 12f;
        [SerializeField] private float stunDuration = 5f;
        [SerializeField] private LayerMask adventurerLayer;
        
        private float hasteWeight;
        private float tickBuffer;
        

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            projector.Initialize(Duration, radius, this);
            
            OnActivated.Register("SetHasteWeight", SetHasteWeight);
            OnCompleted.Register("Bomb", Bomb);
        }
        
        public override void Dispose()
        {
            projector.Dispose();
            
            Destroy(gameObject);
        }


        private void Bomb()
        {
            collidingSystem.TryGetTakersInSphere
            (
                Taker.gameObject.transform.position,
                radius,
                360f,
                adventurerLayer,
                out var takerList
            );

            takerList.ForEach(taker =>
            {
                tickDamage.Completion(taker, 5f);
                taker.Stun(stunDuration);
            });
        }
        
        private void SetHasteWeight() => hasteWeight = tickBuffer = 
            interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

        private void Update()
        {
            if (ProgressTime.Value > 0)
            {
                if (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                }
                else
                {
                    tickDamage.Completion(Taker);
                    tickBuffer = hasteWeight;
                }
            }
            else
            {
                Complete();
            }
        }
    }
}
