using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;

	public AudioSource source;
	// Use this for initialization
	public AudioClip bgClip;
	public AudioClip[] effectClip;
	void Awake()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (gameObject);
		}
	}


	void Start()
	{
		source.clip = bgClip;
		source.Play ();
		source.loop = true;
	}

}
