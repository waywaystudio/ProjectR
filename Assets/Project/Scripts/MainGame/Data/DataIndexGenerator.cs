using System.Text;
using Core;

namespace MainGame.Data
{
    public static class DataIndexGenerator
    {
#if UNITY_EDITOR
        public static string GenerateForm { get; private set; } = 
$@"/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'IDCodeGenerator.cs'  */
// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace

public enum DataIndex
{{        
@IDCode        
}}
";
        public static string Generate()
        {
            WriteIDCode();

            return GenerateForm;
        }

        private static void WriteIDCode()
        {
            var dataList = MainData.DataList;
            var builder = new StringBuilder();
            
            builder.AppendLine("\tNone = 0,");
            builder.AppendLine("");
            
            dataList.ForEach(x =>
            {
                builder.AppendLine($"\t/* {x.name} */");
                
                x.KeyList.ForEach(y =>
                {
                    builder.AppendLine($"\t{DataIndexConvention(y.Name)} = {y.ID},");
                });
                
                builder.AppendLine("");
            });
            
            builder.AppendLine("\tEnd = int.MaxValue");
            GenerateForm = GenerateForm.Replace("@IDCode", builder.ToString());
        }

        private static string DataIndexConvention(string original)
        {
            return original.Replace(" ", "").ToPascalCase();
        }
#endif
    }
}


