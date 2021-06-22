using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySineProjectile : MonoBehaviour {

    private float projectileSpeed;

    private float sineWaveFrequency;
    private float sineWaveMagnitude;

    private Vector3 axis;

    private Vector3 pos;

	private Vector3 oldPos;
	private Vector3 newPos;
	private float startTime;

    private bool inverted = false;

    // Use this for initialization
    void Start () {
        
        projectileSpeed = 2.0f;
        sineWaveFrequency = 10.0f;
        sineWaveMagnitude = 1.0f;

        axis = transform.up;
        pos = this.transform.position;
		startTime = Time.time;
		oldPos = pos;
}
	
	// Update is called once per frame
	void Update () {
        if (inverted)
        {
            sineWaveMagnitude = -1.0f;
        }


		pos -= transform.right * Time.deltaTime * projectileSpeed;
		oldPos = transform.position;
		transform.position = pos - axis * Mathf.Sin(Time.time * sineWaveFrequency) * sineWaveMagnitude;
		newPos = transform.position;
		RotateTowards ();
		//transform.Translate (Vector3.up * Mathf.Sin (Time.deltaTime * sineWaveFrequency) * sineWaveMagnitude);


    }

    public void startSineVelocity()
    {
        inverted = true;

    }
		
	public void RotateTowards()
	{
		//Debug.Log ("Old trans: " + oldPos);
		//Debug.Log ("New trans: " + newPos);
		float velocity = -(newPos.y - oldPos.y) / Time.deltaTime;
		//Debug.Log ("vel of sine thing is: " + (newPos.y - oldPos.y) / Time.deltaTime);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, velocity*5));
	}
}
