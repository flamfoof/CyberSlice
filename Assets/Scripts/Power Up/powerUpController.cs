using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpController : MonoBehaviour {
	public GameObject gameController;
	public AudioClip powerupSFX;
	AudioSource _audio; 

	void Start (){
		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
//			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}
	}

	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			//Wowwwww.... So I spent like 1.5 hours figuring out why this wouldn't work. 
			//All the stuff is happening in the Game Controller object lololol........
			//other.GetComponent<playerController>().projectileCount += 5; //<--- so much sweat and tears for 1 line of code not working.
			PlaySound (powerupSFX, 0.4f); //sound effect
			gameController.GetComponent<playerController> ().projectileCount += 5; //<--- this is the one..
			gameController.GetComponent<playerController> ().SetAmmoCount();

			Destroy (gameObject);
		}
	}
}
