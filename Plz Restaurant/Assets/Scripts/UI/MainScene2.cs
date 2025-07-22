using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene2 : MonoBehaviour
{
    public GameObject stopPopup;
    public TextMeshProUGUI targetIncomeText;
    public TextMeshProUGUI todayIncomeText;

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

    }
}
