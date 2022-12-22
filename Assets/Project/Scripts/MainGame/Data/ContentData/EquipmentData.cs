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
			[SerializeField] private String name;
			[SerializeField] private String part;
			[SerializeField] private Single hp;
			[SerializeField] private Single critical;
			[SerializeField] private Single haste;
			[SerializeField] private Single hit;
			[SerializeField] private Single evade;
			[SerializeField] private Single armor;

			public Int32 ID => id;
			public String Name => name;
			public String Part => part;
			public Single HP => hp;
			public Single Critical => critical;
			public Single Haste => haste;
			public Single Hit => hit;
			public Single Evade => evade;
			public Single Armor => armor;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public readonly string SpreadSheetName = "ContentData";
        public readonly string WorkSheetName = "Equipment";    
  
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
