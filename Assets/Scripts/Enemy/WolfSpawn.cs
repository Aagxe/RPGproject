using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour {

	public int maxNum = 5;				//最大数量
	private int currentNum = 0;			//当前数量
	public float timeInterval = 3;      //产生的时间间隔
	private float timer;                //计时器
	public GameObject enemyPrefab;

	private void Update()
	{
		if(currentNum < maxNum)
		{
			timer += Time.deltaTime;
			if(timer > timeInterval)
			{
				Vector3 pos = transform.position;
				pos.x += Random.Range(-5, 5);
				pos.z += Random.Range(-5, 5);

				GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
				go.GetComponent<Wolf>().spawn = this;
				timer = 0;
				currentNum++;
			}
		}
	}

	public void MinusNum()
	{
		currentNum--;
	}
}
