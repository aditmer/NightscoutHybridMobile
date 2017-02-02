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

			if (ApplicationSettings.URL != ApplicationSettings.SettingsDefaultURL)
			{
				entURL.Text = ApplicationSettings.URL;
			}
			swInfo.IsToggled = ApplicationSettings.InfoNotifications;
			swAlert.IsToggled = ApplicationSettings.AlertNotifications;
			swAnouncement.IsToggled = ApplicationSettings.AnouncementNotifications;
			swVolume.IsToggled = ApplicationSettings.VolumeSliderVisible;
		}

		async void btnSave_Clicked(object sender, System.EventArgs e)
		{
			if (String.IsNullOrEmpty(entURL.Text))
			{
				MessagingCenter.Send<SettingsPage>(this, "URLChanged");
			}
			else
			{
				string sURL = entURL.Text.ToLower();
				sURL = sURL.Replace("https", "");
				//sURL = sURL.Replace("HTTPS", "");
				sURL = sURL.Replace("http", "");
				//sURL = sURL.Replace("HTTP", "");
				sURL = sURL.Replace("://", "");

				sURL = "https://" + sURL;

				if (sURL != ApplicationSettings.URL)
				{
					MessagingCenter.Send<SettingsPage>(this, "URLChanged");

					if (ApplicationSettings.InstallationID != "")
					{
						await Webservices.UnregisterPush(ApplicationSettings.InstallationID);
					}
				}

				ApplicationSettings.URL = sURL;
				ApplicationSettings.InfoNotifications = swInfo.IsToggled;
				ApplicationSettings.AlertNotifications = swAlert.IsToggled;
				ApplicationSettings.AnouncementNotifications = swAnouncement.IsToggled;
				ApplicationSettings.VolumeSliderVisible = swVolume.IsToggled;

				MessagingCenter.Send<SettingsPage>(this, "VolumeSlider");

				Navigation.PopModalAsync(true);



				await Webservices.GetStatusJson(sURL);
				if ((ApplicationSettings.AzureTag != "") && (Device.OS != TargetPlatform.Windows))
				{
					//ApplicationSettings.AzureTag = azureTag;




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
						//added on 1/3/16 by aditmer to prevent the unsubscribe webservice call when no InstallationID exists.
						if (ApplicationSettings.InstallationID != string.Empty)
						{
							//if UnregisterPush does NOT return an empty string, display the error message
							string response = await Webservices.UnregisterPush(ApplicationSettings.InstallationID);
							if (response != "")
							{
								await DisplayAlert("Error", response, "Ok");
							}

							//added on 1/3/16 by aditmer to remove the InstallationID after unsubscribing. 
							ApplicationSettings.InstallationID = string.Empty;
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
				else //azuretag == "" (it's not in the enable string)
				{
					//if they registered for any type of notification, but don't have an AzureTag
					if (ApplicationSettings.InfoNotifications
						|| ApplicationSettings.AlertNotifications
						|| ApplicationSettings.AnouncementNotifications)
					{
						MessagingCenter.Send(this, "No AzureTag");
						//Device.BeginInvokeOnMainThread(async () =>
						//	{
						//		await DisplayAlert("Error", "Your Nightscout website does not have azurepush enabled.  Please ensure you have updated your Nightscout website and added the azurepush string to your enable variable.", "Ok");
						//	});
					}

				}
			}
	    }

		void BtnCancel_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync(true);
		}



		


		
	}
}
