/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class RaidData : DataObject<RaidData.Raid>
    {
        [Serializable]
        public class Raid : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String textKey;
			[SerializeField] private Int32 partyScale;
			[SerializeField] private List<String> bossList;

			public Int32 ID => id;
			public String TextKey => textKey;
			public Int32 PartyScale => partyScale;
			public List<String> BossList => bossList;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public override string SpreadSheetID => "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public override string SpreadSheetName => "ContentData";
        public override string WorkSheetName => "Raid";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Raid>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Raid");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
