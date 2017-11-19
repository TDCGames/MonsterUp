using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistBlock : MonoBehaviour {
	public SpriteRenderer spr;
	public Sprite[] blockSpr;
	public Collider2D[] blockCollider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
			} else {
				blockCollider [i].enabled = false;
			}
		}
	}
}
