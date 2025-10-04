using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropDataRecord", menuName = "Inventory/PropDataRecord")]
public class PropDataRecord : ScriptableObject
{
    [Header("道具数据（初始版本）")]
    public PropData[] propDatas;
}


[Serializable]
public class PropData
{
    public int id;
    public Sprite icon;
    public string name;
    public string description;
    public int count;

    public PropData Clone()
    {
        PropData propData = new PropData();
        propData.id = id;
        propData.icon = icon;
        propData.name = name;
        propData.description = description;
        propData.count = count;
        return propData;
    }
}