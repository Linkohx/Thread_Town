using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [Header("旋转轴（单位向量）")]
    public Vector3 axis = new Vector3(0, 1, 0); // 默认绕Y轴旋转

    [Header("角速度（度/秒）")]
    public float speed = 60f;

    [Header("世界坐标旋转？(true=世界轴, false=本地轴)")]
    public bool useWorldSpace = false;

    void Update()
    {
        if (useWorldSpace)
            transform.Rotate(axis, speed * Time.deltaTime, Space.World);
        else
            transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
    }
}
