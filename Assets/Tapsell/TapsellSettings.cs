using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;

namespace TapsellSDK.Editor {
	public class TapsellSettings{
		private static string pluginVersion = "3.0.34";

		public static string getPluginVersion()
		{
			return pluginVersion;
		}

		public static void setLaTapsellTestV3PluginVersion(string newVersion)
		{
			PlayerPrefs.SetString ("TapsellLaTapsellTestV3Version", newVersion);
		}

		public static string getLaTapsellTestV3PluginVersion()
		{
			return PlayerPrefs.GetString ("TapsellLaTapsellTestV3Version", null);
		}
	}
}