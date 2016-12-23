using System;
using System.Collections.Generic;
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
	public static class ApplicationSettings
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
		private const string InfoNotificationskey = "InfoNotifications";
		private const string AlertNotificationskey = "AlertNotifications";
		private const string AnouncementNotificationskey = "AnouncementNotifications";
		private const string InstallationIDkey = "InstallationID";
		private const string DeviceTokenkey = "DeviceToken";
		private const string AlarmUrgentMins1key = "AlarmUrgentMins1";
		private const string AlarmUrgentMins2key = "AlarmUrgentMins2";
		private static readonly string SettingsDefault = string.Empty;
		public static readonly string SettingsDefaultURL = "https://your-nightscout-url.azurewebsites.net";
		private static readonly bool SettingsDefaultBool = false;
		private static readonly int SettingsDefaultInt = 0;

		#endregion


		public static string URL
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(URLkey, SettingsDefaultURL);
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

		public static bool InfoNotifications
		{
			get
			{
				return AppSettings.GetValueOrDefault<bool>(InfoNotificationskey, SettingsDefaultBool);
			}
			set
			{
				AppSettings.AddOrUpdateValue<bool>(InfoNotificationskey, value);
			}
		}

		public static bool AlertNotifications
		{
			get
			{
				return AppSettings.GetValueOrDefault<bool>(AlertNotificationskey, SettingsDefaultBool);
			}
			set
			{
				AppSettings.AddOrUpdateValue<bool>(AlertNotificationskey, value);
			}
		}

		public static bool AnouncementNotifications
		{
			get
			{
				return AppSettings.GetValueOrDefault<bool>(AnouncementNotificationskey, SettingsDefaultBool);
			}
			set
			{
				AppSettings.AddOrUpdateValue<bool>(AnouncementNotificationskey, value);
			}
		}

		public static string InstallationID
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(InstallationIDkey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(InstallationIDkey, value);
			}
		}

		public static string DeviceToken
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(DeviceTokenkey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(DeviceTokenkey, value);
			}
		}

		public static int AlarmUrgentMins1
		{
			get
			{
				return AppSettings.GetValueOrDefault<int>(AlarmUrgentMins1key, SettingsDefaultInt);
			}
			set
			{
				AppSettings.AddOrUpdateValue<int>(AlarmUrgentMins1key, value);
			}
		}

		public static int AlarmUrgentMins2
		{
			get
			{
				return AppSettings.GetValueOrDefault<int>(AlarmUrgentMins2key, SettingsDefaultInt);
			}
			set
			{
				AppSettings.AddOrUpdateValue<int>(AlarmUrgentMins2key, value);
			}
		}

	}
}
