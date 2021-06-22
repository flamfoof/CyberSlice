using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShuriken : MonoBehaviour {
	public GameObject enemy;
	//duration which the ammo will last in seconds
	public float ammoLife = 10;
	public GameObject gameController;
	public AudioClip EnemyDamageSFX;
	AudioSource _audio; 
	public Animator anim;

	// Use this for initialization
	void Start () {
		//projectile disappears after # seconds
		Destroy (gameObject, ammoLife);
		gameController = GameObject.Find ("GameController");

		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}
		anim = GetComponent<Animator> ();
	}
	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Enemy") {
			
			PlaySound (EnemyDamageSFX, 0.2F);
			other.gameObject.SetActive (false);
			other.GetComponent<BoxCollider2D> ().enabled = false;
			other.transform.parent.gameObject.GetComponent<enemyMove> ().DestroyMe ();
			Destroy (this.gameObject);
			gameController.GetComponent<gameController> ().bossGauge += 3;
			gameController.GetComponent<gameController> ().progressBar.value += 3;
			//Explosion ();
        }
        else if (other.tag == "boss")
        {
			
            other.gameObject.GetComponent<bossHealth>().loseHealth();
			Destroy (this.gameObject);
			//Explosion ();
        }
	}
}
