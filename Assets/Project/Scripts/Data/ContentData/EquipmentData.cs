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
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<Equipment> equipmentList = new ();
        private Dictionary<int, Equipment> equipmentTable = new ();        

/* Properties. */
        public List<Equipment> EquipmentList => equipmentList;
        public Dictionary<int, Equipment> EquipmentTable => equipmentTable ??= new Dictionary<int, Equipment>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "94325414";
    #endif

#if UNITY_EDITOR        
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
/* innerClass. */
        [Serializable]
        public class Equipment
        {
			public Int32 ID;
			public String ItemName;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class EquipmentDrawer : OdinAttributeProcessor<EquipmentData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
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
    #endregion
#endif
}
