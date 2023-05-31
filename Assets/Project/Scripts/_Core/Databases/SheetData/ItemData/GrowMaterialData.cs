/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.ItemData
{    
    public partial class GrowMaterialData : DataObject<GrowMaterialData.GrowMaterial>
    {
        [Serializable]
        public class GrowMaterial : IIdentifier
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
        public readonly string WorkSheetName = "GrowMaterial";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<GrowMaterial>("ItemData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "GrowMaterial");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
