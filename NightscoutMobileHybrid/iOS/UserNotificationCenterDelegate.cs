using System;
using System.Diagnostics;
using Foundation;
using Microsoft.AppCenter.Analytics;
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
        public async override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            //changed on 8/12/18 by aditmer.  Replaces HockeyApp analytics w/ AppCenter
            Analytics.TrackEvent("iOS Notification Ack");
			Debug.WriteLine("Snooze me");
			Console.WriteLine("Snoozed");
            // Take action based on Action ID

			AckRequest ack = new AckRequest();
			ack.time = -1;

			switch (response.ActionIdentifier) 
            {
				
                case "snooze1":
					
					ack.time = ApplicationSettings.AlarmUrgentLowMins1;

					break;
					
				case "snooze2":
					ack.time = ApplicationSettings.AlarmUrgentLowMins2;

					break;

				case "snooze3":
				ack.time = ApplicationSettings.AlarmUrgentMins1;

				break;

				case "snooze4":
				ack.time = ApplicationSettings.AlarmUrgentMins2;

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

			if (ack.time != -1)
			{
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



				await Webservices.SilenceAlarm(ack);
			}

            // Inform caller it has been handled
            completionHandler();
        }
        #endregion
    }
}