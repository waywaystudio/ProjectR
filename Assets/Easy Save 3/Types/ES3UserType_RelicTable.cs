using System;
using Common;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Table", "CurrentRelicType")]
	public class ES3UserType_RelicTable : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_RelicTable() : base(typeof(RelicTable)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (RelicTable)obj;
			
			writer.WriteProperty("Table", instance.Table, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.Dictionary<Common.RelicType, RelicEntity>)));
			writer.WritePrivateProperty("CurrentRelicType", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (RelicTable)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Table":
						instance.Table = reader.Read<System.Collections.Generic.Dictionary<Common.RelicType, RelicEntity>>();
						break;
					case "CurrentRelicType":
					instance = (RelicTable)reader.SetPrivateProperty("CurrentRelicType", reader.Read<Common.RelicType>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new RelicTable();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_RelicTableArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_RelicTableArray() : base(typeof(RelicTable[]), ES3UserType_RelicTable.Instance)
		{
			Instance = this;
		}
	}
}