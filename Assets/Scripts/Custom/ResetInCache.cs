using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInCache : MonoBehaviour {

	public GameObject[] click;
	private int clickLength;

	void Start()
	{
		clickLength = click.Length;
	}

	void OnEnable()
	{
		for (int i = 0; i < clickLength; i++)
		{
			click[i].GetComponent<ScrollUV>().Init();
		}
	}
}
