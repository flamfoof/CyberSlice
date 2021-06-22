using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove : MonoBehaviour {

	public float moveSpeed = 0.1f;
	public float yRange = 0.0f;
	private float origMoveSpeed = 0.1f;
	Animator anim;
	Vector3 vel;
	public float targetVariationDist;
	public float timeBeforeMoving;
	public GameObject lerpLocation;
	public bool stopLerping = false;
	public GameObject lilCollider;
	public float speedMult = 1.0f;
	public GameObject shooterDetector;
	public bool triggerActive = false;
	public float alphaDepth;

	public AudioClip enemyHitSFX;
	public AudioClip explodeSFX;
	AudioSource _audio;

    public bool isBird = false;
    public Sprite secondBirdSprite;
    public GameObject explosion;
    private bool birdOfPreyMode = false;
    private Vector3 attackPosition;
	public float projectileSpeed;
	public Rigidbody2D enemyBullet;

	// Use this for initialization
	void Start () {
		targetVariationDist = Random.Range (-1.0f, 4.0f);
		timeBeforeMoving = Random.Range (3.0f, 7.0f);
		origMoveSpeed = moveSpeed;
		anim = GetComponent<Animator> ();

		_audio = GetComponent<AudioSource> ();
		if (_audio == null) {
//			Debug.LogWarning ("AudioSource component missing from this game object. Adding one.");
			_audio = gameObject.AddComponent <AudioSource> ();
		}
	}
	void PlaySound(AudioClip clip, float volume){
		_audio.PlayOneShot (clip, volume);
	}

	void FixedUpdate () {
        if (!birdOfPreyMode)
        {
            if (!stopLerping)
            {
                lerpLocation = GameObject.FindGameObjectWithTag("target");
                //Debug.Log(lerpLocation.transform.position);
                if (lerpLocation != null)
                {
                    vel = new Vector3(lerpLocation.transform.position.x + targetVariationDist, transform.position.y, 0);
                    if (lerpLocation.transform.position.x > transform.position.x)
                    {
                        moveSpeed = origMoveSpeed * 9 * speedMult;
                    }
                    gameObject.transform.position = Vector3.Lerp(transform.position, vel, Time.deltaTime * moveSpeed);
                }

            }
            else
            {
                vel = new Vector3(this.gameObject.transform.position.x - moveSpeed, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                //gameObject.GetComponent<Rigidbody2D> ().velocity;
                gameObject.transform.position = vel;
            }
            if (lilCollider == null)
            {
                Destroy(gameObject);
            }
        }
		else
        {
            birdLerpToPlayer();
        }
	}

    public void birdLerpToPlayer()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, attackPosition, Time.deltaTime * 6.0f);
    }
	public void UpdateEnemySpeed(float num)
	{
		moveSpeed = origMoveSpeed * num;
		speedMult = num;
	}

	public void DestroyMe()
	{
        if (isBird)
        {

            PlaySound(enemyHitSFX, 0.2f);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //StartCoroutine(EnemyFlashAlpha());
            PlaySound(explodeSFX, 0.2f);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            GameObject newExplosion = Instantiate(explosion);
            newExplosion.transform.position = this.transform.position;

            Destroy(this.gameObject);
        }
        else
        {
            anim.SetTrigger("isHit");
            gameObject.GetComponent<enemyProjectile>().stillAlive = false;
            PlaySound(enemyHitSFX, 0.2f);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(EnemyFlashAlpha());
            PlaySound(explodeSFX, 0.2f);
        }
		
	}

	public void Dead()
	{
		
		Destroy (gameObject);

	}
	public void TurnMoveNormal()
	{
		//Debug.Log ("WOOT SOMETHING");
		StartCoroutine (MoveNormal ());
		if (!isBird) {

			Rigidbody2D enemyProjectile = Instantiate (enemyBullet, transform.position, Quaternion.identity);
			enemyProjectile.velocity = new Vector2 (-projectileSpeed, 0);
		}
	}
	public IEnumerator MoveNormal()
	{
		//Debug.Log ("startng to move normally in: " + timeBeforeMoving);
		yield return new WaitForSeconds (timeBeforeMoving);

        if (isBird) {
            //UpdateEnemySpeed(0.15f);
            birdOfPreyMode = true;
            attackPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;
            attackPosition.x += 7.0f;
            
            this.GetComponent<SpriteRenderer>().sprite = secondBirdSprite;
        }
        else
        {
            UpdateEnemySpeed(-speedMult);
        }
		
		stopLerping = true;
			
	}

	private IEnumerator EnemyFlashAlpha(){
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
