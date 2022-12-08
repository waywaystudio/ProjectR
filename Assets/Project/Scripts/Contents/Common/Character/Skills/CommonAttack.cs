namespace Common.Character.Skills
{
    using Core;
    using Entity;
    
    public class CommonAttack : BaseSkill
    {
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target => target.TakeDamage(damageEntity));
        }

        public override void StartSkill()
        {
            base.StartSkill();

            CompleteSkill();
        }
    }
}