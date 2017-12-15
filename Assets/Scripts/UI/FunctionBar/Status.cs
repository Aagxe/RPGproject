using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {

	private static Status _instance;
	private UIPositionMove status;
	private bool isShow = false;

	//用于显示数据的UILable
	private UILabel attackLable;
	private UILabel defenseLable;
	private UILabel speedLable;
	private UILabel pointRemainLable;
	private UILabel summaryLable;

	//加点按钮
	private GameObject attackButtonGo;
	private GameObject defenseButtonGo;
	private GameObject speedButtonGo;

	private PlayerStatus playerStatus;


	public static Status instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
        status = GetComponent<UIPositionMove>();

		attackLable = transform.Find("Attack").GetComponent<UILabel>();
		defenseLable = transform.Find("Defense").GetComponent<UILabel>();
		speedLable = transform.Find("Speed").GetComponent<UILabel>();
		pointRemainLable = transform.Find("Point_Remain").GetComponent<UILabel>();
		summaryLable = transform.Find("Summary").GetComponent<UILabel>();

		attackButtonGo = transform.Find("Attack_Plus").gameObject;
		defenseButtonGo = transform.Find("Defense_Plus").gameObject;
		speedButtonGo = transform.Find("Speed_Plus").gameObject;
	}

	private void Start()
	{
		playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
	}

	public void TransformState()
	{
		if (isShow == false)
		{
			UpdateShow();
			isShow = true;
            status.PlayForward();
		}
		else
		{
			isShow = false;
            status.PlayReverse();
		}
	}


	/// <summary>
	/// 更新数据显示
	/// </summary>
	public void UpdateShow()
	{
		float attack = playerStatus.attack_base + playerStatus.attack_plus;
		float defense = playerStatus.defense_base + playerStatus.defense_plus;
		float speed = playerStatus.speed_base + playerStatus.speed_plus;

		attackLable.text = attack + "(" + (attack + EquipmentUI.instance.attack) + ")" ;
		defenseLable.text = defense + "(" + (defense + EquipmentUI.instance.defense) + ")";
		speedLable.text = speed + "(" + (speed + EquipmentUI.instance.speed) + ")";

		pointRemainLable.text = playerStatus.point_remain.ToString();

		summaryLable.text = "攻击：" + (attack + EquipmentUI.instance.attack)
			+ "  防御：" + (defense + EquipmentUI.instance.defense)
			+ "  速度：" + (speed + EquipmentUI.instance.speed);

		if (playerStatus.point_remain > 0)
		{
			attackButtonGo.SetActive(true);
			defenseButtonGo.SetActive(true);
			speedButtonGo.SetActive(true);
		}
		else
		{
			attackButtonGo.SetActive(false);
			defenseButtonGo.SetActive(false);
			speedButtonGo.SetActive(false);
		}
	}

	public void OnAttackPlusClick()
	{
		if(playerStatus.GetPoint())
		{
			playerStatus.attack_plus++;
			UpdateShow();
		}
	}

	public void OnDefensePlusClick()
	{
		if (playerStatus.GetPoint())
		{
			playerStatus.defense_plus++;
			UpdateShow();
		}
	}

	public void OnSpeedPlusClick()
	{
		if (playerStatus.GetPoint())
		{
			playerStatus.speed_plus++;
			UpdateShow();
		}
	}
}
