﻿using System;
using System.Text;
using UnityGoogleSheet.Core.Exception;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core.Type
{
    [Type(Type : typeof(byte[]), TypeName : new [] { "byte[]", "Byte[]"})]
    public class ByteArrayType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Failed => " + value + " To " + this.GetType().Name);

            var bytes = Encoding.Default.GetBytes(value);
            return bytes; 
        }

        public string Write(object value)
        {
            return Encoding.Default.GetString(value as byte[] ?? Array.Empty<byte>()); 
        }
    }
}
