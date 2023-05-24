/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.GlobalData
{    
    public partial class EthosData : DataObject<EthosData.Ethos>
    {
        [Serializable]
        public class Ethos : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;

			public Int32 ID => id;
			public String Name => name;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1ql3syYiTkqAEe3caNCL2zhg2EMIivp9Q4MZACx2e6iY";
        public readonly string SpreadSheetName = "GlobalData";
        public readonly string WorkSheetName = "Ethos";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Ethos>("GlobalData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Ethos");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
