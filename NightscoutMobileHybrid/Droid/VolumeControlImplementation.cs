using System;
using Android.Content;
using Android.Media;
using NightscoutMobileHybrid.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(VolumeControlImplementation))]
namespace NightscoutMobileHybrid.Droid
{
	public class VolumeControlImplementation : Java.Lang.Object, IVolumeControl
	{
		public VolumeControlImplementation()
		{
		}

		public double GetVolume()
		{
			AudioManager audioMan = (AudioManager)global::Android.App.Application.Context.GetSystemService(Context.AudioService);
			return audioMan.GetStreamVolume(Android.Media.Stream.Music);
		}

		public void SetVolume(float Volume)
		{
			AudioManager audioMan = (AudioManager)global::Android.App.Application.Context.GetSystemService(Context.AudioService);
			audioMan.SetStreamVolume(Android.Media.Stream.Music, Convert.ToInt32(Volume), 0);
		}

		public double GetMaxVolume()
		{
			AudioManager audioMan = (AudioManager)global::Android.App.Application.Context.GetSystemService(Context.AudioService);
			return audioMan.GetStreamMaxVolume(Android.Media.Stream.Music);
		}
	}
}
