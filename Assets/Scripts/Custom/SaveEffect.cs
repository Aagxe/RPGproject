using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEffect : MonoBehaviour {

	public GameObject savePanel;
	private UISprite[] sprite;

	private void Awake()
	{
		sprite = savePanel.GetComponentsInChildren<UISprite>();
	}

	void OnMouseOver()
	{
		if(UICamera.isOverUI == false && Input.GetMouseButtonDown(0))
		{
			OnShowSavePanel();
		}
	}

	public void OnShowSavePanel()
	{
		foreach (UISprite ui in sprite)
		{
			ui.enabled = true;
		}
	}

	public void OnCancel()
	{
		foreach (UISprite ui in sprite)
		{
			ui.enabled = false;
		}
	}

	public void OnSave()
	{
		GameSave.instance.Save();
		OnCancel();
	}
}
