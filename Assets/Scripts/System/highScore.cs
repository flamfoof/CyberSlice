using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highScore : MonoBehaviour {

	public bool clearHighScore = false;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("HighScore")!= null) {
			
		} else {
			Debug.Log ("Setting new INt HIGHSCORE");
			PlayerPrefs.SetInt ("HighScore", 0);
		}


		if (clearHighScore) {
			PlayerPrefs.SetInt ("HighScore", 0);
		}

		this.gameObject.GetComponent<Text> ().text = "HighScore: " + PlayerPrefs.GetInt ("HighScore");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetNewHighScore(int newHighScore){
		PlayerPrefs.SetInt ("HighScore", newHighScore);
		this.gameObject.GetComponent<Text> ().text = "HighScore: " + PlayerPrefs.GetInt ("HighScore");
	}
}
