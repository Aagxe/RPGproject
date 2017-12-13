using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	private Camera miniMapCamera;

	private void Awake()
	{
		miniMapCamera = GameObject.FindGameObjectWithTag(Tags.miniMap).GetComponent<Camera>();
	}

	//放大
	public void OnZoomInClick()
	{
		if(miniMapCamera.orthographicSize > 5)
		{
			miniMapCamera.orthographicSize -= 1;
		}
	}

	//缩小
	public void ONZoomOutClick()
	{
		if (miniMapCamera.orthographicSize < 15)
		{
			miniMapCamera.orthographicSize += 1;
		}
	}
}
