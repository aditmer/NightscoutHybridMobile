using System;
using UserNotifications;

namespace MonkeyNotification
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
                    // TODO Call SilenceAlarm web service
                    // Needs to pass the following paramaters:
                    // Level (from notification)
                    // Key (from notification)
                    // Group (from notification)
                    // Time (how long to snooze)
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