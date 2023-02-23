namespace Character.Skill.Moragg
{
    public class MoraggCommonAttack : GeneralAttack
    {
        protected override void OnAttack()
        {
            var combatEntity = MainTarget?.TakeDamage(this);

            if (combatEntity == null || MainTarget == null) return;
        }
    }
}
