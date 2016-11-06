using System;
using AVFoundation;
using MediaPlayer;
using NightscoutMobileHybrid.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(VolumeControlImplementation))]
 
namespace NightscoutMobileHybrid.iOS
{
	public class VolumeControlImplementation : IVolumeControl
	{
		MPMusicPlayerController applicationMusicPlayer = new MPMusicPlayerController();
		
		public VolumeControlImplementation()
		{
			AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.MixWithOthers);
		}

		public double GetVolume()
		{
			//var player = new AVPlayer();
			//return player.Volume;

			return applicationMusicPlayer.Volume;
		}

		public void SetVolume(float Volume)
		{
			//var player = new AVPlayer();
			//player.Volume = Volume;


			applicationMusicPlayer.Volume = Volume;

			//TODO implement system volume control implementation 
			//likely using https://developer.xamarin.com/api/type/MonoTouch.MediaPlayer.MPMusicPlayerController/
			//reference:  https://github.com/nightscout/ios-monitor/blob/master/Nighscout/SNVolumeSlider.m
		}
	}
}
