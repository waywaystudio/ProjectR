/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
//     ReSharper disable ConvertToConstant.Local
#pragma warning disable CS0414

#if UNITY_EDITOR
using System.Reflection;
using Sirenix.OdinInspector.Editor;
#endif
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Data.ContentData
{    
    public partial class CharacterClassData : ScriptableObject
    {
        [Serializable]
        public class CharacterClass
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String className;
			[SerializeField] private String baseRole;
			[SerializeField] private Single attackSpeed;
			[SerializeField] private Single range;

			public Int32 ID => id;
			public String ClassName => className;
			public String BaseRole => baseRole;
			public Single AttackSpeed => attackSpeed;
			public Single Range => range;

        }

        [SerializeField]
        private List<CharacterClass> characterClassList = new ();
        private Dictionary<int, CharacterClass> characterClassTable = new ();

        public List<CharacterClass> CharacterClassList => characterClassList;
        public Dictionary<int, CharacterClass> CharacterClassTable
        {
            get
            {
                if (characterClassTable != null) return characterClassTable;

                characterClassTable = new Dictionary<int, CharacterClass>();
                characterClassList.ForEach(x => characterClassTable.Add(x.ID, x));
                return characterClassTable;
            }
        }

#region Editor Functions.
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "210782231";    
  
        private void LoadFromJson()
        {
    
            characterClassList = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<CharacterClass>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "CharacterClass");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }

#region Attribute Setting        
    #if UNITY_EDITOR
    public class CharacterClassDrawer : OdinAttributeProcessor<CharacterClassData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "characterClassList":
                    attributes.Add(new TableListAttribute
                    {
                        AlwaysExpanded = true,
                        HideToolbar = true,
                        DrawScrollView = true,
                        IsReadOnly = true
                    });
                    break;
                case "LoadFromJson":
                    attributes.Add(new PropertySpaceAttribute(5f, 0f));
                    attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                    break;
                case "LoadFromGoogleSpreadSheet":
                    attributes.Add(new ButtonAttribute(ButtonSizes.Medium));
                    break;
            }
        }
    }    
    #endif
#endregion

}
