using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("DataIndex", "Level")]
	public class ES3UserType_WeaponEntity : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_WeaponEntity() : base(typeof(Common.Equipments.WeaponEntity)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Common.Equipments.WeaponEntity)obj;
			
			writer.WriteProperty("DataIndex", instance.DataIndex, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(DataIndex)));
			writer.WriteProperty("Level", instance.Level, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Common.Equipments.WeaponEntity)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "DataIndex":
						instance.DataIndex = reader.Read<DataIndex>();
						break;
					case "Level":
						instance.Level = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Common.Equipments.WeaponEntity();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_WeaponEntityArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_WeaponEntityArray() : base(typeof(Common.Equipments.WeaponEntity[]), ES3UserType_WeaponEntity.Instance)
		{
			Instance = this;
		}
	}
}