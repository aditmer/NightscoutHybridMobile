
using Xamarin.Forms;

namespace NightscoutMobileHybrid
{
	public partial class NightscoutMobileHybridPage : ContentPage
	{
		public NightscoutMobileHybridPage()
		{
			InitializeComponent();

			slVolume.Value = DependencyService.Get<IVolumeControl>().GetVolume();

			//slVolume.ValueChanged += SlVolume_ValueChanged;
		}

		void SlVolume_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			DependencyService.Get<IVolumeControl>().SetVolume((float) slVolume.Value);
		}
	}
}
