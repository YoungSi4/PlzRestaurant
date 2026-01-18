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
    private TextMeshProUGUI remainingAttemptsText; // 시도 횟수 텍스트 - 출력
    [SerializeField]
    private TextMeshProUGUI timerText; // 타이머 텍스트 - 출력
    float currentTime;
    [SerializeField]
    private GameObject gameOverImage; // 게임 오버 이미지 - 타임오버 시 활성화
    [SerializeField]
    private GameObject gameClearImage; // 클리어 이미지 - 미니게임 클리어 시 활성화
    [SerializeField]
    private GameObject gameFailedImage; // 실패 이미지 - 미니게임 실패 시 활성화
    [SerializeField]
    private Image[] OImages; // 정답 시 출력할 O 이미지 배열
    [SerializeField]
    private Image[] XImages; // 오답 시 출력할 X 이미지 배열

    FoodDB foodDB;

    private FoodData[] orderedFoods; // 올바른 주문 정보 저장 배열
    private int tableNum; // 테이블 번호
    private List<int> wrongFoodIndexList; // 정답이 아닌 음식들의 인덱스 리스트
    List<int> correctRandNums; // 정답 음식이 들어갈 위치의 랜덤 인덱스 리스트
    private bool isTimeOver = false; // 타임오버가 이미 되었는지 체크하는 변수
    private bool isCleared = false; // 미니게임 클리어 여부 체크 변수
    private bool isFailed = false; // 미니게임 실패 여부 체크 변수


    private void Awake()
    {
        foodDB = FindObjectOfType<FoodDB>();

        wrongFoodIndexList = new List<int>(); // 초기화
        correctRandNums = new List<int>();
    }

    private void Start()
    {
        // 호출 테스트
        // GetOrderedFoodData(1, new FoodData[] { foodDB.GetFoodData(1), foodDB.GetFoodData(2), foodDB.GetFoodData(4), foodDB.GetFoodData(8), foodDB.GetFoodData(12) });
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
            GameOver();
        }
    }

    // 타임오버 이미지 활성화 코루틴 - 2초 뒤 사라짐
    IEnumerator TimeOverImageSet()
    {
        gameOverImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameOverImage.SetActive(false);
        // 미니게임 팝업 닫기
        CloseGame();
    }
    // 게임클리어 이미지 활성화 코루틴 - 2초 뒤 사라짐
    IEnumerator GameClearImageSet()
    {
        gameClearImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameClearImage.SetActive(false);
        // 미니게임 팝업 닫기
        CloseGame();
    }
    // 게임실패 이미지 활성화 코루틴 - 2초 뒤 사라짐
    IEnumerator GameFailedImageSet()
    {
        gameFailedImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameFailedImage.SetActive(false);
        // 미니게임 팝업 닫기
        CloseGame();
    }

    // UI 출력 텍스트 초기화
    private void InitUIText()
    {
        // 타이머 초기화
        timerText.SetText("15");
        currentTime = 15f;

        // 남은 시도 횟수 초기화
        remainingAttemptsText.SetText("3");

        // tableNumText 출력
        if (tableNum >= 10)
        {
            tableNumText.SetText(tableNum.ToString());
        }
        else if (tableNum < 10 && tableNum > 0)
        {
            tableNumText.SetText("0" + tableNum.ToString());
        }
    }

    // 해당 테이블의 올바른 주문 정보 전달 받기
    // MiniGame1의 시작 - 호출 지점
    // 외부 호출 시 미니게임 UI SetActive(true)가 선행되어야 함
    public void GetOrderedFoodData(int tableNumber, FoodData[] foods)
    {
        tableNum = tableNumber;
        orderedFoods = foods;

        InitUIText();
        InitWrongList(foods);
        SetMinigameStart();
    }

    // 정답이 아닌 음식들의 인덱스 리스트 초기화
    private void InitWrongList(FoodData[] foods)
    {
        // 리스트 비우기 - 재사용 고려
        wrongFoodIndexList.Clear();

        // foodDB의 음식 개수만큼 인덱스 추가 - 데이터 전달 받은 후 정답 데이터 제외시킬 것
        for (int i = 1; i <= foodDB.foodCount; i++)
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
        gameObject.SetActive(true); // 미니게임 UI 팝업 활성화

        isTimeOver = false;
        isCleared = false;
        isFailed = false;

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
            int buttonIndex = i;
            // 리스너 연결 (람다식)
            foodImages[i].onClick.RemoveAllListeners(); // 기존 연결 해제
            foodImages[i].onClick.AddListener(() => CheckAnswer(buttonIndex)); // 새로운 연결 생성

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

    // 버튼 클릭 시 호출 (index는 0 ~ 15)
    private void CheckAnswer(int index)
    {
        if (isTimeOver) return;

        // [핵심 변경] 이미지를 비교할 필요 없이, 
        // "누른 버튼의 번호(index)가 정답 리스트(correctRandNums)에 포함되어 있나?"만 확인하면 끝!
        if (correctRandNums.Contains(index))
        {
            PickCorectAnswer(index); 
        }
        else
        {
            PickWrongAnswer(index);
        }
    }

    private void PickCorectAnswer(int index)
    {
        // 잘 고른 그림에 O표시
        OImages[index].gameObject.SetActive(true);
        // 선택한 버튼 비활성화
        foodImages[index].interactable = false;

        // 선택한 정답을 정답 리스트에서 삭제
        correctRandNums.Remove(index);
        // 모든 정답을 선택한 경우 GameOver() 호출 및 isCleared = true 설정
        if (correctRandNums.Count == 0)
        {
            isCleared = true;
            GameOver();
        }
    }

    private void PickWrongAnswer(int index)
    {
        // 잘못 고른 그림에 X표시
        XImages[index].gameObject.SetActive(true);
        // 선택한 버튼 비활성화
        foodImages[index].interactable = false;

        // 선택한 버튼 비활성화
        foodImages[index].interactable = false;

        var text = remainingAttemptsText.text;
        int attempts = int.Parse(text);
        if (attempts > 1)
        {
            attempts--;
            remainingAttemptsText.SetText(attempts.ToString());
        }
        else if(attempts == 1)
        {
            attempts--;
            remainingAttemptsText.SetText(attempts.ToString());
            isFailed = true;
            GameOver();
        }
    }

    // 게임 오버 처리 - 미니게임 UI 내리기 등 추가해야 할 듯
    private void GameOver()
    {
        // 버튼 상호작용 비활성화(남은 모든 버튼 클릭 방지)
        foreach (var btn in foodImages)
        {
            btn.interactable = false;
        }

        if (isTimeOver)
        {
            StartCoroutine(TimeOverImageSet());
        }
        else if (isFailed)
        {
            StartCoroutine(GameFailedImageSet());
        }
        else if (isCleared)
        {
            StartCoroutine(GameClearImageSet());
        }
    }
    
    private void CloseGame()
    {
        // UI 팝업 비활성화 (창 닫기)
        gameObject.SetActive(false);
    }

}
