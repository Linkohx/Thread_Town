using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BouncyMushroom : MonoBehaviour
{
    public float bounceHeight = 8f;  // Ŀ�굯���߶ȣ��ף�
    public bool onlyWhenFalling = true; // ֻ���������ʱ����

    void OnTriggerStay(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (!rb) return;

        if (onlyWhenFalling && rb.velocity.y > -0.05f) return; // ��������Ͳ������ɹأ�

        // v = sqrt(2gh)  �� ����ֱ�ٶ�̧��Ŀ��ֵ
        float vy = Mathf.Sqrt(2f * Physics.gravity.magnitude * Mathf.Max(0.01f, bounceHeight));
        if (rb.velocity.y < vy)
        {
            Vector3 v = rb.velocity;
            v.y = vy;
            rb.velocity = v;
        }
    }
}
