using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadthPanel : MonoBehaviour {

	private static DeadthPanel _instance;
	public Transform townPos;
	private UISprite[] sprite;
	private PlayerStatus playerStatus;
	private PlayerAttack playerAttack;

	public static DeadthPanel instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		sprite = GetComponentsInChildren<UISprite>();
	}

	private void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		playerStatus = player.GetComponent<PlayerStatus>();
		playerAttack = player.GetComponent<PlayerAttack>();
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnShowDeadthPanel();
		}
	}

	public void OnShowDeadthPanel()
	{
		foreach (UISprite ui in sprite)
		{
			ui.enabled = true;
		}
	}

	public void OnHideDeadthPanel()
	{
		foreach (UISprite ui in sprite)
		{
			ui.enabled = false;
		}
	}

	public void OnReturnToTown()
	{
		OnHideDeadthPanel();

		//load上一次数据
		if (GameSave.instance.Load())
		{
			//设置位置
			playerStatus.transform.position = townPos.position;
			//这样可以避免跑到之前指定的位置
			playerStatus.GetComponent<PlayerDirection>().targetPosition = townPos.position;
			//重生
			playerAttack.playerState = PlayerState.ControlWalk;
		}
		//没有保存数据的情况
		else
		{
			PlayerPrefs.SetInt(SaveKeys.DATA_FROM_SAVE, 0);

			//跳到角色选择
			SceneManager.LoadScene("02_Character Creation");
		}
	}

	public void OnQuit()
	{
		Application.Quit();
	}
}
