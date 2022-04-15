using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManagement : MonoSingleton<PlayerManagement>
{
    //skins & skin names
    [SerializeField] private GameObject poorUI;
    [SerializeField] private GameObject richUI;
    [SerializeField] private GameObject avarageUI;
    [Space]
    [SerializeField] private GameObject localPosition;
    [SerializeField] private GameObject localMoverTarget;
    [SerializeField] private GameObject character;
    [SerializeField] private Bermuda.Runner.BermudaRunnerCharacter bermudaRunnerCharacter;
    [Space]

    private bool isStarted = false;
    private bool canRun = true;
    public float characterProgress = 0;
    public int currentLvMoneyAmount;
    private int status = 0;

    void Start()
    {
        DOTween.Init();
        currentLvMoneyAmount = 0;
        bermudaRunnerCharacter.IdleAnimation();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && canRun)
        {
            bermudaRunnerCharacter._running = true;
            bermudaRunnerCharacter.SetRotateEnabled(true);
            bermudaRunnerCharacter.SetEnabled(true);
            bermudaRunnerCharacter.StartToRun();
            isStarted = true;
            canRun = false;
        }
    }

    public void AddMoney(int collectedAmount)
    {
        currentLvMoneyAmount += collectedAmount;
        if (0 > currentLvMoneyAmount + collectedAmount)
        {
            currentLvMoneyAmount = 0;
        }

        characterProgress += collectedAmount;
        SetUIProgress();

        if (status == 0 && (characterProgress + collectedAmount) <= 0)
        {
            characterProgress = 0;
        }

        if (status == 2 && (characterProgress + collectedAmount) >= 100)
        {
            characterProgress = 100;
        }

        if (status == 1) //avarage
        {
            if (characterProgress > 100) //turn to rich form
            {
                characterProgress -= 100;
                status = 2;

                PlayParticle();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }

            if (characterProgress < 0)//turn to poor form
            {
                characterProgress = 100 + collectedAmount;
                status = 0;

                PlayParticle();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }
        }

        if (status == 2) //rich
        {
            if (characterProgress < 0) //turn to avarage form
            {
                characterProgress = 100 + collectedAmount;
                status = 1;

                PlayParticle();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }
        }

        if (status == 0) //poor
        {
            if (characterProgress > 100) //turn to avarage form
            {
                characterProgress -= 100;
                status = 1;

                PlayParticle();
                ChangeApperance(status);
                SetUIProgress();
                return;
            }
        }
    }

    public void StartToDance()
    {
        bermudaRunnerCharacter._running = false;
        bermudaRunnerCharacter.SetRotateEnabled(false);
        canRun = false;
        bermudaRunnerCharacter.DanceAnimation();
        character.transform.DORotate(new Vector3(0, 180, 0), 1);
        UIManager.Instance.NextLvUI();
    }

    private void PlayParticle()
    {
        var particle = ObjectPooler.Instance.GetPooledObject("ApperanceParticle");
        particle.transform.position = localPosition.transform.position + new Vector3(0f, 1f, 1f);
        particle.transform.rotation = localPosition.transform.rotation;
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();
    }

    private void SetUIProgress()
    {
        float floatCharacterProgress = characterProgress / 100;
        UIManager.Instance.SetProgress(floatCharacterProgress);
    }

    private void ChangeApperance(int statusValue)
    {
        if (statusValue == 0)
        {
            avarageUI.SetActive(false);
            richUI.SetActive(false);
            poorUI.SetActive(true);

            bermudaRunnerCharacter.InjuredRunAnimation();
            UIManager.Instance.ChangeStatusText(status);
        }

        if (statusValue == 1)
        {
            richUI.SetActive(false);
            poorUI.SetActive(false);
            avarageUI.SetActive(true);

            bermudaRunnerCharacter.WalkAnimation();
            UIManager.Instance.ChangeStatusText(status);
        }

        if (statusValue == 2)
        {
            poorUI.SetActive(false);
            avarageUI.SetActive(false);
            richUI.SetActive(true);
            bermudaRunnerCharacter.CatwalkRunAnimation();
            UIManager.Instance.ChangeStatusText(status);
        }

    }

    public void CanRun()
    {
        canRun = true;
    }

    public void CharacterReset()
    {
        characterProgress = 0;
        status = 0;
        isStarted = false;
        currentLvMoneyAmount = 0;

        character.transform.rotation = Quaternion.Euler(0, 0, 0);
        localMoverTarget.transform.localPosition = new Vector3(0, 0, 0.9f);

        ChangeApperance(status);
        SetUIProgress();

        bermudaRunnerCharacter.IdleAnimation();
        bermudaRunnerCharacter._distance = 0;
        UIManager.Instance.PauseButtonUI();
    }
}

