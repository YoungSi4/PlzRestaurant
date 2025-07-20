using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene2 : MonoBehaviour
{
    public GameObject stopPopup;


    public TextMeshProUGUI[] TimeText; //시간 : 분을 나타낼 텍스트
    private int StartTime = 480;
    public float LimitTime = 600; // default: 600 [sec]
    private float time = 0f;
    private bool isRunning = false;

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
}
