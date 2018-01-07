using UnityEngine;
using System;
using System.Collections.Generic;

public class Robot : MonoBehaviour
{
    private LearningSceneManager manager;

    private float speed;
    private float rotationSpeed;

    private Dictionary<string, bool> sensors = new Dictionary<string, bool>();

    private float absoluteRotationDeg = 0.0f;
    private float robotWidth;
    private bool collision = false;
    private CleanedSpaceMap map;
    private Perceptron brain;

    void Awake()
    {
        manager = GameObject.Find("Manager").GetComponent<LearningSceneManager>();


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
        robotWidth = GetComponent<Renderer>().bounds.size.x;
        map = new CleanedSpaceMap(robotWidth);
        brain = new Perceptron();
    }

    void Update()
    {
        sensors["Left Grid Cleaned"] = IsLeftGridCleaned();
        sensors["Front Grid Cleaned"] = IsFrontGridCleaned();
        sensors["Right Grid Cleaned"] = IsRightGridCleaned();

        String s = "";
        s += sensors["Left Grid Cleaned"] ? "<" : " ";
        s += sensors["Front Grid Cleaned"] ? "^" : " ";
        s += sensors["Right Grid Cleaned"] ? ">" : " ";
        Debug.Log(s);


        bool[] sensorsArray = new bool[sensors.Count];
        sensors.Values.CopyTo(sensorsArray, 0);

        float[] outputValues = brain.Guess(sensorsArray);

        speed = outputValues[0];
        rotationSpeed = outputValues[1] * 2 - 1;

        Rotate();
        Move();
    }

    private bool IsLeftGridCleaned()
    {
        float x = map.GetPlayerPosition().x + (robotWidth * 1.4f * (float)Math.Sin((Math.PI / 180) * (absoluteRotationDeg - 90)));
        float y = map.GetPlayerPosition().y + (robotWidth * 1.4f * (float)Math.Cos((Math.PI / 180) * (absoluteRotationDeg - 90)));
        return map.IsGridCleaned(new Vector2(x, y));
    }
    private bool IsFrontGridCleaned()
    {
        float x = map.GetPlayerPosition().x + (robotWidth * 1.4f * (float)Math.Sin((Math.PI / 180) * absoluteRotationDeg));
        float y = map.GetPlayerPosition().y + (robotWidth * 1.4f * (float)Math.Cos((Math.PI / 180) * absoluteRotationDeg));
        return map.IsGridCleaned(new Vector2(x, y));
    }
    private bool IsRightGridCleaned()
    {
        float x = map.GetPlayerPosition().x + (robotWidth * 1.4f * (float)Math.Sin((Math.PI / 180) * (absoluteRotationDeg + 90)));
        float y = map.GetPlayerPosition().y + (robotWidth * 1.4f * (float)Math.Cos((Math.PI / 180) * (absoluteRotationDeg + 90)));
        return map.IsGridCleaned(new Vector2(x, y));
    }

    private void Move()
    {
        float displacement = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * displacement);

        float xAxisDisplacement = displacement * (float)Math.Sin((Math.PI / 180) * absoluteRotationDeg);
        float yAxisDisplacement = displacement * (float)Math.Cos((Math.PI / 180) * absoluteRotationDeg);

        map.MovePlayerPosition(new Vector2(xAxisDisplacement, yAxisDisplacement));
    }

    private void Rotate()
    {
        float rotation = rotationSpeed * Time.deltaTime * 60;
        transform.Rotate(new Vector3(0, rotation, 0));
        absoluteRotationDeg = (absoluteRotationDeg + rotation) % 360;
    }

    void OnCollisionEnter()
    {
        if (collision) return;
        collision = true;
        float[] ADN = brain.GetPerceptronADN();
        int fitness = map.GetNumCleanedPositions();
        manager.RobotDied(ADN, fitness);
        Destroy(gameObject);
    }


    public void UpdateSensorState(string name, bool value)
    {
        sensors[name] = value;
    }
}
