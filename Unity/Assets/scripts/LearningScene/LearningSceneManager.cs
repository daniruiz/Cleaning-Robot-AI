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

    private float[] bestRobotDNA;
    private int bestRobotFitness = 0;


    private string DNATextArea = "New DNA";
    private bool insertDNAButton = false;


    private struct robotInfo
    {
        public int fitness;
        public float[] DNA;

        public robotInfo(int fitness, float[] DNA)
        {
            this.fitness = fitness;
            this.DNA = DNA;
        }
    }

    private robotInfo[] oldGenerationDNAs = new robotInfo[10];
    private robotInfo[] actualGenerationDNAs = new robotInfo[10];

    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif

        Time.timeScale = 5;
        NewGeneration();
    }

#if !UNITY_EDITOR && UNITY_WEBGL
    public void captureKeyboard(int i)
    {
        WebGLInput.captureAllKeyboardInput = (i == 1 ? true : false);
    }
#endif

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
            string bestDNAString = Miscellaneous.ArrayToString<float>(bestRobotDNA);
#if !UNITY_EDITOR && UNITY_WEBGL
        Application.ExternalCall("copyBestDNA", bestDNAString);
#endif
            TextEditor te = new TextEditor();
            te.content = new GUIContent(bestDNAString);
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
                      "      C\tCopy best DNA to clipboard\n" +
                      "      Del\tKill actual robot\n" +
                      "      R\tRestart\n" +
#if UNITY_EDITOR && !UNITY_WEBGL
                      "      Esc\tClose\n" +
#endif
                      "\n" +
                      "    Speed: " + Math.Round(Time.timeScale, 1) + "\n\n" +
                      "    Actual robot Fitness: " + actualRobot.GetFitness() + "\n" +
                      "    Actual robot Max. Fitness: " + actualRobot.GetMaxFitness() + "\n\n" +
                      "    Max. Fitness: " + bestRobotFitness + "\n\n" +
                      "    Generation: " + generation + "\n" +
                      "    Robot Num.: " + robotNum;
        GUI.TextField(new Rect(10, 10, 260, 340), text);
#if UNITY_EDITOR && !UNITY_WEBGL
        DNATextArea = GUI.TextArea(new Rect(10, 380, 260, 100), DNATextArea);
        insertDNAButton = GUI.Button(new Rect(190, 490, 80, 30), "Insert DNA");
#endif
        if (insertDNAButton)
        {
            InsertDNA(DNATextArea);
            DNATextArea = "New DNA";
        }
#if UNITY_EDITOR && !UNITY_WEBGL
        string robotSensors = actualRobot.GetSensorsString();
        GUI.TextField(new Rect(280, 10, 400, 30), robotSensors);
#endif
    }

    public void InsertDNA(string DNAString)
    {
        Miscellaneous.SetFloatStringFormat();

        float[] DNA = Array.ConvertAll(DNAString.Substring(1, DNAString.Length - 2).Split(','), float.Parse);
        for (int i = 0; i < actualGenerationDNAs.Length; i++)
            actualGenerationDNAs[i].DNA = DNA;
        robotNum = 10;
        actualRobot.SetDNA(DNA);
    }

    private void NewGeneration()
    {
        oldGenerationDNAs = actualGenerationDNAs;
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
            actualRobot.SetDNA(GenerateSonDNA());
        else
            actualRobot.SetDNA();
    }

    private float[] GenerateSonDNA()
    {
        int totalFitnessSum = 0;
        foreach (robotInfo info in oldGenerationDNAs)
            totalFitnessSum += info.fitness;
        System.Random random = new System.Random();
        int randomPos1 = random.Next(1, totalFitnessSum);
        int randomPos2 = random.Next(1, totalFitnessSum);

        float[] parent1DNA = null;
        float[] parent2DNA = null;
        int fitnessCounter = 0;
        for (int i = 0; i < oldGenerationDNAs.Length; i++)
        {
            fitnessCounter += oldGenerationDNAs[i].fitness;
            if (parent1DNA == null && randomPos1 <= fitnessCounter)
                parent1DNA = oldGenerationDNAs[i].DNA;
            if (parent2DNA == null && randomPos2 <= fitnessCounter)
                parent2DNA = oldGenerationDNAs[i].DNA;
        }

        float[] finalDNA = new float[parent1DNA.Length];
        for (int i = 0; i < parent1DNA.Length; i++)
            finalDNA[i] = (i % 2 == 0 ? parent1DNA[i] : parent2DNA[i]);

        return Mutate(finalDNA);
    }

    private float[] Mutate(float[] DNA)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < DNA.Length; i++)
            if (random.Next(1, DNA.Length) == 1)
                DNA[i] = (float)(random.NextDouble()) * 2.0f - 1.0f;
        return DNA;
    }

    public int GetBestRobotFitness()
    {
        return bestRobotFitness;
    }

    public void RobotDied(float[] DNA, int fitness)
    {
        actualGenerationDNAs[robotNum - 1] = new robotInfo(fitness, DNA);
        if (fitness > bestRobotFitness)
        {
            bestRobotFitness = fitness;
            bestRobotDNA = DNA;

        }
        AddRobot();
    }
}
