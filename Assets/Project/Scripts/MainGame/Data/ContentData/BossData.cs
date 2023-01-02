/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class BossData : DataObject<BossData.Boss>
    {
        [Serializable]
        public class Boss : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private Single maxHp;
			[SerializeField] private Single maxResource;
			[SerializeField] private Single moveSpeed;
			[SerializeField] private Single critical;
			[SerializeField] private Single haste;
			[SerializeField] private Single hit;
			[SerializeField] private Single evade;
			[SerializeField] private Single armor;
			[SerializeField] private List<Int32> dropItemIDList;
			[SerializeField] private List<Int32> dropItemProbabilities;

			public Int32 ID => id;
			public String Name => name;
			public Single MaxHp => maxHp;
			public Single MaxResource => maxResource;
			public Single MoveSpeed => moveSpeed;
			public Single Critical => critical;
			public Single Haste => haste;
			public Single Hit => hit;
			public Single Evade => evade;
			public Single Armor => armor;
			public List<Int32> DropItemIdList => dropItemIDList;
			public List<Int32> DropItemProbabilities => dropItemProbabilities;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public readonly string SpreadSheetName = "ContentData";
        public readonly string WorkSheetName = "Boss";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Boss>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Boss");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
