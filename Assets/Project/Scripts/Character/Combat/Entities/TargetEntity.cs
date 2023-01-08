using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class TargetEntity : BaseEntity
    {
        [SerializeField] private SortingType sortingType;
        [SerializeField] private string targetLayerType;
        [SerializeField] private float range;
        [SerializeField] private bool isSelf;

        private ISearching searchingEngine;
        private ITargeting targetingEngine;
        private List<ICombatTaker> targetList;
        private LayerMask targetLayer;

        public override bool IsReady => Target != null;
        public float Range => range;

        public ICombatTaker Target
        {
            get
            {
                if (isSelf) return targetingEngine.GetSelf();

                var target
                        = targetingEngine.GetTaker(targetList, Provider, Range, sortingType)
                       ?? searchingEngine.LookTarget;

                return target;
            }
        }

        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);

            var selfLayer = (LayerMask) (1 << Provider.Object.layer);
            var cb = GetComponentInParent<CharacterBehaviour>();

            searchingEngine = cb.SearchingEngine;
            targetingEngine = cb.TargetingEngine;

            targetLayer = targetLayerType is "ally"
                    ? selfLayer
                    : selfLayer.GetEnemyLayerMask();

            targetList = targetLayer == LayerMask.NameToLayer("Adventurer")
                    ? searchingEngine.AdventurerList
                    : searchingEngine.MonsterList;
        }

        public void SetUpValue(string targetLayerType, float range, SortingType sortingType, bool isSelf)
        {
            this.targetLayerType = targetLayerType;
            this.range           = range;
            this.sortingType     = sortingType;
            this.isSelf          = isSelf;
        }
    }
}
