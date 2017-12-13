using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDescribe : MonoBehaviour {

	private static InventoryDescribe _instance;
	private UILabel describe;
	private float timer;			//鼠标离开物品倒数关闭信息框

	public static InventoryDescribe instance
	{
		get { return _instance; }
	}

	void Awake()
	{
		_instance = this;
		describe = GetComponentInChildren <UILabel>();
		gameObject.SetActive(false);
	}

	private void Update()
	{
		//如果被激活就定时关闭
		if(gameObject.activeInHierarchy)
		{
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void Show(string id)
	{
		gameObject.SetActive(true);
		timer = 0.1f;
		//跟随鼠标
		transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);

		//获取描述
		ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);
		string des = "";
		switch(info.type)
		{
			case ObjectType.Drug:
				des = GetDrugDescribe(info);
				break;
			case ObjectType.Equip:
				des = GetEquipDescribe(info);
				break;
		}
		//显示
		describe.text = des;
	}

	private string GetDrugDescribe(ObjectInfo info)
	{
		string str = "";
		str += "名称：" + info.name + "\n";
		str += "+HP : " + info.hp + "\n";
		str += "+MP：" + info.mp + "\n";
		str += "出售价：" + info.price_sell + "\n";
		str += "购买价：" + info.price_buy;

		return str;
	}

	private string GetEquipDescribe(ObjectInfo info)
	{
		string str = "";
		str += "名称：" + info.name + "\n";
		str += "攻击：" + info.attack + "\n";
		str += "防御：" + info.defense + "\n";
		str += "速度：" + info.speed + "\n";

		switch(info.dressType)
		{
			case DressType.Headgear:
				str += "穿戴类型：头盔" + "\n";
				break;
			case DressType.Armor:
				str += "穿戴类型：胸甲" + "\n";
				break;
			case DressType.LeftHand:
				str += "穿戴类型：左手" + "\n";
				break;
			case DressType.RightHand:
				str += "穿戴类型：右手" + "\n";
				break;
			case DressType.Shoe:
				str += "穿戴类型：鞋子" + "\n";
				break;
			case DressType.Accessory:
				str += "穿戴类型：饰品" + "\n";
				break;
		}

		switch(info.applicationType)
		{
			case ApplicationType.Swordman:
				str += "适用类型：剑士" + "\n";
				break;
			case ApplicationType.Magician:
				str += "适用类型：魔法师" + "\n";
				break;
			case ApplicationType.Common:
				str += "适用类型：通用" + "\n";
				break;
		}

		str += "出售价：" + info.price_sell + "\n";
		str += "购买价：" + info.price_buy;
		return str;
	}
}
