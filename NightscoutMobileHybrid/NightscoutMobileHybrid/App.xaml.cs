using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace NightscoutMobileHybrid
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			//var navPage = new NavigationPage(new NightscoutMobileHybridPage());
			//navPage.BarBackgroundColor = Color.Black;
			//navPage.BarTextColor = Color.White;

			MainPage = new NightscoutMobileHybridPage();

		}

		protected override void OnStart()
		{
            // Handle when your app starts

            //added by aditmer on 8/13/18.  Replacing Hockeyapp crash reporting and analytics w/ AppCenter
            AppCenter.Start("ios=39f67042-930a-44c1-81df-b483513212e9;" + "android=9af95349-58b9-4f72-9bf2-fe74575551cf;", typeof(Analytics), typeof(Crashes));
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
