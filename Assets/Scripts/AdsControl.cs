using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
#if ADS_PLUGIN
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
#endif

public class AdsControl : MonoBehaviour
{
	
	
	protected AdsControl ()
	{
	}

	private static AdsControl _instance;

	#if ADS_PLUGIN
	ShowOptions options;
	InterstitialAd interstitial;
	#endif

	public string AdmobID_Android, AdmobID_IOS, UnityID_Android, UnityID_IOS, UnityZoneID;

	public static AdsControl Instance { get { return _instance; } }

	void Awake ()
	{
		
		if (FindObjectsOfType (typeof(AdsControl)).Length > 1) {
			Destroy (gameObject);
			return;
		}
		
		_instance = this;
		MakeNewInterstial ();

		
		DontDestroyOnLoad (gameObject); //Already done by CBManager

		#if ADS_PLUGIN

		if (Advertisement.isSupported) { // If the platform is supported,
			#if UNITY_IOS
			Advertisement.Initialize (UnityID_IOS); // initialize Unity Ads.
			#endif

			#if UNITY_ANDROID
			Advertisement.Initialize (UnityID_Android); // initialize Unity Ads.
			#endif
		}
		options = new ShowOptions ();
		options.resultCallback = HandleShowResult;
		#endif

	}


	public void HandleInterstialAdClosed (object sender, EventArgs args)
	{
	
	
		#if ADS_PLUGIN
		if (interstitial != null)
			interstitial.Destroy ();
		MakeNewInterstial ();
		#endif
	
		
	}

	void MakeNewInterstial ()
	{

#if ADS_PLUGIN
	#if UNITY_ANDROID
		interstitial = new InterstitialAd (AdmobID_Android);
	#endif
	#if UNITY_IPHONE
		interstitial = new InterstitialAd (AdmobID_IOS);
	#endif
		interstitial.OnAdClosed += HandleInterstialAdClosed;
		AdRequest request = new AdRequest.Builder ().Build ();
		interstitial.LoadAd (request);

#endif	
	}


	public void showAds ()
	{
	Debug.Log ("Show interstitial Admob");
		#if ADS_PLUGIN
		interstitial.Show ();
		#endif
	}


	public bool GetRewardAvailable ()
	{
		bool avaiable = false;

		return avaiable;
	}

	public void ShowRewardVideo ()
	{
		#if ADS_PLUGIN
		Advertisement.Show (UnityZoneID, options);
		#endif
	}

	public void HideBannerAds ()
	{
	}

	public void ShowBannerAds ()
	{
	}
	#if ADS_PLUGIN
	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
//			FindObjectOfType<UIManager> ().AddMoreStarAmout ();
			break;
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		}
	}
	#endif
}

