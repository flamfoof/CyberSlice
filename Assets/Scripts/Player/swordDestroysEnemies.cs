using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordDestroysEnemies : MonoBehaviour {

    public GameObject gameController;
	public GameObject player;
	public float distanceInFront = 0.5f;
	public float verticalDist = 0.1f;

	private Vector2 pos;

	void Start () {
		pos = player.transform.position;
	}

	void FixedUpdate () {
		pos = player.transform.position;
		pos.x += distanceInFront;
		pos.y += verticalDist;
		gameObject.transform.position = pos;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Bullet") {
			if (other.tag == "Enemy") {
				other.gameObject.SetActive (false);
				other.GetComponent<BoxCollider2D> ().enabled = false;
				other.transform.parent.gameObject.GetComponent<enemyMove> ().DestroyMe ();



			}
			if (other.tag == "Bullet") {
				Destroy (other.gameObject);
			}
            gameController.GetComponent<Score>().EnemyKilled();
			gameController.GetComponent<gameController> ().bossGauge += 3;
			gameController.GetComponent<gameController> ().progressBar.value += 3;


			//addPoints
		}
        else if (other.tag == "boss")
        {
            other.gameObject.GetComponent<bossHealth>().loseHealth();
        }
    }

}
