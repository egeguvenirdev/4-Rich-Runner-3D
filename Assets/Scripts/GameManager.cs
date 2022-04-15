using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ali.Helper;

public class GameManager : MonoBehaviour
{

    private int totalMoney;

    void Start()
    {
        if(HCLevelManager.Instance.GetGlobalLevelIndex() == 0) //if its a new start
        {
            totalMoney = 0;
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
        }

        if(PlayerPrefs.GetInt("TotalMoney") >= 0) //if the total amount and level are higher than  1;
        {
            SetTotalMoney(0);
        }
    }

    private void SetTotalMoney(int collectedAmount)
    {
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 0) + collectedAmount;
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        UIManager.Instance.SetTotalMoney();

        totalMoney = 0;
    }
}
