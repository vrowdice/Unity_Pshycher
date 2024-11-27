using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    IGameManager m_gameManager = null;

    private void Start()
    {
        m_gameManager = GameManager.Instance;
    }

    public void NextStage()
    {
        m_gameManager.ClearStage(true);
    }

    public void BackSelectStage()
    {
        m_gameManager.ClearStage(false);
    }
}
