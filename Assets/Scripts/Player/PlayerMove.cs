using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物行走动画
/// </summary>
public enum ControlWalkAminState
{
	Idle,
	Moving
}


public class PlayerMove : MonoBehaviour {

	public float speed = 3f;
	public bool isMoving = false;										//鼠标抬起后，玩家还没移动到指定位置
	public ControlWalkAminState animState = ControlWalkAminState.Idle;	//在PlayerAnimation中调用控制动画

	private PlayerAttack attack;										//用于控制是否处于行走状态，避免与攻击冲突
	private PlayerDirection dir;
	private CharacterController player;

	void Start()
	{
		dir = GetComponent<PlayerDirection>();
		player = GetComponent<CharacterController>();
		attack = GetComponent<PlayerAttack>();
	}

	void Update () {
		
		//避免攻击状态下行走
		if(attack.playerState == PlayerState.ControlWalk)
		{
			float distance = Vector3.Distance(transform.position, dir.targetPosition);

			if (distance > 0.3f)
			{
				animState = ControlWalkAminState.Moving;
				isMoving = true;

				//因为移动之前已经已经旋转了玩家的朝向，所以只要给速度即可
				player.SimpleMove(transform.forward * speed);
			}
			else
			{
				isMoving = false;
				animState = ControlWalkAminState.Idle;
			}
		}
	}

	public void SimpleMove(Vector3 targetPosition)
	{
		//朝向target，Direction控制的是点击地面
		transform.LookAt(targetPosition);

		//移动
		player.SimpleMove(transform.forward * speed);
	}
}
