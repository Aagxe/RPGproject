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
		Inventory.instance.Save();
		EquipmentUI.instance.Save();
		playerStatus.Save();

		foreach (ShortCutGrid temp in ShortCutGridList)
		{
			temp.Save();
		}

		PlayerPrefs.Save();
	}


	public void Load()
	{
		Inventory.instance.Load();
		EquipmentUI.instance.Load();
		playerStatus.Load();

		foreach (ShortCutGrid temp in ShortCutGridList)
		{
			temp.Load();
		}
	}
}
