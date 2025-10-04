using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对话触发者
/// </summary>
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("设置是否需要按键开启")]
    public bool isNeedPressKeyCode = true;
    public KeyCode dialogueKeyCode = KeyCode.Space;

    [Header("开启 对话ID")]
    public int dialogueID;

    [Header("提示界面")]
    public TipPanel tipPanel;

    protected bool isCollisionPlayer = false;

    protected const string dialogueTip = "Press the spacebar to initiate a conversation";

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isNeedPressKeyCode)
            {
                DialogueManager.Instance.StartDialogue(dialogueID);
            }
            else
            {
                isCollisionPlayer = true;
                tipPanel.ShowTip(dialogueTip);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollisionPlayer = false;
            tipPanel.HideTip();
        }
    }

    protected virtual void Update()
    {
        if (isCollisionPlayer)
        {
            if (Input.GetKeyDown(dialogueKeyCode))
            {
                DialogueManager.Instance.StartDialogue(dialogueID);
                isCollisionPlayer = false;
                tipPanel.HideTip();
            }
        }
    }
}
