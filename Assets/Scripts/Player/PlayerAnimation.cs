using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	private PlayerMove move;        //move持有行走的动画
	private PlayerAttack attack;    //attack持有人物攻击状态动画和人物的行为

	public string moving;
	public string idle;

	void Start () {
		move = GetComponent<PlayerMove>();
		attack = GetComponent<PlayerAttack>();
	}
	

	//确保PlayerMove中的Update执行完毕
	void LateUpdate () {

		if(attack.playerState == PlayerState.ControlWalk)
		{
			switch (move.animState)
			{
				case ControlWalkAminState.Idle:
					PlayAnimation(idle);
					break;
				case ControlWalkAminState.Moving:
					PlayAnimation(moving);
					break;
			}
		}
		else if(attack.playerState == PlayerState.NormalAttack)
		{
			switch (attack.attackAnimState)
			{
				case AttackAnimState.Moving:
					PlayAnimation(moving);
					break;
			}
		}
	}

	private void PlayAnimation(string animName)
	{
		//动画淡入淡出
		GetComponent<Animation>().CrossFade(animName);
	}
}
