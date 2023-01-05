using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class TargetEntity : BaseEntity
    {
        [SerializeField] private string targetLayerType;
        [SerializeField] private int targetCount;
        [SerializeField] private float range;

        private ICombatTaker combatTaker;
        private ISearchedListTaker searchedListTaker;
        private readonly List<ICombatTaker> combatTakerList = new();
        private List<ICombatTaker> searchedList;

        public override bool IsReady => CombatTaker != null;
        public string TargetLayerType { get => targetLayerType; set => targetLayerType = value; }
        public int TargetCount { get => targetCount; set => targetCount = value; }
        public float Range { get => range; set => range = value; }

        public List<ICombatTaker> CombatTakerList
        {
            get
            {
                if (searchedList.IsNullOrEmpty())
                {
                    return combatTakerList;
                }

                var selfPosition = transform.position;

                combatTakerList.Clear();
                combatTakerList.AddRange(searchedList.FindAll(x =>
                    Vector3.Distance(x.Object.transform.position, selfPosition) <= Range));

                if (combatTakerList.Count > TargetCount)
                {
                    combatTakerList.RemoveRange(TargetCount, combatTakerList.Count - TargetCount);
                }

                return combatTakerList;
            }
        }

        public ICombatTaker CombatTaker
        {
            get
            {
                if (targetLayerType is "self")
                {
                    combatTaker = searchedListTaker.Self;
                }
                else
                {
                    combatTaker = CombatTakerList.IsNullOrEmpty() 
                        ? searchedList.FirstOrDefault()
                        : combatTakerList.First();
                }

                searchedListTaker.MainTarget = combatTaker;

                return combatTaker;
            }
        }


        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);
            
            searchedListTaker = GetComponentInParent<ISearchedListTaker>();
            
            var selfLayer = Provider.Object.layer;

            if (selfLayer == LayerMask.NameToLayer("Adventurer"))
            {
                searchedList = TargetLayerType is "ally" or "self" 
                    ? searchedListTaker.AdventurerList
                    : searchedListTaker.MonsterList;
            }
            
            else if (selfLayer == LayerMask.NameToLayer("Monster"))
            {
                searchedList = TargetLayerType is "ally" or "self" 
                    ? searchedListTaker.MonsterList
                    : searchedListTaker.AdventurerList;
            }
        }
    }
}
