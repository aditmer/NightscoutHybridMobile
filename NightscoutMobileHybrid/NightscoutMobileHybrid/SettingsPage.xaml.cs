using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NightscoutMobileHybrid
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();

			entURL.Text = Settings.URL;

		}

		void btnSave_Clicked(object sender, System.EventArgs e)
		{
			string sURL = entURL.Text;
			//sURL = sURL.Replace("https", "");
			//sURL = sURL.Replace("http", "");
			//sURL = sURL.Replace("://", "");

			//sURL = "https://" + sURL;

			Settings.URL = sURL;

			MessagingCenter.Send<SettingsPage>(this, "URLChanged");

			Navigation.PopModalAsync(true);

			DependencyService.Get<IPushNotifications>().Register();
		}

		void BtnCancel_Clicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync(true);
		}
	}
}
