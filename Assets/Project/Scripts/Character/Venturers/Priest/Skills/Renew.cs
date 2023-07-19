using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class Renew : SkillComponent
    {
        private Vector3 predicatePosition = Vector3.zero;
        private readonly Collider[] buffers = new Collider[32];
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("SetTargetPosition", SetTargetPosition)
                .Add(Section.Execute, "ExecuteRenew", ExecuteRenew)
                .Add(Section.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void SetTargetPosition(Vector3 targetPosition)
        {
            predicatePosition = TargetUtility.GetValidPosition(Cb.Position, PivotRange, targetPosition);
        }
        
        private void ExecuteRenew()
        {
            if (!detector.TryGetTakersInCircle(predicatePosition, AreaRange, buffers, out var takers)) return;
            takers.Sort(predicatePosition, SortingType.DistanceAscending);

            Taker = takers.FirstOrDefault();
            Invoker.Hit(Taker);
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }
    }
}
