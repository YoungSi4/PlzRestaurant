using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene2 : MonoBehaviour
{
    public GameObject stopPopup;
    public GameObject fadeOutPanel;

    public TextMeshProUGUI targetIncomeText;
    public TextMeshProUGUI todayIncomeText;
    public TextMeshProUGUI giveUpTodayText;


    public Animator left;
    public Animator right;
    public Animator up;
    public Animator inventory;

    public GameObject timeOver;

    public void On_UI(GameObject uiObect)
    {
        if (uiObect == null) return;
        uiObect.SetActive(true);
        fadeOutPanel.SetActive(true);
        
    }
    public void Off_UI(GameObject uiObect)
    {
        if (uiObect == null) return;
        uiObect.SetActive(false);
        fadeOutPanel.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Off_UI(stopPopup);
             }
        todayIncomeText.SetText(GameManager.Instance.R_dailyIncome.ToString());
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

    public void EndGame()
    {
        timeOver.SetActive(true);
        GameManager.Instance.R_close();
    }

    public IEnumerator MainScene2Start()
    {
        left.SetBool("In",true);
        right.SetBool("In",true);
        up.SetBool("In",true);
        yield return new WaitForSeconds(1);
        GameManager.Instance.StartGame();
        Debug.Log("시작");
    }

    public void StopGame()
    {
        GameManager.Instance.StopGame();
    }
    public void ReStartGame()
    {
        GameManager.Instance.ReStartGame();
    }
    public void Stop(){
        //영업 중단..
    }
    public void InventoryOn(){
        bool currentInventory = inventory.GetBool("On");
        inventory.SetBool("On", !currentInventory);
    }
    
}
