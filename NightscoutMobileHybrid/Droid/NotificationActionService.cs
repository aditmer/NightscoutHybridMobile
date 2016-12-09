using System;
using Android.App;
using Android.Content;
using Android.OS;
using Newtonsoft.Json;

namespace NightscoutMobileHybrid.Droid
{
	[Service]
	public class NotificationActionService : Android.App.Service
	{
		AckRequest _ack;


		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			string text = intent.GetStringExtra("ack") ?? "no data";

			if (text != "no data")
			{
				_ack = JsonConvert.DeserializeObject<AckRequest>(text);
				Webservices.SilenceAlarm(_ack);
			}

			StopSelf();
			return StartCommandResult.NotSticky;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();


		}

		public override IBinder OnBind(Intent intent)
		{
			// This example isn't of a bound service, so we just return NULL.
			return null;
		}
	}
}
