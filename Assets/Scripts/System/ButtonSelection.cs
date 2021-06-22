using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelection : MonoBehaviour {

	public List<GameObject> buttons;
	int selection = 0;
	public int size = 0;
	bool pressed = false;
	bool onSlider = false;
	bool horizMoved = false;

	// Use this for initialization
	void Start () {
		size = buttons.Count;
		//direction = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMove>();
		//buttons [selection].Select ();
		Debug.Log ("STARTING HOOO");
	}
	
	// Update is called once per frame
	void Update () {

		float dirX = Input.GetAxisRaw ("Horizontal");
		float dirY = Input.GetAxisRaw ("Vertical");
		if (dirX != 0) {
			horizMoved = true;
		} else {
			horizMoved = false;
		}

		if ((dirY != 0 && dirX == 0) || (dirY == 0 && dirX != 0)) {
			
			if ((dirY == 1.0f || dirX == 1.0f) && !pressed) {
				selection--;
				if (selection <= -1) {
					selection = size - 1;
				}
				selection = Mathf.Abs (selection % size);
				Debug.Log (selection % size);
				if (onSlider && horizMoved) {
					selection++;
				} else {
					SelectIt (selection);
				}
			} else if ((dirY == -1.0f || dirX == -1.0f) && !pressed) {
				selection++;
				selection = Mathf.Abs (selection % size);
				Debug.Log (selection % size);
				if (onSlider && horizMoved) {
					selection--;

				} else {
					SelectIt (selection);
				}
			}
			pressed = true;
		} else {
			pressed = false;
		}

	}

	void SelectIt(int num)
	{
		if (buttons [num].GetComponent ("Button") != null) {
			onSlider = false;
			buttons [num].GetComponent<Button> ().Select ();
		} else if (buttons [num].GetComponent ("Slider") != null) {
			onSlider = true;
			buttons [num].GetComponent<Slider> ().Select ();
		}
	}

	void OnEnable()
	{
		Debug.Log ("first button is selected");
		selection = 0;
		SelectIt (0);
		StartCoroutine (SelectFirstButton ());
	}
	IEnumerator SelectFirstButton()
	{
		yield return new WaitForFixedUpdate();
		if (buttons [0].GetComponent ("Button") != null) {
			buttons [0].GetComponent<Button> ().OnSelect (null);
			buttons [0].GetComponent<Button> ().Select ();
		} else if (buttons [0].GetComponent ("Slider") != null) {
			buttons [0].GetComponent<Slider> ().OnSelect (null);
			buttons [0].GetComponent<Slider> ().Select ();
		}

	}

}
