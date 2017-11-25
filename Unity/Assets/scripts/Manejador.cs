using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Manejador : MonoBehaviour
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

		if (Input.GetKeyDown (KeyCode.Plus))
			Time.timeScale += .5f;
		if (Input.GetKeyDown (KeyCode.Minus))
			Time.timeScale = Math.Max (Time.timeScale-.5f, 0);
	}

	void OnGUI ()
	{
		string texto = "" +
		               "    Controles\n\n" +
		               "      +\tmás rápido\n" +
		               "      -\tmás lento\n" +
		               "      c\tcambiar cámara\n" +
		               "      r\treiniciar\n\n" +
		               "    Velocidad: " + Math.Round (Time.timeScale, 1) + "\n";
		GUI.TextField (new Rect (10, 10, 200, 140), texto);
	}
}	