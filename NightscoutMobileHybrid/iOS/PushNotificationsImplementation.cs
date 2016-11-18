using System;
using Foundation;
using NightscoutMobileHybrid.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PushNotificationsImplementation))]
namespace NightscoutMobileHybrid.iOS
{
	public class PushNotificationsImplementation : IPushNotifications
	{
		

		public PushNotificationsImplementation()
		{
		}

		public void Unregister()
		{
			UIApplication.SharedApplication.UnregisterForRemoteNotifications();
		}

		void IPushNotifications.Register()
		{


			//Push notifications registration
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
					   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					   new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();

			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}


		}


	}
}
