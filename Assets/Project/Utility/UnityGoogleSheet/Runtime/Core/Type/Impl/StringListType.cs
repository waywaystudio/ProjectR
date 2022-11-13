﻿using System.Linq;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(typeof(System.Collections.Generic.List<string>), "list<string>", "List<string>")]
    public class StringListType : IType
    {
        public object DefaultValue => string.Empty;
        public object Read(string value)
        {
            var datas = ReadUtil.GetBracketValueToArray(value);
            return datas.ToList();
        }

        public string Write(object value)
        {
            return WriteUtil.SetValueToBracketArray<System.Collections.Generic.List<string>, string>(value);
        }
    }
}

