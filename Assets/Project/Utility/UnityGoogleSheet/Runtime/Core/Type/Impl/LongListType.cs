using System.Collections.Generic;
using System.Linq;
using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(Type: typeof(List<long>), TypeName: new [] { "list<long>", "List<Long>" })]
    public class LongListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + this.GetType().Name);

            var list = new System.Collections.Generic.List<long>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                list.AddRange(datas.Select(long.Parse));
            }
            else
            { 
                    throw new UGSValueParseException("Parse Failed => " + value + " To " + this.GetType().Name);

            }
            
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<long>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}