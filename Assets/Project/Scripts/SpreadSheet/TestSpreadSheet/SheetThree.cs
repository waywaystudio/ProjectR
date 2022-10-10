
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Wayway.Engine.UnityGoogleSheet.Core;
using Wayway.Engine.UnityGoogleSheet.Core.Attribute;
using Wayway.Engine.UnityGoogleSheet.Core.Exception;
using Wayway.Engine.UnityGoogleSheet.Core.HttpProtocolV2;


namespace TestSpreadSheet
{
    [TableStruct]
    public partial class SheetThree : ITable
    {
        public delegate void OnLoadedFromGoogleSheets(List<SheetThree> loadedList, Dictionary<int, SheetThree> loadedDictionary);

        private static bool isLoaded;
        private static string spreadSheetID = "1MH9k2cvQmNzdn0ULcWgg4phx5eHtBb10O1OM7oT3RbE"; // it is file id
        private static string sheetID = "2079770784"; // it is sheet id
        private static UnityFileReader reader = new ();

/* Your Loaded Data Storage. */
    
        public static Dictionary<int, SheetThree> SheetThreeMap = new ();  
        public static List<SheetThree> SheetThreeList = new ();   

        /// <summary>
        /// Get SheetThree List 
        /// Auto Load
        /// </summary>
        public static List<SheetThree> GetList()
        {
           if (isLoaded == false) Load();
           return SheetThreeList;
        }

        /// <summary>
        /// Get SheetThree Dictionary, keyType is your sheet A1 field type.
        /// - Auto Load
        /// </summary>
        public static Dictionary<int, SheetThree>  GetDictionary()
        {
           if (isLoaded == false) Load();
           return SheetThreeMap;
        }

    

/* Fields. */

		public System.Int32 Id;
		public System.String Itemname;
		public System.String Stringvalue;
  

#region functions


        public static void Load(bool forceReload = false)
        {
            if(isLoaded && forceReload == false)
            {
#if UGS_DEBUG
                 Debug.Log("SheetThree is already loaded! if you want reload then, forceReload parameter set true");
#endif
                 return;
            }

            var text = reader.ReadData("TestSpreadSheet"); 
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                if (result != null) CommonLoad(result.jsonObject, forceReload);

                if(!isLoaded)
                    isLoaded = true;
            }
      
        }



        public static (List<SheetThree> list, Dictionary<int, SheetThree> map) CommonLoad(Dictionary<string, Dictionary<string, List<string>>> jsonObject, bool forceReload)
        {
            var map = new Dictionary<int, SheetThree>();
            var list = new List<SheetThree>();
            TypeMap.Init();
            var fields = typeof(SheetThree).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new (); 
            var rows = new List<List<string>>();
            var sheet = jsonObject["SheetThree"];

            foreach (var column in sheet.Keys)
            {
                var split = column.Replace(" ", null).Split(':');
                var columnField = split[0];
                var columnType = split[1];

                typeInfos.Add((column, columnField, columnType));
                var typeValues = sheet[column];
                rows.Add(typeValues);
            }

            // 실제 데이터 로드
            if (rows.Count != 0)
            {
                var rowCount = rows[0].Count;
                for (var i = 0; i < rowCount; i++)
                {
                    var instance = new SheetThree();
                    for (var j = 0; j < typeInfos.Count; j++)
                    {
                        try
                        {
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            //int, float, List<..> etc
                            var type = typeInfos[j].type;
                            if (type.StartsWith(" < ") && type.Substring(1, 4) == "Enum" && type.EndsWith(">"))
                            {
                                 Debug.Log("It's Enum");
                            }

                            var readValue = TypeMap.Map[typeInfo].Read(rows[j][i]);
                            fields[j].SetValue(instance, readValue);

                        }
                        catch (Exception e)
                        {
                            if (e is UGSValueParseException)
                            {
                                Debug.LogError("<color=red> UGS Value Parse Failed! </color>");
                                Debug.LogError(e);
                                return (null, null);
                            }

                            //enum parse
                            var type = typeInfos[j].type;
                            type = type.Replace("Enum<", null);
                            type = type.Replace(">", null);

                            var readValue = TypeMap.EnumMap[type].Read(rows[j][i]);
                            fields[j].SetValue(instance, readValue); 
                        }
                      
                    }
                    list.Add(instance); 
                    map.Add(instance.Id, instance);
                }

                if(isLoaded == false || forceReload)
                { 
                    SheetThreeList = list;
                    SheetThreeMap = map;
                    isLoaded = true;
                }
            }
 
            return (list, map); 
        }
 

        public static void Write(SheetThree data, Action<WriteObjectResult> onWriteCallback = null)
        { 
            TypeMap.Init();
            var fields = typeof(SheetThree).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var dataList = new string[fields.Length];
            for (var i = 0; i < fields.Length; i++)
            {
                var type = fields[i].FieldType;                
                var writeRule = type.IsEnum ? TypeMap.EnumMap[type.Name].Write(fields[i].GetValue(data)) 
                                            : TypeMap.Map[type].Write(fields[i].GetValue(data)); 

                dataList[i] = writeRule; 
            }             
#if UNITY_EDITOR
            UnityPlayerWebRequest.Instance.WriteObject(new WriteObjectReqModel(spreadSheetID, sheetID, dataList[0], dataList), null, onWriteCallback);
#endif
        } 
          

#endregion

#region OdinInspectorExtensions
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button("UploadToSheet")]
        public void Upload()
        {
            Write(this);
        }    
#endif 
#endregion
    }
}
        