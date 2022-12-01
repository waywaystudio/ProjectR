/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
//     ReSharper disable ConvertToConstant.Local
#pragma warning disable CS0414

#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Data.ContentData
{    
    public partial class EquipmentData : ScriptableObject
    {
        [Serializable]
        public class Equipment
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String itemName;

			public Int32 ID => id;
			public String ItemName => itemName;

        }

        [SerializeField]
        private List<Equipment> equipmentList = new ();
        private Dictionary<int, Equipment> equipmentTable = new ();

        public List<Equipment> EquipmentList => equipmentList;
        public Dictionary<int, Equipment> EquipmentTable
        {
            get
            {
                if (equipmentTable != null) return equipmentTable;

                equipmentTable = new Dictionary<int, Equipment>();
                equipmentList.ForEach(x => equipmentTable.Add(x.ID, x));
                return equipmentTable;
            }
        }

#region Editor Functions.
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "94325414";    
  
        private void LoadFromJson()
        {
    
            equipmentList = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Equipment>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "Equipment");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }

#region Attribute Setting        
    #if UNITY_EDITOR
    public class EquipmentDrawer : OdinAttributeProcessor<EquipmentData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "equipmentList":
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
