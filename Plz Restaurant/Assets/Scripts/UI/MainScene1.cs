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
    //public Button[] questGetButtons; // 수령 버튼 
    public GameObject getPopup; // 수령 버튼에 마우스 올리면 나올 팝업
    //public Transform getButtonTransform; // 수령 버튼 위에 팝업을 띄울거라 위치값을 가져옴


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
    Color getColor = new Color(1f, 1f, 0.6f); // 연한 노란색

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
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public void PopupOn()
    {
        getPopup.SetActive(true);
    }
    public void PopupOff()
    {
        getPopup.SetActive(false);
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@

    public Canvas rootCanvas; //최상위 canvas
    public RectTransform tooltipPanel; // 공용 팝업(비활성 시작)
    public TMP_Text tooltipLabel; // 팝업 텍스트
    public Vector2 offset = new Vector2(0, 24f); // 버튼 위로 살짝

    public void ShowOver(Transform target, string text)
    {
        if (!tooltipPanel || !rootCanvas) return;
        if (tooltipLabel) tooltipLabel.text = text ?? "";

        var targetRT = (RectTransform)target; //버튼의 transform을 RectTrasnform으로 캐스팅
        var canvasRT = (RectTransform)rootCanvas.transform; //최상위 캔버스의 trasnform을 recttrasnform으로 바꿈
        var cam = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera;
        //좌표 변환에 사용할 카메라를 선택

        Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, targetRT.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, screen, cam, out var local);
        tooltipPanel.anchoredPosition = local + offset;
        tooltipPanel.gameObject.SetActive(true);
    }
    public void Hide() => tooltipPanel?.gameObject.SetActive(false);
    //tooltipPanel이 null이 아니면 setactive(false)를 실행
}
