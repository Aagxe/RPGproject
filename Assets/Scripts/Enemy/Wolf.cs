using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ai的行为
/// </summary>
public enum WolfState
{
	Idle,
	Walk,
	Attack,
	Death
}

public class Wolf : MonoBehaviour {

	//组件
	private Animation anim;
	private CharacterController enemy;
	private Renderer m_renderer;
	private PlayerStatus playerStatus;
	private PlayerAttack playerAttack;
	private GameObject player;

	//HUDText
	public GameObject hudTextPrefab;
	private GameObject hudTextFollow;
	private GameObject hudTextGo;
	private HUDText hudText;
	private UIFollowTarget followTarget;

	//动画
	public string AnimName_Idle;
	public string AnimName_Walk;
	public string AnimName_NormalAttack;
	public string AnimName_CrazyAttack;
	public string AnimName_Damager;
	public string AnimName_Death;
	private string AnimName_Now;

	public WolfState state = WolfState.Idle;

	private bool isCoroutine = false;			 //协同是否启动,不然连续攻击的时候可能会导致多个协同工作
	private Color normalColor;                   //保存原来的颜色

	                                             //动画控制
	public float timeNormalAttack;				 //攻击的播放时间，动画播放时间上看到的数值
	public float timeCrazyAttack;
	private float patrolTimer = 0;               //巡逻计时器，每一个状态的超时时间
	public float duration = 1;					 //巡逻状态各动画持续时间
	private string animNameCurrentAttack;        //当前是哪一种攻击(普通，疯狂，闲置idle)

	//ai属性
	public int hp = 100;                         //ai生命
	public int exp = 20;						 //死亡掉落经验值
	public int attack = 10;						 //ai攻击
	public float speed = 1;                      //ai移动速度
	public float miss = 0.2f;                    //五分之一的闪避概率
	public float crazyAttackChance = 0.25f;      //疯狂攻击的概率
	public AudioClip missSound;					 //miss音效
	public Transform target;                     //攻击的目标
	public float minDistance = 2;                //最小攻击距离
	public float maxDistance = 6;
	public float attackRate = 1;                 //攻击速率每秒,越大越快
	private float attackTimer;                   //攻击计时器，计时攻击速率
	public WolfSpawn spawn;						 //当前敌人属于的出生点


	private void Awake()
	{
		anim = GetComponent<Animation>();
		enemy = GetComponent<CharacterController>();
		m_renderer = GetComponentInChildren<Renderer>();
		normalColor = m_renderer.material.color;

		hudTextFollow = transform.Find("HUDText").gameObject;
		
		//默认动画
		AnimName_Now = AnimName_Idle;
		animNameCurrentAttack = AnimName_NormalAttack;
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerStatus = player.GetComponent<PlayerStatus>();
		playerAttack = player.GetComponent<PlayerAttack>();

		hudTextGo = HUDTextParent.instance.gameObject.AddChild(hudTextPrefab);
		hudText = hudTextGo.GetComponent<HUDText>();
		followTarget = hudTextGo.GetComponent<UIFollowTarget>();

		followTarget.target = hudTextFollow.transform;
		followTarget.gameCamera = Camera.main;
		followTarget.uiCamera = UICamera.currentCamera;

	}

	private void Update()
	{
		//死亡
		if (state == WolfState.Death)
		{
			anim.CrossFade(AnimName_Death);

			//anim的死亡动画大于1就代表播放完毕,为了稍微延迟所以改大了点，详细见AnimationState
			//只有Loop的动画才能使用这个判断，否则Once的动画会重复播放清除掉播放进度
			if (anim[AnimName_Death].normalizedTime >= 1.2f)
			{
				//通知生成器狼被杀死
				spawn.MinusNum();

				//增加经验
				playerStatus.PlusExp(exp);

				//如果在任务中就增加杀死数量
				BarNPC.instance.OnKillWolf();

				Destroy(gameObject);
			}
		}
		//自动攻击
		else if(state == WolfState.Attack)
		{
			AutoAttack();
		}
		//巡逻,将Idle和Walk作为巡逻
		else
		{
			anim.CrossFade(AnimName_Now);

			if(AnimName_Now == AnimName_Walk)
			{
				enemy.SimpleMove(transform.forward * speed);
			}

			//计时，当超过时间改变巡逻状态
			patrolTimer += Time.deltaTime;
			if (patrolTimer > duration)
			{
				RandomPatrolState();
				patrolTimer = 0;
			}
		}
	}


