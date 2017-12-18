using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	private Transform player;
	private Vector3 offset;
    private bool isRotating = false;

	public float scrollSpeed = 10;   //视野移动速度
    public float distance;			 //用于控制鼠标中间缩放视野，即相机与玩家的距离
    public float rotateSpeed = 2;    //视野旋转速度

	void Start () {
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		offset = transform.position - player.position;

    }
	
	void Update () {
		transform.position = offset + player.position;

		if(UICamera.isOverUI == false)
		{
			//视野远近
			ScrollView();

			//视野旋转
			RotateView();
		}
    }

	//鼠标中键控制视野远近
	void ScrollView()
	{
		//magnitude向量的长度，获取相机与玩家的距离
		distance = offset.magnitude;
		
		//距离减去中键的滑动，就可以得到视野移动的功能
		//减去是因为向后推滚轮是返回负值，向前是正
		distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        //控制最小与最大距离
        distance = Mathf.Clamp(distance, 5, 20);

        /*
         * normalized不改变向量方向，并且返回长度为1的向量
         * 返回的这个向量坐标已经规范化了，和原来的已经不一样了
         * 
         * 向量的规格化：就是让向量的长度等于1;  向量长度 length = sqrt(x² + y² + z²);
         * 要让长度=1，那么向量 V(normalize) = V(x/length,y/length,z/length);
         * 所以他的返回长度为1的向量是这个意思，而且不管多大归一化之后一定是1
         * 控制向量的方向和坐标在0~1之间，然后按照这个模型乘以我们需要的长度还原成原来的向量并且完成了缩放
         * */
        offset = offset.normalized * distance;
    }


    private void RotateView()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }


        if (isRotating)
        {
            //玩家左右旋转 沿着y轴 通过鼠标x轴
            transform.RotateAround(player.position, player.up, Input.GetAxis("Mouse X") * rotateSpeed);

			//记录相机沿x轴改变之前的数据
			Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            //上下转动是相机本身 沿x轴 通过鼠标y轴
            transform.RotateAround(player.position, transform.right, -Input.GetAxis("Mouse Y") * rotateSpeed);

			//欧拉角是属性面板上的数据，但是控制旋转的是四元数rotation
			float x = transform.eulerAngles.x;

            //超出范围就让x轴回到上一次的数据
            if (x < 10 || x > 80)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

            //修改旋转之后需要重新计算偏移值
            offset = transform.position - player.position;
        }
    }
}
