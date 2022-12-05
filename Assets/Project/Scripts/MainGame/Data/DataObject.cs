using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MainGame.Data
{
    public abstract class DataObject : ScriptableObject
    {
#if UNITY_EDITOR
        public abstract string SpreadSheetID { get; }
        public abstract string SpreadSheetName { get; }
        public abstract string WorkSheetName { get; }
#endif
    }

    public abstract class DataObject<T> : DataObject where T : Row
    {
        [SerializeField] protected List<T> list;
        private Dictionary<int, T> dataTable;

        public List<T> List { get => list; set => list = value; }
        public Dictionary<int, T> Table
        {
            get
            {
                if (dataTable != null) return dataTable;

                dataTable = new Dictionary<int, T>();
                list.ForEach(x => dataTable.Add(x.ID, x));
                return dataTable;
            }
            set => dataTable = value;
        }
    }
    
    #region Attribute Setting        
#if UNITY_EDITOR
    public class DataObjectDrawer<T0, T1> : Sirenix.OdinInspector.Editor.OdinAttributeProcessor<T0> where T0 : DataObject<T1> where T1 : Row
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
                // case "LoadFromJson":
                //     attributes.Add(new PropertySpaceAttribute(5f, 0f));
                //     attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                //     break;
                // case "LoadFromGoogleSpreadSheet":
                //     attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                //     break;
            }
        }
    }    
#endif
    #endregion
}
