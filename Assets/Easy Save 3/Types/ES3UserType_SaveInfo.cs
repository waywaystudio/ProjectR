using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("saveName", "saveTime", "lastSceneName", "version")]
	public class ES3UserType_SaveInfo : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SaveInfo() : base(typeof(Main.Save.SaveInfo)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Main.Save.SaveInfo)obj;
			
			writer.WritePrivateField("saveName", instance);
			writer.WritePrivateField("saveTime", instance);
			writer.WritePrivateField("lastSceneName", instance);
			writer.WritePrivateField("version", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Main.Save.SaveInfo)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "saveName":
					instance = (Main.Save.SaveInfo)reader.SetPrivateField("saveName", reader.Read<System.String>(), instance);
					break;
					case "saveTime":
					instance = (Main.Save.SaveInfo)reader.SetPrivateField("saveTime", reader.Read<System.String>(), instance);
					break;
					case "lastSceneName":
					instance = (Main.Save.SaveInfo)reader.SetPrivateField("lastSceneName", reader.Read<System.String>(), instance);
					break;
					case "version":
					instance = (Main.Save.SaveInfo)reader.SetPrivateField("version", reader.Read<System.Single>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Main.Save.SaveInfo();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_SaveInfoArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SaveInfoArray() : base(typeof(Main.Save.SaveInfo[]), ES3UserType_SaveInfo.Instance)
		{
			Instance = this;
		}
	}
}