using System;
using Lamp.Plugin;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace NightscoutMobileHybrid
{
	public partial class NightscoutMobileHybridPage : ContentPage
	{
		private double _width = 0;
		private double _height = 0;

		public NightscoutMobileHybridPage()
		{
			InitializeComponent();



			if (ApplicationSettings.URL != string.Empty)
			{
				TryURL(7000);
			}
			else
			{
				Navigation.PushModalAsync(new SettingsPage(), true);
			}
			slVolume.Value = DependencyService.Get<IVolumeControl>().GetVolume();

			MessagingCenter.Subscribe<SettingsPage>(this, "URLChanged", (SettingsPage obj) =>
			{
				TryURL(7000);
			});

			MessagingCenter.Subscribe<Exception,string>(this, "Register Error", (page,errorMessage) =>
			 {
				DisplayAlert("Error", $"There was an error registering for push notifications: {errorMessage}.  This has already been reported to the developers.", "Ok");
			 });

			slVolume.Maximum = DependencyService.Get<IVolumeControl>().GetMaxVolume();

			//btnChangeURL.Clicked += BtnChangeURL_Clicked;
			//slVolume.ValueChanged += SlVolume_ValueChanged;
			//btnRefresh.Clicked += BtnRefresh_Clicked;
			//swScreenLockOverride.Toggled += SwScreenLockOverride_Toggled;
		}

		private async void TryURL(int timeoutInMilliseconds = 5000)
		{
			if (await CrossConnectivity.Current.IsRemoteReachable(ApplicationSettings.URL, 80, timeoutInMilliseconds))
			{
				wvNightscout.Source = ApplicationSettings.URL;
			}
			else
			{
				var htmlSource = new HtmlWebViewSource();
				htmlSource.Html = $"<html><body style=\"background-color:#000; color:fff\"><div ><p>We could not reach the URL {ApplicationSettings.URL}. &nbsp;Please check your URL for typos and make sure you are online.</p>\n<p>&nbsp;</p>\n<p>This is not the Nightscout you are looking for.</p></div></body></html>";
				wvNightscout.Source = htmlSource;
			}

		}
		//Autohide native controls when in landscape orientation
		//Also force a refresh to try to eleminate some rendering issues - added on 12/21/16
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height); // Important!

			if (width != _width || height != _height)
			{
				_width = width;
				_height = height;

				grdNativeControls.IsVisible = (width < height);

				TryURL(7000);
			}
		}

		//private void ShowExtraButtons(bool visible)
		//{
		//	foreach (View child in grdNativeControls.Children)
		//	{
		//		//if (child is Button && (int)child.GetValue(Grid.ColumnProperty) < 2)
		//		//{
		//			child.IsVisible = visible;
		//		//}
		//	}
		//}

		void SlVolume_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			DependencyService.Get<IVolumeControl>().SetVolume((float) slVolume.Value);
		}

		void BtnRefresh_Clicked(object sender, System.EventArgs e)
		{
			wvNightscout.Source = ApplicationSettings.URL;
		}

		void BtnSettings_Clicked(object sender, System.EventArgs e)
		{
			Navigation.PushModalAsync(new SettingsPage(), true);
		}

		void SwScreenLockOverride_Toggled(object sender, ToggledEventArgs e)
		{
			if (e.Value)
			{
				DependencyService.Get<IScreenLock>().Lock();
			}
			else
			{
				DependencyService.Get<IScreenLock>().Unlock();
			}
		}

		void SwLight_Toggled(object sender, ToggledEventArgs e)
		{
			try
			{
				if (e.Value)
				{
					CrossLamp.Current.TurnOn();
				}
				else
				{
					CrossLamp.Current.TurnOff();
				}
			}
			catch (Exception ex)
			{
				HockeyApp.MetricsManager.TrackEvent($"Light issue: {ex.Message}");
			}
		}
	}
}
