using System;
namespace NightscoutMobileHybrid.iOS
{
	public class Constants
	{
		// Azure app-specific connection string and hub path
		public const string ConnectionString = "Endpoint=sb://nspush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=zAp47KnaK0SguPp+gCp0dgY9RVmqm2wI/51lBg4Nw8w=";
		public const string NotificationHubPath = "Push-Notification-Hub";

		public Constants()
		{
		}
	}
}
