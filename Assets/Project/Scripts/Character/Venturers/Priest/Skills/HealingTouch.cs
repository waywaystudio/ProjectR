using Common;
using Common.Execution.Hits;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class HealingTouch : SkillComponent
    {
        [SerializeField] private HealHit healExecution;

        private Vector3 predicatePosition = Vector3.zero;
        private readonly Collider[] buffers = new Collider[32];

        public HealHit HealExecution => healExecution;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("SavePredicatePosition", TargetHealing)
                .Add(Section.Execute, "ExecuteHealingTouch",ExecuteHealingTouch)
                .Add(Section.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void ExecuteHealingTouch()
        {
            if (detector.TryGetTakersInCircle(predicatePosition, AreaRange, buffers, out var takers))
            {
                takers.ForEach(taker =>
                {
                    Taker = taker;
                    Invoker.Hit(taker);
                });
            }
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }

        private void TargetHealing(Vector3 targetPosition)
        {
            predicatePosition = TargetUtility.GetValidPosition(Cb.Position, PivotRange, targetPosition);
        }
    }
}
