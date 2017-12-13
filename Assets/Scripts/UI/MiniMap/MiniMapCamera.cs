using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

	private Vector3 offset;
	private Camera miniMapCamera;
	private Transform player;
	private float smoothing = 5f;

	private void Awake()
	{
		miniMapCamera = GetComponent<Camera>();
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		offset = transform.position - player.position;
	}

	private void Update()
	{
		CameraFollow();
	}

	private void CameraFollow()
	{
		Vector3 targetCamPos = player.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
