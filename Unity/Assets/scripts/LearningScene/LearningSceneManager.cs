using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LearningSceneManager : MonoBehaviour
{
    public GameObject robot;
    private int generation = 0;
    private int robotNum = 0;

    void Start()
    {
        Time.timeScale = 10;
        NewGeneration();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                      "      +\tfaster\n" +
                      "      -\tslower\n" +
                      "      R\trestart\n" +
                      "      Esc\tclose\n\n" +
                      "    Speed: " + Math.Round(Time.timeScale, 1) + "\n\n" +
                      "    Generation: " + generation + "\n" +
                      "    Robot Num.: " + robotNum;
        GUI.TextField(new Rect(10, 10, 200, 370), text);
    }

    private void NewGeneration()
    {
        AddRobot();
        generation++;
    }

    private void AddRobot()
    {
        Instantiate(robot);
        robotNum++;
    }

    public void RobotDied(float[] ADN, float fitness)
    {
        Debug.Log(arrayToString<float>(ADN));
        Debug.Log(fitness);
        AddRobot();
    }

    private String arrayToString<T>(T[] array) {
        String s = "";
        foreach (T val in array)
            s += val + ", "; 
        return s;
    }
}