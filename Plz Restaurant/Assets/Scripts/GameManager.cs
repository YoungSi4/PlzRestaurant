using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TimeControl timeControl;
    public VisitorSpawner visitorSpawner;

    public Button startButton;

    public VisitorPool pool;


    public override void Awake()
    {
        base.Awake();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else Debug.Log("StartButton is not assigned in the Inspector.");
    }
    public void StartGame()
    {
        // 순서 변경 시 pool == null 인 상태가 발생하여 Spawn동작 X
        visitorSpawner.Start_Spawning();
        timeControl.Start_Timer();
    }
}
