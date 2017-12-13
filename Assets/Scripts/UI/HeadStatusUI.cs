using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStatusUI : MonoBehaviour {

	private static HeadStatusUI _instance;
	private UILabel nameLable;
	private UISlider hpBar;
	private UISlider mpBar;
	private UILabel hpLable;
	private UILabel mpLable;
	private PlayerStatus playerStatus;

	public static HeadStatusUI instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;

		nameLable = transform.Find("Name").GetComponent<UILabel>();
		hpBar = transform.Find("HP").GetComponent<UISlider>();
		mpBar = transform.Find("MP").GetComponent<UISlider>();
		hpLable = hpBar.transform.Find("Label").GetComponent<UILabel>();
		mpLable = mpBar.transform.Find("Label").GetComponent<UILabel>();
	}
	private void Start()
	{
		playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
		UpdatePropertyShow();
	}

	public void UpdatePropertyShow()
	{
		nameLable.text = "Lv." + playerStatus.level + " " + playerStatus.playerName;
		hpBar.value = playerStatus.hp_remain / playerStatus.hpMax;
		mpBar.value = playerStatus.mp_remain / playerStatus.mpMax;

		hpLable.text = playerStatus.hp_remain + "/" + playerStatus.hpMax;
		mpLable.text = playerStatus.mp_remain + "/" + playerStatus.mpMax;
	}
}
