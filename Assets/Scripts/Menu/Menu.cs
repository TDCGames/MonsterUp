using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour {

	public Image soundButton;
	public Sprite soundOn, soundOff;
	void Start()
	{
		AudioSystem ();
	}
	public void StartGame()
	{
		Application.LoadLevel (1);
	}

	public void AudioSystem()
	{
		if (PlayerPrefs.GetInt ("sound") == 0) {
			AudioManager.instance.source.mute = false;
			soundButton.sprite = soundOn;
		} else if (PlayerPrefs.GetInt ("sound") == 1) {
			AudioManager.instance.source.mute = true;
			soundButton.sprite = soundOff;
		}
	}

	public void SetAudio()
	{
		if (PlayerPrefs.GetInt ("sound") == 0) {
			PlayerPrefs.SetInt ("sound", 1);
			AudioManager.instance.source.mute = true;
			soundButton.sprite = soundOff;
		} else if (PlayerPrefs.GetInt ("sound") == 1) {
			PlayerPrefs.SetInt ("sound", 0);
			AudioManager.instance.source.mute = false;
			soundButton.sprite = soundOn;
		}

		AudioSystem ();
	}
}
