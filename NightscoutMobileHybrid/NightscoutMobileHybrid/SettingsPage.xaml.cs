using System;
using System.Collections.Generic;
using System.Net.Http;
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

			MessagingCenter.Send<SettingsPage>(this, "URLChanged");

			Navigation.PopModalAsync(true);

			var azureTag = await GetAzureTag(sURL);
			if (azureTag != "")
			{
				ApplicationSettings.AzureTag = azureTag;
<<<<<<< HEAD
=======
				#if ENABLE_TEST_CLOUD == false
>>>>>>> b511123c7e480ac844666f5e3772bd43d60eb324
				DependencyService.Get<IPushNotifications>().Register();
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
	}
}
