using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeapon : MonoBehaviour {

	private static ShopWeapon _instance;
	private UIPositionMove shopWeapon;
	private bool isShow = false;

	public UIGrid grid;
	public string[] weaponIdArray;
	public GameObject weaponItemPrefab;

	public static ShopWeapon instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		shopWeapon = GetComponent<UIPositionMove>();
	}

	private void Start()
	{
		InitShopWeapon();
	}

	public void TransformState()
	{
		if (isShow == false)
		{
			isShow = true;
			shopWeapon.PlayForward();
		}
		else
		{
			isShow = false;
			shopWeapon.PlayReverse();
		}
	}

	public void CloseButtonClick()
	{
		TransformState();
	}

	private void InitShopWeapon()
	{
		foreach(string id in weaponIdArray)
		{
			GameObject itemGo = grid.gameObject.AddChild(weaponItemPrefab);
			itemGo.GetComponent<WeaponItem>().SetId(id);
		}
	}

	public void Buy(string buyId)
	{
		int price = ObjectsInfo.instance.GetObjectInfoById(buyId).price_buy;
		if(Inventory.instance.GetCoinCount(price))
		{
			Inventory.instance.AddItemToBag(buyId);
		}
	}
}
