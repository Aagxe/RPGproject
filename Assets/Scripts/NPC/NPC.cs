using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {



	protected virtual void OnMouseEnter()
	{
		CursorManager.instance.SetNpcTalk();
	}

    protected virtual void OnMouseExit()
	{
		CursorManager.instance.SetNormal();
	}

}
