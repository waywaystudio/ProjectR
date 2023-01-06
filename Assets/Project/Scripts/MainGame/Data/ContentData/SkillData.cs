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
			[SerializeField] private String name;
			[SerializeField] private Single baseValue;
			[SerializeField] private Single baseCoolTime;
			[SerializeField] private Single range;
			[SerializeField] private Int32 priority;
			[SerializeField] private String animationKey;
			[SerializeField] private Int32 targetCount;
			[SerializeField] private String sortingType;
			[SerializeField] private String targetLayer;
			[SerializeField] private Boolean isSelf;
			[SerializeField] private String skillType;
			[SerializeField] private Single castingTime;
			[SerializeField] private Single resourceObtain;
			[SerializeField] private Int32 statusEffectID;
			[SerializeField] private Int32 projectileID;

			public Int32 ID => id;
			public String Name => name;
			public Single BaseValue => baseValue;
			public Single BaseCoolTime => baseCoolTime;
			public Single Range => range;
			public Int32 Priority => priority;
			public String AnimationKey => animationKey;
			public Int32 TargetCount => targetCount;
			public String SortingType => sortingType;
			public String TargetLayer => targetLayer;
			public Boolean IsSelf => isSelf;
			public String SkillType => skillType;
			public Single CastingTime => castingTime;
			public Single ResourceObtain => resourceObtain;
			public Int32 StatusEffectId => statusEffectID;
			public Int32 ProjectileId => projectileID;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public readonly string SpreadSheetName = "ContentData";
        public readonly string WorkSheetName = "Skill";    
  
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
