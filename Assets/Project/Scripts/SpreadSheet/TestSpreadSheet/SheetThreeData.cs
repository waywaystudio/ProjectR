/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
//     ReSharper disable ConvertToConstant.Local
#pragma warning disable CS0414
#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpreadSheet.TestSpreadSheet
{    
    public partial class SheetThreeData : ScriptableObject
    { 
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<SheetThree> sheetThreeList = new ();
        private Dictionary<int, SheetThree> sheetThreeTable = new ();        

/* Properties. */
        public List<SheetThree> SheetThreeList => sheetThreeList;
        public Dictionary<int, SheetThree> SheetThreeTable => sheetThreeTable ??= new Dictionary<int, SheetThree>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private string spreadSheetID = "1MH9k2cvQmNzdn0ULcWgg4phx5eHtBb10O1OM7oT3RbE";
        private string sheetID = "2079770784";
    #endif
        
        public void LoadFromJson()
        {
    #if UNITY_EDITOR
            sheetThreeList = Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<SheetThree>("TestSpreadSheet");    
    #endif              
        }
        
        public void LoadFromGoogleSpreadSheet()
        {
    #if UNITY_EDITOR
            Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseWorkSheet(spreadSheetID, "SheetThree");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
    #endif
        }
/* innerClass. */
        [Serializable]
        public class SheetThree
        {
			public Int32 ID;
			public String ItemName;
			public String StringValue;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class SheetThreeDrawer : OdinAttributeProcessor<SheetThreeData>
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