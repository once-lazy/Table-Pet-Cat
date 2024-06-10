using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float stop_time;//计时器
    private float want_time = 5;//计时器终止时间
    private bool activt = false;//是否处于运动状态
    private Vector3 mWorldPos;
    private float distance; //设置鼠标触发跟随的距离
    private bool run = false;//是否处于跑步
    private bool sleep = false;//是否处于睡眠
    public bool Run{get{return run;}set{run=value;} }//跑步的setget方法
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();//获取该组件，用作后续转向
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mP = Input.mousePosition;
        // 将鼠标在屏幕上的位置转换为世界空间中的位置
        mWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mP.x, mP.y, Camera.main.transform.position.z));

        distance = mWorldPos.x - transform.position.x;
        float dis_y = mWorldPos.y - transform.position.y;
        if (run)
        {
            activt = true;
        }
        else if (distance < 2 && distance > -2 && dis_y < 1 && dis_y > -1)
        {
            activt = true;
        }
        else
        {
            activt = false;
        }


    }

    void FixedUpdate()
    {
        if (activt && (distance > 0.2f || distance < -0.2f))
        {
            if (rb.velocity.x == 0 && run)
            {
                animator.SetTrigger("run_start");
            }
            else if (rb.velocity.x == 0)
            {
                animator.SetTrigger("walk_start");
            }
            Move();
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            //animator.SetTrigger("idle");
            rb.velocity = new Vector3(0, 0, 0);
            if (!sleep)
            {
                isAction();
            }

        }
    }
    //移动
    private void Move()
    {
        sleep = false;
        if (run)
        {
            rb.velocity = new Vector3(distance > 0 ? 2 : -2.5f, 0, 0);
            animator.SetBool("run", true);
        }
        else
        {
            rb.velocity = new Vector3(distance > 0 ? 0.8f : -0.8f, 0, 0);
            animator.SetBool("walk", true);
        }


        if (distance > 0)
        {
            sr.flipX = false;
        }
        else if (distance < 0)
        {
            sr.flipX = true;
        }
    }
    //动作
    private void isAction()
    {
        stop_time += Time.deltaTime;
        if (stop_time > want_time)
        {
            int num = Random.Range(0, 7);
            switch (num)
            {
                case 0:
                    animator.SetTrigger("idle");
                    break;
                case 1:
                    animator.SetTrigger("itch");
                    break;
                case 2:
                    animator.SetTrigger("laying");
                    sleep = true;
                    break;
                case 3:
                    animator.SetTrigger("licking");
                    break;
                case 4:
                    animator.SetTrigger("licking2");
                    break;
                case 5:
                    animator.SetTrigger("meow");
                    break;
                case 6:
                    animator.SetTrigger("stretching");
                    break;
            }
            want_time = Random.Range(5, 10);
            stop_time = 0;
        }
    }
}
