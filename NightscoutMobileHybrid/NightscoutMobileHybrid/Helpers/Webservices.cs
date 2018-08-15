using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using Microsoft.AppCenter.Crashes;

namespace NightscoutMobileHybrid
{
    public class Webservices
    {
        public static async Task SilenceAlarm(AckRequest ack)
        {
            var httpClient = new HttpClient();

            
            string resourceAddress = ApplicationSettings.URL + "/api/v1/notifications/azure/ack";


            
			//AckRequest ack = new AckRequest { Level = level, Key = key, Group = group, TimeInMinutes = timeInMinutes }

            string postBody = JsonConvert.SerializeObject(ack);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
                //var content = await httpResponse.Content.ReadAsStringAsync();
                //RegisterResponse response = JsonConvert.DeserializeObject<RegisterResponse>(content);

                //ApplicationSettings.InstallationID = response.installationId;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                MessagingCenter.Send<Exception, string>(ex, "Snooze Error", ex.Message);

            }



        }

        public static async Task GetStatusJson(string URL)
        {
			try
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


					content = await response.Content.ReadAsStringAsync();

					site = JsonConvert.DeserializeObject<RootObject>(content);

					if (String.IsNullOrEmpty(site.settings.azureTag))
					{

						ApplicationSettings.AzureTag = "";
					}
					else
					{
						ApplicationSettings.AzureTag = site.settings.azureTag;
					}

					if (site.settings.alarmUrgentHighMins != null)
					{
						ApplicationSettings.AlarmUrgentMins1 = site.settings.alarmUrgentHighMins[0];
						ApplicationSettings.AlarmUrgentMins2 = site.settings.alarmUrgentHighMins[1];
						ApplicationSettings.AlarmUrgentLowMins1 = site.settings.alarmUrgentLowMins[0];
						ApplicationSettings.AlarmUrgentLowMins2 = site.settings.alarmUrgentLowMins[1];
					}
				}
				else
				{
					ApplicationSettings.AzureTag = "";
				}


			}
			catch (Exception ex)
			{

                Crashes.TrackError(ex);
			}
            

        }

        public async static Task RegisterPush(RegisterRequest request)
        {
            var httpClient = new HttpClient();

            string resourceAddress = ApplicationSettings.URL + "/api/v1/notifications/azure/register";

			request.sharedAccessKey = GetAccessKey();

            string postBody = JsonConvert.SerializeObject(request);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                
                HttpResponseMessage httpResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
                var content = await httpResponse.Content.ReadAsStringAsync();
                RegisterResponse response = JsonConvert.DeserializeObject<RegisterResponse>(content);

                ApplicationSettings.InstallationID = response.installationId;
            }
            catch (Exception ex)
            {
                //HockeyApp.MetricsManager.TrackEvent(ex.Message);
                MessagingCenter.Send<Exception, string>(ex, "Register Error", ex.Message);
            }


        }

        public async static Task<String> UnregisterPush(string InstallationID)
        {
            var httpClient = new HttpClient();

            try
            {
                string resourceAddress = ApplicationSettings.URL + "/api/v1/notifications/azure/unregister";


                var installationID = new RegisterResponse { installationId = InstallationID };

                string postBody = JsonConvert.SerializeObject(installationID);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage httpResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));

                return ""; //returns an empty string when no errors occur
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                //returns an error message to show the user if it occurs
                return $"There was an error unregistering for push notifications: {ex.Message}.  This has already been reported to the developers.";
                
            }
            //var content = await httpResponse.Content.ReadAsStringAsync();
            //RegisterResponse response = JsonConvert.DeserializeObject<RegisterResponse>(content);

        }

		private static string GetAccessKey()
		{
			var assembly = typeof(Webservices).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("NightscoutMobileHybrid.Helpers.ServerSettings.config");
			string text = "";
			using (var reader = new System.IO.StreamReader(stream))
			{
				text = reader.ReadToEnd();
			}


			return text;
		}
    }
}
