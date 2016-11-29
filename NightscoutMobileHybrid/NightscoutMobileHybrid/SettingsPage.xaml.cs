using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
			string sURL = entURL.Text;
			sURL = sURL.Replace("https", "");
			sURL = sURL.Replace("HTTPS", "");
			sURL = sURL.Replace("http", "");
			sURL = sURL.Replace("HTTP", "");
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


				DependencyService.Get<IPushNotifications>().Register();

				RegisterRequest registration = new RegisterRequest();
				registration.deviceToken = DependencyService.Get<IPushNotifications>().GetDeviceToken();
				registration.platform = Device.OS.ToString();

				registration.settings = new RegistrationSettings();
				registration.settings.info = ApplicationSettings.InfoNotifications;
				registration.settings.alert = ApplicationSettings.AlertNotifications;
				registration.settings.announcement = ApplicationSettings.AnouncementNotifications;


				await RegisterPush(registration);
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
				var content = await response.Content.ReadAsStringAsync();
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

				string resourceAddress = "http://71.87.114.90/api/v1/notifications/azure/register";

				 

				string postBody = JsonConvert.SerializeObject(request);

				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage httpResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));

				var content = await httpResponse.Content.ReadAsStringAsync();
				RegisterResponse response = JsonConvert.DeserializeObject<RegisterResponse>(content);

			ApplicationSettings.RegistrationID = response.registrationId;

		}

		
	}
}
