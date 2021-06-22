using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerWinPickups : MonoBehaviour {
	
	public bool chip1Collected;
	public bool chip2Collected;
	public bool chip3Collected;
	public bool chip4Collected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (chip1Collected && chip2Collected && chip3Collected && chip4Collected) {
			SceneManager.LoadScene ("SecretWin");
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "chip1") {
			chip1Collected = true;
			Destroy (other.gameObject);
		}
		else if(other.tag == "chip2"){
			chip2Collected = true;
			Destroy (other.gameObject);
		}
		else if(other.tag == "chip3"){
			chip3Collected = true;
			Destroy (other.gameObject);
		}
		else if(other.tag == "chip4"){
			chip4Collected = true;
			Destroy (other.gameObject);
		}
	}

}
