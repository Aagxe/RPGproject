using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeaponNPC : NPC {

	protected void OnMouseOver()
	{
		//避免同时触发UI和任务
		if (UICamera.isOverUI == false)
		{
			if (Input.GetMouseButtonDown(0))
			{
				GetComponent<AudioSource>().Play();
				ShopWeapon.instance.TransformState();
			}
		}
	}
}
