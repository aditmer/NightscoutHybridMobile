
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;

namespace NightscoutMobileHybrid.Droid
{
	// , qScreenOrientation = ScreenOrientation.Portrait) uncomment move this to the end of the line below to disable landscape orientation
	[Activity(Label = "Nightscout", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;


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

            if (IsPlayServicesAvailable())
            {
                CreateNotificationChannel();
            }

            LoadApplication(new App());
		}

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  new Java.Lang.String("FCM Notifications"),
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
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
