using Common.Skills;

namespace Character.Venturers.Knight.Skills
{
    public class Taunt : SkillComponent
    {
        //Minions개념 필요
        public override void Initialize()
        {
            base.Initialize();
            
            Builder.Add(SectionType.Execute, "TauntAction", TauntAction);
        }


        private void TauntAction()
        {
            detector.GetTakers()?.ForEach(executor.ToTaker);
        }
    }
}
