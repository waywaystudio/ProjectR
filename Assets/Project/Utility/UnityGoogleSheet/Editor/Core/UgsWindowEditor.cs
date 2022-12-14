#if ODIN_INSPECTOR
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityGoogleSheet.Core;

namespace UnityGoogleSheet.Editor.Core
{
    public class UgsWindowEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/UnityGoogleSheet")]
        private static void OpenWindow()
        {
            var window = GetWindow<UgsWindowEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 800);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            if (UgsUtility.GetScriptableObject("UgsConfig", "Assets") is null)
            {
                var defaultDirectory = UgsUtility.GetScriptableObjectFilePath("UgsExplorer", "Assets")
                                                 .Replace("/UgsExplorer.asset", "");
                
                UgsUtility.CreateScriptableObject("UgsConfig", defaultDirectory);
            }
            
            var explorer = Resources.LoadAll<UgsExplorer>("").FirstOrDefault();
            var dataList = Resources.LoadAll<UgsDataList>("").FirstOrDefault();
            
            var tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "UGS Config",      UgsConfig.Instance ,      EditorIcons.SettingsCog   },
                { "UGS Generator",   explorer,                 EditorIcons.Table         },
                { "UGS DataList",    dataList,                 EditorIcons.List          },
            };

            tree.DefaultMenuStyle.Height = 40;
            tree.DefaultMenuStyle.IconSize = 20;
            tree.DefaultMenuStyle.IconOffset = -2;
                    
            return tree;
        }
    }
}
#endif