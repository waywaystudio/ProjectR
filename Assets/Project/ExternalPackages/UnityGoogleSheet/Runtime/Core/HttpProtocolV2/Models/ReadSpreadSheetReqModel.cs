// ReSharper disable InconsistentNaming

namespace UnityGoogleSheet.Core.HttpProtocolV2.Models
{
    public class GetDriveDirectoryReqModel : Model
    {
        /*
         * DO NOT change enum name and style
         * Sync with AppScript
         */
        public string folderId;

        public GetDriveDirectoryReqModel(string folderId)
        {
            this.folderId = folderId;
            
            instruction = (int)EInstruction.SEARCH_GOOGLE_DRIVE;
        }
    }
}
