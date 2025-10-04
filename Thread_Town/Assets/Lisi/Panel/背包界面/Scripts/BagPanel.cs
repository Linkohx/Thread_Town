using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BagPanel : MonoBehaviour
{
    [Header("背包物品预制体")]
    public GameObject bagItemPrefab;

    [Header("背包物品父节点")]
    public Transform bagItemParent;

    private void OnEnable()
    {
        UdpateBagItem();
    }

    private void OnDisable()
    {
        ClearBagItem();
    }

    protected void UdpateBagItem()
    {
        var propDictionary = PropManager.Instance.propDictionary;

        if (propDictionary.Count <= 0)
        {
            Debug.Log("请先设置背包物体数据记录表！目前为空！");
            return;
        }

        foreach (var prop in propDictionary)
        {
            if (prop.Value.count <= 0) { continue; }

            BagItem bagItem = CreateBagItem();
            bagItem.itemSprite.sprite = prop.Value.icon;
            bagItem.itemName.text = prop.Value.name;
            bagItem.itemCount.text = prop.Value.count.ToString();

            PropData propData = propDictionary[prop.Key];
            bagItem.itemButton.onClick.AddListener(() =>
            {
                Debug.Log($"当前点击背包物品信息：ID={propData.id} 名字={propData.name} 数量={propData.count}");
            });
        }
    }

    protected void ClearBagItem()
    {
        BagItem[] bagItems = bagItemParent.GetComponentsInChildren<BagItem>();

        for (int i = 0; i < bagItems.Length; i++)
        {
            Destroy(bagItems[i].gameObject);
        }

        bagItems = null;
    }

    protected BagItem CreateBagItem()
    {
        GameObject newBagItem = Instantiate(bagItemPrefab, bagItemParent);
        BagItem bagItem = newBagItem.GetComponent<BagItem>();
        return bagItem;
    }
}
