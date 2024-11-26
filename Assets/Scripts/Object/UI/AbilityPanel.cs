using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    /// <summary>
    /// 각 패널들의 위치
    /// </summary>
    [SerializeField]
    Transform m_skillPanelTrans = null;
    [SerializeField]
    Transform m_toolPanelTrans = null;
    /// <summary>
    /// 선택 표시 마크 위치
    /// </summary>
    [SerializeField]
    Transform m_selectMarkTrans = null;
    /// <summary>
    /// 변경 주기
    /// </summary>
    [SerializeField]
    float m_changeInterval = 0.5f;

    /// <summary>
    /// 스킬 인덱스와 툴 인덱스
    /// 한번에 하나씩만 선택 가능한듯
    /// </summary>
    private int m_skillIndex = 0;
    private int m_toolIndex = 0;
    /// <summary>
    /// 최대 인덱스
    /// 나중에 스킬 데이터를 참조할것
    /// </summary>
    private int m_maxSkillIndex = 5;
    private int m_maxToolIndex = 5;
    /// <summary>
    /// 만약 스킬을 선택했을 경우 true
    /// </summary>
    private bool m_isSkill = true;
    /// <summary>
    /// 스킬 변경 시 변경 할 수 있는 경우
    /// </summary>
    private bool m_isCanChange = true;

    // Start is called before the first frame update
    void Start()
    {
        //임시 설정!!!!!
        m_maxSkillIndex = m_skillPanelTrans.childCount - 1;
        m_maxToolIndex = m_toolPanelTrans.childCount - 1;
    }

    void IsCanChange()
    {
        m_isCanChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isCanChange == false)
        {
            return;
        }

        if (Input.GetAxisRaw("Vertical") == -1)
        {
            m_isSkill = false;
            ToolIndex = SkillIndex;
        }
        else if (Input.GetAxisRaw("Vertical") == 1)
        {
            m_isSkill = true;
            SkillIndex = ToolIndex;
        }

        if (m_isSkill == true)
        {
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                SkillIndex -= 1;
            }
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                SkillIndex += 1;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                ToolIndex -= 1;
            }
            else if (Input.GetAxisRaw("Horizontal") == 1)
            {
                ToolIndex += 1;
            }
        }

        m_isCanChange = false;
        Invoke("IsCanChange", m_changeInterval);
    }

    public int SkillIndex
    {
        get
        { return m_skillIndex; }
        private set
        {
            m_skillIndex = value;

            if(m_skillIndex < 0)
            {
                m_skillIndex = m_maxSkillIndex;
            }
            else if(m_skillIndex > m_maxSkillIndex)
            {
                m_skillIndex = 0;
            }

            m_selectMarkTrans.position = m_skillPanelTrans.GetChild(m_skillIndex).position;
        }
    }

    public int ToolIndex
    {
        get
        { return m_toolIndex; }
        private set
        {
            m_toolIndex = value;

            if (m_toolIndex < 0)
            {
                m_toolIndex = m_maxToolIndex;
            }
            else if (m_toolIndex > m_maxToolIndex)
            {
                m_toolIndex = 0;
            }

            m_selectMarkTrans.position = m_toolPanelTrans.GetChild(m_toolIndex).position;
        }
    }
}
