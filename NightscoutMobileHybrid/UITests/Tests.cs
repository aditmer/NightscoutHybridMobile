using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace NightscoutMobileHybrid.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);
		}

		//[Test]
		//public void WelcomeTextIsDisplayed()
		//{
		//	app.Repl();

		//	//AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin Forms!"));
		//	//app.Screenshot("Welcome screen.");

		//	//Assert.IsTrue(results.Any());
		//}


		[Test]
		public void CarePortal()
		{
			//Enter URL
			app.Tap(x => x.Class("EntryEditText"));
			//app.Screenshot("Tapped on view EntryEditText");
			app.EnterText(x => x.Class("EntryEditText"), "nstest-server.azurewebsites.net");
			app.Screenshot("Entered 'nstest-server.azurewebsites.net' as the URL");
			app.Tap(x => x.Class("AppCompatButton").Text("Save"));
			app.Screenshot("Tapped on Save button");

			//Toggle Screen Lock Switch
			app.Tap(x => x.Class("SwitchCompat"));
			app.Screenshot("Turned Screenlock on");
			app.Tap(x => x.Class("SwitchCompat"));
			app.Screenshot("Turned Screenlock off");

			//Refresh
			app.Tap(x => x.Class("AppCompatButton").Text("Refresh"));
			app.Screenshot("Tapped Refresh button");

			//Enable Care Portal

			app.Tap(x => x.Class("WebView").Marked("Web View").Css("I.icon-menu").Index(0));
			app.Screenshot("Tapped hamburger menu");
			//app.ScrollDown();
			//app.ScrollDown();
			app.ScrollDownTo(x => x.Class("WebView").Marked("Web View").Css("#plugin-careportal"));
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("#plugin-careportal"));
			app.Screenshot("Checked Enable Careportal checkbox");
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("#save"));

		
			//Enter API Secret
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("I.icon-lock").Index(0));
			app.Screenshot("Tapped on Unlock menu");
			app.EnterText(x => x.Class("WebView").Css("#apisecret"), "SRMAPISECRET");
			app.Screenshot("Entered API Secret");
			app.SwipeRightToLeft();
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("SPAN.ui-button-text").Index(1));
			//app.Screenshot("Tapped on view WebView");

			//Enter BG Treatment
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("I.icon-plus").Index(0));
			app.Screenshot("Tapped on Add Treatment menu");
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("#eventType"));
			app.Screenshot("Tapped on Treatment Type picker");
			app.Tap(x => x.Class("AppCompatTextView").Id("text1").Text("BG Check"));
			app.Screenshot("Selected BG Check");
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("#glucoseValue"));
			//app.Screenshot("Tapped on view WebView");
			app.EnterText(x => x.Class("WebView").Css("#glucoseValue"), "222");
			app.Screenshot("Entered '222' into BG value");
			app.ScrollDown();
			app.EnterText(x => x.Class("WebView").Css("#enteredBy"), "Tester");
			app.Screenshot("Entered 'Tester' into Caregiver");
			app.ScrollDownTo(x => x.Class("WebView").Marked("Web View").Css("BUTTON.translate"));
			app.Tap(x => x.Class("WebView").Marked("Web View").Css("BUTTON.translate"));//.Index(1));
			app.Screenshot("Tapped on Submit");
			app.Tap(x => x.Class("AppCompatButton").Id("button1").Text("OK"));
			app.ScrollUp();
			app.Screenshot("Tapped OK");
		
		}

	}
}
