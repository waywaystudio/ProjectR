/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.ItemData
{    
    public partial class ViceMaterialData : DataObject<ViceMaterialData.ViceMaterial>
    {
        [Serializable]
        public class ViceMaterial : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private String iconFileName;

			public Int32 ID => id;
			public String Name => name;
			public String IconFileName => iconFileName;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1aVEkmzQKe1I_Osk6VFG-crUDd77jQVs3wlAaUaYQnys";
        public readonly string SpreadSheetName = "ItemData";
        public readonly string WorkSheetName = "ViceMaterial";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<ViceMaterial>("ItemData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "ViceMaterial");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
