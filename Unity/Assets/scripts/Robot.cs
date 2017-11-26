using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Robot : MonoBehaviour
{
	private const float initialSpeed = 4f;
	private float speed = initialSpeed;
	private const float initialRotationSpeed = 180f;
	private float rotationSpeed = initialRotationSpeed;
	private float remainingRotation = 0f;

	Dictionary<string, bool> sensors = new Dictionary<string, bool> ();

	void Awake ()
	{
		sensors.Add ("Left Sensor", false);
		sensors.Add ("Front Sensor", false);
		sensors.Add ("Right Sensor", false);
		sensors.Add ("Left Button", false);
		sensors.Add ("Front Button", false);
		sensors.Add ("Right Button", false);
		sensors.Add ("Right Wall Sensor", false);
	}

	void Update ()
	{
		if (!sensors ["Left Sensor"] &&
		    !sensors ["Front Sensor"] &&
		    !sensors ["Right Sensor"]) {
			speed = initialSpeed;
			rotationSpeed = initialRotationSpeed;
			remainingRotation = 0;
		} else {
			if (sensors ["Left Button"] ||
			    sensors ["Front Button"] ||
			    sensors ["Right Button"]) {
				speed = 0;
				rotationSpeed = initialRotationSpeed;
				if (remainingRotation == 0) {
					int direction = (sensors ["Left Button"] && !(sensors ["Right Wall Sensor"] && sensors["Front Sensor"])) ? 1 : -1;
					remainingRotation = 180 * direction;
				}
			} else if ((sensors ["Left Sensor"] || sensors ["Right Sensor"]) &&
			           remainingRotation == 0) {
				speed = 1.5f;
				rotationSpeed = 45;
				int direction = (sensors ["Left Sensor"]  && !(sensors ["Right Wall Sensor"] && sensors["Front Sensor"])) ? 1 : -1;
				remainingRotation = 90 * direction;
			}
		}

		Move ();
		Rotate ();
	}

	private void Move ()
	{
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}

	private void Rotate ()
	{
		int direction = remainingRotation > 1 ? 1 : -1;
		float rotate = rotationSpeed * Time.deltaTime * direction;
		if (Math.Abs (remainingRotation) < Math.Abs (rotate))
			rotate = remainingRotation;
		remainingRotation -= rotate;
		transform.Rotate (new Vector3 (0, rotate, 0));
	}


	public void updateSensorState (string name, bool value)
	{
		sensors [name] = value;
	}
}
