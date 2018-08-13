using System;
namespace NightscoutMobileHybrid
{
	public class AckRequest
	{
		public string level
		{
			get;
			set;
		}

		public string key
		{
			get;
			set;
		}

		public string group
		{
			get;
			set;
		}

		public int time
		{
			get;
			set;
		}


		public AckRequest()
		{
		}
	}
}
