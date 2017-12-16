using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour {

	public GameObject magicianPrefab;
	public GameObject swordmanPrefab;

	private void Awake()
	{
		//获取选择的人物和名字
		int selectedIndex = PlayerPrefs.GetInt(SaveKeys.SELECTED_CHARACTER_INDEX);
		string playerName = PlayerPrefs.GetString(SaveKeys.NAME);

		GameObject go = null;
		switch(selectedIndex)
		{
			case 0:
				go = Instantiate(magicianPrefab);
				break;
			case 1:
				go = Instantiate(swordmanPrefab);
				break;
		}

		go.GetComponent<PlayerStatus>().playerName = playerName;
	}

	private void Start()
	{
		//等于1代表点了LoadGame
		int save = PlayerPrefs.GetInt(SaveKeys.DATA_FROM_SAVE);
		if (save == 1)
		{
			GameSave.instance.Load();
		}
	}
}
