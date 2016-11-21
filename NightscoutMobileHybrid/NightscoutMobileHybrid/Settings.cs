using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NightscoutMobileHybrid
{
	public class Thresholds
	{
		public int bgHigh { get; set; }
		public int bgTargetTop { get; set; }
		public int bgTargetBottom { get; set; }
		public int bgLow { get; set; }
	}


	public class Settings
	{
		public string units { get; set; }
		public int timeFormat { get; set; }
		public bool nightMode { get; set; }
		public bool editMode { get; set; }
		public string showRawbg { get; set; }
		public string customTitle { get; set; }
		public string theme { get; set; }
		public bool alarmUrgentHigh { get; set; }
		public List<int> alarmUrgentHighMins { get; set; }
		public bool alarmHigh { get; set; }
		public List<int> alarmHighMins { get; set; }
		public bool alarmLow { get; set; }
		public List<int> alarmLowMins { get; set; }
		public bool alarmUrgentLow { get; set; }
		public List<int> alarmUrgentLowMins { get; set; }
		public List<int> alarmUrgentMins { get; set; }
		public List<int> alarmWarnMins { get; set; }
		public bool alarmTimeagoWarn { get; set; }
		public int alarmTimeagoWarnMins { get; set; }
		public bool alarmTimeagoUrgent { get; set; }
		public int alarmTimeagoUrgentMins { get; set; }
		public string language { get; set; }
		public string scaleY { get; set; }
		public string showPlugins { get; set; }
		public string showForecast { get; set; }
		public int focusHours { get; set; }
		public int heartbeat { get; set; }
		public string baseURL { get; set; }
		public string authDefaultRoles { get; set; }
		public Thresholds thresholds { get; set; }
		public List<string> DEFAULT_FEATURES { get; set; }
		public List<string> alarmTypes { get; set; }
		public List<string> enable { get; set; }
		public string azureTag { get; set; }
	}

	public class ExtendedSettings
	{
	}

	public class RootObject
	{
		public string status { get; set; }
		public string name { get; set; }
		public string version { get; set; }
		public string serverTime { get; set; }
		public long serverTimeEpoch { get; set; }
		public bool apiEnabled { get; set; }
		public bool careportalEnabled { get; set; }
		public bool boluscalcEnabled { get; set; }
		public string head { get; set; }
		public Settings settings { get; set; }
		public ExtendedSettings extendedSettings { get; set; }
		public object authorized { get; set; }
	}
}
