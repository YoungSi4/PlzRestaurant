using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainScene1 : MonoBehaviour
{
    //public Button dropDownMenu;
    public GameObject menu;
    public GameObject storeMenu;
    public GameObject ExitMenu;
    public TextMeshProUGUI asset;
    public int a = 10000000;

    public void DropDownMenu()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else menu.SetActive(true);
    }

    private void Update()
    {
        
    }

    /// //////////////////////////////////////////////////
    public void Asset()
    {
        asset.text = "asset" + "FormatNumber(a)";
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
        Debug.Log("시작");
    }
}
