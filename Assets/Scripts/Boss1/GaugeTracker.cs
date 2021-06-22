using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeTracker : MonoBehaviour {

	public Slider bossGauge;

	// Use this for initialization
	void Start () {
		bossGauge.onValueChanged.AddListener (delegate {
			Debug.Log(bossGauge.value);
		});
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T))
		{
			bossGauge.value += 1.0f;
		}
	}
}
