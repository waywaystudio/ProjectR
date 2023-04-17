using System.Collections.Generic;
using System.Text;
using Adventurers;
using Character.Adventurers;
using Character.Bosses;
using Common;
using Common.PlayerCamps;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform adventurerHierarchy;
        [SerializeField] private Transform monsterHierarchy;
        [SerializeField] private Boss boss;
        [SerializeField] private int dropItemCount;

        [ShowInInspector] public List<Adventurer> AdventurerList { get; } = new();
        [ShowInInspector] public Boss Boss => boss;


        public void Initialize(List<DataIndex> adventurerEntry)
        {
            if (adventurerEntry.IsNullOrEmpty())
            {
                Debug.Log($"Adventurer Not Existed. HasEntry ? : {!adventurerEntry.IsNullOrEmpty()}");
                return;
            }
            
            if (adventurerEntry.Count > 8)
            {
                Debug.Log($"Adventurer Count Error. EntryCount : {adventurerEntry.Count}");
                return;
            }

            adventurerEntry.ForEach((adventurerIndex, index) =>
            {
                if (!Database.CombatClassPrefabData.Get<Adventurer>(adventurerIndex, out var adventurerPrefab)) return;
                
                var profitPosition   = RaidDirector.StageDirector.GetAdventurerPosition(index).position;
                var adventurer = Instantiate(adventurerPrefab, profitPosition, Quaternion.identity,
                    adventurerHierarchy);

                adventurer.gameObject.SetActive(true);
                AdventurerList.Add(adventurer);
            });
            
            Boss.ForceInitialize();
            AdventurerList.ForEach(adventurer => adventurer.ForceInitialize());

            Rewards();
        }

        private void Rewards()
        {
            Boss.DeadBehaviour.OnCompleted.Register("DropItems", AddRewardItem);
        }


        private void AddRewardItem()
        {
            /*
             * 보스 공략에 성공하면 인벤토리에 즉시 n개의 아이템이 들어오고,
             * 드랍 아이템 정보창을 띄운다.
             * 드랍 아이템 정보창은 정보로서의 역할만 수행할 뿐이다.
             * 인벤토리에 제한이 있거나 즉시 분해 혹은 판매 기능을 추가하고 싶다면 시퀀스가 바뀌어야 한다.
             */
            
            // Boss.DeadBehaviour.OnCompleted.Register("Show Reward", );
            var sb = new StringBuilder();
            
            for (var i = 0; i < dropItemCount; i++)
            {
                var dropItemPrefab = Boss.DropItemTable.Random();
                var dropItem       = Instantiate(dropItemPrefab);
                
                PlayerCamp.Inventories.Add(dropItem);

                sb.Append($"{dropItem.name} Get!\n");
            }
            
            Debug.Log(sb.ToString());
        }
    }
}