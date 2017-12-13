using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTextParent : MonoBehaviour {
	private static HUDTextParent _instance;

	public static HUDTextParent instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
	}

}
