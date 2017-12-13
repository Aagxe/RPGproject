using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 这段代码实际运行在clone物体身上，因为NGUI的内部实现
 * UIDragDropItem item = clone.GetComponent<UIDragDropItem>();
 * item.Start();
 * item.OnClone(gameObject);
 * item.OnDragDropStart();
 * 创建了一个新的克隆物体之后，获取克隆物体身上的拖拽并调用
 * */
public class SkillItemIconDrag : UIDragDropItem {

	private string m_id;

	//用于拖拽到快捷栏中
	protected override void OnDragDropStart()
	{
		base.OnDragDropStart();

		//用于设置icon到快捷栏
		m_id = transform.parent.GetComponent<SkillItem>().id;

		//为了让物体离开scroll view
		transform.parent = transform.root;
		GetComponent<UISprite>().depth = 999;

	}

	protected override void OnDragDropRelease(GameObject surface)
	{
		base.OnDragDropRelease(surface);

		//拖放到快捷栏
		if (surface.tag == Tags.shortCut)
		{
			surface.GetComponent<ShortCutGrid>().SetSkill(m_id);
		}
	}
}
