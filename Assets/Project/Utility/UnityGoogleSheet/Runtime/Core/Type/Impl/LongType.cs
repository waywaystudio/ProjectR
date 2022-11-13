using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(Type : typeof(long), TypeName : new [] {"long","Long"})]
    public class LongType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);

            var b = long.TryParse(value, out var @long);
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
