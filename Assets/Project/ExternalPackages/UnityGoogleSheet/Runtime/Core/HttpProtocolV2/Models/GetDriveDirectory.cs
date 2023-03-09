// ReSharper disable InconsistentNaming

namespace UnityGoogleSheet.Core.HttpProtocolV2.Models
{
    public class ReadSpreadSheetReqModel : Model
    {
        /*
         * DO NOT change enum name and style
         * Sync with AppScript
         */
        public string fileId;

        public ReadSpreadSheetReqModel(string fileId)
        {
            this.fileId = fileId;
            
            instruction = (int)EInstruction.READ_SPREADSHEET;
        }
    }
}
