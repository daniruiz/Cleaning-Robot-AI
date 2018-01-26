using UnityEngine;
using System.Collections.Generic;
using System;

public class Robot : MonoBehaviour
{
    private LearningSceneManager manager;

    private float speed;
    private float rotationSpeed;

    private float robotWidth;
    private bool dead = false;
    private int negativeFitness = 0;
    private int maxFitness = 0;

    private Dictionary<string, bool> sensors = new Dictionary<string, bool>();
    private CleanedSpaceMap map;
    private Perceptron brain;

    void Awake()
    {
        sensors.Add("Left Sensor", false);
        sensors.Add("Front Sensor", false);
        sensors.Add("Right Sensor", false);

        sensors.Add("Left Button", false);
        sensors.Add("Front Button", false);
        sensors.Add("Right Button", false);

        sensors.Add("Right Wall Sensor", false);

        sensors.Add("Left Grid Cleaned", false);
        sensors.Add("Front Grid Cleaned", false);
        sensors.Add("Right Grid Cleaned", false);
    }

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<LearningSceneManager>();
        robotWidth = GetComponent<Renderer>().bounds.size.x;
        map = new CleanedSpaceMap(robotWidth);
    }

    void Update()
    {
        if (GetFitness() > maxFitness) maxFitness = GetFitness();


        sensors["Left Grid Cleaned"] = map.IsLeftGridCleaned();
        sensors["Front Grid Cleaned"] = map.IsFrontGridCleaned();
        sensors["Right Grid Cleaned"] = map.IsRightGridCleaned();


        /*** Ask next action to the brain ***/

        bool[] sensorsArray = new bool[sensors.Count];
        sensors.Values.CopyTo(sensorsArray, 0);

        float[] outputValues = brain.Guess(sensorsArray);
        speed = outputValues[0];
        rotationSpeed = outputValues[1] * 2.0f - 1.0f;
        if (0.475f < outputValues[1] && outputValues[1] < 0.525)
        {
            rotationSpeed = 0;
            speed = 1;
        }

        /***  ***/


        /*** Move the robot ***/

        Rotate();
        Move();

        /***  ***/


        if (map.WasActualGridCleaned()) // Kill robot if loop
        {
            if (GetFitness() == 0) Die();
            negativeFitness++;
        }

    }

    public void SetDNA()
    {
        brain = new Perceptron();
    }
    public void SetDNA(float[] DNA)
    {
        Reset(DNA);
    }

    private void Reset(float[] DNA)
    {
        map = new CleanedSpaceMap(robotWidth);
        negativeFitness = 0;
        GameObject.Find("Trail").GetComponent<TrailRenderer>().Clear();
        dead = false;
        brain = new Perceptron(DNA);
    }

    public int GetFitness()
    {
        return map.GetNumCleanedPositions() - negativeFitness;
    }
    public int GetMaxFitness()
    {
        return maxFitness;
    }

    public string GetSensorsString()
    {
        bool[] sensorsArray = new bool[sensors.Count];
        sensors.Values.CopyTo(sensorsArray, 0);
        return Miscellaneous.ArrayToString(sensorsArray);
    }

    private void Move()
    {
        float displacement = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * displacement);
        map.MovePlayerPosition(displacement);
    }
    private void Rotate()
    {
        float rotation = rotationSpeed * Time.deltaTime * 60;
        transform.Rotate(new Vector3(0, rotation, 0));
        map.UpdateDirectionAngle(rotation);
    }

    void OnCollisionEnter()
    {
        Die();
    }
    public void Die()
    {
        if (dead) return;
        dead = true;
        float[] DNA = brain.GetPerceptronDNA();
        manager.RobotDied(DNA, GetMaxFitness());
        Destroy(gameObject);
    }



    public void UpdateSensorState(string name, bool value) // Communication between the sensors and the robot
    {
        sensors[name] = value;
    }
}
