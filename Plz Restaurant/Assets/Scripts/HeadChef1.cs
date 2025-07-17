using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadChef1 : MonoBehaviour
{
    //public List<int> H_cookingList = new List<int>(); //현재 조리중인 주문서 목록
    public Queue<int> H_cookingList = new Queue<int>(); // 거의 큐나 스택으로 진행하여서 일단 리스트에서 큐로 바꿈
                                                        //주문서를 넘기면(외부스크립트에서 뭐 enque로 넣어주는식)
                                                        // or addCooking함수를 외부에서 써서 넣어줌 
    public FoodDB foodDB;
    public FoodData food;
    bool isCooking = false; //지금 조리중인지 체크
    int nextFood = 0; //인덱스를 위해
    private void Update()
    {
        H_startCooking();
    }
    void H_startCooking() //조리시작
    {
        if (!isCooking && H_cookingList.Count > 0)
        {
            nextFood = H_cookingList.Peek(); //Deque는 아예 삭제하는거고 얘는 꺼내기만 함
            StartCoroutine(Cook(nextFood));
        }
    }

    public void AddCooking(int foodNum)
    {
        H_cookingList.Enqueue(foodNum);
    }

    void H_placeFoodOnTray(int foodNum)//음식 완성 후 트레이에 올림 
    {
        food = foodDB.GetFoodData(foodNum);
        if (food == null) return;
        else
        {
            FoodManager.Instance.AddCompletedFood(food);
            Debug.Log("조리 완료" + foodNum + " 번 음식 조리 완료");
            //트레이에서 위치 찾아서 food 올려줌 or 기존에 트레이에 올리는 함수를 여기서 불러줌..
        }
        H_cookingList.Dequeue(); 
    }

    IEnumerator Cook(int foodNum)
    {
        isCooking = true;
        Debug.Log("조리시작. " + foodNum + " 번 음식 조리 중");
        // 조리하는 애니메이션 넣기 
        yield return new WaitForSeconds(3f); //조리 시간 임의로 지정함.
        H_placeFoodOnTray(foodNum);
        isCooking=false;
    }
}
