using Common;
using Common.Skills;

namespace Character.Venturers.Ranger.Skills
{
    public class InstantTrap : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();

        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Active, "Jump", Jump)
                           .Add(SectionType.Active, "Execution", SkillInvoker.Execute)
                           .Add(SectionType.Execute, "DropInstantTrap", () => executor.Execute(MainTarget));
        }

        private void Jump()
        {
            var direction = Cb.transform.forward * -1f;
            
            Cb.Pathfinding.Jump(direction, 10f, 2, 0.5f);
        }
    }
}