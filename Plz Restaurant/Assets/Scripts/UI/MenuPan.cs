using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MenuPan : MonoBehaviour // 페이지 1, 2, 3가 존재 
{
    int pageNumber = 1;

    public Button rightButton;
    public Button leftButton;
    public TextMeshProUGUI pageText;

    public FoodDB foodDB;

    [SerializeField] private Transform content;



    private void Awake()
    {
        leftButton.gameObject.SetActive(false); //페이지 1부터 시작하니까 
    }
    private void Start()
    {
        SettingMenuPan(0, 19);
    }

    void CheckPage()
    {
        switch (pageNumber)
        {
            case 1:
                rightButton.gameObject.SetActive(true);
                leftButton.gameObject.SetActive(false);
                SettingMenuPan(0, 19);
                break;
            case 2:
                rightButton.gameObject.SetActive(false);
                leftButton.gameObject.SetActive(true);
                SettingMenuPan(20, 39);
                break;
        }
        pageText.text = pageNumber.ToString();
    }


    void SettingMenuPan(int StartNumber, int EndNumber)
    {
        int panelIndex = 0;

        for (int i = StartNumber; i<= EndNumber; i++)
        {
            if (panelIndex >= content.childCount)
                break;

            Transform panel = content.GetChild(panelIndex);
            if (i < foodDB.foodDatas.Length)
            {
                FoodData foodData = foodDB.GetFoodData(i+1);
                var texts = panel.GetComponentsInChildren<Text>();
                texts[0].text = (foodData.foodName).ToString();
                texts[1].text = (foodData.foodPrice).ToString();

                panel.gameObject.SetActive(true);
            }
            else
            {
                panel.gameObject.SetActive(false);
            }
            panelIndex++;
        }
    }

    public void OnRightButton()
    {
        pageNumber += 1;
        CheckPage();
    }

    public void OnLeftButton()
    {
        pageNumber -= 1;
        CheckPage();
    }

}
