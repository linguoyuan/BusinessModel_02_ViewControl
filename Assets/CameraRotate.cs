using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public float macDas = 5;

    public float minDis = 1;

    public float minAngle = 30;
    public float macAngle = 80;

    private Camera _camera;

    public Transform 轴2;
    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }
    public void Update()
    {
        if (!Main.instance. isUse)
        {
            if (Input.GetMouseButton(0))
            {
                SetRotate();
            }
            if (Input.GetMouseButton(1))
            {
                SetRotate();
            }
            _camera.transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"));
        }
        if (Input.GetMouseButton(1))
        {
            SetRotate();
        }

        float dis = Vector3.Distance(_camera.transform.localPosition, Vector3.zero);

        if (-dis > -minDis)
            _camera.transform.localPosition = new Vector3(0, 0, -minDis);
        if (-dis < -macDas)
            _camera.transform.localPosition = new Vector3(0, 0, -macDas);
    }

    private void SetRotate()
    {
        Vector3 eulerAngles = transform.localEulerAngles;
        Vector3 eulerAngles_2 = 轴2.localEulerAngles;
        //镜头负责y轴方向的旋转，也就是水平（左右）方向旋转
        transform.localEulerAngles = new Vector3(eulerAngles.x, eulerAngles.y + Input.GetAxis("Mouse X"), eulerAngles.z);
        //轴负责x轴方向的旋转，也就是垂直（上下）方向旋转
        轴2.localEulerAngles = new Vector3(eulerAngles_2.x - Input.GetAxis("Mouse Y"), eulerAngles_2.y, eulerAngles_2.z);

        
        if (轴2.localEulerAngles.x < minAngle)
            轴2.localEulerAngles = new Vector3(minAngle, 0, 0);
        if (轴2.localEulerAngles.x > macAngle)
            轴2.localEulerAngles = new Vector3(macAngle, 0, 0);

        //这种办法视角办法不是很好，角度限定也有bug
        //transform.localEulerAngles = new Vector3(eulerAngles.x - Input.GetAxis("Mouse Y"), eulerAngles.y + Input.GetAxis("Mouse X"), eulerAngles_2.z);
        //if (transform.localEulerAngles.x < minAngle)
        //    transform.localEulerAngles = new Vector3(minAngle, 0, 0);
        //if (transform.localEulerAngles.x > macAngle)
        //    transform.localEulerAngles = new Vector3(macAngle, 0, 0);
    }
}
