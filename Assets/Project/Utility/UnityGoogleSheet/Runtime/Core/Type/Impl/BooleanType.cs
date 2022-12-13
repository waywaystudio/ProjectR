using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(typeof(bool), "bool", "Bool")]
    public class BooleanType : IType
    {
        public object DefaultValue => 0.0f;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);

            var b = bool.TryParse(value, out var bl);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Failed => " + value + " To " + GetType().Name);
            }
            return bl;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
