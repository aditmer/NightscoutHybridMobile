using System;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using Android.OS;
//using Android.Hardware;

using NightscoutMobileHybrid.Droid;
using Camera = Android.Hardware.Camera;

[assembly: Xamarin.Forms.Dependency(typeof(LampImplementation))]
namespace NightscoutMobileHybrid.Droid
{
	public class LampImplementation : Java.Lang.Object, ILamp
	{
		private Camera camera;

		public LampImplementation()
		{
		}


		public void TurnOff()
		{
			if (camera == null)
				camera = Camera.Open();

			if (camera == null)
			{
				Console.WriteLine("Camera failed to initialize");
				return;
			}

			var p = camera.GetParameters();
			var supportedFlashModes = p.SupportedFlashModes;

			if (supportedFlashModes == null)
				supportedFlashModes = new List<string>();

			var flashMode = string.Empty;

			if (supportedFlashModes.Contains(Android.Hardware.Camera.Parameters.FlashModeTorch))
				flashMode = Android.Hardware.Camera.Parameters.FlashModeOff;

			if (!string.IsNullOrEmpty(flashMode))
			{
				p.FlashMode = flashMode;
				camera.SetParameters(p);
			}

			camera.StopPreview();
		}

		public void TurnOn()
		{
			// Additional information about using the camera light here:
			// http://forums.xamarin.com/discussion/24237/camera-led-or-flash
			// http://stackoverflow.com/questions/5503480/use-camera-flashlight-in-android?rq=1

			if (camera == null)
				camera = Android.Hardware.Camera.Open();

			if (camera == null)
			{
				Console.WriteLine("Camera failed to initialize");
				return;
			}

			var p = camera.GetParameters();
			var supportedFlashModes = p.SupportedFlashModes;

			if (supportedFlashModes == null)
				supportedFlashModes = new List<string>();

			var flashMode = string.Empty;

			if (supportedFlashModes.Contains(Android.Hardware.Camera.Parameters.FlashModeTorch))
				flashMode = Android.Hardware.Camera.Parameters.FlashModeTorch;

			if (!string.IsNullOrEmpty(flashMode))
			{
				p.FlashMode = flashMode;
				camera.SetParameters(p);
			}

			// nexus 5 fix here: http://stackoverflow.com/questions/21417332/nexus-5-4-4-2-flashlight-led-not-turning-on
			try
			{
				camera.SetPreviewTexture(new SurfaceTexture(0));
			}
			catch (IOException ex)
			{
				// Ignore
			}

			camera.StartPreview();



		}


	}
}
