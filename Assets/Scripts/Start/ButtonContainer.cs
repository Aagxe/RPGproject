using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 场景分类：
 * 1.开始场景
 * 2.角色选择界面
 * 3.实际游戏场景
 * **/

public class ButtonContainer : MonoBehaviour {

	public AudioClip buttonDownClip;

	private AudioSource m_Audio;

	private void Start()
	{
		m_Audio = GetComponent<AudioSource>();
	}

	//加载角色选择场景
	public void OnNewGame()
	{
		PlayerPrefs.SetInt(SaveKeys.DATA_FROM_SAVE, 0);
		m_Audio.PlayOneShot(buttonDownClip);
		SceneManager.LoadScene("02_Character Creation");
	}

	//加载存档的游戏场景
	public void OnLoadGame()
	{
		PlayerPrefs.SetInt(SaveKeys.DATA_FROM_SAVE, 1);
		m_Audio.PlayOneShot(buttonDownClip);
		SceneManager.LoadScene("03_Play");
	}
}
