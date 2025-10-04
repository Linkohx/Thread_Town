using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DialoguePanel : MonoBehaviour
{
    [Header("名字")]
    public Text name;

    [Header("内容")]
    public Text content;

    [Header("人物")]
    public Image character;

    [Header("继续按钮")]
    public Button continueButton;

    protected CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateName(string updateContent)
    {
        name.text = updateContent;
    }

    public void UpdateContent(string updateContent)
    {
        content.text = updateContent;
    }

    public void UpdateCharacter(Sprite characterSprite)
    {
        character.enabled = characterSprite != null;
        character.sprite = characterSprite;
    }

    #region 展示
    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(IShow());
    }

    public IEnumerator IShow()
    {
        canvasGroup.blocksRaycasts = false;

        while (canvasGroup.alpha < 1.0f)
        {
            yield return null;
            canvasGroup.alpha += Time.deltaTime * 2f;
        }

        canvasGroup.blocksRaycasts = true;

        yield break;
    }
    #endregion

    #region 隐藏
    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(IHide());
    }

    public IEnumerator IHide()
    {
        canvasGroup.blocksRaycasts = false;

        while (canvasGroup.alpha > 0f)
        {
            yield return null;
            canvasGroup.alpha -= Time.deltaTime * 2f;
        }

        yield break;
    }
    #endregion
}
