using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    public TextMeshProUGUI[] TimeText;
    public GameObject TimeOutImage;

    private float time = 0f;
    private int StartTime = 480;
    public float LimitTime = 600; // sec
    private bool isTimeOver = false;
    
    void Start()
    {
        
    }

    void Update()
    {
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
            isTimeOver = true;
            Handle_Timeover();
        }

    }
    
    void Handle_Timeover()
    {
        TimeOutImage.SetActive(true);
        Invoke("Restart_Timer", 2f);
    }

    void Restart_Timer()
    {
        TimeOutImage.SetActive(false);
        isTimeOver = false;
        time = 0f;
        TimeText[0].text = (StartTime / 60).ToString("D2");
        TimeText[1].text = (StartTime % 60).ToString("D2");
    }
}
