using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Coordinate
{
    Top,
    Left,
    Right,
    Bottom
}


public class UIPositionMove : MonoBehaviour {


	public Coordinate coordinate;
	public float speed = 5.5f;
	public Transform anchorDependent;       //锚点依赖

	private bool isPlay = false;            //控制正序播放还是反序播放
	private bool isFinish = false;			//是否播放完动画

	private int spriteWidth;
	private int spriteHeight;
	private UISprite sprite;
	private Vector4 originalAnctorPosition;
	private Vector3 originalPosition;

	void Awake()
	{
		sprite = GetComponent<UISprite>();

		spriteWidth = sprite.width;
		spriteHeight = sprite.height;

		originalPosition = transform.position;
		originalAnctorPosition.Set(
			sprite.leftAnchor.absolute,
			sprite.rightAnchor.absolute,
			sprite.topAnchor.absolute,
			sprite.bottomAnchor.absolute);
	}


	void Update() {

		switch (coordinate)
		{
			case Coordinate.Right:
				Vector3 move = Vector3.zero;
				if (isPlay)
				{
					//抛物线的返回数值
					float x = easeOutQuart(transform.position.x, anchorDependent.position.x, speed);
					//乘0.1是因为太快
					move.Set(x * 0.1f, 0, 0);
				}
				else
				{
					float x = easeOutQuart(transform.position.x, originalPosition.x, speed);
					move.Set(x * 0.1f, 0, 0);

				}

				isFinish = Mathf.Abs(move.x) < 1 ? true : false;

				sprite.leftAnchor.absolute -= (int)move.x;
				sprite.rightAnchor.absolute = sprite.leftAnchor.absolute + spriteWidth;

				break;
		}
	}


	private float easeOutQuart(float start, float end, float value)
	{
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
	}

	/// <summary>
	/// 正序播放
	/// </summary>
	public void PlayForward()
	{
		isFinish = false;
		isPlay = true;
	}

	/// <summary>
	/// 倒叙播放
	/// </summary>
	public void PlayReverse()
	{
		//为了避免没有正序播放直接反序播放
		if(isPlay)
		{
			isPlay = false;		//控制正序播放还是倒叙
			isFinish = false;
		}
	}

	/// <summary>
	/// 播放完毕后关闭面板
	/// </summary>
	public void PlayFinishEnable()
	{
		if(isFinish)
		{
			gameObject.SetActive(false);
		}
		else
		{
			StartCoroutine(WaitFinish());
		}
	}

	private IEnumerator WaitFinish()
	{
		while(true)
		{
			if (isFinish == false)
				yield return new WaitForEndOfFrame();
			else
			{
				gameObject.SetActive(false);
				break;
			}
		}

	}

	/// <summary>
	/// 重设锚点就等于重设位置了
	/// </summary>
	public void ResetPosition()
	{
		sprite.leftAnchor.absolute   = (int)originalAnctorPosition.x;
		sprite.rightAnchor.absolute  = (int)originalAnctorPosition.y;
		sprite.topAnchor.absolute    = (int)originalAnctorPosition.z;
		sprite.bottomAnchor.absolute = (int)originalAnctorPosition.w;
	}
}
