using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMover : MonoBehaviour {


    public Camera mainCamera;
    private Vector3 cameraStartPos;
	public float cameraSpeed = 10.0f;
	private float originalSpeed;

	public bool playerDead = false;
	// Use this for initialization
	void Start () {
        cameraStartPos = mainCamera.transform.position;
		originalSpeed = cameraSpeed;

	}
	
	// Update is called once per frame
	void Update () {
		if (playerDead == false) {
			mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + (cameraSpeed * Time.deltaTime), cameraStartPos.y, cameraStartPos.z);
		}
        
	}

	public void UpdateSpeed(float num)
	{
		cameraSpeed = originalSpeed * num;
	}
}
