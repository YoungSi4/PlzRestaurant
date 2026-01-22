using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainScene1 : MonoBehaviour
{
    [Header("Quest get popup")]
   
    public GameObject getPopup; // 수령 버튼에 마우스 올리면 나올 팝업

    public GameObject fadeOutPanel; //팝업 창 띄우면 화면 밝기 어둡게 할 페이드아웃 패널 


    [Header("Update Text")]
    [SerializeField] TextMeshProUGUI currentAsset;
    [SerializeField] TextMeshProUGUI currentDate;

    [Header("Start Animation")]
    public Animator uiLeft;
    public Animator uiRight;
    public Animator uiDown;


    public GameObject mainScene1_UI; // mainScene1_UI자체를 말함
    public MainScene2 mainScene2;    // mainScene2 manager를 말함 (함수 불러오는거라서)
    public GameObject mainScene2_UI; // mainScene2_UI자체를 말함

    Color activeColor = new Color(0.4f, 0.7f, 1f); //파란색
    Color inactiveColor = new Color(0.7f, 0.7f, 0.7f); //회색
    Color getColor = new Color(1f, 1f, 0.6f); // 연한 노란색

    private void Start()
    {
        TodayInit();//씬이 시작하면 현재 플레이어의 자산과 현재 영업일을 업데이트해줌
    }

    private void TodayInit()
    {
        Asset();
        currentDate.SetText("Day " + GameManager.Instance.R_day.ToString()); // 게임매니저에서 가져옴
    }

    /// //////////////////////////////////////////////////
    public void Asset()
    {
        currentAsset.SetText("asset " + FormatNumber(GameManager.Instance.R_totalIncome)); // 게임매니저에서 가져옴
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
        uiRight.SetBool("Out", true);
        uiLeft.SetBool("Out", true);
        uiDown.SetBool("Out", true);
        StartCoroutine(IGameStart());
    }

    IEnumerator IGameStart()
    {
        yield return new WaitForSeconds(1);
        mainScene2_UI.SetActive(true);
        StartCoroutine(mainScene2.MainScene2Start());
        mainScene1_UI.SetActive(false);
    }

    public void OnOff_UI(GameObject uiObject) //오브젝트를 on off 토글로 할 수 있는데 얘는 페이드아웃이 되지않음
    {
        if (uiObject == null) return;
        uiObject.SetActive(!uiObject.activeSelf);
    }

    public void On_UI_Fadeout(GameObject uiObject) //오브젝트를 On만 할 수있는데 페이드아웃이 됨
    {
        if (uiObject == null) return;
        uiObject.SetActive(true);
        fadeOutPanel.SetActive(true);
    }
    public void Off_UI_FadeIn(GameObject uiObject) //오브젝트를 Off만 할 수있는데 페이드아웃이 풀림
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
    public void PopupOn()
    {
        getPopup.SetActive(true);
    }
    public void PopupOff()
    {
        getPopup.SetActive(false);
    }
}
