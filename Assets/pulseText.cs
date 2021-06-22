using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pulseText : MonoBehaviour {

	private Text restartText;

	public int minFont;	
	public int maxFont;

	public 
	// Use this for initialization
	void Start () {
		restartText = this.GetComponent<Text> ();
		StartCoroutine (pulseSizeFont (minFont, maxFont));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator pulseSizeFont(int minFont, int maxFont){
        while (true)
        {
            restartText.fontSize = minFont;
            for (int i = 0; i < 30; i++)
            {
                restartText.fontSize = minFont + i;
                yield return new WaitForSeconds(0.015f);
            }
            yield return new WaitForSeconds(0.075f);
            for (int i = minFont + 30; i >= minFont; i--)
            {
                restartText.fontSize = i;
                yield return new WaitForSeconds(0.015f);
            }
            yield return new WaitForSeconds(0.075f);
        }
	}
}
