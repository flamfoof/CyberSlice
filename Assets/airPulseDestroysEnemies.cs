using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPulseDestroysEnemies : MonoBehaviour {

		public GameObject gameController;
		public GameObject player;
		public float distanceInFront = 0.5f;
		public float verticalDist = 0.1f;

		private Vector2 pos;
	private Vector2 initPosStartAttack;
	private bool isSlicing = false;
	private int iterator = 0;
		void Start () {
			pos = player.transform.position;
		}

		void FixedUpdate () {
		if (isSlicing == false) {
			pos = player.transform.position;
			pos.x += distanceInFront;
			pos.y += verticalDist;
			gameObject.transform.position = pos;
		} else if (isSlicing) {
			pos = player.transform.position;
			pos.y = initPosStartAttack.y;
			pos.x += 1.0f + (.25f * iterator);
			gameObject.transform.position = pos;
			iterator++;
		}
		}
	public void airPulse(){
		pos = player.transform.position;
		iterator = 0;
		isSlicing = true;
		initPosStartAttack = player.transform.position;
	}
	public void stopAirPulse(){
		isSlicing = false;
		//pos = player.transform.position;
	}

		void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Bullet") {
			if (other.tag == "Enemy") {
				other.gameObject.SetActive (false);
				other.GetComponent<BoxCollider2D> ().enabled = false;
				other.transform.parent.gameObject.GetComponent<enemyMove> ().DestroyMe ();

			if (other.tag == "Bullet") {
				Destroy (other.gameObject);
			}

				gameController.GetComponent<Score> ().EnemyKilled ();
				gameController.GetComponent<gameController> ().bossGauge += 3;
				gameController.GetComponent<gameController> ().progressBar.value += 3;


				//addPoints
			} else if (other.tag == "boss") {
				other.gameObject.GetComponent<bossHealth> ().loseHealth ();
 
			}

		}
	}

}