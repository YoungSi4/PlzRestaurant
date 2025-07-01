using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderMemo : MonoBehaviour
{
    /* ����� ������ ���
     *  1. OrderMemoBlock�� �ֹ� ���� �°� ����
     *  2. ���� �°� ���� ���� �� ��ũ�� �����ϰ� ����
     */

    // ���ڰ� �������� �Ʒ�
    public GameObject OrderMemoBlock1;
    public GameObject OrderMemoBlock2;
    public GameObject OrderMemoBlock3;
    public GameObject OrderMemoBlock4;
    public Button accept;
    public Button reject;

    private int tableNum;
    private string foodName;
    // food image?
    private int foodPrice;

    // FoodInfo
    private VisitorOrder order;
    private FoodData foodData;

    void Start()
    {

    }

    // FoodDB ��ü���� �� �Լ��� ����...?
    // Food Manager ���� �߰� �Ű�ü�� �����ϴ� �� �����غ��δ�.
    
    // ���̺� ��ȣ�� ��� �޾Ƽ� �ѱ���? -> visitor order ��ü
    public void GetFoodInfo(FoodData foodData, int tableNum)
    {
        this.foodData = foodData;
        this.tableNum = tableNum;
        
        SetData();
    }

    private void SetData()
    {
        foodName = foodData.foodName;
        foodPrice = foodData.foodPrice; 
    }

    private void SetText()
    {
        var tableNum = foodData.foodNum;
        var foodName = foodData.foodName;
        var foodPrice = foodData.foodPrice;

        var texts = OrderMemoBlock1.GetComponentsInChildren<TextMeshPro>();

        // 1. ���̺� ��ȣ, 2. ���� �̸�, 3. ���� ����
        texts[1].SetText(tableNum.ToString());
        texts[2].SetText(foodName.ToString());
        texts[3].SetText(foodPrice.ToString());
    }
}
