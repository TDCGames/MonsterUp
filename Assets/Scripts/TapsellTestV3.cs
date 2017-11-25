using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using TapsellSDK;
using TapsellSimpleJSON;
using ArabicSupport;

public class TapsellTestV3 : MonoBehaviour {

	//private string zoneId = "5a1699df45227100012953cb";
	public static bool available = false;
	public static TapsellAd ad = null;
	public static TapsellNativeBannerAd nativeAd = null;

	void Start() {
		
		// Use your tapsell key for initialization
		Tapsell.initialize ("lcbsennldtamphdmbhbesbpchejasddgnkildqpmesmpdmbhdrkbjrcpkmnfjhqmfpbkio");

		Debug.Log("Tapsell Version: "+Tapsell.getVersion());
		Tapsell.setDebugMode (true);
		Tapsell.setPermissionHandlerConfig (Tapsell.PERMISSION_HANDLER_AUTO);

		//Tapsell.setRewardListener (
			//(TapsellAdFinishedResult result) => 
			//{
				// onFinished, you may give rewards to user if result.completed and result.rewarded are both True
				//Debug.Log ("onFinished, adId:"+result.adId+", zoneId:"+result.zoneId+", completed:"+result.completed+", rewarded:"+result.rewarded);

				// You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
				//if(result.completed && result.rewarded)
				//{
					//validateSuggestion(result.adId);
				//}
			//}
		//);

		Tapsell.requestBannerAd ("5a1699df45227100012953cb",BannerType.BANNER_320x50, Gravity.BOTTOM, Gravity.CENTER);
	}

	public void validateSuggestion(string suggestionId)
	{
		try
		{
			string ourPostData = "{\"suggestionId\":\"" + suggestionId +"\"}";
			System.Collections.Generic.Dictionary<string,string> headers = new System.Collections.Generic.Dictionary<string, string>();
			headers.Add("Content-Type", "application/json");

			byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());

			WWW api = new WWW("http://api.tapsell.ir/v2/suggestions/validate-suggestion", pData, headers);
			StartCoroutine(WaitForRequest(api));
		}
		catch (UnityException ex)
		{ 
			Debug.Log(ex.Message); 
		}
		return;
	}

	IEnumerator WaitForRequest(WWW data)
	{
		Debug.Log("my start waiting...");
		yield return data; // Wait until the download is done
		if (data.error != null)
		{
			Debug.Log("my server error is " + data.error);
		}
		else
		{
			Debug.Log("my server result is "+data.text);

			JSONNode node = JSON.Parse (data.text);
			bool valid = node ["valid"].AsBool;
			if (valid) {
				// if suggestion is valid, you can give in game gifts to the user
				Debug.Log ("Ad is valid");
			} 
			else {
				Debug.Log ("Ad is not valid");
			}
		}
	}

	private void requestAd(string zone,bool cached)
	{
		Tapsell.requestAd(zone,cached,
			(TapsellAd result) => {
				// onAdAvailable
				Debug.Log("Action: onAdAvailable");
				TapsellTestV3.available = true;
				TapsellTestV3.ad = result;
			},

			(string zoneId) => {
				// onNoAdAvailable
				Debug.Log("No Ad Available");
			},

			(TapsellError error) => {
				// onError
				Debug.Log(error.error);
			},

			(string zoneId) => {
				// onNoNetwork
				Debug.Log("No Network: "+zoneId);
			},

			(TapsellAd result) => {
				//onExpiring
				Debug.Log("Expiring");
				TapsellTestV3.available=false;
				TapsellTestV3.ad=null;
				requestAd(result.zoneId,true);
			}

		);
	}


	public void ShowAd()
	{

		requestAd ("5a1699df45227100012953cb",true);

		if (TapsellTestV3.available) {

			//if(GUI.Button(new Rect(250, 50, 200, 100), "Show Ad")){
			//}

			TapsellTestV3.available = false;
			TapsellShowOptions options = new TapsellShowOptions ();
			options.backDisabled = true;
			options.immersiveMode = false;
			options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_PORTRAIT;
			options.showDialog = true;
			Tapsell.showAd (ad, options);

		}
			
			//if(GUI.Button(new Rect(50, 50, 200, 100), "Request Video Ad")){
			//}

		


		#if UNITY_ANDROID && !UNITY_EDITOR
		if(TapsellTestV3.nativeAd==null)
		{
		if(GUI.Button(new Rect(50, 150, 200, 100), "Request Banner Ad")){
		requestNativeBannerAd ("59b6903e468465281bde0d25");
		}
		}
		if(TapsellTestV3.nativeAd!=null)
		{
		GUIStyle titleStyle = new GUIStyle ();
		titleStyle.alignment = TextAnchor.UpperRight;
		GUI.Label (new Rect (50, 250, 450, 30), ArabicFixer.Fix(TapsellTestV3.nativeAd.getTitle (),true), titleStyle);

		GUIStyle descriptionStyle = new GUIStyle ();
		descriptionStyle.richText = true;
		descriptionStyle.alignment = TextAnchor.MiddleRight;
		GUI.Label (new Rect (50, 280, 450, 20), ArabicFixer.Fix(TapsellTestV3.nativeAd.getDescription (),true), descriptionStyle);
		GUI.DrawTexture (new Rect(500, 250, 50, 50), TapsellTestV3.nativeAd.getIcon() );
		Rect callToActionRect;
		if(TapsellTestV3.nativeAd.getLandscapeBannerImage()!=null)
		{
		GUI.DrawTexture (new Rect(50, 300, 500, 280), TapsellTestV3.nativeAd.getLandscapeBannerImage() );
		callToActionRect = new Rect(50, 580, 500, 50);
		}
		else if(TapsellTestV3.nativeAd.getPortraitBannerImage()!=null)
		{
		GUI.DrawTexture (new Rect(50, 300, 500, 280), TapsellTestV3.nativeAd.getPortraitBannerImage() );
		callToActionRect = new Rect(50, 580, 500, 50);
		}
		else
		{
		callToActionRect = new Rect(50, 300, 500, 50);
		}
		TapsellTestV3.nativeAd.onShown ();
		if(GUI.Button (callToActionRect, ArabicFixer.Fix(TapsellTestV3.nativeAd.getCallToAction (),true) ))
		{
		TapsellTestV3.nativeAd.onClicked ();
		}
		}
		#endif

	}

	private void requestNativeBannerAd(string zone)
	{
		Tapsell.requestNativeBannerAd(this, zone, 
			(TapsellNativeBannerAd result) => {
				// onAdAvailable
				Debug.Log("Action: onNativeRequestFilled");

				TapsellTestV3.nativeAd = result;

			},

			(string zoneId) => {
				// onNoAdAvailable
				Debug.Log("No Ad Available");
			},

			(TapsellError error) => {
				// onError
				Debug.Log(error.error);
			},

			(string zoneId) => {
				// onNoNetwork
				Debug.Log("No Network: "+zoneId);
			}
		);
	}

}

