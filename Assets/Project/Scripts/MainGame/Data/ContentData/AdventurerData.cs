/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class AdventurerData : DataObject<AdventurerData.Adventurer>
    {
        [Serializable]
        public class Adventurer : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String textKey;
			[SerializeField] private String role;
			[SerializeField] private String job;

			public Int32 ID => id;
			public String TextKey => textKey;
			public String Role => role;
			public String Job => job;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public override string SpreadSheetID => "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public override string SpreadSheetName => "ContentData";
        public override string WorkSheetName => "Adventurer";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Adventurer>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Adventurer");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
