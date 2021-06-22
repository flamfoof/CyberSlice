using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2Attacks : MonoBehaviour {

	public GameObject fistConvey;
	public GameObject fistBullet;
	public GameObject gunConvey;
	public GameObject gunBullet;
	public bool hasFist = true;
	private GameObject player;
	private GameObject controller;
	public int gunBulletsAmount = 4;
	private float angle;


	[Header("Technical animation length values for attacking")]
	


	public Animator gunAnimator;
	public Animator fistAnimator;

    // Use this for initialization
    void Start () {
		controller = GameObject.FindGameObjectWithTag ("MasterController");
		player = GameObject.FindGameObjectWithTag ("Player");
        fistConvey.gameObject.SetActive(false);
        gunConvey.gameObject.SetActive(false);
		//controller = controller.GetComponent<gameController> ();

		StartCoroutine (StartAttackRoutine ());

    }
	
	// Update is called once per frame
	void Update () {
		//testing
		if (Input.GetKeyDown (KeyCode.U) && controller.GetComponent<gameController>().adminMode) {
			FindPlayerDiffAngle (fistConvey);

			//fistConvey.transform.rotation = Quaternion.Euler(0, 0, angle);
			StartCoroutine(FistAttack());
		} else if (Input.GetKeyDown (KeyCode.I) && controller.GetComponent<gameController>().adminMode) {
			FindPlayerDiffAngle (gunConvey);
			//gunConvey.transform.rotation = Quaternion.Euler(0, 0, angle);
			StartCoroutine(GunAttack());
		}
//		Debug.Log (fistConvey.transform.eulerAngles);
		//Debug.Log (Vector2.Angle (fistConvey.transform.position, player.transform.position));
	}
	private IEnumerator StartAttackRoutine()
	{
		while(true)
		{
			float randomAttackInterval = Random.Range(1.0f, 2.0f);

			if (Random.Range (0, 10) <= 3) {
				StartCoroutine (GunAttack ());
				yield return new WaitForSeconds (4.0f);
			} else {
				if (hasFist) {
					StartCoroutine (FistAttack ());
					yield return new WaitForSeconds (5.0f);
				}
			}
			yield return new WaitForSeconds(randomAttackInterval);


		}
	}

	private IEnumerator FistAttack()
	{
		hasFist = false;
		fistAnimator.SetTrigger ("FistFire");
		yield return new WaitForSeconds(1.5f);
		FindPlayerDiffAngle (fistConvey);
		fistConvey.transform.rotation = Quaternion.Euler(0, 0, angle);
		fistConvey.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		GameObject fist = Instantiate(fistBullet, fistConvey.transform.position, fistConvey.transform.rotation, gameObject.transform);
		fistConvey.SetActive (false);
		fistAnimator.SetTrigger ("NoFistIdle");
		yield return new WaitForSeconds(7.0f);
		fistAnimator.SetTrigger ("FistReload");
		hasFist = true;
		//change convey to angle
		//instantiate the fist.
	}

	private IEnumerator GunAttack()
	{
		gunAnimator.SetTrigger ("GunShoot");
		yield return new WaitForSeconds(1.0f);
		FindPlayerDiffAngle (gunConvey);
		gunConvey.transform.rotation = Quaternion.Euler(0, 0, angle);
		gunConvey.SetActive (true);
		yield return new WaitForSeconds(1.0f);
		gunConvey.SetActive (false);

		for (int i = 0; i < gunBulletsAmount; i++) {
			GameObject fist = Instantiate(gunBullet, gunConvey.transform.position, gunConvey.transform.rotation, gameObject.transform);
			yield return new WaitForSeconds (0.5f);
		}


		//change convey to angle
		//instantiate the gun bullet.
	}

	void FindPlayerDiffAngle(GameObject conveyObject)
	{
		Vector3 dir = player.transform.position - conveyObject.transform.position;
		angle = (Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg) - 180.0f;
		Debug.Log ("Angle is: " + angle);
	}

	void OnDisable()
	{
		fistConvey.gameObject.SetActive(false);
		gunConvey.gameObject.SetActive(false);
	}
	void OnEnable()
	{
		fistConvey.gameObject.SetActive(false);
		gunConvey.gameObject.SetActive(false);
	}
		
}
