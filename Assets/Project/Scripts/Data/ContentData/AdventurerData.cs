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
    public partial class AdventurerData : ScriptableObject
    {
        [Serializable]
        public class Adventurer
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String adventurerName;
			[SerializeField] private String role;
			[SerializeField] private String job;

			public Int32 ID => id;
			public String AdventurerName => adventurerName;
			public String Role => role;
			public String Job => job;

        }

        [SerializeField]
        private List<Adventurer> adventurerList = new ();
        private Dictionary<int, Adventurer> adventurerTable = new ();

        public List<Adventurer> AdventurerList => adventurerList;
        public Dictionary<int, Adventurer> AdventurerTable
        {
            get
            {
                if (adventurerTable != null) return adventurerTable;

                adventurerTable = new Dictionary<int, Adventurer>();
                adventurerList.ForEach(x => adventurerTable.Add(x.ID, x));
                return adventurerTable;
            }
        }

#region Editor Functions.
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "400488683";    
  
        private void LoadFromJson()
        {
    
            adventurerList = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Adventurer>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "Adventurer");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }

#region Attribute Setting        
    #if UNITY_EDITOR
    public class AdventurerDrawer : OdinAttributeProcessor<AdventurerData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "adventurerList":
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
