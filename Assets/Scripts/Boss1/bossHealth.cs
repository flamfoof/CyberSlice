using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossHealth : MonoBehaviour {

    public int health;
	public int maxHealth;
	public Slider bosshealthUI;
	public gameController controller;
	public bool spawned = false;
	private Animator anim;
	public float alphaDepth;
	public AudioClip bossDamageSFX;
	public GameObject explosionAnimation;
	AudioSource _audio; 



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}
	}
	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}
	
	// Update is called once per frame
	void Update () {
		bosshealthUI.value = ((health * 1.0f) / (maxHealth * 1.0f));
		if (bosshealthUI.gameObject.activeSelf) {
		
		} else {
			bosshealthUI.gameObject.SetActive (true);
		}
        
    }
    public void loseHealth()
    {
        health--;
		PlaySound (bossDamageSFX, 0.2f);
		StartCoroutine (BossFlashAlpha ());
        if (health <= 0)
        {
            //Destroy(this.gameObject);
			if (gameObject.name == "boss1") {
				//anim.SetTrigger ("dead");
				GameObject explosion = Instantiate(explosionAnimation, transform.position, Quaternion.identity);
				gameObject.SetActive(false);
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				SettingBossInactive ();
				controller.SwitchObstacleSprite ();
				controller.GetComponent<backgroundSpawner> ().clearBackgrounds4AwayFromPlayer ();
				controller.switchBossSliderImage ();
			} else if (gameObject.name == "boss2") {
				Debug.Log ("boss 2 got rekt");
				GameObject explosion = Instantiate(explosionAnimation, transform.position, Quaternion.identity);
				gameObject.SetActive(false);
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				SettingBossInactive ();
				controller.SwitchObstacleSprite ();
				controller.GetComponent<backgroundSpawner> ().clearBackgrounds4AwayFromPlayer ();
				controller.switchBossSliderImage ();
			}
			//temporary place holder for new boss hp

        }
    }

	public void SettingBossInactive()
	{
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		bosshealthUI.gameObject.SetActive(false);
		bosshealthUI.value = 1.0f;
		controller.bossIsActive = false;
		controller.BossDeath ();
		health = maxHealth;
		controller.boss[controller.currentBoss].SetActive (false);
        doubleBossHealth();

    }

	//not actually double
    private void doubleBossHealth()
    {
		maxHealth = Mathf.FloorToInt(maxHealth * 1.5f);
        health = maxHealth;
    }

	private IEnumerator BossFlashAlpha(){
		float maxAlpha = 1.0f;
		Color newColor = Color.red;
		for (int e = 0; e < 10; e++) {
			for (int i = 0; i < 10; i++) {
				newColor.a = newColor.a - ((maxAlpha - alphaDepth) / 10);
				GetComponent<SpriteRenderer> ().color = newColor;
				yield return new WaitForSeconds (.02f);
			}
			for (int i = 0; i < 10; i++) {
				newColor.a = newColor.a + ((maxAlpha - alphaDepth) / 10);
				GetComponent<SpriteRenderer> ().color = newColor;
				yield return new WaitForSeconds (.02f);
			}
		}
		GetComponent<SpriteRenderer> ().color = Color.white;
	}
}
