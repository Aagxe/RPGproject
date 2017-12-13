using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicSphere : MonoBehaviour {

    public float attack = 0;

	//存储已经被本次技能伤害过的，避免二次伤害
    private List<Wolf> wolfList = new List<Wolf>();

	public void OnTriggerEnter(Collider col)
	{
		if (col.tag == Tags.enemy)
		{
			Wolf enemy = col.GetComponent<Wolf>();

			//查找列表中是否存在此对象，不存在返回-1
			int index = wolfList.IndexOf(enemy);
			if (index == -1)
			{
				enemy.TakeDamage((int)attack);
				wolfList.Add(enemy);
			}
		}
	}

}
