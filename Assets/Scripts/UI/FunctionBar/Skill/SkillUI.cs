using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour {

	private static SkillUI _instance;
	private UIPositionMove skillUI;
	private bool isShow = false;
	private PlayerStatus playerStatus;

	public UIGrid grid;
	public GameObject skillItemPrefab;
	public string[] swordmanSkillIdList;	//技能id
	public string[] magicianSkillIdList;

	public static SkillUI instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		skillUI = GetComponent<UIPositionMove>();
	}

	private void Start()
	{
		//角色也需要初始化的，所以放在start
		playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

		//根据角色获取技能id
		string[] list = null;
		switch (playerStatus.heroType)
		{
			case HeroType.Swordman:
				list = swordmanSkillIdList;
				break;
			case HeroType.Magician:
				list = magicianSkillIdList;
				break;
		}

		//设置技能数据
		if(list != null)
		{
			foreach(string id in list)
			{
				GameObject itemGo = grid.gameObject.AddChild(skillItemPrefab);
				itemGo.GetComponent<SkillItem>().SetId(id);
			}
		}
	}

	public void TransformState()
	{
		if (isShow == false)
		{
			isShow = true;
			skillUI.PlayForward();
			UpdateShow();
		}
		else
		{
			isShow = false;
			skillUI.PlayReverse();
		}
	}

	//更新技能是否可用
	private void UpdateShow()
	{
		SkillItem[] items = GetComponentsInChildren<SkillItem>();

		foreach (SkillItem item in items)
		{
			item.UpdateShow(playerStatus.level);
		}
	}
}
