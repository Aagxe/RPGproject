using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarNPC : NPC {

	public UIPositionMove quest;	//需要移动的任务框
	public UILabel des;             //任务描述
	public GameObject acceptBtnGo;
	public GameObject cancelBtnGo;
	public GameObject okBtnGo;

	private static BarNPC _instance;
	private bool _isInTask = false;	//是否在任务中
	public int killCount = 0;
	private PlayerStatus playStatus;

	public static BarNPC instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		playStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
	}

	protected void OnMouseOver()
	{
        //避免同时出发UI和任务
        if (UICamera.isOverUI == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_isInTask)
                {
                    ShowTaskProgress();
                }
                else
                {
                    ShowTaskDes();
                }

				GetComponent<AudioSource>().Play();
                ShowQuest();
            }
        }       
	}

	void ShowQuest()
	{
		quest.gameObject.SetActive(true);
		quest.PlayForward();
	}

	void HiddenQuest()
	{
		quest.PlayReverse();
		quest.PlayFinishEnable();
	}

	public void OnCloseButtonClick()
	{
		HiddenQuest();
	}

	public void OnKillWolf()
	{
		if (_isInTask)
		{
			killCount++;
		}
	}
	
	//任务描述
	void ShowTaskDes()
	{
		des.text = "任务：\n杀死10只小野狼\n\n奖励：\n1000金币";
		okBtnGo.SetActive(false);
		acceptBtnGo.SetActive(true);
		cancelBtnGo.SetActive(true);
	}

	//任务进度
	void ShowTaskProgress()
	{
		des.text = "任务：\n你已经杀死了" + killCount + "/10只小野狼\n\n奖励：\n1000金币";
		okBtnGo.SetActive(true);
		acceptBtnGo.SetActive(false);
		cancelBtnGo.SetActive(false);
	}


	public void OnAcceptButtonClick()
	{
		ShowTaskProgress();
		_isInTask = true;
	}

	public void OnCancelButtonClick()
	{
		HiddenQuest();
	}

	public void OnOkButtonClick()
	{
		if(killCount >= 10)
		{
			//完成任务
			Inventory.instance.PlusCoin(1000);
			killCount = 0;
			_isInTask = false;
			ShowTaskDes();
		}
		else
		{
			//未完成任务
			HiddenQuest();
		}
	}
}
