using System;
using Android.Views;
using NightscoutMobileHybrid.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ScreenLockImplementation))]
namespace NightscoutMobileHybrid.Droid
{
	public class ScreenLockImplementation : Java.Lang.Object, IScreenLock
	{
		public ScreenLockImplementation()
		{
		}

		public void Lock()
		{
			var ctx = Forms.Context; // useful for many Android SDK features
			//TODO implement screen lock on Android
			//reference:  https://forums.xamarin.com/discussion/38489/preventing-sleep-mode-keeping-the-app-alive
			//this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
		}

		public void Unlock()
		{
			throw new NotImplementedException();
		}
	}
}
