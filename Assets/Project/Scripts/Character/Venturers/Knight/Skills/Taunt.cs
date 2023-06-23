using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class Taunt : SkillComponent
    {
        // 스킬구조는 간단하지만, Minions개념이 있어야 하고
        public override void Initialize()
        {
            base.Initialize();
            
            SequenceBuilder.Add(SectionType.Execute, "CommonExecution",
                                () => detector.GetTakers()?.ForEach(executor.Execute));
        }
    }
}