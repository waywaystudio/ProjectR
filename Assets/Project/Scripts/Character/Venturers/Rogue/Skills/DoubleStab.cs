using Common.Characters;
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
            
            cost.PayCondition.Add("HasTarget", detector.HasTarget);

            Builder
                .AddApplying("PhantomsDoubleStab",PhantomsDoubleStab)
                .Add(Section.Execute, "DoubleStabAttack", DoubleStabAttack);
        }


        private void DoubleStabAttack()
        {
            detector.GetTakers()?.ForEach(Invoker.Hit);
        }
        
        private void PhantomsDoubleStab(Vector3 targetPosition)
        {
            master.DoubleStab(targetPosition, 1f + Haste);
        }
    }
}
