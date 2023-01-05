using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    /* Annotation :: 타겟팅 추후에 Sorting이 들어갈 필요가 있다.
     Sorting을 Searching 이후에 별도의 클래스를 통해 할 수도 있고, 현 클래스에서 할 수도 있다.
     현재 클래스에서 한다면, 스킬별로 우선순위가 다를 수 있기 때문일 것이고, 
     별도의 클래스에서 한다면, 레이드 내에서 전략적인 이유에 따라 판단할 수 있다. 
     시스템이 아닌, 플레이어의 강제적 인터렉션에 의해서 타겟이 잡힐 수도 있다. */
    public class TargetEntity : BaseEntity
    {
        [SerializeField] private string targetLayerType;
        [SerializeField] private int targetCount;
        [SerializeField] private float range;

        private ICombatTaker combatTaker;
        private ISearchedListTaker searchedListTaker;
        private readonly List<ICombatTaker> combatTakerList = new(16);
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
                    return combatTakerList;

                var selfPosition = transform.position;

                combatTakerList.Clear();
                
                searchedList.ForEach(x =>
                {
                    if (Vector3.Distance(x.Object.transform.position, selfPosition) <= Range)
                        combatTakerList.Add(x);
                });

                if (combatTakerList.Count > TargetCount)
                {
                    // TODO. 많은 GC 발생 중.
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
