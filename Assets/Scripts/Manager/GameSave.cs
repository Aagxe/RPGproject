using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSave : MonoBehaviour {

	private static GameSave _instance;
	public ShortCutGrid[] ShortCutGridList;
	private PlayerStatus playerStatus;


	public static GameSave instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Save();
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			Load();
		}
	}

	public void Save()
	{
		playerStatus.Save();
		Inventory.instance.Save();
		EquipmentUI.instance.Save();

		foreach (ShortCutGrid temp in ShortCutGridList)
		{
			temp.Save();
		}

		BarNPC.instance.Save();

		PlayerPrefs.Save();
	}


	public bool Load()
	{
		//如果无法读取代表英雄类型不同
		if (playerStatus.Load())
		{
			Inventory.instance.Load();
			EquipmentUI.instance.Load();

			foreach (ShortCutGrid temp in ShortCutGridList)
			{
				temp.Load();
			}

			BarNPC.instance.Load();

			return true;
		}
		return false;
	}
}