	//随机巡逻状态
	private void RandomPatrolState()
	{
		int value = Random.Range(0, 2);

		switch(value)
		{
			case 0:
				AnimName_Now = AnimName_Idle;
				state = WolfState.Idle;
				break;
			case 1:
				//如果上一个状态是idle，就随机生成旋转方向
				if (AnimName_Now != AnimName_Walk)
				{
					transform.Rotate(transform.up * Random.Range(0,361));
				}

				AnimName_Now = AnimName_Walk;
				state = WolfState.Walk;
				break;
		}
	}

	/// <summary>
	/// 受到伤害
	/// </summary>
	public void TakeDamage(int attack)
	{
		if (state == WolfState.Death)
			return;

		//转换为攻击状态
		state = WolfState.Attack;
		//target会被设置为null，所以要每一次受伤都设置
		target = player.transform;

		float value = Random.Range(0f, 1f);

		//miss效果
		if (value < miss)
		{
			AudioSource.PlayClipAtPoint(missSound, transform.position);
			hudText.Add("Miss", Color.gray, 1);
		}
		//打中
		else
		{
			ShowEffect();
			//掉血多少
			hudText.Add("-" + attack, Color.red, 1);

			hp -= attack;
			if(hp <= 0)
			{
				state = WolfState.Death;
			}
		}
	}

	private void ShowEffect()
	{
		if (isCoroutine == false)
		{
			StartCoroutine(ShowEffectRed());
		}
		else
		{
			m_renderer.material.color = Color.red;
		}

	}

	private IEnumerator ShowEffectRed()
	{
		isCoroutine = true;

		m_renderer.material.color = Color.red;

		while (true)
		{
			yield return new WaitForEndOfFrame();
			m_renderer.material.color = Color.Lerp(m_renderer.material.color, normalColor, 5 * Time.deltaTime);

			//低于这个值插值会变得比较慢，效果也不明显，所以直接变回原色退出
			if (m_renderer.material.color.r < 0.7f)
			{
				m_renderer.material.color = normalColor;
				break;
			}
		}

		isCoroutine = false;
	}

	private void AutoAttack()
	{
		if (target == null || target.GetComponent<PlayerAttack>().playerState == PlayerState.Death)
		{
			state = WolfState.Idle;
			target = null;
			return;
		}


		float distance = Vector3.Distance(transform.position, target.position);

		//朝向target
		Vector3 offset = target.position - transform.position;
		offset.y = 0;
		transform.rotation = Quaternion.LookRotation(offset);

		//超出了距离，停止攻击
		if (distance > maxDistance)
		{
			target = null;
			state = WolfState.Idle;
		}
		//攻击
		else if(distance < minDistance)
		{
			attackTimer += Time.deltaTime;

			//动画
			anim.CrossFade(animNameCurrentAttack);
			  
			//普通攻击
			if(animNameCurrentAttack == AnimName_NormalAttack)
			{
				//动画播放完毕
				if(attackTimer > timeNormalAttack)
				{
					//产生伤害
					target.GetComponent<PlayerAttack>().TakeDamage(attack);
					//闲置直到超过攻击速率时间
					animNameCurrentAttack = AnimName_Idle;
				}
			}
			//疯狂攻击
			else if(animNameCurrentAttack == AnimName_CrazyAttack)
			{
				//动画播放完毕
				if (attackTimer > timeCrazyAttack)
				{
					//产生伤害
					target.GetComponent<PlayerAttack>().TakeDamage(attack);
					//闲置直到超过攻击速率时间
					animNameCurrentAttack = AnimName_Idle;
				}
			}

			//计时大于攻击速率，重新执行攻击
			if(attackTimer > (1/attackRate))
			{
				RandomAttack();
				attackTimer = 0;
			}
		}
		//跟踪目标
		else
		{
			//移动
			enemy.SimpleMove(transform.forward * speed);

			//动画
			anim.CrossFade(AnimName_Walk);
		}
	}


	/// <summary>
	/// 随机攻击类型
	/// </summary>
	private void RandomAttack()
	{
		float value = Random.Range(0f, 1f);

		//疯狂攻击的概率
		if(value < crazyAttackChance)
		{
			animNameCurrentAttack = AnimName_CrazyAttack;
		}
		//普通攻击
		else
		{
			animNameCurrentAttack = AnimName_NormalAttack;
		}
	}

	private void OnDestroy()
	{
		Destroy(hudTextGo);
	}

	private void OnMouseEnter()
	{
		if(playerAttack.isLockingTarget == false)
			CursorManager.instance.SetAttack();
	}

	private void OnMouseExit()
	{
		if (playerAttack.isLockingTarget == false)
			CursorManager.instance.SetNormal();
	}
}
