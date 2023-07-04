using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class Renew : SkillComponent
    {
        private Vector3 predicatePosition = Vector3.zero;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddActiveParam("SetTargetPosition", SetTargetPosition)
                .Add(SectionType.Execute, "ExecuteRenew", ExecuteRenew)
                .Add(SectionType.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void SetTargetPosition(Vector3 targetPosition)
        {
            predicatePosition = TargetUtility.GetValidPosition(Cb.Position, Range, targetPosition);
        }
        
        private void ExecuteRenew()
        {
            var nearestTarget = detector.GetNearestTarget(predicatePosition, 6f);

            if (nearestTarget is null) return;

            executor.ToTaker(nearestTarget);
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }
    }
}
