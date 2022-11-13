using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(typeof(string), "string", "String")]
    public class StringType : IType
    {
        public object DefaultValue => string.Empty;
        public object Read(string value)
        {
           return value;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}

