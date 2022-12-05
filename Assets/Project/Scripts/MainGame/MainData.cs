using System;
using System.Collections.Generic;
using Core;
using MainGame.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainGame
{
    // Protocol이 Data로 있는게 좋겠다. 해당 Protocol에서 카테고리 분류를 번호로 받아오자.
    public class MainData : Core.Singleton.MonoSingleton<MainData>
    {
        [SerializeField] private List<DataObject> dataList;
        private readonly Dictionary<int, DataObject> dataTable = new();

        public List<DataObject> DataList => dataList;
        public static Dictionary<int, DataObject> DataTable => Instance.dataTable;

        [Button]
        public bool TryGetData<T>(string category, int id, out T value) where T : Row
        {
            // var data = dataList.Find(x => x.Category == category);

            value = null;
            return false;
        }

        // [Button]
        // private void GetRowData()
        // {
        //     Finder.TryGetObjectList(out List<SpreadSheetObject> rowDataList);
        //
        //     dataList = rowDataList;
        // }
        //
        // [Button]
        // private void DownCastingTest()
        // {
        //     var singleRow = dataList.Find(x => x.GetType() == typeof(RaidData));
        //     var downCast = singleRow as RaidData;
        //
        //     if (downCast != null) Debug.Log(downCast.List[0].RaidName);
        // }

        protected override void Awake()
        {
            base.Awake();

            dataList.ForEach((x, index) => dataTable.Add(index, x));
        }

#if UNITY_EDITOR
        [OnInspectorInit]
        public void EditorInitialize()
        {
            Finder.TryGetObjectList(out dataList);
        }
#endif
    }
}
