using System;
using System.Threading.Tasks;

namespace NightscoutMobileHybrid
{
	public class CheckInstallationID
	{
		public CheckInstallationID()
		{
		}

		public async static Task CheckNewInstallationID(string InstallationID)
		{
			

			if (ApplicationSettings.InstallationID != InstallationID && ApplicationSettings.InstallationID != "")
			{
				await Webservices.UnregisterPush(ApplicationSettings.InstallationID);

			}


		}
	}
}
