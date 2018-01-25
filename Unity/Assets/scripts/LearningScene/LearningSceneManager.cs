using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LearningSceneManager : MonoBehaviour
{
    private const string version = "2c-8";

    public GameObject robotInstance;
    private Robot actualRobot;
    private int generation = 0;
    private int robotNum = 0;

    private float[] bestRobotADN;
    private int bestRobotFitness = 0;


    private string ADNTextArea = "New ADN";
    private bool insertADNButton = false;


    private struct robotInfo
    {
        public int fitness;
        public float[] ADN;

        public robotInfo(int fitness, float[] ADN)
        {
            this.fitness = fitness;
            this.ADN = ADN;
        }
    }

    private robotInfo[] oldGenerationADNs = new robotInfo[10];
    private robotInfo[] actualGenerationADNs = new robotInfo[10];

    void Start()
    {
        Time.timeScale = 5;
        NewGeneration();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.Delete))
            actualRobot.Die();
        if (Input.GetKeyDown(KeyCode.C))
        {
            TextEditor te = new TextEditor();
            te.content = new GUIContent(
                Miscellaneous.ArrayToString<float>(bestRobotADN));
            te.SelectAll();
            te.Copy();
        }
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
            Time.timeScale = Math.Min(Time.timeScale + 1, 15);
        if (Input.GetKeyDown(KeyCode.Minus))
            Time.timeScale = Math.Max(Time.timeScale - 1, 0);
    }

    void OnGUI()
    {
        string text = "\n    Version: " + version + "\n\n" +
                      "\n    Controls\n\n" +
                      "      +\tFaster\n" +
                      "      -\tSlower\n" +
                      "      C\tCopy best ADN to clipboard\n" +
                      "      Del\tKill actual robot\n" +
                      "      R\tRestart\n" +
                      "      Esc\tClose\n\n" +
                      "    Speed: " + Math.Round(Time.timeScale, 1) + "\n\n" +
                      "    Actual robot Fitness: " + actualRobot.GetFitness() + "\n" +
                      "    Actual robot Max. Fitness: " + actualRobot.GetMaxFitness() + "\n\n" +
                      "    Max. Fitness: " + bestRobotFitness + "\n\n" +
                      "    Generation: " + generation + "\n" +
                      "    Robot Num.: " + robotNum;
        GUI.TextField(new Rect(10, 10, 260, 340), text);
        ADNTextArea = GUI.TextArea(new Rect(10, 380, 260, 100), ADNTextArea);
        insertADNButton = GUI.Button(new Rect(190, 490, 80, 30), "Insert ADN");
        if (insertADNButton)
        {
            InsertADN(ADNTextArea);
            ADNTextArea = "New ADN";
        }

        string robotSensors = actualRobot.GetSensorsString();
        GUI.TextField(new Rect(280, 10, 400, 30), robotSensors);
    }

    public void InsertADN(string ADNString)
    {
        Miscellaneous.SetFloatStringFormat();

        float[] ADN = Array.ConvertAll(ADNString.Substring(1, ADNString.Length - 2).Split(','), float.Parse);
        for (int i = 0; i < actualGenerationADNs.Length; i++)
            actualGenerationADNs[i].ADN = ADN;
        robotNum = 10;
        actualRobot.SetADN(ADN);
    }

    private void NewGeneration()
    {
        oldGenerationADNs = actualGenerationADNs;
        robotNum = 0;
        generation++;
        AddRobot();
    }

    private void AddRobot()
    {
        if (robotNum == 10)
        {
            NewGeneration();
            return;
        }

        actualRobot = Instantiate(robotInstance).GetComponent<Robot>();
        robotNum++;

        if (generation > 1)
            actualRobot.SetADN(GenerateSonADN());
        else
            actualRobot.SetADN();
    }

    private float[] GenerateSonADN()
    {
        int totalFitnessSum = 0;
        foreach (robotInfo info in oldGenerationADNs)
            totalFitnessSum += info.fitness;
        System.Random random = new System.Random();
        int randomPos1 = random.Next(1, totalFitnessSum);
        int randomPos2 = random.Next(1, totalFitnessSum);

        float[] parent1ADN = null;
        float[] parent2ADN = null;
        int fitnessCounter = 0;
        for (int i = 0; i < oldGenerationADNs.Length; i++)
        {
            fitnessCounter += oldGenerationADNs[i].fitness;
            if (parent1ADN == null && randomPos1 <= fitnessCounter)
                parent1ADN = oldGenerationADNs[i].ADN;
            if (parent2ADN == null && randomPos2 <= fitnessCounter)
                parent2ADN = oldGenerationADNs[i].ADN;
        }

        float[] finalADN = new float[parent1ADN.Length];
        for (int i = 0; i < parent1ADN.Length; i++)
            finalADN[i] = (i % 2 == 0 ? parent1ADN[i] : parent2ADN[i]);

        return Mutate(finalADN);
    }

    private float[] Mutate(float[] ADN)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < ADN.Length; i++)
            if (random.Next(1, ADN.Length) == 1)
                ADN[i] = (float)(random.NextDouble()) * 2.0f - 1.0f;
        return ADN;
    }

    public int GetBestRobotFitness()
    {
        return bestRobotFitness;
    }

    public void RobotDied(float[] ADN, int fitness)
    {
        actualGenerationADNs[robotNum - 1] = new robotInfo(fitness, ADN);
        if (fitness > bestRobotFitness)
        {
            bestRobotFitness = fitness;
            bestRobotADN = ADN;

        }
        AddRobot();
    }
}
