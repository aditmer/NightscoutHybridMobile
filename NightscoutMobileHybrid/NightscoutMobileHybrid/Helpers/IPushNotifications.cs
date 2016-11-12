using System;
namespace NightscoutMobileHybrid
{
	public interface IPushNotifications
	{
		void Register();

		void Unregister();
	}
}
