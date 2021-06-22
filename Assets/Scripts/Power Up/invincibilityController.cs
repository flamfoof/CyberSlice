using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invincibilityController : MonoBehaviour {
	public GameObject gameController;

	void Start()
	{

	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {

			gameController.GetComponent<playerController> ().SetStateInvincible (gameObject.tag);
			//Debug.Log (gameController.GetComponent<playerController> ().playerState);
			Destroy (gameObject);
		}
	}
}
