using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Mage.Skills
{
    public class VileScrapper : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder.Add(Section.Execute, "Fire", Fire);
        }


        private void Fire()
        {
            var forwardPosition = Provider.Position + Provider.Forward;

            executor.ToPosition(forwardPosition);
        }
    }
}
