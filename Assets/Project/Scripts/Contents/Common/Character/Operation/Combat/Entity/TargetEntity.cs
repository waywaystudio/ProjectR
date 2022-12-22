using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class TargetEntity : BaseEntity
    {
        [SerializeField] private string targetLayerType;
        [SerializeField] private int targetCount;
        [SerializeField] private float range;
        [ShowInInspector] private List<GameObject> searchedList;
        [ShowInInspector] private ICombatTaker combatTaker;
        private List<ICombatTaker> combatTakerList;
        private readonly List<ICombatTaker> combatTakerFilterCache = new();

        public override bool IsReady => CombatTaker != null;
        public float Range => range;

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
                combatTakerFilterCache.Where(x => Vector3.Distance(x.Object.transform.position, transform.position) <= Range)
                               .ToList();

            CombatTakerList = inRangedTargetList.Count >= targetCount
                ? inRangedTargetList.Take(targetCount).ToList()
                : inRangedTargetList;
        }

        public void UpdateMainTarget()
        {
            if (targetLayerType is "self")
            {
                combatTaker = Cb;
            }
            else
            {
                combatTaker = CombatTakerList.IsNullOrEmpty() 
                    ? searchedList.Select(x => x.GetComponent<ICombatTaker>()).FirstOrDefault()
                    : combatTakerList.First();
            }

            Cb.MainTarget = combatTaker;
        }

        public override void SetEntity()
        {
            
        }

        protected override void Awake()
        {
            base.Awake();

            SetEntity();

            searchedList = targetLayerType is "ally" or "self"
                ? Cb.CharacterSearchedList // ally
                : Cb.MonsterSearchedList;  // enemy
        }

        private void Reset()
        {
            flag = EntityType.Target;
            
            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName);

            targetCount = skillData.TargetCount;
            range = skillData.Range;
            targetLayerType = skillData.TargetLayer;
        }

    }
}
