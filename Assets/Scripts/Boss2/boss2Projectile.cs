using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2Projectile : MonoBehaviour {

	public float speed = 3.0f;
	public float step;
	public bool fist;
	public bool bulletThing;

	// Use this for initialization
	void Start () {
		step = speed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D> ().AddForce (-transform.right * step);
	}
}
