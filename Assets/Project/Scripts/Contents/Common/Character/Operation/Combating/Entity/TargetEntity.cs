using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class TargetEntity : BaseEntity
    {
        private string targetLayerType;
        private int targetCount;
        private List<GameObject> searchedList;
        private ICombatTaker combatTaker;
        private List<ICombatTaker> combatTakerList;
        private readonly List<ICombatTaker> combatTakerFilterCache = new();

        public override bool IsReady => CombatTaker != null;
        public float Range { get; private set; }

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
                ? Cb.CharacterSearchedList.Select(x => x.GetComponent<ICombatTaker>()).FirstOrDefault()
                : combatTakerList.First();
        }

        protected void SetEntity()
        {
            targetCount = SkillData.TargetCount;
            Range = SkillData.Range;
        }

        protected override void Awake()
        {
            base.Awake();
            
            searchedList = targetLayerType is "ally"
                ? Cb.CharacterSearchedList // ally
                : Cb.MonsterSearchedList;  // enemy
        }


#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            flag = EntityType.Target;

            SetEntity();
            targetLayerType = SkillData.TargetLayer; // ally or enemy
        }
#endif
    }
}
