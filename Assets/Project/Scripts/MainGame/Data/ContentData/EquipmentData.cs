/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class EquipmentData : DataObject<EquipmentData.Equipment>
    {
        [Serializable]
        public class Equipment : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String textKey;

			public Int32 ID => id;
			public String TextKey => textKey;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public override string SpreadSheetID => "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public override string SpreadSheetName => "ContentData";
        public override string WorkSheetName => "Equipment";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Equipment>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Equipment");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
