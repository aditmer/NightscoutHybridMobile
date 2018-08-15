
using Android.App;
using Android.Content.PM;
using Android.OS;
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
			


			LoadApplication(new App());
		}

		void CheckForUpdates()
		{
			// Remove this for store builds!
			
		}

		void UnregisterManagers()
		{
			
		}

		protected override void OnPause()
		{
			base.OnPause();

			
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			
		}
	}
}
