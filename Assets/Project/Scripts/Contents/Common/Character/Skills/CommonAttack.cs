namespace Common.Character.Skills
{
    using Core;
    using Entity;
    
    public class CommonAttack : SkillAttribution
    {
        public override void InvokeEvent()
        {
            var hasProvider = TryGetComponent(out DamageEntity damageEntity);
            var hasTargetList = TryGetComponent(out TargetEntity targetEntity);
            
            if (hasProvider && hasTargetList)
                targetEntity.CombatTakerList.ForEach(target => target.TakeDamage(damageEntity));
        }
    }
}

// 1. Casting(or Channeling), Instant 에 따라서 OnStart 와 OnEnd의 구조가 달라진다.
// 2. Animation Event를 활용여부에 따라서 Invoke의 할당 위치가 달리진다.
// 3. 대부분의 공격은 Prefab Effect를 가진다. Trajectory 형태에 따라서 Instant와 Projectile로 나뉜다.
// CB.SKill에서 실행시키는 것은 결국 OnStart 뿐이며, OnEnd는 1번에 의해서 결정된다.     