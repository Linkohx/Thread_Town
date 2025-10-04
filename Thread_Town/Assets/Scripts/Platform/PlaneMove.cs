using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 2f;              // 移动速度（单位/秒）
    public float arriveThreshold = 0.05f;     // 判定到达目标的距离
    public float waitTimeAtEnds = 0.5f;       // 到达端点后的等待时间（秒）
    public bool startAtPointA = true;         // 是否从 A 点出发（否则从 B 点）
    public bool pingPong = true;              // true=乒乓来回；false=循环 A->B->A->B（两点时效果类似）
    public bool useLocalSpaceForGizmos = false; // 仅影响Gizmos可视化，运行时按世界坐标移动

    [Header("两个路点")]
    public Transform pointA;
    public Transform pointB;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool isMoving = true;
    private int direction = 1; // A->B 为 1，B->A 为 -1
    private Vector3 frameVelocity; // 本帧平台速度（供玩家跟随）

    void Reset()
    {
        // 方便初次挂载时自动创建两个路点
        if (pointA == null)
        {
            var a = new GameObject("PointA").transform;
            a.SetParent(transform.parent, true);
            a.position = transform.position + Vector3.left * 2f;
            pointA = a;
        }
        if (pointB == null)
        {
            var b = new GameObject("PointB").transform;
            b.SetParent(transform.parent, true);
            b.position = transform.position + Vector3.right * 2f;
            pointB = b;
        }

        // 默认加上平台常用的标记（可按需改）
        gameObject.tag = "MovingPlatform";
    }

    void Start()
    {
        // 刚体准备
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;   // 平台用运动学刚体最稳
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (pointA == null || pointB == null)
        {
            Debug.LogError("[PlatformMoveTwoPoints] 请在 Inspector 指定 pointA / pointB 路点。");
            enabled = false;
            return;
        }

        // 初始位置与目标
        if (startAtPointA)
        {
            rb.position = pointA.position;
            targetPosition = pointB.position;
            direction = 1; // A->B
        }
        else
        {
            rb.position = pointB.position;
            targetPosition = pointA.position;
            direction = -1; // B->A
        }

        StartCoroutine(MoveRoutine());
    }

    void FixedUpdate()
    {
        if (!isMoving) { frameVelocity = Vector3.zero; rb.velocity = Vector3.zero; return; }

        Vector3 prev = rb.position;
        Vector3 next = Vector3.MoveTowards(prev, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // 计算并写入本帧速度（方便玩家脚本通过 rigidbody.velocity 或 GetPlatformVelocity 获取）
        frameVelocity = (next - prev) / Time.fixedDeltaTime;
        rb.velocity = frameVelocity; // isKinematic 下不会用于物理推进，但读取得到的值正确
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            // 到达判定
            if (Vector3.Distance(rb.position, targetPosition) <= arriveThreshold)
            {
                isMoving = false;

                // 精确贴到目标点，防累计误差
                rb.MovePosition(targetPosition);
                frameVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;

                // 端点等待
                if (waitTimeAtEnds > 0f)
                    yield return new WaitForSeconds(waitTimeAtEnds);
                else
                    yield return null;

                // 切换下一个目标
                if (pingPong)
                {
                    direction *= -1; // 反向
                    targetPosition = (direction > 0) ? pointB.position : pointA.position;
                }
                else
                {
                    // 两点循环（与乒乓几乎等效）
                    targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
                }

                isMoving = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 提供平台速度给玩家（更稳妥的获取方式）
    /// </summary>
    public Vector3 GetPlatformVelocity()
    {
        return frameVelocity;
    }

    void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;

        Vector3 a = useLocalSpaceForGizmos && transform.parent ? transform.parent.TransformPoint(pointA.localPosition) : pointA.position;
        Vector3 b = useLocalSpaceForGizmos && transform.parent ? transform.parent.TransformPoint(pointB.localPosition) : pointB.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawWireSphere(a, 0.12f);
        Gizmos.DrawWireSphere(b, 0.12f);
    }
}
