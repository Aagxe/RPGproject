using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cheat : MonoBehaviour {

	public UILabel textLable;
	public UIInput cheatInput;
	public GameObject bg;

	private string cheatStr;			//input获取的字符存到这里
	private bool panelState = true;
	private GameObject player;
	private Queue<string> showInputText = new Queue<string>();

	private void Awake()
	{
		TransformState();
	}

	void Start () {
		player = GameObject.FindGameObjectWithTag(Tags.player);
	}
	

	void Update () {
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			TransformState();
		}
	}

	void TransformState()
	{
		panelState = !panelState;

		textLable.gameObject.SetActive(panelState);
		cheatInput.gameObject.SetActive(panelState);
		bg.gameObject.SetActive(panelState);
	}

	public void OnSubmitCheat()
	{
		cheatStr = cheatInput.value;
		cheatInput.value = "";
		Analysis();
		ShowInputText();
	}

	private void ShowInputText()
	{
		string text = "";
		foreach(string str in showInputText)
		{
			text += str + "\r\n";
		}

		textLable.text = text;
	}

	/// <summary>
	/// 解析输入的代码
	/// </summary>
	private void Analysis()
	{
		try
		{
			string[] str = cheatStr.Split(' ');

			int value = 0;
			if (int.TryParse(str[1], out value))
			{
				switch (str[0])
				{
					case "exp":
						player.GetComponent<PlayerStatus>().PlusExp(value);

						break;
					case "point":
						player.GetComponent<PlayerStatus>().point_remain += value;
						break;
					case "money":
						Inventory.instance.PlusCoin(value);
						break;
					case "equipment":
						for (int i = 0; i < value; i++)
						{
							Inventory.instance.AddItemToBag(Random.Range(2001, 2023).ToString());
						}
						break;
					case "save":
						GameSave.instance.Save();
						break;
					case "load":
						GameSave.instance.Load();
						break;
				}

				showInputText.Enqueue(cheatStr);
			}
		}
		catch
		{
			showInputText.Enqueue("输入有误");
		}
		finally
		{
			if (showInputText.Count > 7)
			{
				showInputText.Dequeue();
			}
		}
	}
}
