using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Rogue.Skills
{
    public class MarkOfDeath : SkillComponent
    {
        [SerializeField] private PhantomMaster master;

        private Vector3 positionBuffer;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddApplying("PhantomsMarkOfDeath", PhantomsMarkOfDeath)
                .Add(Section.Execute,"MarkOfDeathFire", MarkOfDeathFire);
        }


        private void PhantomsMarkOfDeath(Vector3 targetPosition)
        {
            positionBuffer = targetPosition;
            
            master.MarkOfDeath(targetPosition, 1 + Haste);
        }

        private void MarkOfDeathFire()
        {
            Invoker.Fire(positionBuffer);
        }
    }
}
