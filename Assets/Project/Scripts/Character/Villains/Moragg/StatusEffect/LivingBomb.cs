using Common;
using Common.StatusEffect;
using Common.Systems;
using UnityEngine;

namespace Character.Villains.Moragg.StatusEffect
{
    public class LivingBomb : StatusEffectComponent, IProjectorSequencer
    {
        [SerializeField] private CollidingSystem collidingSystem;
        [SerializeField] private float interval;
        [SerializeField] private float radius = 6f;
        [SerializeField] private LayerMask adventurerLayer;
        
        private float hasteWeight;
        private float tickBuffer;

        public FloatEvent Progress => ProgressTime;
        public float CastWeightTime => Duration;
        public Vector2 SizeVector => new (radius * 2f, radius * 2f);
        

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active,"SetHasteWeight", SetHasteWeight)
                           .Add(SectionType.Complete,"Bomb", Bomb);
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

            takerList.ForEach(taker => executor.Execute(ExecuteGroup.Group2, taker));
        }
        
        private void SetHasteWeight() => hasteWeight = tickBuffer = 
            interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste);

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
                    executor.Execute(ExecuteGroup.Group1, Taker);
                    tickBuffer = hasteWeight;
                }
            }
            else
            {
                SequenceInvoker.Complete();
            }
        }
    }
}
