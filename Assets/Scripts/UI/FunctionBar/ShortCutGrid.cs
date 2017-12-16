using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShortCutType
{
	None,
	Skill,
	Drug
}


public class ShortCutGrid : MonoBehaviour {

	public KeyCode keyCode;

	private string m_id;
	private UISprite icon;
	private UILabel keyNum;
	private ShortCutType shortCutType = ShortCutType.None;
	private SkillInfo skillInfo;
	private ObjectInfo objectInfo;

	private PlayerStatus playerStatus;
	private PlayerAttack playerAttack;
	private PlayerMove playerMove;

	private void Awake()
	{
		//不能用GetComponentInChildren，因为父亲也有这个组件
		icon = transform.Find("Icon").GetComponent<UISprite>();
		icon.gameObject.SetActive(false);

		keyNum = transform.Find("KeyNum").GetComponent<UILabel>();
	}

	private void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		playerStatus = player.GetComponent<PlayerStatus>();
		playerAttack = player.GetComponent<PlayerAttack>();
	}

	private void Update()
	{
		//控制快捷键
		if (Input.GetKeyDown(keyCode))
		{
			if (shortCutType == ShortCutType.Drug)
			{
				DrugUse();
			}
			else if (shortCutType == ShortCutType.Skill)
			{
				if (playerStatus.CheckMP(skillInfo.mp))
				{
					playerAttack.UseSkill(skillInfo);
				}
				//MP不足
				else
				{
					playerAttack.NotEnoughMP();
				}
			}
		}
	}

	/// <summary>
	/// 将技能设置到快捷栏
	/// </summary>
	public void SetSkill(string id)
	{
		m_id = id;
		icon.gameObject.SetActive(true);
		shortCutType = ShortCutType.Skill;

		skillInfo = SkillsInfo.instance.GetSkillInfoById(id);
		icon.spriteName = skillInfo.icon_name;

		//技能拖拽上去之后，关闭显示
		keyNum.gameObject.SetActive(false);
	}

	/// <summary>
	/// 将药品设置到快捷栏
	/// </summary>
	public void SetInventoryItem(string id)
	{
		objectInfo = ObjectsInfo.instance.GetObjectInfoById(id);

		if (objectInfo.type == ObjectType.Drug)
		{
			m_id = id;
			icon.gameObject.SetActive(true);
			shortCutType = ShortCutType.Drug;

			icon.spriteName = objectInfo.icon_name;

			keyNum.gameObject.SetActive(false);
		}
	}

	private void DrugUse()
	{
		//减少物品成功,就治疗
		if (Inventory.instance.MinusItem(m_id))
		{
			playerStatus.Remedy(objectInfo.hp, objectInfo.mp);
		}

		//不管治疗成不成功都检测数量，避免物品不存在还保留在快捷栏上
		if (Inventory.instance.CheckItemNum(m_id) == null)
		{
			m_id = "";
			shortCutType = ShortCutType.None;
			icon.gameObject.SetActive(false);
			objectInfo = null;
		}
	}

	public void Save()
	{
		//根据格子名字，保存当前格子存放的是什么类型
		switch(this.shortCutType)
		{
			case ShortCutType.None:
				PlayerPrefs.SetInt(gameObject.name, 0);
				break;
			case ShortCutType.Skill:
				PlayerPrefs.SetInt(gameObject.name, 1);
				break;
			case ShortCutType.Drug:
				PlayerPrefs.SetInt(gameObject.name, 2);
				break;
		}

		//根据技能栏名字保存id
		PlayerPrefs.SetString(gameObject.name + "_Id", m_id);
	}

	public void Load()
	{
		int type = PlayerPrefs.GetInt(gameObject.name);
		m_id = PlayerPrefs.GetString(gameObject.name + "_Id");

		switch (type)
		{
			// 0必须是None，因为有可能读取到的就是没存东西，然后返回0
			case 0:
				shortCutType = ShortCutType.None;
				icon.gameObject.SetActive(false);
				break;
			case 1:
				shortCutType = ShortCutType.Skill;
				SetSkill(m_id);
				break;
			case 2:
				shortCutType = ShortCutType.Drug;
				SetInventoryItem(m_id);
				break;
		}

		
	}
}
