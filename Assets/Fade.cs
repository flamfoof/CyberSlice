using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
	public GameObject Opening;



	// Use this for initialization
	void Start () {
		StartCoroutine (DoFade());
		
	}
	IEnumerator DoFade(){
		yield return new WaitForSeconds (13.0f);
		CanvasGroup CanvasGroup = GetComponent<CanvasGroup> ();

		for (float i = 0f; i < 1.0f; i += .05f){
			yield return new WaitForSeconds (0.05f);
			CanvasGroup.alpha = i;

		}

	}
	
	// Update is called once per frame
	void Update () {
		

}
}
