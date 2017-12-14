using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承UIDragDropItem，重写Start等方法必须base.Start这样调用
public class InventoryItem : UIDragDropItem {

	private UISprite sprite;
	private int originalDepth;      //记录原来物品的层
	private bool isHover = false;	//鼠标是否在物品上
	private string m_id;

	protected override void Awake()
	{
		base.Awake();
		sprite = GetComponent<UISprite>();
	}

	protected override void Update()
	{
		base.Update();
		if(isHover)
		{
			//显示提示信息
			InventoryDescribe.instance.Show(m_id);

			//左键穿戴装备
			if(Input.GetMouseButtonDown(1))
			{
				//穿戴成功，物品数量减少
				if(EquipmentUI.instance.Dress(m_id))
				{
					transform.parent.GetComponent<InventoryItemGrid>().MinusNumber();
				}
			}
		}
	}

	protected override void OnDragDropStart()
	{
		base.OnDragDropStart();

		//关闭数量显示
		transform.parent.GetComponentInChildren<UILabel>().enabled = false;

		//记录原本的depth
		originalDepth = sprite.depth;

		//置顶
		sprite.depth = 999;
	}

	//surface本质上传过来的是UICamera.hoveredObject，也就是图片下面的是那个ui物体
	protected override void OnDragDropRelease(GameObject surface)
	{
		base.OnDragDropRelease(surface);

		bool isCleanChildren = false;

		//拖放到空格子
		if(surface.tag == Tags.inventoryItemGrid)
		{
			//拖放到非自身的空格子
			if (surface != transform.parent.gameObject)
			{

				InventoryItem hideItem = surface.GetComponentInChildren<InventoryItem>();
				InventoryItemGrid oldGrid = transform.parent.GetComponent<InventoryItemGrid>();
				InventoryItemGrid newGrid = surface.GetComponent<InventoryItemGrid>();

				//判断这个空格子是真的没有物体，还是有一个隐藏的InventoryItem
				if (hideItem == null)
				{
					//必须把设置放在SetId之前,SetId会获取子组件
					transform.parent = surface.transform;

					newGrid.SetId(oldGrid.id, oldGrid.itemNum);

					oldGrid.CleanInfo();
				}
				else
				{
					hideItem.GetComponent<UISprite>().enabled = true;
					newGrid.SetId(oldGrid.id, oldGrid.itemNum);

					oldGrid.CleanInfoInChildren();
					isCleanChildren = true;
				}
			}
		}
		//拖放到有物品的格子，两个格子信息交换
		else if (surface.tag == Tags.inventoryItem)
		{
			InventoryItemGrid originalGrid = transform.parent.GetComponent<InventoryItemGrid>();
			InventoryItemGrid targetGrid = surface.transform.parent.GetComponent<InventoryItemGrid>();

			string id = targetGrid.id;
			int num = targetGrid.itemNum;

			targetGrid.SetId(originalGrid.id, originalGrid.itemNum);
			originalGrid.SetId(id, num);
		}
		//拖放到快捷栏
		else if (surface.tag == Tags.shortCut)
		{
			surface.GetComponent<ShortCutGrid>().SetInventoryItem(m_id);
		}

		ResetItemPosition();

		//因为如果清空格子的子物体代表会执行隐藏操作，这里把它显示有不合理
		if(isCleanChildren == false)
			transform.parent.GetComponentInChildren<UILabel>().enabled = true;

		sprite.depth = originalDepth;
	}

	private void ResetItemPosition()
	{
		transform.localPosition = Vector3.zero;
	}

	/// <summary>
	/// 通过id设置物品的ui
	/// </summary>
	public void SetId(string id)
	{
		ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);
		sprite.spriteName = info.icon_name;
	}

	/// <summary>
	/// 设置物品的ui
	/// 因为prefab只是图片不一样，所以采用这种更换图片的方式即可
	/// </summary>
	public void SetIconName(string id, string icon_name)
	{
		sprite.spriteName = icon_name;
		m_id = id;
	}

	public void CleanInfo()
	{
		sprite.enabled = false;
	}

	//UIEventListener和UIEventOver触发
	public void OnHoverOver()
	{
		isHover = true;
	}
	public void OnHoverOut()
	{
		isHover = false;
	}

}
