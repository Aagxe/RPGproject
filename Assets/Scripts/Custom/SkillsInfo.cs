using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInfo : MonoBehaviour {

	private static SkillsInfo _instance;
	private Dictionary<string, SkillInfo> skillInfoDic = new Dictionary<string, SkillInfo>();

	public TextAsset skillsInfoListText;    //读取txt数据
	public static SkillsInfo instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		ReadInfo();
	}

	/// <summary>
	/// 通过id获取技能信息
	/// </summary>
	/// <param name="id">要获取的技能id</param>
	/// <returns></returns>
	public SkillInfo GetSkillInfoById(string id)
	{
		SkillInfo info = null;

		skillInfoDic.TryGetValue(id, out info);

		return info;
	}

	private void ReadInfo()
	{
		//读取数据
		string text = skillsInfoListText.text;
		//按行切分数据
		string[] strArr = text.Split('\n');

		//细分行数据
		foreach (string str in strArr)
		{
			string[] propertyArr = str.Split(',');
			SkillInfo info = new SkillInfo();

			info.id = propertyArr[0];
			info.name = propertyArr[1];
			info.icon_name = propertyArr[2];
			info.describe = propertyArr[3];
			
			switch(propertyArr[4])
			{
				case "Passive":
					info.applyType = ApplyType.Passive;
					break;
				case "Buff":
					info.applyType = ApplyType.Buff;
					break;
				case "SingleTarget":
					info.applyType = ApplyType.SingleTarget;
					break;
				case "MultiTarget":
					info.applyType = ApplyType.MultiTarget;
					break;
			}

			switch(propertyArr[5])
			{
				case "Attack":
					info.applyProperty = ApplyProperty.Attack;
					break;
				case "Defense":
					info.applyProperty = ApplyProperty.Defense;
					break;
				case "Speed":
					info.applyProperty = ApplyProperty.Speed;
					break;
				case "AttackSpeed":
					info.applyProperty = ApplyProperty.AttackSpeed;
					break;
				case "HP":
					info.applyProperty = ApplyProperty.HP;
					break;
				case "MP":
					info.applyProperty = ApplyProperty.MP;
					break;
			}

			info.applyValue = int.Parse(propertyArr[6]);
			info.applyTime = int.Parse(propertyArr[7]);
			info.mp = int.Parse(propertyArr[8]);
			info.coldTime = int.Parse(propertyArr[9]);

			switch(propertyArr[10])
			{
				case "Swordman":
					info.applicableRole = ApplicableRole.Swordman;
					break;
				case "Magician":
					info.applicableRole = ApplicableRole.Magician;
					break;
			}

			info.level = int.Parse(propertyArr[11]);

			switch(propertyArr[12])
			{
				case "Self":
					info.releaseType = ReleaseType.Self;
					break;
				case "Enemy":
					info.releaseType = ReleaseType.Enemy;
					break;
				case "Position":
					info.releaseType = ReleaseType.Position;
					break;
			}

			info.distance = float.Parse(propertyArr[13]);

			info.efxName = propertyArr[14];
			info.animNmae = propertyArr[15];
			info.animTime = float.Parse(propertyArr[16]);

			//存入字典
			skillInfoDic.Add(info.id, info);
		}
	}
}

/// <summary>
/// 技能信息
/// </summary>
public class SkillInfo
{
	public string id;
	public string name;                         //游戏内名称
	public string icon_name;                    //icon文件名
	public string describe;	                    //技能描述
	public ApplyType applyType;                 //作用类型
	public ApplyProperty applyProperty;         //作用属性
	public int applyValue;                      //作用值
	public int applyTime;                       //作用时间
	public int mp;                              //消耗魔法值
	public int coldTime;                        //冷却时间
	public ApplicableRole applicableRole;       //适用角色
	public int level;                           //适用等级
	public ReleaseType releaseType;             //释放类型
	public float distance;                      //释放距离
	public string efxName;                      //特效名称
	public string animNmae;                     //动画名称
	public float animTime = 0;					//动画时间
}

/// <summary>
/// 适用角色
/// </summary>
public enum ApplicableRole
{
	Swordman,
	Magician
}

/// <summary>
/// 作用类型
/// </summary>
public enum ApplyType
{
	Passive,        //增益（加MP,HP）
	Buff,           //增强（增加伤害，防御，移动速度，攻击速度）
	SingleTarget,   //单个目标
	MultiTarget     //多个目标
}

/// <summary>
/// 作用属性
/// </summary>
public enum ApplyProperty
{
	Attack,			//攻击
	Defense,		//防御
	Speed,			//行走速度
	AttackSpeed,	//攻击速度
	HP,				//生命
	MP				//魔法
}

/// <summary>
/// 释放类型
/// </summary>
public enum ReleaseType
{
	Self,		//当前位置释放
	Enemy,		//指定敌人释放
	Position	//指定位置释放

}