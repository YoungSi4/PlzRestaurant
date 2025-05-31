using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Korean, Japanese, Chinese, Italian, French, American
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "CustomData/Create Item Data")]
public class FoodData : ScriptableObject // ��ũ���ͺ� ������Ʈ ���
{
    public Sprite foodImage;
    public string foodName;

    [TextArea]
    public string foodDescription;
    public FoodType foodType;
    // ���� ��
    public GameObject foodPrefab;
}
