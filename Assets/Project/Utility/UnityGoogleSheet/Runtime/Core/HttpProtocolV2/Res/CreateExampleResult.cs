// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedField.Global

namespace UnityGoogleSheet.Core.HttpProtocolV2.Res
{
    public partial class CreateExampleResult : Response
    {
        /*
         * DO NOT change enum name and style
         * Sync with AppScript
         */
        /// <summary>
        /// true = updated
        /// false = created new
        /// </summary>
        public string createdFolderId; 
    }
}
