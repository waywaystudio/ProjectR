/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class CharacterClassData : DataObject<CharacterClassData.CharacterClass>
    {
        [Serializable]
        public class CharacterClass : Row
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String className;
			[SerializeField] private String baseRole;
			[SerializeField] private Single attackSpeed;
			[SerializeField] private Single range;

			public override Int32 ID => id;
			public String ClassName => className;
			public String BaseRole => baseRole;
			public Single AttackSpeed => attackSpeed;
			public Single Range => range;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public override string SpreadSheetID => "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public override string SpreadSheetName => "ContentData";
        public override string WorkSheetName => "CharacterClass";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<CharacterClass>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "CharacterClass");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
