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
    public partial class SheetTwoData : ScriptableObject
    { 
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<SheetTwo> sheetTwoList = new ();
        private Dictionary<int, SheetTwo> sheetTwoTable = new ();        

/* Properties. */
        public List<SheetTwo> SheetTwoList => sheetTwoList;
        public Dictionary<int, SheetTwo> SheetTwoTable => sheetTwoTable ??= new Dictionary<int, SheetTwo>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private string spreadSheetID = "1MH9k2cvQmNzdn0ULcWgg4phx5eHtBb10O1OM7oT3RbE";
        private string sheetID = "813505868";
    #endif
        
        public void LoadFromJson()
        {
    #if UNITY_EDITOR
            sheetTwoList = Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<SheetTwo>("TestSpreadSheet");    
    #endif              
        }
        
        public void LoadFromGoogleSpreadSheet()
        {
    #if UNITY_EDITOR
            Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseWorkSheet(spreadSheetID, "SheetTwo");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
    #endif
        }
/* innerClass. */
        [Serializable]
        public class SheetTwo
        {
			public Int32 ID;
			public String ItemName;
			public Single FloatValue;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class SheetTwoDrawer : OdinAttributeProcessor<SheetTwoData>
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