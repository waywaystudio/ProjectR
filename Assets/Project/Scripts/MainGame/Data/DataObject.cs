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
        
        public abstract DataCategory Category { get; }
        public abstract void RegisterNameTable(Dictionary<string, int> nameTable);
        public abstract bool TryGetData<T>(int id, out T result) where T : class;

#if UNITY_EDITOR
        public abstract string SpreadSheetID { get; }
        public abstract string SpreadSheetName { get; }
        public abstract string WorkSheetName { get; }
#endif
    }

    public abstract class DataObject<T> : DataObject where T : IIdentifier
    {
        [SerializeField] protected List<T> list;
        private Dictionary<int, T> dataTable;

        public List<T> List { get => list; set => list = value; }
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
        
        public override DataCategory Category => category;
        
        public override void RegisterNameTable(Dictionary<string, int> nameTable)
        {
            list.ForEach(x =>
            {
                if (!nameTable.ContainsKey(x.TextKey))
                    nameTable.Add(x.TextKey, x.ID);
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
#endif
    }
    
    #region Attribute Setting        
#if UNITY_EDITOR
    public class DataObjectDrawer<T0, T1> : Sirenix.OdinInspector.Editor.OdinAttributeProcessor<T0> where T0 : DataObject<T1> where T1 : IIdentifier
    {
        public override void ProcessChildMemberAttributes(Sirenix.OdinInspector.Editor.InspectorProperty parentProperty, System.Reflection.MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "list":
                    attributes.Add(new TableListAttribute
                    {
                        AlwaysExpanded = true,
                        HideToolbar = true,
                        DrawScrollView = true,
                        IsReadOnly = true
                    });
                    break;
                case "LoadFromJson":
                    attributes.Add(new PropertySpaceAttribute(5f, 0f));
                    attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                    break;
                case "LoadFromGoogleSpreadSheet":
                    attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                    break;
            }
        }
    }    
#endif
    #endregion
}
