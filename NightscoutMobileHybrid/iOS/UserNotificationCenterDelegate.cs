using System;
using System.Diagnostics;
using Foundation;
using HockeyApp.iOS;
using UserNotifications;

namespace NightscoutMobileHybrid.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        #region Constructors
        public UserNotificationCenterDelegate()
        {
        }
        #endregion

        #region Override Methods
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.MetricsManager.TrackEvent("iOS Notification Ack");
			Debug.WriteLine("Snooze me");
            // Take action based on Action ID
			switch (response.ActionIdentifier) 
            {
                case "snooze":
					
					AckRequest ack = new AckRequest();

					var userInfo = response.Notification.Request.Content.UserInfo;

					if (userInfo.ContainsKey(new NSString("level")))
					{
						ack.level = userInfo.ValueForKey(new NSString("level")) as NSString;
						//ack.Level = level.Int32Value;
					}

					if (userInfo.ContainsKey(new NSString("group")))
					{
						ack.group = (userInfo.ValueForKey(new NSString("group")) as NSString).ToString();
					}

					if (userInfo.ContainsKey(new NSString("key")))
					{
						ack.key = (userInfo.ValueForKey(new NSString("key")) as NSString).ToString();
					}

					ack.time = 15;

					Webservices.SilenceAlarm(ack);
                    break;
               // default:
                    // Take action based on identifier
                    //switch (response.ActionIdentifier)
                    //{
                    //    case UNActionIdentifier.Default:
                    //// Handle default
                    //...
                    //break;
                    //    case UNActionIdentifier.Dismiss:
                    // Handle dismiss
                    //...
                    //break;
                    //}
                    //break;
            }

            // Inform caller it has been handled
            completionHandler();
        }
        #endregion
    }
}