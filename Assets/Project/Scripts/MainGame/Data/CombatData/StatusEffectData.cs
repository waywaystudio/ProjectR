/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.CombatData
{    
    public partial class StatusEffectData : DataObject<StatusEffectData.StatusEffect>
    {
        [Serializable]
        public class StatusEffect : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private Boolean isBuff;
			[SerializeField] private String targetLayer;
			[SerializeField] private Single duration;
			[SerializeField] private List<Single> valueList;

			public Int32 ID => id;
			public String Name => name;
			public Boolean IsBuff => isBuff;
			public String TargetLayer => targetLayer;
			public Single Duration => duration;
			public List<Single> ValueList => valueList;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1cyWz87_gPB1FD-bfOZszEQ0QCgtHhSvRZYYERxbyCFI";
        public readonly string SpreadSheetName = "CombatData";
        public readonly string WorkSheetName = "StatusEffect";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<StatusEffect>("CombatData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "StatusEffect");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
