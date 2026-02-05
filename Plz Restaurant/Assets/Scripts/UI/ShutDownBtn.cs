using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShutDownBtn : MonoBehaviour
{
    Button shutdown;

    private void Start()
    {
        shutdown = GameObject.FindWithTag("ShutDown").GetComponent<Button>();
        shutdown.onClick.AddListener(ExitGame);
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
