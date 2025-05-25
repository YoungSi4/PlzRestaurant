using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAndToggle : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineVirtualCamera topDownCam;
    private bool isFirstPerson = true; //현재 시점이 1인칭인지 아닌지 
    public float moveSpeed = 2f;  
    private Rigidbody rb;

    public Vector3 direction { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToView();
        }
        
        Move();
    }
    void SwitchToView()
    {
        isFirstPerson = !isFirstPerson;
        firstPersonCam.Priority = isFirstPerson ? 1 : 0;
        topDownCam.Priority = isFirstPerson ? 0 : 1;
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y);
    }

    public void Move()
    {
        rb.velocity = direction * moveSpeed + Vector3.up * rb.velocity.y; 
    }
}
