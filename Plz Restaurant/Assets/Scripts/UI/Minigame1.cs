using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Minigame1 : MonoBehaviour
{
    [SerializeField]
    private Button[] foodImages; // 테이블에 음식 이미지가 들어갈 오브젝트 저장
    [SerializeField]
    private TextMeshProUGUI tableNumText; // 테이블 번호 텍스트 - 출력
    [SerializeField]
    private TextMeshProUGUI tryedTimesText; // 시도 횟수 텍스트 - 출력
    [SerializeField]
    private TextMeshProUGUI timerText; // 타이머 텍스트 - 출력
    float currentTime;
    [SerializeField]
    private GameObject gameOverImage; // 게임 오버 이미지 - 타임오버 시 활성화
    [SerializeField]
    private GameObject gameClearImage; // 클리어 이미지 - 미니게임 클리어 시 활성화

    FoodDB foodDB;

    private FoodData[] orderedFoods; // 올바른 주문 정보 저장 배열
    private int tableNum; // 테이블 번호
    private List<int> wrongFoodIndexList; // 정답이 아닌 음식들의 인덱스 리스트
    List<int> correctRandNums; // 정답 음식이 들어갈 위치의 랜덤 인덱스 리스트
    private bool isTimeOver = false; // 타임오버가 이미 되었는지 체크하는 변수


    private void Awake()
    {
        foodDB = FindObjectOfType<FoodDB>();

        wrongFoodIndexList = new List<int>(); // 초기화
        correctRandNums = new List<int>();

        // 호출 테스트
        // GetOrderedFoodData(1, new FoodData[]{foodDB.GetFoodData(1), foodDB.GetFoodData(2)});
        InitTimer();
    }

    private void Update()
    {
        if (!isTimeOver)
        {
            CountTimer();
        }
    }

    private void CountTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
        if (currentTime <= 0)
        {
            timerText.text = "0";
            isTimeOver = true;
            StartCoroutine(TimeOverImageSet());
        }
    }

    // 타임오버 이미지 활성화 코루틴 - 2초 뒤 사라짐
    IEnumerator TimeOverImageSet()
    {
        gameOverImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameOverImage.SetActive(false);
    }

    // 타이머 초기화
    private void InitTimer()
    {
        isTimeOver = false;
        timerText.SetText("15");
        currentTime = 15f;
    }

    // 해당 테이블의 올바른 주문 정보 전달 받기
    // MiniGame1의 시작 - 호출 지점
    public void GetOrderedFoodData(int tableNumber, FoodData[] foods)
    {
        tableNum = tableNumber;
        orderedFoods = foods;

        // 타이머 초기화
        InitTimer();
        InitWrongList(foods);
        SetMinigameStart();
    }

    // 정답이 아닌 음식들의 인덱스 리스트 초기화
    private void InitWrongList(FoodData[] foods)
    {
        // 리스트 비우기 - 재사용 고려
        wrongFoodIndexList.Clear();

        // foodDB의 음식 개수만큼 인덱스 추가 - 데이터 전달 받은 후 정답 데이터 제외시킬 것
        for (int i = 0; i < foodDB.foodCount; i++)
        {
            wrongFoodIndexList.Add(i);
        }
        // 정답 데이터의 인덱스 제거
        foreach (FoodData correctFood in foods)
        {
            int correctIndex = correctFood.foodNum;
            wrongFoodIndexList.Remove(correctIndex);
        }
    }

    // 게임 시작을 위한 세팅 기능
    // 필요한 동작들 추가할 예정
    private void SetMinigameStart()
    {
        // tableNumText 출력
        if (tableNum >= 10)
        {
            tableNumText.SetText(tableNum.ToString());
        }
        else if (tableNum < 10 && tableNum > 0)
        {
            tableNumText.SetText("0" + tableNum.ToString());
        }

        SetFoodImages();
    }

    // 올바른 주문 정보 개수만큼 0~15 사이의 중복되지 않는 랜덤 숫자 뽑기
    // 해당 숫자의 인덱스에 정답 음식 이미지를 넣기 위함
    private List<int> GetRandomNumbersToCorrect()
    {
        // 0~15까지 숫자가 든 리스트 생성
        List<int> pool = new List<int>();
        for (int i = 0; i < 16; i++) pool.Add(i);

        // 결과 담을 리스트
        List<int> result = new List<int>();

        // 뽑을 개수 : 주문된 음식 개수
        int count = orderedFoods.Length;

        // 결정된 개수만큼 뽑기
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index); // 뽑은 건 제거해서 중복 방지
        }

        return result;
    }

    // 정답과 오답 음식 이미지 16개 세팅
    private void SetFoodImages()
    {
        correctRandNums = GetRandomNumbersToCorrect();
        int index = 0;
        for (int i = 0; i < 16; i++)
        {
            // 정답을 넣기 위해 선택해 둔 위치인 경우
            if (correctRandNums.Contains(i))
            {
                // 정답의 이미지를 순서대로 넣기
                foodImages[i].GetComponent<Image>().sprite = orderedFoods[index].foodImage;
                index++;
            }
            // 틀린 음식의 이미지가 들어가야 하는 경우
            else
            {
                // 오답 리스트(wrongFoodIndexList)에서 랜덤하게 하나 뽑음
                int randomPick = Random.Range(0, wrongFoodIndexList.Count);
                int wrongFoodID = wrongFoodIndexList[randomPick];

                // FoodDB에서 해당 ID의 음식 이미지 가져옴
                FoodData wrongFood = foodDB.GetFoodData(wrongFoodID);

                // 오답 이미지 넣기
                foodImages[i].GetComponent<Image>().sprite = wrongFood.foodImage;

                // 오답끼리 중복 안 되게 하려면 뽑은 건 제거
                wrongFoodIndexList.RemoveAt(randomPick);
            }
        }
    }

    // ---------------------------------------------------------------
    // 미니게임 플레이 관련 로직 작성 시작
    // ---------------------------------------------------------------

    // 호출로 미니게임 시작
    private void PlayMinigame()
    {

    }

    private void PickCorectAnswer()
    {

    }
    
    private void PickWrongAnswer()
    {

    }

}
