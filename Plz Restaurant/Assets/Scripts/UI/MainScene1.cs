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

    private void Start()
    {
        TodayInit();//@@@@@@@@@@ 씬이 시작하면 현재 플레이어의 자산과 현재 영업일을 업데이트해줌
    }

    private void TodayInit()
    {
        Asset();
        currentDate.SetText("Day" + GameManager.Instance.R_day.ToString()); //@@@@@@@@@ 게임매니저에서 가져옴
    }

    /// //////////////////////////////////////////////////
    public void Asset()
    {
        currentAsset.SetText("asset" + FormatNumber(GameManager.Instance.R_totalIncome)); //@@@@@@@  게임매니저에서 가져옴
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
        Debug.Log(uiObject.transform.root.name);
        Debug.Log(uiObject.transform.parent.name);
        Debug.Log(uiObject.transform.name);

        uiObject.transform.root.SetAsLastSibling();
        //Transform parent = uiObject.transform.parent;
        //parent.SetAsLastSibling();        // 부모 그룹이 위로
        //uiObject.transform.SetAsLastSibling(); // 해당 오브젝트가 그 그룹 내 가장 위

    }
    public void Off_UI(GameObject uiObject)
    {
        if (uiObject == null) return;
        uiObject.SetActive(false);
    }
}
