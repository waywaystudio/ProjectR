/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Data.ContentData
{    
    public partial class ProjectorData : DataObject<ProjectorData.Projector>
    {
        [Serializable]
        public class Projector : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private Vector2 size;
			[SerializeField] private Single castingTime;
			[SerializeField] private String targetLayerType;
			[SerializeField] private String shapeType;
			[SerializeField] private Single combatValue;
			[SerializeField] private Int32 statusEffectID;
			[SerializeField] private String particleKey;

			public Int32 ID => id;
			public String Name => name;
			public Vector2 Size => size;
			public Single CastingTime => castingTime;
			public String TargetLayerType => targetLayerType;
			public String ShapeType => shapeType;
			public Single CombatValue => combatValue;
			public Int32 StatusEffectId => statusEffectID;
			public String ParticleKey => particleKey;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public readonly string SpreadSheetName = "ContentData";
        public readonly string WorkSheetName = "Projector";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Projector>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Projector");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
