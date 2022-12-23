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

        private ISearchable searchEngine;
        private List<ICombatTaker> combatTakerList;
        private readonly List<ICombatTaker> combatTakerFilterCache = new();

        public override bool IsReady => CombatTaker != null;
        public string TargetLayerType { get => targetLayerType; set => targetLayerType = value; }
        public int TargetCount { get => targetCount; set => targetCount = value; }
        public float Range { get => range; set => range = value; }

        public List<ICombatTaker> CombatTakerList
        {
            get
            {
                UpdateTargetList();
                
                return combatTakerList;
            }
            
            private set => combatTakerList = value;
        }

        public ICombatTaker CombatTaker
        {
            get
            {
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

            CombatTakerList = inRangedTargetList.Count >= TargetCount
                ? inRangedTargetList.Take(TargetCount).ToList()
                : inRangedTargetList;
        }

        public void UpdateMainTarget()
        {
            if (targetLayerType is "self")
            {
                combatTaker = GetComponentInParent<ICombatTaker>();
            }
            else
            {
                combatTaker = CombatTakerList.IsNullOrEmpty() 
                    ? searchedList.Select(x => x.GetComponent<ICombatTaker>()).FirstOrDefault()
                    : combatTakerList.First();
            }

            searchEngine.MainTarget = combatTaker;
        }


        protected override void Awake()
        {
            searchEngine = GetComponentInParent<ISearchable>();
            
            base.Awake();
            
            searchedList = TargetLayerType is "ally" or "self"
                         ? searchEngine.AdventureList // ally or self
                         : searchEngine.MonsterList;  // enemy
        }
    }
}
