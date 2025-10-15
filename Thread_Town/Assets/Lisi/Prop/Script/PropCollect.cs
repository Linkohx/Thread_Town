using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PropCollect : MonoBehaviour
{
    protected Collider collider;
    protected Rigidbody rigidbody;

    [Header("收集按键")]
    public KeyCode collectKeyCode = KeyCode.F;

    [Header("收集提示")]
    public TipPanel tipPanel;

    protected Prop collectProp;
    protected bool isCanCheckCollect = false;

    protected const string collectTip = "Press the F key to collect";

    void Awake()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // void Start()
    // {
    //     collider.isTrigger = true;
    // }

    private void OnTriggerEnter(Collider other)
    {
        Prop prop = other.GetComponent<Prop>();
        if (prop != null)
        {
            tipPanel.ShowTip(collectTip);
            collectProp = prop;
            isCanCheckCollect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Prop prop = other.GetComponent<Prop>();
        if (collectProp == null) { return; }
        if (prop != null && prop.GetInstanceID() == collectProp.GetInstanceID())
        {
            tipPanel.HideTip();
            collectProp = null;
            isCanCheckCollect = false;
        }
    }

    private void Update()
    {
        if (isCanCheckCollect)
        {
            if (collectProp == null) { return; }
            if (Input.GetKeyDown(collectKeyCode))
            {
                collectProp.Collect();
                tipPanel.HideTip();
                collectProp = null;
            }
        }
    }
}
