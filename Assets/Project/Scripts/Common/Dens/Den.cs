using System.Collections.Generic;
using Common.Characters;
using Singleton;
using UnityEngine;

namespace Common.Dens
{
    public class Den : MonoSingleton<Den>, IEditable
    {
        [SerializeField] private List<VillainData> villainDataList;

        private Dictionary<DataIndex, VillainData> villainTable;
        private static Dictionary<DataIndex, VillainData> VillainTable
        {
            get
            {
                if (Instance.villainTable.IsNullOrEmpty())
                {
                    Instance.villainTable = new Dictionary<DataIndex, VillainData>();
                    Instance.villainDataList.ForEach(boss => Instance.villainTable.Add(boss.DataIndex, boss));
                }

                return Instance.villainTable;
            }
        }

        public static VillainData GetVillainData(DataIndex dataIndex) => VillainTable[dataIndex];

        public static void SetVillainDifficulty(DataIndex dataIndex, int difficulty)
        {
            var villainData = GetVillainData(dataIndex);
            
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            Finder.TryGetObjectList(out villainDataList);
        }
#endif
    }
}
