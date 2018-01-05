using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
	public Camera cam1;
	public Camera cam2;

	void Start ()
	{
		Time.timeScale = 1;
		cam1.enabled = false;
		cam2.enabled = true;
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape))
			Application.Quit ();
		if (Input.GetKeyDown (KeyCode.C)) {
			cam1.enabled = !cam1.enabled;
			cam2.enabled = !cam2.enabled;
		}
		if (Input.GetKeyDown (KeyCode.R))
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

        if (Input.GetKeyDown(KeyCode.Equals)) 
            Time.timeScale += .5f;
			
		if (Input.GetKeyDown (KeyCode.Minus))
			Time.timeScale = Math.Max (Time.timeScale - .5f, 0);
	}

	void OnGUI ()
	{
		string text = "\n    Controls\n\n" +
		              "      +\tquicker\n" +
		              "      -\tslower\n" +
		              "      C\tchange camera\n" +
		              "      R\trestart\n" +
		              "      Esc\tclose\n\n" +
		              "    Speed: " + Math.Round (Time.timeScale, 1);
		GUI.TextField (new Rect (10, 10, 200, 170), text);
	}
}	