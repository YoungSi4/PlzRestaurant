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

    public Button moneyButton; // 테스트 용도;

    public VisitorPool pool; // ?

    // 진행상황 관련 변수
    private int R_day = 1;
    private int R_targetIncome = 5000;
    private int R_targetIncomeIncrease = 5000;
    private int R_season = 0; // spring 0, summer 1, autumn 2, winter 3
    private int R_chapter = 1;
    private int R_totalIncome = 0; // 플레이어의 재화량
    private int R_dailyIncome = 0;
    private bool R_isOpen = false;

    // VisitorSpawner에 정지시키는 플래그 필요
    // public 함수 하나 만들어서 GameManager에서 제어

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
        // 순서 변경 시 pool == null 인 상태가 발생하여 Spawn동작 X
        visitorSpawner.Start_Spawning();
        timeControl.Start_Timer();

        R_isOpen = true;
    }

    private void ShowMeTheMoney()
    {
        R_dailyIncome += 1000;
    }

    // 함수 실행 조건 : 시간 // 일일 수익 넘어서도 계속 영업하는 게 좋을 듯?
    public void R_close()
    {

    }

    public void R_checkSuccess()
    {
        // 성공 여부에 따라 다른 UI 표시
        // 플레이어는 동일한 확인 버튼을 누르지만 함수는 다르게 동작
        if (R_dailyIncome >= R_targetIncome)
        {
            // 돈 늘어나는 건 애니메이션을 넣어도 좋을 듯?
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

    // 필요한 변수를 모두 초기화 해주는 함수
    private void R_reset()
    {
        R_dailyIncome = 0;
        R_isOpen = false;
        // 타이머 초기화
        // VisitorSpawner 초기화
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
