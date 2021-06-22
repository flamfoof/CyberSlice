using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitThenDestroy : MonoBehaviour {

    public float secondsTillDestroy;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitThenDestroyThis());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator WaitThenDestroyThis()
    {
        yield return new WaitForSeconds(secondsTillDestroy);
        Destroy(this.gameObject);
    }
}
