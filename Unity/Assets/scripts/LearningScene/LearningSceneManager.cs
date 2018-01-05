using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LearningSceneManager : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    void Start()
    {
        Time.timeScale = .1f;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.C))
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
        }
        if (Input.GetKeyDown(KeyCode.Equals))
            if (Time.timeScale == 1) Time.timeScale = 10;
            else
                Time.timeScale += 10;
        if (Input.GetKeyDown(KeyCode.Minus))
            if (Time.timeScale == 1) Time.timeScale = 0;
            else
                Time.timeScale = Math.Max(Time.timeScale - 10, 1);
    }

    void OnGUI()
    {
        string text = "\n    Controls\n\n" +
                      "      +\tquicker\n" +
                      "      -\tslower\n" +
                      "      C\tchange camera\n" +
                      "      R\trestart\n" +
                      "      Esc\tclose\n\n" +
                      "    Speed: " + Math.Round(Time.timeScale, 1);
        GUI.TextField(new Rect(10, 10, 200, 170), text);
    }
}