using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Databases.ResourceData
{
    public class SpriteData : ResourceData<Sprite>
    {
#if UNITY_EDITOR
        
        /*
         * Sprite 가져오는 방법이 일반적인 UnityObject와 조금 다르다.
         * Sprite MultipleMode 일 때 하위 스프라이트를 정확한 이름으로 가져올 수 있는 코드로 오버라이드 하였다.
         * 자세한 내용은 하단 Annotation 참조
         * */
        protected override void ImportResource(string targetPath, string paramNames)
        {
            resourceList.Clear();

            var spriteLog = new StringBuilder();

            foreach (var dataObject in Database.SheetDataList)
            {
                // Get the generic type of the data object
                var genericType = dataObject.GetType().BaseType?.GetGenericArguments().FirstOrDefault();

                // Skip if there's no generic type or it doesn't implement IIdentifier
                if (genericType == null || !typeof(IIdentifier).IsAssignableFrom(genericType))
                    continue;

                // Check if the generic type has an IconPath property
                var iconPathProperty = genericType.GetProperty(paramNames);
                var idProperty       = genericType.GetProperty("ID");

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

                    if (Finder.TryGetSprite(targetPath, iconPath, out var result))
                    {
                        resourceList.Add(new ResourceTable((DataIndex)dataIndex, result));
                        spriteLog.AppendLine($"Added Sprite file name of <color=green>{iconPath}</color>");
                    }
                }
            }

            if (spriteLog.Length != 0)
                Debug.Log($"Added Sprite file : \n{spriteLog}");
        }
#endif
    }
}

/* Annotation
 * 
 * The issue here is due to how Unity's AssetDatabase.LoadAssetAtPath() function works.
 * When you give it a path to an asset that contains sub-assets (like your sprite sheet),
 * it will always load the main asset by default.
 * In the case of a sprite sheet, the main asset is the full sprite sheet image, not the individual slices.
 * 
 * To load a specific sub-asset (i.e., a specific sprite from the sheet),
 * you need to use the AssetDatabase.LoadAllAssetsAtPath() method instead, which returns an array of all assets at the given path.
 * Then, you can iterate through the array to find the specific sprite you're interested in.
 *
 * Finder.TryGetSprite 참조
 *
 * In this version of the method, I specifically designed it for loading sprites,
 * so the type parameter is removed and the value output parameter is specifically a Sprite.
 * You can create a similar function for other types of sub-assets if necessary.
 * The key change here is the usage of AssetDatabase.
 * LoadAllAssetsAtPath() and the subsequent loop to check each loaded asset until it finds a Sprite with the correct name.
 *
 * Note: Like the previous method, this assumes that sprite names within a sprite sheet are unique across the entire project.
 * If you have duplicate sprite names in different sprite sheets, this method might still return the wrong sprite.
 * If that's the case, you'll need to adjust your sprite naming convention or your search method to uniquely identify each sprite.
 */
