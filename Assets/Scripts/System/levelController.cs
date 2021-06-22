using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelController : MonoBehaviour {

	public bool isPaused = false;

	public GameObject PauseMenu;
    public GameObject ControlsMenu;
	void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
		{
			Pause ();
		}
	}
	public void Pause()
	{
		if (PauseMenu != null) {
			isPaused = !isPaused;
			if (isPaused) {
				Time.timeScale = 0;
				PauseMenu.SetActive (true);
			} else {
				Time.timeScale = 1;
				PauseMenu.SetActive (false);
			}

		}
	}
    public void Controls()
    {
        if (ControlsMenu != null)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                ControlsMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                ControlsMenu.SetActive(false);
            }

        }
    }


    public void Resume()
	{
		isPaused = !isPaused;
		Time.timeScale = 1;
		PauseMenu.SetActive (false);
        ControlsMenu.SetActive(false);
    }
		

	public void Quit()
	{
		Application.Quit();
	}


	public void ChangeScene(int scene)
	{
		SceneManager.LoadScene(scene);
		if (isPaused) {
			Resume ();
		}
	}

	public void toggleObject(GameObject obj)
	{
		obj.SetActive(!obj.activeInHierarchy);
	}
}
