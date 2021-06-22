using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenuStop : MonoBehaviour {
	public GameObject targetMenu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		targetMenu.GetComponent<ButtonSelection> ().enabled = false;
		targetMenu.GetComponent<ButtonSelection> ().size = 0;
	}

	void OnDisable()
	{
		targetMenu.GetComponent<ButtonSelection> ().enabled = true;
		targetMenu.GetComponent<ButtonSelection> ().size = 3;
	}
}
