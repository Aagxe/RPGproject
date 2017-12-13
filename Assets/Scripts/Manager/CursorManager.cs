using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

	private static CursorManager _instance;
	
	public Texture2D cursor_normal;		//正常情况
	public Texture2D cursor_npc_talk;	//npc对话
	public Texture2D cursor_attack;		//攻击
	public Texture2D cursor_lockTarget;	//锁定目标
	public Texture2D cursor_pick;		//捡东西

	private Vector2 hotspot = Vector2.zero;				//左上角为原点
	private CursorMode cursorMode = CursorMode.Auto;	//自动选择用软件还是硬件控制鼠标的设置

	private CursorManager() { }

	public static CursorManager instance
	{
		get {return _instance; }
	}

	void Awake()
	{
		//必须在这里中给他赋值，因为图片是从外部引入的，如果自己new就会导致图片全部为空
		_instance = this;
	}

	public void SetNormal()
	{
		Cursor.SetCursor(cursor_normal, hotspot, cursorMode);
	}

	public void SetNpcTalk()
	{
		Cursor.SetCursor(cursor_npc_talk, hotspot, cursorMode);
	}

	public void SetAttack()
	{
		Cursor.SetCursor(cursor_attack, hotspot, cursorMode);
	}

	public void SetLockTarget()
	{
		Cursor.SetCursor(cursor_lockTarget, hotspot, cursorMode);
	}
}
