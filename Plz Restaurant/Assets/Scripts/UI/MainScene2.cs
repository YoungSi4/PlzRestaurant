using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene2 : MonoBehaviour
{
    [Header("오브젝트들")]
    public GameObject stopPopup;
    public GameObject fadeOutPanel;
    public GameObject orderMemo;
    public GameObject MenuPan;
    

    [Header("GameManager와 연결될 텍스트")]
    public TextMeshProUGUI targetIncomeText;
    public TextMeshProUGUI todayIncomeText;
    public TextMeshProUGUI giveUpTodayText;

    [Header("애니메이션")]
    public Animator left; // 
    public Animator right;//
    public Animator up;//



    public MainScene1 mainScene1_UIManager;
    public GameObject mainScene1_UI;


    //public Animator inventory;

    //public GameObject timeOver;


    public void OnOff_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }
    public void On_UI_Fadeout(GameObject uiObject) // ui활성화, 화면 어두워짐
    {
        if (uiObject == null) return;
        uiObject.SetActive(true);
        fadeOutPanel.SetActive(true);
    }
    public void Off_UI_Fadein(GameObject uiObject) // ui비활성화, 화면 밝아짐
    {
        if (uiObject == null) return;
        uiObject.SetActive(false);
        fadeOutPanel.SetActive(false);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape)) { //ESC를 눌렀을 때 팝업이 꺼지게 
        //    Off_UI(stopPopup);
        //     }
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
    public void Stop(){
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

}
