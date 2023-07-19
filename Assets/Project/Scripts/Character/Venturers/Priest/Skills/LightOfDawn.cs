using Common.Characters;
using Common.Execution.Hits;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class LightOfDawn : SkillComponent
    {
        [SerializeField] private HealHit healExecution;

        public HealHit HealExecution => healExecution;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .Add(Section.Execute, "ExecuteLightOfDawn", ExecuteLightOfDawn)
                .Add(Section.Execute, "TryConsumeLightWeaver", TryConsumeLightWeaver);
        }


        private void ExecuteLightOfDawn()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
        
        private void TryConsumeLightWeaver()
        {
            Cb.DispelStatusEffect(DataIndex.LightWeaverStatusEffect);
        }
    }
}
