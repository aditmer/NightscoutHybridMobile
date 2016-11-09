using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;

namespace NightscoutMobileHybrid.Droid
{
	[Activity(Label = "Nightscout", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			//HockeyApp
			CrashManager.Register(this);
			MetricsManager.Register(this, Application);
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
