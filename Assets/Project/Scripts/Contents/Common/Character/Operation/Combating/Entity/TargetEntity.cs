using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class TargetEntity : BaseEntity
    {
        [SerializeField] private string targetLayerType;
        [SerializeField] private int targetCount;
        [SerializeField] private float range;
        [ShowInInspector]
        private List<GameObject> searchedList;
        private ICombatTaker combatTaker;
        private List<ICombatTaker> combatTakerList;
        private readonly List<ICombatTaker> combatTakerFilterCache = new();

        public override bool IsReady => CombatTaker != null;
        public float Range { get => range; set => range = value; }

        public List<ICombatTaker> CombatTakerList
        {
            get
            {
                SetEntity();
                UpdateTargetList();
                
                return combatTakerList;
            }
            private set => combatTakerList = value;
        }

        public ICombatTaker CombatTaker
        {
            get
            {
                SetEntity();
                UpdateMainTarget();
                
                return combatTaker;
            }
        }

        public void UpdateTargetList()
        {
            if (searchedList.IsNullOrEmpty()) return;
            
            combatTakerFilterCache.Clear();

            searchedList.ForEach(x =>
            {
                if (x.TryGetComponent(out ICombatTaker taker))
                    combatTakerFilterCache.Add(taker);
            });
            
            var inRangedTargetList =
                combatTakerFilterCache.Where(x => Vector3.Distance(x.Taker.transform.position, transform.position) <= Range)
                               .ToList();

            CombatTakerList = inRangedTargetList.Count >= targetCount
                ? inRangedTargetList.Take(targetCount).ToList()
                : inRangedTargetList;
        }

        public void UpdateMainTarget()
        {
            combatTaker = CombatTakerList.IsNullOrEmpty() 
                ? searchedList.Select(x => x.GetComponent<ICombatTaker>()).FirstOrDefault()
                : combatTakerList.First();
        }

        public override void SetEntity()
        {
            targetCount = SkillData.TargetCount;
            range = SkillData.Range;
            targetLayerType = SkillData.TargetLayer;
        }

        protected override void Awake()
        {
            base.Awake();

            SetEntity();
            
            searchedList = targetLayerType is "ally"
                ? Cb.CharacterSearchedList // ally
                : Cb.MonsterSearchedList;  // enemy
        }

        private void Reset()
        {
            flag = EntityType.Target;
            SetEntity();
        }

    }
}
