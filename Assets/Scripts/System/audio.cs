using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audio : MonoBehaviour {
	public Slider volumeSlider;
	public Slider sfxSlider;
	public AudioSource volumeAudio;

	// Use this for initialization
	void Start () {
		volumeAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (volumeSlider != null) {
			SaveSliderVolume ();
		}
		VolumeController ();
	}
	public void SaveSliderVolume()
	{
		PlayerPrefs.SetFloat ("SliderMusic", volumeSlider.value);
		PlayerPrefs.SetFloat ("SliderFX", volumeSlider.value);
	}

	public void VolumeController()
	{
		volumeAudio.volume = PlayerPrefs.GetFloat("SliderMusic", volumeAudio.volume);
	}
}
