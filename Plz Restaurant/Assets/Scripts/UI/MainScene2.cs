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
    //public Animator inventory;

    //public GameObject timeOver;


    public void OnOff_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }

    public void On_UI(GameObject uiObect) // ui활성화, 화면 어두워짐
    {
        if (uiObect == null) return;
        uiObect.SetActive(true);
        fadeOutPanel.SetActive(true);
        
    }

    public void Off_UI(GameObject uiObect) // ui비활성화, 화면 밝아짐
    {
        if (uiObect == null) return;
        uiObect.SetActive(false);
        fadeOutPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { //ESC를 눌렀을 때 팝업이 꺼지게 
            Off_UI(stopPopup);
             }
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

    public IEnumerator MainScene2Start() //메인씬1에서 메인씬2로 넘어갈 때 
    {
        left.SetBool("In",true); //ui들 생기고 
        right.SetBool("In",true);
        up.SetBool("In",true);
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
        //영업 중단..
    }
    public void InventoryOn(){
        //bool currentInventory = inventory.GetBool("On");
        //inventory.SetBool("On", !currentInventory);
    }
    public void OrderMemoOn() //다른 함수에서 불러줘야 하기 때문에 따로 만들었다.
    {
        orderMemo.SetActive(true);
    }

    public void MenuPanOnOff(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }

}
