using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Robot : MonoBehaviour
{
	private Rigidbody rb;
	private float velocidad = 4f;		// metros/segundos
	private float velocidadGiro = 180f;	// grados/segundos
	private float pendienteGirar = 0f;

	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
	}


	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			pendienteGirar += 90;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			pendienteGirar -= 90;
		}
		Mover ();
		Girar ();
	}

	private void Mover ()
	{
		if (pendienteGirar == 0) {
			transform.Translate (Vector3.forward * velocidad * Time.deltaTime);
		}
	}


	void OnTriggerEnter (Collider collider)
	{
		//if (pendienteGirar != 0)
		//	pendienteGirar = 90;
	}
	void OnTriggerStay (Collider collider)
	{
	}
	void OnTriggerExit (Collider collider)
	{
	}

	private void Girar ()
	{
		if (pendienteGirar != 0) {
			int direccion = pendienteGirar > 1 ? 1 : -1;
			float giro = velocidadGiro * Time.deltaTime * direccion;
			if (Math.Abs(pendienteGirar) < Math.Abs(giro))
				giro = pendienteGirar;
			pendienteGirar -= giro;
			transform.Rotate (new Vector3 (0, giro, 0));
		}
	}
}
