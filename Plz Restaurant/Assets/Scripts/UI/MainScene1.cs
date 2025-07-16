using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class MainScene1 : MonoBehaviour
{
    //public Button dropDownMenu;
    public GameObject dropDownMenu;
    public GameObject storeMenu;
    public GameObject ExitMenu;
    public TextMeshProUGUI assetMenuText;
    public GameObject StartMenu;
    public int a = 10000000;

    public GameObject uiLeft;
    public GameObject uiRight;

    Animator uiLeftGo;
    Animator uiRightGo;

    private void Start()
    {
        uiLeftGo = uiLeft.GetComponent<Animator>();
        uiRightGo = uiRight.GetComponent<Animator>();
    }

    public void DropDownMenu()
    {
        if (dropDownMenu.activeSelf)
        {
            dropDownMenu.SetActive(false);
        }
        else dropDownMenu.SetActive(true);
    }

    private void Update()
    {
        
    }

    /// //////////////////////////////////////////////////
    public void Asset()
    {
        assetMenuText.text = "asset" + FormatNumber(a);
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
    
    public void OnStore()
    {
        if (storeMenu.activeSelf)
        {
            storeMenu.SetActive(false);
        }
        else storeMenu.SetActive(true);
    }

    public void OnExitMenu()
    {
        if (ExitMenu.activeSelf)
        {
            ExitMenu.SetActive(false);
        }
        else ExitMenu.SetActive(true);
    }
    public void GameStart()
    {
        StartMenu.SetActive(false);
        uiRightGo.SetBool("Go", true);
        uiLeftGo.SetBool("Go", true);
        Debug.Log("시작");
    }
}
