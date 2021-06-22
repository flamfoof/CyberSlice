using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioGame : MonoBehaviour {
	public AudioSource musicAudio;
	public AudioSource sfxAudio;

	// Use this for initialization
	void Start () {
		musicAudio = GetComponent<AudioSource> ();
		sfxAudio = GetComponent<AudioSource> ();
		VolumeController ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void VolumeController()
	{
		if (musicAudio != null) {
			musicAudio.volume = PlayerPrefs.GetFloat ("SliderMusic", musicAudio.volume);
		}
		if (sfxAudio != null) {
			sfxAudio.volume = PlayerPrefs.GetFloat ("SliderFX", sfxAudio.volume);
		}
	}
}
