using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {
	public static ObjectPooling instance;

	public GameObject standBlock;
	public GameObject fistBlock;
	public List<GameObject> poolStandBlock;
	public List<GameObject> poolFistBlock;
	private	void Awake ()
	{
		instance = this;	

		FillObject (100, standBlock, poolStandBlock);
		FillObject (3, fistBlock, poolFistBlock);
	}

	///<Summary>
	///Implement this method to fill ObjectPool
	///</Summary>
	void FillObject(int fillAmout, GameObject objectToFill, List<GameObject> pooledList)
	{
		for (int i = 0; i < fillAmout; i++) {
			GameObject obj = Instantiate (objectToFill, transform.position, transform.rotation);
			obj.transform.SetParent (transform);
			obj.name = i.ToString();
			pooledList.Add (obj);
			obj.SetActive (false);
		}
	}

	///<Summary>
	///Implement this method to get object from a pool list
	///</Summary>
	public GameObject GetPoolObject(List<GameObject> pooledList, Vector3 newObjPos)
	{
		for (int i = 0; i < pooledList.Count; i++) {
			if (!pooledList [i].activeInHierarchy) {
				pooledList [i].transform.position = newObjPos;
				pooledList [i].SetActive (true);
				return pooledList [i];
			}
		}
		return null;
	}

	///<Summary>
	///Verticle arrangement
	///</Summary>
	public void ArrangeObject()
	{
		for (int i = 0; i < poolStandBlock.Count; i++) {
			if (poolStandBlock [i].activeInHierarchy) {
				poolStandBlock [i].transform.position = new Vector3 (Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2f, 0, 0)).x, poolStandBlock [i].transform.position.y, poolStandBlock [i].transform.position.z);
			}
		}
	}
}
