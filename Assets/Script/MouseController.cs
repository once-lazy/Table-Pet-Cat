using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    public GameObject setUI;//设置按钮UI
    public Text runText;//UI中是奔跑还是慢走的文本
    private GameObject cat; //用于查询当前的猫咪对象
    private float MaxTime = 5;//设置按钮最大存在时间
    private float waitTime = 0;//计算按钮当前存在时间
    public GameObject[] cats; //当前猫咪种类总数
    private int cat_index = 0;//用于切换猫的下标
    private bool isCat = false;//判断鼠标是否选中猫

    // Start is called before the first frame update
    void Start()
    {
        setUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mouseDetection();
        if (setUI.activeSelf)
        {
            waitTime += Time.deltaTime;
            if (waitTime > MaxTime)    //判断存在时间
            {
                setUI.SetActive(false);
                waitTime = 0;
            }
        }
    }

    //鼠标射线检测
    private void mouseDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //鼠标射线实现与2D交互
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);
            // 判断射线与物体相交，处理鼠标点击事件
            if (hit.collider)
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.tag == "Cat")
                {
                    isCat = true;
                    if (cat == null)
                    {
                        cat = clickedObject;
                    }


                }
            }
        }
        if (isCat && Input.GetMouseButton(0))
        {
            //将使猫被鼠标拖动
            Vector3 mP = Input.mousePosition;
            cat.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mP.x, mP.y, 10));

            //出现按钮交互，并调整到猫咪上方
            if (!setUI.activeSelf)
            {
                setUI.SetActive(true);
            }

            //先将世界坐标转为屏幕坐标
            Vector2 uiPos = Camera.main.transform.InverseTransformPoint(cat.transform.position);
            if (cat.transform.position.y < 0)
            {
                setUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiPos.x * 100, uiPos.y * 100 + 50);
            }
            else
            {
                setUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiPos.x * 100, uiPos.y * 100 - 50);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isCat)
            {
                isCat = false;
            }
        }


    }

    public void toExit()
    {
        Application.Quit();//退出
    }

    public void toMove()
    {
        //转换喵咪脚本的run的状态
        CatController catController = cat.GetComponent<CatController>();
        catController.Run = !catController.Run;
        //切换按钮文字
        if (catController.Run)
        {
            runText.text = "慢步";
        }
        else
        {
            runText.text = "奔跑";
        }
        setUI.SetActive(false);
    }

    public void switchCat()
    {
        //获取猫咪总数
        int num = cats.Length;
        if (num <= 0)
        {
            Debug.Log("忘记添加猫咪预制体");
        }
        else
        {
            //获取下一只猫咪的预制体，然后生成
            cat_index++;
            cat_index %= num;
            GameObject cat_new = GameObject.Instantiate(cats[cat_index], cat.transform.position, cat.transform.rotation);
            //销毁原来那只
            Destroy(cat);
            //将当前的cat赋给对象中
            cat = cat_new;
        }
        runText.text = "奔跑";

    }
}
