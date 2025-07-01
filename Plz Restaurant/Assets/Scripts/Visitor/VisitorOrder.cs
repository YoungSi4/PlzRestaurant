using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorOrder : MonoBehaviour
{
    // Vars related to FoodDB
    // ���� ��ȣ, �̸�, ���� ���
    private FoodData foodData;
    private int foodNum; // FoodDB ���� ���� index. �մԿ��Լ� �����ϰ� ������.
    private string foodName;
    private int foodPrice;
    private int tableNum;

    private FoodDB foodDB;
    private OrderMemo orderMemo; // �÷��̾� UI ���� �ֹ� ���

    private void Start()
    {
        // initialize
        orderMemo = GameObject.FindObjectOfType<OrderMemo>();
    }

    // setter, getter
    // ���� ���� ������ �����ϴ� �Լ� - �÷��̾��� ��ȣ�ۿ� E���� ���
    public void SetFoodNumFromVisitor(int foodNum, int tableNum)
    {
        this.foodNum = foodNum;
        this.tableNum = tableNum;

        GetFoodInfoFromDB(foodNum);
        SendFoodInfo(foodData, foodNum);
    }

    // ������ ���� �帧
    // VistorOrder -> FoodDB -> VisitorOrder
    private void GetFoodInfoFromDB(int foodNum)
    {
        foodData = foodDB.GetFoodData(foodNum);
    }

    public void SendFoodInfo(FoodData foodData, int tableNum)
    {
        // OrderMemo �󿡼� ǥ�õ� ������ �� �Լ����� �ʱ�ȭ
        orderMemo.GetFoodInfo(foodData, tableNum);
    }
}
