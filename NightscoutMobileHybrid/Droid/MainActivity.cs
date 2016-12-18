
using Android.App;
using Android.Content.PM;
using Android.OS;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace NightscoutMobileHybrid.Droid
{
	// , qScreenOrientation = ScreenOrientation.Portrait) uncomment move this to the end of the line below to disable landscape orientation
	[Activity(Label = "Nightscout", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
			{
				Window.SetStatusBarColor(Color.FromHex("#000000").ToAndroid());
			}
			//HockeyApp
			CrashManager.Register(this);
			MetricsManager.Register(Application);
			//MetricsManager.EnableUserMetrics();
			CheckForUpdates();


			LoadApplication(new App());
		}

		void CheckForUpdates()
		{
			// Remove this for store builds!
			UpdateManager.Register(this);
		}

		void UnregisterManagers()
		{
			UpdateManager.Unregister();
		}

		protected override void OnPause()
		{
			base.OnPause();

			UnregisterManagers();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			UnregisterManagers();
		}
	}
}
