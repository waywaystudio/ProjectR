using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class TargetEntity : EntityAttribution
    {
        private string targetLayerType;
        private int targetCount;
        private float range;
        private List<GameObject> searchedList;
        private ICombatTaker mainTarget;
        private List<ICombatTaker> targetList;
        private readonly List<ICombatTaker> combatTakerList = new();
        
        public List<ICombatTaker> TargetList
        {
            get
            {
                SetEntity();
                UpdateTargetList();
                
                return targetList;
            }
            private set => targetList = value;
        }

        public ICombatTaker MainTarget
        {
            get
            {
                SetEntity();
                UpdateMainTarget();
                
                return mainTarget;
            }
        }
        

        public void UpdateTargetList()
        {
            combatTakerList.Clear();

            searchedList.ForEach(x =>
            {
                var combatTaker = x.GetComponent<ICombatTaker>();
                if (combatTaker != null) combatTakerList.Add(combatTaker);
            });
            
            var inRangedTargetList =
                combatTakerList.Where(x => Vector3.Distance(x.Taker.transform.position, transform.position) <= range)
                               .ToList();

            TargetList = inRangedTargetList.Count >= targetCount
                ? inRangedTargetList.Take(targetCount).ToList()
                : inRangedTargetList;
        }

        public void UpdateMainTarget()
        {
            mainTarget = TargetList.IsNullOrEmpty() 
                ? Cb.CharacterSearchedList.Select(x => x.GetComponent<ICombatTaker>()).FirstOrDefault()
                : targetList.First();
        }

        public void SetEntity()
        {
            targetCount = SkillData.TargetCount;
            range = SkillData.Range;
        }

        private void Awake()
        {
            searchedList = targetLayerType is "ally"
                ? Cb.CharacterSearchedList // ally
                : Cb.MonsterSearchedList;  // enemy
        }


#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            Flag = EntityType.Target;

            SetEntity();
            targetLayerType = SkillData.TargetLayer; // ally or enemy
        }
#endif
    }
}
