using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static PlayerController instance;
	[Header("Default Component")]
	public Rigidbody2D rig;
	public Animator anim;
	public SpriteRenderer spr;
	private GameObject blockStandOn;

	[Header("PlayerDie")]
	public GameObject playerBlood;
	public bool isDie;
	[Header("Check out screen")]
	Plane[] planes;
	public Collider2D col;

	[Header("Player Type (Color)")]
	///<Summary>
	///Player type(color)
	/// 1 - blue
	/// 2 - orange
	/// 3 - green
	/// 4 - purple
	///</Summary>
	public int playerType;
	public Sprite[] playerSprite;

	//This variable to count jump time
	private int jumpTime;


	private	void Awake ()
	{
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		anim.Play ("player" + playerType.ToString ());

		//Set background and player sprite
		ChangeSprite(playerType-1);
		GameManager.instance.ChangeBackground(playerType-1);
	}
	
	// Update is called once per frame
	void Update () {
		JumpControl ();

		if (!OnScreen ()) {
			if (!isDie) {
				isDie = true;
				gameObject.SetActive (false);
				PlayerDie ();
			}
		}
	}

	///<Summary>
	///Jump Control
	///</Summary>
	void JumpControl()
	{
		if (Input.GetMouseButtonDown (0)) {
			//I plus 1 to jump count
			jumpTime += 1;
			//if jump count <=1, i made the player jump
			//if jump count > 1, i do no thing
			if (jumpTime <= 1) {
				rig.gravityScale = 2;
				rig.AddForce (new Vector2 (0, 0450));
			} else {
				return;
			}

			if (AudioManager.instance != null) {
				AudioManager.instance.source.PlayOneShot (AudioManager.instance.effectClip [0]);
			}
		}
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "star") {
			
			col.GetComponent<Animator> ().Play("star");

			playerType += 1;
			if (playerType == 5) {
				playerType = 1;
			}
				
			for (int i = 0; i < ObjectPooling.instance.poolStandBlock.Count; i++) {
				if (ObjectPooling.instance.poolStandBlock [i].activeInHierarchy && ObjectPooling.instance.poolStandBlock [i].GetComponent<StandBlock> ().canMove) {
					ObjectPooling.instance.poolStandBlock [i].SetActive (false);
				}
			}

			ChangeSprite(playerType-1);

			GameManager.instance.ChangeBackground (playerType - 1);

			ObjectPooling.instance.ArrangeObject ();

			if (!GameManager.instance.exchange) {
				GameManager.instance.exchange = true;
				GameManager.instance.Wait ();
				GameManager.instance.GenBottom ();
			}
			col.gameObject.transform.position = new Vector3 (col.gameObject.transform.position.x, col.transform.position.y + GameManager.instance.starPosOffset, col.transform.position.z);
			transform.position = new Vector3 (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0)).x, transform.position.y, transform.position.z);

			if (AudioManager.instance != null) {
				AudioManager.instance.source.PlayOneShot (AudioManager.instance.effectClip [2]);
			}
		}
	}

	private	void OnCollisionEnter2D (Collision2D col)
	{
		GameManager.instance.currentBlock = col.gameObject;



		if (col.gameObject.tag == "standblock") {

			if (transform.position.y > col.transform.position.y) {
				jumpTime = 0;
				if (AudioManager.instance != null) {
					AudioManager.instance.source.PlayOneShot (AudioManager.instance.effectClip [1]);
				}
			} else {
				anim.Play ("player" + playerType.ToString () + "dead");

				if (!isDie) {
					isDie = true;
					if (AudioManager.instance != null) {
						AudioManager.instance.source.PlayOneShot (AudioManager.instance.effectClip [3]);
					}
				}
			}

			if (col.gameObject.GetComponent<SpriteRenderer> ().sprite.name == "block-sheet0_0") {
				GameManager.instance.nextFistPos = col.gameObject.transform.position;
				GameManager.instance.nextFistBlock = col.gameObject;
			}

			StandBlock ins = col.gameObject.GetComponent<StandBlock> ();
			if (ins == null) {
				return;
			} else {
				blockStandOn = col.gameObject;
//				InvokeRepeating ("WaitToDie", 0, 0.1f);
				ins.canMove = false;
				if (rig != null) {
					ins.rig.gravityScale = 0.5f;
				}
			}


		}
	}

	private void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.tag == "standblock") {
			blockStandOn = null;
		}
	}

	///<Summary>
	///This method is called when my player die
	///</Summary>
	GameObject myBlood;
	void PlayerDie()
	{
		myBlood = Instantiate (playerBlood, transform.position, transform.rotation);
		GameManager.instance.isGameOver = true;
		myBlood.GetComponent<ParticleSystemRenderer> ().material.mainTexture = GameManager.instance.bloodTexture [playerType - 1];
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;

		Invoke ("EndGame", 3);
	}

	///<Summary>
	///CommentHere
	///</Summary>
	void EndGame()
	{
		if (myBlood.activeInHierarchy) {
			myBlood.SetActive (false);
		}
		GameObject[] obj = GameObject.FindGameObjectsWithTag ("standblock");
		foreach (var obj1 in obj) {
			obj1.SetActive (false);
		}
		GameManager.instance.GameOver (0);
	}

	///<Summary>
	///This method to check if this object is on seen by camera
	///</Summary>
	private bool OnScreen ()
	{
		planes = GeometryUtility.CalculateFrustumPlanes (Camera.main);
		if (GeometryUtility.TestPlanesAABB (planes, col.bounds)) {
			return true;
		} else {
			return false;
		}
	}
	///<Summary>
	///Change Player Sprite
	///</Summary>
	void ChangeSprite(int index)
	{
		spr.sprite = playerSprite [index];
	}
}
