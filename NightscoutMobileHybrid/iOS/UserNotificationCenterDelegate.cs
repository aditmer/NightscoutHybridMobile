using System;
using Foundation;
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
            // Take action based on Action ID
            switch (response.Notification.Request.Identifier)
            {
                case "snooze":
					
					AckRequest ack = new AckRequest();

					var userInfo = response.Notification.Request.Content.UserInfo;

					if (userInfo.ContainsKey(new NSString("level")))
					{
						ack.Level = (userInfo.ObjectForKey(new NSString("level")) as NSNumber).Int32Value;
					}

					if (userInfo.ContainsKey(new NSString("group")))
					{
						ack.Group = (userInfo.ObjectForKey(new NSString("group")) as NSString).ToString();
					}

					if (userInfo.ContainsKey(new NSString("key")))
					{
						ack.Key = (userInfo.ObjectForKey(new NSString("key")) as NSString).ToString();
					}

					ack.TimeInMinutes = 15;

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