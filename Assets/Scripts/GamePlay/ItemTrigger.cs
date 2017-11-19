using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour {

	public enum Item
	{
		deadTrigger,
		scoreTrigger
	}
	public Item itemtrigger;
	void OnTriggerEnter2D (Collider2D col)
	{
		switch (itemtrigger) {
		case Item.scoreTrigger:
			if (col.tag == "standblock") {
				StandBlock ins = col.GetComponent<StandBlock> ();
				if (ins == null) {
					return;
				} else {
					if (!ins.canGiveScove) {
						ins.canGiveScove = true;
						GameManager.instance.score += 1;
						GameManager.instance.AddPoint (0);
					}
				}
			}
			break;
		case Item.deadTrigger:
			if (col.tag == "standblock") {
				PlayerController ins = transform.parent.gameObject.GetComponent<PlayerController> ();
				ins.anim.Play ("player" + ins.playerType.ToString () + "dead");
				if (!ins.isDie) {
					ins.isDie = true;
					if (AudioManager.instance != null) {
						AudioManager.instance.source.PlayOneShot (AudioManager.instance.effectClip [3]);
					}
				}
				gameObject.SetActive (false);
//				transform.parent.gameObject.transform.GetComponentInChildren<Collider2D> ().enabled = false;
			}
			break;
		}
	}
}
