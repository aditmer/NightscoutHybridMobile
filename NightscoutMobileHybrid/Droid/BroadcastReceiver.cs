using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;
using WindowsAzure.Messaging;
using NightscoutMobileHybrid.Droid;
using Xamarin.Forms;
using Android.Media;
using NightscoutMobileHybrid;
using Newtonsoft.Json;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is needed only for Android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

[BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
[Service(Exported = false), IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" })]
public class BroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
{
    public static string[] SENDER_IDS = new string[] { NightscoutMobileHybrid.Constants.SenderID };

    
}

[Service] // Must use the service tag
public class PushHandlerService : GcmServiceBase
{
    public static string RegistrationID { get; private set; }
    private NotificationHub Hub { get; set; }

    public PushHandlerService() : base(BroadcastReceiver.SENDER_IDS)
    {

    }

    protected override void OnMessage(Context context, Intent intent)
    {
        var msg = new StringBuilder();

        if (intent != null && intent.Extras != null)
        {
            foreach (var thisKey in intent.Extras.KeySet())
                msg.AppendLine(thisKey + "=" + intent.Extras.Get(thisKey).ToString());
        }

        //Get the data from the ANH template
        string message = intent.Extras.GetString("message");
        string title = intent.Extras.GetString("title", "Nightscout");
        string key = intent.Extras.GetString("key", "0");   //Key is used as the tag, 0 is the default for unregister and errors
        //TODO: Do something with the rest of the payload
        //string eventName = intent.Extras.GetString("eventName");
        string group = intent.Extras.GetString("group");
        string level = intent.Extras.GetString("level");
        string sound = intent.Extras.GetString("sound");
        int soundResourceId = 0;
        if (sound == "alarm.mp3")
        {
            soundResourceId = NightscoutMobileHybrid.Droid.Resource.Raw.alarm;
        }
        else if (sound == "alarm2.mp3")
        {
            soundResourceId = NightscoutMobileHybrid.Droid.Resource.Raw.alarm2;
        }

        if (!string.IsNullOrEmpty(message))
        {
            createNotification(key, title, message, soundResourceId, group, level);
        }
        else
        {
            Log.Error(ApplicationSettings.AzureTag, "Unknown message details: " + msg.ToString());
            createNotification("0", "Unknown message details", msg.ToString(), soundResourceId,"0","0");
        }
    }

    protected override void OnError(Context context, string errorId)
    {
        Log.Error(ApplicationSettings.AzureTag, "GCM Error: " + errorId);
    }

    protected override void OnRegistered(Context context, string registrationId)
    {
		PushNotificationImplementation.registerRequest.deviceToken = registrationId;
		ApplicationSettings.DeviceToken = registrationId;
		Webservices.RegisterPush(PushNotificationImplementation.registerRequest);
		//commented out on 11/29/16 by aed so we can register on the server
        //RegistrationID = registrationId;

        //Hub = new NotificationHub(NightscoutMobileHybrid.Constants.NotificationHubPath, NightscoutMobileHybrid.Constants.ConnectionString, context);

        //try
        //{
        //    Hub.UnregisterAll(registrationId);
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(ApplicationSettings.AzureTag, ex.Message);
        //}

        
        //var tags = new List<string>() { ApplicationSettings.AzureTag };

        //try
        //{
        //    const string templateBodyGCM = "{\"data\":{\"message\":\"$(message)\",\"eventName\":\"$(eventName)\",\"group\":\"$(group)\",\"key\":\"$(key)\",\"level\":\"$(level)\",\"sound\":\"$(sound)\",\"title\":\"$(title)\"}}";

        //    var hubRegistration = Hub.RegisterTemplate(registrationId, "nightscout", templateBodyGCM, tags.ToArray());
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(ApplicationSettings.AzureTag, ex.Message);
        //}
    }

    protected override void OnUnRegistered(Context context, string registrationId)
    {
        createNotification("0", "GCM Unregistered...", "The device has been unregistered!", 0,"0","0");
    }

    void createNotification(string key, string title, string desc, int sound, string group, string level)
    {
        var intent = new Intent(this, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.SingleTop);
        var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

		//addd on 12/5/16 by aed to add actions to the push notifications

		AckRequest ack = new AckRequest();
		ack.group = group;
		ack.key = key;
		ack.level = level;
		ack.time = ApplicationSettings.AlarmUrgentMins1;
		var notificationIntent = new Intent(this, typeof(NotificationActionService));
		notificationIntent.PutExtra("ack", JsonConvert.SerializeObject(ack));
		var snoozeIntent1 = PendingIntent.GetService(this, 0, notificationIntent, PendingIntentFlags.OneShot);

		//adds 2nd action that snoozes the alarm for the ApplicationSettings.AlarmUrgentMins[1] amount of time
		var notificationIntent2 = new Intent(this, typeof(NotificationActionService));
		AckRequest ack2 = new AckRequest();
		ack2.group = group;
		ack2.key = key;
		ack2.level = level;
		ack2.time = ApplicationSettings.AlarmUrgentMins2;
		notificationIntent2.PutExtra("ack", JsonConvert.SerializeObject(ack2));
		var snoozeIntent2 = PendingIntent.GetService(this, 0, notificationIntent2, PendingIntentFlags.OneShot);

		//Notification.Action notificationAction = new Notification.Action(0, "Snooze", snoozeIntent);


        var notificationBuilder = new Notification.Builder(this)
            .SetSmallIcon(NightscoutMobileHybrid.Droid.Resource.Drawable.icon)
            .SetContentTitle(title)
            .SetContentText(desc)
            .SetAutoCancel(true)
            .SetPriority((int)NotificationPriority.Max)
            .SetContentIntent(pendingIntent)
		 .AddAction(0, $"Snooze {ack.time} min", snoozeIntent1)
          .AddAction(0, $"Snooze {ack2.time} min", snoozeIntent2);

        if (sound == 0)
        {
            notificationBuilder.SetDefaults(NotificationDefaults.Vibrate | NotificationDefaults.Lights | NotificationDefaults.Sound);
        }
        else
        {
            notificationBuilder.SetDefaults(NotificationDefaults.Vibrate | NotificationDefaults.Lights);
            notificationBuilder.SetSound(Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + PackageName + "/Raw/" + sound));
        }

        var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
        var notification = notificationBuilder.Build();
        if (sound != 0)
        {
            notification.Flags = NotificationFlags.Insistent;
        }
        notificationManager.Notify(key, 1, notification);
        
        //Not using in-app notifications (Nightscout handles all of that for us)
        //dialogNotify(title, desc);
    }

    //Not using in-app notifications
    //protected void dialogNotify(String title, String message)
    //{
    //    var ctx = Forms.Context;
    //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
    //        AlertDialog.Builder dlg = new AlertDialog.Builder(ctx);
    //        AlertDialog alert = dlg.Create();
    //        alert.SetTitle(title);
    //        alert.SetButton("Ok", delegate {
    //            alert.Dismiss();
    //        });
    //        alert.SetMessage(message);
    //        alert.Show();
    //    });
    //}
}