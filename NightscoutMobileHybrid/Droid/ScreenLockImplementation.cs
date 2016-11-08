using System;
using Android.OS;
using Android.Views;
using NightscoutMobileHybrid.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ScreenLockImplementation))]
namespace NightscoutMobileHybrid.Droid
{
	public class ScreenLockImplementation : Java.Lang.Object, IScreenLock
	{
		PowerManager _pm;
		PowerManager.WakeLock _wl;
		public ScreenLockImplementation()
		{
			var ctx = Forms.Context; // useful for many Android SDK features
			_pm = (PowerManager)ctx.GetSystemService(Android.Content.Context.PowerService);
			_wl = _pm.NewWakeLock(WakeLockFlags.Full, "Stay on");
		}

		public void Lock()
		{
			_wl.Acquire();

		}

		public void Unlock()
		{
			_wl.Release();
		}
	}
}
