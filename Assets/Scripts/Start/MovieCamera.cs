using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieCamera : MonoBehaviour {

	public float speed = 10;

	//移动到石盘为-20,-41为welcome
	private float endZ = -41;

	void Update() {

		if (transform.position.z < endZ)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * 10);
		}

	}
}
