using Newtonsoft.Json;
using UnityGoogleSheet.Core.HttpProtocolV2.Res;

namespace UnityGoogleSheet.Core.IO
{
    public class DataGenerator : ICodeGenerator
    {
        private ReadSpreadSheetResult info;
        public DataGenerator(ReadSpreadSheetResult info)
        {
             this.info = info;
        }
       
        public string Generate() => JsonConvert.SerializeObject(info);
    }
}
