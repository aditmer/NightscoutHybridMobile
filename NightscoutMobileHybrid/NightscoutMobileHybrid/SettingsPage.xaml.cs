using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NightscoutMobileHybrid
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();


			entURL.Text = ApplicationSettings.URL;
			swInfo.IsToggled = ApplicationSettings.InfoNotifications;
			swAlert.IsToggled = ApplicationSettings.AlertNotifications;
			swAnouncement.IsToggled = ApplicationSettings.AnouncementNotifications;
		}

		async void btnSave_Clicked(object sender, System.EventArgs e)
		{
			string sURL = entURL.Text.ToLower();
			sURL = sURL.Replace("https", "");
			//sURL = sURL.Replace("HTTPS", "");
			sURL = sURL.Replace("http", "");
			//sURL = sURL.Replace("HTTP", "");
			sURL = sURL.Replace("://", "");

			sURL = "https://" + sURL;

			ApplicationSettings.URL = sURL;
			ApplicationSettings.InfoNotifications = swInfo.IsToggled;
			ApplicationSettings.AlertNotifications = swAlert.IsToggled;
			ApplicationSettings.AnouncementNotifications = swAnouncement.IsToggled;

			MessagingCenter.Send<SettingsPage>(this, "URLChanged");

			Navigation.PopModalAsync(true);

			var azureTag = await Webservices.GetAzureTag(sURL);
			if ((azureTag != "") && (Device.OS != TargetPlatform.Windows))
			{
				ApplicationSettings.AzureTag = azureTag;




				RegisterRequest registration = new RegisterRequest();
				//registration.deviceToken = DependencyService.Get<IPushNotifications>().GetDeviceToken();
				registration.platform = Device.OS.ToString();

				registration.settings = new RegistrationSettings();
				registration.settings.info = ApplicationSettings.InfoNotifications;
				registration.settings.alert = ApplicationSettings.AlertNotifications;
				registration.settings.announcement = ApplicationSettings.AnouncementNotifications;


				//If all switches are turned off for notifications, then Unregister
				bool bUnregister = true;

				//Loops through all properties of the RegistrationSettings to see if ALL of them are false
				Type t = typeof(RegistrationSettings);
				foreach (PropertyInfo propertyInfo in t.GetRuntimeProperties())
				{
					bool bProp = Convert.ToBoolean(propertyInfo.GetValue(registration.settings));

					if (bProp == true)
					{
						bUnregister = false;
					}
				}

				if (bUnregister)
				{
                    //if UnregisterPush does NOT return an empty string, display the error message
                    string response = await Webservices.UnregisterPush(ApplicationSettings.InstallationID);
                    if (response != "")
                    {
                        await DisplayAlert("Error", response, "Ok");
                    }
				}
				else
				{
					if (ApplicationSettings.InstallationID == "")
					{
						DependencyService.Get<IPushNotifications>().Register(registration);
					}
					else
					{
						registration.deviceToken = ApplicationSettings.DeviceToken;
						registration.installationId = ApplicationSettings.InstallationID;
						await Webservices.RegisterPush(registration);
					}
				}
			}
			else //azuretag == "" (it's not in the enable string
			{
				Device.BeginInvokeOnMainThread(() =>
					{
						DisplayAlert("Error", "Your Nightscout website does not have azurepush enabled.  Please ensure you have updated your Nightscout website and added the azurepush string to your enable variable.", "Ok");
					});
			}
		}

		void BtnCancel_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync(true);
		}



		


		
	}
}
