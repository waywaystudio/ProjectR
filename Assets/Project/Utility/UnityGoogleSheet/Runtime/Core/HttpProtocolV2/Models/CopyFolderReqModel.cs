// ReSharper disable InconsistentNaming

namespace UnityGoogleSheet.Core.HttpProtocolV2.Models
{
    public class CopyFolderReqModel : Model
    {
        /*
         * DO NOT change enum name and style
         * Sync with AppScript
         */
        
        public string folderId;

        public CopyFolderReqModel(string folderId)
        {
            this.folderId = folderId;
            
            instruction = (int)EInstruction.COPY_FOLDER;
        }
    }
}
