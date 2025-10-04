using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 2f;              // �ƶ��ٶȣ���λ/�룩
    public float arriveThreshold = 0.05f;     // �ж�����Ŀ��ľ���
    public float waitTimeAtEnds = 0.5f;       // ����˵��ĵȴ�ʱ�䣨�룩
    public bool startAtPointA = true;         // �Ƿ�� A ������������ B �㣩
    public bool pingPong = true;              // true=ƹ�����أ�false=ѭ�� A->B->A->B������ʱЧ�����ƣ�
    public bool useLocalSpaceForGizmos = false; // ��Ӱ��Gizmos���ӻ�������ʱ�����������ƶ�

    [Header("����·��")]
    public Transform pointA;
    public Transform pointB;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool isMoving = true;
    private int direction = 1; // A->B Ϊ 1��B->A Ϊ -1
    private Vector3 frameVelocity; // ��֡ƽ̨�ٶȣ�����Ҹ��棩

    void Reset()
    {
        // ������ι���ʱ�Զ���������·��
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

        // Ĭ�ϼ���ƽ̨���õı�ǣ��ɰ���ģ�
        gameObject.tag = "MovingPlatform";
    }

    void Start()
    {
        // ����׼��
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;   // ƽ̨���˶�ѧ��������
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (pointA == null || pointB == null)
        {
            Debug.LogError("[PlatformMoveTwoPoints] ���� Inspector ָ�� pointA / pointB ·�㡣");
            enabled = false;
            return;
        }

        // ��ʼλ����Ŀ��
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

        // ���㲢д�뱾֡�ٶȣ�������ҽű�ͨ�� rigidbody.velocity �� GetPlatformVelocity ��ȡ��
        frameVelocity = (next - prev) / Time.fixedDeltaTime;
        rb.velocity = frameVelocity; // isKinematic �²������������ƽ�������ȡ�õ���ֵ��ȷ
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            // �����ж�
            if (Vector3.Distance(rb.position, targetPosition) <= arriveThreshold)
            {
                isMoving = false;

                // ��ȷ����Ŀ��㣬���ۼ����
                rb.MovePosition(targetPosition);
                frameVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;

                // �˵�ȴ�
                if (waitTimeAtEnds > 0f)
                    yield return new WaitForSeconds(waitTimeAtEnds);
                else
                    yield return null;

                // �л���һ��Ŀ��
                if (pingPong)
                {
                    direction *= -1; // ����
                    targetPosition = (direction > 0) ? pointB.position : pointA.position;
                }
                else
                {
                    // ����ѭ������ƹ�Ҽ�����Ч��
                    targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
                }

                isMoving = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// �ṩƽ̨�ٶȸ���ң������׵Ļ�ȡ��ʽ��
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
