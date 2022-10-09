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
    public partial class SheetOneData : ScriptableObject
    { 
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<SheetOne> sheetOneList = new ();
        private Dictionary<int, SheetOne> sheetOneTable = new ();        

/* Properties. */
        public List<SheetOne> SheetOneList => sheetOneList;
        public Dictionary<int, SheetOne> SheetOneTable => sheetOneTable ??= new Dictionary<int, SheetOne>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private string spreadSheetID = "1MH9k2cvQmNzdn0ULcWgg4phx5eHtBb10O1OM7oT3RbE";
        private string sheetID = "0";
    #endif
        
        public void LoadFromJson()
        {
    #if UNITY_EDITOR
            sheetOneList = Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<SheetOne>("TestSpreadSheet");    
    #endif              
        }
        
        public void LoadFromGoogleSpreadSheet()
        {
    #if UNITY_EDITOR
            Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseWorkSheet(spreadSheetID, "SheetOne");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
    #endif
        }
/* innerClass. */
        [Serializable]
        public class SheetOne
        {
			public Int32 ID;
			public String ItemName;
			public Int32 IntegerValue;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class SheetOneDrawer : OdinAttributeProcessor<SheetOneData>
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