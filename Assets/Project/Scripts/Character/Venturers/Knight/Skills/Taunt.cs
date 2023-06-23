using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class Taunt : SkillComponent
    {
        //Minions개념 필요
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution",
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}
