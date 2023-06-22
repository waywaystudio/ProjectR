using Common;
using Common.StatusEffects;

namespace Character.Venturers.Knight.StatusEffects
{
    public class WillOfKnight : StatusEffect
    {
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active, "AddEffect", AddEffect)
                           .Add(SectionType.End, "RemoveEffect", RemoveEffect);
        }


        private void AddEffect()
        {
            // 방어력++
            Taker.StatTable.Add(new StatEntity(StatType.Armor, "WillOfKnight", 100));
            
            // 기절, 넉백 면역
            Taker.KnockBackBehaviour.Builder.AddCondition("ImmuneByWillOfKnight", () => false);
            Taker.StunBehaviour.Builder.AddCondition("ImmuneByWillOfKnight", () => false);
        }

        private void RemoveEffect()
        {
            // 방어력--
            Taker.StatTable.Remove(StatType.Armor, "WillOfKnight");
            
            // 기절, 넉백 면역 해제
            Taker.KnockBackBehaviour.Builder.RemoveCondition("ImmuneByWillOfKnight");
            Taker.StunBehaviour.Builder.RemoveCondition("ImmuneByWillOfKnight");
        }
    }
}
