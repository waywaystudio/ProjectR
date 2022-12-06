using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.IO;
using UnityGoogleSheet.Core.Type;

namespace UnityGoogleSheet.Core
{
    public class CodeGeneratorScriptableObject : ICodeGenerator
    {
        public CodeGeneratorScriptableObject(SheetInfo info)
        {
            sheetInfo = info;
        }
        
        private static SheetInfo sheetInfo;

        public string GenerateForm { get; private set; } = 
$@"/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
@assemblies
namespace @namespace
{{    
    public partial class @Class@suffix : DataObject<@Class@suffix.@Class>
    {{
        [Serializable]
        public class @Class : IIdentifier
        {{
@types
@properties
        }}
        
#region Editor Functions.
    #if UNITY_EDITOR
        public override string SpreadSheetID => ""@spreadSheetID"";
        public override string SpreadSheetName => ""@spreadSheetName"";
        public override string WorkSheetName => ""@Class"";    
@loadFunctions
    #endif
#endregion
    }}
}}
";
        public string Generate()
        {
            var soScriptPath = UgsConfig.Instance.ScriptableObjectScriptPath; // = Assets/Project/Scripts/MainGame/Data
            var trimmedPath = soScriptPath.Replace("Assets/Project/Scripts/", "").Replace("/", ".");
            var @namespace = $"{trimmedPath}.{sheetInfo.sheetFileName}";
            var className = sheetInfo.sheetName;
            
            TypeMap.Init();

            WriteAssembly(new [] 
            {
                "System",
                "System.Collections.Generic",
                "UnityEngine"
            }, 
                sheetInfo.sheetTypes, 
                sheetInfo.isEnumChecks);

            WriteNamespace(@namespace);
            WriteLoadFunction(sheetInfo.sheetFileName);
            WritePascalClassReplace(ToPascalCasing(className));
            WriteCamelClassReplace(ToCamelCasing(className));
            WriteClassSuffix(UgsConfig.Instance.Suffix);
            WriteSpreadSheetData(sheetInfo.spreadSheetID, sheetInfo.sheetFileName);
            WriteTypes(sheetInfo.sheetTypes, 
                       sheetInfo.sheetVariableNames, 
                       sheetInfo.isEnumChecks);

            Debug.Log($"Generate <color=green><b>{className} ScriptableObject.cs</b></color> Complete");
            
            return GenerateForm;
        }
        

        private void WriteAssembly(string[] assemblies, IReadOnlyList<string> types, IReadOnlyList<bool> isEnums)
        {
            if (assemblies != null)
            {
                var builder = new StringBuilder();
                foreach (var assembly in assemblies)
                {
                    builder.AppendLine($"using {assembly};");
                }

                builder.AppendLine("@assemblies");
                GenerateForm = GenerateForm.Replace("@assemblies", builder.ToString());
            }

            if (types != null && isEnums != null)
            {
                var builder = new StringBuilder();
                var duplicationCheck = new List<string>();

                for (var i = 0; i < types.Count; i++)
                {
                    var type = types[i];
                    var isEnum = isEnums[i];

                    type = type.Replace(" ", null);
                    type = type.Replace("Enum<", null);
                    type = type.Replace(">", null);

                    if (!isEnum || string.IsNullOrEmpty(TypeMap.EnumMap[type].NameSpace)) continue;
                    
                    var namespacePhase = $"using {TypeMap.EnumMap[type].NameSpace};";

                    if (duplicationCheck.Contains(namespacePhase))
                    {
                        continue;
                    }
                        
                    duplicationCheck.Add(namespacePhase);
                    builder.AppendLine(namespacePhase);
                }
                
                GenerateForm = GenerateForm.Replace("@assemblies", builder.ToString());
            }
            else
            {
                GenerateForm = GenerateForm.Replace("@assemblies", null);
            }
        }
       
        private void WriteNamespace(string @namespace)
        {
            GenerateForm = GenerateForm.Replace("@namespace", @namespace);
        }
        
        private void WritePascalClassReplace(string @class)
        {
            GenerateForm = GenerateForm.Replace("@Class", @class);
        }
        
        private void WriteCamelClassReplace(string @class)
        {
            GenerateForm = GenerateForm.Replace("@class", @class);
        }

