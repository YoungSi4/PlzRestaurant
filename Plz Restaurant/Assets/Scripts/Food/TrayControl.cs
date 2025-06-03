using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayControl : MonoBehaviour
{
    // Ʈ���̿� ������ �ø� ��ġ ����
    public Transform foodPos1;
    public Transform foodPos2;
    // ���� Ʈ���̿� �ö��ִ� ���� ������Ʈ ����
    public GameObject foodObj1;
    public GameObject foodObj2;

    void Update()
    {
        // �׽�Ʈ�� �����߰� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FoodData food = Resources.Load<FoodData>("GameObject/Food/Ramen");
            FoodData food2 = Resources.Load<FoodData>("GameObject/Food/Spaghetti");

            int ran = Random.Range(0, 2);
            if (food != null && food2 != null)
            {
                if (ran == 0)
                {
                    FoodManager.Instance.AddCompletedFood(food);
                    Debug.Log("�����߰�1��");
                }
                if (ran == 1)
                {
                    FoodManager.Instance.AddCompletedFood(food2);
                    Debug.Log("�����߰�2��");
                }
            }
            else Debug.Log("Null����");

        }
        // ť�� ������ �����鼭 Ʈ���̿� ���ڸ��� ������ Ʈ���̿� ������ ä��� ���� ��� Ȯ��
        TryUpdateTray();
    }
    void TryUpdateTray()
    {
        // 1�� ��ġ�� �ö� ������ ���� ť�� �ϼ��� ������ �������� ��
        if (foodObj1 == null && FoodManager.Instance.HasFood())
        {
            // ť���� ������ ����
            FoodData food1 = FoodManager.Instance.GetNextFood();
            // ������ Ʈ������ ���� ��ġ�� �ø�
            foodObj1 = SpawnFoodOnTray(food1, foodPos1);
        }
        // 2�� ��ġ�� �ö� ������ ���� ť�� �ϼ��� ������ �������� ��
        if (foodObj2 == null && FoodManager.Instance.HasFood())
        {
            // ť���� ������ ����
            FoodData food2 = FoodManager.Instance.GetNextFood();
            // ������ Ʈ������ ���� ��ġ�� �ø�
            foodObj2 = SpawnFoodOnTray(food2, foodPos2);
        }
    }

    GameObject SpawnFoodOnTray(FoodData food,  Transform foodPos)
    {
        // foodPos�� ��ġ�� food�� 3D������Ʈ ����
        GameObject instantFood = Instantiate(food.foodPrefab, foodPos.position, foodPos.rotation);
        return instantFood;
    }
    
    // Ʈ���̿��� ���� ����
    // NPC.cs ���� ȣ��
    public void ClearFood(int foodIndex)
    {
        if(foodIndex == 1)
        {
            if (foodObj1 != null)
            {
                Destroy(foodObj1);
                foodObj1 = null;
            }
        }
        else if(foodIndex == 2)
        {
            if (foodObj2 != null)
            {
                Destroy(foodObj2);
                foodObj2 = null;
            }
        }

    }
}
