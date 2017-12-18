using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物的行为
/// </summary>
public enum PlayerState
{
	ControlWalk,
	NormalAttack,
	SkillAttack,
	Death
}

/// <summary>
/// 控制攻击的动画
/// </summary>
public enum AttackAnimState
{
	Moving,
	Idle,
	Attack
}

public class PlayerAttack : MonoBehaviour {

	private Transform targetNormalAttack;		    //普通攻击的对象，为了区分技能攻击
	private PlayerMove playerMove;
	private PlayerDirection playerDir;
	private Animation anim;
	private PlayerStatus playerStatus;
	private Renderer bodyRenderer;                  //控制受伤改变颜色
	public AudioClip missSound;
	public AudioClip buffSound;
	public AudioClip normalAttackSound;
	public GameObject body;

	//HUDText
	public GameObject hudTextPrefab;
	private GameObject hudTextGo;
	private HUDText hudText;
	private GameObject hudTextFollow;
	
	//动画
	public string animNormalAttack;
	public string animIdle;
	public string animDeath;
	private string animNow;
	private string animSkill;
	private float animTimer;                        //动画计时器
	public GameObject AttackEffect;					//攻击特效

	//属性
	private float attackRate = 0.75f;				//攻击速率/s，1.5秒一次
	private float timerNormalAttack = 0.833f;       //普通攻击的时间
	private bool isShownEffect = false;             //是否显示过特效
	private bool isAttacked = false;				//改变攻击速度之后，会导致动画还没播放完，攻击就重设了
	public float minDistance = 5;                   //最小攻击距离
	public float miss = 0.25f;

	//状态
	public PlayerState playerState = PlayerState.ControlWalk;
	public AttackAnimState attackAnimState = AttackAnimState.Idle;

	private bool _isLockingTarget = false;		 //是否在选择目标
	private bool isCoroutine = false;
	private Color normalColor;                   //保存原来的颜色
	private SkillInfo skillInfo = null;

	public GameObject[] efxArray;                //存储技能特效
	private Dictionary<string, GameObject> efxDictionary = new Dictionary<string, GameObject>();

	public bool isLockingTarget
	{
		get { return _isLockingTarget; }
	}


	private void Awake()
	{
		playerStatus = GetComponent<PlayerStatus>();
		playerMove = GetComponent<PlayerMove>();
		playerDir = GetComponent<PlayerDirection>();
		anim = GetComponent<Animation>();
		hudTextFollow = transform.Find("HUDText").gameObject;

		bodyRenderer = body.GetComponent<Renderer>();
		normalColor = bodyRenderer.material.color;

		animNow = animNormalAttack;
		animSkill = animIdle;
	}

	private void Start()
	{
		hudTextGo = HUDTextParent.instance.gameObject.AddChild(hudTextPrefab);
		hudText = hudTextGo.GetComponent<HUDText>();

		UIFollowTarget follow = hudTextGo.GetComponent<UIFollowTarget>();
		follow.target = hudTextFollow.transform;
		follow.gameCamera = Camera.main;
		follow.uiCamera = UICamera.currentCamera;

		foreach(GameObject efx in efxArray)
		{
			efxDictionary.Add(efx.name, efx);
		}
	}

	private void Update()
	{

		if (isLockingTarget == false && Input.GetMouseButtonDown(0) && playerState != PlayerState.Death)
		{
			//控制需要攻击还是走路
			ControlPlayerStatus();
		}

		if(playerState == PlayerState.NormalAttack)
		{
			OnAttack();
		}
		else if(playerState == PlayerState.SkillAttack)
		{
			anim.CrossFade(animSkill);

			//必须冻结旋转角度
			Vector3 v = transform.eulerAngles;
			v.x = 0;
			v.z = 0;
			transform.eulerAngles = v;
		}
		else if(playerState == PlayerState.Death)
		{
			anim.CrossFade(animDeath);

			DeadthPanel.instance.OnShowDeadthPanel();
		}


		if(isLockingTarget && Input.GetMouseButtonDown(0))
		{
			OnLockTarget();
		}
	}

