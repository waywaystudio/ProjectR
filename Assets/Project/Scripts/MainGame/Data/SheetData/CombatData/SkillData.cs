/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.SheetData.CombatData
{    
    public partial class SkillData : DataObject<SkillData.Skill>
    {
        [Serializable]
        public class Skill : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private String description;
			[SerializeField] private String skillType;
			[SerializeField] private List<Single> completionValueList;
			[SerializeField] private Single processTime;
			[SerializeField] private Single coolTime;
			[SerializeField] private Single cost;
			[SerializeField] private Int32 basePriority;
			[SerializeField] private String animationKey;
			[SerializeField] private Boolean isRigid;
			[SerializeField] private String sortingType;
			[SerializeField] private String targetLayer;
			[SerializeField] private Vector2 targetParam;
			[SerializeField] private Int32 statusEffect;

			public Int32 ID => id;
			public String Name => name;
			public String Description => description;
			public String SkillType => skillType;
			public List<Single> CompletionValueList => completionValueList;
			public Single ProcessTime => processTime;
			public Single CoolTime => coolTime;
			public Single Cost => cost;
			public Int32 BasePriority => basePriority;
			public String AnimationKey => animationKey;
			public Boolean IsRigid => isRigid;
			public String SortingType => sortingType;
			public String TargetLayer => targetLayer;
			public Vector2 TargetParam => targetParam;
			public Int32 StatusEffect => statusEffect;

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
