using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class MainScene1 : MonoBehaviour
{
    [Header("On/Off UI")]
    public GameObject dropDownMenu;
    public GameObject storeMenu;
    public GameObject exitMenu;
    public GameObject startMenu;

    [Header("Update Text")]
    [SerializeField] TextMeshProUGUI currentAsset;
    [SerializeField] TextMeshProUGUI currentDate;

    [Header("Start Animation")]
    public GameObject uiLeft;
    public GameObject uiRight;
    Animator uiLeftGo;
    Animator uiRightGo;

    private void Start()
    {
        uiLeftGo = uiLeft.GetComponent<Animator>();
        uiRightGo = uiRight.GetComponent<Animator>();
        TodayInit();//@@@@@@@@@@ 씬이 시작하면 현재 플레이어의 자산과 현재 영업일을 업데이트해줌
    }

    //public void DropDownMenu()
    //{
    //    if (dropDownMenu.activeSelf)
    //    {
    //        dropDownMenu.SetActive(false);
    //    }
    //    else dropDownMenu.SetActive(true);
    //}

    private void Update()
    {
        
    }

    private void TodayInit()
    {
        Asset();
        currentDate.text = "Day" + GameManager.Instance.R_day.ToString(); //@@@@@@@@@ 게임매니저에서 가져옴
    }

    /// //////////////////////////////////////////////////
    public void Asset()
    {
        currentAsset.text = "asset" + FormatNumber(GameManager.Instance.R_totalIncome); //@@@@@@@  게임매니저에서 가져옴
    }
    string FormatNumber(int num)
    {
        if (num >= 1_000_000_000)
        {
            return (num / 1_000_000_000f).ToString("0.##") + "B"; //정수 부분은 무조건 출력 소수점은 2자리만 출력
        }
        else return num.ToString();
    }
    //////////////////////////////////////////////////////
    
    //public void OnStore()
    //{
    //    if (storeMenu.activeSelf)
    //    {
    //        storeMenu.SetActive(false);
    //    }
    //    else storeMenu.SetActive(true);
    //}

    //public void OnExitMenu()
    //{
    //    if (exitMenu.activeSelf)
    //    {
    //        exitMenu.SetActive(false);
    //    }
    //    else exitMenu.SetActive(true);
    //}

    public void GameStart()
    {
        startMenu.SetActive(false);
        uiRightGo.SetBool("Go", true);
        uiLeftGo.SetBool("Go", true);
        Debug.Log("시작");
    }
    public void OnOff_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }
}
