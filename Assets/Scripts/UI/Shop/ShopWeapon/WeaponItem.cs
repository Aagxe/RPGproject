using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour {

	private string m_id;
	private UISprite icon_sprite;
	private UILabel name_lable;
	private UILabel effect_lable;
	private UILabel priceSell_lable;

	public string id
	{
		get { return m_id; }
	}

	private void InitProperty()
	{
		icon_sprite = transform.Find("Icon").GetComponent<UISprite>();
		name_lable = transform.Find("Name").GetComponent<UILabel>();
		effect_lable = transform.Find("Effect").GetComponent<UILabel>();
		priceSell_lable = transform.Find("PriceSell").GetComponent<UILabel>();
	}

	public void SetId(string id)
	{
		InitProperty();

		m_id = id;
		ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);

		string type = "";
		switch (info.applicationType)
		{
			case ApplicationType.Swordman:
				type = "剑士";
				break;
			case ApplicationType.Magician:
				type = "魔法师";
				break;
			case ApplicationType.Common:
				type = "通用";
				break;
		}

		icon_sprite.spriteName = info.icon_name;
		name_lable.text = info.name + " (" + type + ")";
		
		//每一个装备只有一个属性
		if(info.attack > 0)
		{
			effect_lable.text = "效果：+" + info.attack + "攻击";
		}
		else if (info.defense > 0)
		{
			effect_lable.text = "效果：+" + info.defense + "防御";
		}
		else if (info.speed > 0)
		{
			effect_lable.text = "效果：+" + info.speed + "速度";
		}

		priceSell_lable.text = "售价：" + info.price_buy;
	}

	public void OnBuyClick()
	{
		ShopWeapon.instance.Buy(m_id);
	}
}
