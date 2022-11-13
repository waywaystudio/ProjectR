using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(Type : typeof(short), TypeName : new [] { "short", "Short"})]
    public class ShortType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);

            var b = short.TryParse(value, out var @short);
            if (b == false)
            { 
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);
            }
            
            return @short;
        }

        public string Write(object value)
        {
           return value.ToString();
        }
    }
}
