using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShutDownBtn : MonoBehaviour
{
    Button shutdown;

    private void Start()
    {
        shutdown = GameObject.FindGameObjectWithTag("ShutDown").GetComponent<Button>();
        shutdown.onClick.AddListener(ExitGame);
    }

    void ExitGame()
    {
        Debug.Log("종료버튼");
        Application.Quit();
    }
}
