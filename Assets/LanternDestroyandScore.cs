using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDestroyandScore : MonoBehaviour {

	public GameObject gameController;

	public GameObject secretChip1;
	public GameObject secretChip2;
	public GameObject secretChip3;
	public GameObject secretChip4;
	AudioSource _audio; 
	public AudioClip explosionSFX;

	public GameObject SmokeEmptyObject;
	//temp = Instantiate (SmokeEmptyObject);



	public GameObject player;

	public float spawnChipPercent = 0.1f;

	private bool willSpawnChip = false;
	private bool alreadySpawnedChip = false;
	private Animator smokeAnimation;
	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController");
		player = GameObject.FindGameObjectWithTag ("Player");
		smokeAnimation = GetComponent<Animator> ();

		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Finish" && alreadySpawnedChip == false) {
			alreadySpawnedChip = true;

			//spawn explosion 
			GameObject tempExplosion = new GameObject ();

			tempExplosion = Instantiate (SmokeEmptyObject);
			tempExplosion.transform.position = this.gameObject.transform.position;
			tempExplosion.GetComponent<Animator> ().Play ("Lantern Smoke");


			//determine if chip will spawn
			float randomChance = (Random.Range(0,100)/100.0f);
			Debug.Log ("randomChange = " + randomChance);
			if (spawnChipPercent >= randomChance) {
				willSpawnChip = true;
			}


			if (willSpawnChip) {
				
				//spawn chip
		
				if (player.GetComponent<playerWinPickups> ().chip1Collected == false) {
					GameObject temp = new GameObject ();

					temp = Instantiate (secretChip1);
					temp.transform.position = this.gameObject.transform.position;
				
				}else if (player.GetComponent<playerWinPickups> ().chip3Collected == true) {
					GameObject temp = new GameObject ();
					temp = Instantiate (secretChip4);
					temp.transform.position = this.gameObject.transform.position;
				}else if (player.GetComponent<playerWinPickups> ().chip2Collected == true) {
					GameObject temp = new GameObject ();
					temp = Instantiate (secretChip3);
					temp.transform.position = this.gameObject.transform.position;
				}else if (player.GetComponent<playerWinPickups> ().chip1Collected == true) {
					GameObject temp = new GameObject ();
					temp = Instantiate (secretChip2);
					temp.transform.position = this.gameObject.transform.position;
				}



			}
			gameController.GetComponent<Score>().EnemyKilled();
			gameController.GetComponent<gameController> ().bossGauge += 3;
			gameController.GetComponent<gameController> ().progressBar.value += 3;
			//smokeAnimation.SetInt("
			//destory lantern

			//GetComponent<Animator> ().SetTrigger ("Smoke");

			Destroy (this.gameObject);
			PlaySound (explosionSFX, 0.2f);

				
			//smokeAnimation = GetComponent<Animator> ();
			//GetComponent<Animator>().SetTrigger("Smoke");
			//smokeAnimation.SetTrigger("Smoke");
		}
	}
}
