using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public TapsellTapsellTestV3V3 TapsellTapsellTestV3Instance;

	[Header("GenBlock")]
	private bool genFistTime = true;
	public GameObject currentBlock = null;
	private int blockDirection = 0;
	public GameObject nextFistBlock;
	public Vector3 nextFistPos;
	public GameObject fistBlock;
	private GameObject myFistBlock;


	[Header("Game Over")]
	public bool isGameOver;
	public GameObject endGamePanel;
	public Material bloodMat;
	public Texture2D[] bloodTexture;
	public ParticleSystem bloodParticle;
	// Use this for initialization

	[Header("Game Background")]
	public Image background;
	public Sprite[] backGroud;
	public Image land;
	public Sprite[] landSpr;
	public Vector4[] skyColor;

	[Header("UI Text")]
	[HideInInspector]
	public int score;
	private int type;
	public Image[] numberImage;
	public Image[] levelScoreImage;
	public Image[] bestScoreImage;
	public Sprite[] blueSpriteNumber;
	public Sprite[] orangeSpriteNumber;
	public Sprite[] greenSpriteNumber;
	public Sprite[] purpleSpriteNumber;
	public Sprite[] endSpriteNumber;



	[Header("Game parameter")]
	public float blockSpeedMax;
	public float blockSpeedOffset;
	public float blockSpeed;
	public float starPosOffset;

	private	void Awake ()
	{
		instance = this;	
	}

	void Start ()
	{
		AudioSystem ();

		myFistBlock = transform.GetChild (0).transform.gameObject;

		InvokeRepeating ("GenBlock", 0, 2);
		InvokeRepeating ("GenBottom", 15,30);

		type = PlayerController.instance.playerType;

		AddPoint (0);

		InvokeRepeating ("UpgradeParameter", 15, 15);

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isGameOver) {
			isGameOver = false;
			CancelInvoke ();
		}
