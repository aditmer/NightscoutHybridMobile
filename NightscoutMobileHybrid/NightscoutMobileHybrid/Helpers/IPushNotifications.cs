using System;
namespace NightscoutMobileHybrid
{
	public interface IPushNotifications
	{
		void Register(RegisterRequest registerRequest);
		void Unregister();


	}
}
