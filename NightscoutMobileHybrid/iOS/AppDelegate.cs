using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using HockeyApp.iOS;
using UIKit;
using WindowsAzure.Messaging;
using Newtonsoft.Json.Linq;
using UserNotifications;

namespace NightscoutMobileHybrid.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		private SBNotificationHub Hub { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			

			global::Xamarin.Forms.Forms.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			LoadApplication(new App());

			//HockeyApp
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("d8783c34856046d9bc081c47708843f6");
			manager.StartManager();



			// Handling Push notification when app is closed if App was opened by Push Notification...
			if (options != null && options.Keys != null && options.Keys.Count() != 0 && options.ContainsKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")))
			{
				NSDictionary UIApplicationLaunchOptionsRemoteNotificationKey = options.ObjectForKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;

				ProcessNotification(UIApplicationLaunchOptionsRemoteNotificationKey, true);
			}


			return base.FinishedLaunching(app, options);
		}


		public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

			//added on 12/03/16 by aed to add custom actions to the notifications (I think this code goes here)
			// Create action
			var actionID = "snooze";
			var title = "Snooze";
			var action = UNNotificationAction.FromIdentifier(actionID, title, UNNotificationActionOptions.None);


			// Create category
			var categoryID = "event";
			var actions = new UNNotificationAction[] { action };
			var intentIDs = new string[] { };
			var categoryOptions = new UNNotificationCategoryOptions[] { };
			var category = UNNotificationCategory.FromIdentifier(categoryID, actions, intentIDs, UNNotificationCategoryOptions.AllowInCarPlay);

			// Register category
			var categories = new UNNotificationCategory[] { category };
			UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(categories));




			return base.WillFinishLaunching(uiApplication, launchOptions);
		}

		//// And do this, instead of creating a seperate delegate
		//#region IUNUserNotificationCenterDelegate

		//[Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
		//public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		//{

		//	var manager = BITHockeyManager.SharedHockeyManager;
		//	manager.MetricsManager.TrackEvent("iOS Notification Ack");

		//	// Take action based on Action ID
		//	switch (response.ActionIdentifier)
		//	{
		//		case "snooze":

		//			AckRequest ack = new AckRequest();

		//			var userInfo = response.Notification.Request.Content.UserInfo;

		//			if (userInfo.ContainsKey(new NSString("level")))
		//			{
		//				ack.level = userInfo.ValueForKey(new NSString("level")) as NSString;
		//				//ack.Level = level.Int32Value;
		//			}

		//			if (userInfo.ContainsKey(new NSString("group")))
		//			{
		//				ack.group = (userInfo.ValueForKey(new NSString("group")) as NSString).ToString();
		//			}

		//			if (userInfo.ContainsKey(new NSString("key")))
		//			{
		//				ack.key = (userInfo.ValueForKey(new NSString("key")) as NSString).ToString();
		//			}

		//			ack.time = 15;

		//			Webservices.SilenceAlarm(ack);
		//			break;
		//			// default:
		//			// Take action based on identifier
		//			//switch (response.ActionIdentifier)
		//			//{
		//			//    case UNActionIdentifier.Default:
		//			//// Handle default
		//			//...
		//			//break;
		//			//    case UNActionIdentifier.Dismiss:
		//			// Handle dismiss
		//			//...
		//			//break;
		//			//}
		//			//break;
		//	}

		//	// Inform caller it has been handled
		//	completionHandler();
		//}

		//#endregion

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			string s = deviceToken.Description.Replace("<","").Replace(">","").Replace(" ","");
			//Byte[]  tokenBytes = deviceToken.Bytes;
			//for (int i = 0; i < 8;i++)
			//{
				
			//	s += String.Format("%08x%08x%08x%08x%08x%08x%08x%08x", deviceToken.Bytes[i])
			//}
			 
			PushNotificationsImplementation.registerRequest.deviceToken = s;
			ApplicationSettings.DeviceToken = s;
			Webservices.RegisterPush(PushNotificationsImplementation.registerRequest);


            

            //Commented out on 11/29/16 by aed so we can register for notifications on the server
            //Hub = new SBNotificationHub(Constants.ConnectionString, Constants.NotificationHubPath);

            //Hub.UnregisterAllAsync(deviceToken, (error) =>
            //{
            //	if (error != null)
            //	{
            //		Console.WriteLine("Error calling Unregister: {0}", error.ToString());
            //		return;
            //	}
            //});


            //	//adds a tag for the current Nightscout URL in the App Settings
            //	NSSet tags = new NSSet(ApplicationSettings.AzureTag); 

            //	//const string template = "{\"aps\":{\"alert\":\"$(message)\"},\"request\":\"$(requestid)\"}";

            //	const string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(message)\",\"sound\":\"$(sound)\"},\"eventName\":\"$(eventName)\",\"group\":\"$(group)\",\"key\":\"$(key)\",\"level\":\"$(level)\",\"title\":\"$(title)\"}";

            //	//var alert = new JObject(
            //	//new JProperty("aps", new JObject(new JProperty("alert", notificationText))),
            //	//new JProperty("inAppMessage", notificationText))
            //	//.ToString(Newtonsoft.Json.Formatting.None);

            //	//JObject templates = new JObject();
            //	//templates["genericMessage"] = new JObject
            //	//{
            //	//	{"body", templateBodyAPNS}
            //	//};

            //	var expiryDate = DateTime.Now.AddDays(90).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            //Hub.RegisterTemplateAsync(deviceToken,"nightscout",templateBodyAPNS,
            //                   expiryDate,tags,(errorCallback) =>
            //	{
            //		if (errorCallback != null)
            //			Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
            //	});

        }

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			//base.FailedToRegisterForRemoteNotifications(application, error);
			Console.WriteLine("RegisterNativeAsync error: " + error.ToString());
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}

		//public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		//{
		//	CrossAzurePushNotifications.Platform.ProcessNotification(userInfo);
		//}

		//public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		//{
		//	CrossAzurePushNotifications.Platform.RegisteredForRemoteNotifications(deviceToken);
		//}

		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			// Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
			if (null != options && options.ContainsKey(new NSString("aps")))
			{
				//Get the aps dictionary
				NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

				string alert = string.Empty;

				//Extract the alert text
				// NOTE: If you're using the simple alert by just specifying
				// "  aps:{alert:"alert msg here"}  ", this will work fine.
				// But if you're using a complex alert with Localization keys, etc.,
				// your "alert" object from the aps dictionary will be another NSDictionary.
				// Basically the JSON gets dumped right into a NSDictionary,
				// so keep that in mind.
				if (aps.ContainsKey(new NSString("alert")))
					alert = (aps[new NSString("alert")] as NSString).ToString();

				//NSDictionary alertDictionary = aps.ObjectForKey(new NSString("alert")) as NSDictionary;
				//string title = string.Empty;
				//if (alertDictionary.ContainsKey(new NSString("title")))
				//	title = (alertDictionary[new NSString("title")] as NSString).ToString();

				//If this came from the ReceivedRemoteNotification while the app was running,
				// we of course need to manually process things like the sound, badge, and alert.
				if (!fromFinishedLaunching)
				{
					//Manually show an alert
					if (!string.IsNullOrEmpty(alert))
					{
						UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
						avAlert.Show();
					}
				}
			}
		}
	}
}
