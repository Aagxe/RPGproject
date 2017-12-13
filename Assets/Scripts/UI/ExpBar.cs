using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBar : MonoBehaviour
{

	private static ExpBar _instance;
	private UISlider progressBar;

	public static ExpBar instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		progressBar = GetComponent<UISlider>();
	}

	public void SetValue(float value)
	{
		progressBar.value = value;
	}
}
