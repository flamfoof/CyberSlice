using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {


    public Text PlayerScoreText;
    public int PlayerScore;
    public int PointsForEnemyKill;

	public bool playerIsDead = false;

	// Use this for initialization
	void Start () {
		PlayerScore = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (playerIsDead == false) {
			PlayerScore ++;
			PlayerScoreText.text = "Score: " + PlayerScore;
		}
    }
    public void EnemyKilled()
    {
//		Debug.Log ("EKIA");
        PlayerScore += PointsForEnemyKill;
    }
}
