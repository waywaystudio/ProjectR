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
        [Serializable]
        public class Skill
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String skillName;
			[SerializeField] private Single baseCoolTime;
			[SerializeField] private Single range;
			[SerializeField] private Int32 priority;
			[SerializeField] private List<String> assignedClass;
			[SerializeField] private String motionType;
			[SerializeField] private Int32 targetCount;
			[SerializeField] private String targetLayer;
			[SerializeField] private String skillType;
			[SerializeField] private Single castingTime;

			public Int32 ID => id;
			public String SkillName => skillName;
			public Single BaseCoolTime => baseCoolTime;
			public Single Range => range;
			public Int32 Priority => priority;
			public List<String> AssignedClass => assignedClass;
			public String MotionType => motionType;
			public Int32 TargetCount => targetCount;
			public String TargetLayer => targetLayer;
			public String SkillType => skillType;
			public Single CastingTime => castingTime;

        }

        [SerializeField]
        private List<Skill> skillList = new ();
        private Dictionary<int, Skill> skillTable = new ();

        public List<Skill> SkillList => skillList;
        public Dictionary<int, Skill> SkillTable
        {
            get
            {
                if (skillTable != null) return skillTable;

                skillTable = new Dictionary<int, Skill>();
                skillList.ForEach(x => skillTable.Add(x.ID, x));
                return skillTable;
            }
        }

#region Editor Functions.
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "1563938778";    
  
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
#endregion
    }

#region Attribute Setting        
    #if UNITY_EDITOR
    public class SkillDrawer : OdinAttributeProcessor<SkillData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "skillList":
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
