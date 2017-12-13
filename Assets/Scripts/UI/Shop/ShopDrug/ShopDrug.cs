using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDrug : MonoBehaviour
{
    private static ShopDrug _instance;
    private UIPositionMove shopDrug;
    private bool isShow = false;

    private GameObject NumberDialog;
    private UIInput NumberInput;
	private string buy_id;

    public static ShopDrug instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
        shopDrug = GetComponent<UIPositionMove>();

        NumberDialog = transform.Find("NumberDialog").gameObject;
        NumberInput = NumberDialog.transform.Find("NumberInput").GetComponent<UIInput>();
		NumberDialog.SetActive(false);
    }

    public void TransformState()
    {
        if (isShow == false)
        {
            isShow = true;
            shopDrug.PlayForward();
        }
        else
        {
            isShow = false;
            shopDrug.PlayReverse();
		}
    }

    public void CloseButtonClick()
    {
        TransformState();
    }

    public void OnBuyId1001()
    {
		Buy("1001");
    }

    public void OnBuyId1002()
    {
		Buy("1002");
    }

    public void OnBuyId1003()
    {
		Buy("1003");
    }

	private void ShowNumberDialog()
	{
		NumberDialog.SetActive(true);
		NumberInput.value = "1";
	}

	private void Buy(string id)
	{
		ShowNumberDialog();
		buy_id = id;
	}

    public void OnOkButtonClick()
    {
		//获取购买数量
		int count = int.Parse(NumberInput.value);
		//根据id获取物品价格
		ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(buy_id);
		int price_total = info.price_buy * count;

		//判断金币是否足够，并且购买数量大于0
		if(Inventory.instance.GetCoinCount(price_total) && count > 0)
		{
			Inventory.instance.AddItemToBag(buy_id, count);
		}

		NumberDialog.SetActive(false);
	}
}
