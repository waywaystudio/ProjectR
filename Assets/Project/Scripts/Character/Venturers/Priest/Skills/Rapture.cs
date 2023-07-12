using Common.Skills;

namespace Character.Venturers.Priest.Skills
{
    public class Rapture : SkillComponent
    {
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddCondition("HasRaptureAlready", HasRaptureAlready)
                .Add(Section.Execute, "ExecuteHealingTouch",ExecuteRapture);
        }


        private bool HasRaptureAlready()
        {
            return !Provider.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect);
        }
        
        private void ExecuteRapture()
        {
            detector.GetTakers()?.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}
