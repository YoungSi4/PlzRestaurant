using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Food Data", menuName = "CustomData/Create Food Data")]
public class FoodData : ScriptableObject // 스크립터블 오브젝트 상속
{
    public Sprite foodImage;
    public int foodNum;
    public string foodName;
    public int foodPrice;

    [TextArea]
    public string foodDescription;

    // 음식 모델
    public GameObject foodPrefab;
}
