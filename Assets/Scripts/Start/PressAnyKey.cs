using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour {

	private GameObject buttonContainer;

	private void Start()
	{
		buttonContainer = transform.parent.Find("ButtonContainer").gameObject;
	}

	void Update () {
		if (Input.anyKeyDown)
		{
			ShowButton();
		}
	}

	void ShowButton()
	{
		buttonContainer.SetActive(true);
		gameObject.SetActive(false);
	}
}
