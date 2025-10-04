using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TipPanel : MonoBehaviour
{
    public GameObject tipGameObject;
    public Text tipText;

    public void ShowTip(string tipContent)
    {
        tipText.text = tipContent;
        tipGameObject.SetActive(true);
    }

    public void HideTip()
    {
        tipGameObject.SetActive(false);
    }
}
