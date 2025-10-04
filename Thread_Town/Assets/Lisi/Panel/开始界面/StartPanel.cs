using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class StartPanel : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("退出游戏：保存数据！");
        GameData.Save();
        Application.Quit();
    }
}
