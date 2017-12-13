using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour {

	private string m_id;
	private SkillInfo info;
	
	//需要显示的技能信息
	private UISprite icon_sprite;
	private UILabel name_lable;
	private UILabel applytype_lable;
	private UILabel describe_lable;
	private UILabel mp_lable;
	private GameObject icon_mask;

	public string id
	{
		get { return m_id; }
	}
	
	private void InitProperty()
	{
		icon_sprite = transform.Find("Icon").GetComponent<UISprite>();
		name_lable = transform.Find("Property/NameBG/Name").GetComponent<UILabel>();
		applytype_lable = transform.Find("Property/ApplyTypeBG/ApplyType").GetComponent<UILabel>();
		describe_lable = transform.Find("Property/DescribeBG/Describe").GetComponent<UILabel>();
		mp_lable = transform.Find("Property/MPBG/MP").GetComponent<UILabel>();

		icon_mask = transform.Find("IconMask").gameObject;
		icon_mask.SetActive(false);
	}

	/// <summary>
	/// 根据等级显示哪一个技能可用
	/// </summary>
	public void UpdateShow(int level)
	{
		//技能可用
		if(info.level <= level)
		{
			//取消遮罩
			icon_mask.SetActive(false);
			//技能不可拖拽
			icon_sprite.GetComponent<SkillItemIconDrag>().enabled = true;
		}
		else
		{
			icon_mask.SetActive(true);
			icon_sprite.GetComponent<SkillItemIconDrag>().enabled = false;
		}
	}

	public void SetId(string id)
	{
		InitProperty();
		m_id = id;
		info = SkillsInfo.instance.GetSkillInfoById(id);

		icon_sprite.spriteName = info.icon_name;
		name_lable.text = info.name;

		switch (info.applyType)
		{
			case ApplyType.Passive:
				applytype_lable.text = "增益";
				break;
			case ApplyType.Buff:
				applytype_lable.text = "增强";
				break;
			case ApplyType.SingleTarget:
				applytype_lable.text = "单个目标";
				break;
			case ApplyType.MultiTarget:
				applytype_lable.text = "多个目标";
				break;
		}

		describe_lable.text = info.describe;
		mp_lable.text = info.mp + "MP";
	}
}
