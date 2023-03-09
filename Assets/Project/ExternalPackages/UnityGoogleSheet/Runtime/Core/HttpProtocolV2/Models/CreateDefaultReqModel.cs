// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace UnityGoogleSheet.Core.HttpProtocolV2.Models
{
    public class CreateDefaultReqModel : Model
    {
        /*
         * DO NOT change field Name and style
         * Sync with AppScript
         */
        public string folderID;
        public string fileName;

        public CreateDefaultReqModel(string folderID, string fileName)
        {
            this.folderID = folderID;
            this.fileName = fileName;
            
            instruction = (int)EInstruction.CREATE_DEFAULT_TABLE;
        }
    }
}
