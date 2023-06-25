using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Rogue.Skills
{
    public class DoubleStab : SkillComponent
    {
        [SerializeField] private PhantomMaster master;
        
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", HasTarget);

            SequenceBuilder.AddActiveParam("MasterDoubleStab",targetPosition => master.DoubleStab(targetPosition, 1f + Haste))
                           .Add(SectionType.Execute, "CommonExecution", () => detector.GetTakers()?.ForEach(executor.Execute));
        }


        private bool HasTarget()
        {
            var takers = detector.GetTakers();

            return !takers.IsNullOrEmpty() 
                   && takers[0].DynamicStatEntry.Alive.Value;
        }
    }
}
