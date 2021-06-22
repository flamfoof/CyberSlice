using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileDestroyer : MonoBehaviour {

	public gameController controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D other)
	{

		Destroy (other.gameObject);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(other.gameObject);
	}

}
