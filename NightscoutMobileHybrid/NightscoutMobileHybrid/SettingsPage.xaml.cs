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

			var azureTag = await GetAzureTag(sURL);
			if (azureTag != "")
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
					await UnregisterPush(ApplicationSettings.InstallationID);
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
						await RegisterPush(registration);
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



		public static async Task<string> GetAzureTag(string URL)
		{
			
				RootObject site = new RootObject();

				var client = new HttpClient();
				client.MaxResponseContentBufferSize = 256000;

				string sRestUrl = URL + "/api/v1/status.json";  // $"https://{sNSWebsite}/api/v1/entries/sgv.json?[count]=20";
				var uri = new Uri(string.Format(sRestUrl, string.Empty));

				var response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					var content = "";

					try
					{
						content = await response.Content.ReadAsStringAsync();
					}
					catch (Exception ex)
					{
						
						HockeyApp.MetricsManager.TrackEvent(ex.Message);
					}
					site = JsonConvert.DeserializeObject<RootObject>(content);

				if (!site.settings.enable.Contains("azurepush"))
				{
					
						site.settings.azureTag = "";
					}


				}
				else
				{
					site.settings.azureTag = "";
				}

			

			return site.settings.azureTag;
		}

		public async static Task RegisterPush(RegisterRequest request)
		{
			var httpClient = new HttpClient();

			string resourceAddress = ApplicationSettings.URL + "/api/v1/notifications/azure/register";

			 

			string postBody = JsonConvert.SerializeObject(request);

			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			try
			{
				HttpResponseMessage httpResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
				var content = await httpResponse.Content.ReadAsStringAsync();
				RegisterResponse response = JsonConvert.DeserializeObject<RegisterResponse>(content);

				ApplicationSettings.InstallationID = response.installationId;
			}
			catch(Exception ex)
			{
				HockeyApp.MetricsManager.TrackEvent(ex.Message);
				MessagingCenter.Send<Exception, string>(ex, "Register Error",ex.Message);
			}


		}

		
		async Task UnregisterPush(string InstallationID)
		{
			var httpClient = new HttpClient();

			try
			{
				string resourceAddress = ApplicationSettings.URL + "/api/v1/notifications/azure/unregister";


				var installationID = new RegisterResponse { installationId = InstallationID };

				string postBody = JsonConvert.SerializeObject(installationID);

				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage httpResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
			}
			catch (Exception ex)
			{
				HockeyApp.MetricsManager.TrackEvent(ex.Message);
				Device.BeginInvokeOnMainThread(() =>
				{
					DisplayAlert("Error", $"There was an error unregistering for push notifications: {ex.Message}.  This has already been reported to the developers.", "Ok");
				});
			}
			//var content = await httpResponse.Content.ReadAsStringAsync();
			//RegisterResponse response = JsonConvert.DeserializeObject<RegisterResponse>(content);

		}
	}
}
