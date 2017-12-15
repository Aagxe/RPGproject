using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour {

	public GameObject effectClickPrefab;	//鼠标点击移动的特效
	public Vector3 targetPosition;          //人物需要移动到的目标位置

	private PlayerAttack attack;
	private PlayerMove playerMove;
	private bool isMoving = false;			//鼠标是否一直按下

	void Start()
	{
		//初始位置为自身位置
		targetPosition = transform.position;
		playerMove = GetComponent<PlayerMove>();
		attack = GetComponent<PlayerAttack>();
	}

	void Update () {

		if (attack.playerState != PlayerState.Death)
		{
			//判断鼠标下面是否NGUI，记得加碰撞体
			//本来用UICamera.hoveredObject判断一定会得到UI，没有碰撞体的都能获取
			//attack.isLockingTarget attack脚本必须慢于当前脚本
			if (attack.isLockingTarget == false && Input.GetMouseButtonDown(0) && UICamera.isOverUI == false)
			{
				RaycastHit groundHit;
				if (CheckGroundHit(out groundHit))
				{
					isMoving = true;
					LookRotationTarget(groundHit.point);
					ShowClickEffect(groundHit.point);
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				isMoving = false;
			}

			//鼠标按下状态
			if (isMoving)
			{
				RaycastHit groundHit;
				if (CheckGroundHit(out groundHit))
				{
					LookRotationTarget(groundHit.point);
				}
			}
			//鼠标抬起后，玩家还没移动到目标位置
			else if (playerMove.isMoving)
			{
				//为了避开障碍物可能会导致转向改变，所以必须更新
				LookRotationTarget(targetPosition);
			}
		}
	}

	//鼠标点击显示的特效
	void ShowClickEffect(Vector3 hitPoint)
	{
		GameObject go = ObjectPool.GetInstance().RequestCacheGameObject(effectClickPrefab);
		hitPoint.y += 0.1f;
		go.transform.position = hitPoint;

		StartCoroutine(ReturnCache(go, 0.32f));
	}

	//定时回收资源
	IEnumerator ReturnCache(GameObject go, float time)
	{
		yield return new WaitForSeconds(time);
		ObjectPool.GetInstance().ReturnCacheGameObejct(go);
	}

	//检测射线是否碰撞地面
	private bool CheckGroundHit(out RaycastHit hit)
	{
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(camRay, out hit) && hit.collider.tag == Tags.ground)
		{
			return true;
		}

		return false;
	}

	//控制玩家朝向目标位置
	public void LookRotationTarget(Vector3 targetPos)
	{
		targetPosition = targetPos;

		targetPosition.Set(targetPosition.x, transform.position.y, targetPosition.z);
		transform.LookAt(targetPosition);
	}
}
