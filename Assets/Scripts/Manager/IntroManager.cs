using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    /// <summary>
    /// 화면 페이드용 이미지 (검은 화면을 덮는 CanvasGroup)
    /// </summary>
    public CanvasGroup FadeImage;

    /// <summary>
    /// 텍스트 박스 패널 (대화 상자가 포함된 CanvasGroup)
    /// </summary>
    public CanvasGroup TextBoxPanel;

    /// <summary>
    /// 대화 텍스트 (TextMeshPro를 이용한 텍스트 컴포넌트)
    /// </summary>
    public TextMeshProUGUI DialogueText;

    /// <summary>
    /// 페이드 인/아웃 효과가 진행되는 시간 (초 단위)
    /// </summary>
    public float fadeDuration = 1.0f;

    /// <summary>
    /// 대사 배열 (표시될 대화의 텍스트 목록)
    /// </summary>
    public string[] dialogueLines;

    /// <summary>
    /// 현재 표시 중인 대사의 인덱스
    /// </summary>
    private int currentLineIndex = 0;

    /// <summary>
    /// 페이드 상태 관리
    /// </summary>
    private bool isFading = false; 


    // Start 메서드는 스크립트가 활성화될 때 처음 실행됩니다.
    void Start()
    {
        // 텍스트 박스를 처음에 비활성화
        TextBoxPanel.SetActive(false);

        // 검은 화면 페이드 아웃 코루틴 시작
        StartCoroutine(FadeOutScene());

    }

    /// <summary>
    /// 검은 화면을 페이드 아웃시키고 대화 상자를 표시하는 코루틴
    /// </summary>
    IEnumerator FadeOutScene()
    {
        // 화면을 검은색에서 투명으로 페이드 아웃
        yield return FadeCanvasGroup(FadeImage, 1, 0, fadeDuration);

        // 약간의 대기 시간 (0.5초)
        yield return new WaitForSeconds(0.5f);

        // 대화 상자를 표시하는 코루틴 실행
        yield return ShowDialogueBox();
    }
    
    /// <summary>
    /// 대화 상자를 페이드 인하여 표시하고 대화를 진행하는 코루틴
    /// </summary>
    IEnumerator ShowDialogueBox()
    {
        // 대화 상자 페이드 인
        TextBoxPanel.SetActive(true);


        // 대화 진행
        while (currentLineIndex < dialogueLines.Length)
        {
            // 현재 대사 배열의 내용을 텍스트 박스에 표시
            DialogueText.text = dialogueLines[currentLineIndex];

            // 다음 대사를 준비하기 위해 인덱스 증가
            currentLineIndex++;

            // 사용자가 스페이스 키를 누를 때까지 대기
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // 대화가 끝난 후 약간의 대기 시간 (0.5초)
        yield return new WaitForSeconds(0.5f);
        TextBoxPanel.SetActive(false);

        // 화면을 투명에서 검은색으로 페이드 인 (암전)
        yield return FadeCanvasGroup(FadeImage, 0, 1, fadeDuration);
    }

    /// <summary>
    /// CanvasGroup의 Alpha 값을 점진적으로 변경하여 페이드 인/아웃 효과를 주는 코루틴
    /// </summary>
    /// <param name="canvasGroup">페이드 효과를 적용할 CanvasGroup</param>
    /// <param name="startAlpha">시작 Alpha 값 (0: 완전히 투명, 1: 완전히 불투명)</param>
    /// <param name="endAlpha">끝 Alpha 값</param>
    /// <param name="duration">변화에 걸리는 시간 (초)</param>
    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float time = 0; // 경과 시간 초기화

        // Alpha 값을 점진적으로 변경
        while (time < duration)
        {
            // 경과 시간 증가
            time += Time.deltaTime;

            // Lerp 함수로 Alpha 값을 선형적으로 보간
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);

            // 한 프레임 대기
            yield return null;
        }

        // 종료 시 정확한 Alpha 값 설정 (오차 방지)
        canvasGroup.alpha = endAlpha;
    }


    void Update()
    {

    }
}
