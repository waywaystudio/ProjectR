#if UNITY_EDITOR
using System.Reflection;
#endif
using System.Collections.Generic;
using System.IO;
using Databases;
using UnityEngine;
// ReSharper disable CheckNamespace

public partial class Database
{
#if UNITY_EDITOR
    [SerializeField] private string iconPath;
    [SerializeField] private string idCodePath;
    [SerializeField] private string dataScriptPath;
    [SerializeField] private string dataObjectPath;
        
    /// <summary>
    /// Editor Function
    /// </summary>
    public static bool TryGetIcon(string iconName, out Sprite icon)
    {
        icon = !Finder.TryGetObject($"{Instance.iconPath}", $"{iconName}", out Sprite result, true)
            ? null
            : result;

        return icon is not null;
    }

    private void EditorSetUp()
    {
        CreateAndUpdateDataObjects();
        GenerateIDCode();
    }


    // SheetData Generator
    private void CreateAndUpdateDataObjects()
    {
        Finder.TryGetObjectList(dataScriptPath, $"t:MonoScript, Data", out List<UnityEditor.MonoScript> monoList);
      
        monoList.ForEach(x =>
        {
            if (!x.name.EndsWith("Data")) return;
            if (!Finder.TryGetObject(dataObjectPath, x.name, out ScriptableObject dataObject))
            {
                dataObject = Finder.CreateScriptableObject(dataObjectPath, x.name, x.name);
            }

            var dataObjectType = dataObject.GetType();
            var info = dataObjectType.GetMethod("LoadFromJson", BindingFlags.NonPublic | BindingFlags.Instance);

            if (info != null)
            {
                info.Invoke(dataObject, null);
            }

            UnityEditor.EditorUtility.SetDirty(dataObject);
        });
            
        Finder.TryGetObjectList(out sheetDataList);

        sheetDataList.ForEach(x => x.Index = x.name.Replace("Data", "").ToEnum<DataIndex>());
        sheetDataList.Sort((dataA, dataB) => dataA.Index.CompareTo(dataB.Index));
            
        UnityEditor.AssetDatabase.Refresh();
    }
        
    private void GenerateIDCode()
    {
        if (!Directory.Exists(idCodePath))
            Directory.CreateDirectory(idCodePath);
            
        File.WriteAllText($"{idCodePath}/DataIndex.cs", DataIndexGenerator.Generate());
    }
        
    private void OpenSpreadSheetPanel() 
        => UnityEditor.EditorApplication.ExecuteMenuItem("Tools/UnityGoogleSheet");
        
    [UnityEditor.MenuItem("Quick Menu/Data")]
    private static void QuickMenu()
    {
        var mainObject = GameObject.Find("CoreGameObject");
        if (mainObject != null)
        {
            var dbComponent = mainObject.GetComponentInChildren<Database>();
                
            UnityEditor.EditorUtility.OpenPropertyEditor(dbComponent);
        }
        else
        {
            Debug.LogWarning("MainData GameObject not found in scene hierarchy.");
        }
    }
#endif
}
