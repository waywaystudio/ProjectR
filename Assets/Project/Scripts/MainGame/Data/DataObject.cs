using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MainGame.Data
{
    public abstract class DataObject : ScriptableObject
    {
        [SerializeField] protected DataCategory category;

        public DataCategory Category => category;
        public abstract void RegisterNameTable(Dictionary<string, int> nameTable);
        public abstract bool TryGetData<T>(int id, out T result) where T : class;
    }

    public abstract class DataObject<T> : DataObject where T : IIdentifier
    {
        [SerializeField] protected List<T> list;
        private Dictionary<int, T> dataTable;

        protected List<T> List { get => list; set => list = value; }
        public Dictionary<int, T> Table
        {
            get
            {
                if (dataTable != null) return dataTable;

                dataTable = list.ToDictionary(x => x.ID);
                return dataTable;
            }
            set => dataTable = value;
        }
        

        public override void RegisterNameTable(Dictionary<string, int> nameTable)
        {
            list.ForEach(x =>
            {
                if (x.Name != "" && !nameTable.ContainsKey(x.Name))
                {
                    nameTable.Add(x.Name, x.ID);
                }
                else
                {
                    // TODO. 서로다른 카테고리에도 중복된 키 값이 사용될 수도 있을 것 같다.
                    // TODO. 키 값을 만드는 아이디어를 바꾸거나, 키 값을 바꾸자.
                    Debug.LogWarning($"{x.Name} is Duplicated.");
                }
            });
        }

        public override bool TryGetData<T1>(int id, out T1 result)
        {
            Table.TryGetValue(id, out var value);
        
            result = value as T1;
            return true;
        }


        protected void Awake()
        {
            if (category == DataCategory.None) SetCategory();
        }

        private void SetCategory()
        {
            var index = List.IsNullOrEmpty()
                ? 0
                : int.Parse(List[0].ID.ToString()[..2]);

            category = (DataCategory)index;
        }

#if UNITY_EDITOR

        [OnInspectorInit]
        public void EditorInitialize()
        {
            SetCategory();
        }
    }
#endif
}
