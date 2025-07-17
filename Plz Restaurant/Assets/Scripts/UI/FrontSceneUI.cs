using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FrontSceneUI : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI tmpText;
    Image buttonImage;
    public Image fadeImage; //페이드 아웃 위한 전체를 감싼 이미지, 알파값이 0임

    public float blinkSpeed = 2f;
    float alpha;

    float fadeDuration = 1.5f;
    bool isFading = false;
    float fadeTimer = 0f;

    AudioSource audioSource;

    private void Start()
    {
        tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = button.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }
    // sin 함수는 010-10 이니까 Abs 붙여서 절댓값으로 => 01010101로 만듦
    void Update()
    {   //Time.time은 게임이 시작된 후 경과 시간 (1초씩 늘어남)
        // Sin(x)에서 x값은 sin함수가 얼마나 빠르게 변할지(주기)에 관한 값 
        alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        if (tmpText != null)
        {
            Color color = tmpText.color;
            color.a = alpha; //알파값(불투명도)을 바꿔줌
            tmpText.color = color;
        }
        if (buttonImage != null)
        {
            Color imgColor = buttonImage.color;
            imgColor.a = alpha;
            buttonImage.color = imgColor;
        }

        if (Input.anyKeyDown)
        {
            isFading = true;
            audioSource.Play();
        }
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha1 = (fadeTimer / fadeDuration); //0과 1사이로 제한하는 함수
            
            Color c = fadeImage.color;
            c.a = alpha1;
            fadeImage.color = c;

            if (alpha >= 1f)
            {
                Debug.Log("다음 씬으로 넘어감");
                //SceneManager.LoadScene("GameScene");
            }
        }
    }
    
}
