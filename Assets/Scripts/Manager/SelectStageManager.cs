using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageManager : MonoBehaviour
{
    /// <summary>
    /// 스테이지 선택 패널
    /// </summary>
    [SerializeField]
    GameObject m_selectStagePanelPrefeb = null;

    /// <summary>
    /// 스테이지 선택 버튼
    /// </summary>
    [SerializeField]
    GameObject m_selectStageBtnPrefeb = null;

    /// <summary>
    /// 게임메니저 인터페이스
    /// </summary>
    private IGameManager m_gameManager;

    /// <summary>
    /// 스테이지 선택 패널 콘텐츠 위치
    /// </summary>
    private Transform m_stageSelectPanelContentTrans = null;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Instance;

        m_stageSelectPanelContentTrans = Instantiate(m_selectStagePanelPrefeb, m_gameManager.CanvasTrans)
            .GetComponent<ScrollRect>().content.transform;
        ResetSelectStageBtn();
    }

    /// <summary>
    /// 스테이지 버튼들을 리셋
    /// </summary>
    void ResetSelectStageBtn()
    {
        List<int> stageCodeList = m_gameManager.StageCodeList;
        for (int i = 0; i < stageCodeList.Count; i++)
        {
            Instantiate(m_selectStageBtnPrefeb, m_stageSelectPanelContentTrans).GetComponent<SelectStageBtn>()
                .ResetBtn(stageCodeList[i], this);
        }
    }
}
