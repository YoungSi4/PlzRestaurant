using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndToggle : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineVirtualCamera topDownCam;
    private bool isFirstPerson = true; //현재 시점이 1인칭인지 아닌지 
    public float moveSpeed = 2f;  
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = (transform.right*x + transform.forward*z).normalized;
        rb.velocity = moveDir * moveSpeed; //AddForce는 힘이 누적됨

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
