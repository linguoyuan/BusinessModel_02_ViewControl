using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Hub[] hubs;//车毂，实际没什么作用，这里只起取到车轮wheel作用
    public Wheel[] wheels;

    public Animator _animator; 

    public void Awake()
    {
        hubs = GetComponentsInChildren<Hub>();
        wheels = GetComponentsInChildren<Wheel>();
        _animator = GetComponent<Animator>();
    }

    //3、进入分解状态
    public void OpenResolve()
    {
        for (int i = 0; i < hubs.Length ; i++)
        {
            hubs[i].wheel = wheels[i];
            wheels[i].transform.parent = hubs[i].transform;
            wheels[i].hub = hubs[i];
            wheels[i]._rigidbody.isKinematic = true;//允许运动
            wheels[i]._collider.enabled = true;//激活碰撞器，允许物理属性
            wheels[i]._collider.isTrigger = false;//不允许穿越
            wheels[i].gameObject.layer = LayerMask.NameToLayer("Default"); ;
        }

        //打开动画
        _animator.enabled = true;
        //待机状态转换到拆解状态
        _animator.SetBool("Open", true);

        //四个状态，将分解状态置位为true，其他为false
        Main.instance.isResolve = true;
        Main.instance.isCombination = false;
        Main.instance.isAssemble = false;
        Main.instance.isDisassemble = false;
    }

    //4、按下组合按键
    public void OpenCombination()
    {
        for (int i = 0; i < hubs.Length; i++)
        {
            hubs[i].wheel = wheels[i];
            wheels[i].transform.parent = hubs[i].transform;
            wheels[i].hub = hubs[i];
            wheels[i]._rigidbody.isKinematic = true;
            wheels[i]._collider.enabled = true;
            wheels[i]._collider.isTrigger = false;
            wheels[i].gameObject.layer = LayerMask.NameToLayer("Default"); ;
        }

        _animator.enabled = true;
        _animator.SetBool("Open", false);//过度到收回轮子动画状态

        Main.instance.isResolve = false;
        Main.instance.isCombination = true;//此时为组合组合状态
        Main.instance.isAssemble = false;
        Main.instance.isDisassemble = false;
    }

    //2、按下组装按键
    public void OpenResume()
    {
        Debug.Log(Main.instance.isResolve +"==11111=="+ Main.instance.isCombination);
        _animator.enabled = false;
        if (Main.instance.isResolve || Main.instance.isCombination)
        {
            Main.instance.isResolve = false;
            Main.instance.isCombination = false;
            transform.localPosition = Vector3.zero;
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].transform.localPosition = Vector3.zero;//轮子重新归位
            }
        }
    }

    public Wheel CurrentWheel;
    //实现拆轮子功能
    public void DisassembleRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //显示射线，只有在scene视图中才能看到
            Debug.DrawLine(ray.origin, hit.point);

            if (CurrentWheel)
            {
                CurrentWheel.transform.position = hit.point + new Vector3(0, 0.5f, 0);//鼠标移动到哪里，轮子就放到哪里
            }
            else
            {
                Wheel wheel = hit.collider.GetComponent<Wheel>();
                if (wheel)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        CurrentWheel = wheel;
                        CurrentWheel._collider.enabled = false;
                        CurrentWheel._rigidbody.isKinematic = true;//可以运动
                        CurrentWheel.transform.parent = null;
                        if (CurrentWheel.hub)
                        {
                            Main.instance.hudConut++;//拆卸了的轮子+1
                            CurrentWheel.hub.wheel = null;//去除轮子特性
                            CurrentWheel.hub = null;
                        }

                        Main.instance.isUse = true;
                    }
                }
            }
        }
        if (CurrentWheel)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                CurrentWheel._collider.enabled = true;//启用刚体
                CurrentWheel._rigidbody.isKinematic = false;//松开鼠标后，物体不可运动
                CurrentWheel = null;
                Main.instance.isUse = false;//放置好物体后，可以旋转角度
            }
        }
    }
    //实现装轮子功能
    public void AssembleRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //显示射线，只有在scene视图中才能看到
            Debug.DrawLine(ray.origin, hit.point);
            Debug.Log(hit.point);//碰撞坐标


            if (CurrentWheel)
            {
                CurrentWheel.transform.position = hit.point + new Vector3(0, 0.5f, 0);//鼠标移动到哪里，轮子就放到哪里
            }
            else
            {
                Wheel wheel = hit.collider.GetComponent<Wheel>();
                if (wheel&& wheel.hub==null)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        CurrentWheel = wheel;
                        //解决拆卸只拆了部分轮子，然后又开始组装，因为轮子仍然接受射线，位置就会，否则出现的轮子乱闪bug
                        //当前的轮子在鼠标按住时不再接受射线检测，也会导致放置一次不对后，下一次就不能再放了。
                        CurrentWheel.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                        CurrentWheel._collider.isTrigger = true;
                        CurrentWheel._rigidbody.isKinematic = true;
                        CurrentWheel.transform.parent = null;
                        if (CurrentWheel.hub)
                        {
                            CurrentWheel.hub.wheel = null;
                            CurrentWheel.hub = null;
                        }
                        Main.instance.isUse = true;
                    }
                }
            }
        }
        if (CurrentWheel)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CurrentWheel._collider.isTrigger = false;
                CurrentWheel._collider.enabled = true;//启用物理特性
                CurrentWheel._rigidbody.isKinematic = false;
                CurrentWheel = null;
                gameObject.layer = LayerMask.NameToLayer("Default");
                Main.instance.isUse = false;
            }
           else if (CurrentWheel.hub)
            {
                CurrentWheel. transform.localPosition = Vector3.zero;
                CurrentWheel.transform.localEulerAngles = Vector3.zero;
                CurrentWheel = null;
                Main.instance.isUse = false;
            }
        }
    }



    public void Update()
    {
        //处于拆卸状态才进行射线检测，可以将四个轮子逐个拆下来
        if(Main.instance.isDisassemble)
            DisassembleRay();
        //处于组装状态，射线检测，将四个轮子逐个装上去
        if (Main.instance.isAssemble)
            AssembleRay();
    }
}
