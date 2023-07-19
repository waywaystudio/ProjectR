using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class Taunt : SkillComponent
    {
        //Minions개념 필요
        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(Section.Execute, "TauntAction", TauntAction);
        }


        private void TauntAction()
        {
            if (!detector.TryGetTakers(out var takers)) return;
            
            takers.ForEach(taker =>
            {
                Taker = taker;
                Invoker.Hit(taker);
            });
        }
    }
}
