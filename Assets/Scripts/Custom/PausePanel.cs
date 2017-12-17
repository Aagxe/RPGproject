using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour {

	private static PausePanel _instance;
	private UISprite[] sprite;

	public static PausePanel instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		sprite = GetComponentsInChildren<UISprite>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			TransformState();
		}
	}

	public void TransformState()
	{
		foreach (UISprite ui in sprite)
		{
			ui.enabled = !ui.enabled;
		}
	}

	public void OnQuit()
	{
		Application.Quit();
	}

	public void OnMenu()
	{
		//回到第一个场景
		SceneManager.LoadScene(0);
	}
}
