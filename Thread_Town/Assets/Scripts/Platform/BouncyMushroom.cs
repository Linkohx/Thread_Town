using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BouncyMushroom : MonoBehaviour
{
    public float bounceHeight = 8f;  // 目标弹跳高度（米）
    public bool onlyWhenFalling = true; // 只在玩家向下时触发

    void OnTriggerStay(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (!rb) return;

        if (onlyWhenFalling && rb.velocity.y > -0.05f) return; // 不是下落就不弹（可关）

        // v = sqrt(2gh)  → 把竖直速度抬到目标值
        float vy = Mathf.Sqrt(2f * Physics.gravity.magnitude * Mathf.Max(0.01f, bounceHeight));
        if (rb.velocity.y < vy)
        {
            Vector3 v = rb.velocity;
            v.y = vy;
            rb.velocity = v;
        }
    }
}
