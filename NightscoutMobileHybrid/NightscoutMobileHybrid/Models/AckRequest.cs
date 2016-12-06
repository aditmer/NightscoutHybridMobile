using System;
namespace NightscoutMobileHybrid
{
	public class AckRequest
	{
		public int Level
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public string Group
		{
			get;
			set;
		}

		public int TimeInMinutes
		{
			get;
			set;
		}


		public AckRequest()
		{
		}
	}
}
