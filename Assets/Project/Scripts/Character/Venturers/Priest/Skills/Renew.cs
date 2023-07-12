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
                .AddApplying("SetTargetPosition", SetTargetPosition)
                .Add(Section.Execute, "ExecuteRenew", ExecuteRenew)
                .Add(Section.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void SetTargetPosition(Vector3 targetPosition)
        {
            predicatePosition = TargetUtility.GetValidPosition(Cb.Position, Distance, targetPosition);
        }
        
        private void ExecuteRenew()
        {
            Taker = detector.GetNearestTarget(predicatePosition, Range);

            if (Taker is null) return;
            
            Invoker.Hit(Taker);
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }
    }
}
