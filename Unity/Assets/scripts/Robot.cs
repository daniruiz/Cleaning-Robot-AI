using UnityEngine;
using System;
using System.Collections.Generic;

public class Robot : MonoBehaviour
{
    private const float initialSpeed = 4f;
    private float speed = initialSpeed;
    private const float initialRotationSpeed = 180f;
    private float rotationSpeed = initialRotationSpeed;
    private float remainingRotation = 0f;

    private Dictionary<string, bool> sensors = new Dictionary<string, bool>();

    private float absoluteRotationDeg = 0.0f;
    private CleanedSpaceMap map;
    private Perceptron brain = new Perceptron();

    void Awake()
    {
        sensors.Add("Left Sensor", false);
        sensors.Add("Front Sensor", false);
        sensors.Add("Right Sensor", false);
        sensors.Add("Left Button", false);
        sensors.Add("Front Button", false);
        sensors.Add("Right Button", false);
        sensors.Add("Right Wall Sensor", false);

        float[] ADN = brain.getPerceptronADN();
        Debug.Log(arrayToString(ADN));
        Perceptron brain2 = new Perceptron(ADN);
        float[] ADN2 = brain.getPerceptronADN();
        Debug.Log(arrayToString(ADN2));
    }

    private String arrayToString(float[] array)
    {
        String finalString = "(";
        foreach (float val in array)
            finalString += val + ',';
        return finalString + ')';

    }

    private void Start()
    {
        float elementWidth = GetComponent<Renderer>().bounds.size.x;
        map = new CleanedSpaceMap(elementWidth);
    }

    void Update()
    {
        if (!sensors["Left Sensor"] &&
            !sensors["Front Sensor"] &&
            !sensors["Right Sensor"] &&
            !sensors["Right Button"] &&
            !sensors["Left Button"]
            )
        {
            speed = initialSpeed;
            rotationSpeed = initialRotationSpeed;
            remainingRotation = 0;
        }
        else
        {
            if (sensors["Left Button"] ||
                sensors["Front Button"] ||
                sensors["Right Button"])
            {
                speed = 0;
                rotationSpeed = initialRotationSpeed;
                if (remainingRotation == 0)
                {
                    int direction = (sensors["Left Button"] &&
                            !(sensors["Right Wall Sensor"] &&
                            sensors["Front Sensor"])) ? 1 : -1;
                    remainingRotation = 180 * direction;
                }
            }
            else if ((sensors["Left Sensor"] || sensors["Right Sensor"]) &&
                     remainingRotation == 0)
            {
                speed = 1.5f;
                rotationSpeed = 45;
                int direction = (sensors["Left Sensor"] && !(sensors["Right Wall Sensor"] && sensors["Front Sensor"])) ? 1 : -1;
                remainingRotation = 90 * direction;
            }
        }

        Rotate();
        Move();
    }

    private void Move()
    {
        float displacement = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * displacement);

        float xAxisDisplacement = displacement * (float)Math.Cos((Math.PI / 180) * absoluteRotationDeg);
        float yAxisDisplacement = displacement * (float)Math.Sin((Math.PI / 180) * absoluteRotationDeg);

        map.movePlayerPosition(new Vector2(xAxisDisplacement, yAxisDisplacement));
    }

    private void Rotate()
    {
        int direction = remainingRotation > 1 ? 1 : -1;
        float rotate = rotationSpeed * Time.deltaTime * direction;
        if (Math.Abs(remainingRotation) < Math.Abs(rotate))
            rotate = remainingRotation;
        remainingRotation -= rotate;
        absoluteRotationDeg = (absoluteRotationDeg + rotate) % 360;
        transform.Rotate(new Vector3(0, rotate, 0));
    }


    public void updateSensorState(string name, bool value)
    {
        sensors[name] = value;
    }
}
