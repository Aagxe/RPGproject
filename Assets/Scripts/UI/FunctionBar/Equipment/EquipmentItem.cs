using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : MonoBehaviour {

	private UISprite item;
	private string m_id;
	private bool isHover = false;

	public string id
	{
		get { return m_id; }
	}

	private void Awake()
	{
		item = GetComponent<UISprite>();
	}

	private void Update()
	{
		//装备卸下
		if (isHover && Input.GetMouseButtonDown(1))
		{
			EquipmentUI.instance.TakeOffEquipment(id, gameObject);
		}
	}

	public void SetId(string id)
	{
		m_id = id;
		ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);
		SetIconName(info);
	}

	/// <summary>
	/// 更换当前显示的图片
	/// </summary>
	public void SetIconName(ObjectInfo info)
	{
		m_id = info.id;
		item.spriteName = info.icon_name;
	}

	//和InventroyItem中的不一样，它里面的要自己定义Trigger和Event
	//这个是NGUI提供的函数和Unity提供的Start一样
	public void OnHover(bool isOver)
	{
		isHover = isOver;
	}
}
