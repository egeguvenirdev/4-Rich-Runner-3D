using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Ali.Helper;

public class UIManager : MonoSingleton<UIManager>
{
    private Bermuda.Runner.BermudaRunnerCharacter bermudaRunnerCharacter;

    //Main UIs
    [SerializeField] private GameObject tapToPlayUI;
    [SerializeField] private GameObject nextLvMenuUI;
    [SerializeField] private GameObject restartMenuUI;
    [Space]

    //pause button ui uthilities
    [SerializeField] private GameObject pausedText;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle vibrationToggle;

    [Space]
    //status texts UIs
    [SerializeField] private TMP_Text currentLV;
    [SerializeField] private TMP_Text totalMoneyText;
    [SerializeField] private TMP_Text collectedMoneyText;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private Button multiplierButton;
    [SerializeField] private TMP_Text multiplierButtonText;
    [SerializeField] private TMP_Text multiplierGameText;
    [SerializeField] private Image multiplierImage;
    private int multiplier;
    private int multiplierCounter = 0;

    [Space]
    //status texts
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject poorText;
    [SerializeField] private GameObject richText;
    [SerializeField] private GameObject avarageText;

    [Space]
    //randommultiplier selection
    [SerializeField] private RectTransform arrowImage;
    private bool clickBonusCheck = false;