//------------------------------------------------------------
		type = PlayerController.instance.playerType;
	}
	GameObject tempObj;
	void GenBlock()
	{
		
		if (genFistTime) {
			genFistTime = false;
			currentBlock = transform.GetChild (0).transform.gameObject;
		} else {
			tempObj = currentBlock;
			switch (blockDirection) {
			case 0:
				blockDirection = 1;
				currentBlock = ObjectPooling.instance.GetPoolObject (ObjectPooling.instance.poolStandBlock, new Vector3 (Camera.main.ScreenToWorldPoint (Vector3.zero).x - 0.5f, currentBlock.transform.position.y + 1f, 0));
				currentBlock.GetComponent<StandBlock> ().direction = 1;
				break;
			case 1:
				

				currentBlock = ObjectPooling.instance.GetPoolObject (ObjectPooling.instance.poolStandBlock, new Vector3 (Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0)).x + 0.5f, currentBlock.transform.position.y + 1f, 0));

				blockDirection = 0;
				currentBlock.GetComponent<StandBlock> ().direction = -1;

				break;
			}
			if (tempObj != null) {
									currentBlock.GetComponent<SpriteRenderer> ().sortingOrder = tempObj.GetComponent<SpriteRenderer> ().sortingOrder + 1;
			}


	

		}
	}
	GameObject tempFistBlock;
	private int countTemp;
	[HideInInspector]
	public bool exchange = false;
	public void GenBottom()
	{
		for (int i = 0; i < ObjectPooling.instance.poolStandBlock.Count; i++) {
			if (ObjectPooling.instance.poolStandBlock [i].activeInHierarchy && ObjectPooling.instance.poolStandBlock [i].GetComponent<StandBlock> ().OnScreen ()) {
				switch (ObjectPooling.instance.poolStandBlock [i].GetComponent<SpriteRenderer> ().sprite.name) {
				case "block-sheet0_0":
					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
					GameObject obj;
					obj = Instantiate (ObjectPooling.instance.fistBlock, new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0)).x, ObjectPooling.instance.poolStandBlock [i].transform.position.y,0), 
						ObjectPooling.instance.poolStandBlock [i].transform.rotation);
					obj.GetComponent<FistBlock> ().RandomBlock (0);
					break;
				case "block-sheet0_1":
					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
					GameObject obj1;
					obj1 = Instantiate (ObjectPooling.instance.fistBlock, new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0)).x, ObjectPooling.instance.poolStandBlock [i].transform.position.y,0), 
						ObjectPooling.instance.poolStandBlock [i].transform.rotation);
					obj1.GetComponent<FistBlock> ().RandomBlock (1);
					break;
				case "block-sheet0_2":
					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
					GameObject obj2;
					obj2 = Instantiate (ObjectPooling.instance.fistBlock, new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0)).x, ObjectPooling.instance.poolStandBlock [i].transform.position.y,0), 
						ObjectPooling.instance.poolStandBlock [i].transform.rotation);
					obj2.GetComponent<FistBlock> ().RandomBlock (2);
					break;
				case "block-sheet0_3":
					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
					GameObject obj3;
					obj3 = Instantiate (ObjectPooling.instance.fistBlock, new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0)).x, ObjectPooling.instance.poolStandBlock [i].transform.position.y,0), 
						ObjectPooling.instance.poolStandBlock [i].transform.rotation);
					obj3.GetComponent<FistBlock> ().RandomBlock (3);
					break;
				case "block-sheet0_4":
					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
					GameObject obj4;
					obj4 = Instantiate (ObjectPooling.instance.fistBlock, new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0)).x, ObjectPooling.instance.poolStandBlock [i].transform.position.y,0), 
						ObjectPooling.instance.poolStandBlock [i].transform.rotation);
					obj4.GetComponent<FistBlock> ().RandomBlock (4);
					break;
				}
				break;
			}
		}
	}

	public void Wait()
	{
		Invoke ("CanExchange", 2);
	}
	void CanExchange()
	{
		exchange = false;
	}

	void DestroyBlock()
	{
		tempFistBlock.SetActive (false);
	}

	///<Summary>
	///Handle background of the game
	///</Summary>
	public void ChangeBackground(int index)
	{
		background.sprite = backGroud [index];
		land.sprite = landSpr [index];
		Camera.main.backgroundColor = skyColor [index];
	}


	///<Summary>
	///I call this method when game over and in button even in the end menu
	///</Summary>
	public void GameOver(int index)
	{

		switch (index) {
		case 0:
			Time.timeScale = 0;


//			for (int i = 0; i < ObjectPooling.instance.poolStandBlock.Count; i++) {
//				if (ObjectPooling.instance.poolStandBlock [i].activeInHierarchy) {
//					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
//				}
//			}


			CancelInvoke ();

			if (score > PlayerPrefs.GetInt ("bestscore")) {
				PlayerPrefs.SetInt ("bestscore", score);
			}

			AddPoint (1);
			// Show Tapsell ADS
			TapsellTapsellTestV3Instance.ShowAd();
			endGamePanel.SetActive (true);
			break;
		case 1:
			Time.timeScale = 1;
			Application.LoadLevel (0);
			break;
		case 2:
			Time.timeScale = 1;
			Application.LoadLevel (Application.loadedLevel);
			break;
		case 3:
			SetAudio ();
			break;

		}
	}

	void UpgradeParameter()
	{
		//i increase game parameter
		if (GameManager.instance.blockSpeed < GameManager.instance.blockSpeedMax) {
			GameManager.instance.blockSpeed += GameManager.instance.blockSpeedOffset;
		}

		GameManager.instance.starPosOffset += 1;
	}

	///<Summary>
	///Add Point
	///</Summary>


	public void AddPoint(int index)
	{
		if (index == 0) {
			switch (type) {
			case 1:
				if (score < 10) {
					//set active image
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (false);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = blueSpriteNumber [score];
				} else if (score > 9 && score < 100) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = blueSpriteNumber [(int)score / 10];
					numberImage [1].sprite = blueSpriteNumber [(int)score % 10];
				} else if (score > 99 && score < 1000) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (true);
					numberImage [0].sprite = blueSpriteNumber [(int)score / 100];
					numberImage [1].sprite = blueSpriteNumber [(int)(score % 100) / 10];
					numberImage [2].sprite = blueSpriteNumber [(int)(score % 100) % 10];
				}
				break;
			case 2:
				if (score < 10) {
					//set active image
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (false);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = orangeSpriteNumber [score];
				} else if (score > 9 && score < 100) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = orangeSpriteNumber [(int)score / 10];
					numberImage [1].sprite = orangeSpriteNumber [(int)score % 10];
				} else if (score > 99 && score < 1000) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (true);
					numberImage [0].sprite = orangeSpriteNumber [(int)score / 100];
					numberImage [1].sprite = orangeSpriteNumber [(int)(score % 100) / 10];
					numberImage [2].sprite = orangeSpriteNumber [(int)(score % 100) % 10];
				}
				break;
			case 3:
				if (score < 10) {
					//set active image
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (false);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = greenSpriteNumber [score];
				} else if (score > 9 && score < 100) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = greenSpriteNumber [(int)score / 10];
					numberImage [1].sprite = greenSpriteNumber [(int)score % 10];
				} else if (score > 99 && score < 1000) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (true);
					numberImage [0].sprite = greenSpriteNumber [(int)score / 100];
					numberImage [1].sprite = greenSpriteNumber [(int)(score % 100) / 10];
					numberImage [2].sprite = greenSpriteNumber [(int)(score % 100) % 10];
				}
				break;
			case 4:
				if (score < 10) {
					//set active image
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (false);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = purpleSpriteNumber [score];
				} else if (score > 9 && score < 100) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (false);
					//set number
					numberImage [0].sprite = purpleSpriteNumber [(int)score / 10];
					numberImage [1].sprite = purpleSpriteNumber [(int)score % 10];
				} else if (score > 99 && score < 1000) {
					numberImage [0].gameObject.SetActive (true);
					numberImage [1].gameObject.SetActive (true);
					numberImage [2].gameObject.SetActive (true);
					numberImage [0].sprite = purpleSpriteNumber [(int)score / 100];
					numberImage [1].sprite = purpleSpriteNumber [(int)(score % 100) / 10];
					numberImage [2].sprite = purpleSpriteNumber [(int)(score % 100) % 10];
				}
				break;
			}
		} else if (index == 1) {
			if (score < 10) {
				//set active image
				levelScoreImage [0].gameObject.SetActive (true);
				levelScoreImage [1].gameObject.SetActive (false);
				levelScoreImage [2].gameObject.SetActive (false);

				//set number
				levelScoreImage [0].sprite = endSpriteNumber [score];

			} else if (score > 9 && score < 100) {
				levelScoreImage [0].gameObject.SetActive (true);
				levelScoreImage [1].gameObject.SetActive (true);
				levelScoreImage [2].gameObject.SetActive (false);

				//set number
				levelScoreImage [0].sprite = endSpriteNumber [(int)score / 10];
				levelScoreImage [1].sprite = endSpriteNumber [(int)score % 10];

			} else if (score > 99 && score < 1000) {
				levelScoreImage [0].gameObject.SetActive (true);
				levelScoreImage [1].gameObject.SetActive (true);
				levelScoreImage [2].gameObject.SetActive (true);

				levelScoreImage [0].sprite = endSpriteNumber [(int)score / 100];
				levelScoreImage [1].sprite = endSpriteNumber [(int)(score % 100) / 10];
				levelScoreImage [2].sprite = endSpriteNumber [(int)(score % 100) % 10];

			}

			if (PlayerPrefs.GetInt ("bestscore") < 10) {
				bestScoreImage [0].gameObject.SetActive (true);
				bestScoreImage [1].gameObject.SetActive (false);
				bestScoreImage [2].gameObject.SetActive (false);
				bestScoreImage [0].sprite = endSpriteNumber [PlayerPrefs.GetInt ("bestscore")];
			} else if (PlayerPrefs.GetInt ("bestscore") < 100 && PlayerPrefs.GetInt ("bestscore") > 9) {
				bestScoreImage [0].gameObject.SetActive (true);
				bestScoreImage [1].gameObject.SetActive (true);
				bestScoreImage [2].gameObject.SetActive (false);
				bestScoreImage [0].sprite = endSpriteNumber [(int)PlayerPrefs.GetInt ("bestscore") / 10];
				bestScoreImage [1].sprite = endSpriteNumber [(int)PlayerPrefs.GetInt ("bestscore") % 10];
			} else if (PlayerPrefs.GetInt ("bestscore") < 1000 && PlayerPrefs.GetInt ("bestscore") > 99) {
				bestScoreImage [0].gameObject.SetActive (true);
				bestScoreImage [1].gameObject.SetActive (true);
				bestScoreImage [2].gameObject.SetActive (true);
				bestScoreImage [0].sprite = endSpriteNumber [(int)PlayerPrefs.GetInt("bestscore") / 100];
				bestScoreImage [1].sprite = endSpriteNumber [(int)(PlayerPrefs.GetInt("bestscore") % 100) / 10];
				bestScoreImage [2].sprite = endSpriteNumber [(int)(PlayerPrefs.GetInt("bestscore") % 100) % 10];
			}
		}

	}

	[Header("Audio")]
	public Image audioButton;
	public Sprite soundOn,soundOff;
	public void AudioSystem()
	{
		if (PlayerPrefs.GetInt ("sound") == 0) {
			if (AudioManager.instance != null) {
				AudioManager.instance.source.mute = false;
			}
			audioButton.sprite = soundOn;
		} else if (PlayerPrefs.GetInt ("sound") == 1) {
			if (AudioManager.instance != null) {
				AudioManager.instance.source.mute = true;
			}
			audioButton.sprite = soundOff;
		}
	}

	public void SetAudio()
	{
		if (PlayerPrefs.GetInt ("sound") == 0) {
			PlayerPrefs.SetInt ("sound", 1);
			if (AudioManager.instance != null) {
				AudioManager.instance.source.mute = true;
			}
			audioButton.sprite = soundOff;
		} else if (PlayerPrefs.GetInt ("sound") == 1) {
			PlayerPrefs.SetInt ("sound", 0);
			if (AudioManager.instance != null) {
				AudioManager.instance.source.mute = false;
			}
			audioButton.sprite = soundOn;
		}

		AudioSystem ();
	}
}

