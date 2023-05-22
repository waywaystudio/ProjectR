using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("dataIndex", "DynamicSpec", "UpgradeLevel")]
	public class ES3UserType_EquipmentEntity : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EquipmentEntity() : base(typeof(Common.Equipments.EquipmentEntity)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Common.Equipments.EquipmentEntity)obj;
			
			writer.WritePrivateField("dataIndex", instance);
			writer.WriteProperty("DynamicSpec", instance.DynamicSpec, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(Common.Spec)));
			writer.WriteProperty("UpgradeLevel", instance.UpgradeLevel, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Common.Equipments.EquipmentEntity)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "dataIndex":
					instance = (Common.Equipments.EquipmentEntity)reader.SetPrivateField("dataIndex", reader.Read<DataIndex>(), instance);
					break;
					case "DynamicSpec":
						instance.DynamicSpec = reader.Read<Common.Spec>();
						break;
					case "UpgradeLevel":
						instance.UpgradeLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Common.Equipments.EquipmentEntity();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_EquipmentEntityArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_EquipmentEntityArray() : base(typeof(Common.Equipments.EquipmentEntity[]), ES3UserType_EquipmentEntity.Instance)
		{
			Instance = this;
		}
	}
}