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
    public partial class SkillData : ScriptableObject
    { 
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<Skill> skillList = new ();
        private Dictionary<int, Skill> skillTable = new ();        

/* Properties. */
        public List<Skill> SkillList => skillList;
        public Dictionary<int, Skill> SkillTable => skillTable ??= new Dictionary<int, Skill>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "1563938778";
    #endif

#if UNITY_EDITOR        
        private void LoadFromJson()
        {
    
            skillList = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Skill>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "Skill");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
/* innerClass. */
        [Serializable]
        public class Skill
        {
			public Int32 ID;
			public String SkillName;
			public Single BaseCoolTime;
			public List<String> AssignedClass;
			public String ActionType;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class SkillDrawer : OdinAttributeProcessor<SkillData>
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
