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


        // private bool HasTarget()
        // {
        //     var takers = detector.GetTakers();
        //
        //     return !takers.IsNullOrEmpty() 
        //            && takers[0].DynamicStatEntry.Alive.Value;
        // }
    }
}
