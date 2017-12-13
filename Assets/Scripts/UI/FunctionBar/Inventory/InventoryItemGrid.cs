using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemGrid : MonoBehaviour {

	private string m_id = "";			  //存储当前在这个格子上的是什么物品
	private int m_itemNum = 0;			  //当前格子存了多少个物品
	private UILabel m_itemNumLable;		  //num对应的ui
	private ObjectInfo m_info = null;     //记录当前物品的信息
	private InventoryItem item = null;	  //当前格子保存的物品Item

	public string id
	{
		get { return m_id; }
	}
	public int itemNum
	{
		get { return m_itemNum; }
	}

	void Start()
	{
		m_itemNumLable = GetComponentInChildren<UILabel>();
	}

	/// <summary>
	/// 设置格子显示的物品
	/// </summary>
	/// <param name="id">物品的id</param>
	/// <param name="num">物品的数量</param>
	public void SetId(string id, int num = 1)
	{
		m_id = id;
		m_info = ObjectsInfo.instance.GetObjectInfoById(id);
		item = GetComponentInChildren<InventoryItem>();

		//设置当前格子物品的图片
		item.SetIconName(id, m_info.icon_name);

		//激活并显示数量
		m_itemNumLable.enabled = true;
		m_itemNum = num;
		m_itemNumLable.text = m_itemNum.ToString();
	}

	/// <summary>
	/// 当物品移动到别的格子的时候清空当前格子记录
	/// 但是物品本身不会被销毁
	/// </summary>
	public void CleanInfo()
	{
		m_id = "";
		m_itemNum = 0;
		m_itemNumLable.enabled = false;
		m_info = null;
	}

	public void PlusNumber(int num = 1)
	{
		m_itemNum += num;
		m_itemNumLable.text = m_itemNum.ToString();
	}

	public bool MinusNumber(int num = 1)
	{
		if(m_itemNum >= num)
		{
			m_itemNum -= num;
			m_itemNumLable.text = m_itemNum.ToString();

			if (m_itemNum == 0)
			{
				CleanInfo();
				Destroy(GetComponentInChildren<InventoryItem>().gameObject);
			}

			return true;
		}

		return false;
	}


	public void DestroyGridItem()
	{
		if (id != "")
		{
			CleanInfo();
			Destroy(GetComponentInChildren<InventoryItem>().gameObject);
		}
	}
}
