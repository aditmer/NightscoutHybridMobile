using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace NightscoutMobileHybrid
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class AppSettings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string URLkey = "URL";
		private const string AzureTagkey = "AzureTag";
		private static readonly string SettingsDefault = string.Empty;

		#endregion


		public static string URL
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(URLkey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(URLkey, value);


			}
		}

		public static string AzureTag
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(AzureTagkey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(AzureTagkey, value);
			}
		}



	}
}
