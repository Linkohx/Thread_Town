using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    private static PropManager instance;
    public static PropManager Instance => instance;

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

    public PropDataRecord propDataRecord;

    public Dictionary<int, PropData> propDictionary { protected set; get; }

    protected void Init()
    {
        propDictionary = new Dictionary<int, PropData>();

        for (int i = 0; i < propDataRecord.propDatas.Length; i++)
        {
            PropData propData = propDataRecord.propDatas[i].Clone();
            propDictionary.Add(propDataRecord.propDatas[i].id, propData);
        }
    }

    public void Collect(int id, int collectCount = 1)
    {
        if (!propDictionary.ContainsKey(id))
        {
            Debug.Log($"当前没有道具ID为{id}，请前往PropDataRecord文件中添加道具的信息！！！");
            return;
        }

        propDictionary[id].count += collectCount;

        Debug.Log($"{propDictionary[id].name}：当前拥有{propDictionary[id].count}");
    }

    public int GetCount(int id)
    {
        return propDictionary[id].count;
    }
}
