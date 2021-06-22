using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectileLife : MonoBehaviour {

	//duration which the ammo will last in seconds
	public float ammoLife = 10;

	// Use this for initialization
	void Start () {
		//projectile disappears after # seconds
		Destroy (gameObject, ammoLife);
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (-0.2f, 0, 0);
	}
}
