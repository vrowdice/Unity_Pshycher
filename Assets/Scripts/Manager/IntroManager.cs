using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    /// <summary>
    /// 화면 페이드용 이미지
    /// </summary>
    public CanvasGroup fadeImage;
    /// <summary>
    /// 텍스트 박스 패널
    /// </summary>
    public CanvasGroup textBoxPanel;
    /// <summary>
    /// 대화 텍스트
    /// </summary>
    public TextMeshProUGUI dialogueText;
    /// <summary>
    /// 페이드 인/아웃 지속 시간
    /// </summary>
    public float fadeDuration = 1.0f;
    /// <summary>
    /// 대사 배열
    /// </summary>
    public string[] dialogueLines;

    private int currentLineIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutScene());
    }
    IEnumerator FadeOutScene()
    {
        yield return FadeCanvasGroup(fadeImage, 0, 1, fadeDuration); // 페이드 아웃 (검은 화면)
        yield return new WaitForSeconds(0.5f);
        yield return ShowDialogueBox(); // 대화 상자 표시
    }
    IEnumerator ShowDialogueBox()
    {
        yield return FadeCanvasGroup(textBoxPanel, 0, 1, fadeDuration); // 텍스트 박스 페이드 인

        // 대화 진행
        while (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // 스페이스 키로 대화 넘기기
        }

        yield return new WaitForSeconds(0.5f);
        yield return FadeCanvasGroup(textBoxPanel, 1, 0, fadeDuration); // 텍스트 박스 페이드 아웃
        yield return FadeCanvasGroup(fadeImage, 1, 0, fadeDuration); // 화면 페이드 아웃 (암전 해제)
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
