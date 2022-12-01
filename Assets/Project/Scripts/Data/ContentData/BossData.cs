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
    public partial class BossData : ScriptableObject
    {
        [Serializable]
        public class Boss
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String bossName;
			[SerializeField] private Single tempDifficulty;
			[SerializeField] private List<Int32> dropItemIDList;
			[SerializeField] private List<Int32> dropItemProbabilities;

			public Int32 ID => id;
			public String BossName => bossName;
			public Single TempDifficulty => tempDifficulty;
			public List<Int32> DropItemIdList => dropItemIDList;
			public List<Int32> DropItemProbabilities => dropItemProbabilities;

        }

        [SerializeField]
        private List<Boss> bossList = new ();
        private Dictionary<int, Boss> bossTable = new ();

        public List<Boss> BossList => bossList;
        public Dictionary<int, Boss> BossTable
        {
            get
            {
                if (bossTable != null) return bossTable;

                bossTable = new Dictionary<int, Boss>();
                bossList.ForEach(x => bossTable.Add(x.ID, x));
                return bossTable;
            }
        }

#region Editor Functions.
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "75529785";    
  
        private void LoadFromJson()
        {
    
            bossList = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Boss>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(spreadSheetID, "Boss");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }

#region Attribute Setting        
    #if UNITY_EDITOR
    public class BossDrawer : OdinAttributeProcessor<BossData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case "bossList":
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
