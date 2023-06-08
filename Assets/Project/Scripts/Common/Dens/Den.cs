using Serialization;
using Singleton;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Common
{
    using Characters;

    public class Den : MonoSingleton<Den>, ISavable, IEditable
    {
        [SerializeField] private Table<DataIndex, VillainData> villainTable = new();

        private static Table<DataIndex, VillainData> VillainTable => Instance.villainTable;

        public static VillainData GetVillainData(DataIndex dataIndex) => VillainTable[dataIndex];

        public void Save() 
        {
            villainTable.Iterate(data => data.Save());
        }

        public void Load()
        {
            villainTable.Iterate(data => data.Load());
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            Finder.TryGetObjectList<VillainData>(out var villainDataList);
            
            villainTable.CreateTable(villainDataList, data => data.DataIndex);
        }
#endif
        
    }
}
