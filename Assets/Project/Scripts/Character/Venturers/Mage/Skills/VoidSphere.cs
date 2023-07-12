using Common.Skills;

namespace Character.Venturers.Mage.Skills
{
    public class VoidSphere : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(Section.Execute, "Fire", Fire);
        }
        
        
        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            Invoker.Fire(forwardPosition);
        }
    }
}
