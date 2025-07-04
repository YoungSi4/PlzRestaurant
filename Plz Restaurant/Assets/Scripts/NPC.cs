using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    public TrayControl trayControl;
    public Transform tablePos; // ������ ���̺� ��ġ ����
    public Transform bossHandPos; // ������ �� ������� �� ��ġ ����. ��մ� �Ϸ��� �ϳ� �� ��ġ �߰��ؼ� �����ϸ� ��.
    public GameObject foodCopy; // ����� �տ� �� ����

    private Vector3 startPos; // ������� �⺻��ġ ����
    private Quaternion startRot; // ������� �⺻��ġ ���� ����
    private NavMeshAgent nav; // ���̰��̼�

    private bool isBusy = false; // ���� ������ �����ϴ� �ڷ�ƾ�� ���������� �˻�

    // ���� �ֱٿ� ì�� ������ Ʈ���� ��ġ ����
    // Ʈ���̿� �ø� �� �ִ� ���İ����� �þ�� �Ǹ� �ϳ��� �߰��ϴ� ������� ������ ��
    public enum TrayPickupIndex
    {
        None, First, Second
    }
    // ���� �ֱٿ� ì�� ������ Ʈ���� ��ġ �ʱ�ȭ
    private TrayPickupIndex lastPickedIndex = TrayPickupIndex.None;


    void Awake()
    {
        // ����� ������ġ�� �����ص�
        startPos = transform.position;
        // ����� ���۹��� �����ص�
        startRot = transform.rotation;
        nav = gameObject.GetComponent<NavMeshAgent>(); // �ʱ�ȭ
    }

    void Update()
    {
        // �ڷ�ƾ�� ���������� �����鼭 Ʈ���̰� ���ڸ��� ä���� �ִ� ��� ����
        if (!isBusy && (trayControl.foodObj1 != null || trayControl.foodObj2 != null))
        {
            isBusy = true;
            StartCoroutine(BossRoutine());
        }

    }

    // ������� ������ �Ѱ�
    IEnumerator BossRoutine()
    {
        switch (lastPickedIndex)
        {
            case TrayPickupIndex.None:
            case TrayPickupIndex.Second:
                if (trayControl.foodObj1 != null)
                {
                    // Ʈ���� 1�� ������ġ ������ �̵�
                    /*MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f));
                    yield return new WaitForSeconds(0.5f);*/
                    yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                    // ���� ���
                    PickFood(1);
                    yield return new WaitForSeconds(0.5f);
                    // �ֹ��� ���̺� ��ġ�� �̵�
                    /*MoveToPos(tablePos.position);
                    yield return new WaitForSeconds(0.5f);*/
                    yield return StartCoroutine(MoveToPos(tablePos.position));
                    // ���̺� ���� ��������
                    ServeFood();

                    // �ֱ� Ʈ���̿��� ������ ì�� ��ġ ����
                    lastPickedIndex = TrayPickupIndex.First;

                }
                break;
            case TrayPickupIndex.First:
                if (trayControl.foodObj2 != null)
                {
                    // Ʈ���� 2�� ������ġ ������ �̵�
                    /*MoveToPos(trayControl.foodPos2.transform.position + new Vector3(0, 0, 1f));
                    yield return new WaitForSeconds(0.5f);*/
                    yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                    // ���� ���
                    PickFood(2);
                    yield return new WaitForSeconds(0.5f);
                    // �ֹ��� ���̺� ��ġ�� �̵�
                    /*MoveToPos(tablePos.position);
                    yield return new WaitForSeconds(0.5f);*/
                    yield return StartCoroutine(MoveToPos(tablePos.position));                    // ���̺� ���� ��������
                    ServeFood();

                    // �ֱ� Ʈ���̿��� ������ ì�� ��ġ ����
                    lastPickedIndex = TrayPickupIndex.Second;
                }
                else if (trayControl.foodObj2 == null && trayControl.foodObj1 != null)
                {
                    // Ʈ���� 1�� ������ġ ������ �̵�
                    /*MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f));
                    yield return new WaitForSeconds(0.5f);*/
                    yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                    // ���� ���
                    PickFood(1);
                    yield return new WaitForSeconds(0.5f);
                    // �ֹ��� ���̺� ��ġ�� �̵�
                    /*MoveToPos(tablePos.position);
                    yield return new WaitForSeconds(0.5f);*/
                    yield return StartCoroutine(MoveToPos(tablePos.position));                    // ���̺� ���� ��������
                    ServeFood();

                    // �ֱ� Ʈ���̿��� ������ ì�� ��ġ ����
                    lastPickedIndex = TrayPickupIndex.First;
                }
                break;
        }

        // ������ ��� ó���Ǹ� ����ġ�� ����
        if (trayControl.foodObj1 == null && trayControl.foodObj2 == null)
        {
            // MoveToPos(startPos);
            yield return StartCoroutine(MoveToPos(startPos));

            yield return StartCoroutine(RotateToStart());
        }

        isBusy = false;
    }

    // ������� Ʈ���̿��� ������ ì��� ����
    void PickFood(int trayIndex)
    {
        // ����� ����ġ�� ì�� ���� ����
        if (trayIndex == 1)
        {
            // ������ ���� ������Ʈ foodCopy�� ����
            foodCopy = Instantiate(trayControl.foodObj1, bossHandPos.position, bossHandPos.rotation);
            // foodCopy�� ������ ������Ʈ �� ��ġ�� ����
            foodCopy.transform.SetParent(bossHandPos);
        }
        else if (trayIndex == 2)
        {
            foodCopy = Instantiate(trayControl.foodObj2, bossHandPos.position, bossHandPos.rotation);
            foodCopy.transform.SetParent(bossHandPos);
        }

        // Ʈ���̿��� ���� ����
        trayControl.ClearFood(trayIndex);
    }

    // ���̺� ���� ��������
    void ServeFood()
    {
        if (foodCopy != null)
        {
            // ���̺� ���� �������� �߰� �ʿ�
            GameObject tableFood = Instantiate(foodCopy, tablePos.position, tablePos.rotation);
            // ���̺� �ø� ������ ��� ���� �� ���� ��ü��
            tableFood.transform.SetParent(null);

            Destroy(foodCopy);
            foodCopy = null;
        }
    }
    // ���� ��ġ�� �̵�
    IEnumerator MoveToPos(Vector3 targetPos)
    {
        // ���� ��ġ�� �̵�
        nav.SetDestination(targetPos);

        // ��ΰ�� �� ���
        while(nav.pathPending)
            yield return null;

        // �̵� �� ���(������ ��������)
        // �����Ÿ� > �����Ÿ� || �ӵ� ����
        while (nav.remainingDistance > nav.stoppingDistance || nav.velocity.sqrMagnitude > 0.01f)
            yield return null;
    }
    // �ʱ� ��ġ �̵� �� �ʱ� ���·� ȸ��
    IEnumerator RotateToStart()
    {
        // ������ ���� �ð���
        float t = 0f;

        // ���� ���� (���۰�)
        Quaternion current = transform.rotation; 
        while (t < 1f)
        {
            t += Time.deltaTime * 2f; // ȸ�� �ӵ� ����
            // ����(����) ȸ�������� ����(�ʱ�) ȸ�������� ���� �� t�� ���� 0 -> 1 ȸ��
            transform.rotation = Quaternion.Slerp(current, startRot, t);
            yield return null;
        }
    }


}
