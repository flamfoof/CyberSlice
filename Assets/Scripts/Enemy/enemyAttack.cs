using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour {
	public Rigidbody2D enemyBullet;
	public float projectileSpeed = 5.0f;
	public float fireRate = 10.0f;
	public float shootNow = 0.0f;
	public float rangeToShoot = 5.0f;
	public float minTimeToShoot = 3.0f;
	public float maxTimeToShoot = 7.0f;
	public bool stillAlive = true;
	public bool bird;


	public bool enteringAttackRange = false;


	void Start()
	{
		shootNow = Random.Range (minTimeToShoot, maxTimeToShoot);
		//StartCoroutine (ShootPlayah ());

	}

	IEnumerator ShootPlayer()
	{
		while (enteringAttackRange) {
			yield return new WaitForSeconds (shootNow);


			if (Time.time > shootNow) {
				//shootNow is the delay between each fire
				shootNow = Time.time + fireRate;

				//			Debug.Log ("SHOOTING!!");/*
				if (stillAlive) {

						Debug.Log ("Enemy minion threw a dagger thing");
						Rigidbody2D enemyProjectile = Instantiate (enemyBullet, transform.position, Quaternion.identity);
						enemyProjectile.velocity = new Vector2 (projectileSpeed, 0);
				}
			}
		}

	}


	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Detector") {
			Debug.Log ("Touched detector");
			StartCoroutine(ShootPlayer());
			enteringAttackRange = true;
		}
	}
}
