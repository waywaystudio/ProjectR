/* ref from : https://bitbucket.org/snippets/Bjarkeck/keRbr4 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Wayway.Engine;

namespace Wayway.Editor
{
    public class ScriptableObjectCreator : OdinMenuEditorWindow
    {
        private ScriptableObject previewObject;
        private string targetFolder;
        private Vector2 scroll;
    
        private static HashSet<Type> ScriptableObjectTypes
        {
            get
            {
                var temp = AssemblyUtilities.GetTypes(AssemblyTypeFlags.CustomTypes)
                    .Where(t =>
                        t.IsClass &&
                        typeof(ScriptableObject).IsAssignableFrom(t) &&
                        !typeof(EditorWindow).IsAssignableFrom(t) &&
                        !typeof(UnityEditor.Editor).IsAssignableFrom(t)); 
                
                return (HashSet<Type>)GetHashSet(temp);
            }
        }
        
        private Type SelectedType
        {
            get
            {
                var m = MenuTree.Selection.LastOrDefault();
                return m?.Value as Type;
            }
        }
    
        private static IEnumerable<T> GetHashSet<T>(IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        [MenuItem("Assets/Create Scriptable Object", priority = -1000)]
        private static void ShowDialog()
        {
            var path = "Assets";
            var obj = Selection.activeObject;
            if (obj && AssetDatabase.Contains(obj))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!Directory.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }

            var window = CreateInstance<ScriptableObjectCreator>();
            window.ShowUtility();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.titleContent = new GUIContent(path);
            
            if (path != null) 
                window.targetFolder = path.Trim('/');
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            MenuWidth = 270;
            WindowPadding = Vector4.zero;

            var tree = new OdinMenuTree(false)
            {
                Config =
                {
                    DrawSearchToolbar = true
                },
                DefaultMenuStyle = OdinMenuStyle.TreeViewStyle
            };
            tree.AddRange(ScriptableObjectTypes.Where(x => !x.IsAbstract), GetMenuPathForType).AddThumbnailIcons();
            tree.SortMenuItemsByName();
            tree.Selection.SelectionConfirmed += _ => CreateAsset();
            tree.Selection.SelectionChanged += e =>
            {
                if (previewObject && !AssetDatabase.Contains(previewObject))
                {
                    DestroyImmediate(previewObject);
                }

                if (e != SelectionChangedType.ItemAdded)
                {
                    return;
                }

                var t = SelectedType;
                if (t is {IsAbstract: false})
                {
                    previewObject = CreateInstance(t);
                }
            };

            return tree;
        }

        private string GetMenuPathForType(Type t)
        {
            if (t == null || !ScriptableObjectTypes.Contains(t)) return "";
            var name = t.Name.Split('`').First().SplitPascalCase();
            return GetMenuPathForType(t.BaseType) + "/" + name;
        }

        protected override IEnumerable<object> GetTargets()
        {
            yield return previewObject;
        }

        protected override void DrawEditor(int index)
        {
            scroll = GUILayout.BeginScrollView(scroll);
            {
                base.DrawEditor(index);
            }
            GUILayout.EndScrollView();

            if (previewObject)
            {
                GUILayout.FlexibleSpace();
                SirenixEditorGUI.HorizontalLineSeparator();
                if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30)))
                {
                    CreateAsset();
                }
            }
        }

        private void CreateAsset()
        {
            if (previewObject)
            {
                var dest = $"{targetFolder}/{MenuTree.Selection.First().Name.ToPascalCase()}.asset";
                dest = AssetDatabase.GenerateUniqueAssetPath(dest);
                AssetDatabase.CreateAsset(previewObject, dest);
                AssetDatabase.Refresh();
                Selection.activeObject = previewObject;
                EditorApplication.delayCall += Close;
            }
        }
    }
}
