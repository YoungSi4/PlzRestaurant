using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Korean, Japanese, Chinese, Italian, French, American
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "CustomData/Create Item Data")]
public class FoodData : ScriptableObject // 스크립터블 오브젝트 상속
{
    public Sprite foodImage;
    public string foodName;

    [TextArea]
    public string foodDescription;
    public FoodType foodType;
    // 음식 모델
    public GameObject foodPrefab;
}
