using System;
using NightscoutMobileHybrid.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ScreenLockImplementation))]
namespace NightscoutMobileHybrid.iOS
{
	public class ScreenLockImplementation : IScreenLock
	{
		public ScreenLockImplementation()
		{
		}

		public void Lock()
		{
			UIApplication.SharedApplication.IdleTimerDisabled = true;
		}

		public void Unlock()
		{
			UIApplication.SharedApplication.IdleTimerDisabled = false;
		}
	}
}
