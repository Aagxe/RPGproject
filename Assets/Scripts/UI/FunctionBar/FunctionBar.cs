using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBar : MonoBehaviour {

	public void OnStatusButtonClick()
	{
        Status.instance.TransformState();
	}
	public void OnBagButtonClick()
	{
		Inventory.instance.TransformState();
	}
	public void OnEquipButtonClick()
	{
		EquipmentUI.instance.TransformState();
	}
	public void OnSkillButtonClick()
	{
		SkillUI.instance.TransformState();
	}
	public void OnSettingButtonClick()
	{

	}
}
