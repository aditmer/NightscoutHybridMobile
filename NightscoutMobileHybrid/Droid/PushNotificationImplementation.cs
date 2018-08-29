using Gcm.Client;
using Xamarin.Forms;
using NightscoutMobileHybrid.Droid;
using System;
using Android.App;
using Firebase.Iid;
using Android.Util;

[assembly: Xamarin.Forms.Dependency(typeof(PushNotificationImplementation))]
namespace NightscoutMobileHybrid.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class PushNotificationImplementation : FirebaseInstanceIdService, IPushNotifications
	{
		public static RegisterRequest registerRequest;
        const string TAG = "MyFirebaseIIDService";

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            SendRegistrationToServer(refreshedToken);
        }
        async void SendRegistrationToServer(string token)
        {
            if (registerRequest != null)
            {
                //registerRequest.deviceToken = token;
                await Webservices.RegisterPush(registerRequest);
            }
        }

        //public PushNotificationImplementation()
		//{
		//}

		public void Register(RegisterRequest request)
		{
            var ctx = BaseContext;
              registerRequest = request;
            OnTokenRefresh();

            
        }

        public void Unregister()
		{

		}


	}
}
