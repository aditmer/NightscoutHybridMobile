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
		private bool _bScreenLocked = false;
		private bool _bLightOn = false;
		private bool _bButtonHide = true;

		public NightscoutMobileHybridPage()
		{
			InitializeComponent();

			slVolume.IsVisible = ApplicationSettings.VolumeSliderVisible;

			if (ApplicationSettings.URL != ApplicationSettings.SettingsDefaultURL)// != "" || ApplicationSettings.URL != null)
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

			//added on 1/25/17 by aditmer to allow the volume slider to be turned on/off
			MessagingCenter.Subscribe<SettingsPage>(this, "VolumeSlider", (obj) =>
			{
				slVolume.IsVisible = ApplicationSettings.VolumeSliderVisible;
			});

			MessagingCenter.Subscribe<Exception,string>(this, "Register Error", (page,errorMessage) =>
			 {
				DisplayAlert("Error", $"There was an error registering for push notifications: {errorMessage}.  This has already been reported to the developers.", "Ok");
			 });

			MessagingCenter.Subscribe<SettingsPage>(this, "No AzureTag", (obj) =>
			  {
				  DisplayAlert("No Notifications!", "Your Nightscout website does not have azurepush enabled.  Please ensure you have updated your Nightscout website and added the azurepush string to your enable variable.", "Ok");
			});


			//added on 1/5/16 by aditmer because most tablets don't have a flash and the light button would not work (or cause a crash)
			if ( Device.Idiom == TargetIdiom.Tablet)
			{
				btnLight.IsVisible = false;
			}
			//slVolume.Maximum = DependencyService.Get<IVolumeControl>().GetMaxVolume();

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



				htmlSource.Html = @"<!DOCTYPE html>
					<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">

					<title>This is not the Nightscout you are looking for</title>
					<style type=""text/css"">
					body,td,th {
						font-size: x-large;
						font-family: Constantia, ""Lucida Bright"", ""DejaVu Serif"", Georgia, serif;
						color: #B9B9B9;
					}
					body {
						background-color: #000000;
					}
					</style>
					</head>

					<body><center>
					  <h1><strong>OOPS! </strong></h1>
					  <p>We can't reach that site right now. </p>
					  <p><img src=""error_page_logo.jpg"" width=""100"" height=""138"" alt=""""></p>
					  <p>Please check for typos in your URL and make sure you are online.</p>
					  <p>&nbsp;</p>
					  <p>&nbsp;</p>
					</center>


					</body></html>";
//$"<html><body style=\"background-color:#000; color:fff\"><div ><p>We could not reach the URL {ApplicationSettings.URL}. &nbsp;Please check your URL for typos and make sure you are online.</p>\n<p>&nbsp;</p>\n<p>This is not the Nightscout you are looking for.</p></div></body></html>";
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

				if (ApplicationSettings.URL != ApplicationSettings.SettingsDefaultURL)
				{
					TryURL(7000);
				}
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

		//added again on 1/27/17 by aditmer by request; Added a setting to hide it
		//removed volume control slider on 12/22/16 because you can just use the hardware buttons to change volume
		void SlVolume_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			DependencyService.Get<IVolumeControl>().SetVolume((float) slVolume.Value);
		}

		//added on 12/22/16 by aditmer to hide the buttons - don't think I'm going to use this though.
		void btnHide_Clicked(object sender, System.EventArgs e)
		{
			if (_bButtonHide)
			{
				_bButtonHide = false;
			}
			else
			{
				_bButtonHide = true;
			}
		}

		void BtnRefresh_Clicked(object sender, System.EventArgs e)
		{
			//wvNightscout.Source = ApplicationSettings.URL;
			TryURL();
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

		void btnScreenLockOverride_Clicked(object sender, System.EventArgs e)
		{
			if (_bScreenLocked)
			{
				_bScreenLocked = false;
				DependencyService.Get<IScreenLock>().Unlock();

				btnScreenLockOverride.BackgroundColor = Color.Gray;
				btnScreenLockOverride.Text = "Lock";
			}
			else
			{
				_bScreenLocked = true;
				DependencyService.Get<IScreenLock>().Lock();

				btnScreenLockOverride.BackgroundColor = Color.Green;
				btnScreenLockOverride.Text = "Locked";
			}
		}

		void SwLight_Toggled(object sender, ToggledEventArgs e)
		{
			try
			{
				if (e.Value)
				{
					//CrossLamp.Current.TurnOn();
					DependencyService.Get<ILamp>().TurnOn();
				}
				else
				{
					//CrossLamp.Current.TurnOff();
					DependencyService.Get<ILamp>().TurnOff();
				}
			}
			catch (Exception ex)
			{
				HockeyApp.MetricsManager.TrackEvent($"Light issue: {ex.Message}");
			}
		}

		void btnLight_Clicked(object sender, EventArgs e)
		{
			//try
			//{
			if (_bLightOn)
			{
				_bLightOn = false;

				Device.OnPlatform(() =>
				{
						//iOS
						CrossLamp.Current.TurnOff();
				}, () =>
				 {
						 //Android
						 DependencyService.Get<ILamp>().TurnOff();

				 });

				btnLight.BackgroundColor = Color.Gray;
			}
			else
			{
				_bLightOn = true;

					Device.OnPlatform(() =>
					{
						//iOS
						CrossLamp.Current.TurnOn();
					}, () =>
					 {
						//Android
						DependencyService.Get<ILamp>().TurnOn();
					 });

					btnLight.BackgroundColor = Color.Green;
				}
			//}
			//catch (Exception ex)
			//{
			//	HockeyApp.MetricsManager.TrackEvent($"Light issue: {ex.Message}");
			//}
		}
	}
}
