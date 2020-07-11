using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;

    public  bool isUse;//当前是否可以点击鼠标左键旋转角度，false表示可以旋转，true表示不可以旋转

    public int hudConut;//当前已经卸载了的轮子数量

    /// <summary>
    /// 拆卸
    /// </summary>
    public  bool isDisassemble;
    /// <summary>
    /// 组装
    /// </summary>
    public  bool isAssemble;


    /// <summary>
    /// 拆解
    /// </summary>
    public  bool isResolve;
    /// <summary>
    /// 组合
    /// </summary>
    public  bool isCombination;

    //设置为拆卸状态
    public void IsDisassemble (bool isb)
    {
        isDisassemble = isb;
    }
    //设置为组装状态
    public void IsAssemble(bool isb)
    {
        isAssemble = isb;
    }
    //设置为分解状态
    public void IsResolve(bool isb)
    {
        isResolve = isb;
    }
    //设置为组合状态
    public void IsCombination(bool isb)
    {
        isCombination = isb;
    }
    public void Awake()
    {
        instance = this;
    }
    
}
