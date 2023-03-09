// ReSharper disable ClassNeverInstantiated.Global

namespace UnityGoogleSheet.Core.HttpProtocolV2.Models
{
    public class CopyExampleReqModel : Model
    {
        public CopyExampleReqModel()
        {
            instruction = (int)EInstruction.COPY_EXAMPLE;
        }
    }
}
