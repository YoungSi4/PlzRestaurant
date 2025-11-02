using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FrontSceneUI : MonoBehaviour
{
    public TextMeshProUGUI text; // "Press Any Key" 텍스트 
    public Image buttonImage; // Button 이미지
    public Image fadeOutImage; // 페이드 아웃 위한 전체를 감싼 이미지, 알파값이 0임


    public float blinkSpeed = 2f; // 버튼 깜빡이는 속도
    
    float alpha; // 0~1사이의 알파값으로 버튼과 텍스트의 깜빡임을 담당
    float alpha1; // 페이드아웃이미지의 알파값 조정

    float fadeDuration = 1.5f;
    bool isFading = false; // 페이드 아웃이 시작됬는지 확인하는 변수
    float fadeTimer = 0f;

    AudioSource audioSource;

    Color text_Color;
    Color buttonImage_Color;
    Color fadeOutImage_Color;

    private void Start()
    {
        text_Color = text.color;            // Color는 구조체라 직접 알파값을 바꾸지 못해서 여기에 담고 바꿔야함
        buttonImage_Color = buttonImage.color;
        fadeOutImage_Color = fadeOutImage.color;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // sin 함수는 010-10 이니까 Abs 붙여서 절댓값으로 => 01010101로 만듦
        //Time.time은 게임이 시작된 후 경과 시간 (1초씩 늘어남)
        // Sin(x)에서 x값은 sin함수가 얼마나 빠르게 변할지(주기)에 관한 값
        
        alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        if (text != null)
        {
            text_Color.a = alpha; //알파값(불투명도)을 바꿔줌
            text.color = text_Color; 
        }

        if (buttonImage != null)
        {
            buttonImage_Color.a = alpha;
            buttonImage.color = buttonImage_Color;
        }

        if (Input.anyKeyDown)
        {
            isFading = true;
            audioSource.Play();
        }

        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            alpha1 = (fadeTimer / fadeDuration); //0과 1사이로 제한하는 함수      

            fadeOutImage_Color.a = alpha1;
            fadeOutImage.color = fadeOutImage_Color;

            if (alpha1 >= 1f)
            {
                Debug.Log("다음 씬으로 넘어감");
                SceneManager.LoadScene("SJ");
            }
        }
    }
    
}
