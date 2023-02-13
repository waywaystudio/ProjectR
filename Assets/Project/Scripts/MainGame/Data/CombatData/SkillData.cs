/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.CombatData
{    
    public partial class SkillData : DataObject<SkillData.Skill>
    {
        [Serializable]
        public class Skill : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private String description;
			[SerializeField] private Single processTime;
			[SerializeField] private Single coolTime;
			[SerializeField] private Single cost;
			[SerializeField] private Int32 basePriority;
			[SerializeField] private String animationKey;
			[SerializeField] private String sortingType;
			[SerializeField] private List<String> completionList;
			[SerializeField] private String targetSystem;
			[SerializeField] private Single targetParam1;
			[SerializeField] private Single targetParam2;

			public Int32 ID => id;
			public String Name => name;
			public String Description => description;
			public Single ProcessTime => processTime;
			public Single CoolTime => coolTime;
			public Single Cost => cost;
			public Int32 BasePriority => basePriority;
			public String AnimationKey => animationKey;
			public String SortingType => sortingType;
			public List<String> CompletionList => completionList;
			public String TargetSystem => targetSystem;
			public Single TargetParam1 => targetParam1;
			public Single TargetParam2 => targetParam2;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1cyWz87_gPB1FD-bfOZszEQ0QCgtHhSvRZYYERxbyCFI";
        public readonly string SpreadSheetName = "CombatData";
        public readonly string WorkSheetName = "Skill";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Skill>("CombatData"); 
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
