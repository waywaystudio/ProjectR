/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class SkillData : DataObject<SkillData.Skill>
    {
        [Serializable]
        public class Skill : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String textKey;
			[SerializeField] private Single baseCoolTime;
			[SerializeField] private Single range;
			[SerializeField] private Int32 priority;
			[SerializeField] private List<String> assignedClass;
			[SerializeField] private String animationKey;
			[SerializeField] private Int32 targetCount;
			[SerializeField] private String targetLayer;
			[SerializeField] private String skillType;
			[SerializeField] private Single castingTime;

			public Int32 ID => id;
			public String TextKey => textKey;
			public Single BaseCoolTime => baseCoolTime;
			public Single Range => range;
			public Int32 Priority => priority;
			public List<String> AssignedClass => assignedClass;
			public String AnimationKey => animationKey;
			public Int32 TargetCount => targetCount;
			public String TargetLayer => targetLayer;
			public String SkillType => skillType;
			public Single CastingTime => castingTime;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public override string SpreadSheetID => "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public override string SpreadSheetName => "ContentData";
        public override string WorkSheetName => "Skill";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Skill>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Skill");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
