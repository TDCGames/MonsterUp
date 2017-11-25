using System.Collections;
using System.Collections.Generic;
using TapsellSimpleJSON;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace TapsellSDK.Editor {
	[InitializeOnLoad]
	public class TapsellEditor : MonoBehaviour {

		static TapsellEditor ()
		{
			loadRequiredAssets ();
			WWW wwwUpdate = new WWW("http://api.tapsell.ir/v2/sdks/unity/versions/laTapsellTestV3");
			TapsellContinuationManager.StartCoroutine (CheckTapsellUpdate (wwwUpdate), () => wwwUpdate.isDone);
		}

		static IEnumerator CheckTapsellUpdate (WWW wwwUpdate)
		{
			yield return wwwUpdate;
			JSONNode node = JSONNode.Parse (wwwUpdate.text);
			string laTapsellTestV3Version = null;

			if (node != null) {
				string pluginVersion = node ["pluginVersion"].Value;
				//string androidSDKVersion = node ["androidSDKVersion"].Value;
				//string iosSDKVersion = node ["iosSDKVersion"].Value;
				//Debug.Log ("Plugin version: "+pluginVersion);
				TapsellSettings.setLaTapsellTestV3PluginVersion (pluginVersion);
				laTapsellTestV3Version = pluginVersion;
			} else {
				laTapsellTestV3Version = TapsellSettings.getLaTapsellTestV3PluginVersion ();
			}

			if(laTapsellTestV3Version != null)
			{
				bool newVersionAvailable = false;
				bool versionsAreEqual = true;
				string currentVersion = TapsellSettings.getPluginVersion ();
				string[] pluginVersions = currentVersion.Split (new string[]{"."}, System.StringSplitOptions.None);
				string[] laTapsellTestV3Versions = laTapsellTestV3Version.Split (new string[]{"."}, System.StringSplitOptions.None);
				for(int i=0;i<pluginVersions.Length && i<laTapsellTestV3Versions.Length;i++)
				{
					try {
						int newV = int.Parse(laTapsellTestV3Versions[i]);
						int curV = int.Parse(pluginVersions[i]);

						if ( newV > curV )
						{
							newVersionAvailable = true;
						}
						if ( newV != curV )
						{
							versionsAreEqual = false;
						}
					} catch {
						versionsAreEqual = false;
					}
				}
				if(versionsAreEqual && currentVersion.Length<laTapsellTestV3Version.Length)
				{
					newVersionAvailable = true;
				}

				if(newVersionAvailable)
				{
					TapsellUpdateWindow window = (TapsellUpdateWindow)EditorWindow.GetWindow(typeof(TapsellUpdateWindow));
					window.ShowPopup();
				}
			}
		}

		public static Texture2D tapsellLogo;

		public static string WhereIs(string _file)
		{
			string[] assets = { Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar};
			FileInfo[] myFile = new DirectoryInfo ("Assets").GetFiles (_file, SearchOption.AllDirectories);
			string[] temp = myFile [0].ToString ().Split (assets, 2, System.StringSplitOptions.None);
			return "Assets" + Path.DirectorySeparatorChar + temp [1];
		}

		public static void loadRequiredAssets()
		{
			if(tapsellLogo == null)
			{
				tapsellLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(WhereIs("TapsellLogo.png"), typeof(Texture2D));
			}
		}

	}
}
#endif