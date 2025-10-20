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

    [Header("视频界面")]
    public VedioPanel vedioPanel;

    public bool npcState
    { protected set; get; }

    protected void Awake()
    {
        npcState = false;
        SetNPCState(npcState);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            vedioPanel.PlayVedio();
        }
    }
#endif

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
                DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
                dialogueTrigger.dialogueID = finishedDialogueID;
                dialogueTrigger.dialogueEvent.RemoveAllListeners();
                dialogueTrigger.dialogueEvent.AddListener(() => { vedioPanel.PlayVedio(); });
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
