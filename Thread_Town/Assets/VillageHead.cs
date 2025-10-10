using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHead : MonoBehaviour
{
    [Header("对话相关")]
    public int unfinishedDialogueID;
    public int finishedDialogueID;

    [Header("NPC相关")]
    public NPCController[] npcControllers;

    public bool npcState { protected set; get; }

    protected void Awake()
    {
        npcState = false;
        SetNPCState(npcState);
    }

    public void SetNPCState(bool state)
    {
        npcState = state;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcState = CheckAllNPCState();
            if (npcState)
            {
                GetComponent<DialogueTrigger>().dialogueID = finishedDialogueID;
            }
            else
            {
                GetComponent<DialogueTrigger>().dialogueID = unfinishedDialogueID;
            }
        }
    }

    bool CheckAllNPCState()
    {
        foreach (var npc in npcControllers)
        {
            if (!npc.npcState)
            {
                return false;
            }
        }
        return true;
    }
}
