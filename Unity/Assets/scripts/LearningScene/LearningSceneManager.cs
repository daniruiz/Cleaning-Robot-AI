using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LearningSceneManager : MonoBehaviour
{
    private const string version = "2c-10";

    public GameObject robotInstance;
    private Robot actualRobot;
    private int generation = 0;
    private int robotNum = 0;

    private bool demoDNAInserted = false;
    private string demoDNA = "[0.4363345, -0.5431719, 0.2297739, -0.658473, -0.2446964, 0.3844358, -0.1956752, 0.1611564, 0.4958922, -0.6672357, 0.6204656, 0.05922021, -0.4470385, 0.7832506, 0.3850292, 0.8542311, 0.8413686, 0.8053117, 0.6160218, 0.3537445, 0.7903312, -0.9117945, -0.2635008, -0.09289524, -0.6760907, 0.3696324, -0.1246518, 0.7908856, 0.7384198, 0.8473799, 0.395005, -0.5411443, 0.3428722, -0.9319425, 0.2725643, -0.3136321, -0.3365454, -0.1056552, -0.7517362, -0.5754653, -0.7974565, 0.03574792, -0.7679306, -0.4637659, 0.4650282, -0.7894409, 0.1822473, 0.7879444, 0.140718, 0.1791399, -0.006390057, -0.0975039, 0.7037268, 0.4193246, -0.9599906, 0.22688, 0.4085247, -0.341151, -0.7713933, 0.2422128, 0.05274469, 0.1385842, -0.5073628, 0.9745129, -0.7284337, -0.5439279, 0.3017705, 0.9228482, 0.6180493, 0.7664464, 0.1211045, 0.4423186, -0.5820866, -0.7969351, -0.7763225, 0.6312989, -0.3679987, 0.0857686, -0.8380955, -0.7600968, -0.9002668, -0.6694614, 0.7591009, 0.419746, 0.6217234, -0.4452468, 0.2882189, 0.4205462, -0.8906037, -0.159189, -0.5475469, 0.6684933, -0.4845657, -0.4446121, -0.4261874, -0.9698882, 0.2196836, -0.6477933, -0.2295745, 0.2208165, -0.995289, -0.1839911, -0.004038941, -0.04290744, -0.6261371, 0.8838712, -0.484624, -0.5205777, -0.5384502, 0.7750328, -0.4820544, 0.5812501, 0.6750723, -0.126666, -0.9164541, -0.4295324, -0.01912924, -0.4051234, -0.9592842, -0.6441196]";
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
        if (Input.GetKeyUp(KeyCode.R)) {
            demoDNAInserted = true;
            bestRobotDNA = null;
            bestRobotFitness = 0;
            generation = 0;
            robotNum = 0;
            actualRobot.Die();
        }
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

        if (!demoDNAInserted)
        {
            InsertDNA(demoDNA);
            demoDNAInserted = true;
        } else if (generation > 1)
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
        if (robotNum > 0) {
            actualGenerationDNAs[robotNum - 1] = new robotInfo(fitness, DNA);
            if (fitness > bestRobotFitness)
            {
                bestRobotFitness = fitness;
                bestRobotDNA = DNA;

            }
        }
        AddRobot();
    }
}
