using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour {

	public GameObject[] characterPrefabs;
	public UIInput inputName;

	private GameObject[] characterGameObjects;	//prefab的实例
	private int selectedIndex = 0;				//当前显示的角色索引
	private int prefabsLength;

	void Start () {
		prefabsLength = characterPrefabs.Length;
		characterGameObjects = new GameObject[prefabsLength];

		for (int i = 0; i < prefabsLength; i++)
		{
			characterGameObjects[i] = Instantiate(characterPrefabs[i], transform.position, transform.rotation);
		}

		UpdateCharacterShow();
	}

	/// <summary>
	/// 按钮按下后更新角色显示
	/// </summary>
	void UpdateCharacterShow()
	{
		characterGameObjects[selectedIndex].SetActive(true);
		for(int i=0;i<prefabsLength;i++)
		{
			if (i != selectedIndex)
			{
				characterGameObjects[i].SetActive(false);
			}
		}
	}

	//下一个角色按钮
	public void OnNextCharacter()
	{
		selectedIndex++;
		selectedIndex %= prefabsLength;
		UpdateCharacterShow();
	}

	//上一个角色按钮
	public void OnPrevCharacter()
	{
		selectedIndex--;
		if (selectedIndex < 0)
			selectedIndex = prefabsLength - 1;

		UpdateCharacterShow();
	}

	public void OnOkButtonClick()
	{
		//保存选择的人物和名字
		PlayerPrefs.SetInt(SaveKeys.SELECTED_CHARACTER_INDEX, selectedIndex);
		PlayerPrefs.SetString(SaveKeys.NAME, inputName.value);

		//加载下个场景
		SceneManager.LoadScene("03_Play");
	}
}
