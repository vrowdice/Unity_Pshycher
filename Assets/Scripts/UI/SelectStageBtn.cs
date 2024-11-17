using UnityEngine;
using UnityEngine.UI;

public class SelectStageBtn : MonoBehaviour
{
    /// <summary>
    /// 이 버튼의 텍스트
    /// </summary>
    [SerializeField]
    Text m_text = null;

    /// <summary>
    /// 스테이지 코드
    /// </summary>
    private int m_stageCode = 0;

    /// <summary>
    /// 스테이지 선택 매니저
    /// </summary>
    private SelectStageManager m_selectStageManager = null;

    /// <summary>
    /// 이 버튼 초기화
    /// </summary>
    /// <param name="argStageCode">스테이지 코드</param>
    /// <param name="argManager">스테이지 선택 매니저</param>
    public void ResetBtn(int argStageCode, SelectStageManager argManager)
    {
        m_stageCode = argStageCode;
        m_selectStageManager = argManager;
        //변경의 여지 있음
        m_text.text = (argStageCode % 10000).ToString();
    }

    /// <summary>
    /// 클릭 시
    /// </summary>
    public void Click()
    {
        GameManager.Instance.EnterStage(m_stageCode);
    }
}
