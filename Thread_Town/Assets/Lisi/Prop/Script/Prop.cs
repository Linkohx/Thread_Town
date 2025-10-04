using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Prop : MonoBehaviour
{
    protected Collider collider;
    protected Rigidbody rigidbody;

    [Header("道具ID")]
    public int id;

    [Header("数量")]
    public int count = 1;

    [Header("是否收集后消失")]
    public bool isCollectDisappear = true;

    protected virtual void Awake()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();

        collider.isTrigger = true;
    }

    public virtual void Collect()
    {
        PropManager.Instance.Collect(id, count);
        gameObject.SetActive(!isCollectDisappear);
    }
}
