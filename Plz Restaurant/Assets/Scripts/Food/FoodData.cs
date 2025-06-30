using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Food Data", menuName = "CustomData/Create Food Data")]
public class FoodData : ScriptableObject // ��ũ���ͺ� ������Ʈ ���
{
    public Sprite foodImage;
    public int foodNum;
    public string foodName;
    public int foodPrice;

    [TextArea]
    public string foodDescription;

    // ���� ��
    public GameObject foodPrefab;
}
