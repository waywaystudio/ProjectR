using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using MainGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.Data
{
    public class DropTable : MonoBehaviour, IDataSetUp
    {
        [Serializable]
        public struct ItemDrop
        {
            public ItemDrop(DataIndex itemCode, int probability)
            {
                ItemCode = itemCode;
                Probability = probability;
            }
            
            public DataIndex ItemCode;
            public int Probability;
        }
        
        [SerializeField] private List<ItemDrop> dropTable = new();

        private readonly List<int> probabilityList = new();
        private DataIndex DropCodeIndex
        {
            get
            {
                var totalValue = dropTable.Sum(x => x.Probability);
                var randomValue = Random.Range(1, totalValue + 1);
                var targetIndex = 0;

                for (var i = 0; i < probabilityList.Count; ++i)
                {
                    if (randomValue >= probabilityList[i])
                    {
                        randomValue -= probabilityList[i];
                        continue;
                    }
                
                    targetIndex = i;
                    break;
                }

                return dropTable[targetIndex].ItemCode;
            }
        }


        public List<DataIndex> GetRewards(int count)
        {
            if (!DropAssertion(count)) return null;
            
            dropTable.ForEach(x => probabilityList.Add(x.Probability));
            var result = new List<DataIndex>(count);

            for (var i = 0; i < count; ++i)
            {
                result.Add(DropCodeIndex);
            }

            probabilityList.Clear();
            
            //
            result.ForEach(x => Debug.Log(x));
            //
            
            return result;
        }


        private static bool DropAssertion(int count)
        {
            if (count <= 0)
            {
                Debug.LogError($"Drop Count must be Larger then 0. Current Count:{count}");
                return false;
            }

            return true;
        }
        
#if UNITY_EDITOR
        public void SetUp()
        {
            // var mb = GetComponentInParent<OldMonsterBehaviour>();
            // var monsterID = mb.DataIndex;
            // var monsterData = MainData.GetBoss(monsterID);
            //
            // var itemCodeList = monsterData.DropItemIdList.ConvertAll(itemID => (DataIndex)itemID);
            // var itemProbabilityList = monsterData.DropItemProbabilities;
            //
            // if (itemCodeList.Count != itemProbabilityList.Count)
            // {
            //     Debug.LogError($"Miss Matching Item Code & Probability Count. CodeCount:{itemCodeList.Count}, ProbabilityCount:{itemProbabilityList.Count}");
            //     return;
            // }
            //
            // dropTable.Clear();
            // itemCodeList.ForEach((code, index) =>
            // {
            //     var itemDrop = new ItemDrop(code, itemProbabilityList[index]);
            //     dropTable.Add(itemDrop);
            // });
        }
#endif
    }
}
