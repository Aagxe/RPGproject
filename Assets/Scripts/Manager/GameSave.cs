using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSave : MonoBehaviour {

	public ShortCutGrid[] ShortCutGridList;
	private PlayerStatus playerStatus;

	private void Start()
	{
		playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
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
		else if (Input.GetKeyDown(KeyCode.L))
		{
			Inventory.instance.Load();
			EquipmentUI.instance.Load();
			playerStatus.Load();
		}
	}
}
