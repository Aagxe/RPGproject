using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieCamera : MonoBehaviour {

	public float speed = 10;

	private float endZ = -20;

	void Update() {

		if (transform.position.z < endZ)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * 10);
		}

	}
}
