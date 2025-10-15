using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("模型相关")]
    public GameObject unfinishedModel;
    public GameObject finishedModel;

    [Header("对话相关")]
    public int unfinishedDialogueID;
    public int finishedDialogueID;

    [Header("道具相关")]
    public int needPropID;
    public int needPropCount;

    public bool npcState { protected set; get; }
    protected bool isDone;

    protected void Awake()
    {
        isDone = false;
        npcState = false;
        SetNPCState(npcState);
    }

    void Update()
    {
        if (isDone)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Finish();
                isDone = false;
            }
        }
    }

    public void SetNPCState(bool state)
    {
        npcState = state;
        UpdateNPCState();
    }

    protected void UpdateNPCState()
    {
        if (npcState)
        {
            TransitionPanel.Instance.transitionEvent.AddListener(() =>
            {
                unfinishedModel.SetActive(false);
                finishedModel.SetActive(true);
            });
            TransitionPanel.Instance.Show();
        }
        else
        {
            finishedModel.SetActive(false);
            unfinishedModel.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcState = PropManager.Instance.GetCount(needPropID) >= needPropCount;
            if (npcState)
            {
                GetComponent<DialogueTrigger>().dialogueID = finishedDialogueID;
                isDone = true;
            }
            else
            {
                GetComponent<DialogueTrigger>().dialogueID = unfinishedDialogueID;
            }
        }
    }

    public void Finish()
    {
        PropManager.Instance.Reduce(needPropID, needPropCount);
        SetNPCState(true);
        GetComponent<Collider>().enabled = false;
    }
}
