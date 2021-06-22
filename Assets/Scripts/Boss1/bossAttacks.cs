using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAttacks : MonoBehaviour {

    public GameObject topAttackConveyance;
    public GameObject bottomAttackConveyance;
    public GameObject topAttackConveyanceLazer;
    public GameObject topAttackLazer;
    public GameObject PS_Lazer;

    public GameObject topAttackPosition;
    public GameObject bottomAttackPosition;

    public GameObject enemySineProjectile;
    private int numShurikensPerAttack = 3;

	public gameController controller;


	[Header("Technical animation length values for attacking")]
	public float delayBetweenAllAttacks = 4.0f;

	public float laserOpenTimeAnim = 1.0f;
	public float laserCloseTimeAnim = 1.0f;
	public float laserDelayWarning = 1.0f;
	public float laserAttackDuration = 1.0f;

	public float missileShootTimeAnim = 0.5f;
	public float missileDelayWarning = 1.0f;
	public float missileDelayBetweenShots = 0.5f;


	private Animator animator;

    // Use this for initialization
    void Start () {
        topAttackConveyance.gameObject.SetActive(false);
        bottomAttackConveyance.gameObject.SetActive(false);
		animator = GetComponent<Animator> ();
		controller = controller.GetComponent<gameController> ();

    }
	
	// Update is called once per frame
	void Update () {
		//remove these inputs before building
		if (controller.adminMode == true) {
			//Push Q first before using missiles
			if (Input.GetKeyDown (KeyCode.X)) {
				StartCoroutine (AttackTopSide ());
			} else if (Input.GetKeyDown (KeyCode.C)) {
				StartCoroutine (AttackBottomSide ());
			} else if (Input.GetKeyDown (KeyCode.V)) {
				StartCoroutine (FireLazerBeam ());
			} else if (Input.GetKeyDown (KeyCode.Q)) {
				StartCoroutine (BossSpawnPrep ());
			}
		}

	}
		
    public void StartAttackCycle()
    {
		StartCoroutine (BossSpawnPrep ());
		Debug.Log ("Started my attack coroutine");
    }

	private IEnumerator BossSpawnPrep()
	{
//		Debug.Log ("MiSSILE ON");
		yield return new WaitForSeconds (0.2f);
		animator.SetBool ("missileOpen", true);
		animator.SetTrigger ("missileToggle");
		yield return new WaitForSeconds (2.0f);
		StartCoroutine(AttackCycle());
	}

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            if (Random.Range(0, 2) >= 1){
				//Debug.Log ("Attacking top");
                yield return new WaitForSeconds(5.0f);
                StartCoroutine(AttackTopSide());
				//Debug.Log ("Attacking bot");
                yield return new WaitForSeconds(5.0f);
                StartCoroutine(AttackBottomSide());

            }
            else {
				//Debug.Log ("Firing my laser");
                yield return new WaitForSeconds(5.0f);
                StartCoroutine(FireLazerBeam());
            }
			yield return new WaitForSeconds (delayBetweenAllAttacks);
        }
        
    }

    private IEnumerator FireLazerBeam()
    {
		//this trigger will open mouth
		animator.SetTrigger ("laserIn");
		Debug.Log ("Opening laser");
		yield return new WaitForSeconds(laserOpenTimeAnim);

       
        topAttackConveyanceLazer.SetActive(true);
		yield return new WaitForSeconds(laserDelayWarning);

        topAttackConveyanceLazer.SetActive(false);
        PS_Lazer.SetActive(true);
        PS_Lazer.GetComponent<ParticleSystem>().Play();
        topAttackLazer.SetActive(true);
        yield return new WaitForSeconds(laserAttackDuration);

        PS_Lazer.GetComponent<ParticleSystem>().Stop();
        //PS_Lazer.SetActive(false);

        topAttackLazer.SetActive(false);
		Debug.Log ("Laser Out");
		//this trigger will close the mouth.
		animator.SetTrigger ("laserOut");
		yield return new WaitForSeconds(laserCloseTimeAnim);

    }
    private IEnumerator AttackTopSide()
    {
		if (animator.GetBool ("missileOpen")) {
			topAttackConveyance.SetActive (true);
			yield return new WaitForSeconds (missileDelayWarning);
			topAttackConveyance.SetActive (false);

			for (int i = 0; i < numShurikensPerAttack; i++) {
				animator.SetInteger ("shots", controller.boss1Shot++);
				animator.SetTrigger ("attacking");
				yield return new WaitForSeconds (missileShootTimeAnim);
				GameObject newShuriken = Instantiate (enemySineProjectile, topAttackPosition.transform.position, Quaternion.identity);
				if (i == 1) {
					newShuriken.GetComponent<enemySineProjectile> ().startSineVelocity ();
					//this is the amount of shots the first boss can make.
					if (controller.boss1Shot >= 5) {
						controller.boss1Shot = 0;
						animator.SetInteger ("shots", controller.boss1Shot);
						//animator.SetBool ("missileOpen", false);
						animator.SetTrigger ("missileToggle");
						Debug.Log ("trigged missile closing");
						break;
					}
				}
				yield return new WaitForSeconds (missileDelayBetweenShots);
			}
		}
    }

    private IEnumerator AttackBottomSide()
    {
		if (animator.GetBool ("missileOpen")) {
			bottomAttackConveyance.SetActive (true);
			yield return new WaitForSeconds (missileDelayWarning);
			bottomAttackConveyance.SetActive (false);

			for (int i = 0; i < numShurikensPerAttack; i++) {
				animator.SetInteger ("shots", controller.boss1Shot++);
				animator.SetTrigger ("attacking");
				yield return new WaitForSeconds (missileShootTimeAnim);
				GameObject newShuriken = Instantiate (enemySineProjectile, bottomAttackPosition.transform.position, Quaternion.identity);
				if (i == 0 || i == 2) {
					newShuriken.GetComponent<enemySineProjectile> ().startSineVelocity ();
					animator.SetInteger ("shots", controller.boss1Shot++);
					//this is the amount of shots the first boss can make.
					if (controller.boss1Shot >= 5) {
						Debug.Log ("trigged missile closing");
						controller.boss1Shot = 0;
						animator.SetInteger ("shots", controller.boss1Shot);
						//animator.SetBool ("missileOpen", false);
						animator.SetTrigger ("missileToggle");
						break;
					}
				}
				yield return new WaitForSeconds (missileDelayBetweenShots);
			}
		}
    }

	void OnDisable()
	{
		topAttackConveyance.gameObject.SetActive(false);
		bottomAttackConveyance.gameObject.SetActive(false);
	}
	void OnEnable()
	{
		topAttackConveyance.gameObject.SetActive(false);
		bottomAttackConveyance.gameObject.SetActive(false);
	}
}
