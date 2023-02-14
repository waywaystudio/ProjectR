using System.Collections.Generic;
using System.Linq;
using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(Type: typeof(List<float>), TypeName: new [] { "list<float>", "List<Float>", "List<float>" })]
    public class FloatListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + this.GetType().Name);

            var list = new System.Collections.Generic.List<float>();
            if (value == "[]") return list;

            var data = ReadUtil.GetBracketValueToArray(value);
            if (data != null)
            {
                list.AddRange(data.Select(float.Parse));
            }
            else
            { 
                    throw new UGSValueParseException("Parse Failed => " + value + " To " + this.GetType().Name);

            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<float>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}