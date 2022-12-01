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
    public partial class RaidData : ScriptableObject
    {
        [Serializable]
        public class Raid
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String raidName;
			[SerializeField] private Int32 partyScale;
			[SerializeField] private List<String> bossList;

			public Int32 ID => id;
			public String RaidName => raidName;
			public Int32 PartyScale => partyScale;
			public List<String> BossList => bossList;

        }

        [SerializeField]
        private List<Raid> raidList = new ();
        private Dictionary<int, Raid> raidTable = new ();

        public List<Raid> RaidList => raidList;
        public Dictionary<int, Raid> RaidTable
        {
            get
            {
                if (raidTable != null) return raidTable;

                raidTable = new Dictionary<int, Raid>();
                raidList.ForEach(x => raidTable.Add(x.ID, x));
                return raidTable;
            }
        }

#region Editor Functions.
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "898406998";    
  
        private void LoadFromJson()
        {
    
            raidList = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Raid>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "Raid");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }

#region Attribute Setting        
    #if UNITY_EDITOR
    public class RaidDrawer : OdinAttributeProcessor<RaidData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "raidList":
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
