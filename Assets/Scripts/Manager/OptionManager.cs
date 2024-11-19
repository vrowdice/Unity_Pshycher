using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    /// <summary>
    /// 옵션 셋팅하는 오브젝트 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_optionSetupPrefeb = null;

    /// <summary>
    /// 게임메니저 인터페이스
    /// </summary>
    private IGameManager m_gameManager;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Instance;

        if (m_gameManager.CanvasTrans != null)
        {
            Instantiate(m_optionSetupPrefeb, m_gameManager.CanvasTrans);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
