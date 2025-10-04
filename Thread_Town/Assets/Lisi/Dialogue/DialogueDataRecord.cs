using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]

[CreateAssetMenu(fileName = "DialogueDataRecord", menuName = "Inventory/DialogueData")]
public class DialogueDataRecord : ScriptableObject
{
    [Header("剧情数据")]
    public DialogueData[] dialogueDatas;
}

/// <summary>
/// 一个对话数据
/// </summary>
[Serializable]
public class BaseDialogue
{
    public string name;
    public string content;
    public Sprite character;
}

/// <summary>
/// 一段剧情数据
/// </summary>
[Serializable]
public class DialogueData
{
    [Header("剧情ID（要求唯一）")]
    public int id;
    [Header("剧情开始时事件")]
    public UnityEvent startActionEvent;
    [Header("剧情结束时事件")]
    public UnityEvent endActionEvent;
    [Header("对话数据")]
    public BaseDialogue[] datas;
}
