using System;
using AVFoundation;
using NightscoutMobileHybrid.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(VolumeControlImplementation))]
 
namespace NightscoutMobileHybrid.iOS
{
	public class VolumeControlImplementation : IVolumeControl
	{
		
		public VolumeControlImplementation()
		{
			AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.MixWithOthers);
		}

		public double GetVolume()
		{
			var player = new AVPlayer();
			return player.Volume;
		}

		public void SetVolume(float Volume)
		{
			var player = new AVPlayer();
			player.Volume = Volume;

		}
	}
}
