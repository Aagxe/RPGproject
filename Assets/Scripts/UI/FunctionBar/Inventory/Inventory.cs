using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public List<InventoryItemGrid> itemGrid = new List<InventoryItemGrid>();    //用于控制格子
	public UILabel coinNumberLabel;     //控制金币的显示
	public GameObject inventoryItem;    //物体

	private static Inventory _instance;
	private UIPositionMove tween;       //控制界面移动
	private int coinCount = 1000;       //默认数量
	private bool isShow = false;

	public static Inventory instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		tween = GetComponent<UIPositionMove>();
	}

	void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.X))
		{
			AddItemToBag(Random.Range(2001, 2023).ToString());
		}
#endif
	}


	//拾取到id物品，并添加到物品栏里面
	public void AddItemToBag(string id, int count = 1)
	{
		InventoryItemGrid grid = null;

		//查找是否存在该物品，
		foreach (InventoryItemGrid temp in itemGrid)
		{
			if (temp.id == id)
			{
				grid = temp;
				break;
			}
		}

		//存在物品数量+1
		if (grid != null)
		{
			grid.PlusNumber(count);
		}
		else
		{
			//不存在，判断背包是否满
			foreach (InventoryItemGrid temp in itemGrid)
			{
				//当前格子id为空，则未满
				if (temp.id == "")
				{
					grid = temp;
					break;
				}
			}

			//背包未满，则创建物体
			if (grid != null)
			{
				InventoryItem item = grid.GetComponentInChildren<InventoryItem>();

				//如果不存在隐藏的Item则创建
				if(item == null )
				{
					//NGUI的实例化
					GameObject itemGo = grid.gameObject.AddChild(inventoryItem);
					itemGo.transform.localPosition = Vector3.zero;

					//格子深度为5，数量为7，物品为6
					itemGo.GetComponent<UISprite>().depth = 6;
					grid.SetId(id, count);
				}
				else
				{
					item.GetComponent<UISprite>().enabled = true ;
					grid.SetId(id, count);
				}
			}
		}

	}

	private void Show()
	{
		isShow = true;
		tween.PlayForward();
	}

	private void Hide()
	{
		isShow = false;
		tween.PlayReverse();
	}

	/// <summary>
	/// 状态转移
	/// </summary>
	public void TransformState()
	{
		if (isShow == false)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	/// <summary>
	/// 减少金币数量
	/// </summary>
	public bool GetCoinCount(int count)
	{
		if (coinCount >= count)
		{
			coinCount -= count;
			coinNumberLabel.text = coinCount.ToString();    //更新显示
			return true;
		}

		return false;
	}

	public void PlusCoin(int count)
	{
		coinCount += count;
		coinNumberLabel.text = coinCount.ToString();
	}

	/// <summary>
	/// 查找是否存在该物品
	/// </summary>
	public InventoryItemGrid CheckItemNum(string id)
	{
		InventoryItemGrid grid = null;
		foreach (InventoryItemGrid temp in itemGrid)
		{
			if (temp.id == id)
			{
				grid = temp;
				break;
			}
		}

		return grid;
	}

	/// <summary>
	/// 减去物品
	/// </summary>
	public bool MinusItem(string id, int count = 1)
	{
		InventoryItemGrid grid = CheckItemNum(id);

		return grid == null ? false : grid.MinusNumber(count);
	}

	public void Save()
	{
		int index = 1;

		foreach (InventoryItemGrid temp in itemGrid)
		{
			//不管有没有物品都必须存
			PlayerPrefs.SetString(SaveKeys.INVENTORY + index, temp.id);
			//物品数量
			PlayerPrefs.SetInt(SaveKeys.INVENTORY_NUM + index, temp.itemNum);
			index++;
		}

		//金币
		PlayerPrefs.SetInt(SaveKeys.MONEY, coinCount);
	}

	public void Load()
	{

		coinCount = PlayerPrefs.GetInt(SaveKeys.MONEY);
		coinNumberLabel.text = coinCount.ToString();

		//必须清除所有数据
		foreach (InventoryItemGrid temp in itemGrid)
		{
			//如果格子存在物品则清空信息，避免添加到背包时出错
			temp.CleanInfoInChildren();
		}

		
		int index = 1;
		foreach (InventoryItemGrid temp in itemGrid)
		{
			//获取到的id不等于空
			string id = PlayerPrefs.GetString(SaveKeys.INVENTORY + index);

			//物品数量
			int num = PlayerPrefs.GetInt(SaveKeys.INVENTORY_NUM + index);

			if ( id != "" && num > 0)
			{
				AddItemToBag(id, num);
			}

			index++;
		}
	}
}
