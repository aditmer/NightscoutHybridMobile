using System;
namespace NightscoutMobileHybrid
{
	public class RegisterRequest
	{
		public string installationId
		{
			get;
			set;
		}

		public string platform
		{
			get;
			set;
		}

		public string deviceToken
		{
			get;
			set;
		}

		public RegistrationSettings settings
		{
			get;
			set;
		}

		public string sharedAccessKey
		{
			get;
			set;
		}

		public RegisterRequest()
		{
		}
	}
}
