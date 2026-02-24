using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : Singleton<TimeControl>
{
    // 임시로 GameManager 받아서 사용
    [SerializeField]
    private GameManager gameManager;

    public TextMeshProUGUI[] TimeText;
    // public GameObject TimeOutImage;
    //public Button StartButton; @@@@@@@ start버튼은 없앰 mainscene1에서 영업시작을 하기 때문에

    private int StartTime = 480;
    public float LimitTime = 600; // default: 600 [sec]
    private float time = 0f;

    private bool isRunning = false;

    public override void Awake()
    {
        base.Awake();
    }

    void Update()
    {

        if (!isRunning) return;


        if(time < LimitTime)
        {
            time += Time.deltaTime;

            int totalSeconds = Mathf.FloorToInt(time);
            int displayedTotalMinutes = StartTime + totalSeconds;
            int hour = displayedTotalMinutes / 60;
            int min = displayedTotalMinutes % 60;

            TimeText[0].text = hour.ToString("D2");
            TimeText[1].text = min.ToString("D2");
        }
        else
        {
            //TimeOutImage.SetActive(true);
            isRunning = false;
            gameManager.R_close();
        }

    }

    public void Start_Timer()
    {
        isRunning = true;
        time = 0f;
        //TimeOutImage.SetActive(false);
        TimeText[0].text = (StartTime / 60).ToString("D2");
        TimeText[1].text = (StartTime % 60).ToString("D2");
    }
}
