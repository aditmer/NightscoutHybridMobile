using System;
namespace NightscoutMobileHybrid
{
	public class Constants
	{
		// Azure app-specific connection string and hub path
        public const string ConnectionString = "Endpoint=sb://nspush.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=VdtFDvldTypVUgroq6JtrbI162upFh7MeWcpSdJu3yE=";
		public const string NotificationHubPath = "Push-Notification-Hub";

        // GCM app-specific sender information
        public const string SenderID = "531133939294";

		public Constants()
		{
		}
	}
}
