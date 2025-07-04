using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    public TrayControl trayControl;
    public Transform tablePos; // ������ ���̺� ��ġ ����
    public Transform bossHandPos1; // ������ �� ������� �� ��ġ ����.
    public Transform bossHandPos2; // ������ �� ������� �� ��ġ ����.

    private Vector3 B_startPos; // ������� �⺻��ġ ����
    private Quaternion B_startRot; // ������� �⺻��ġ ���� ����
    private NavMeshAgent nav; // �׺���̼�
    private float B_speed = 5; // ������� �̵� �ӵ� (���� ����)
    private int B_abillity = 2; // �� ���� �� �� �ִ� ���� �� (�ִ�4�� ����)
    private Queue<GameObject> foodCopies = new Queue<GameObject>(); // ����� �տ� �� ���� ������Ʈ ����Ʈ

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
        B_startPos = transform.position;
        // ����� ���۹��� �����ص�
        B_startRot = transform.rotation;
        nav = gameObject.GetComponent<NavMeshAgent>(); // �ʱ�ȭ

        nav.speed = B_speed;
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
        // Ʈ���̰� ä���� �ִٸ� ������ƾ �ݺ�
        while(trayControl.foodObj1 != null || trayControl.foodObj2 != null)
        {
            // Ʈ���̿��� ���� ��� ����
            while (foodCopies.Count < B_abillity)
            {
                if (trayControl.foodObj1 == null && trayControl.foodObj2 == null)
                    break;
                switch (lastPickedIndex)
                {
                    case TrayPickupIndex.None:
                    case TrayPickupIndex.Second:
                        if (trayControl.foodObj1 != null)
                        {
                            // Ʈ���� 1�� ������ġ ������ �̵�

                            yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                            // ���� ���
                            PickFood(1);
                            yield return new WaitForSeconds(0.5f);

                            // �ֱ� Ʈ���̿��� ������ ì�� ��ġ ����
                            lastPickedIndex = TrayPickupIndex.First;

                        }
                        break;
                    case TrayPickupIndex.First:
                        if (trayControl.foodObj2 != null)
                        {
                            // Ʈ���� 2�� ������ġ ������ �̵�
                            yield return StartCoroutine(MoveToPos(trayControl.foodPos2.transform.position + new Vector3(0, 0, 1f)));
                            // ���� ���
                            PickFood(2);
                            yield return new WaitForSeconds(0.5f);

                            // �ֱ� Ʈ���̿��� ������ ì�� ��ġ ����
                            lastPickedIndex = TrayPickupIndex.Second;
                        }
                        else if (trayControl.foodObj2 == null && trayControl.foodObj1 != null)
                        {
                            // Ʈ���� 1�� ������ġ ������ �̵�
                            yield return StartCoroutine(MoveToPos(trayControl.foodPos1.transform.position + new Vector3(0, 0, 1f)));
                            // ���� ���
                            PickFood(1);
                            yield return new WaitForSeconds(0.5f);

                            // �ֱ� Ʈ���̿��� ������ ì�� ��ġ ����
                            lastPickedIndex = TrayPickupIndex.First;
                        }
                        break;
                }
            }
            // ������ ���̺�� �̵� �� ������ �δ� ����. ��� �ִ� ������ ��� ���� �� ���� �ݺ�
            while (foodCopies.Count > 0)
            {
                // ������ ���̺� ��ġ�� ��� ������ �ʿ�

                // �ֹ��� ���̺� ��ġ�� �̵�
                yield return StartCoroutine(MoveToPos(tablePos.position));
                // ���̺� ���� ��������
                ServeFood();
            }
        }


        // ������ ��� ó���Ǹ� ����ġ�� ����
        if (trayControl.foodObj1 == null && trayControl.foodObj2 == null)
        {
            yield return StartCoroutine(MoveToPos(B_startPos));
            yield return StartCoroutine(RotateToStart());
        }

        isBusy = false;
    }

    // ������� Ʈ���̿��� ������ ì��� ����
    void PickFood(int trayIndex)
    {
        // �������� �� �� �ִ� ���� �� ��ŭ ��� ������ 
        if (foodCopies.Count >= B_abillity) return;
        GameObject foodCopy = null;
        // ������ ������� ���� ��
        if(foodCopies.Count == 0)
        {
            // ����� ����ġ�� ì�� ���� ����
            if (trayIndex == 1)
            {
                // ������ ���� ������Ʈ foodCopy�� ����
                foodCopy = Instantiate(trayControl.foodObj1, bossHandPos1.position, bossHandPos1.rotation);
            }
            else if (trayIndex == 2)
            {
                foodCopy = Instantiate(trayControl.foodObj2, bossHandPos1.position, bossHandPos1.rotation);
            }

            // foodCopy�� ������ ������Ʈ �� ��ġ�� ����
            foodCopy.transform.SetParent(bossHandPos1);
            // ����ִ� ���� ť�� �ֱ�
            foodCopies.Enqueue(foodCopy);
            // Ʈ���̿��� ���� ����
            trayControl.ClearFood(trayIndex);
        }
        // ������ �� �� ��� ���� ��
        else if(foodCopies.Count == 1)
        {
            // ����� ����ġ�� ì�� ���� ����
            if (trayIndex == 1)
            {
                // ������ ���� ������Ʈ foodCopy�� ����
                foodCopy = Instantiate(trayControl.foodObj1, bossHandPos2.position, bossHandPos2.rotation);
            }
            else if (trayIndex == 2)
            {
                foodCopy = Instantiate(trayControl.foodObj2, bossHandPos2.position, bossHandPos2.rotation);
            }

            // foodCopy�� ������ ������Ʈ �� ��ġ�� ����
            foodCopy.transform.SetParent(bossHandPos2);
            // ����ִ� ���� ť�� �ֱ�
            foodCopies.Enqueue(foodCopy);
            // Ʈ���̿��� ���� ����
            trayControl.ClearFood(trayIndex);
        }
        // else if(2,3 ���� 4����)
    }

    // ���̺� ���� ��������
    void ServeFood()
    {
        while(foodCopies.Count> 0) // ������� ����ִ� ������ �ִ� ���(foodCopies�� empty�� �ƴ� ���)
        {
            GameObject foodCopy = foodCopies.Dequeue();
            // ���̺� ���� ����
            GameObject tableFood = Instantiate(foodCopy, tablePos.position, tablePos.rotation);
            // ���̺� �ø� ������ �θ� ���� �� ���� ��ü��
            tableFood.transform.SetParent(null);

            // �տ��� ������Ʈ ����
            Destroy(foodCopy);
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
            transform.rotation = Quaternion.Slerp(current, B_startRot, t);
            yield return null;
        }
    }


}
