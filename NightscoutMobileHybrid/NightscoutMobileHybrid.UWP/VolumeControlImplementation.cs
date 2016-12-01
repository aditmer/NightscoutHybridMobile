using NightscoutMobileHybrid.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

[assembly: Xamarin.Forms.Dependency(typeof(VolumeControlImplementation))]
namespace NightscoutMobileHybrid.UWP
{
    class VolumeControlImplementation : IVolumeControl
    {
        MediaPlayer player = new MediaPlayer();
        public double GetMaxVolume()
        {
            return 1;
        }

        public double GetVolume()
        {

            return player.Volume;
        }

        public void SetVolume(float Volume)
        {
            player.Volume = Volume;
        }
    }
}
