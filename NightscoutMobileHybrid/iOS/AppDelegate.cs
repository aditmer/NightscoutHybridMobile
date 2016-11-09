using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using HockeyApp.iOS;
using UIKit;

namespace NightscoutMobileHybrid.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			LoadApplication(new App());

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("d8783c34856046d9bc081c47708843f6");
			manager.StartManager();

			return base.FinishedLaunching(app, options);
		}
	}
}
