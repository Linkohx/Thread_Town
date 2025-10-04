using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对话管理器
/// </summary>
public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        UnityEvent unityEvent = new UnityEvent();
    //        unityEvent.AddListener(() =>
    //        {
    //            StartDialogue(1002);
    //        });
    //        StartDialogue(1001, unityEvent);
    //    }
    //}

    [Header("剧情面板")]
    public DialoguePanel dialoguePanel;

    [Header("剧情数据")]
    public DialogueDataRecord dialogueDataRecord;

    [Header("字符展示间隔")]
    public float charShowIntervale = 0.02f;

    protected void Init()
    {
        if (dialoguePanel == null)
        {
            Debug.Log($"{this.GetType()}：DialoguePanel 没有拖动设置！");
            return;
        }

        if (dialogueDataRecord == null)
        {
            Debug.Log($"{this.GetType()}：DialogueDataRecord 没有拖动设置！");
            return;
        }

        dialogueDataDictionary = new Dictionary<int, DialogueData>();
        for (int i = 0; i < dialogueDataRecord.dialogueDatas.Length; i++)
        {
            DialogueData dialogueData = dialogueDataRecord.dialogueDatas[i];
            dialogueDataDictionary.Add(dialogueData.id, dialogueData);
        }
    }

    protected Dictionary<int, DialogueData> dialogueDataDictionary;
    protected DialogueData nowDialogueData;     //当前对话数据
    protected UnityEvent dialogueEndEvent;

    public void StartDialogue(int dialogueID, UnityEvent endEvent = null)
    {
        if (dialogueCoroutine != null) { StopDialogue(); }

        if (!dialogueDataDictionary.ContainsKey(dialogueID))
        {
            Debug.Log($"不存在剧情ID：{dialogueID}");
            return;
        }

        dialogueEndEvent = endEvent;
        nowDialogueData = dialogueDataDictionary[dialogueID];
        dialoguePanel.Show();
        dialogueCoroutine = StartCoroutine(IDialogue());
    }

    public void StopDialogue()
    {
        if (dialogueCoroutine == null) { return; }
        StopAllCoroutines();
        dialogueCoroutine = null;
        dialoguePanel.Hide();
        nowDialogueData = null;
        dialogueEndEvent = null;
    }

    protected Coroutine dialogueCoroutine;
    protected IEnumerator IDialogue()
    {
        yield return null;

        bool isContinue = false;
        WaitUntil waitUntil = new WaitUntil(() => { return isContinue; });
        dialoguePanel.continueButton.onClick.AddListener(() => { isContinue = true; });

        for (int i = 0; i < nowDialogueData.datas.Length; i++)
        {
            isContinue = false;

            BaseDialogue baseDialogue = nowDialogueData.datas[i];

            dialoguePanel.UpdateName(baseDialogue.name);
            dialoguePanel.UpdateCharacter(baseDialogue.character);
            dialoguePanel.continueButton.interactable = false;

            yield return IVerbatim(baseDialogue.content);

            dialoguePanel.continueButton.interactable = true;

            yield return waitUntil;
        }

        dialoguePanel.Hide();
        dialogueCoroutine = null;
        nowDialogueData = null;

        dialogueEndEvent?.Invoke();
        dialogueEndEvent = null;

        yield break;
    }

    protected IEnumerator IVerbatim(string updateContent)
    {
        int charCount = 0;
        WaitForSeconds waitForSeconds = new WaitForSeconds(charShowIntervale);

        while (charCount < updateContent.Length)
        {
            charCount++;
            dialoguePanel.UpdateContent(updateContent.Substring(0, charCount));
            yield return waitForSeconds;
        }

        waitForSeconds = null;

        yield break;
    }
}

/*
 * 触发方：通过按键触发。
 * 回答方：根据目前进度开始对话。
 * 对话管理：触发开始事件 && UI淡入->逐字输出->点击继续->触发结束事件 && UI淡出
 * 对话数据：
 */