using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Analytics;


public class FireBase : MonoBehaviour {

	public static FireBase _instance;

	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () {

		dependencyStatus = FirebaseApp.CheckDependencies();
		if (dependencyStatus != DependencyStatus.Available) {
			FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
				dependencyStatus = FirebaseApp.CheckDependencies();
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase();
				} else {
					Debug.LogError(
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Handle initialization of the necessary firebase modules:
	void InitializeFirebase() {
		//DebugLog("Enabling data collection.");
		FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

		//DebugLog("Set user properties.");
		// Set the user's sign up method.
		FirebaseAnalytics.SetUserProperty(
			FirebaseAnalytics.UserPropertySignUpMethod,
			"Google");
		// Set the user ID.
		FirebaseAnalytics.SetUserId("uber_user_510");
	}

	public void LogHome()
	{
		FirebaseAnalytics.LogEvent ("Home");
	}

	public void LogLevel(int _level)
	{
		FirebaseAnalytics.LogEvent ("Level_" + _level.ToString());
	}
	public void LogEndGame()
	{
		FirebaseAnalytics.LogEvent ("End Game");
	}
	public void LogRewardVideo()
	{
		FirebaseAnalytics.LogEvent ("Show reward video");
	}



}
