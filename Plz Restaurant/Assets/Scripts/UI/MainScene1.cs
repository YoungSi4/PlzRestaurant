using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainScene1 : MonoBehaviour
{
    [Header("On/Off UI")]
    public GameObject dropDownMenu;
    public GameObject storeMenu;
    public GameObject exitMenu;
    public GameObject fadeOutPanel;
    //public GameObject startMenu;

    [Header("Update Text")]
    [SerializeField] TextMeshProUGUI currentAsset;
    [SerializeField] TextMeshProUGUI currentDate;

    [Header("Start Animation")]
    public Animator uiLeft;
    public Animator uiRight;
    public Animator uiDown;

    public MainScene2 mainScene2;
    public GameObject mainScene_2;

    Color activeColor = new Color(0.4f, 0.7f, 1f); //파란색
    Color inactiveColor = new Color(0.7f, 0.7f, 0.7f); //회색

    private void Start()
    {
        TodayInit();//@@@@@@@@@@ 씬이 시작하면 현재 플레이어의 자산과 현재 영업일을 업데이트해줌
    }

    private void TodayInit()
    {
        Asset();
        currentDate.SetText("Day " + GameManager.Instance.R_day.ToString()); //@@@@@@@@@ 게임매니저에서 가져옴
    }

    /// //////////////////////////////////////////////////
    public void Asset()
    {
        currentAsset.SetText("asset " + FormatNumber(GameManager.Instance.R_totalIncome)); //@@@@@@@  게임매니저에서 가져옴
    }
    string FormatNumber(int num)
    {
        if (num >= 1_000_000)
        {
            return (num/1_000_000).ToString("0.##") + "M";
        }
        else if (num >= 1_000_000_000)
        {
            return (num / 1_000_000_000f).ToString("0.##") + "B"; //정수 부분은 무조건 출력 소수점은 2자리만 출력
        }
        else return num.ToString();
    }

    public void GameStart()
    {
        //startMenu.SetActive(false);
        uiRight.SetBool("Out", true);
        uiLeft.SetBool("Out", true);
        uiDown.SetBool("Out", true);
        StartCoroutine(IGameStart());
    }

    IEnumerator IGameStart()
    {
        yield return new WaitForSeconds(1);
        mainScene_2.SetActive(true);
        StartCoroutine(mainScene2.MainScene2Start());
    }

    public void OnOff_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }

    public void On_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(true);
        fadeOutPanel.SetActive(true);
    }
    public void Off_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(false);
        fadeOutPanel.SetActive(false);
    }

    public void BtnOn(GameObject uiObject)
    {
        Image img = uiObject.GetComponent<Image>(); 
        if(img != null)
        {
            img.color = activeColor;
        }
    }
    public void BtnOff(GameObject uiObject)
    {
        Image img = uiObject.GetComponent<Image>();
        if (img != null)
        {
            img.color = inactiveColor;
        }
    }
}
