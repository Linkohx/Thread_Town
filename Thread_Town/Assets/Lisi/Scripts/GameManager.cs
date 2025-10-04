using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }
}
