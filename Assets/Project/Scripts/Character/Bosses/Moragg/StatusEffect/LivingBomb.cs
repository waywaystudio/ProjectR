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
        [SerializeField] private DamageCompletion tickDamage;
        [SerializeField] private DamageCompletion bombDamage;
        [SerializeField] private float interval;
        
        [SerializeField] private float radius = 12f;
        [SerializeField] private float stunDuration = 5f;
        [SerializeField] private CollidingSystem collidingSystem;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private SphereProjector projector;
        
        private float hasteWeight;
        private float tickBuffer;
        

        public override void Initialized(ICombatProvider provider)
        {
            base.Initialized(provider);
            
            tickDamage.Initialize(provider, ActionCode);
            bombDamage.Initialize(provider, ActionCode);
            projector.Initialize(0.25f, radius);
            projector.AssignTo(this);
            OnCompleted.Register("Bomb", Bomb);
        }

        public override void Execution(ICombatTaker taker)
        {
            base.Execution(taker);
            
            hasteWeight = tickBuffer = 
                interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);
            
            projector.SetTaker(taker);
        }
        
        public override void Disposed()
        {
            OnCompleted.Unregister("Bomb");
            
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
            
            // TODO. 함수 구조를 수정하여 데미지 주는 주객 순서와 기절의 주객 순서를 통일하면 좋겠다.
            takerList.ForEach(taker =>
            {
                bombDamage.Damage(taker);
                taker.Stun(stunDuration);
            });
        }

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
                    tickDamage.Damage(Taker);
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
