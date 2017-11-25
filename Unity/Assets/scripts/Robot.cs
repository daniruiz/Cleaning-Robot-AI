using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Robot : MonoBehaviour
{
	private Rigidbody rb;
	private float speed = 4f;		// metros/segundos
	private float rotationSpeed = 180f;	// grados/segundos
	private float remainingRotation = 0f;

	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
	}


	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			remainingRotation += 90;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			remainingRotation -= 90;
		}
		Move ();
		Rotate ();
	}

	private void Move ()
	{
		if (remainingRotation == 0)
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}


	void OnTriggerEnter (Collider collider)
	{
	}
	void OnTriggerStay (Collider collider)
	{
	}
	void OnTriggerExit (Collider collider)
	{
	}

	private void Rotate ()
	{
		if (remainingRotation != 0) {
			int direction = remainingRotation > 1 ? 1 : -1;
			float rotate = rotationSpeed * Time.deltaTime * direction;
			if (Math.Abs(remainingRotation) < Math.Abs(rotate))
				rotate = remainingRotation;
			remainingRotation -= rotate;
			transform.Rotate (new Vector3 (0, rotate, 0));
		}
	}
}
