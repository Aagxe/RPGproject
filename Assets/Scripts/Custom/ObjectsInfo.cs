using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理所有物品信息
/// </summary>
public class ObjectsInfo : MonoBehaviour {


	private static ObjectsInfo _instance;
	private Dictionary<string, ObjectInfo> objectInfoDic = new Dictionary<string, ObjectInfo>();	//根据id获取数据


	public TextAsset objectsInfoListText;	//读取txt文本的数据

	public static ObjectsInfo instance
	{
		get { return _instance; }
	}


	private void Awake()
	{
		_instance = this;
		ReadInfo();
	}

	public ObjectInfo GetObjectInfoById(string id)
	{
		ObjectInfo info = null;

		objectInfoDic.TryGetValue(id, out info);

		return info;

	}

	private void ReadInfo()
	{
		//读取txt的所有数据
		string text = objectsInfoListText.text;
		//window下的换行是\r\n，如果数据结尾是字符串，就会导致字符串带上\r
		text = text.Replace("\r",string.Empty);
		//根据换行符分割数据，切割的字符会丢弃
		string[] strArray = text.Split('\n');

		//把每一行数据细分为ObjectInfo
		foreach (string str in strArray)
		{
			string[] propertyArray = str.Split(',');
			ObjectInfo info = new ObjectInfo();

			//顺序参照表
			info.id = propertyArray[0];
			info.name = propertyArray[1];
			info.icon_name = propertyArray[2];

			switch (propertyArray[3])
			{
				case "Drug":
					info.type = ObjectType.Drug;
					break;
				case "Equip":
					info.type = ObjectType.Equip;
					break;
				case "Mat":
					info.type = ObjectType.Mat;
					break;
			}

			info.price_sell = int.Parse(propertyArray[4]);
			info.price_buy = int.Parse(propertyArray[5]);

			if (info.type == ObjectType.Drug)
			{
				info.hp = int.Parse(propertyArray[6]);
				info.mp = int.Parse(propertyArray[7]);
			}
			else if(info.type == ObjectType.Equip)
			{
				info.attack = int.Parse(propertyArray[6]);
				info.defense = int.Parse(propertyArray[7]);
				info.speed = int.Parse(propertyArray[8]);
				
				switch(propertyArray[9])
				{
					case "Headgear":
						info.dressType = DressType.Headgear;
						break;
					case "Armor":
						info.dressType = DressType.Armor;
						break;
					case "LeftHand":
						info.dressType = DressType.LeftHand;
						break;
					case "RightHand":
						info.dressType = DressType.RightHand;
						break;
					case "Shoe":
						info.dressType = DressType.Shoe;
						break;
					case "Accessory":
						info.dressType = DressType.Accessory;
						break;
				}

				switch(propertyArray[10])
				{
					case "Swordman":
						info.applicationType = ApplicationType.Swordman;
						break;
					case "Magician":
						info.applicationType = ApplicationType.Magician;
						break;
					case "Common":
						info.applicationType = ApplicationType.Common;
						break;
				}
			}


			//添加到字典，并用id查找存储的数据
			objectInfoDic.Add(info.id, info);
		}
	}
}

/// <summary>
/// 物品类型
/// </summary>
public enum ObjectType
{
	Drug,	//药品
	Equip,	//装备
	Mat		//材料
}

/// <summary>
/// 穿戴类型
/// </summary>
public enum DressType
{
	Headgear,	//头盔
	Armor,		//胸甲
	LeftHand,	//左手
	RightHand,	//右手
	Shoe,		//鞋子
	Accessory	//饰品
}

/// <summary>
/// 适用类型
/// </summary>
public enum ApplicationType
{
	Swordman,	//剑士
	Magician,	//魔法师
	Common		//通用
}


public class ObjectInfo
{
	public string id;
	public string name;                         // 游戏内名称
	public string icon_name;                    // icon文件名
	public ObjectType type;                     // 类型
	public int price_sell;                      // 出售价
	public int price_buy;                       // 购买价

	public int hp;                              // 加血量值
	public int mp;                              // 加魔法值

	public int attack;                          // 攻击
	public int defense;                         // 防御
	public int speed;	                        // 速度
	public DressType dressType;                 // 穿戴类型
	public ApplicationType applicationType;		// 适用类型
}