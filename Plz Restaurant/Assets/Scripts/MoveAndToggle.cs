using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAndToggle : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam; //1��Ī ������ �� ī�޶�
    public CinemachineVirtualCamera topDownCam; // 3��Ī ������ �� ī�޶�


    private bool isFirstPerson = true; //���� ������ 1��Ī���� �ƴ��� 
    public float moveSpeed = 2f;  //�÷��̾� �̵��ӵ�. �Ƹ� ���߿� �÷��̾� ��ũ��Ʈ�� �־���ҵ�
    
    Rigidbody rb; //������ٵ�

    public Transform playerBody; //�¿� ȸ���� (���� ��ü)
    public Transform cameraHolder; // ���� ȸ���� (ī�޶� �θ�)

    float mouseSensitivity = 10f; //���콺 ������
    public float xRotation = 0f; // ī�޶� ���� ȸ�� ���� ���� ����
    public float mouseY;


    public Transform cameraTransform;

    Vector2 moveInput; //��ǲ�ý��ۿ��� Ű����� ���� ����(x,y������ Ű���� ���� �ޱ� ������ vector2)
    Vector3 camForward; // ī�޶� �յڸ� �ٷ� ���� 
    Vector3 camRight; // ī�޶� ���� �������� �ٷ� ���� 

    public Vector3 direction { get; private set; } //�÷��̾��� �̵������� ���� ����

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Cursor.lockState = CursorLockMode.Locked; //���콺�� ȭ�� �߾ӿ� ����
       // Cursor.visible = false; //���콺 Ŀ���� �� ���̰� ��
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //T������ ���� ��ȯ, ���߿� ��ǲ�ý������� ���ľ���
        {
            SwitchToView();
        }
        Move();
    }
    void SwitchToView()
    {
        isFirstPerson = !isFirstPerson; //����ġ ���
        firstPersonCam.Priority = isFirstPerson ? 1 : 0; //1��Ī�̸� 1��Īī�޶��� �켱������ 1�� ������ 1��Ī ī�޶��� ��ġ�� ���̰� ��
        topDownCam.Priority = isFirstPerson ? 0 : 1; //�̰� �ݴ�
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); //��ǲ�ý������� �̵� �� ����
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>(); //2D ����(x:�¿�,y:����)�� �޾ƿ�.

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime; 
        mouseY = mouseInput.y* mouseSensitivity * Time.deltaTime;
        //Time.deltaTime���� �ϰ��� �ְ� �޾��ְ�

        xRotation -= mouseY;
        //XRotation(ī�޶� ����)�� mouseY������ �����Ѵ�. (X�� ȸ���̱� ������ y���� �ٲ�)

        // 1) ī�޶� ������ �Ʒ�ó�� �����ϰ� ������ 
        // xRotation = Mathf.Clamp(xRotation, -50f, 50f); //-50f 50f�� �Ѿ�� �� ������ ������.
        // cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 2) ī�޶�� Rotation�� x������ ���Ϸ� ������ �� ����. ī�޶� rotation�� x���� 50�̸� �Ʒ��� ���� ��
        // 3) 

        

        // ���콺�� ���� �ø��� mouseY���� +�� �Ǵµ� XRotation +=

        xRotation = Mathf.Clamp(xRotation, -50f, 50f); //-50f 50f�� �Ѿ�� �� ������ ������.
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX); //�÷��̾� �����⵵ ���� ȸ���ؾ� ���� ���� ������ ������ �� �� �ִ�.
    }

    public void Move()
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction = camRight*moveInput.x + camForward*moveInput.y;

        rb.velocity = direction * 3f + Vector3.up * rb.velocity.y;
        //float horizontalSpeed = new Vector3(rb.velocity.x,0f,rb.velocity.z).magnitude;
        

    }
}
