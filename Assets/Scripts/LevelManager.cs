using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("StartLevel", 1);

        if (PlayerPrefs.GetInt("CurrentLevel", 0) == 0)
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("StartLevel"));
        }
    }
}
