using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			GetComponent<BoxCollider2D> ().enabled = !GetComponent<BoxCollider2D> ().enabled;
		}
	}
}
