using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BagItem : MonoBehaviour
{
    public Image itemSprite { protected set; get; }
    public Button itemButton { protected set; get; }
    public Text itemName { protected set; get; }
    public Text itemCount { protected set; get; }

    protected void Awake()
    {
        itemSprite = transform.Find("ItemIcon").GetComponent<Image>();
        itemButton = transform.Find("ItemButton").GetComponent<Button>();
        itemName = transform.Find("ItemName").GetComponent<Text>();
        itemCount = transform.Find("ItemCount").GetComponent<Text>();
    }
}
