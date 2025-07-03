using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TimeControl timeControl;
    public VisitorSpawner visitorSpawner;

    public Button startButton;

    public Button moneyButton; // �׽�Ʈ �뵵;

    public VisitorPool pool; // ?

    // �����Ȳ ���� ����
    private int R_day = 1;
    private int R_targetIncome = 5000;
    private int R_targetIncomeIncrease = 5000;
    private int R_season = 0; // spring 0, summer 1, autumn 2, winter 3
    private int R_chapter = 1;
    private int R_totalIncome = 0; // �÷��̾��� ��ȭ��
    private int R_dailyIncome = 0;
    private bool R_isOpen = false;

    // VisitorSpawner�� ������Ű�� �÷��� �ʿ�
    // public �Լ� �ϳ� ���� GameManager���� ����

    [SerializeField]
    private TextMeshProUGUI R_targetIncome_Tmp;
    private TextMeshProUGUI R_dailyIncome_Tmp;

    public override void Awake()
    {
        base.Awake();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else Debug.Log("StartButton is not assigned in the Inspector.");

        moneyButton.onClick.AddListener(ShowMeTheMoney);
    }
    public void StartGame()
    {
        // ���� ���� �� pool == null �� ���°� �߻��Ͽ� Spawn���� X
        visitorSpawner.Start_Spawning();
        timeControl.Start_Timer();

        R_isOpen = true;
    }

    private void ShowMeTheMoney()
    {
        R_dailyIncome += 1000;
    }

    // �Լ� ���� ���� : �ð� // ���� ���� �Ѿ�� ��� �����ϴ� �� ���� ��?
    public void R_close()
    {

    }

    public void R_checkSuccess()
    {
        // ���� ���ο� ���� �ٸ� UI ǥ��
        // �÷��̾�� ������ Ȯ�� ��ư�� �������� �Լ��� �ٸ��� ����
        if (R_dailyIncome >= R_targetIncome)
        {
            // �� �þ�� �� �ִϸ��̼��� �־ ���� ��?
            R_totalIncome += R_dailyIncome;
            // nextDay()
        }
        else
        {
            R_totalIncome += R_dailyIncome / 3;
            // repeatDay()
        }

        R_reset();
    }

    // �ʿ��� ������ ��� �ʱ�ȭ ���ִ� �Լ�
    private void R_reset()
    {
        R_dailyIncome = 0;
        R_isOpen = false;
        // Ÿ�̸� �ʱ�ȭ
        // VisitorSpawner �ʱ�ȭ
    }

    private void R_nextDay()
    {
        R_day++;
        R_targetIncome += R_targetIncomeIncrease;
    }

    private void R_repeatDay()
    {
        // blank
    }
}
