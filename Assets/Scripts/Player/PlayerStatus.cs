using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
	Swordman,
	Magician
}

public class PlayerStatus :MonoBehaviour{
	
	public HeroType heroType;

	public string playerName = "默认";	     //名字
    public int level = 1;                    //等级，每一级经验 100 + level * 30
    public int hpMax = 100;                  //总血量
    public int mpMax = 100;                  //总魔法
	public float hp_remain = 100;			 //剩余血量
	public float mp_remain = 100;            //剩余魔法
	public float exp = 0;					 //经验

    public float attack_base = 20;           //基础攻击
    public float defense_base = 20;          //基础防御
    public float speed_base = 20;            //基础速度

    public int attack_plus = 0;              //附加攻击
    public int defense_plus = 0;             //附加防御
    public int speed_plus = 0;               //附加速度

    public int point_remain = 0;             //剩余技能点数

	public GameObject LevelUpPrefab;		 //升级特效

	private void Start()
	{
		PlusExp(0);
	}


	/// <summary>
	///	加点
	/// </summary>
	public bool GetPoint(int count = 1)
	{
		if (point_remain >= count)
		{
			point_remain -= count;
			return true;
		}
		return false;
	}

	/// <summary>
	/// 治疗
	/// </summary>
	public void Remedy(int hp, int mp)
	{
		hp_remain += hp;
		mp_remain += mp;

		hp_remain = Mathf.Clamp(hp_remain, 0, hpMax);
		mp_remain = Mathf.Clamp(mp_remain, 0, mpMax);

		HeadStatusUI.instance.UpdatePropertyShow();
	}

	public void PlusExp(int exp)
	{
		this.exp += exp;
		//升级需要的exp
		int totalExp = 100 + level * 30;
		
		//为了避免经验足够升好几级的情况
		while(this.exp >= totalExp)
		{
			//等级增加
			level++;

			//血量魔法恢复与增加
			hpMax += 50;
			hp_remain = hpMax;
			mpMax += 10;
			mp_remain = mpMax;

			//加点
			point_remain += 5;

			//升级特效
			GameObject go = Instantiate(LevelUpPrefab, transform.position, Quaternion.identity);
			go.transform.parent = transform;

			this.exp -= totalExp;
			totalExp = 100 + level * 30;

			HeadStatusUI.instance.UpdatePropertyShow();
		}

		ExpBar.instance.SetValue(this.exp / totalExp);
	}

	/// <summary>
	///  判断MP是否足够
	/// </summary>
	public bool CheckMP(int count)
	{
		if (mp_remain >= count)
			return true;

		return false;
	}

	/// <summary>
	/// 判断并扣除MP
	/// </summary>
	public bool GetMP(int count)
	{
		if(mp_remain >= count)
		{
			mp_remain -= count;
			HeadStatusUI.instance.UpdatePropertyShow();
			return true;
		}

		return false;
	}


	public void Save()
	{
		PlayerPrefs.SetString(SaveKeys.HERO_TYPE, heroType.ToString());
		PlayerPrefs.SetString(SaveKeys.NAME, playerName);

		PlayerPrefs.SetInt(SaveKeys.LEVEL, level);
		PlayerPrefs.SetInt(SaveKeys.HP_MAX, hpMax);
		PlayerPrefs.SetInt(SaveKeys.MP_MAX, mpMax);
		PlayerPrefs.SetFloat(SaveKeys.HP_REMAIN, hp_remain);
		PlayerPrefs.SetFloat(SaveKeys.MP_REMAIN, mp_remain);
		PlayerPrefs.SetFloat(SaveKeys.EXP, exp);

		PlayerPrefs.SetFloat(SaveKeys.ATTACK_BASE, attack_base);
		PlayerPrefs.SetFloat(SaveKeys.DEFENSE_BASE, defense_base);
		PlayerPrefs.SetFloat(SaveKeys.SPEED_BASE, speed_base);

		PlayerPrefs.SetInt(SaveKeys.ATTACK_PLUS, attack_plus);
		PlayerPrefs.SetInt(SaveKeys.DEFENSE_PLUS, defense_plus);
		PlayerPrefs.SetInt(SaveKeys.SPEED_PLUS, speed_plus);

		PlayerPrefs.SetInt(SaveKeys.POINT_REMAIN, point_remain);
	}

	public void Load()
	{
		string hero = PlayerPrefs.GetString(SaveKeys.HERO_TYPE);
		switch(hero)
		{
			case "Magician":
				heroType = HeroType.Magician;
				break;
			case "Swordman":
				heroType = HeroType.Swordman;
				break;
		}

		playerName = PlayerPrefs.GetString(SaveKeys.NAME);

		level = PlayerPrefs.GetInt(SaveKeys.LEVEL);
		hpMax = PlayerPrefs.GetInt(SaveKeys.HP_MAX);
		mpMax = PlayerPrefs.GetInt(SaveKeys.MP_MAX);
		hp_remain = PlayerPrefs.GetFloat(SaveKeys.HP_REMAIN);
		mp_remain = PlayerPrefs.GetFloat(SaveKeys.MP_REMAIN);
		exp = PlayerPrefs.GetFloat(SaveKeys.EXP);

		attack_base = PlayerPrefs.GetFloat(SaveKeys.ATTACK_BASE);
		defense_base = PlayerPrefs.GetFloat(SaveKeys.DEFENSE_BASE);
		speed_base = PlayerPrefs.GetFloat(SaveKeys.SPEED_BASE);

		attack_plus = PlayerPrefs.GetInt(SaveKeys.ATTACK_PLUS);
		defense_plus = PlayerPrefs.GetInt(SaveKeys.DEFENSE_PLUS);
		speed_plus = PlayerPrefs.GetInt(SaveKeys.SPEED_PLUS);

		point_remain = PlayerPrefs.GetInt(SaveKeys.POINT_REMAIN);
	}
}
