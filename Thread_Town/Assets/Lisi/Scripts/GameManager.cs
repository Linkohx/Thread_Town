using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SettingPanel settingPanel;

    private void Awake()
    {
        GameData.InitData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            GameData.Delete();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingPanel.gameObject.SetActive(true);
        }
    }
}
