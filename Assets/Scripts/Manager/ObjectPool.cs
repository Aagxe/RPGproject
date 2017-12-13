using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	private static ObjectPool m_Instance;
	private static GameObject CachePanel;
	
	//存储对象实例,根据预制物体的ID
	private Dictionary<string, Queue<GameObject>> m_Pool = new Dictionary<string, Queue<GameObject>>();

	//记录出队实例，避免在外部创建的数据进来了
	private Dictionary<GameObject, string> m_Tag = new Dictionary<GameObject,string>();

	private  ObjectPool() {	}

	public static ObjectPool GetInstance()
	{
		if (m_Instance == null)
		{
			m_Instance = new ObjectPool();
		}

		return m_Instance;
	}


	/// <summary>
	/// 回收GameObject
	/// 返回对象函数，对象池有进有出，当外部功能用完资源后，通过该函数重新让资源入池。
	/// 这里处理了让对象重新进入队列，同时关闭物体激活和设置父物体。
	/// </summary>
	/// <param name="go"></param>
	public void ReturnCacheGameObejct(GameObject go)
	{
		if (CachePanel == null)
		{
			CachePanel = new GameObject();
			CachePanel.name = "CachePanel";
			GameObject.DontDestroyOnLoad(CachePanel);
		}

		if (go == null)
			return;

		go.transform.parent = CachePanel.transform;
		go.SetActive(false);

		//从对象池生成的就去除标记并进队，否则销毁
		if(m_Tag.ContainsKey(go))
		{
			string tag = m_Tag[go];
			RemoveOutMark(go);

			if (m_Pool.ContainsKey(tag) == false)
			{
				m_Pool[tag] = new Queue<GameObject>();
			}

			m_Pool[tag].Enqueue(go);
		}
		else 
		{
			GameObject.Destroy(go);
		}
	}	


	/// <summary>
	/// 请求对象
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	public GameObject RequestCacheGameObject(GameObject prefab)
	{
		string tag = prefab.GetInstanceID().ToString();
		GameObject go = GetFromPool(tag);

		if (go == null)
		{
			go = GameObject.Instantiate<GameObject>(prefab);
			go.name = prefab.name + Time.time;
		}

		MarkAsOut(go, tag);
		return go;
	}

	private GameObject GetFromPool(string tag)
	{
		if (m_Pool.ContainsKey(tag) && m_Pool[tag].Count > 0)
		{
			GameObject obj = m_Pool[tag].Dequeue();
			obj.SetActive(true);
			return obj;
		}
		else 
		{
			return null;
		}
	}

	private void MarkAsOut(GameObject go, string tag)
	{
		m_Tag.Add(go,tag);
	}

	private void RemoveOutMark(GameObject go)
	{
		if (go != null)
		{
			m_Tag.Remove(go);
		}
		else
		{
			Debug.LogError("remove out mark error, gameObject has not been marked");
		}

	}
}
