using Common.Execution.Variants;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class LightOfDawn : SkillComponent
    {
        [SerializeField] private HealExecution healExecution;

        public HealExecution HealExecution => healExecution;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(SectionType.Execute, "ExecuteLightOfDawn", ExecuteLightOfDawn)
                .Add(SectionType.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void ExecuteLightOfDawn()
        {
            detector.GetTakers()?.ForEach(taker => executor.ToTaker(taker));
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }
    }
}
