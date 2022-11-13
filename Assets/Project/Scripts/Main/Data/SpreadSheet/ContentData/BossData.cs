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


namespace Main.Data.SpreadSheet.ContentData
{    
    public partial class BossData : ScriptableObject
    { 
/* Fields. */    
        [SerializeField] 
        [TableList(AlwaysExpanded = true, HideToolbar = true, DrawScrollView = true, IsReadOnly = true)] 
        private List<Boss> bossList = new ();
        private Dictionary<int, Boss> bossTable = new ();        

/* Properties. */
        public List<Boss> BossList => bossList;
        public Dictionary<int, Boss> BossTable => bossTable ??= new Dictionary<int, Boss>();

/* Editor Functions. */
    #if UNITY_EDITOR
        private readonly string spreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        private readonly string sheetID = "75529785";
    #endif

#if UNITY_EDITOR        
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
/* innerClass. */
        [Serializable]
        public class Boss
        {
			public Int32 ID;
			public String BossName;
			public Single TempDifficulty;
			public List<Int32> DropItemIdList;
			public List<Int32> DropItemProbabilities;

        }
    }
        
#if UNITY_EDITOR
    #region Attribute Setting
    public class BossDrawer : OdinAttributeProcessor<BossData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
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
    #endregion
#endif
}