        private void WriteTypes(IReadOnlyList<string> types, IReadOnlyList<string> fieldNames, IReadOnlyList<bool> isEnum)
        {
            if (types == null) return;
            
            var typeBuilder = new StringBuilder();
            var propertyBuilder = new StringBuilder();
                
            for (var i = 0; i < types.Count; i++)
            {
                var condensedFieldName = fieldNames[i].Replace("\n", "");
                
                if (isEnum[i] == false)
                {
                    var targetType = types[i];
                    var targetField = fieldNames[i];
                    TypeMap.StrMap.TryGetValue(targetType, out var outType);
                        
                    if (outType == null)
                    {
                        var debugTypes = string.Join("  ", sheetInfo.sheetTypes);
                            
                        Debug.Log("<color=#00ff00><b>-------UGS IMPORTANT ERROR DEBUG---------</b></color>");
                        Debug.LogError($"<color=white><b>Error Sheet Name => </b></color>{sheetInfo.sheetFileName}.{sheetInfo.sheetName}");
                        Debug.LogError($"<color=white><b>Your type list => </b></color> => {debugTypes}");
                        Debug.LogError($"<color=#00ff00><b>error field =>:</b></color> {targetField} : {sheetInfo.sheetTypes[i]}");
                            
                        throw new TypeParserNotFoundException("Type Parser Not Found, You made your own type parser? check custom type document on gitbook document.");
                    }

                    var typeName = GetCSharpRepresentation(TypeMap.StrMap[types[i]], true);
                    var camelFieldName = ToCamelCasing(condensedFieldName);
                    var pascalFieldName = ToPascalCasing(condensedFieldName);
                    
                    typeBuilder.AppendLine($"\t\t\t[SerializeField] private {typeName} {camelFieldName};");
                    propertyBuilder.AppendLine($"\t\t\tpublic {typeName} {pascalFieldName} => {camelFieldName};");
                }
                else
                {
                    // TODO. 본 프로젝트는 Assembly definition 적용하였다. UGS Attribute를 사용하기가 힘들다보니 Sheet에 EnumType 을 안쓰게 되어 필요없는 else절이 됐다.
                    // TODO. 나중에라도 사용한다면 위 if절과 비슷하게 바꾸어야 한다.
                    var str = types[i];

                    str = str.Replace("<", null);
                    str = str.Replace(">", null);
                    str = str.Replace(" ", null);
                    str = str.Remove(0, 4);

                    typeBuilder.AppendLine($"\t\t\t[SerializeField] private {GetCSharpRepresentation(TypeMap.EnumMap[str].Type, true)} {ToCamelCasing(condensedFieldName)};");
                    propertyBuilder.AppendLine($"\t\t\tpublic {GetCSharpRepresentation(TypeMap.StrMap[types[i]], true)} {ToPascalCasing(condensedFieldName)} => {ToCamelCasing(condensedFieldName)};");
                }
            }
                
            GenerateForm = GenerateForm.Replace("@types", typeBuilder.ToString());
            GenerateForm = GenerateForm.Replace("@properties", propertyBuilder.ToString());
            GenerateForm = GenerateForm.Replace("@keyType", types[0]);
        }

        private void WriteSpreadSheetData(string spreadID, string spreadName)
        {
            GenerateForm = GenerateForm.Replace("@spreadSheetID", spreadID);
            GenerateForm = GenerateForm.Replace("@spreadSheetName", spreadName);
        }

        private void WriteLoadFunction(string sheetFileName)
        {
            var builder = new StringBuilder();
            builder.Append(@"  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<@Class>(""@sheetFileName""); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, ""@Class"");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }
");
            
            GenerateForm = GenerateForm.Replace("@loadFunctions", builder.ToString());
            GenerateForm = GenerateForm.Replace("@sheetFileName", sheetFileName);
        }
        
        private void WriteClassSuffix(string suffix)
        {
            GenerateForm = GenerateForm.Replace("@suffix", suffix);
        }
        
        private static string GetCSharpRepresentation(System.Type t, bool trimArgCount)
        {
            if (!t.IsGenericType) return t.Name;
            
            var genericArgs = t.GetGenericArguments().ToList();

            return GetCSharpRepresentation(t, trimArgCount, genericArgs);
        }

        private static string GetCSharpRepresentation(System.Type t, bool trimArgCount, IList<System.Type> availableArguments)
        {
            if (!t.IsGenericType) return t.Name;
            
            var value = t.Name;
            
            if (trimArgCount && value.IndexOf("`", StringComparison.Ordinal) > -1)
            {
                value = value[..value.IndexOf("`", StringComparison.Ordinal)];
            }

            if (t.DeclaringType != null)
            {
                value = GetCSharpRepresentation(t.DeclaringType, trimArgCount, availableArguments) + "+" + value;
            }
                
            var argString = "";
            var thisTypeArgs = t.GetGenericArguments();
                
            for (var i = 0; i < thisTypeArgs.Length && availableArguments.Count > 0; i++)
            {
                if (i != 0) argString += ", ";
                argString += GetCSharpRepresentation(availableArguments[0], trimArgCount);
                availableArguments.RemoveAt(0);
            }
                
            if (argString.Length > 0)
            {
                value += "<" + argString + ">";
            }

            return value;

        }
        
        private static string ToCamelCasing(string original)
        {
            if (string.IsNullOrEmpty(original) || char.IsLower(original, 0))
            {
                return original;
            }

            return char.ToLowerInvariant(ToPascalCasing(original)[0]) + original[1..];
        }
        
        private static string ToPascalCasing(string original)
        {
            var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            var whiteSpace = new Regex(@"(?<=\s)");
            var startsWithLowerCaseChar = new Regex("^[a-z]");
            var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                .Split(new [] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));
            
            var result = string.Concat(pascalCase);

            return result.Length <= 2
                ? result.ToUpper()
                : result;
        }
    }
}
