/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.CombatData
{    
    public partial class SkillData : DataObject<SkillData.Skill>
    {
        [Serializable]
        public class Skill : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private String behaviourMask;
			[SerializeField] private Int32 priority;
			[SerializeField] private String detectorType;
			[SerializeField] private String sortingType;
			[SerializeField] private Vector2 targetParam;
			[SerializeField] private String targetLayer;
			[SerializeField] private String animationKey;
			[SerializeField] private Boolean isLoop;
			[SerializeField] private Boolean hasEvent;
			[SerializeField] private String skillType;
			[SerializeField] private String animationCallback;
			[SerializeField] private Single coolTime;
			[SerializeField] private String coolTimeInvoker;
			[SerializeField] private Single castTime;
			[SerializeField] private String castCallback;
			[SerializeField] private List<Single> completionValueList;
			[SerializeField] private Single cost;
			[SerializeField] private Int32 statusEffect;
			[SerializeField] private String iconFileName;
			[SerializeField] private String description;

			public Int32 ID => id;
			public String Name => name;
			public String BehaviourMask => behaviourMask;
			public Int32 Priority => priority;
			public String DetectorType => detectorType;
			public String SortingType => sortingType;
			public Vector2 TargetParam => targetParam;
			public String TargetLayer => targetLayer;
			public String AnimationKey => animationKey;
			public Boolean IsLoop => isLoop;
			public Boolean HasEvent => hasEvent;
			public String SkillType => skillType;
			public String AnimationCallback => animationCallback;
			public Single CoolTime => coolTime;
			public String CoolTimeInvoker => coolTimeInvoker;
			public Single CastTime => castTime;
			public String CastCallback => castCallback;
			public List<Single> CompletionValueList => completionValueList;
			public Single Cost => cost;
			public Int32 StatusEffect => statusEffect;
			public String IconFileName => iconFileName;
			public String Description => description;

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
