using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour {
	public Animator anim;
	public bool groundStay = true;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "realGround") {
//			Debug.Log ("grounded");
			anim.SetBool ("Grounded", true);
		}

	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "realGround") {
//			Debug.Log ("ungrounded");
			anim.SetBool ("Grounded", false);
		}
	}
}
