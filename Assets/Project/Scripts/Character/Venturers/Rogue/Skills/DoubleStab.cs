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

            Builder
                .AddApplying("PhantomsDoubleStab",PhantomsDoubleStab)
                .Add(Section.Execute, "DoubleStabAttack", DoubleStabAttack);
        }


        private void DoubleStabAttack()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            SkillCost.PayCost(Cb.Resource);
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
        
        private void PhantomsDoubleStab(Vector3 targetPosition)
        {
            master.DoubleStab(targetPosition, Haste.Invoke());
        }
    }
}
