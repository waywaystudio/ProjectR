namespace UnityGoogleSheet.Core.HttpProtocolV2.Res
{
    public partial class WriteObjectResult : Response
    {
        /// <summary>
        /// true = updated
        /// false = created new
        /// </summary>
        public bool isUpdate; 
    }
}
