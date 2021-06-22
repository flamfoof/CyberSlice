using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearBoard : MonoBehaviour {

    private int maxSweepRange = 100;
	private List<GameObject> objToDestroy = new List<GameObject> ();

	public GameObject player;
	public Transform camPos;

	public float distanceOffset = 5.0f;
	public void Update()
	{
		/*
		if(Input.GetKeyDown(KeyCode.Z))
		{
			Sweet ();
			Debug.Log ("Sweep range beyond: " + camPos.position.x + distanceOffset);

		}
		*/
	}
	public void Sweet()
	{
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if(Vector3.Distance(enemy.transform.position, player.transform.position) > distanceOffset)
			{
				objToDestroy.Add (enemy);
			}
		}

		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall")) {
			if(Vector3.Distance(wall.transform.position, player.transform.position) >  distanceOffset)
			{
				objToDestroy.Add (wall);
			}
		}

		foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
			if(Vector3.Distance(bullet.transform.position, player.transform.position) >  distanceOffset)
			{
				objToDestroy.Add (bullet);
			}
		}

		for(int i = 0; i < objToDestroy.Count; i++)
		{
			Destroy(objToDestroy[i]);
		}

		objToDestroy.Clear();
	}
	public void SweetEverything()
	{
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if(Vector3.Distance(enemy.transform.position, player.transform.position) > distanceOffset)
			{
				objToDestroy.Add (enemy);
			}
		}

		foreach(GameObject pUp in GameObject.FindGameObjectsWithTag("Power Up"))
		{
			if(Vector3.Distance(pUp.transform.position, player.transform.position) > distanceOffset)
			{
				objToDestroy.Add (pUp);
			}
		}

		foreach(GameObject pUp in GameObject.FindGameObjectsWithTag("Invincible"))
		{
			if(Vector3.Distance(pUp.transform.position, player.transform.position) > distanceOffset)
			{
				objToDestroy.Add (pUp);
			}
		}

		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall")) {
			if(Vector3.Distance(wall.transform.position, player.transform.position) >  distanceOffset)
			{
				objToDestroy.Add (wall);
			}
		}

		foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
			if(Vector3.Distance(bullet.transform.position, player.transform.position) >  distanceOffset)
			{
				objToDestroy.Add (bullet);
			}
		}

		for(int i = 0; i < objToDestroy.Count; i++)
		{
			Destroy(objToDestroy[i]);
		}

		objToDestroy.Clear();
	}
}
