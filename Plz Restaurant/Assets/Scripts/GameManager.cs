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
        // ���� ���� �� pool == null �� ���°� �߻��Ͽ� Spawn���� X
        visitorSpawner.Start_Spawning();
        timeControl.Start_Timer();
    }
}