    public bool isPaused;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("vibrationOnOff") == 0)
        {
            vibrationToggle.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt("soundOnOff") == 0)
        {
            soundToggle.GetComponent<Toggle>().isOn = false;
        }
    }

    private void Start()
    {
        isPaused = true;
        DOTween.Init();
        LevelText();
    }

    public void PlayResButton()
    {
        if (tapToPlayUI.activeSelf)
        {
            tapToPlayUI.SetActive(false);
            isPaused = false;
            PlayerManagement.Instance.CanRun();
        }

        if (nextLvMenuUI.activeSelf)
        {
            nextLvMenuUI.SetActive(false);
            isPaused = false;
            ResMultiplierButton();

            HCLevelManager.Instance.LevelUp();
            LevelText();
            PlayerManagement.Instance.CharacterReset();
            HCLevelManager.Instance.GenerateCurrentLevel();
        }

        if (restartMenuUI.activeSelf)
        {
            restartMenuUI.SetActive(false);
            isPaused = false;

            PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney")
                + PlayerManagement.Instance.currentLvMoneyAmount);
            SetTotalMoney();

        }
    }

    public void NextLvUI()
    {
        if (!isPaused) //if the game not stopped
        {
            tapToPlayUI.SetActive(false);
            nextLvMenuUI.SetActive(true);
            isPaused = true;
            //collectedMoneyText.text = "" + PlayerManagement.Instance.currentLvMoneyAmount + "$";
        }
        Invoke("MoveMultiplierArrow", 0.5f);
    }

    public void RestartButtonUI()
    {
        if (!isPaused) //if the game not stopped
        {
            restartMenuUI.SetActive(true);
            isPaused = true;
        }
    }

    public void PauseButtonUI()
    {
        if (!isPaused) //if the game not stopped
        {
            //pausedText.SetActive(true);
            tapToPlayUI.SetActive(true);
            isPaused = true;
        }
    }

    public void UIVibrationToggle(bool checkOnOff)
    {
        if (checkOnOff)
        {
            vibrationToggle.GetComponent<Toggle>().isOn = true;
            PlayerPrefs.SetInt("vibrationOnOff", 1);
        }
        else
        {
            vibrationToggle.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("vibrationOnOff", 0);
        }
    }

    public void UISoundToggle(bool checkOnOff)
    {
        if (checkOnOff)
        {
            soundToggle.GetComponent<Toggle>().isOn = true;
            PlayerPrefs.SetInt("soundOnOff", 1);
        }
        else
        {
            soundToggle.GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetInt("soundOnOff", 0);
        }
    }

    public void LevelText()
    {
        int levelInt = HCLevelManager.Instance.GetGlobalLevelIndex() + 1;
        currentLV.text = "Level " + levelInt;
        //nextLV.text = "Lv " + (PlayerPrefs.GetInt("CurrentLevel") + 1);
    }

    public void SetProgress(float progress)
    {
        //slider.value = progress;
        progressBarImage.fillAmount = progress;
        //progressText.text = "% " + progress;
    }

    public void SetTotalMoney()
    {
        totalMoneyText.text = "" + PlayerPrefs.GetInt("TotalMoney", 0) + "$";
    }

    public void ChangeStatusText(int status)
    {
        if (status == 0)
        {
            avarageText.SetActive(false);
            richText.SetActive(false);
            poorText.SetActive(true);
            progressBarImage.color = Color.red;
        }

        if (status == 1)
        {
            richText.SetActive(false);
            poorText.SetActive(false);
            avarageText.SetActive(true);
            progressBarImage.color = new Color(1.0f, 0.64f, 0.0f);
        }

        if (status == 2)
        {
            poorText.SetActive(false);
            avarageText.SetActive(false);
            richText.SetActive(true);
            progressBarImage.color = Color.green;
        }
    }

    private void MoveMultiplierArrow()
    {
        arrowImage.DORotate(arrowImage.forward * 90, 0.01f);
        arrowImage.DORotate(arrowImage.forward * -90, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(HandTransform());
    }

    IEnumerator HandTransform()
    {
        float arrowAngle = arrowImage.eulerAngles.z;
        Debug.Log(arrowImage.eulerAngles.z);
        if (arrowAngle > 45 && arrowAngle < 90)
        {
            SetMuiltiplier("Get 1X", 1);
        }

        if (arrowAngle > 0 && arrowAngle < 45)
        {
            SetMuiltiplier("Get 2X", 2);
        }

        if (arrowAngle > 315 && arrowAngle < 360)
        {
            SetMuiltiplier("Get 3X", 3);
        }

        if (arrowAngle > 270 && arrowAngle < 315)
        {
            SetMuiltiplier("Get 4X", 4);
        }

        if (!clickBonusCheck)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(HandTransform());
        }
        else
        {
            MultiplierButton(true);
        }
    }

    private void SetMuiltiplier(string textString, int multiplierInt)
    {
        multiplierButtonText.text = textString;
        multiplier = multiplierInt;

        if (multiplierInt == 1)
        {
            multiplierImage.color = new Color32(231, 12, 12, 255);
        }

        if (multiplierInt == 2)
        {
            multiplierImage.color = new Color32(255, 153, 21, 255);
        }

        if (multiplierInt == 3)
        {
            multiplierImage.color = new Color32(250, 205, 51, 255);
        }

        if (multiplierInt == 4)
        {
            multiplierImage.color = new Color32(105, 179, 76, 255);
        }
    }

    public void MultiplierButton(bool coroutineCheck)
    {
        clickBonusCheck = true;
        StopCoroutine(HandTransform());
        DOTween.Kill(arrowImage.transform);
        multiplierButton.interactable = false;

        if (coroutineCheck)
        {
            multiplierGameText.text = "You Won";
            multiplierButtonText.text = PlayerManagement.Instance.currentLvMoneyAmount * multiplier + "$";
            Debug.Log("test");
            PlayerManagement.Instance.currentLvMoneyAmount *= multiplier;
            nextLevelButton.SetActive(true);
            PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney")
                + PlayerManagement.Instance.currentLvMoneyAmount);
            SetTotalMoney();
        }
    }
    public void ResMultiplierButton()
    {
        clickBonusCheck = false;
        nextLevelButton.SetActive(false);
        multiplierGameText.text = "Tap to Win";
        multiplierButtonText.text = "Get 1X";
        multiplierButton.interactable = true;
    }

    public void UIQuitGame()
    {
        Application.Quit();
    }
}
