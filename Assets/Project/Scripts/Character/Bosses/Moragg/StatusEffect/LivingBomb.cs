using System.Collections;
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
        [SerializeField] private PowerCompletion tickPower;
        [SerializeField] private float interval;
        
        [SerializeField] private float radius = 12f;
        [SerializeField] private float stunDuration = 5f;
        [SerializeField] private CollidingSystem collidingSystem;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private SphereProjector projector;
        [SerializeField] private PowerCompletion bombPower;

        public override void Active(ICombatProvider provider, ICombatTaker taker)
        {
            projector.Initialize(0.25f, radius);
            projector.AssignTo(this);
            projector.SetTaker(taker);
            
            base.Active(provider, taker);
            
            tickPower.Initialize(provider, ActionCode);
            bombPower.Initialize(provider, ActionCode);
        }
        
        public override void OnOverride()
        {
            ProgressTime.Value += duration;
        }
        

        protected void Bomb()
        {
            collidingSystem.TryGetTakersInSphere
            (
                Taker.gameObject.transform.position,
                radius,
                360f,
                adventurerLayer,
                out var takerList
            );
            
            // TODO. 함수 구조를 수정하여 데미지 주는 주객 순서와 기절의 주객 순서를 통일하면 좋겠다.
            takerList.ForEach(taker =>
            {
                bombPower.Damage(taker);
                taker.Stun(stunDuration);
            });
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
        
        private void OnEnable()
        {
            OnCompleted.Register("Bomb", Bomb);
        }

        private void OnDisable()
        {
            OnCompleted.Unregister("Bomb");
        }
    }
}
