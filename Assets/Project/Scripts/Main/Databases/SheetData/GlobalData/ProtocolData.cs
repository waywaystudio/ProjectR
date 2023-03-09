/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.GlobalData
{    
    public partial class ProtocolData : DataObject<ProtocolData.Protocol>
    {
        [Serializable]
        public class Protocol : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private Int32 index;
			[SerializeField] private String name;
			[SerializeField] private String description;

			public Int32 ID => id;
			public Int32 Index => index;
			public String Name => name;
			public String Description => description;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1ql3syYiTkqAEe3caNCL2zhg2EMIivp9Q4MZACx2e6iY";
        public readonly string SpreadSheetName = "GlobalData";
        public readonly string WorkSheetName = "Protocol";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Protocol>("GlobalData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Protocol");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
