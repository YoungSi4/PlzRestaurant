using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TimeControl timeControl;
    public VisitorSpawner visitorSpawner;
    //public MainScene2 mainScene2;

    // 진행상황 관련 변수 -> 25.12.22 YH : get만 public으로 열고 set은 닫아도 괜찮지 않아?
    public int R_day = 1;          //@@@@@@@@@ UI에서 나타내기위해 private->public 변경
    public int R_targetIncome = 5000; // @@@@@@@@@@@@
    private int R_targetIncomeIncrease = 5000; 
    private int R_season = 0; // spring 0, summer 1, autumn 2, winter 3
    private int R_chapter = 1;
    public int R_totalIncome = 0; // 플레이어의 재화량                                //@@@@@@@@@ UI에서 나타내기위해 private->public 변경
    public int R_dailyIncome = 0;
    private bool R_isOpen = false;

    // VisitorSpawner에 정지시키는 플래그 필요
    // public 함수 하나 만들어서 GameManager에서 제어

    //public Button startButton; @@@@@@@@@@ 이거필요 없는게 mainscene1에서 start누르면 바로 시작이라
    //public Button endButton;

    //public Button moneyButton; // 테스트 용도;
    //public Button moneyLoseButton;

    //[SerializeField]
    //private TextMeshProUGUI R_targetIncome_Tmp;
    //[SerializeField]
    //private TextMeshProUGUI R_dailyIncome_Tmp;
    //[SerializeField]
    //private TextMeshProUGUI R_day_Tmp;

    public bool R_Success_Fail;
    public MainScene2 mainScene2UiManager; //메인씬2ui매니저의 결과창 보여주는 함수를 이 스크립트의 rclose함수안에서 실행함.



    public override void Awake()
    {
        base.Awake();

        // UI 좌측상단 수익 관련 텍스트 초기화
        //R_targetIncome_Tmp.SetText(R_targetIncome.ToString());
        //R_dailyIncome_Tmp.SetText(R_dailyIncome.ToString());
        //R_day_Tmp.SetText(R_day.ToString());

        //// UI 테스트용 버튼 연결 
        //if (startButton != null)
        //{
        //    startButton.onClick.AddListener(StartGame);
        //}
        //else Debug.Log("StartButton is not assigned in the Inspector.");

        //endButton.onClick.AddListener(R_close);      @@@@@@@@@@
        //moneyButton.onClick.AddListener(ShowMeTheMoney);@@@@@@@@@@
        //moneyLoseButton.onClick.AddListener(EraseMoney);@@@@@@@@@@@@
    }
    public void StartGame() // 나중에 R_Open로 바꿀 것
    {
        visitorSpawner.Start_Spawning();
        timeControl.Start_Timer();
        R_isOpen = true;
    }

    /* time control을 game manager가 관찰하다가 (옵저버 디자인 패턴?)
    *   시간이 지나면 해당 함수 실행?
    *   OR 애초에 time control에서 game manager의 함수를 실행?
    */
    public void R_close()
    {
        // 아직 둘 다 함수가 없음
        // visitorSpawner.stop
        // timeControl.stop
        visitorSpawner.StopSpawning();

        R_isOpen = false;
        R_checkSuccess();
        R_resetVars();
        mainScene2UiManager.ResultWindowOn();
        
    }


    // 버튼에 연결한 돈 증가, 감소 시키는 함수 - 테스트 용도
    public void ShowMeTheMoney() //@@@@@@@@@ public으로 바꿈
    {
        R_dailyIncome += 1000;
        string tempIncome = R_dailyIncome.ToString();
        //R_dailyIncome_Tmp.SetText(tempIncome);
    }

    public void EraseMoney() //@@@@@@@@@@@@@@@@@@ public으로 바꿈
    {
        R_dailyIncome -= 1000;
        //R_dailyIncome_Tmp.SetText(R_dailyIncome.ToString());
    }

    // 수익을 반영시키는 함수 (매개변수는 음수 양수 상관없음)
    public void AddDailyIncome(int income)
    {
        R_dailyIncome += income;
        //R_dailyIncome_Tmp.SetText(R_dailyIncome.ToString());
    }

    public void R_checkSuccess()
    {
        // 성공 여부에 따라 다른 UI 표시
        // 플레이어는 동일한 확인 버튼을 누르지만 함수는 다르게 동작
        if (R_dailyIncome >= R_targetIncome)
        {
            R_Success_Fail = true;
            // 아래 수익 처리하는 건 따로 함수로 빼고
            // 결과창 표시는 성공 실패 2개로 만들어서 처리해야 할 듯

            // 돈 늘어나는 건 애니메이션을 넣어도 좋을 듯?
            // 화면에 표시할 값을 담은 변수
            R_totalIncome += R_dailyIncome;
            // 표시할 값 변수 -> R_totalIncome까지 숫자 기분좋게 올라가는 애니메이션 실행

            Debug.Log("total money : " + R_totalIncome);
            Debug.Log("next day");
            R_nextDay();
        }
        else
        {
            R_Success_Fail = false;
            R_totalIncome += R_dailyIncome / 3;

            Debug.Log("total money : " + R_totalIncome);
            Debug.Log("target fail");

            R_repeatDay();
        }
    }

    // 필요한 변수를 모두 초기화 해주는 함수
    private void R_resetVars()
    {
        R_dailyIncome = 0;
        //R_dailyIncome_Tmp.SetText(R_dailyIncome.ToString());

        R_isOpen = false;
        // 타이머 초기화
        // VisitorSpawner 초기화
    }

    private void R_nextDay()
    {
        R_day++;
        //R_day_Tmp.SetText(R_day.ToString());
        R_targetIncome += R_targetIncomeIncrease;
        var tempTarget = R_targetIncome.ToString();
        //R_targetIncome_Tmp.SetText(tempTarget);
    }

    private void R_repeatDay()
    {
        // blank
        // 동일한 날짜를 다시 플레이
        // 그럼 어느 씬으로 이동? or 그냥 게임 실패 및 초기화?
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }
    public void ReStartGame()
    {
        Time.timeScale = 1;
    }
}
