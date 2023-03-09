using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(Type : typeof(ulong), TypeName : new [] {"ulong","ULong"})]
    public class ULongType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);

            var b = ulong.TryParse(value, out var @long);
            if (b == false)
            { 
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);
            }
            return @long;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
