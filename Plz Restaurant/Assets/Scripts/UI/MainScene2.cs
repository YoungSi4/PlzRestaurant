using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene2 : MonoBehaviour
{
    [Header("오브젝트들")]
    public GameObject fadeOutPanel;
    public GameObject orderMemo;
    public GameObject MenuPan;
    

    [Header("GameManager와 연결될 텍스트")]
    public TextMeshProUGUI targetIncomeText;
    public TextMeshProUGUI todayIncomeText;
    public TextMeshProUGUI giveUpTodayText;

    [Header("애니메이션")]
    public Animator left; 
    public Animator right;
    public Animator up;

    public MainScene1 mainScene1_UIManager; //메인씬1의 ui manager
    public GameObject mainScene1_UI; //메인씬1의 UI자체를 말함 
    //public GameManager gameManager; //게임이 끝나고 결과창에 보일 텍스트들을 위해 가져온다.



    [Header("결과창에 필요한 텍스트들")]
    public TextMeshProUGUI successFailureText;
    public TextMeshProUGUI todayDateText;
    //public TextMeshProUGUI targetIncomeText; 이 두가지는 위에 이미 선언했다.
    //public TextMeshProUGUI todayIncomeText;
    public TextMeshProUGUI handledAnomaliesText;
    public TextMeshProUGUI FailedAnomaliesText;

    [Header("결과창")]
    public GameObject resultWindow;







    [Header("UI인풋맵 관련")]
    public PlayerInput playerInput; // 플레이어 인풋
    public enum inputState  //플레이어인풋 종류 (이름만 저장할 수 있으면 되니까)
    {
        FirstPerspective,
        ThreePerspective,
        UI,
        Nothing
    }
    public string previousPlayerState; //이전 상태의 인풋맵을 기억한다. string으로 이름만 기억하면 된다.

    GameObject uiPopUp; // esc로 꺼야할 "Popup"이라는 태그를 가진 게임 오브젝트를 말한다.
    



    public void OnOff_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }
    public void On_UI_Fadeout(GameObject uiObject) // ui활성화, 화면 어두워짐
    {
        if (uiObject.CompareTag("Popup"))
        {
            uiPopUp = uiObject;
            PopUpOn2();
        }
        if (uiObject == null) return;
        uiObject.SetActive(true);
        fadeOutPanel.SetActive(true);
    }
    public void On_UI(GameObject uiObject) // ui활성화만 실행 => 메뉴판 esc로 닫을 때를 위해서 만듦 => 얘는 다른 팝업창 처럼 화면이 페이드아웃 들어가면 안되기 떄문에
    {
        if (uiObject.CompareTag("Popup"))
        {
            uiPopUp = uiObject;
            PopUpOn2();
        }
        if (uiObject == null) return;
        uiObject.SetActive(true);
    }

    public void Off_UI_Fadein(GameObject uiObject) // ui비활성화, 화면 밝아짐
    {
        if (uiObject.CompareTag("Popup"))
        {
            uiPopUp = null;
            playerInput.SwitchCurrentActionMap(previousPlayerState);
        }
        if (uiObject == null) return;
        uiObject.SetActive(false);
        fadeOutPanel.SetActive(false);
    }

    private void Update()
    {
        todayIncomeText.SetText(GameManager.Instance.R_dailyIncome.ToString()); //항상 업데이트 되는 텍스트
        targetIncomeText.SetText(GameManager.Instance.R_targetIncome.ToString());
        giveUpTodayText.SetText("Day "+GameManager.Instance.R_day.ToString());
    }

    public void ShowMeTheMoney() 
    {
        GameManager.Instance.ShowMeTheMoney();
    }

    public void EraseMoney()
    {
        GameManager.Instance.EraseMoney();
    }

    public void EndGame() //끝났을 때 
    {
        //timeOver.SetActive(true);
        GameManager.Instance.R_close();

    }

    public IEnumerator IMainScene2Start() //메인씬1에서 메인씬2로 넘어갈 때 
    {
        left.SetTrigger("In");
        right.SetTrigger("In");
        up.SetTrigger("In");
        yield return new WaitForSeconds(1); // 1초 있다가 게임 시작
        GameManager.Instance.StartGame();
        Debug.Log("시작");
    }

    public void StopGame() //게임 정지 
    {
        GameManager.Instance.StopGame();
    }
    public void ReStartGame() //게임 다시 시작 
    {
        GameManager.Instance.ReStartGame();
    }
    public void Stop(){ //영업이 끝날 때 
        left.SetTrigger("Out");
        right.SetTrigger("Out");
        up.SetTrigger("Out");
        mainScene1_UI.SetActive(true);
        StartCoroutine(mainScene1_UIManager.IBackMainScene1UI());
        //영업 중단..
    }


    public void OrderMemoOn() //다른 함수에서 불러줘야 하기 때문에 따로 만들었다.
    {
        orderMemo.SetActive(true);
    }

    public void PopUpOn2() // ESC키를 눌러 꺼야하는 팝업이 켜지면 
    {
        previousPlayerState = (playerInput.currentActionMap.name);
        playerInput.SwitchCurrentActionMap("UI2");
    }

    public void Esc() 
    {
        if (uiPopUp != null)
        {
            uiPopUp.SetActive(false);
            uiPopUp = null;
        }
        playerInput.SwitchCurrentActionMap(previousPlayerState);
        Debug.Log("Esc");
        fadeOutPanel.SetActive(false);
    }

    public void ResultWindowOn()
    {
        resultWindow.SetActive(true);
        successFailureText.SetText(GameManager.Instance.R_Success_Fail ? "Success" : "Failure");
        todayIncomeText.SetText(GameManager.Instance.R_dailyIncome.ToString()); //항상 업데이트 되는 텍스트
        targetIncomeText.SetText(GameManager.Instance.R_targetIncome.ToString());
        todayDateText.SetText("Day " + GameManager.Instance.R_day.ToString());
        //handledAnomaliesText.SetText("Day " + GameManager.Instance..ToString()); //기현상의 개수는 아직 정해진게 없는거 같아서 이렇게 둠
        //FailedAnomaliesText.SetText("Day " + GameManager.Instance..ToString());
    }

}
