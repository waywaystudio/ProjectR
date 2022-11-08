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
using Adventurer;


namespace SpreadSheet.ContentData
{    
    public partial class AdventurerData : ScriptableObject
    { 
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<Adventurer> adventurerList = new ();
        private Dictionary<int, Adventurer> adventurerTable = new ();        

/* Properties. */
        public List<Adventurer> AdventurerList => adventurerList;
        public Dictionary<int, Adventurer> AdventurerTable => adventurerTable ??= new Dictionary<int, Adventurer>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "400488683";
    #endif

#if UNITY_EDITOR        
        private void LoadFromJson()
        {
    
            adventurerList = Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Adventurer>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            Wayway.Engine.UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "Adventurer");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
/* innerClass. */
        [Serializable]
        public class Adventurer
        {
			public Int32 ID;
			public String AdventurerName;
			public Role Role;
			public Job Job;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class AdventurerDrawer : OdinAttributeProcessor<AdventurerData>
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