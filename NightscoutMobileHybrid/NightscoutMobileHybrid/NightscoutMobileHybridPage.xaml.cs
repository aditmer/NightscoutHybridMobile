using Xamarin.Forms;

namespace NightscoutMobileHybrid
{
	public partial class NightscoutMobileHybridPage : ContentPage
	{
		public NightscoutMobileHybridPage()
		{
			InitializeComponent();

			if (Settings.URL != string.Empty)
			{
				wvNightscout.Source = Settings.URL;
			}
			else
			{
				Navigation.PushModalAsync(new SettingsPage(), true);
			}
			slVolume.Value = DependencyService.Get<IVolumeControl>().GetVolume();

			MessagingCenter.Subscribe<SettingsPage>(this, "URLChanged", (SettingsPage obj) =>
			{
				wvNightscout.Source = Settings.URL;
			});

			//btnChangeURL.Clicked += BtnChangeURL_Clicked;
			//slVolume.ValueChanged += SlVolume_ValueChanged;
			//btnRefresh.Clicked += BtnRefresh_Clicked;
			//swScreenLockOverride.Toggled += SwScreenLockOverride_Toggled;
		}

		void SlVolume_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			DependencyService.Get<IVolumeControl>().SetVolume((float) slVolume.Value);
		}

		void BtnRefresh_Clicked(object sender, System.EventArgs e)
		{
			wvNightscout.Source = Settings.URL;
		}

		void BtnChangeURL_Clicked(object sender, System.EventArgs e)
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
	}
}
