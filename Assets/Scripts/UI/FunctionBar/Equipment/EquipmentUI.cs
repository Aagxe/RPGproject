using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {

	private static EquipmentUI _instance;
	private UIPositionMove equipmentUI;
	private bool isShow = false;

	//装备格子
	private GameObject headgear;    //头部
	private GameObject armor;       //身体
	private GameObject leftHand;    //左手
	private GameObject rightHand;   //右手
	private GameObject shoe;        //鞋子
	private GameObject accessory;   //首饰

	//装备加成
	private int m_attack;
	private int m_defense;
	private int m_speed;

	private PlayerStatus playerStatus;

	public GameObject equipmentItem;

	public static EquipmentUI instance
	{
		get { return _instance; }
	}

	public int attack
	{
		get
		{
			return m_attack;
		}
	}

	public int defense
	{
		get
		{
			return m_defense;
		}
	}

	public int speed
	{
		get
		{
			return m_speed;
		}
	}

	private void Awake()
	{
		_instance = this;
		equipmentUI = GetComponent<UIPositionMove>();

		//获取装备格子
		headgear = transform.Find("Headgear").gameObject;
		armor = transform.Find("Armor").gameObject;
		leftHand = transform.Find("LeftHand").gameObject;
		rightHand = transform.Find("RightHand").gameObject;
		shoe = transform.Find("Shoe").gameObject;
		accessory = transform.Find("Accessory").gameObject;
	}

	private void Start()
	{
		playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

	}

	public void TransformState()
	{
		if (isShow == false)
		{
			isShow = true;
			equipmentUI.PlayForward();
		}
		else
		{
			isShow = false;
			equipmentUI.PlayReverse();
		}
	}

	/// <summary>
	/// 穿上装备
	/// </summary>
	/// <param name="id">需要穿上的装备id</param>
	/// <returns></returns>
	public bool Dress(string id)
	{
		ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);

		//穿戴必须是装备
		if(info.type != ObjectType.Equip)
			return false;

		//人物是剑士，但是装备是魔法师
		if (playerStatus.heroType == HeroType.Swordman)
			if (info.applicationType == ApplicationType.Magician)
				return false;

		if (playerStatus.heroType == HeroType.Magician)
			if (info.applicationType == ApplicationType.Swordman)
				return false;

		//判断装备要放在哪个格子下面
		GameObject parent = null;
		switch(info.dressType)
		{
			case DressType.Headgear:
				parent = headgear;
				break;
			case DressType.Armor:
				parent = armor;
				break;
			case DressType.LeftHand:
				parent = leftHand;
				break;
			case DressType.RightHand:
				parent = rightHand;
				break;
			case DressType.Shoe:
				parent = shoe;
				break;
			case DressType.Accessory:
				parent = accessory;
				break;
		}

		EquipmentItem item = parent.GetComponentInChildren<EquipmentItem>();

		//如果不为空，代表已经装备类同类型装备
		if (item != null)
		{
			//把当前的物品添加到背包，装备栏的物品更换为新的装备
			Inventory.instance.AddItemToBag(item.id);
			item.SetIconName(info);
		}
		else
		{
			GameObject itemGo = parent.AddChild(equipmentItem);
			itemGo.transform.localPosition = Vector3.zero;
			itemGo.GetComponent<EquipmentItem>().SetIconName(info);

			itemGo.GetComponent<UISprite>().depth = parent.GetComponent<UISprite>().depth + 1;
		}

		UpdateProperty();

		return true;
	}

	/// <summary>
	/// 脱下装备
	/// </summary>
	public void TakeOffEquipment(string id, GameObject go)
	{
		Inventory.instance.AddItemToBag(id);
		Destroy(go);
		UpdateProperty();
	}

	//更新属性
	private void UpdateProperty()
	{
		m_attack = 0;
		m_defense = 0;
		m_speed = 0;

		//取得每一个格子下面是否有装备
		EquipmentItem headgearItem = headgear.GetComponentInChildren<EquipmentItem>();
		PlusProprty(headgearItem);

		EquipmentItem armorItem = armor.GetComponentInChildren<EquipmentItem>();
		PlusProprty(armorItem);

		EquipmentItem leftHandItem = leftHand.GetComponentInChildren<EquipmentItem>();
		PlusProprty(leftHandItem);

		EquipmentItem rightHandItem = rightHand.GetComponentInChildren<EquipmentItem>();
		PlusProprty(rightHandItem);

		EquipmentItem shoeItem = shoe.GetComponentInChildren<EquipmentItem>();
		PlusProprty(shoeItem);

		EquipmentItem accessoryItem = accessory.GetComponentInChildren<EquipmentItem>();
		PlusProprty(accessoryItem);

	}

	private void PlusProprty(EquipmentItem item)
	{
		if(item != null)
		{
			ObjectInfo equipInfo = ObjectsInfo.instance.GetObjectInfoById(item.id);
			m_attack += equipInfo.attack;
			m_defense += equipInfo.defense;
			m_speed += equipInfo.speed;
		}
	}

	public void Save()
	{
		//不管是否为空都存储
		EquipmentItem headgearItem = headgear.GetComponentInChildren<EquipmentItem>();
		SaveEquipID(headgearItem, SaveKeys.HEADGEAR_ITEM_ID);

		EquipmentItem armorItem = armor.GetComponentInChildren<EquipmentItem>();
		SaveEquipID(armorItem, SaveKeys.ARMOR_ITEM_ID);

		EquipmentItem leftHandItem = leftHand.GetComponentInChildren<EquipmentItem>();
		SaveEquipID(leftHandItem, SaveKeys.LEFT_HAND_ITEM_ID);

		EquipmentItem rightHandItem = rightHand.GetComponentInChildren<EquipmentItem>();
		SaveEquipID(rightHandItem, SaveKeys.RIGHT_HAND_ITEM_ID);

		EquipmentItem shoeItem = shoe.GetComponentInChildren<EquipmentItem>();
		SaveEquipID(shoeItem, SaveKeys.SHOE_ITEM_ID);

		EquipmentItem accessoryItem = accessory.GetComponentInChildren<EquipmentItem>();
		SaveEquipID(accessoryItem, SaveKeys.ACCESSORY_ITEM_ID);
	}

	private void SaveEquipID(EquipmentItem item, string key)
	{
		if (item == null)
			PlayerPrefs.SetString(key, "");
		else
			PlayerPrefs.SetString(key, item.id);
	}

	public void Load()
	{
		//运行状态读取和保存没问题，非运行状态读取会导致全部装备去到一个格子
		LoadEquip(PlayerPrefs.GetString(SaveKeys.HEADGEAR_ITEM_ID)  , headgear);
		LoadEquip(PlayerPrefs.GetString(SaveKeys.ARMOR_ITEM_ID)     , armor);
		LoadEquip(PlayerPrefs.GetString(SaveKeys.LEFT_HAND_ITEM_ID) , leftHand);
		LoadEquip(PlayerPrefs.GetString(SaveKeys.RIGHT_HAND_ITEM_ID), rightHand);
		LoadEquip(PlayerPrefs.GetString(SaveKeys.SHOE_ITEM_ID)      , shoe);
		LoadEquip(PlayerPrefs.GetString(SaveKeys.ACCESSORY_ITEM_ID) , accessory);

	}

	private void LoadEquip(string itemID,GameObject parent)
	{
	
		EquipmentItem item = parent.GetComponentInChildren<EquipmentItem>();

		if (itemID == "")
		{
			//读取的数据为空，装备栏不为空
			if (item != null)
				Destroy(item.gameObject);
		}
		//读取的数据不为空
		else
		{
			if (item != null)
			{
				item.SetId(itemID);
			}
			else
			{
				GameObject itemGo = parent.AddChild(equipmentItem);
				itemGo.transform.localPosition = Vector3.zero;
				itemGo.GetComponent<EquipmentItem>().SetId(itemID);

				itemGo.GetComponent<UISprite>().depth = parent.GetComponent<UISprite>().depth + 1;
			}
		}

		UpdateProperty();
	}
}
