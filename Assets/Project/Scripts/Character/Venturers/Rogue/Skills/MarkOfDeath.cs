using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Rogue.Skills
{
    public class MarkOfDeath : SkillComponent
    {
        [SerializeField] private PhantomMaster master;
        
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.AddActiveParam("PhantomAction", PhantomAction)
                           .Add(SectionType.Execute, "Throw", Throw);
        }


        private void PhantomAction(Vector3 targetPosition)
        {
            master.MarkOfDeath(targetPosition, 1 + Haste);
        }

        private void Throw()
        {
            executor.Execute(null);
        }
    }
}
