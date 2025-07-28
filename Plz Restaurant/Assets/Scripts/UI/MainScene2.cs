using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene2 : MonoBehaviour
{
    public GameObject stopPopup;
    public TextMeshProUGUI targetIncomeText;
    public TextMeshProUGUI todayIncomeText;

    public Animator left;
    public Animator right;
    public Animator up;

    public GameObject timeOver;

    public void On_UI(GameObject uiObect)
    {
        if (uiObect == null) return;
        uiObect.SetActive(true);
    }
    public void Off_UI(GameObject uiObect)
    {
        if (uiObect == null) return;
        uiObect.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Off_UI(stopPopup);
             }
        todayIncomeText.SetText(GameManager.Instance.R_dailyIncome.ToString());
        targetIncomeText.SetText(GameManager.Instance.R_targetIncome.ToString());
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
        Debug.Log("Ω√¿€");
    }

    public void StopGame()
    {
        GameManager.Instance.StopGame();
    }
    public void ReStartGame()
    {
        GameManager.Instance.ReStartGame();
    }

}
