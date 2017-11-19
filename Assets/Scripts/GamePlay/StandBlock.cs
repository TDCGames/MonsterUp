using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandBlock : MonoBehaviour {
	public SpriteRenderer spr;
	public Rigidbody2D rig;
	public Sprite[] blockSpr;
	public Collider2D[] blockCollider;
	public float speed;
	public bool canMove = true;

	public int direction;

	//check seen by camera variable
	Plane[] planes;
	public Collider2D col;
	private bool startcheck;

	[HideInInspector]
	public bool canGiveScove;

	private float yStartPos;

	void OnEnable ()
	{
		canMove = true;
		RandomBlock (Random.Range (0, 5));
		Invoke ("StartCheckOutScreen", 3);
		yStartPos = transform.position.y;
	}

	void OnDisable()
	{
		startcheck = false;
		if (rig != null) {
			rig.gravityScale = 0;
		}
		transform.eulerAngles = Vector3.zero;

		canGiveScove = false;
	}
	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy && canMove) {
			transform.eulerAngles = Vector3.zero;
			MoveBlock (speed);
		}

		//after 3 seconds, if object is not seen by camera, i disactive the object
		if (startcheck && !OnScreen ()) {
			if (transform.position.y == yStartPos) {
				gameObject.SetActive (false);
			}
		}
			
	}

	///<Summary>
	///Select randomblock and enable the correspond collider
	///</Summary>
	public void RandomBlock(int index)
	{
		for (int i = 0; i < blockSpr.Length; i++) {
			if (i == index) {
				spr.sprite = blockSpr [index];
				blockCollider [i].enabled = true;
				col = blockCollider [i];
			} else {
				blockCollider [i].enabled = false;
			}
		}
	}

	///<Summary>
	///This method to check if this object is on seen by camera
	///</Summary>
	public bool OnScreen ()
	{
		planes = GeometryUtility.CalculateFrustumPlanes (Camera.main);
		if (GeometryUtility.TestPlanesAABB (planes, col.bounds)) {
			return true;
		} else {
			return false;
		}
	}

	///<Summary>
	///Move block
	///</Summary>
	public void MoveBlock(float speed)
	{
		transform.position = new Vector3 (transform.position.x + GameManager.instance.blockSpeed * direction, transform.position.y, 0);
	}

	///<Summary>
	///Start check if object is out screen
	///</Summary>
	void StartCheckOutScreen()
	{
		startcheck = true;
	}
}
