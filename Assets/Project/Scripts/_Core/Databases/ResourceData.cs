using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Databases
{
    public abstract class ResourceData<T> : ScriptableObject, IEditable where T : Object
    {
        [SerializeField] protected string searchingTargetPath;
        [SerializeField] protected string dataObjectParamName;
        [SerializeField] protected List<ResourceTable> resourceList = new();
        
        [Serializable] public class ResourceTable
        {
            public DataIndex DataIndex;
            public T Resource;
            public DataIndex Category => DataIndex.GetCategory();
        
            public ResourceTable(DataIndex dataIndex, T resource)
            {
                DataIndex = dataIndex;
                Resource  = resource;
            }
        }

        public T Get(DataIndex index)
        {
            var result = resourceList.TryGetElement(element => element.DataIndex == index);

            return result != null ? result.Resource : null;
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            ImportResource(searchingTargetPath, dataObjectParamName);
        }
        
        
        protected virtual void ImportResource(string targetPath, string paramNames)
        {
            resourceList.Clear();

            var missingAssetLog = new StringBuilder();
            
            
            foreach (var dataObject in Database.SheetDataList)
            {
                // Get the generic type of the data object
                var genericType = dataObject.GetType().BaseType?.GetGenericArguments().FirstOrDefault();

                // Skip if there's no generic type or it doesn't implement IIdentifier
                if (genericType == null || !typeof(IIdentifier).IsAssignableFrom(genericType))
                    continue;

                // Check if the generic type has an IconPath property
                var iconPathProperty = genericType.GetProperty(paramNames);
                var idProperty = genericType.GetProperty("ID");

                // Skip if there's no IconPath property
                if (iconPathProperty == null || idProperty == null)
                    continue;

                // Get the list property from the data object
                var listProperty = dataObject.GetType().GetProperty("List", BindingFlags.NonPublic | BindingFlags.Instance);

                // Skip if there's no list or it's empty
                if (listProperty?.GetValue(dataObject) is not IList list || list.Count == 0)
                    continue;

                // Loop through all items in the list
                foreach (var item in list)
                {
                    // Get the IconPath value and add it to the list
                    var iconPath  = iconPathProperty.GetValue(item) as string;
                    var dataIndex = (int)idProperty.GetValue(item);

                    if (!Finder.TryGetObject(targetPath, iconPath, out T result))
                    {
                        missingAssetLog.AppendLine($"Not Exist {typeof(T).Name} file name of <color=yellow>{iconPath}</color>");
                        continue;
                    }
                    
                    resourceList.Add(new ResourceTable((DataIndex)dataIndex, result));
                }
            }

            if (missingAssetLog.Length != 0)
                Debug.LogWarning($"Missing {typeof(T).Name} file : \n{missingAssetLog}");
        }
#endif
    }
}
