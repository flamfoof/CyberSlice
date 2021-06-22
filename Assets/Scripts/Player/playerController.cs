using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour {

	[Header("Player Settings")]
    public GameObject playerGO;

	//Jumping Mechanics
    public float playerJumpSpeed;
	public bool isFalling = false;
	public levelController pauseControl;

	//Shooting Mechanics
	[Header("Shooting")]
	public Rigidbody2D playerProjectile;
	public float projectileSpeed;
	public int projectileCount;
	public Text ammoCount;

	//Sword Mechanics
    //I like these headers, they look nice :3
	[Header("Sword")]
	public GameObject playerSword;
	public GameObject playerAirSlice;
	public float lengthOfSwordSwingInSeconds = 1.0f;
	public float coolDownTimeInSeconds = 2.0f;
	private bool swordOnCoolDown = false;

    //Wall Mechanics
    [Header("Wall")]
    public GameObject mainCamera;
    private float startingXDistanceBetweenPlayerToCam = 0;
    private float playerStartingX;
    public float PlayerRecoverFromWallspeed = 1.0f;

	//Player States
	public enum RunnerState{Normal, Invincible};
	[Header("Player States")]
	public RunnerState playerState;
	public float invincibilityTime;
	public AudioClip ninjastarSFX;
	public AudioClip jumpSFX;
	public AudioClip swordswingSFX;
	public bool isDead = false;
	public bool isHit = false;
	AudioSource _audio; 
	public float numberOfInvincibleFlashes;
	public float alphaDepth;

	private bool shouldJump = false;
	private bool jumped = false;

	//Animator
	public Animator animator;

	//Invincibility
	public GameObject invincibilityGlow;

    void Start () {
        //wall Mechanic
        playerStartingX = playerGO.transform.position.x;
        startingXDistanceBetweenPlayerToCam = mainCamera.transform.position.x - playerStartingX;

		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}
		if (GetComponent<gameController> ().adminMode) {
			projectileCount += 200;
		}
    }
	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}
	//Player Inputs and Wall Hazard lerp 
	void Update () {
		if (mainCamera == null) {
			mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		}
		if (playerGO == null) {
			playerGO = GameObject.Find ("Player");
		}
		if (!isDead) {
			if (!isHit) {
				//Jumping
                if (Input.GetKeyDown("space")){
                    
                }
				if ((shouldJump) && !pauseControl.isPaused) {
					
					playerGO.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, playerJumpSpeed);
					if (!jumped) {
						PlaySound(jumpSFX, 0.1F); //jump sound effect
						Debug.Log("jumped");
						jumped = true;
					}
				}
				if ((Input.GetKeyDown("space") || Input.GetButtonDown("Fire1")) && !pauseControl.isPaused) {
					PlaySound(jumpSFX, 0.1F); //jump sound effect
					playerGO.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, playerJumpSpeed);
				}

				if (playerGO.GetComponent<Rigidbody2D> ().velocity.y <= -0.01f) {
					animator.SetBool ("Falling", true);

				} else {
					animator.SetBool ("Falling", false);
				}

			
				if (Input.GetKeyDown ("w") || Input.GetButtonDown("Fire2")) {
					if (GetComponent<gameController> ().adminMode) {
						setSwordCoolDown (true);
						StartCoroutine (SwingSword ());
						PlaySound (swordswingSFX, 0.12F);//sword swing sound effect
						animator.SetTrigger ("Sword");
					} else {
						if (swordOnCoolDown == false) {
							setSwordCoolDown (true);
							StartCoroutine (SwingSword ());
							PlaySound (swordswingSFX, 0.12F);//sword swing sound effect
							animator.SetTrigger ("Sword");
						} else {
							Debug.Log ("already swinging");
						}
					}

				}
				if (Input.GetKeyDown ("f")) {
					SetStateInvincible (null);

				}
				//Player shooting
				//If we ever have time to clean up code, I will move this into it's own script and correct
				//any references to this shooting action
				if (GetComponent<gameController> ().adminMode) {
					projectileCount += 200;
					if (Input.GetKey ("e") || Input.GetButtonDown("Fire3")) {

						Rigidbody2D projectile = Instantiate (playerProjectile, playerGO.transform.position, Quaternion.identity);
						PlaySound (ninjastarSFX, 0.8F); //dart trhow sfx
						projectile.velocity = new Vector2 (projectileSpeed, 0);
						animator.SetTrigger ("Shuriken");
					}
				} else {
					if ((Input.GetKeyDown ("e") || Input.GetButtonDown("Fire3")) && projectileCount > 0) {
						projectileCount--;
						SetAmmoCount ();
						Rigidbody2D projectile = Instantiate (playerProjectile, playerGO.transform.position, Quaternion.identity);
						PlaySound (ninjastarSFX, 0.8F); //dart trhow sfx
						projectile.velocity = new Vector2 (projectileSpeed, 0);
						animator.SetTrigger ("Shuriken");
					}
				}
			}
		}
        //Wall Lerp
        if(mainCamera.transform.position.x - playerGO.transform.position.x > startingXDistanceBetweenPlayerToCam & playerGO.GetComponent<playerCollidingWithWall>().playerIsCollidingWall == false)
        {
            Vector3 startLerp = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, playerGO.transform.position.z);
            Vector3 endLerp = new Vector3(mainCamera.transform.position.x - startingXDistanceBetweenPlayerToCam, playerGO.transform.position.y, playerGO.transform.position.z);
            playerGO.transform.position = Vector3.Lerp(startLerp, endLerp, Time.deltaTime * PlayerRecoverFromWallspeed);
        }

		//if restarting
		if (Input.GetKeyDown (KeyCode.Space)) {

		}

		//Animation parameters
		//Animator.SetBool("Grounded", _controller.state.Grounded);
	}

	private void setSwordCoolDown(bool coolDownSwitch){
		swordOnCoolDown = coolDownSwitch;
	}

	private IEnumerator SwingSword(){
		StartCoroutine(SwordPulse ());
		playerSword.SetActive (true);
		yield return new WaitForSeconds (lengthOfSwordSwingInSeconds);
		playerSword.SetActive (false);
		yield return new WaitForSeconds (coolDownTimeInSeconds);
		setSwordCoolDown (false);
	}
	private IEnumerator SwordPulse(){
		yield return new WaitForSeconds (0.2f);
		playerAirSlice.GetComponent<airPulseDestroysEnemies> ().airPulse ();
		yield return new WaitForSeconds (0.1f);
		playerAirSlice.GetComponent<SpriteRenderer> ().enabled = true;
		playerAirSlice.GetComponent<BoxCollider2D> ().enabled = true;

		yield return new WaitForSeconds (0.35f);
		playerAirSlice.GetComponent<airPulseDestroysEnemies> ().stopAirPulse ();
		playerAirSlice.GetComponent<SpriteRenderer> ().enabled = false;
		playerAirSlice.GetComponent<BoxCollider2D> ().enabled = false;

	}

	public void LimbitlessControllerJump(){
		
		shouldJump = true;

	}

	public void LimbitlessControllerJumpOff(){

		shouldJump = false;
		jumped = false;
					
	}

	public void SetAmmoCount()
	{
		ammoCount.text = "x " + projectileCount;
	}

	public void SetStateInvincible(string tag)
	{
		playerState = RunnerState.Invincible;
		StartCoroutine (InvincibleMode(tag));
	}

	private IEnumerator InvincibleMode(string tag)
	{
		//StartCoroutine(playerFlashAlpha (tag));
		invincibilityGlow.SetActive(true);
		yield return new WaitForSeconds (invincibilityTime);
		invincibilityGlow.SetActive (false);
		playerState = RunnerState.Normal;
	}

	private IEnumerator playerFlashAlpha(string tag){
		float maxAlpha = 1.0f;
		Color newColor;
		if (tag == "Invincible") {
			newColor = Color.yellow;
		} else {
			//if enemies or anything else
			newColor = Color.red;
		}
		for (int e = 0; e < numberOfInvincibleFlashes; e++) {
			for (int i = 0; i < 10; i++) {
				newColor.a = newColor.a - ((maxAlpha - alphaDepth) / 10);
				playerGO.GetComponent<SpriteRenderer> ().color = newColor;
				yield return new WaitForSeconds (.02f);
			}
			for (int i = 0; i < 10; i++) {
				newColor.a = newColor.a + ((maxAlpha - alphaDepth) / 10);
				playerGO.GetComponent<SpriteRenderer> ().color = newColor;
				yield return new WaitForSeconds (.02f);
			}
		}
		playerGO.GetComponent<SpriteRenderer> ().color = Color.white;
	}

}
