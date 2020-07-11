using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Collider _collider;
    public Rigidbody _rigidbody;

    public Hub hub;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        hub = GetComponentInParent<Hub>();
    }

    //与车毂发生碰撞时---模拟吸附功能
    public void OnTriggerEnter(Collider other)
    {
        //组装状态，4个轮子处于散的状态
        if (Main.instance. isAssemble)
        {
            Hub hub = other.GetComponent<Hub>();
            if (hub)//车毂存在
            {
                if (hub.wheel == null)
                {
                    hub.wheel = this;
                    this.hub = hub;
                    Main.instance.hudConut--;//散装的轮子数量减一
                    gameObject.layer = LayerMask.NameToLayer("Default");//装好后重新接受射线检测
                    _rigidbody.isKinematic = true;//重新赋予可运动属性
                    _collider.enabled = true;//重新启用碰撞器
                    _collider.isTrigger = false;//重新激活物理属性，不会发生穿入
                    transform.parent = hub.transform;//设置父物体
                    transform.localPosition = Vector3.zero;//重新调整位置和角度
                    transform.localEulerAngles = Vector3.zero;
                }
            }
        }
    }
}
