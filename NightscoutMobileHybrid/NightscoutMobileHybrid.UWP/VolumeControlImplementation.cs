using NightscoutMobileHybrid.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Xaml;

[assembly: Xamarin.Forms.Dependency(typeof(VolumeControlImplementation))]
namespace NightscoutMobileHybrid.UWP
{
    class VolumeControlImplementation : IVolumeControl
    {
        
        public double GetMaxVolume()
        {
            return 1;
        }

        public double GetVolume()
        {

            return BackgroundMediaPlayer.Current.Volume; 
        }

        public void SetVolume(float Volume)
        {
            BackgroundMediaPlayer.Current.Volume = Volume;
        }
    }
}
