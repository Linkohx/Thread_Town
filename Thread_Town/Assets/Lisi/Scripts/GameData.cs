using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    //需要保存的数据定义在这里，如：
    public static int charcterLevel;

    public static void InitData()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            Load();
        }
        else
        {
            PlayerPrefs.SetString("GameData", Time.time.ToString());

            Init();
        }
    }

    protected static void Init()
    {
        charcterLevel = 1;

        Save();
        Debug.Log("首次进入游戏，初始化数据！");
    }

    protected static void Load()
    {
        charcterLevel = PlayerPrefs.GetInt("CharacterLevel");

        Debug.Log("游戏数据 加载完成！");
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("CharacterLevel", charcterLevel);

        Debug.Log("游戏数据保存完成！");
    }

    public static void Delete()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("游戏数据清理完成！");
    }
}
