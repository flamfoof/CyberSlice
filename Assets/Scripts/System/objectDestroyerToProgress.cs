using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectDestroyerToProgress : MonoBehaviour {

	public gameController controller;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.tag == "Bullet") {
			controller.bossGauge += 1;
			controller.progressBar.value += 1;
			Destroy (other.gameObject);
		}
		if (other.transform.tag == "Wall") {
			controller.bossGauge += 2;
			controller.progressBar.value += 2;
			Destroy (other.gameObject);
		}

		if (other.transform.tag == "Ground") {
			Destroy (other.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Enemy") {
			controller.bossGauge += 1;
			//Debug.Log ("enemy");
			controller.progressBar.value += 1;
			Destroy (other.gameObject);
		}
	}
}
