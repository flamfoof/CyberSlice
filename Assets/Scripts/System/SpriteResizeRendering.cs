using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteResizeRendering : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
	void Awake() {
		SpriteRenderer render = GetComponent<SpriteRenderer>();

		float worldScreenHeight = Camera.main.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		transform.localScale = new Vector3 (worldScreenWidth / render.sprite.bounds.size.x, worldScreenHeight / render.sprite.bounds.size.y, 1);
	}

}

