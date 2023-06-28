using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Mage.Skills
{
    public class VileScrapper : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();
            
            // cost.PayCondition.Add("HasTarget", HasTarget);

            SequenceBuilder.Add(SectionType.Execute, "CommonExecution", 
                                () => executor.Execute(null));
        }
    }
}