	private void ControlPlayerStatus()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//点击到敌人，转换为攻击状态自动进行普通攻击
		if (Physics.Raycast(ray, out hit) && hit.collider.tag == Tags.enemy)
		{
			playerState = PlayerState.NormalAttack;
			targetNormalAttack = hit.transform;

			animTimer = 0;
			isShownEffect = false;
			isAttacked = false;
		}
		//否则转换为行走，不然没办法退出攻击状态
		else
		{
			playerState = PlayerState.ControlWalk;
			targetNormalAttack = null;
		}
	}

	private void OnAttack()
	{
		if (targetNormalAttack == null)
		{
			playerState = PlayerState.ControlWalk;

			//这样可以避免攻击结束后又跑到之前指定的位置
			playerDir.targetPosition = transform.position;
			return;
		}

		playerDir.LookRotationTarget(targetNormalAttack.position);
		float distance = Vector3.Distance(targetNormalAttack.position, transform.position);

		//进行攻击
		if (distance <= minDistance)
		{
			//必须改变攻击状态，避免PlayerAnimation中同时执行跑的动画
			attackAnimState = AttackAnimState.Attack;

			animTimer += Time.deltaTime;
			anim.CrossFade(animNow);

			//动画播放完毕
			if (animTimer >= timerNormalAttack)
			{
				animNow = animIdle;
				//攻击效果是否显示过
				if (isShownEffect == false)
				{
					isAttacked = true;
					isShownEffect = true;
					//攻击特效
					Instantiate(AttackEffect, targetNormalAttack.position, Quaternion.identity);
					targetNormalAttack.GetComponent<Wolf>().TakeDamage(GetAttack());

					AudioSource.PlayClipAtPoint(normalAttackSound, targetNormalAttack.position);
				}
			}

			//计时大于攻击速率，重新启动攻击
			if ((animTimer > (1f / attackRate)) && isAttacked)
			{
				animTimer = 0;
				animNow = animNormalAttack;

				isShownEffect = false;
				isAttacked = false;
			}
		}
		//走向敌人
		else
		{
			//设置状态即可，动画的播放在PlayerAnimation中控制了
			attackAnimState = AttackAnimState.Moving;
			playerMove.SimpleMove(targetNormalAttack.position);
		}
	}

	/// <summary>
	/// 返回总攻击力
	/// </summary>
	public int GetAttack()
	{
		return EquipmentUI.instance.attack + (int)playerStatus.attack_base + playerStatus.attack_plus;
	}


	public void TakeDamage(float attack)
	{
		if (playerState == PlayerState.Death)
			return;
		float def = EquipmentUI.instance.defense + playerStatus.defense_base + playerStatus.defense_plus;

		//防御公式(200 - def) / 200
		float temp = attack * ((200 - def) / 200);
		if(temp < 1)
			temp = 1;

		//闪避几率
		float value = Random.Range(0f,1f);
		
		//miss
		if (value < miss)
		{
			AudioSource.PlayClipAtPoint(missSound, transform.position);
			hudText.Add("MISS", Color.gray, 1);
		}
		else
		{
			ShowEffect();

			//掉血多少
			hudText.Add("-" + temp, Color.red, 1);

			//扣除血量，更新显示
			playerStatus.hp_remain -= temp;
			HeadStatusUI.instance.UpdatePropertyShow();

			if (playerStatus.hp_remain <= 0)
			{
				playerState = PlayerState.Death;
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
			bodyRenderer.material.color = Color.red;
		}

	}

	private IEnumerator ShowEffectRed()
	{
		isCoroutine = true;

		bodyRenderer.material.color = Color.red;

		while (true)
		{
			yield return new WaitForEndOfFrame();
			bodyRenderer.material.color = Color.Lerp(bodyRenderer.material.color, normalColor, 5 * Time.deltaTime);

			//低于这个值插值会变得比较慢，效果也不明显，所以直接变回原色退出
			if (bodyRenderer.material.color.r < 0.7f)
			{
				bodyRenderer.material.color = normalColor;
				break;
			}
		}

		isCoroutine = false;
	}

	private void OnDestroy()
	{
		Destroy(hudTextGo);
	}

	public void UseSkill(SkillInfo info)
	{
		//避免出错
		if (playerStatus.heroType == HeroType.Magician)
			if (info.applicableRole == ApplicableRole.Swordman)
				return;

		if (playerStatus.heroType == HeroType.Swordman)
			if (info.applicableRole == ApplicableRole.Magician)
				return;

		//不同类型技能使用不同方式触发
		switch(info.applyType)
		{
			case ApplyType.Passive:
				playerStatus.GetMP(info.mp);
				StartCoroutine(OnUsePassiveSkill(info));
				break;
			case ApplyType.Buff:
				playerStatus.GetMP(info.mp);
				StartCoroutine(OnUseBuffSkill(info));
				break;
			case ApplyType.SingleTarget:
				//mp扣除放在了真正释放技能的函数中
				OnUseAttackSkill(info);
				break;
			case ApplyType.MultiTarget:
				OnUseAttackSkill(info);
				break;
		}
	}

	private IEnumerator OnUsePassiveSkill(SkillInfo info)
	{
		AudioSource.PlayClipAtPoint(buffSound, transform.position);

		//设置状态，播放动画
		playerState = PlayerState.SkillAttack;

		//设置动画
		animSkill = info.animNmae;
		yield return new WaitForSeconds(info.animTime);
		animSkill = animIdle;

		//因为是增益技能，所以可以播放完动画就改状态
		playerState = PlayerState.ControlWalk;

		//根据类型增加属性
		int hp = 0, mp = 0;
		if (info.applyProperty == ApplyProperty.HP)
			hp = info.applyValue;
		else if (info.applyProperty == ApplyProperty.MP)
			mp = info.applyValue;

		//治疗
		playerStatus.Remedy(hp, mp);

		//播放技能特效
		GameObject go = Instantiate(efxDictionary[info.efxName], transform.position, Quaternion.identity);
		go.transform.parent = transform;
	}
	
	private IEnumerator OnUseBuffSkill(SkillInfo info)
	{
		AudioSource.PlayClipAtPoint(buffSound, transform.position);

		//设置状态，播放动画
		playerState = PlayerState.SkillAttack;

		//设置动画
		animSkill = info.animNmae;
		yield return new WaitForSeconds(info.animTime);
		animSkill = animIdle;

		//因为是增强技能，所以可以播放完动画就改状态
		playerState = PlayerState.ControlWalk;

		//播放技能特效
		GameObject go = Instantiate(efxDictionary[info.efxName], transform.position, Quaternion.identity);
		go.transform.parent = transform;

		//增加属性
		switch (info.applyProperty)
		{
			case ApplyProperty.Attack:
				//因为是百分比所以要/100
				playerStatus.attack_base *= (info.applyValue / 100f);
				break;
			case ApplyProperty.AttackSpeed:
				attackRate *= (info.applyValue / 100f);
				break;
			case ApplyProperty.Defense:
				playerStatus.defense_base *= (info.applyValue / 100f);
				break;
			case ApplyProperty.Speed:
				playerMove.speed *= (info.applyValue / 100f);
				break;
		}

		//等待buff时间结束
		yield return new WaitForSeconds(info.applyTime);

		//取消属性
		switch (info.applyProperty)
		{
			case ApplyProperty.Attack:
				playerStatus.attack_base /= (info.applyValue / 100f);
				break;
			case ApplyProperty.AttackSpeed:
				attackRate /= (info.applyValue / 100f);
				break;
			case ApplyProperty.Defense:
				playerStatus.defense_base /= (info.applyValue / 100f);
				break;
			case ApplyProperty.Speed:
				playerMove.speed /= (info.applyValue / 100f);
				break;
		}
	}
	private void OnUseAttackSkill(SkillInfo info)
	{
		//设置状态
		playerState = PlayerState.SkillAttack;

		//更改鼠标
		CursorManager.instance.SetLockTarget();

		//锁定敌人
		_isLockingTarget = true;

		skillInfo = info;

		//剩下的在update中判断鼠标左键是否按下，并调用
	}

	//选择目标完成，开始释放技能
	private void OnLockTarget()
	{
		if(skillInfo != null)
		{
			//这样可以避免攻击结束后又跑到之前指定的位置
			playerDir.targetPosition = transform.position;

			switch (skillInfo.applyType)
			{
				case ApplyType.SingleTarget:
					StartCoroutine(OnLockSingleTarget());
					break;
				case ApplyType.MultiTarget:
					StartCoroutine(OnLockMultiTarget());
					break;
			}
		}

		_isLockingTarget = false;
	}

	private IEnumerator OnLockSingleTarget()
	{
		CursorManager.instance.SetNormal();

		//判断鼠标按下的位置是否敌人，是就释放技能
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(camRay, out hit) && hit.collider.tag == Tags.enemy)
		{
			transform.LookAt(hit.transform);

			playerStatus.GetMP(skillInfo.mp);

			//设置动画
			animSkill = skillInfo.animNmae;
			yield return new WaitForSeconds(skillInfo.animTime);
			animSkill = animIdle;

			playerState = PlayerState.ControlWalk;

			//播放技能特效
			GameObject go = Instantiate(efxDictionary[skillInfo.efxName], hit.transform.position, Quaternion.identity);
			go.transform.parent = hit.transform;

			//造成伤害
			hit.collider.GetComponent<Wolf>().TakeDamage((int)(GetAttack() * (skillInfo.applyValue / 100f)));
		}
		else
		{
			//如果不处理的话，会导致技能释放状态下还能移动
			playerState = PlayerState.NormalAttack;
		}
	}


	private IEnumerator OnLockMultiTarget()
	{
		CursorManager.instance.SetNormal();

		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(camRay, out hit, LayerMask.GetMask("Ground")))
		{
			transform.LookAt(hit.transform);

			playerStatus.GetMP(skillInfo.mp);

			//设置动画
			animSkill = skillInfo.animNmae;
			yield return new WaitForSeconds(skillInfo.animTime);
			animSkill = animIdle;

			playerState = PlayerState.ControlWalk;

			//播放技能特效
			GameObject go = Instantiate(efxDictionary[skillInfo.efxName], hit.point + Vector3.up, Quaternion.identity);
			//传递伤害
			go.GetComponent<MagicSphere>().attack = (int)(GetAttack() * (skillInfo.applyValue / 100f));
		}
		else
		{
			playerState = PlayerState.NormalAttack;
		}

	}

	/// <summary>
	/// 显示MP不足
	/// </summary>
	public void NotEnoughMP()
	{
		hudText.Add("MP不足", Color.blue, 1);
	}
}
