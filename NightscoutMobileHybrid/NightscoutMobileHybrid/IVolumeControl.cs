using System;
namespace NightscoutMobileHybrid
{
	public interface IVolumeControl
	{
		void SetVolume(float Volume);

		double GetVolume();
	}
}
