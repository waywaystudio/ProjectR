using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class TargetModule : Module, ITargetModule, IReady
    {
        [SerializeField] private SortingType sortingType;
        [SerializeField] private string targetLayerType;
        [SerializeField] private int targetCount;
        [SerializeField] private float range;
        [SerializeField] private bool isSelf;

        private ISearching searchingEngine;
        private ITargeting targetingEngine;
        private List<ICombatTaker> targetList;
        private LayerMask targetLayer;

        public bool IsReady => Target != null;
        public float Range => range;

        public ICombatTaker Target => isSelf
            ? targetingEngine.GetSelf() 
            : targetingEngine.GetTaker(targetList, Provider, Range, sortingType) ?? searchingEngine.LookTarget;

        public List<ICombatTaker> TargetList 
            => targetingEngine.GetTakerList(targetList, Provider, Range, sortingType, targetCount);

        public void TakeDamage(ICombatTable damage)
        {
            if (targetCount == 1) Target.TakeDamage(damage);
            else 
                TargetList.ForEach(target => target.TakeDamage(damage));
        }
        public void TakeHeal(ICombatTable damage)
        {
            if (targetCount == 1) Target.TakeHeal(damage);
            else 
                TargetList.ForEach(target => target.TakeHeal(damage));
        }
        public void TakeSpell(ICombatTable damage)
        {
            if (targetCount == 1) Target.TakeSpell(damage);
            else 
                TargetList.ForEach(target => target.TakeSpell(damage));
        }
        public void TakeProjectile(IProjectileModule projectile)
        {
            if (targetCount == 1) projectile.Fire(Target);
            else TargetList.ForEach(projectile.Fire);
        }
        public void TakeStatusEffect(IStatusEffectModule statusEffect)
        {
            if (targetCount == 1) statusEffect.Effectuate(Target);
            else TargetList.ForEach(statusEffect.Effectuate);
        }

        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);

            var cb = GetComponentInParent<CharacterBehaviour>();
            
            searchingEngine = cb.SearchingEngine;
            targetingEngine = cb.TargetingEngine;
            targetLayer     = CharacterUtility.SetLayer(Provider, targetLayerType);
            targetList = targetLayer == LayerMask.NameToLayer("Adventurer")
                    ? searchingEngine.AdventurerList
                    : searchingEngine.MonsterList;
        }

#if UNITY_EDITOR
        public void SetUpValue(string targetLayerType, int targetCount, float range, SortingType sortingType, bool isSelf)
        {
            Flag                 = ModuleType.Target;
            this.targetLayerType = targetLayerType;
            this.targetCount     = targetCount;
            this.range           = range;
            this.sortingType     = sortingType;
            this.isSelf          = isSelf;
        }
#endif
    }
}
