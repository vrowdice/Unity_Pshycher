using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// 타이틀 패널
    /// </summary>
    [SerializeField]
    GameObject m_titlePanelPrefeb = null;

    // Start is called before the first frame update
    void Start()
    {
        m_titlePanelPrefeb = Instantiate(m_titlePanelPrefeb, GameManager.Instance.CanvasTrans);

        m_titlePanelPrefeb.transform.Find("NewGameBtn").GetComponent<Button>().onClick.AddListener(() =>
        GameManager.Instance.MoveSceneAsName("SelectStage"));

        m_titlePanelPrefeb.transform.Find("QuitGameBtn").GetComponent<Button>().onClick.AddListener(() =>
        QuitGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 게임 나가기
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
