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


    public bool isFirstPerson = true; //���� ������ 1��Ī���� �ƴ��� 
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

    public PlayerInput playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked; //���콺�� ȭ�� �߾ӿ� ����
        Cursor.visible = false; //���콺 Ŀ���� �� ���̰� ��
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //T������ ���� ��ȯ, ���߿� ��ǲ�ý������� ���ľ���
        {
            SwitchToView();
        }
        Move();
    }
    public void OnToggleInput(InputAction.CallbackContext ctx)  //On???Input ���·� �׻� �ۼ��ؾ��ϴµ�
    {
        if (ctx.performed) //���� ���� �� ���⼭(��ǲ�ý���)�� button���� �����ؼ� ��������?
        {
            SwitchToView();
        }
    }
    void SwitchToView()
    {
        isFirstPerson = !isFirstPerson; //����ġ ���
        firstPersonCam.Priority = isFirstPerson ? 1 : 0; //1��Ī�̸� 1��Īī�޶��� �켱������ 1�� ������ 1��Ī ī�޶��� ��ġ�� ���̰� ��
        topDownCam.Priority = isFirstPerson ? 0 : 1; //�̰� �ݴ�
        if (isFirstPerson)
            playerInput.SwitchCurrentActionMap("FirstPerspective"); //�׼Ǹ� ��ü
        else
            playerInput.SwitchCurrentActionMap("ThreePerspective");
    }



    //��ǲ�ý������� �̵� �� ����.
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); 
    }
    //��ǲ�ý������� ���콺�� 2D ����(x:�¿�,y:����)�� �޾ƿ�.
    public void OnMouseInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>(); //2D ����(x:�¿�,y:����)�� �޾ƿ�.

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime; 
        mouseY = mouseInput.y* mouseSensitivity * Time.deltaTime;
        //Time.deltaTime���� �ϰ��� �ְ� �޾��ְ�

        // 1) ī�޶�� Rotation�� x������ ���Ϸ� ������ �� ����.ī�޶� rotation�� x���� 50�̸� �Ʒ��� ���� ��
        // 2) XRotation(ī�޶� ����)�� mouseY������ �����Ѵ�. (X�� ȸ���̱� ������ y���� �ٲ�)
        // 3) mouseY���� Ŀ������(���� ���� ����) ī�޶��� Rotation�� x���� -�� ���ؾ� ī�޶� ���� ���� ��

        // => �׷��� ī�޶� ������ �Ʒ�ó�� ������ �� ����.
        xRotation -= mouseY; 

        xRotation = Mathf.Clamp(xRotation, -50f, 50f); //-50f 50f�� �Ѿ�� �� ������ ������.
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //ī�޶��� ����ȸ����(���ʹϾ�)�� �ٲٴµ� x�ุ �ٲ㼭 ���Ʒ��� �ٲٰ���. 

        playerBody.Rotate(Vector3.up * mouseX); //�÷��̾� �����⵵ ���� ȸ���ؾ� ���� ���� ������ ������ �� �� �ִ�.
        //Vector3.up = (0,1,0) => y�� ȸ��(y����)�� �ǹ���, mouseX�� ���콺 �¿� ������, Rotate() : �ش� ���� �������� ȸ����Ŵ 
    }


    public void Move()
    {
        if (!isFirstPerson && moveInput != Vector2.zero) //3��Ī �̰� �Է¹޴� ���� �����Ҷ�(Ű���尡 ������?)
        {
            Vector3 direction3 = new Vector3(moveInput.x, 0, moveInput.y); //3��Ī�� ���� �׳� ��ǲ����ü�� direction���� �޾ƾ���... ī�޶� ������� ��ǥ��� ���������ϱ� ������..
            playerBody.rotation = Quaternion.LookRotation(direction3);
            rb.velocity = direction3.normalized * moveSpeed;
            //�÷��̾��� rotation�� �ε巴�� ȸ������.
            //Quaternion.Slerp(����ȸ��, ��ǥȸ��, �ӵ�)
        }
        else // 1��Ī�� ��
        {
            camForward = cameraTransform.forward; //ī�޶� ���� ���� ����
            camRight = cameraTransform.right; // ī�޶��� ������ ����

            camForward.y = 0f; //���� ī�޶��� ��/�Ʒ� ���� ������ �������ְ�
            camRight.y = 0f;

            camForward.Normalize(); //������ ���̸� 1�� ����(���⸸ �޾��ش�?)
            camRight.Normalize();

            Vector3 direction = camRight * moveInput.x + camForward * moveInput.y;
            // moveInput.x�� �� ��(A/D)���� �̵��ϴ� ���� �޴°Ű� moveInput.y�� �յ�(W/S)�� �̵��ϴ� ���� �޴´�.
            // �밢������ ���� ���ؼ��� �Ѵ� ���� �޾Ƽ� ��������� ���� ���� �Ǹ���.

            // w������ �� ���� ���� �ִ� ī�޶� �������� ������ �����ϱ� ������ ī�޶��� ������ ���� ��

            rb.velocity = direction * 3f;
            //+ Vector3.up * rb.velocity.y;
            // ������ٵ��� ���ν�Ƽ(��ü ��ü �ӵ�)�� ������ ���� direction�� player�� ������ �ӵ��� �����ְ�
            // ������ �ʿ��ϴٸ� y����������� y�� �ӵ��� �����ϱ� ���� ������ ������ ����� �ʿ� ��� ��

            //float horizontalSpeed = new Vector3(rb.velocity.x,0f,rb.velocity.z).magnitude;
            //magnitude�� ������ ���̰�(�븧or����?)�� ���ϴµ� �̰ɷ� �÷��̾��� �ӵ���?�� Ȯ���� �� ����
            //=> ���߿� �޸� ���� ���� ���� �� ������ �����ؼ� �ִϸ��̼� ���� �� ����.

        }
    }
    // Rotation���� �⺻������ ���ʹϾ�(4���� ȸ����)�ε�
    // �� ���ʹϾ��� �츮�� �ٷ�� ����(?) �׷��� Euler�� �ٿ��� �츮�� �� �� �ְ� 3��������
    // ���Ǹ� �� �� �ִ°���. ex) Quaternion.Euler(xRotation, 0f, 0f);
    // �׷��� ȸ���� ��ü�� ������ �� ����� ���� ��ó�� quaternion�� ����. 
}
