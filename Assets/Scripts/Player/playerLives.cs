using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerLives : MonoBehaviour {

	public Image lifeUI1;
	public Image lifeUI2;
	public Image lifeUI3;

	public int life = 3;
    public Text LivesText;

	public GameObject gameController;
	public playerController playerControl;

	public GameObject restartText;
	public GameObject highScore;
	public bool playerIsHit = false;

	private bool canRestart = false;
	private bool playerIsAlive = true;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if (gameController == null) {
			gameController = GameObject.FindGameObjectWithTag ("MasterController");
		}
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && canRestart){
			SceneManager.LoadScene (3);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		gameController.GetComponent<playerController> ().isHit = playerIsHit;
		if (other.tag == "Enemy" && gameController.GetComponent<playerController>().playerState == playerController.RunnerState.Normal) {
			other.gameObject.SetActive (false);
			other.GetComponent<BoxCollider2D> ().enabled = false;
			other.transform.parent.gameObject.GetComponent<enemyMove> ().DestroyMe ();
			Debug.Log ("taking damage still");
			PlayerIsHit ();
		} else if (other.tag == "Enemy" && gameController.GetComponent<playerController>().playerState == playerController.RunnerState.Invincible) {
			other.gameObject.SetActive (false);
			other.GetComponent<BoxCollider2D> ().enabled = false;
			other.transform.parent.gameObject.GetComponent<enemyMove> ().DestroyMe ();
		}
		if ((other.tag == "Bullet" || other.tag == "Fist") && gameController.GetComponent<playerController>().playerState == playerController.RunnerState.Normal) {
			Destroy (other.gameObject);
			Debug.Log ("ran into a bullet");
			PlayerIsHit ();


		} else if ((other.tag == "Bullet" || other.tag == "Fist") && gameController.GetComponent<playerController>().playerState == playerController.RunnerState.Invincible) {
			Destroy (other.gameObject);
		}
        if (other.tag == "Lazer" && gameController.GetComponent<playerController>().playerState == playerController.RunnerState.Normal)
        {
            //Destroy(other.gameObject);
			PlayerIsHit ();
        }
        
		if (life < 1 & playerIsAlive) {
			StartCoroutine (setActiveRestartUI ());
		}
		if (other.gameObject.name == "PlayerDestroyer" & playerIsAlive) {
			StartCoroutine (setActiveRestartUI ());
		}
		//the playerIsHit is being controlled by the animator controller for being hit


	}


	private void PlayerIsHit()
	{
		life--;
		if (life > 0) {
			gameController.GetComponent<playerController> ().animator.SetTrigger ("isHit");
			playerIsHit = true;

			playerControl.SetStateInvincible (null);
		}
		LoseLifeUI();
	}

	private void LoseLifeUI(){
		if (lifeUI1.enabled == true) {
			lifeUI1.enabled = false;
		} else if (lifeUI2.enabled == true) {
			lifeUI2.enabled = false;
		} else if (lifeUI3.enabled == true) {
			lifeUI3.enabled = false;
		}
	}

	private IEnumerator setActiveRestartUI(){
		gameController.GetComponent<cameraMover> ().playerDead = true;
		gameController.GetComponent<Score> ().playerIsDead = true;
		gameController.GetComponent<playerController> ().isDead = true;
		gameController.GetComponent<playerController> ().animator.SetTrigger ("Death");
		//this.GetComponent<BoxCollider2D> ().enabled = false;
		playerIsAlive = false;
		//Time.timeScale = 0;
		highScore.GetComponent<highScore>().SetNewHighScore(gameController.GetComponent<Score>().PlayerScore);

		yield return new WaitForSeconds (1.0f);
		restartText.SetActive (true);
		canRestart = true;
	}
}
