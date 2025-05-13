using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndToggle : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineVirtualCamera topDownCam;
    private bool isFirstPerson = true;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToView();
        }
    }
    void SwitchToView()
    {
        isFirstPerson = !isFirstPerson;
        firstPersonCam.Priority = isFirstPerson ? 1 : 0;
        topDownCam.Priority = isFirstPerson ? 0 : 1;
    }
}
